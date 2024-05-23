using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class Membership
{
    public string MembershipId { get; set; } = null!;

    public string MembershipLevel { get; set; } = null!;

    public string MembershipDescription { get; set; } = null!;

    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
