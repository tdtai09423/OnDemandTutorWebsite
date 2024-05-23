using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public bool AccountStatus { get; set; }

    public virtual Learner? Learner { get; set; }

    public virtual Tutor? Tutor { get; set; }
}
