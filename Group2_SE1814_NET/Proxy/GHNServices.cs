using System.Net;
using System.Text;
using System.Text.Json;
using Group2_SE1814_NET.ViewModels;
using Newtonsoft.Json.Linq;


namespace Group2_SE1814_NET.Proxy {
    public class GHNService : IGHNService {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly string _token;
        private readonly int _shopId;
        private readonly string _baseUrl;

        //public GHNService(HttpClient client)
        //{
        //    _client = client;
        //    _client.DefaultRequestHeaders.Add("token", "fa19bc40-fc2a-11ef-82e7-a688a46b55a3");
        //}

        public GHNService(HttpClient client, IConfiguration config) {
            _client = client;
            _config = config;
            _token = config["GHN:ApiToken"];
            _shopId = int.Parse(config["GHN:ShopId"]);
            _baseUrl = config["GHN:BaseUrl"];
            _client.DefaultRequestHeaders.Add("token", _config["GHN:ApiToken"]);
        }

        public async Task<List<OrderGHNViewModel>> GetAllOrders() {
            List<OrderGHNViewModel> results = new List<OrderGHNViewModel>();
            HttpResponseMessage response = await _client.GetAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/search");
            if (response.StatusCode == HttpStatusCode.OK) {
                string result = await response.Content.ReadAsStringAsync();
                JsonElement doc = JsonDocument.Parse(result).RootElement;
                JsonElement data = doc.GetProperty("data");
                JsonElement orderList = data.GetProperty("data");
                foreach (JsonElement item in orderList.EnumerateArray()) {
                    string createDate = item.GetProperty("created_date").GetString() ?? "";
                    DateTime createdDate = DateTime.Parse(createDate);
                    string toDate = item.GetProperty("finish_date").GetString() ?? "";
                    DateTime finishDate = DateTime.Parse(createDate);
                    results.Add(new OrderGHNViewModel
                    {
                        OrderCode = item.TryGetProperty("order_code", out var orderCodeProp) && orderCodeProp.ValueKind != JsonValueKind.Undefined
                            ? orderCodeProp.GetString() ?? "" : "",
                        CreateDate = createdDate,
                        FinishDate = finishDate,
                        ToName = item.TryGetProperty("to_name", out var toNameProp) && toNameProp.ValueKind != JsonValueKind.Undefined
                            ? toNameProp.GetString() ?? "" : "",
                        ToPhone = item.TryGetProperty("to_phone", out var toPhoneProp) && toPhoneProp.ValueKind != JsonValueKind.Undefined
                            ? toPhoneProp.GetString() ?? "" : "",
                        ToAddress = item.TryGetProperty("to_address", out var toAddressProp) && toAddressProp.ValueKind != JsonValueKind.Undefined
                            ? toAddressProp.GetString() ?? "" : "",
                        Status = item.TryGetProperty("status", out var statusProp) && statusProp.ValueKind != JsonValueKind.Undefined
                            ? statusProp.ToString() : "",
                        CodAmount = item.TryGetProperty("cod_amount", out var codAmountProp) && codAmountProp.ValueKind != JsonValueKind.Undefined
                            ? codAmountProp.GetDecimal() : 0,
                        RequiredNote = item.TryGetProperty("required_note", out var requiredNoteProp) && requiredNoteProp.ValueKind != JsonValueKind.Undefined
                            ? requiredNoteProp.GetString() ?? "" : "",

                        ProductGHNs = item.TryGetProperty("items", out var itemsProp) && itemsProp.ValueKind != JsonValueKind.Undefined
                            ? GetProducts(itemsProp) ?? new List<ProductGHNViewModel>()
                            : new List<ProductGHNViewModel>()
                    });
                }
            }

            return results;
        }


        public async Task<OrderGHNViewModel> GetOrderDetailByOrderCode(string orderCode) {
            var requestData = new StringContent(JsonSerializer.Serialize(new
            {
                order_code = orderCode
            }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/detail", requestData);
            if (response.StatusCode == HttpStatusCode.OK) {
                string result = await response.Content.ReadAsStringAsync();
                JsonElement doc = JsonDocument.Parse(result).RootElement;
                JsonElement data = doc.GetProperty("data");
                string createDate = data.GetProperty("created_date").GetString() ?? "";
                DateTime createdDate = DateTime.Parse(createDate);
                string toDate = data.GetProperty("finish_date").GetString() ?? "";
                DateTime finishDate = DateTime.Parse(createDate);
                return new OrderGHNViewModel
                {
                    OrderCode = data.GetProperty("order_code").GetString() ?? "",
                    CreateDate = createdDate,
                    FinishDate = finishDate,
                    ToName = data.GetProperty("to_name").GetString() ?? "",
                    ToPhone = data.GetProperty("to_phone").GetString() ?? "",
                    ToAddress = data.GetProperty("to_address").GetString() ?? "",
                    Status = data.GetProperty("status").ToString(),
                    CodAmount = data.GetProperty("cod_amount").GetDecimal(),
                    RequiredNote = data.GetProperty("required_note").GetString() ?? "",
                    Note = data.GetProperty("note").GetString() ?? "",
                    ProductGHNs = GetProducts(data.GetProperty("items")) ?? new List<ProductGHNViewModel>()
                };
            }
            return new OrderGHNViewModel();
        }

        private List<ProductGHNViewModel> GetProducts(JsonElement data) {
            List<ProductGHNViewModel> results = new List<ProductGHNViewModel>();
            foreach (JsonElement item in data.EnumerateArray()) {
                results.Add(new ProductGHNViewModel
                {
                    Name = item.TryGetProperty("name", out var nameProp) && nameProp.ValueKind != JsonValueKind.Undefined
                        ? nameProp.GetString() ?? ""
                        : "",
                    Quantity = item.TryGetProperty("quantity", out var quantityProp) && quantityProp.ValueKind != JsonValueKind.Undefined
                        ? quantityProp.GetInt32()
                        : 0,
                    Code = item.TryGetProperty("code", out var codeProp) && codeProp.ValueKind != JsonValueKind.Undefined
                        ? codeProp.GetString() ?? ""
                        : "",
                    Weight = item.TryGetProperty("weight", out var weightProp) && weightProp.ValueKind != JsonValueKind.Undefined
                        ? weightProp.GetInt32()
                        : 0
                });
            }
            return results;
        }

        public async Task<bool> UpdateOrderNoteById(string orderCode, string note) {
            var requestData = new StringContent(JsonSerializer.Serialize(new
            {
                note = note,
                order_code = orderCode

            }), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/update", requestData);
            if (response.StatusCode == HttpStatusCode.OK) {
                string result = await response.Content.ReadAsStringAsync();
                JsonElement doc = JsonDocument.Parse(result).RootElement;
                return doc.GetProperty("code").GetString() == "200";
            }
            return false;
        }

        public async Task<bool> CancelOrderById(string orderCode) {
            var requestData = new StringContent(JsonSerializer.Serialize(new
            {
                order_codes = new string[] { orderCode }
            }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/switch-status/cancel", requestData);
            if (response.StatusCode == HttpStatusCode.OK) {

                string result = await response.Content.ReadAsStringAsync();
                JsonElement doc = JsonDocument.Parse(result).RootElement;
                return doc.GetProperty("code").GetInt32() == 200;
            }
            return false;
        }

        //Changed
        public async Task<bool> UpdateOrderStatusById(string orderCode, string status) {
            if (status == "cancel") {
                return await CancelOrderById(orderCode);
            }
            var requestData = new StringContent(JsonSerializer.Serialize(new
            {
                status = status,
                order_code = orderCode

            }), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/update", requestData);
            if (response.StatusCode == HttpStatusCode.OK) {
                string result = await response.Content.ReadAsStringAsync();
                JsonElement doc = JsonDocument.Parse(result).RootElement;
                return doc.GetProperty("code").GetString() == "200";
            }
            return false;
        }

        //shipping fee

        // Lấy danh sách tỉnh/thành phố
        public async Task<JObject> GetProvincesAsync() {
            try {
                var response = await _client.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province");
                response.EnsureSuccessStatusCode();//200
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            } catch (Exception ex) {
                Console.WriteLine($"Error getting provinces: {ex.Message}");
                return new JObject(); // Trả về object rỗng nếu lỗi
            }
        }
        public async Task<string> GetProvinceName(int provinceId) {
            try {
                var response = await _client.GetAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province");
                if (response.IsSuccessStatusCode) {
                    var provinces = await response.Content.ReadFromJsonAsync<ProvinceResponse>();
                    return provinces?.Data.FirstOrDefault(p => p.ProvinceID == provinceId)?.ProvinceName ?? "Unknown Province";
                }
                return "Unknown Province";
            } catch (Exception ex) {
                // Log lỗi nếu cần
                Console.WriteLine($"Error fetching province name: {ex.Message}");
                return "Unknown Province";
            }
        }
        // Lấy danh sách quận/huyện theo ProvinceID
        public async Task<JObject> GetDistrictsAsync(int provinceId) {
            try {
                var response = await _client.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id={provinceId}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            } catch (Exception ex) {
                Console.WriteLine($"Error getting districts: {ex.Message}");
                return new JObject(); // Trả về object rỗng nếu lỗi
            }
        }

        // Lấy danh sách phường/xã theo DistrictID
        public async Task<JObject> GetWardsAsync(int districtId) {
            try {
                var response = await _client.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id={districtId}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            } catch (Exception ex) {
                Console.WriteLine($"Error getting wards: {ex.Message}");
                return new JObject(); // Trả về object rỗng nếu lỗi
            }
        }

        public async Task<JObject> GetShopsAsync() {
            try {
                var response = await _client.GetAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shop/all");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            } catch (Exception ex) {
                Console.WriteLine($"Error getting shops: {ex.Message}");
                return new JObject(); // Trả về object rỗng nếu lỗi
            }
        }

        public async Task<JObject> GetAvailableServicesAsync(int shopId, int fromDistrictId, int toDistrictId) {
            try {
                var requestData = new
                {
                    shop_id = shopId,
                    from_district = fromDistrictId,
                    to_district = toDistrictId
                };

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services", content);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            } catch (Exception ex) {
                Console.WriteLine($"Error getting available services: {ex.Message}");
                return new JObject();
            }
        }

        public async Task<JObject> GetShippingFeeAsync(int shopId, int fromDistrictId, string toWardCode, int toDistrictId, int serviceId) {
            try {
                // Fake data cho weight và items
                var requestData = new
                {
                    shop_id = shopId,
                    from_district_id = fromDistrictId,
                    to_district_id = toDistrictId,
                    service_id = serviceId, // Fake service_type_id (Tiết kiệm = 2, Nhanh = 1)
                    to_ward_code = toWardCode,
                    weight = 200, // Fake weight: 200 gram
                    items = new[]
                    {
                        new
                        {
                            name = "Product 1",
                            quantity = 200,
                            height = 200,
                            weight = 200,
                            length = 200,
                            width = 200
                        }

                    },
                };

                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee", content);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonString);
            } catch (Exception ex) {
                Console.WriteLine($"Error getting shipping fee: {ex.Message}");
                return new JObject();
            }
        }
        private const string ApiUrl = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create";
        private const string ShopId = "196112";
        private const string Token = "fa19bc40-fc2a-11ef-82e7-a688a46b55a3";

        public static async Task<string> CreateOrder(string ito_name, string ito_phone, string ito_address,
            string ito_ward_name, string ito_district_name, string ito_province_name, int iservice_type_id, double icod_amount, List<Item> items) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("ShopId", ShopId);
                client.DefaultRequestHeaders.Add("Token", Token);


                var orderData = new
                {
                    payment_type_id = 2,
                    required_note = "KHONGCHOXEMHANG",
                    from_name = "TinTest",
                    from_phone = "0987654321",
                    from_address = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
                    from_ward_name = "Phường 14",
                    from_district_name = "Quận 10",
                    from_province_name = "HCM",
                    to_name = ito_name,
                    to_phone = ito_phone,
                    to_address = ito_address,
                    to_ward_name = ito_ward_name,
                    to_district_name = ito_district_name,
                    to_province_name = ito_province_name,
                    cod_amount = icod_amount,
                    cod_failed_amount = 2000,
                    weight = 1200,
                    length = 12,
                    width = 12,
                    height = 12,
                    service_type_id = iservice_type_id,
                    items = items.Select(i => new
                    {
                        name = i.Product.Name,
                        code = i.Product.Id.ToString(),
                        quantity = i.Quantity,
                        price = i.Product.Price * 24000,
                        length = 12,
                        width = 12,
                        height = 12,
                        weight = 1200,
                        category = new { level1 = i.Product.CategoryId.ToString() }
                    }).ToArray() // ✅ Chuyển danh sách item thành array
                };



                // Serialize với System.Text.Json
                var jsonContent = new StringContent(JsonSerializer.Serialize(orderData), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(ApiUrl, jsonContent);
                return await response.Content.ReadAsStringAsync();
            }
        }
        public static async Task<string> GetTrackingCode(string ito_name, string ito_phone, string ito_address,
    string ito_ward_name, string ito_district_name, string ito_province_name, int iservice_type_id, double icod_amount, List<Item> items) {
            // First, call the CreateOrder method to get the response
            string responseJson = await CreateOrder(ito_name, ito_phone, ito_address,
                ito_ward_name, ito_district_name, ito_province_name, iservice_type_id, icod_amount, items);

            // Parse the response JSON
            using JsonDocument doc = JsonDocument.Parse(responseJson);
            JsonElement root = doc.RootElement;

            // Check if the API call was successful
            if (root.TryGetProperty("code", out JsonElement codeElement) && codeElement.GetInt32() == 200) {
                // Extract the order code (tracking number)
                if (root.TryGetProperty("data", out JsonElement dataElement) &&
                    dataElement.TryGetProperty("order_code", out JsonElement orderCodeElement)) {
                    return orderCodeElement.GetString();
                }
            }

            // If unsuccessful or tracking code not found
            return null;
        }






    }
}
