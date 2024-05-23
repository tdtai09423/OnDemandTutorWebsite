using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class LearnerOrder
{
    public int OrderId { get; set; }

    public string OrderType { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public int Total { get; set; }

    public int? CurriculumId { get; set; }

    public int? LearnerId { get; set; }

    public virtual Curriculum? Curriculum { get; set; }

    public virtual Learner? Learner { get; set; }
}
