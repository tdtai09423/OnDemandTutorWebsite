namespace ODTDemoAPI.OperationModel
{
    public class BookingRequest
    {
        public int TutorId { get; set; }

        public int LearnerId { get; set; }

        public string CurriculumType { get; set; } = null!;

        public string CurriculumDescription { get; set; } = null!;

        public DateTime startTime { get; set; }

        public int Duration { get; set; }
    }
}
