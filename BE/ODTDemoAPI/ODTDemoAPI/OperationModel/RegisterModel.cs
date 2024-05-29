using System.ComponentModel.DataAnnotations;

namespace ODTDemoAPI.OperationModel
{
    public class RegisterModel
    {
        [Required]
        public virtual string FirstName { get; set; } = null!;

        [Required]
        public virtual string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public virtual string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public virtual string ConfirmPassword { get; set; } = null!;
    }
}
