using DTOs;
using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<OrdersDTO> addOrder(OrdersDTO order);
        Task<OrdersDTO> getOrderById(int id);
    }
}