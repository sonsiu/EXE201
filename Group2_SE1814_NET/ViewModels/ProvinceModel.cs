namespace Group2_SE1814_NET.ViewModels {
    public class ProvinceModel {
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
    }

    public class DistrictModel {
        public int DistrictID { get; set; }
        public int ProvinceID { get; set; }
        public string DistrictName { get; set; }
    }

    public class ShippingFeeModel {
        public int total { get; set; }
        public int service_fee { get; set; }
    }
    public class WardModel {
        public int WardCode { get; set; }
        public string WardName { get; set; }
    }
    public class ShopModel {
        public int _id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ward_code { get; set; }
        public int district_id { get; set; }
        public int client_id { get; set; }
    }

    public class ServiceModel {
        public int service_id { get; set; }
        public string short_name { get; set; }
        public int service_type_id { get; set; }
    }
    public class ProvinceResponse {
        public List<ProvinceModel> Data { get; set; }
    }
}