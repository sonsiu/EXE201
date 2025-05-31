namespace Group2_SE1814_NET.ViewModels {
    public class PayOSWebhookRequest {
        public int OrderCode { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Signature { get; set; }
    }

}
