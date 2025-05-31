using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Group2_SE1814_NET.Repositories {
    public class OrderRepository : IOrderRepository {
        private readonly WebkinhdoanhquanaoContext _context;

        public OrderRepository(WebkinhdoanhquanaoContext context) {
            _context = context;
        }
        public void AddOrder(Order order, List<Item> cart) {
            _context.Orders.Add(order);
            _context.SaveChanges();
            foreach (var item in cart) {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = Math.Round((decimal)(item.Quantity * item.Product.Price), 2)
                };
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
        }
        public int GetMaxOrderId() {
            // If using Entity Framework Core
            if (_context.Orders.Any()) {
                return _context.Orders.Max(o => o.Id);
            }

            // If no orders exist, return 0 or another default value
            return 0;
        }


        public List<Order> GetAll() {
            return _context.Orders.ToList();
        }

        public Order GetById(int id) {
            return _context.Orders
                     .Include(x => x.OrderDetails)
                     .ThenInclude(od => od.Product)
                     .FirstOrDefault(x => x.Id == id);
        }

        public List<Order> GetByUserId(int userId) {
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public async Task<List<Order>> GetByUserID(int userId) {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public void UpdateOrder(Order order) {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        public void UpdateOrderStatus(int orderId, string status) {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null) {
                order.OrderStatus = status;
                _context.SaveChanges();
            }
        }

    }
}
