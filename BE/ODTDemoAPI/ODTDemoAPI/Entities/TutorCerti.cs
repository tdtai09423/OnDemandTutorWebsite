using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class TutorCerti
{
    public int? TutorId { get; set; }

    public string TutorCertificate { get; set; } = null!;
    [JsonIgnore]
    public virtual Tutor? Tutor { get; set; }
}
