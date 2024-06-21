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
}

