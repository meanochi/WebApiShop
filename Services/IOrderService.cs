using DTOs;
using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<OrderDTO> addOrder(OrderCreateDTO order);
        Task<OrderDTO> getOrderById(int id);
    }
}