using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimCode.ServiceBus.Services;
using SimCode.Services.EmailApi.Data;
using SimCode.Services.ShoppingCartApi.Models;
using SimCode.Services.ShoppingCartApi.Models.AppResponse;
using SimCode.Services.ShoppingCartApi.Models.Dto;
using SimCode.Services.ShoppingCartApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;

namespace SimCode.Services.ShoppingCartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CartController(AppDbContext context, IMapper mapper, IProductService productService, IMessageBus messageBus, IConfiguration configuration) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ApiResponse _response = new();
        private readonly IProductService _productService = productService;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost]
        [Route("cartUpsert")]
        public async Task<ApiResponse> CartUpsert([FromBody] CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //Create cartheader and CartDetail
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _context.CartHeaders.Add(cartHeader);
                    await _context.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _context.cartDetails.Add(_mapper.Map<CartDetail>(cartDto.CartDetails.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //Check if the details has the same product 

                    var cartDetailFromDb = await _context.cartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.
                                                                      First().ProductId && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (cartDetailFromDb == null)
                    {
                        //Create cartDetails 
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _context.cartDetails.Add(_mapper.Map<CartDetail>(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //Update count in cart details 
                        cartDto.CartDetails.First().Count += cartDetailFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailId = cartDetailFromDb.CartDetailId;

                        _context.cartDetails.Update(_mapper.Map<CartDetail>(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                }

                _response.Result = cartDto;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }


        [HttpPost]
        [Route("RemoveCart")]
        public async Task<ApiResponse> RemoveCart([FromBody] int cartDetailId)
        {
            try
            {
                CartDetail cartDetail = _context.cartDetails.First(u => u.CartDetailId == cartDetailId);

                int totalCountOfCartItem = _context.cartDetails.Where(u => u.CartHeaderId == cartDetail.CartHeaderId).Count();

                _context.cartDetails.Remove(cartDetail);

                if (totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetail.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _context.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpGet]
        [Route("GetCart/{userId}")]
        public async Task<ApiResponse> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_context.CartHeaders.First(u => u.UserId == userId))
                };

                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailDto>>(_context.cartDetails.
                                    Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                //IEnumerable<ProductDto> productDtos = await _productService.GetProducts();
                var products = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    cart.CartHeader.CartTotal += (item.Count * item.ProductDto.Price);
                }

                _response.Result = cart;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpPost]
        [Route("emailCartRequest")]
        public async Task<ApiResponse> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {
                await _messageBus.PublishMessage(cartDto, _configuration.GetValue<string>("TopicAndQueueNames:EmailCartQueue"));
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        //
    //    public async string GetLoggedInUserFromJwtToken()
    //    {
    //        var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
    //        return userId;
    //    }
    }
}
