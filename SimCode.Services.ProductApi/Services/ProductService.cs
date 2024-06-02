using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimCode.Services.EmailApi.Data;
using SimCode.Services.EmailApi.Models;
using SimCode.Services.EmailApi.Models.AppResponse;
using SimCode.Services.EmailApi.Models.Dto;

namespace SimCode.Services.EmailApi.Services
{
    public class ProductService(AppDbContext context, IMapper mapper) : IProductService
    {
        private readonly AppDbContext _context = context;
        private readonly ApiResponse _response = new();
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> GetAll()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                if (products.Count > 0)
                {
                    _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
                }
                else { ReturnResponse(false, "Data not found", "01"); }
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        public async Task<ApiResponse> GetSingle(int id)
        {
            try
            {
                var coupon = await _context.Products.FindAsync(id);
                if (coupon != null)
                {
                    _response.Result = _mapper.Map<ProductDto>(coupon);
                }
                else {ReturnResponse(false, "Data not found", "01");}
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;   
        }

        public async Task<ApiResponse> GetSingleByCode(string productName)
        {
            try
            {
                var coupon = await _context.Products.FirstOrDefaultAsync(c => c.ProductName.Equals(productName, StringComparison.CurrentCultureIgnoreCase));
                if (coupon != null)
                {
                    _response.Result = _mapper.Map<ProductDto>(coupon);
                }
                else { ReturnResponse(false, "Data not found", "01"); }
            }
            catch (Exception) { ReturnResponse(false, "Data not found", ""); }
            return _response;
        }

        public async Task<ApiResponse> CreateProduct(ProductDto product)
        {
            try
            {
                var proObj = _mapper.Map<Product>(product);
                await _context.Products.AddAsync(proObj);

                await _context.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(proObj);
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        public async Task<ApiResponse> DeleteProduct(int id)
        {
            try
            {
                var coupon = await _context.Products.FindAsync(id);
                if (coupon != null)
                {
                    _context.Products.Remove(coupon);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        public async Task<ApiResponse> UpdateProduct(ProductDto product)
        {
            try
            {
                var copObj = _mapper.Map<Product>(product);
                _context.Products.Update(copObj);

                await _context.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(copObj);
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        private void ReturnResponse(bool isSuccess, string message, string statusCode)
        {
            _response.IsSuccess = isSuccess;
            _response.Message = message;
            _response.StatusCode = statusCode;
        }
    }
}
