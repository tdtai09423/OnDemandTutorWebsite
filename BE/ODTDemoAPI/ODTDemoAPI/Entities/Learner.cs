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

    public DateTime? MembershipCreatedDate { get; set; }

    public virtual Account LearnerNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<LearnerOrder> LearnerOrders { get; set; } = new List<LearnerOrder>();
    [JsonIgnore]
    public virtual ICollection<ChatBox> ChatBoxes { get; set; } = new List<ChatBox>();

    public virtual Membership? Membership { get; set; }
    [JsonIgnore]
    public virtual ICollection<ReviewRating> ReviewRatings { get; set; } = new List<ReviewRating>();

    public void CheckAndUpdateMembership()
    {
        if(Membership != null)
        {
            var date = (DateTime)MembershipCreatedDate!;
            DateTime membershipEndDate = date.AddDays(Membership.DurationInDays);
            if(membershipEndDate < DateTime.Now)
            {
                Membership = null;
                MembershipId = null;
            }
        }
    }
}
