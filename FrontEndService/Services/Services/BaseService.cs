using FrontEndService.Models;
using FrontEndService.Services.IServices;
using FrontEndService.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FrontEndService.Services.Services
{
    public class BaseService : IBaseService
    {
        public WebAPIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClient,ITokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.responseModel = new WebAPIResponse();
            _tokenProvider = tokenProvider;
        }

        public async Task<T> SendAsync<T>(WebAPIRequest apiRequest, string clientName)
        {
            try
            {
                var client = httpClient.CreateClient(clientName);
                // Get the JWT token from the token provider
                string jwtToken = _tokenProvider.GetToken();

                HttpRequestMessage message = new HttpRequestMessage
                {
                    Headers = { { "Accept", "application/json" } },
                    RequestUri = new Uri(apiRequest.Url)
                };

                UriBuilder uriBuilder = new UriBuilder(apiRequest.Url);
                message.RequestUri = uriBuilder.Uri;

                // Add the JWT token to the request headers
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }

                switch (apiRequest.apiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        Console.WriteLine($"POST Request from {clientName} to {apiRequest.Url}");
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        Console.WriteLine($"PUT Request from {clientName} to {apiRequest.Url}");
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        Console.WriteLine($"DELETE Request from {clientName} to {apiRequest.Url}");
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        Console.WriteLine($"GET Request from {clientName} to {apiRequest.Url}");
                        break;
                }

                Console.WriteLine($"Request URL: {message.RequestUri}");
                Console.WriteLine($"Request Type: {apiRequest.apiType}");
                if (apiRequest.Data != null)
                {
                    Console.WriteLine($"Request Payload: {JsonConvert.SerializeObject(apiRequest.Data)}");
                }

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Status Code: {apiResponse.StatusCode}");
                Console.WriteLine($"Response Headers: {apiResponse.Headers}");
                Console.WriteLine($"Response=== Content: {apiContent}");

                try
                {
                    // Directly parse the token if the structure of WebAPIResponse does not match
                    var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiContent);
                    if (responseObject != null && responseObject.ContainsKey("token"))
                    {
                        var apiResponseObj = new WebAPIResponse
                        {
                            IsSuccess = true,
                            Result = responseObject["token"],
                            StatusCode = HttpStatusCode.OK
                        };
                        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(apiResponseObj));
                    }

                    var ApiResponse = JsonConvert.DeserializeObject<WebAPIResponse>(apiContent);
                    if (ApiResponse != null && (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        Console.WriteLine($"Error Occurred: {string.Join(", ", ApiResponse.ErrorMessages)}");
                        ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception occurred while processing the API response");
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception e)
            {
                var dto = new WebAPIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                if (e.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                }
                return APIResponse;
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}

//using FrontEndService.Models;
//using FrontEndService.Services.IServices;
//using FrontEndService.Utility;
//using Newtonsoft.Json;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using Microsoft.Extensions.Logging;

//namespace FrontEndService.Services.Services
//{
//    public class BaseService : IBaseService
//    {
//        public WebAPIResponse responseModel { get; set; }
//        public IHttpClientFactory httpClient { get; set; }
//        //private readonly ITokenProvider _tokenProvider;
//        private readonly ILogger<BaseService> _logger;

//        public BaseService(IHttpClientFactory httpClient,/* ITokenProvider tokenProvider, */ILogger<BaseService> logger)
//        {
//            this.httpClient = httpClient;
//            this.responseModel = new WebAPIResponse();
//            //_tokenProvider = tokenProvider;
//            _logger = logger;
//        }

//        public async Task<T> SendAsync<T>(WebAPIRequest apiRequest, string clientName)
//        {
//            try
//            {
//                var client = httpClient.CreateClient(clientName);
//                //var token = _tokenProvider.GetToken();

//                HttpRequestMessage message = new HttpRequestMessage
//                {
//                    Headers = { { "Accept", "application/json" } },
//                    RequestUri = new Uri(apiRequest.Url)
//                };

//                if (!string.IsNullOrEmpty(token))
//                {
//                    message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//                    _logger.LogInformation("Token added to request headers.");
//                }

//                UriBuilder uriBuilder = new UriBuilder(apiRequest.Url);
//                message.RequestUri = uriBuilder.Uri;

//                if (apiRequest.Data != null)
//                {
//                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
//                }

//                switch (apiRequest.apiType)
//                {
//                    case SD.ApiType.POST:
//                        message.Method = HttpMethod.Post;
//                        break;
//                    case SD.ApiType.PUT:
//                        message.Method = HttpMethod.Put;
//                        break;
//                    case SD.ApiType.DELETE:
//                        message.Method = HttpMethod.Delete;
//                        break;
//                    default:
//                        message.Method = HttpMethod.Get;
//                        break;
//                }

//                HttpResponseMessage apiResponse = await client.SendAsync(message);

//                var apiContent = await apiResponse.Content.ReadAsStringAsync();
//                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
//                return APIResponse;
//            }
//            catch (Exception e)
//            {
//                var dto = new WebAPIResponse
//                {
//                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
//                    IsSuccess = false
//                };
//                var res = JsonConvert.SerializeObject(dto);
//                var APIResponse = JsonConvert.DeserializeObject<T>(res);
//                return APIResponse;
//            }
//        }

//        public void Dispose()
//        {
//            GC.SuppressFinalize(true);
//        }
//    }
//}
