using System;
using System.Collections.Generic;

namespace DemoBETQT.Entities;

public partial class TutorCerti
{
    public int? TutorId { get; set; }

    public string TutorCertificate { get; set; } = null!;

    public virtual Tutor? Tutor { get; set; }
}
