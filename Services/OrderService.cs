using AutoMapper;
using DTOs;
using Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<OrderDTO>> getAllOrders()
        {
            List<Order> orders = await _repository.getAllOrders();
            List<OrderDTO> ordersDTO = _mapper.Map<List<Order>, List<OrderDTO>>(orders);
            return ordersDTO;
        }

        public async Task<List<OrderDTO>> getOrdersForUser(int id)
        {
            List<Order> orders = await _repository.getOrdersForUser(id);
            List<OrderDTO> ordersDTO = _mapper.Map<List<Order>, List<OrderDTO>>(orders);
            return ordersDTO;
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
            order.OrderDate = DateTime.Now;
            order = await _repository.addOrder(order);
            OrderDTO orderDTO = _mapper.Map < Order, OrderDTO > (order);
            return orderDTO;
        }

        public async Task<OrderDTO> updateOrder(OrderUpdateDTO orderToUpdate, int id)
        {
            Order order = _mapper.Map<OrderUpdateDTO, Order>(orderToUpdate);
            order.OrderDate = DateTime.Now;
            order = await _repository.updateOrder(order);
            OrderDTO orderDTO = _mapper.Map<Order, OrderDTO>(order);
            return orderDTO;
        }
    }
}
