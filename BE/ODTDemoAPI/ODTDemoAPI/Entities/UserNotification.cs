using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class UserNotification
{
    public int NotificationId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? NotificateDay { get; set; }

    public int AccountId { get; set; }
    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;
}
