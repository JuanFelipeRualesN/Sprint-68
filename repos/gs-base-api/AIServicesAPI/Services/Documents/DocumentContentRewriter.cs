using GSAIServicesAPI.Services.Images;
using HtmlAgilityPack;

namespace GSAIServicesAPI.Services.Documents
{
    public sealed class DocumentContentRewriter : IDocumentContentRewriter
    {
        private readonly IBlobUrlSigner _signer;

        public DocumentContentRewriter(IBlobUrlSigner signer) => _signer = signer;

        public string? ToStorageForm(string? content)
            => Rewrite(content, _signer.ToCanonicalUrl);

        public string? ToClientForm(string? content)
            => Rewrite(content, _signer.ToSignedUrl);

        private string? Rewrite(string? content, Func<string, string> map)
        {
            if (string.IsNullOrWhiteSpace(content))
                return content;

            // Cheap guard: skip the parse entirely when there is nothing image-like present.
            if (!content.Contains("<img", StringComparison.OrdinalIgnoreCase))
                return content;

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var images = doc.DocumentNode.SelectNodes("//img[@src]");
            if (images is null)
                return content;

            var changed = false;

            foreach (var img in images)
            {
                var src = img.GetAttributeValue("src", null);
                if (src is null)
                    continue;

                // Decoding matters: the stored HTML has &amp; inside the SAS query string.
                var decoded = HtmlEntity.DeEntitize(src);

                if (!_signer.TryGetBlobName(decoded, out var blobName) || blobName is null)
                    continue; // Foreign URL — leave untouched.

                img.SetAttributeValue("src", map(blobName));
                changed = true;
            }

            return changed ? doc.DocumentNode.OuterHtml : content;
        }
    }
}
