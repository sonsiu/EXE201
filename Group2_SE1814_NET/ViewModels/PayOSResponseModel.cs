namespace Group2_SE1814_NET.ViewModels {
    public class PayOSResponseModel {
        public bool Success { get; set; }
        public string PaymentUrl { get; set; }
        public int OrderId { get; set; }
        public string PayosResponseCode { get; set; }
        public string TransactionId { get; set; }
        public Dictionary<string, string> ResponseData { get; set; }
    }
}
