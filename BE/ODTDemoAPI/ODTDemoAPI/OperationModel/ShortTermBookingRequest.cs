namespace ODTDemoAPI.OperationModel
{
    public class ShortTermBookingRequest : BookingRequest
    {

        public static string CurriculumType { get; set; } = "ShortTerm";

        public DateTime startTime { get; set; }

        public int Duration { get; set; }
    }
}
