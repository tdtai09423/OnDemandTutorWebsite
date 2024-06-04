namespace ODTDemoAPI.OperationModel
{
    public class UpdateTutorModel : UpdateUserModel
    {
        public int? Age { get; set; }

        public string? Nationality { get; set; }

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}
