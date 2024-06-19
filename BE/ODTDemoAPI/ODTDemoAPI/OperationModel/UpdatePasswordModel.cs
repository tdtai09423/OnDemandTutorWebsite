using System.ComponentModel.DataAnnotations;

namespace ODTDemoAPI.OperationModel
{
    public class UpdatePasswordModel
    {
        public string? CurrentPassword { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
