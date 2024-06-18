namespace ODTDemoAPI.OperationModel
{
    public class BookingRequest
    {
        public int TutorId { get; set; }

        public int LearnerId { get; set; }

        public string CurriculumDescription { get; set; } = null!;
    }
}
