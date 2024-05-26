using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public enum CertiStatus
{
    Pending,
    Approved,
    Rejected
}
public partial class Tutor
{
    public int TutorId { get; set; }

    public int TutorAge { get; set; }

    public string TutorEmail { get; set; } = null!;

    public string TutorDescription { get; set; } = null!;

    public string? MajorId { get; set; }

    public CertiStatus CertiStatus { get; set; } = CertiStatus.Pending;
    [JsonIgnore]
    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();
    [JsonIgnore]
    public virtual Major? Major { get; set; }
 
    public virtual Account TutorNavigation { get; set; } = null!;

    //public IFormFile Image { get; set; }
}
