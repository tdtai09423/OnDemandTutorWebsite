using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Section
{
    public int SectionId { get; set; }

    public DateTime SectionStart { get; set; }

    public DateTime SectionEnd { get; set; }

    public string SectionStatus { get; set; } = null!;

    public int Price { get; set; }

    public int? CurriculumId { get; set; }
    [JsonIgnore]
    public virtual Curriculum? Curriculum { get; set; }
}
