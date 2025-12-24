using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        IOrderRepository _repository;
        IMapper _mapper;

        public OrderService(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }
        public async Task<OrdersDTO> getOrderById(int id)
        {
            Order order = await _repository.getOrderById(id);
            OrdersDTO orderDTO = _mapper.Map<Order, OrdersDTO>(order);
            return orderDTO;
        }
        public async Task<OrdersDTO> addOrder(OrdersDTO orderDTO)
        {
            Order order = _mapper.Map<OrdersDTO, Order>(orderDTO);
            order = await _repository.addOrder(order);
            orderDTO = _mapper.Map < Order, OrdersDTO > (order);
            return orderDTO;
        }
    }
}
