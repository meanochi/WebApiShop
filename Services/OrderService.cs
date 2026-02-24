using AutoMapper;
using DTOs;
using Entities;
using Repositories;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

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
            OrderDTO orderDTO = _mapper.Map<Order, OrderDTO>(order);
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

        public async Task<OrderDTO> Checkout(CheckoutDTO orderToUpdate)
        {
            Order order = await _repository.Checkout(orderToUpdate);
            OrderDTO orderDTO = _mapper.Map<Order, OrderDTO>(order);
            return orderDTO;
        }

        public async Task<int> UnLockseat(int id, int userId)
        {
            Order order = await _repository.getOrderByOrderesSeatId(id);
            if (order.UserId == userId)
                return await _repository.unLockSeat(id);
            return 0;
        }

        public async Task<OrderedSeatReadDTO> LockSeat(LockSeatDTO orderDTO)
        {
            List<Order> ordForUser = await _repository.getOrdersForUser(orderDTO.UserId);
            Order ord = ordForUser.FirstOrDefault(o => o.OrderedSeats.Where(o => o.Status == 1) != null);
            if (ord != null)
            {
                OrderedSeat os = _mapper.Map<LockSeatDTO, OrderedSeat>(orderDTO);
                os.OrderId = ord.Id;
                OrderedSeat orderedSeat = await _repository.addOrderedSeat(os);
                return _mapper.Map<OrderedSeat, OrderedSeatReadDTO>(orderedSeat);
            }
            else
            {
                Order order = new Order();
                order.UserId = orderDTO.UserId;
                order.OrderDate = DateTime.Now;
                order.Price = 0;
                order = await _repository.addOrder(order);
                return _mapper.Map<OrderedSeat, OrderedSeatReadDTO>(order.OrderedSeats.FirstOrDefault(o => o.OrderId == order.Id));
            }
        }
    }
}
