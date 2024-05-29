using System.ComponentModel.DataAnnotations;

namespace ODTDemoAPI.OperationModel
{
    public class RegisterTutorModel : RegisterModel
    {
        [Required]
        public int TutorAge { get; set; }

        [Required]
        public string Nationality { get; set; } = null!;

        [Required]
        public string TutorDescription { get; set; } = null!;

        [Required]
        public string TutorPicture { get; set; } = null!;

        [Required]
        public string MajorId { get; set; } = null!;

        [Required]
        public string CertificateLink { get; set; } = null!;
    }
}
