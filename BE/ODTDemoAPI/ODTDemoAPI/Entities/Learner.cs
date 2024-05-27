using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Learner
{
    public int LearnerId { get; set; }

    public int LearnerAge { get; set; }

    public string LearnerEmail { get; set; } = null!;

    public string LearnerPicture { get; set; } = null!;

    public string? MembershipId { get; set; }

    public virtual Account LearnerNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<LearnerOrder> LearnerOrders { get; set; } = new List<LearnerOrder>();

    public virtual Membership? Membership { get; set; }
}
