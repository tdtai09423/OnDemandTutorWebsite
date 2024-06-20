using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Wallet
{
    public int WalletId { get; set; }

    public decimal Balance { get; set; }

    [JsonIgnore]
    public virtual Account WalletNavigation { get; set; } = null!;
}
