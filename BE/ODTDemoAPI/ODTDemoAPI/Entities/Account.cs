using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public bool Status { get; set; }

    public bool IsEmailVerified { get; set; }

    public DateTime CreatedDate { get; set; }
    [JsonIgnore]
    public virtual Learner? Learner { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [JsonIgnore]
    public virtual Tutor? Tutor { get; set; }
    [JsonIgnore]
    public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();

    public virtual Wallet? Wallet { get; set; }

    public void NavigateAccount(string roleId)
    {
        if (roleId == "TUTOR")
        {
            this.Learner = null;
            this.Tutor = new Tutor();
            Tutor.TutorId = this.Id;
        }
        if (roleId == "LEARNER")
        {
            this.Tutor = null;
            this.Learner = new Learner();
            this.Learner.LearnerId = this.Id;
        }
    }
}