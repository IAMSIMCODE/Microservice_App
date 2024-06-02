using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimCode.Services.EmailApi.Models.Dto;
using SimCode.Services.EmailApi.Services;

namespace SimCode.Services.EmailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var coupoons = await _productService.GetAll();
            return Ok(coupoons);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var coupoon = await _productService.GetSingle(id);
            return Ok(coupoon);
        }

        [HttpGet]
        //[Authorize(Roles = "ADMIN")]
        [Route("GetByCode/{productName}")]
        public async Task<IActionResult> GetByCode(string productName)
        {
            var coupoon = await _productService.GetSingleByCode(productName);
            return Ok(coupoon);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateCoupon([FromBody] ProductDto productDto)
        {
            var coupoon = await _productService.CreateProduct(productDto);
            return Ok(coupoon);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCoupon([FromBody] ProductDto productDto)
        {
            var product = await _productService.UpdateProduct(productDto);
            return Ok(product);
        }

        [HttpDelete]
		[Route("{id:int}")]
		public async Task<IActionResult> DeleteCoupon(int id)
        {
            var product = await _productService.DeleteProduct(id);
            return Ok(product);
        }
    }
}
