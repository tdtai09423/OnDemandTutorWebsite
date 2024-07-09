using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class ReviewRating
{
    public int ReviewId { get; set; }

    public int TutorId { get; set; }

    public int LearnerId { get; set; }

    public string? Review { get; set; }

    public int? Rating { get; set; }

    public DateTime ReviewDate { get; set; }

    public int OrderId { get; set; }
    [JsonIgnore]
    public virtual Learner Learner { get; set; } = null!;
    [JsonIgnore]
    public virtual Tutor Tutor { get; set; } = null!;
    [JsonIgnore]
    public virtual LearnerOrder Order { get; set; } = null!;
}
