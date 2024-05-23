using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class Section
{
    public int SectionId { get; set; }

    public DateTime SectionStart { get; set; }

    public DateTime SectionEnd { get; set; }

    public string SectionStatus { get; set; } = null!;

    public int Price { get; set; }

    public int? CurriculumId { get; set; }

    public virtual Curriculum? Curriculum { get; set; }
}
