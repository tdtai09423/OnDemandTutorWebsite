namespace ODTDemoAPI.Entities
{
    public partial class ChatBox
    {
        public int Id { get; set; }

        public int LearnerId { get; set; }

        public int TutorId { get; set; }

        public DateTime SendDate { get; set; }

        public int? LastMessageId { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        public virtual ChatMessage? LastMessage { get; set; }

        public virtual Learner Learner { get; set; } = null!;

        public virtual Learner Tutor { get; set; } = null!;
    }
}
