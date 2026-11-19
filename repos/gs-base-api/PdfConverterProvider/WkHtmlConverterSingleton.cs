using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

/// <summary>
/// This is to fix the singleton details when using the library WkHtmlToPdf for more than one Class Library in a single API. 
/// This was created to fix the issue reported in 01/14/2026 when Planning Preview works, the results are affected.
/// </summary>
public static class WkHtmlConverterSingleton
{
    // One instance for the whole process
    private static readonly Lazy<IConverter> _lazy =
        new(() => new SynchronizedConverter(new PdfTools()), isThreadSafe: true);

    public static IConverter Instance => _lazy.Value;
}