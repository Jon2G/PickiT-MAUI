namespace PickiT_MAUI;

public class PickiTonCompleteListener(string? path, bool wasDriveFile, bool wasUnknownProvider, bool wasSuccessful, string? reason)
{
    public string? Path { get; } = path;
    public bool WasDrivenFile { get; } = wasDriveFile;
    public bool WasUnknownProvider { get; } = wasUnknownProvider;
    public bool WasSuccessful { get; } = wasSuccessful;
    public string? Reason { get; } = reason;
}
