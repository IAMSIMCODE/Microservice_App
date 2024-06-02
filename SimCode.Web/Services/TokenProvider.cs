using Newtonsoft.Json.Linq;
using SimCode.Web.Services.IServices;
using static SimCode.Web.Utility.StaticDetail;

namespace SimCode.Web.Services
{
    public class TokenProvider(IHttpContextAccessor contextAccessor) : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public void ClearToken()
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(TokenCookie);
        }

        public string GetToken()
        {
            string token = null;
            bool hasToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(TokenCookie, out token);
            return hasToken is true ? token : null; 
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(TokenCookie, token);
        }
    }
}
