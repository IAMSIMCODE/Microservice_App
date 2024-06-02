using AutoMapper;
using SimCode.Services.EmailApi.Models;
using SimCode.Services.EmailApi.Models.Dto;

namespace SimCode.Services.EmailApi.AutoMapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                //config.CreateMap<CouponUpdateDto, Coupon>();
            });
            return mappingConfig;
        }
    }
}
