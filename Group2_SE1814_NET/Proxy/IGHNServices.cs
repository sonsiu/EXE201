using Group2_SE1814_NET.ViewModels;
using Newtonsoft.Json.Linq;

namespace Group2_SE1814_NET.Proxy {
    public interface IGHNService {
        Task<OrderGHNViewModel> GetOrderDetailByOrderCode(string orderCode);

        Task<List<OrderGHNViewModel>> GetAllOrders();

        Task<bool> UpdateOrderNoteById(string orderCode, string note);

        Task<bool> CancelOrderById(string orderCode);

        public Task<JObject> GetProvincesAsync();
        public Task<string> GetProvinceName(int provinceId);
        public Task<JObject> GetDistrictsAsync(int provinceId);
        public Task<JObject> GetWardsAsync(int districtId);
        public Task<JObject> GetShopsAsync();
        public Task<JObject> GetAvailableServicesAsync(int shopId, int fromDistrictId, int toDistrictId);
        public Task<JObject> GetShippingFeeAsync(int shopId, int fromDistrictId, string toWardCode, int toDistrictId, int serviceId);


        Task<bool> UpdateOrderStatusById(string orderCode, string status);
    }
}
