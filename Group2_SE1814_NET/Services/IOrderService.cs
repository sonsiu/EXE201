using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.ViewModels;

namespace Group2_SE1814_NET.Services {
    public interface IOrderService {
        public void AddOrder(Order order, List<Item> cart);
        public List<Order> GetAll();
        public Order GetById(int id);
        public List<Order> GetByUserId(int userId);
        public void UpdateOrder(Order order);

        public void UpdateOrderStatus(int orderId, string status);
        int GetMaxOrderId();
        Task<List<Order>> GetByUserID(int userId);

    }
}
