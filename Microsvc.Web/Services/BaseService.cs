using Microsvc.Web.Models;
using Microsvc.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Microsvc.Web.Utility.SD;

namespace Microsvc.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MicrosvcAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResonse = null;

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResonse = await client.SendAsync(message);

                switch (apiResonse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    default:
                        var apiContent = await apiResonse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto()
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return dto;
            }
        }    
    }
}
