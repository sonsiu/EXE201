namespace Group2_SE1814_NET.ViewModels
{
    public class ProductGHNViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Status { get; set; }
        public string ItemOrderCode
        {
            get; set;
        }
    }
}
