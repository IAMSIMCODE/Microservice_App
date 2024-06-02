using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimCode.Services.AuthAPI.Configuration.AppConfig;
using SimCode.Services.AuthAPI.Models.User;
using SimCode.Services.AuthAPI.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimCode.Services.AuthAPI.Services
{
    public class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions) : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new(JwtRegisteredClaimNames.Name, applicationUser.UserName),
                new(JwtRegisteredClaimNames.Email, applicationUser.Email),
     
            };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            
            //foreach (var claim in claims)
            //{

            //}


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
