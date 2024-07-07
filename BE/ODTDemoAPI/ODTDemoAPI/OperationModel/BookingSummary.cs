using ODTDemoAPI.Entities;

namespace ODTDemoAPI.OperationModel
{
    public class BookingSummary
    {
        public int TotalBookings { get; set; }
        public decimal TotalAmount { get; set; }
        public List<LearnerOrder> Orders { get; set; }
    }
}
