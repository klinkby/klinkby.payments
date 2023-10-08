namespace Klinkby.Payments;

public record PaymentOptions
{
    public string Cardtypelock { get; set; } = "dankort";
    public string Currency { get; set; } = "DKK";
    public string Language { get; set; } = "da";
    public string Protocol { get; set; } = "4";
    public string TestMode { get; set; } = "1";
    public string AutoCapture { get; set; } = "";
    public string AutoFee { get; set; } = "";
    public string MsgType { get; set; } = "authorize";
    public string SplitPayment { get; set; } = "";
}