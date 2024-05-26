using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Membership
{
    public string MembershipId { get; set; } = null!;

    public string MembershipLevel { get; set; } = null!;

    public string MembershipDescription { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Learner> Learners { get; set; } = new List<Learner>();
}
