using FrontEndService.Models;
using FrontEndService.Models.WebDTO.LoginDTO;
using FrontEndService.Models.WebDTO.SignUpDTO;
using FrontEndService.Services.IServices;
using FrontEndService.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace FrontEndService.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider, ILogger<AuthController> logger)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(loginUserDTO);
        //    }

        //    var response = await _authService.LoginAsync<WebAPIResponse>(loginUserDTO);

        //    var responseJson = JsonConvert.SerializeObject(response);
        //    _logger.LogInformation("Response received: {ResponseJson}", responseJson);

        //    if (response.IsSuccess)
        //    {
        //        var token = response.Result.ToString();
        //        _tokenProvider.SetToken(token);

        //        var handler = new JwtSecurityTokenHandler();
        //        var jwtToken = handler.ReadJwtToken(token);

        //        var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
        //        Console.WriteLine($"Role Claim is:{roleClaim}");
        //        if (roleClaim != null)
        //        {
        //            var role = roleClaim.Value;
        //            if (role == "Doctor")
        //            {
        //                Console.WriteLine("Doctor Dashboard!!!!!");
        //                return RedirectToAction("DoctorDashboard", "Doctor");
        //            }
        //            else if (role == "Patient")
        //            {
        //                return RedirectToAction("PatientDashboard", "Patient");
        //            }
        //        }

        //        return RedirectToAction("Index", "Home");
        //    }

        //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //    return View(loginUserDTO);
        //}

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(loginUserDTO);
            }

            var response = await _authService.LoginAsync<WebAPIResponse>(loginUserDTO);

            var responseJson = JsonConvert.SerializeObject(response);
            _logger.LogInformation("Response received: {ResponseJson}", responseJson);

            //if (response.IsSuccess)
            //{
            //    var token = response.Result.ToString();
            //    _tokenProvider.SetToken(token);

            //    var handler = new JwtSecurityTokenHandler();
            //    var jwtToken = handler.ReadJwtToken(token);

            //    var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            //    if (roleClaim != null)
            //    {
            //        var role = roleClaim.Value;
            //        if (role == "Doctor")
            //        {
            //            return RedirectToAction("Dashboard", "DoctorDashboard");
            //        }
            //        else if (role == "Patient")
            //        {
            //            return RedirectToAction("PatientDashboard", "Patient");
            //        }
            //    }

            //    return RedirectToAction("Index", "Home");
            //}

            //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //return View(loginUserDTO);
            if (response.IsSuccess)
            {
                var token = response.Result.ToString();
                _tokenProvider.SetToken(token);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                if (roleClaim != null)
                {
                    var role = roleClaim.Value;
                    if (role == "Doctor")
                    {
                        return RedirectToAction("Dashboard", "DoctorDashboard");
                    }
                    else if (role == "Patient")
                    {
                        return RedirectToAction("Dashboard", "PatientDashboard");
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginUserDTO);

        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO, string role)
        {
            if (!ModelState.IsValid)
            {
                return View(registerUserDTO);
            }

            var response = await _authService.RegisterAsync<WebAPIResponse>(registerUserDTO);
            if (response.IsSuccess)
            {
                // Assign role after successful registration
                // await _authService.AssignRoleAsync<WebAPIResponse>(registerUserDTO.Email, role);
                return RedirectToAction("Login", "Auth");
            }

            ModelState.AddModelError(string.Empty, "Registration failed.");
            return View(registerUserDTO);
        }

        [HttpGet]
        public IActionResult RoleSelection()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleSelection(string role)
        {
            if (role == "Doctor")
            {
                return RedirectToAction("DoctorRegistration");
            }
            else if (role == "Patient")
            {
                return RedirectToAction("PatientRegistration");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DoctorRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DoctorRegistration(RegisterUserDTO registerUserDTO)
        {
            return await Register(registerUserDTO, SD.RoleDoctor);
        }

        [HttpGet]
        public IActionResult PatientRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PatientRegistration(RegisterUserDTO registerUserDTO)
        {
            return await Register(registerUserDTO, SD.RolePatient);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _tokenProvider.ClearToken();
            return RedirectToAction("Login", "Auth");
        }
    }
}
