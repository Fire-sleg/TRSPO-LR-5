using AutoMapper;
using BasketAPI.Models;
using BasketAPI.Models.Dto;
using Microsoft.Extensions.Logging;

namespace BasketAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {

            CreateMap<Product, ProductDTO>().ReverseMap(); ;

            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
        }

    }
}
