using System.ComponentModel.DataAnnotations;

namespace ODTDemoAPI.OperationModel
{
    public class ToggleAccountStatusModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public bool Status { get; set; }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 698c35669d05c2a798bf88142c05a314fd01a03f
