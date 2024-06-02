using Newtonsoft.Json;
using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto.Request;
using SimCode.Web.Services.IServices;
using System.Net;
using System.Text;
using static SimCode.Web.Utility.StaticDetail;

namespace SimCode.Web.Services
{
    public class BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        public async Task<ApiResponse> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("SimCodeApi");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                //token when i add authentication 
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(requestDto.ApiUrl);

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                message.Method = requestDto.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get,
                };

                var httpResponse = await client.SendAsync(message);

                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found", StatusCode = "404" };

                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized", StatusCode = "401" };

                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied", StatusCode = "403" };

                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error", StatusCode = "500" };
                    default:
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                        return apiResponse;
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
