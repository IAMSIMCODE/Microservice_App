using AutoMapper;
using SimCode.Services.ShoppingCartApi.Models;
using SimCode.Services.ShoppingCartApi.Models.Dto;

namespace SimCode.Services.ShoppingCartApi.AutoMapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailDto, CartDetail>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
