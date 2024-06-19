using System.Security.AccessControl;
using static FrontEndService.Utility.SD;

namespace FrontEndService.Models
{
    public class WebAPIRequest
    {
        public ApiType apiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}
