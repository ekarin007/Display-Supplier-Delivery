namespace Display_Supplier_Delivery.Models
{
    public class ReceivingItemVM
    {
        public int ItemNo { get; set; }
        public string Supplier { get; set; }
        public string Time { get; set; }
        public int VolumePN { get; set; }
        public int VolumePC { get; set; }
        public string Status { get; set; }   // ON PLAN / Delay
        public string Remark { get; set; }
    }
}
