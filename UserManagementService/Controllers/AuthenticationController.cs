
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UserManagementService.Models;
using UserManagementService.Models.Authentication.Login;
using UserManagementService.Models.Authentication.SignUp;

namespace UserManagementService.Controllers
{
    

    [Route("api/AuthAPI")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(UserManager<IdentityUser> userManager, IConfiguration configuration, ApplicationDbContext context, ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            var userExist = await _context.PreRegisteredUsers.FirstOrDefaultAsync(u => u.Email == registerUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "User Already Exists" });
            }

            var token = Generate2FAToken();

            // Hash the password using UserManager
            var identityUser = new IdentityUser { UserName = registerUser.UserName, Email = registerUser.Email };
            var hashedPassword = _userManager.PasswordHasher.HashPassword(identityUser, registerUser.Password);

            var preRegisteredUser = new PreRegisteredUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                PasswordHash = hashedPassword,
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = role,
                TwoFactorToken = token,
                TokenExpiry = DateTime.UtcNow.AddMinutes(10) // Token valid for 10 minutes
            };

            _context.PreRegisteredUsers.Add(preRegisteredUser);
            await _context.SaveChangesAsync();

            await Send2FACodeEmail(preRegisteredUser.Email, token);

            return StatusCode(StatusCodes.Status201Created, new Response { Status = "Success", Message = "User pre-registered successfully! Please verify your email to complete the registration." });
        }


        [HttpPost("verify2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] Verify2FARequest request)
        {
            try
            {
                var preRegisteredUser = await _context.PreRegisteredUsers.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (preRegisteredUser == null || preRegisteredUser.TwoFactorToken != request.Token || preRegisteredUser.TokenExpiry < DateTime.UtcNow)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Invalid or expired 2FA code" });
                }

                var user = new IdentityUser
                {
                    Id = preRegisteredUser.Id,
                    Email = preRegisteredUser.Email,
                    UserName = preRegisteredUser.UserName,
                    SecurityStamp = preRegisteredUser.SecurityStamp,
                    PasswordHash = preRegisteredUser.PasswordHash // Directly assign the pre-hashed password
                    
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed" });
                }
                // Generate and confirm the email token
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, emailToken);

                await _userManager.AddToRoleAsync(user, preRegisteredUser.Role);

                _context.PreRegisteredUsers.Remove(preRegisteredUser);
                await _context.SaveChangesAsync();
                var roles=await _userManager.GetRolesAsync(user);
                var jwtToken = GenerateJwtToken(user, roles);
                //// Add logic to mark the email as confirmed
                await _userManager.ConfirmEmailAsync(user, jwtToken);
                return Ok(new Response { Status = "Success", Message = "2FA verified and user registered successfully", Token = jwtToken });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during 2FA verification: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "An error occurred during 2FA verification. Please try again later." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginUser loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.UserName);
            if (user != null)
            {
                // Verify the password using UserManager
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginUser.Password);
                if (passwordCheck)
                {
                    // Password is correct
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = GenerateJwtToken(user, roles);
                    return Ok(new { Token = token });
                }
                else
                {
                    // Password is incorrect
                    return Unauthorized(new Response { Status = "Error", Message = "Invalid password" });
                }
            }
            else
            {
                // User not found
                return Unauthorized(new Response { Status = "Error", Message = "User not found" });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("AuthenticationCookie");
            // Invalidate the token on the client-side by deleting it
            return Ok(new Response { Status = "Success", Message = "Logged out successfully" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            Console.WriteLine("Email is:" + user.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                Console.WriteLine("Deos not esist ");
                return Ok(new Response { Status = "Success", Message = "If your email is registered, you will receive password reset instructions." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = Url.Action(
                "ResetPassword",
                "Authentication",
                new { token = encodedToken, email },
                Request.Scheme);

            await SendPasswordResetEmail(email, callbackUrl);

            return Ok(new Response { Status = "Success", Message = "If your email is registered, you will receive password reset instructions." });
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token, string email)
        {
            // Decode the token
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            return Ok(new { Token = token, Email = email });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok(new Response { Status = "Error", Message = "User not found." });
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);
            if (result.Succeeded)
            {
                return Ok(new Response { Status = "Success", Message = "Password reset successful." });
            }

            return BadRequest(new Response { Status = "Error", Message = "Failed to reset password." });
        }

        private async Task SendPasswordResetEmail(string email, string callbackUrl)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new System.Net.NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"]),
            };

            var message = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:FromEmail"]),
                Subject = "Reset your password",
                Body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                IsBodyHtml = true,
            };

            message.To.Add(email);

            await smtpClient.SendMailAsync(message);
        }

        private async Task Send2FACodeEmail(string email, string token)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new System.Net.NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"]),
            };

            var message = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:FromEmail"]),
                Subject = "Your 2FA Code",
                Body = $"Your 2FA code is {token}",
                IsBodyHtml = false,
            };

            message.To.Add(email);

            await smtpClient.SendMailAsync(message);
        }
        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }

        private string Generate2FAToken()
        {
            // Generate a random 6-digit token
            var rng = new Random();
            return rng.Next(100000, 999999).ToString();
        }
        private string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            // Add roles as claims
            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Generate a new JWT signing key with sufficient size (256 bits)
            // Use the consistent signing key from the configuration
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}

    public class Verify2FARequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }


    

