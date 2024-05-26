using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class LearnerOrder
{
    public int OrderId { get; set; }

    public string OrderType { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public int Total { get; set; }

    public int? CurriculumId { get; set; }

    public int? LearnerId { get; set; }
    [JsonIgnore]
    public virtual Curriculum? Curriculum { get; set; }
    [JsonIgnore]
    public virtual Learner? Learner { get; set; }
}
