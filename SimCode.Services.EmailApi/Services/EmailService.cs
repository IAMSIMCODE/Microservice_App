using Microsoft.EntityFrameworkCore;
using SimCode.Services.EmailApi.Data;
using SimCode.Services.EmailApi.Models.Dto;

namespace SimCode.Services.EmailApi.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public Task EmailCartLog(CartDto cartDto)
        {
            throw new NotImplementedException();
        }
    }
}
