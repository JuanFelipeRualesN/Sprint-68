using GSAIServicesAPI.Services.Images;
using HtmlAgilityPack;

namespace GSAIServicesAPI.Services.Documents
{
    /// <summary>
    /// Rewrites image references inside document HTML between their stored and client-facing forms.
    /// </summary>
    /// <remarks>
    /// Only URLs that resolve to this application's blob container are altered. External images —
    /// pasted from other sites, data URIs, relative paths — are left exactly as authored.
    /// </remarks>
    public interface IDocumentContentRewriter
    {
        /// <summary>Removes SAS query strings from owned image URLs before persistence.</summary>
        string? ToStorageForm(string? content);

        /// <summary>Re-signs owned image URLs before returning content to a client.</summary>
        string? ToClientForm(string? content);
    }
}