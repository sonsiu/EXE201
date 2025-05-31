using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.ViewModels;

namespace Group2_SE1814_NET.Repositories {
    public interface IOrderRepository {
        public void AddOrder(Order order, List<Item> cart);
        public List<Order> GetAll();
        public List<Order> GetByUserId(int userId);
        public Order GetById(int id);
        public void UpdateOrder(Order order);
        void UpdateOrderStatus(int orderId, string status);
        int GetMaxOrderId();
        Task<List<Order>> GetByUserID(int userId);

    }
}
