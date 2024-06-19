using FrontEndService.Models;

namespace FrontEndService.Services.IServices
{
    public interface IBaseService:IDisposable
    {
        WebAPIResponse responseModel { get; set; }
        public Task<T> SendAsync<T>(WebAPIRequest apiRequest, string clientName);
    }
}
