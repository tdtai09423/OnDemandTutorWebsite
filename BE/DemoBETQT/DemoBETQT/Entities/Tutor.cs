using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class Tutor
{
    public int TutorId { get; set; }

    public string TutorName { get; set; } = null!;

    public int TutorAge { get; set; }

    public string TutorEmail { get; set; } = null!;

    public string TutorDescription { get; set; } = null!;

    public string? MajorId { get; set; }

    public string TutorStatus { get; set; } = null!;

    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();

    public virtual Major? Major { get; set; }

    public virtual Account TutorNavigation { get; set; } = null!;
}
