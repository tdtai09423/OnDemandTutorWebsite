using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class LearnerFavourite
{
    public int FavoId { get; set; }

    public int LearnerId { get; set; }

    public int TutorId { get; set; }
    [JsonIgnore]
    public virtual Learner Learner { get; set; } = null!;
    [JsonIgnore]
    public virtual Tutor Tutor { get; set; } = null!;
}
