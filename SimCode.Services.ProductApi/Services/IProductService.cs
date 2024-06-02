using SimCode.Services.EmailApi.Models.AppResponse;
using SimCode.Services.EmailApi.Models.Dto;

namespace SimCode.Services.EmailApi.Services
{
    public interface IProductService
    {
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetSingle(int id);
        Task<ApiResponse> GetSingleByCode(string ProductName);
        Task<ApiResponse> CreateProduct(ProductDto product);
        Task<ApiResponse> UpdateProduct(ProductDto product);
        Task<ApiResponse> DeleteProduct(int id);
    }
}
