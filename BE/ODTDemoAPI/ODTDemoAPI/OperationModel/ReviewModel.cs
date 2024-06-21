using ODTDemoAPI.Entities;

namespace ODTDemoAPI.OperationModel
{
    public class ReviewModel
    {
        public int TutorId { get; set; }

        public int LearnerId { get; set; }

        public int Rating { get; set; }

        public string Review { get; set; } = null!;
    }
}
