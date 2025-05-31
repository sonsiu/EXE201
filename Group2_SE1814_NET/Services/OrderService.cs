using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Repositories;
using Group2_SE1814_NET.ViewModels;

namespace Group2_SE1814_NET.Services {
    public class OrderService : IOrderService {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository) {
            _orderRepository = orderRepository;
        }
        public void AddOrder(Order order, List<Item> cart) {
            _orderRepository.AddOrder(order, cart);
        }

        public List<Order> GetAll() {
            return _orderRepository.GetAll();
        }

        public Order GetById(int id) {
            return _orderRepository.GetById(id);
        }

        public List<Order> GetByUserId(int userId) {
            return _orderRepository.GetByUserId(userId);
        }

        public async Task<List<Order>> GetByUserID(int userId) {
            return await _orderRepository.GetByUserID(userId);
        }

        public void UpdateOrder(Order order) {
            _orderRepository.UpdateOrder(order);
        }
        public void UpdateOrderStatus(int orderId, string status) {
            var order = _orderRepository.GetById(orderId);
            if (order != null) {
                _orderRepository.UpdateOrderStatus(orderId, status);
            }
        }
        // In your OrderService class
        public int GetMaxOrderId() {
            // Using the repository to get maximum order ID
            return _orderRepository.GetMaxOrderId();
        }

    }
}
