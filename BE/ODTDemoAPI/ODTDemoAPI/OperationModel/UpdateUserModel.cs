using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Runtime.InteropServices;

namespace ODTDemoAPI.OperationModel
{
    public class UpdateUserModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set;}

        [ValidateNever]
        public UpdatePasswordModel? PasswordModel { get; set;}
    }
}
