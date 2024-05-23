using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class Major
{
    public string MajorId { get; set; } = null!;

    public string MajorName { get; set; } = null!;

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
