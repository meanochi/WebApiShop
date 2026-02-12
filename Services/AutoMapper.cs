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
            CreateMap<OrderItemDTO, OrderItem>();
            CreateMap<UserLoginDTO, User>();

        }
    }
}
//michmich!!145