namespace GSWebAPI.GSWebAPI.Domain.Models.General
{
    /// <summary>A file to store (custom plan evidence, community resource), decoupled from ASP.NET's IFormFile.</summary>
    /// <param name="Content">File content stream.</param>
    /// <param name="FileName">Original file name. Its extension must be an allowed type and decides the stored content type.</param>
    /// <param name="ContentType">Content type sent by the client. Not used: the stored type comes from the extension.</param>
    /// <param name="Length">Size in bytes. Greater than 0 and up to the limit of the receiving service.</param>
    public sealed record UploadedFile(Stream Content, string FileName, string? ContentType, long Length);
}
