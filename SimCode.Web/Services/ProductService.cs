using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto;
using SimCode.Web.Models.Dto.Request;
using SimCode.Web.Services.IServices;
using static SimCode.Web.Utility.StaticDetail;

namespace SimCode.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ApiResponse> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                ApiUrl = ProductApiBase + "api/product"
            });
        }

        public async Task<ApiResponse> GetProductAsync(string productName)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                ApiUrl = ProductApiBase + "api/product/GetByProductName/" + productName
            });
        }

        public async Task<ApiResponse> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                ApiUrl = ProductApiBase + "api/product/" + id
            });
        }

        public async Task<ApiResponse> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = productDto,
                ApiUrl = ProductApiBase + "api/product/"
            });
        }

        public async Task<ApiResponse> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = productDto,
                ApiUrl = ProductApiBase + "api/product/"
            });
        }

        public async Task<ApiResponse> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                ApiUrl = ProductApiBase + "api/product/" + id
            });
        }
    }
}
