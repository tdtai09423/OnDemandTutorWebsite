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
    [JsonIgnore]
    public virtual Learner? Learner { get; set; }
    [JsonIgnore]
    public virtual Tutor? Tutor { get; set; }

    public void NavigateAccount()
    {
        if(this.RoleId == "TUTOR")
        {
            Learner = null;
            Tutor = new Tutor();
            Tutor.TutorId = this.Id;
        }
        if(this.RoleId == "LEARNER")
        {
            Tutor = null;
            Learner = new Learner();
            Learner.LearnerId = this.Id;
        }
    }
}
