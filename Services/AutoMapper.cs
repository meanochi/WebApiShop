using AutoMapper;
using DTOs;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AutoMapper: Profile
    {
        public AutoMapper() {
            CreateMap<Order, OrdersDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductsDTO>().ForMember(
                product => product.CategoryName,
                productDTO => productDTO.MapFrom(src => src.Category.CategoryName)
    ).ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();


        }
    }
}
