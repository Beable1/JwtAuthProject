using AutoMapper;
using JwtAuthProject.Core.Dtos;
using JwtAuthProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JwtAuthProject.Service.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<UserApp, UserAppDto>().ReverseMap();

        }
    }
}
