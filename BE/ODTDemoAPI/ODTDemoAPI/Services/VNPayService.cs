using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace ODTDemoAPI.Services
{
    public class VNPayService
    {
        private readonly VNPaySetting _vNPaySetting;

        public VNPayService(IOptionsMonitor<VNPaySetting> optionsMonitor)
        {
            _vNPaySetting = optionsMonitor.CurrentValue;
        }

        public string CreatePaymentUrl(int orderId, decimal amount, string orderDescription)
        {
            var tmnCode = _vNPaySetting.TmnCode;
            var hashSecret = _vNPaySetting.HashSecret;
            var url = _vNPaySetting.Url;
            var returnUrl = _vNPaySetting.ReturnUrl;
            var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");

            var vnpayData = new Dictionary<string, string>();
            vnpayData.Add("vnp_Version", "2.1.0");
            vnpayData.Add("vnp_Command", "pay");
            vnpayData.Add("vnp_TmnCode", tmnCode);
            vnpayData.Add("vnp_Amount", ((int)(amount * 100)).ToString());
            vnpayData.Add("vnp_CreateDate", createDate);
            vnpayData.Add("vnp_CurrCode", "VND");
            vnpayData.Add("vnp_IpAddr", "127.0.0.1");
            vnpayData.Add("vnp_Locale", "vn");
            vnpayData.Add("vnp_OrderInfo", orderDescription);
            vnpayData.Add("vnp_OrderType", "other");
            vnpayData.Add("vnp_ReturnUrl", returnUrl);
            vnpayData.Add("vnp_TxnRef", orderId.ToString());

            var query = string.Join("&", vnpayData.OrderBy(x => x.Key).Select(x => $"{x.Key}={x.Value}"));
            var signData = query + hashSecret;
            var vnp_SecureHash = ComputeSha256Hash(signData);

            return $"{url}?{query}&vnp_SecureHash={vnp_SecureHash}";
        }

        private object ComputeSha256Hash(string signData)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(signData));
                var builder = new StringBuilder();
                foreach(var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool ValidateResponse(IQueryCollection vnpayData, string secureHash)
        {
            var hashSecret = _vNPaySetting.HashSecret;
            var data = new Dictionary<string, object>();

            foreach (var key in vnpayData.Keys)
            {
                if (key.StartsWith("vnp_"))
                {
                    data.Add(key, vnpayData[key]);
                }
            }

            var query = string.Join("&", vnpayData.OrderBy(x => x.Key).Select(x => $"{x.Key}={x.Value}"));
            var signData = query + hashSecret;
            var vnp_SecureHash = ComputeSha256Hash(signData);

            return vnp_SecureHash.ToString() == secureHash;
        }
    }
}
