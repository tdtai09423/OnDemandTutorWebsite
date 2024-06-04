using System.ComponentModel.DataAnnotations;

namespace ODTDemoAPI.OperationModel
{
    public class GoogleLoginInputModel
    {
        [Required]
        public int Age { get; set; }

        public string RoleId { get; set; } = "LEARNER";

        public IFormFile? Picture { get; set; }
    }
}
