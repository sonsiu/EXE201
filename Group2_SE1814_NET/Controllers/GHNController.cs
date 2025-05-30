using Group2_SE1814_NET.Proxy;
using Group2_SE1814_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Group2_SE1814_NET.Controllers
{
    public class GHNController : Controller
    {
        private readonly GHNService _ghnService;


        public GHNController(GHNService ghnService)
        {
            _ghnService = ghnService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProvinces()
        {
            var provincesJson = await _ghnService.GetProvincesAsync();
            var provinces = provincesJson["data"]?.ToObject<List<ProvinceModel>>() ?? new List<ProvinceModel>();
            return new JsonResult(provinces);
        }

        // Lấy danh sách quận/huyện theo ProvinceID
        [HttpGet]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            if (provinceId == 0)
            {
                return BadRequest("Province ID is required.");
            }

            var districtsJson = await _ghnService.GetDistrictsAsync(provinceId);
            var districts = districtsJson["data"]?.ToObject<List<DistrictModel>>() ?? new List<DistrictModel>();
            return new JsonResult(districts);
        }

        // Lấy danh sách phường/xã theo DistrictID
        [HttpGet]
        public async Task<IActionResult> GetWards(int districtId)
        {
            if (districtId == 0)
            {
                return BadRequest("District ID is required.");
            }

            var wardsJson = await _ghnService.GetWardsAsync(districtId);
            var wards = wardsJson["data"]?.ToObject<List<WardModel>>() ?? new List<WardModel>();
            return new JsonResult(wards);
        }

        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            var shopsJson = await _ghnService.GetShopsAsync();
            var shops = shopsJson["data"]?["shops"]?.ToObject<List<ShopModel>>() ?? new List<ShopModel>();
            return new JsonResult(shops);
        }


        [HttpGet]
        public async Task<IActionResult> GetAvailableServices(int shopId, int fromDistrictId, int toDistrictId)
        {
            if (shopId == 0 || fromDistrictId == 0 || toDistrictId == 0)
            {
                return BadRequest("Shop ID, From District ID, and To District ID are required.");
            }

            var servicesJson = await _ghnService.GetAvailableServicesAsync(shopId, fromDistrictId, toDistrictId);
            var services = servicesJson["data"]?.ToObject<List<ServiceModel>>() ?? new List<ServiceModel>();
            return new JsonResult(services);
        }

        // Lấy phí vận chuyển
        [HttpGet]
        public async Task<IActionResult> GetShippingFee(int shopId, int fromDistrictId, string toWardCode, int toDistrictId, int serviceId)
        {
            if (shopId == 0 || fromDistrictId == 0 || toDistrictId == 0)
            {
                return BadRequest("Shop ID, From District ID, To District ID, and To Ward Code are required.");
            }

            var feeJson = await _ghnService.GetShippingFeeAsync(shopId, fromDistrictId, toWardCode, toDistrictId, serviceId);
            var feeData = feeJson["data"]?.ToObject<ShippingFeeModel>() ?? new ShippingFeeModel();

            return new JsonResult(feeData);
        }
    }
}
