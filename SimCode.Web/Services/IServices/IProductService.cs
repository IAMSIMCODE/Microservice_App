using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto;

namespace SimCode.Web.Services.IServices
{
    public interface IProductService
    {
        Task<ApiResponse> GetAllProductAsync();
        Task<ApiResponse> GetProductByIdAsync(int id);
        Task<ApiResponse> GetProductAsync(string productName);
        Task<ApiResponse> CreateProductAsync(ProductDto productDto);
        Task<ApiResponse> UpdateProductAsync(ProductDto productDto);
        Task<ApiResponse> DeleteProductAsync(int id);
    }
}
