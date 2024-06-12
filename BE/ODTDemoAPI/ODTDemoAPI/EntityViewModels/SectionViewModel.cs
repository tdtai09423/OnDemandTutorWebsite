namespace ODTDemoAPI.EntityViewModels
{
    public class SectionViewModel
    {
        public int Id { get; set; }

        public DateTime SectionStart { get; set; }

        public DateTime SectionEnd { get; set; }

        public string? SectionStatus { get; set; }

        public int Price { get; set; }

        public string? MeetUrl { get; set; }
    }
}
