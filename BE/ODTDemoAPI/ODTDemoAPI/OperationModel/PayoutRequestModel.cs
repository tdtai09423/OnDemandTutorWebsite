namespace ODTDemoAPI.OperationModel
{
    public class PayoutRequestModel
    {
        public int AccountId { get; set; }

        public long Amount { get; set; }

        public static string Currency { get; set; } = "usd";

        public string StripeAccountId { get; set; } = null!;
    }
}
