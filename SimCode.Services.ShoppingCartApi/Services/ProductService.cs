using Newtonsoft.Json;
using SimCode.Services.ShoppingCartApi.Models.Dto;

namespace SimCode.Services.ShoppingCartApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");

            var response = await client.GetAsync($"/api/product");

            var apiContent = await response.Content.ReadAsStringAsync();

            var res = JsonConvert.DeserializeObject<ProductDto>(apiContent);

            if (res != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(res));
            }
            return null;
        }
    }
}
