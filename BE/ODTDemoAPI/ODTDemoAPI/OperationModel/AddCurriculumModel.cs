﻿namespace ODTDemoAPI.OperationModel
{
    public class AddCurriculumModel
    {
        public int TutorId { get; set; }

        public string CurriculumType { get; set; } = null!;

        public int TotalSlot { get; set; }

        public string CurriculumDescription { get; set; } = null!;

        public decimal PricePerSection { get; set; }
    }
}
