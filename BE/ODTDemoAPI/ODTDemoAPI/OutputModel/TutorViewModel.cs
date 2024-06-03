using ODTDemoAPI.Entities;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.OutputModel
{
    public class TutorViewModel
    {
        public int TutorId { get; set; }

        public int TutorAge { get; set; }

        public string TutorEmail { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public string TutorDescription { get; set; } = null!;

        public string TutorPicture { get; set; } = null!;

        public CertiStatus CertiStatus { get; set; } = CertiStatus.Pending;

        public string? MajorId { get; set; }
        [JsonIgnore]
        public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();
        [JsonIgnore]
        public virtual Major? Major { get; set; }

        public virtual Account TutorNavigation { get; set; } = null!;

        public double AverageRating { get; set; } = 0;

        public string RatingReport { get; set; } = null!;

        public double MinPrice { get; set; } = 0;

        public double MaxPrice { get; set; }

        public int NumOfReviews { get; set; } = 0;
    }
}
