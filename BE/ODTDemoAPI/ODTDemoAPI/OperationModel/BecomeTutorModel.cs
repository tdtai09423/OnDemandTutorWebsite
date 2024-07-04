using System.ComponentModel.DataAnnotations;

namespace ODTDemoAPI.OperationModel
{
    public class BecomeTutorModel
    {
        [Required]
        public string Nationality { get; set; } = null!;

        [Required]
        public string TutorDescription { get; set; } = null!;

        [Required]
        public IFormFile TutorImage { get; set; } = null!;

        [Required]
        public string MajorId { get; set; } = null!;

        [Required]
        public string CertificateLink { get; set; } = null!;
    }
}
