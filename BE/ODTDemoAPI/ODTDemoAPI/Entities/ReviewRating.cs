using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class ReviewRating
{
    public int? TutorId { get; set; }

    public int? LearnerId { get; set; }

    public string? Review { get; set; }

    public int? Rating { get; set; }

    public DateTime ReviewDate { get; set; }

    [JsonIgnore]
    public virtual Learner? Learner { get; set; }

    [JsonIgnore]
    public virtual Tutor? Tutor { get; set; }
}
