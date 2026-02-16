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
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserReadDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<UserLoginDTO, User>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category> ();
            CreateMap<OrderedSeat, OrderedSeatReadDTO>();
            CreateMap<OrderUpdateDTO, Order>();

        }
    }
}
//michmich!!145