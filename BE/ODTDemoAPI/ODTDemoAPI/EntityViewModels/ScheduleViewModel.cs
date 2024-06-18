namespace ODTDemoAPI.EntityViewModels
{
    public class ScheduleViewModel
    {
        public DateTime Date { get; set; }

        public List<SectionViewModel> Sections { get; set; } = new List<SectionViewModel>();
    }
}
