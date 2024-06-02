using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimCode.Web.Models;
using SimCode.Web.Models.Dto;
using SimCode.Web.Services.IServices;
using SimCode.Web.Utility;
using System.Diagnostics;

namespace SimCode.Web.Controllers
{
    public class HomeController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index()
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

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto product = new();

            var response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(product);
        }

        [Authorize(Roles = StaticDetail.RoleAdmin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
