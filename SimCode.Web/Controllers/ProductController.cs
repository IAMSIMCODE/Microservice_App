using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimCode.Web.Models.Dto;
using SimCode.Web.Models.Dto.Request;
using SimCode.Web.Services.IServices;

namespace SimCode.Web.Controllers
{
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> productList = [];

            var response = await _productService.GetAllProductAsync();
            if (response != null && response.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(productList);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> ProductCreate(ProductDto productDto) 
		{
			if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync(productDto);
                if (response != null && response.IsSuccess) 
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex)); 
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(productDto);
		}

		public async Task<IActionResult> ProductDelete(int productId)
		{
			var response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());  
                return View(product);
            }
            else
            {
                TempData["error"] = response.Message;
            } 
            return NotFound();
		}

        [HttpPost]
		public async Task<IActionResult> ProductDelete(ProductDto productDto)
		{
			var response = await _productService.DeleteProductAsync(productDto.ProductId);
			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
			}
            else
            {
                TempData["error"] = response.Message;
            }
            return View(productDto);
		}

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
                return View(product);
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            var response = await _productService.UpdateProductAsync(productDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(productDto);
        }
    }
}
