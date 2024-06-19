using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Wallet
{
    public int WalletId { get; set; }

    public int AccountId { get; set; }

    public decimal Balance { get; set; }

    [JsonIgnore]
    public virtual Account Account { get; set; } = null!;
<<<<<<< HEAD
}
=======
}
>>>>>>> 698c35669d05c2a798bf88142c05a314fd01a03f
