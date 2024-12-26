namespace PickiT_MAUI;

public class PickiTonMultipleCompleteListener(
    IList<string>? paths,
    bool wasSuccessful,
    string? reason)
{
    public IList<string>? Paths { get; } = paths;
    public bool WasSuccessful { get; } = wasSuccessful;
    public string? Reason { get; } = reason;
}