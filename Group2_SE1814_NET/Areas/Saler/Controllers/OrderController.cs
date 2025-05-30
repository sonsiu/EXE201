using Group2_SE1814_NET.Extensions;
using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Proxy;
using Group2_SE1814_NET.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Drawing.Drawing2D;
using X.PagedList.Extensions;

namespace Group2_SE1814_NET.Areas.Saler.Controllers
{
    [Area("Saler")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IGHNService _ghnService;
        public OrderController(IOrderService orderService, IGHNService gHNService)
        {
            _orderService = orderService;
            _ghnService = gHNService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _ghnService.GetAllOrders();
            ViewBag.Orders = orders;
            return View("~/Areas/Saler/Views/Order/Index.cshtml");
        }

        public async Task<IActionResult> Detail([FromQuery] string orderCode)
        {
            var order = await _ghnService.GetOrderDetailByOrderCode(orderCode);
            ViewBag.Order = order;
            return View("~/Areas/Saler/Views/Order/Detail.cshtml");
        }
        public IActionResult UpdateStatus(string orderCode, string orderStatus)
        {

            _ghnService.UpdateOrderStatusById(orderCode, orderStatus);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateNote(string orderCode, string note = " ")
        {
            _ghnService.UpdateOrderNoteById(orderCode, note);
            return RedirectToAction("Index");
        }

    }
}
