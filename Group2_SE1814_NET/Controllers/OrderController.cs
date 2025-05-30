using Group2_SE1814_NET.Extensions;
using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Proxy;
using Group2_SE1814_NET.Services;
using Group2_SE1814_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Group2_SE1814_NET.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IGHNService _ghnService;
        public OrderController(IOrderService orderService, IGHNService ghnService)
        {
            _orderService = orderService;
            _ghnService = ghnService;
        }
        public IActionResult Index()
        {
            User u = HttpContext.Session.GetObjectFromSession<User>("user");
            var orders = _orderService.GetByUserId(u.Id);
            return View(orders);
        }
        public IActionResult Detail(int id = 0)
        {
            var order = _orderService.GetById(id);
            return View(order);
        }

        //[HttpGet]
        //[Route("index")]
        //public async Task<IActionResult> GetAllOrder()
        //{

        //    List<OrderGHNViewModel> orders = await _ghnService.GetAllOrders();
        //    ViewBag.Orders = orders;
        //    return View("~/Views/Order/Index.cshtml");
        //}

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> GetAllOrder()
        {
            User user = Extensions.SessionExtensions.GetObjectFromSession<User>(HttpContext.Session, "user");
            
            List<Order> orders = await _orderService.GetByUserID(user.Id);
            List<OrderGHNViewModel> orderGHN = await _ghnService.GetAllOrders();
            List<OrderGHNViewModel> filterOrderGHN = orderGHN.Where(x => orders.Any(y => y.OrderCode == x.OrderCode)).ToList();
            ViewBag.Orders = filterOrderGHN;
            return View("~/Views/Order/Index.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> Detail([FromQuery] string orderCode)
        {
            var order = await _ghnService.GetOrderDetailByOrderCode(orderCode);
            ViewBag.Order = order;
            return View("~/Views/Order/Detail.cshtml");
        }


        [HttpPost]
        public IActionResult CancelOrder(string orderCode)
        {
            _ghnService.CancelOrderById(orderCode);
            return RedirectToAction("GetAllOrder");
        }

    }
}
