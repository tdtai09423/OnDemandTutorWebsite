using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class Curriculum
{
    public int CurriculumId { get; set; }

    public string CurriculumType { get; set; } = null!;

    public int TotalSlot { get; set; }

    public string CurriculumStatus { get; set; } = null!;

    public string? CurriculumDesription { get; set; }

    public int? TutorId { get; set; }

    public virtual ICollection<LearnerOrder> LearnerOrders { get; set; } = new List<LearnerOrder>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual Tutor? Tutor { get; set; }
}
