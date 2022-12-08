using System;
using System.Collections.Immutable;
public class ProcessOrder
{
    public const string MessageUri = "";
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }

    public ImmutableDictionary<string, string> Context { get; set; }
}

