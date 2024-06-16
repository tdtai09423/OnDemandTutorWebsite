namespace ODTDemoAPI.EntityViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string RoleId { get; set; } = null!;

        public bool Status { get; set; }

        public bool IsEmailVerified { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
