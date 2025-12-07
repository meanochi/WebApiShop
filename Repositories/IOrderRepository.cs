using Entities;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<Order> addOrder(Order order);
        Task<List<Order>> getAllOrders();
        Task<Order> getOrderById(int id);
        Task<List<Order>> getOrdersForUser(User user);
        Task<Order> updateOrder(Order order, int id);
    }
}