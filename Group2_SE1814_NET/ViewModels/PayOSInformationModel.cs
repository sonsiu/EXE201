namespace Group2_SE1814_NET.ViewModels {
    public class PayOSInformationModel {
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
        public string OrderType { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public Dictionary<string, string> ExtraData { get; set; }
    }
}
