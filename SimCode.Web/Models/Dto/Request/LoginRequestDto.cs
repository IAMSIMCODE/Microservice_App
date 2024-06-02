using System.ComponentModel.DataAnnotations;

namespace SimCode.Web.Models.Dto.Request
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
