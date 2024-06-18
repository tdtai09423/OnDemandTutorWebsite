namespace ODTDemoAPI.OperationModel
{
    public class LongTermBookingRequest : BookingRequest
    {
        public static string CurriculumType { get; set; } = "LongTerm";

        public TimeSpan startTime { get; set; }

        public DayOfWeek Day1 { get; set; }

        public DayOfWeek Day2 { get; set; }

        public int Duration { get; set; }
    }
}
