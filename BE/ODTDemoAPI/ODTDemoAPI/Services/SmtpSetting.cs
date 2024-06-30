namespace ODTDemoAPI.Services
{
    public class SmtpSetting
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string SenderName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
