namespace ODTDemoAPI.Services
{
    public class VNPaySetting
    {
        public string TmnCode { get; set; } = null!;

        public string HashSecret { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string ReturnUrl { get; set; } = null!;
    }
}
