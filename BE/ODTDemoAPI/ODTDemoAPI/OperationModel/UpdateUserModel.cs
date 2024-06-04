namespace ODTDemoAPI.OperationModel
{
    public class UpdateUserModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set;}

        public string? Email { get; set;}

        public UpdatePasswordModel? PasswordModel { get; set;}
    }
}
