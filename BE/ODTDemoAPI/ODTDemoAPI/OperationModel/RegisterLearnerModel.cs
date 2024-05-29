using System.ComponentModel.DataAnnotations;

namespace ODTDemoAPI.OperationModel
{
    public class RegisterLearnerModel : RegisterModel
    {
        [Required]
        public int LearnerAge { get; set; }

        public string? LearnPicture { get; set; }
    }
}
