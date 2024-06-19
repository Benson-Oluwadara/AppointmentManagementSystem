using FrontEndService.Services.IServices;
using FrontEndService.Utility;

namespace FrontEndService.Services.Services
{
    public class TokenProvider:ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<TokenProvider> _logger;

        public TokenProvider(IHttpContextAccessor contextAccessor, ILogger<TokenProvider> logger)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
        }


        //public void ClearToken()
        //{
        //    _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
        //}

        //public string? GetToken()
        //{
        //    string? token = null;
        //    bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);
        //    return hasToken is true ? token : null;
        //}

        //public void SetToken(string token)
        //{
        //    _contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
        //}


        public void ClearToken()
        {
            _logger.LogInformation("Clearing token from cookies.");
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
        }

        public string? GetToken()
        {
            _logger.LogInformation("Retrieving token from cookies.");
            string? token = null;
            bool? hasToken = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);
            _logger.LogInformation("Token retrieved: {Token}", token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _logger.LogInformation("Setting token in cookies.");
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
        }
    }
}
