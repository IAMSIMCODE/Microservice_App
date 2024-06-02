using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimCode.Services.EmailApi.Models.Dto;
using SimCode.Services.EmailApi.Services;

namespace SimCode.Services.EmailApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponController(ICouponService couponService) : ControllerBase
    {
        private readonly ICouponService _couponService = couponService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var coupoons = await _couponService.GetAll();
            return Ok(coupoons);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var coupoon = await _couponService.GetSingle(id);
            return Ok(coupoon);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("GetByCode/{couponCode}")]
        public async Task<IActionResult> GetByCode(string couponCode)
        {
            var coupoon = await _couponService.GetSingleByCode(couponCode);
            return Ok(coupoon);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateCoupon([FromBody] CouponDto couponDto)
        {
            var coupoon = await _couponService.CreateCoupon(couponDto);
            return Ok(coupoon);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCoupon([FromBody] CouponUpdateDto couponDto)
        {
            var coupoon = await _couponService.UpdateCoupon(couponDto);
            return Ok(coupoon);
        }

        [HttpDelete]
		[Route("{id:int}")]
		public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupoon = await _couponService.DeleteCoupon(id);
            return Ok(coupoon);
        }
    }
}
