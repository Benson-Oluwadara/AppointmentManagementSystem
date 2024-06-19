using System.Net;

namespace FrontEndService.Models
{
    public class WebAPIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
        public string Token { get; set; } // Add this if the token is not part of Result
    }
}
