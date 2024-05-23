using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class Learner
{
    public int LearnerId { get; set; }

    public string LearnerName { get; set; } = null!;

    public int LearnerAge { get; set; }

    public string LearnerEmail { get; set; } = null!;

    public string? MembershipId { get; set; }

    public virtual Account LearnerNavigation { get; set; } = null!;

    public virtual ICollection<LearnerOrder> LearnerOrders { get; set; } = new List<LearnerOrder>();

    public virtual Membership? Membership { get; set; }
}
