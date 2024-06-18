using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Curriculum
{
    public int CurriculumId { get; set; }

    public string? CurriculumType { get; set; }

    public int TotalSlot { get; set; }

    public string CurriculumStatus { get; set; } = null!;

    public string? CurriculumDescription { get; set; }

    public decimal PricePerSection { get; set; }

    public int? TutorId { get; set; }
    [JsonIgnore]
    public virtual ICollection<LearnerOrder> LearnerOrders { get; set; } = new List<LearnerOrder>();
    [JsonIgnore]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual Tutor? Tutor { get; set; }
}
