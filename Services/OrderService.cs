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
        public async Task<OrderDTO> getOrderById(int id)
        {
            Order order = await _repository.getOrderById(id);
            OrderDTO orderDTO = _mapper.Map<Order, OrderDTO>(order);
            return orderDTO;
        }
        public async Task<OrderDTO> addOrder(OrderCreateDTO orderCDTO)
        {
            Order order = _mapper.Map<OrderCreateDTO, Order>(orderCDTO);
            order = await _repository.addOrder(order);
            OrderDTO orderDTO = _mapper.Map < Order, OrderDTO > (order);
            return orderDTO;
        }
    }
}
