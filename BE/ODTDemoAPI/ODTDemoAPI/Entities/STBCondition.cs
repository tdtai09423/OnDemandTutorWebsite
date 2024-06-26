namespace ODTDemoAPI.Entities
{
    public partial class STBCondition
    {
        public int CID { get; set; }

        public int? OrderId { get; set; }

        public DateTime StartTime { get; set; }

        public int Duration { get; set; }
    }
}
