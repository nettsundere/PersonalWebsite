namespace PersonalWebsite.ViewModels.Error;

/// <summary>
/// Error view model
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Error code.
    /// </summary>
    public int StatusCode { get; }

    public ErrorViewModel(int code) => StatusCode = code;
}