using System;
using System.Collections.Generic;

namespace ODTDemoAPI.Entities;

public partial class ReviewRating
{
    public int? TutorId { get; set; }

    public int? LearnerId { get; set; }

    public string? Review { get; set; }

    public int? Rating { get; set; }

    public virtual Learner? Learner { get; set; }

    public virtual Tutor? Tutor { get; set; }
}
