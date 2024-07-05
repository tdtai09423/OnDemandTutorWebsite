using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities
{
    public partial class Report
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Content { get; set; } = null!;

        public string Status { get; set; } = null!;
        [JsonIgnore]
        public virtual Account User { get; set; } = null!;
    }
}
