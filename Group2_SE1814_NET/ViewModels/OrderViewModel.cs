using System.ComponentModel.DataAnnotations;

namespace Group2_SE1814_NET.ViewModels
{
    public class OrderViewModel
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số nhà và tên đường")]
        public string StreetAddress { get; set; } // Số nhà + tên đường
        public string Phone { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; } // Tên tỉnh/thành phố
        public int DistrictId { get; set; }
        public string DistrictName { get; set; } // Tên quận/huyện
        public string WardId { get; set; }
        public string WardName { get; set; } // Tên phường/xã
        public string ShopId { get; set; }
        public int ServiceId { get; set; }
        public decimal ShippingFee { get; set; }
        public double GrandTotal { get; set; }

        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string PaymentMethod { get; set; } // Phương thức thanh toán: PayOs, VNpay, COD
    }
}
