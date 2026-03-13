namespace Display_Supplier_Delivery.Models
{
    public class PartDetailViewModel
    {
        public string PartNo { get; set; }
        public int PlanPcs { get; set; } // Sum of QtyPlanReceive
        public int PlanBox { get; set; } // Count of Kanban rows
        public int ActualPcs { get; set; }
        public int ActualBox { get; set; }
        public int DiffPcs { get; set; }
        public int DiffBox { get; set; }
    }
}
