using static SimCode.Web.Utility.StaticDetail;

namespace SimCode.Web.Models.Dto.Request
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
