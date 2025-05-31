using Group2_SE1814_NET.Services;
using Group2_SE1814_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Group2_SE1814_NET.Controllers {
    [Route("payment/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase {
        private readonly ILogger<WebhookController> _logger;
        private readonly IOrderService _orderService;

        public WebhookController(ILogger<WebhookController> logger, IOrderService orderService) {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook([FromBody] PayOSWebhookRequest request) {


            if (request.Status == "PAID") {
                _orderService.UpdateOrderStatus(request.OrderCode, "Completed");
            }

            return Ok();
        }
    }
}