namespace ODTDemoAPI.OperationModel
{
    public class UpdateCurriculumModel
    {
        public string? CurriculumType { get; set; }

        public int? TotalSlot { get; set; }

        public string? CurriculumDescription { get; set; }

        public decimal? PricePerSection { get; set; }
    }
}
