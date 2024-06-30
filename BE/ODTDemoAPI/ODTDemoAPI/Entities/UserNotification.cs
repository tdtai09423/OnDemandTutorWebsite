using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class UserNotification
{
    public int NotificationId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? NotificateDay { get; set; }

    public int AccountId { get; set; }

    public string NotiStatus { get; set; } = "NEW";
    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;
}
