namespace GSAIServicesAPI.Models.Images
{
    /// <summary>
    /// To generate an image, the client sends a prompt and optional number of steps; the backend builds the request to the image generation endpoint.
    /// </summary>
    /// <param name="prompt">The prompt (request) of the image.</param>
    /// <param name="steps">The number of steps.</param>
    public record GenerateImageRequest(
         string prompt,
         int? steps = 50
     );
}
