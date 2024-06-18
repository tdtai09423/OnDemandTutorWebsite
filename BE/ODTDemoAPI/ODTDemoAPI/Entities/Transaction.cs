using System;
using System.Collections.Generic;

namespace ODTDemoAPI.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;
}
