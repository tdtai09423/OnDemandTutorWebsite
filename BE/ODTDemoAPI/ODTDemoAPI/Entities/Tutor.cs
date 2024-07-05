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

    public string Nationality { get; set; } = null!;

    public string TutorDescription { get; set; } = null!;

    public string TutorPicture { get; set; } = null!;

    public CertiStatus CertiStatus { get; set; } = CertiStatus.Pending;

    public string? MajorId { get; set; }
    [JsonIgnore]
    public virtual ICollection<ChatBox> ChatBoxes { get; set; } = new List<ChatBox>();
    [JsonIgnore]
    public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();
    [JsonIgnore]
    public virtual ICollection<LearnerFavourite> LearnerFavourites { get; set; } = new List<LearnerFavourite>();
    [JsonIgnore]
    public virtual Major? Major { get; set; }
    [JsonIgnore]
    public virtual ICollection<ReviewRating> ReviewRatings { get; set; } = new List<ReviewRating>();

    public virtual Account TutorNavigation { get; set; } = null!;
}
