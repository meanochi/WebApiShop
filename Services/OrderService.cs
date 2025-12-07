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

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }
        public async Task<Order> getOrderById(int id)
        {
            return await _repository.getOrderById(id);
        }
        public async Task<Order> addOrder(Order order)
        {
            return await _repository.addOrder(order);
        }
    }
}
