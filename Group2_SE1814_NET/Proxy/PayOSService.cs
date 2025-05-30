using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using Group2_SE1814_NET.ViewModels;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Group2_SE1814_NET.Proxy
{
    public class PayosService : IPayosService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayosService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreatePaymentUrl(PayOSInformationModel model, HttpContext context)
        {
            var clientId = _configuration["PayOS:ClientId"];
            var apiKey = _configuration["PayOS:ApiKey"];
            var secretKey = _configuration["PayOS:SecretKey"];
            var apiEndpoint = _configuration["PayOS:ApiEndpoint"];

            // Tạo các tham số thanh toán
            var requestId = DateTime.UtcNow.Ticks.ToString();
            var orderInfo = model.OrderDescription;
            var amount = (int)model.Amount;

            // Tạo URL callback
            var returnUrl = _configuration["PayOS:ReturnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
            {
                var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
                returnUrl = $"{baseUrl}/Payment/PaymentCallback";
            }

            // Tạo dữ liệu cho API PayOS
            var requestData = new
            {
                clientId = clientId,
                apiKey = apiKey,
                orderId = requestId,
                amount = amount,
                description = orderInfo,
                returnUrl = returnUrl,
                cancelUrl = model.CancelUrl ?? returnUrl,
                extraData = model.ExtraData ?? new Dictionary<string, string>()
            };

            // Tạo chữ ký
            var jsonData = JsonConvert.SerializeObject(requestData);
            var signature = CreateSignature(jsonData, secretKey);

            // Gửi request đến PayOS API
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Signature", signature);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = client.PostAsync($"{apiEndpoint}/create-payment-link", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    var paymentResponse = JsonConvert.DeserializeObject<dynamic>(responseData);
                    return paymentResponse.paymentUrl;
                }
            }

            return string.Empty;
        }

        public PayOSResponseModel PaymentExecute(IQueryCollection queryCollection)
        {
            var response = new PayOSResponseModel();

            try
            {
                var secretKey = _configuration["PayOS:SecretKey"];

                // Lấy dữ liệu từ query string
                var responseData = new Dictionary<string, string>();
                foreach (var key in queryCollection.Keys)
                {
                    responseData.Add(key, queryCollection[key]);
                }

                // Kiểm tra chữ ký từ PayOS
                if (ValidateSignature(responseData, secretKey))
                {
                    // Xử lý dữ liệu response
                    response.Success = responseData["status"] == "00";
                    response.PayosResponseCode = responseData["status"];
                    response.TransactionId = responseData["transactionId"];
                    response.ResponseData = responseData;

                    // Lấy orderId nếu có
                    if (responseData.ContainsKey("orderId") && !string.IsNullOrEmpty(responseData["orderId"]))
                    {
                        int.TryParse(responseData["orderId"], out int orderId);
                        response.OrderId = orderId;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
            }

            return response;
        }

        private string CreateSignature(string data, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }

        private bool ValidateSignature(Dictionary<string, string> responseData, string secretKey)
        {
            // Tạo chuỗi để kiểm tra chữ ký
            var receivedSignature = responseData["signature"];
            responseData.Remove("signature");

            // Sắp xếp dữ liệu theo key
            var sortedData = responseData.OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            var json = JsonConvert.SerializeObject(sortedData);
            var calculatedSignature = CreateSignature(json, secretKey);

            return receivedSignature == calculatedSignature;
        }
    }
}