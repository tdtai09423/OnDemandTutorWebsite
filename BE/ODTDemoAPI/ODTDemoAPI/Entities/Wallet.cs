using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ODTDemoAPI.Entities;

public partial class Wallet
{
    public int WalletId { get; set; }

    public decimal Balance { get; set; }
}
