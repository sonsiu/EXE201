using Group2_SE1814_NET.Extensions;
using Group2_SE1814_NET.Libraries;
using Group2_SE1814_NET.Models;
using Group2_SE1814_NET.Proxy;
using Group2_SE1814_NET.Repositories;
using Group2_SE1814_NET.Services;
using Group2_SE1814_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Group2_SE1814_NET.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IGHNService _ghnService;
        private readonly IVnPayService _vnPayService;
        private readonly IPayosService _payosService;

        public CartController(IProductService productService, IOrderService orderService, IGHNService ghnService, IVnPayService vnPayService, IPayosService payOSService)
        {
            _productService = productService;
            _orderService = orderService;
            _ghnService = ghnService;
            _vnPayService = vnPayService;
            _payosService = payOSService;
        }
        public IActionResult Index()
        {
            var shoppingCart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart") ?? new List<Item>();
            return View(shoppingCart);
        }

        public IActionResult Add(int id = 0)
        {
            var p = _productService.GetProductById(id);
            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
            if (cart == null) //chua có sp trong giỏ
            {
                cart = new List<Item>();  ///tao new list
				cart.Add(new Item { Product = p, Quantity = 1 }); //them sp chưa có vào giỏ
            }
            else //có sp trong giỏ
            {
                Item existingItem = cart.FirstOrDefault(i => i.Product.Id == id);
                if (existingItem != null) // Nếu sản phẩm id{?} đã có trong giỏ hàng
                {
                    if (existingItem.Quantity < existingItem.Product.Quantity)
                    {
                        existingItem.Quantity += 1; // Tăng số lượng sản phẩm lên
                    }
                }
                else
                {
                    cart.Add(new Item { Product = p, Quantity = 1 }); //them sp thuôc id ? chưa có vào giỏ
                }
            }
            HttpContext.Session.SetObjectAsSession("cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult Sub(int id = 0)
        {
            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
            Item existingItem = cart.FirstOrDefault(i => i.Product.Id == id);
            if (existingItem.Quantity > 1)
            {
                existingItem.Quantity -= 1; // Giam số lượng sản phẩm lên
            }
            else
            {
                cart.Remove(existingItem);
            }
            HttpContext.Session.SetObjectAsSession("cart", cart);
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int id = 0)
        {
            List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
            Item existingItem = cart.FirstOrDefault(i => i.Product.Id == id);
            cart.Remove(existingItem);
            HttpContext.Session.SetObjectAsSession("cart", cart);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var shoppingCart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart") ?? new List<Item>();
            // Calculate the total price of items in the cart
            var total = shoppingCart.Sum(x => x.Quantity * x.Product.Price);

            // Check if the total is 0
            if (total == 0)
            {
                // If total is 0, redirect to Cart page
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.Cart = shoppingCart; // Đảm bảo dòng này có trong action
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderViewModel orderView)
        {
            if (ModelState.IsValid)
            {
                User u = HttpContext.Session.GetObjectFromSession<User>("user");

                if (u != null)
                {
                    List<Item> cart = HttpContext.Session.GetObjectFromSession<List<Item>>("cart");
                    if (cart == null || !cart.Any())
                    {
                        TempData["ErrorMessage"] = "Your cart is empty!";
                        return RedirectToAction("Index");
                    }

                    string fullAddress = $"{orderView.StreetAddress}; {orderView.ProvinceName}; {orderView.DistrictName}; {orderView.WardName}";

                    Order order = new Order();
                    order.UserId = u.Id;

                    order.TotalAmountBefore = Math.Round((double)cart.Sum(x => x.Quantity * x.Product.Price), 2);
                    order.OrderDate = DateTime.Now;
                    order.PaymentMethod = orderView.PaymentMethod;
                    order.OrderStatus = orderView.PaymentMethod == "COD" ? "Processing" : "Pending"; // Trạng thái tùy thuộc vào phương thức
                    order.Name = orderView.Name;
                    order.Address = fullAddress;
                    order.Phone = orderView.Phone;
                    order.ShippingFee = orderView.ShippingFee; // Lấy phí vận chuyển từ form
                    order.TotalAmountAfter = orderView.GrandTotal; // Lấy tổng chi phí từ form
                    order.ProvinceName = orderView.ProvinceName;
                    order.DistrictName = orderView.DistrictName;
                    order.WardName = orderView.WardName;
                    order.ShopId = orderView.ShopId;
                    order.ServiceTypeId = orderView.ServiceTypeId;
                    



                    // Xử lý điều hướng theo phương thức thanh toán
                    switch (orderView.PaymentMethod)
                    {
                        case "COD":
                            string code = await GHNService.GetTrackingCode(orderView.Name, orderView.Phone
                            , orderView.StreetAddress, orderView.WardName, orderView.DistrictName,
                                orderView.ProvinceName, orderView.ServiceTypeId, ConvertUsdToVnd(orderView.GrandTotal), cart);
                            order.OrderCode = code;
                            _orderService.AddOrder(order, cart);
                            _productService.reduceQuantity(cart);

                            HttpContext.Session.Remove("cart");

                            TempData["SuccessMessage"] = "Order successful!";


                            return RedirectToAction("CheckoutWithCOD", new { orderId = order.Id });

                        case "VNpay":
                            string code2 = await GHNService.GetTrackingCode(orderView.Name, orderView.Phone
                            , orderView.StreetAddress, orderView.WardName, orderView.DistrictName,
                                orderView.ProvinceName, orderView.ServiceTypeId, ConvertUsdToVnd(orderView.GrandTotal), cart);
                            order.OrderCode = code2;
                            _orderService.AddOrder(order, cart);
                            _productService.reduceQuantity(cart);

                            HttpContext.Session.Remove("cart");

                            //TempData["SuccessMessage"] = "Order successful!";
                            return RedirectToAction("CheckoutWithVNPay", new { orderId = order.Id });

                        case "PayOS":
                            return RedirectToAction("CheckoutWithPayos", new { orderId = order.Id });

                        default:
                            TempData["ErrorMessage"] = "Invalid payment method!";
                            return RedirectToAction("Checkout");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Order failed! Please check your information.";
                return RedirectToAction("Checkout");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckoutWithCOD(int orderId)
        {
            var order = _orderService.GetById(orderId);

            return View(order);
        }

        public double ConvertUsdToVnd(double codAmountUsd, double exchangeRate = 24000)
        {
            return (double)Math.Round(codAmountUsd * exchangeRate);
        }
        [HttpGet]
        public IActionResult CheckoutWithVNPay(int orderId)
        {
            var order = _orderService.GetById(orderId);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found!";
                return RedirectToAction("Index");
            }

            // Store the orderId in TempData
            TempData["CurrentOrderId"] = orderId;

            var paymentModel = new PaymentInformationModel
            {
                Amount = ConvertUsdToVnd(order.TotalAmountAfter),
                OrderDescription = $"Thanh toán đơn hàng #{order.Id}",
                Name = order.Name,
                OrderType = "billpayment"
            };

            string vnpayUrl = _vnPayService.CreatePaymentUrl(paymentModel, HttpContext);
            return Redirect(vnpayUrl);
        }
        // Trong phương thức xử lý thanh toán VNPay
        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            // First check if the response contains a cancellation or error code
            if (response != null && !string.IsNullOrEmpty(response.VnPayResponseCode))
            {
                // Check for known cancellation/error codes
                if (response.VnPayResponseCode == "24" || // User cancellation
                    response.VnPayResponseCode == "99" || // Common error
                    response.VnPayResponseCode != "00")   // "00" is typically success
                {
                    string errorMessage = "Giao dịch đã bị hủy hoặc thất bại.";
                    TempData["ErrorMessage"] = errorMessage;
                    return RedirectToAction("PaymentFailed");
                }
            }

            // If we get here, check if the payment was actually successful
            if (response != null && response.Success)
            {
                // Get orderId as you did before
                int orderId = _orderService.GetMaxOrderId();
                

                TempData["ProcessedOrderId"] = orderId;
                TempData["ErrorMessage"] = "Giao dịch không thành công.";
                return RedirectToAction("PaymentSuccess");
            }

            // If we get here, something else went wrong
            
            return RedirectToAction("PaymentFailed");
        }

        [HttpGet]
        public IActionResult PaymentSuccess()
        {
            // Không thay đổi logic của action này
            return View();
        }
        [HttpGet]
        public IActionResult PaymentFailed()
        {
            // Không thay đổi logic của action này
            return View();
        }
        /*[HttpGet]
        public IActionResult CheckoutWithPayos(int orderId)
        {
            var order = _orderService.GetById(orderId);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("Index", "Home");
            }

            var paymentModel = new PayOSInformationModel
            {
                Amount = order.TotalAmountAfter,
                OrderDescription = $"Thanh toan don hang #{order.Id}",
                Name = order.Name,
                OrderType = "billpayment",
                ExtraData = new Dictionary<string, string>
        {
            { "orderId", order.Id.ToString() }
        }
            };

            string payosUrl = _payosService.CreatePaymentUrl(paymentModel, HttpContext);
            if (string.IsNullOrEmpty(payosUrl))
            {
                TempData["ErrorMessage"] = "Không thể tạo liên kết thanh toán!";
                return RedirectToAction("Checkout");
            }

            return Redirect(payosUrl);
        }*/


        

        /*[HttpPost]
        public async Task<IActionResult> CreateShippingOrder(string from_name, string from_phone, string from_address, string to_name, string to_phone, string to_address, string to_ward_code, int to_district_id, int cod_amount, int service_id)
        {
            var orderJson = await _ghnService.CreateShippingOrderAsync(from_name, from_phone, from_address, to_name, to_phone, to_address, to_ward_code, to_district_id, cod_amount, service_id);
            if (orderJson["code"]?.ToObject<int>() == 200)
            {
                return Ok(new { message = "Order created successfully", order_code = orderJson["data"]?["order_code"] });
            }
            return BadRequest(new { message = "Failed to create order", error = orderJson["message"] });
        }*/




    }

}
