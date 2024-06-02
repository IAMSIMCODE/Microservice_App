using SimCode.Services.EmailApi.Models.Dto;

namespace SimCode.Services.EmailApi.Services
{
    public interface IEmailService
    {
        Task EmailCartLog(CartDto cartDto);
    }
}
