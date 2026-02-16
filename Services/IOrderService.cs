using DTOs;
using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<OrderDTO> addOrder(OrderCreateDTO order);
        Task<List<OrderDTO>> getAllOrders();
        Task<OrderDTO> getOrderById(int id);
        Task<List<OrderDTO>> getOrdersForUser(int id);
        Task<OrderDTO> updateOrder(OrderUpdateDTO orderToUpdate, int id);
    }
}