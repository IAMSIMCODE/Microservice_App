using SimCode.Services.ShoppingCartApi.Models.Dto;

namespace SimCode.Services.ShoppingCartApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
