using System.ComponentModel.DataAnnotations;

namespace SimCode.Web.Models.Dto.Request
{
    public class RegRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
