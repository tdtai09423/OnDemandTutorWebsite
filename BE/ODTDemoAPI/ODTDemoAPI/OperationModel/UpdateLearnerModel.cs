namespace ODTDemoAPI.OperationModel
{
    public class UpdateLearnerModel : UpdateUserModel
    {
        public int? Age { get; set; }

        public IFormFile? Image { get; set; }
    }
}
