namespace Umbra.Models;

public sealed class ConfirmationRequest
{
    public string ActionKey { get; set; } = "";
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public string Before { get; set; } = "";
    public string After { get; set; } = "";
    public string Risk { get; set; } = "Low";
    public bool AllowSuppress { get; set; }
}
