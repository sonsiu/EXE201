using Group2_SE1814_NET.Proxy;
using Group2_SE1814_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Group2_SE1814_NET.Controllers {
    public class PaymentController : Controller {

        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService) {

            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model) {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }



    }
}