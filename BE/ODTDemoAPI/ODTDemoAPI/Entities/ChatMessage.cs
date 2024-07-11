using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities
{
    public partial class ChatMessage
    {
        public int Id { get; set; }

        public string Sender { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int ChatBoxId { get; set; }

        public DateTime SendDate { get; set; }
        [JsonIgnore]
        public virtual ChatBox ChatBox { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<ChatBox> ChatBoxes { get; set; } = new List<ChatBox>();
    }
}
