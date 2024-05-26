using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Major
{
    public string MajorId { get; set; } = null!;

    public string MajorName { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>(); 
}
