using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.DTOs;
using GSAIServicesAPI.Services.Documents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GSAIServicesAPI.Controllers
{
    /// <summary>
    /// Handles CRUD operations for teacher documents.
    /// All endpoints require a valid session token and are scoped to the authenticated user —
    /// a user can only read, create, update, or delete their own documents.
    /// </summary>
    /// <remarks>
    /// Inherits session validation and localization from <see cref="BaseController"/>.
    /// All user-facing error messages are sourced from the <c>Messages</c> section
    /// in <c>appsettings.json</c> and returned in the language detected from the request.
    /// <br/>
    /// Document content is stored with image URLs in their canonical, unsigned form and is
    /// re-signed on the way out, so a SAS never reaches the database and stored links do not
    /// rot as signatures expire. See <see cref="IDocumentContentRewriter"/>.
    /// </remarks>
    [Route("aiservicesapi/[controller]")]
    public class DocumentsController : BaseController
    {
        private readonly AppDbContext _db;

        /// <summary>Rewrites owned image URLs in document content between their stored and client-facing forms.</summary>
        private readonly IDocumentContentRewriter _rewriter;

        /// <summary>
        /// The module name forwarded to the external auth service on every session validation call.
        /// Defined as a constant so it is easy to locate and change without scanning method bodies.
        /// </summary>
        private const string Module = "AIDocuments";

        /// <summary>
        /// Initializes the controller with the application database context, configuration
        /// and content rewriter.
        /// </summary>
        /// <param name="db">The EF Core database context injected by the DI container.</param>
        /// <param name="config">
        /// The application configuration passed to <see cref="BaseController"/> to support
        /// localized message resolution.
        /// </param>
        /// <param name="rewriter">
        /// Strips SAS query strings from image URLs before persistence and re-signs them
        /// before responses. External image URLs are left untouched.
        /// </param>
        public DocumentsController(
            AppDbContext db,
            IConfiguration config,
            IDocumentContentRewriter rewriter) : base(config)
        {
            _db = db;
            _rewriter = rewriter;
        }

        /// <summary>
        /// Retrieves all documents that belong to the authenticated user, ordered by most recently updated.
        /// </summary>
        /// <remarks>
        /// <b>GET</b> /api/documents
        /// <br/>
        /// Returns an empty list (not an error) when the user has no documents yet.
        /// On a database failure the endpoint still returns <c>200 OK</c> with a warning field
        /// so the client can degrade gracefully without crashing.
        /// <br/>
        /// The entities are materialized before projection: <see cref="MapDocumentRow"/> re-signs
        /// image URLs in managed code and cannot be translated to SQL.
        /// </remarks>
        /// <returns>
        /// <c>200 OK</c> with <c>{ documents: DocumentRow[] }</c> on success.
        /// <c>401 Unauthorized</c> when the session token is missing or invalid.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            try
            {
                var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
                if (!isValid)
                    return Unauthorized();

                var entities = await _db.TeacherDocuments
                    .AsNoTracking()
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.UpdatedAt)
                    .ToListAsync();

                var documents = entities.Select(MapDocumentRow).ToList();

                return Ok(new { documents });
            }
            catch (Exception e)
            {
                return Ok(new { documents = Array.Empty<object>(), source = "db-error", warning = e.Message });
            }
        }

        /// <summary>
        /// Creates a new document and associates it with the authenticated user.
        /// </summary>
        /// <remarks>
        /// <b>POST</b> /api/documents
        /// <br/>
        /// If the request body includes an <c>Id</c>, that value is used as the document's primary key;
        /// otherwise a new <see cref="Guid"/> is generated. Both <c>CreatedAt</c> and <c>UpdatedAt</c>
        /// are set to the current UTC time at the moment of creation.
        /// <br/>
        /// The submitted content is normalized before it is stored: any SAS query string on an owned
        /// image URL is removed so the persisted value stays valid indefinitely.
        /// </remarks>
        /// <param name="request">The document payload received from the client.</param>
        /// <returns>
        /// <c>201 Created</c> with <c>{ document: DocumentRow }</c> on success.
        /// <c>401 Unauthorized</c> when the session token is missing or invalid.
        /// <c>500 Internal Server Error</c> with <c>{ error: string }</c> on a database failure.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentRequest request)
        {
            try
            {
                var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
                if (!isValid)
                    return Unauthorized();

                var doc = new TeacherDocument
                {
                    Id = request.Id ?? Guid.NewGuid().ToString(),
                    UserId = userId!,
                    Title = request.Title,
                    ToolTitle = request.ToolTitle,
                    Category = request.Category,
                    Kind = request.Kind,
                    Model = request.Model,
                    Content = _rewriter.ToStorageForm(request.Content),
                    Prompt = request.Prompt,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _db.TeacherDocuments.Add(doc);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created, new { document = MapDocumentRow(doc) });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
            }
        }

        /// <summary>
        /// Partially updates an existing document that belongs to the authenticated user.
        /// Only the fields provided in the request body are overwritten; omitted fields retain their current values.
        /// </summary>
        /// <remarks>
        /// <b>PATCH</b> /api/documents/{id}
        /// <br/>
        /// The lookup is performed with both the document <paramref name="id"/> and the authenticated
        /// <c>userId</c>, so attempting to update another user's document intentionally returns
        /// <c>404 Not Found</c> rather than <c>403 Forbidden</c> to avoid leaking whether that ID exists.
        /// <br/>
        /// Incoming content is normalized before it is stored. Because the client received signed URLs
        /// and will echo them back on save, this is what prevents an expiring signature from being
        /// written into the database.
        /// </remarks>
        /// <param name="id">The unique identifier of the document to update.</param>
        /// <param name="request">
        /// A partial document payload. Any non-null field will overwrite the stored value.
        /// </param>
        /// <returns>
        /// <c>200 OK</c> with <c>{ document: DocumentRow }</c> on success.
        /// <c>401 Unauthorized</c> when the session token is missing or invalid.
        /// <c>404 Not Found</c> when no document with the given ID exists for this user.
        /// <c>500 Internal Server Error</c> with <c>{ error: string }</c> on a database failure.
        /// </returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateDocument(string id, [FromBody] DocumentRequest request)
        {
            try
            {
                var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
                if (!isValid)
                    return Unauthorized();

                var doc = await _db.TeacherDocuments
                    .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

                if (doc == null)
                    return NotFound(new { error = GetLocalizedMessage("DocumentNotFound") });

                doc.Title = request.Title ?? doc.Title;
                doc.ToolTitle = request.ToolTitle ?? doc.ToolTitle;
                doc.Category = request.Category ?? doc.Category;
                doc.Kind = request.Kind ?? doc.Kind;
                doc.Model = request.Model ?? doc.Model;
                doc.Content = _rewriter.ToStorageForm(request.Content) ?? doc.Content;
                doc.Prompt = request.Prompt ?? doc.Prompt;
                doc.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return Ok(new { document = MapDocumentRow(doc) });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
            }
        }

        /// <summary>
        /// Permanently deletes a document that belongs to the authenticated user.
        /// </summary>
        /// <remarks>
        /// <b>DELETE</b> /api/documents/{id}
        /// <br/>
        /// As with <see cref="UpdateDocument"/>, the lookup requires both the document <paramref name="id"/>
        /// and the authenticated <c>userId</c>. Attempting to delete another user's document returns
        /// <c>404 Not Found</c> instead of <c>403 Forbidden</c> to avoid leaking document existence.
        /// <br/>
        /// Only the database row is removed; any blobs referenced by the document's content are left
        /// in storage and are expected to be reclaimed by a lifecycle policy.
        /// </remarks>
        /// <param name="id">The unique identifier of the document to delete.</param>
        /// <returns>
        /// <c>200 OK</c> with <c>{ ok: true }</c> on success.
        /// <c>401 Unauthorized</c> when the session token is missing or invalid.
        /// <c>404 Not Found</c> when no document with the given ID exists for this user.
        /// <c>500 Internal Server Error</c> with <c>{ error: string }</c> on a database failure.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            try
            {
                var (isValid, userId) = await GetAuthenticatedUserAsync(Module);
                if (!isValid)
                    return Unauthorized();

                var doc = await _db.TeacherDocuments
                    .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

                if (doc == null)
                    return NotFound(new { error = GetLocalizedMessage("DocumentNotFound") });

                _db.TeacherDocuments.Remove(doc);
                await _db.SaveChangesAsync();

                return Ok(new { ok = true });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
            }
        }

        /// <summary>
        /// Projects a <see cref="TeacherDocument"/> entity into a camelCase anonymous object
        /// safe for JSON serialization and API responses.
        /// </summary>
        /// <param name="d">The entity to project.</param>
        /// <returns>An anonymous object containing all public document fields.</returns>
        /// <remarks>
        /// Image URLs in <c>content</c> are re-signed here, so the returned value differs from what
        /// is stored. This runs in managed code and must not be used inside a LINQ-to-Entities
        /// query — materialize the entities first.
        /// </remarks>
        private object MapDocumentRow(TeacherDocument d) => new
        {
            id = d.Id,
            userId = d.UserId,
            title = d.Title,
            toolTitle = d.ToolTitle,
            category = d.Category,
            kind = d.Kind,
            model = d.Model,
            content = _rewriter.ToClientForm(d.Content),
            prompt = d.Prompt,
            createdAt = AsUtc(d.CreatedAt),
            updatedAt = AsUtc(d.UpdatedAt),
        };

        /// <summary>
        /// Tag as UTC so the client can show the document's local time.
        /// </summary>
        private static DateTime AsUtc(DateTime value) =>
            value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value.ToUniversalTime();
    }
}