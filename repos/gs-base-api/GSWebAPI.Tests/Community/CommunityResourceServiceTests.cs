using System.Text;
using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.Community;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GSWebAPI.Tests.Community
{
    [Collection(MySqlCollection.Name)]
    public sealed class CommunityResourceServiceTests : CommunityTestBase
    {
        public CommunityResourceServiceTests(MySqlDatabaseFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task Draft_resources_are_visible_only_to_author_and_moderators()
        {
            var (owner, group) = await NewGroupAsync();
            var author = await NewActiveMemberAsync(group, owner);
            var reader = await NewActiveMemberAsync(group, owner);
            var outsider = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var service = Resources(db);

            var draft = (await service.CreateAsync(author, new CommunityResourceRequestModel { CommunityGroupId = group.Id, Title = "Draft", Link = "https://example.org/d" }, null)).Data!;
            var published = (await service.CreateAsync(author, new CommunityResourceRequestModel { CommunityGroupId = group.Id, Title = "Published", Link = "https://example.org/p", Publish = true }, null)).Data!;
            Assert.Null(draft.PublishedDate);
            Assert.NotNull(published.PublishedDate);

            Assert.Equal(2, (await service.ListAsync(author, group.Id, 0, 100)).Data!.Items.Count);
            Assert.Equal(2, (await service.ListAsync(owner, group.Id, 0, 100)).Data!.Items.Count);
            Assert.Equal(new[] { published.Id }, (await service.ListAsync(reader, group.Id, 0, 100)).Data!.Items.Select(r => r.Id));
            Assert.Equal(ServiceErrorKind.NotFound, (await service.GetAsync(reader, draft.Id)).ErrorKind);

            // Group resources need active membership, even when published.
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.ListAsync(outsider, group.Id, 0, 100)).ErrorKind);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.GetAsync(outsider, published.Id)).ErrorKind);
        }

        [Fact]
        public async Task Any_user_creates_platform_resources_and_the_group_filter_is_optional()
        {
            var staff = TestUsers.User(TestUsers.NewId(), isStaff: true);
            var author = TestUsers.User(TestUsers.NewId());
            var anyone = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var service = Resources(db);

            // No group: any user creates it (it used to be staff only).
            var draft = (await service.CreateAsync(author, new CommunityResourceRequestModel { Title = "Platform draft", Link = "https://example.org/pd" }, null)).Data!;
            var published = (await service.CreateAsync(author, new CommunityResourceRequestModel { Title = "Platform", Link = "https://example.org/pp", ResourceType = CommunityResourceType.Video, Publish = true }, null)).Data!;
            Assert.Null(published.CommunityGroupId);

            var visible = (await service.ListAsync(anyone, null, 0, 100)).Data!.Items.Select(r => r.Id).ToList();
            Assert.Contains(published.Id, visible);
            Assert.DoesNotContain(draft.Id, visible);
            Assert.Contains(draft.Id, (await service.ListAsync(author, null, 0, 100)).Data!.Items.Select(r => r.Id));
            Assert.Contains(draft.Id, (await service.ListAsync(staff, null, 0, 100)).Data!.Items.Select(r => r.Id));
            Assert.Equal(CommunityResourceType.Video, (await service.GetAsync(anyone, published.Id)).Data!.ResourceType);

            // Without the filter, a member also gets the resources of their groups, never those of other groups.
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            var inGroup = (await service.CreateAsync(member, new CommunityResourceRequestModel { CommunityGroupId = group.Id, Title = "Group", Link = "https://example.org/g", Publish = true }, null)).Data!;
            var mine = (await service.ListAsync(member, null, 0, 100)).Data!.Items.Select(r => r.Id).ToList();
            Assert.Contains(inGroup.Id, mine);
            Assert.Contains(published.Id, mine);
            Assert.DoesNotContain(inGroup.Id, (await service.ListAsync(anyone, null, 0, 100)).Data!.Items.Select(r => r.Id));
            Assert.Equal(new[] { inGroup.Id }, (await service.ListAsync(member, group.Id, 0, 100)).Data!.Items.Select(r => r.Id));
            var paged = (await service.ListAsync(author, null, 0, 1)).Data!;
            Assert.Single(paged.Items);
            Assert.True(paged.TotalItems >= 2);
            Assert.Equal(paged.TotalItems, paged.TotalPages);
        }

        [Fact]
        public async Task Deleted_resources_are_hidden()
        {
            var (owner, group) = await NewGroupAsync();
            var author = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = Resources(db);
            var resource = (await service.CreateAsync(author, new CommunityResourceRequestModel { CommunityGroupId = group.Id, Title = "Old", Link = "https://example.org/o", Publish = true }, null)).Data!;

            // The group owner can delete a resource of another author.
            Assert.True((await service.DeleteAsync(owner, resource.Id)).Valid);

            await using var check = Fixture.CreateContext();
            Assert.False(await check.CommunityResources.AnyAsync(r => r.Id == resource.Id));
            Assert.NotNull((await check.CommunityResources.IgnoreQueryFilters().SingleAsync(r => r.Id == resource.Id)).DeletedAt);
        }

        private static UploadedFile File(string name, byte[] bytes) => new(new MemoryStream(bytes), name, "application/octet-stream", bytes.Length);

        private static byte[] Bytes(string start, int padTo = 64) => Encoding.ASCII.GetBytes(start).Concat(new byte[Math.Max(0, padTo - start.Length)]).ToArray();

        private static CommunityResourceRequestModel Upload(string title) => new() { Title = title, Publish = true };

        [Fact]
        public async Task Uploaded_files_are_stored_in_blob_storage_and_read_through_a_signed_link()
        {
            var author = TestUsers.User(TestUsers.NewId());
            var reader = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var service = Resources(db);

            // The link and type in the request are ignored for a file: the type comes from the file.
            var request = Upload("Guía de ciencias");
            request.Link = "https://ignored.example.org";
            request.ResourceType = CommunityResourceType.Video;
            var created = await service.CreateAsync(author, request, File("Guía ciencias.pdf", Bytes("%PDF-1.7\n")));

            Assert.True(created.Valid, string.Join(",", created.Messages));
            var pdf = created.Data!;
            Assert.True(pdf.IsUploadedFile);
            Assert.Equal(CommunityResourceType.Document, pdf.ResourceType);
            Assert.Equal("Guía ciencias.pdf", pdf.FileName);
            Assert.Equal("application/pdf", pdf.ContentType);
            Assert.Equal(64UL, pdf.SizeBytes);
            var blob = Assert.Single(BlobStorage.Blobs);
            Assert.StartsWith("community/resources/", blob.Key);
            Assert.EndsWith(".pdf", blob.Key);
            Assert.Equal("application/pdf", blob.Value.ContentType);

            // Every read signs the link; the database keeps the blob name and the unsigned URL.
            var read = (await service.GetAsync(reader, pdf.Id)).Data!;
            Assert.Equal($"https://storage.test/gsweb-api/{blob.Key}?sas=read", read.Link);
            Assert.Contains(read.Link, (await service.ListAsync(reader, null, 0, 100)).Data!.Items.Select(r => r.Link));
            await using var check = Fixture.CreateContext();
            var row = await check.CommunityResources.SingleAsync(r => r.Id == pdf.Id);
            Assert.Equal(blob.Key, row.BlobName);
            Assert.DoesNotContain("sas", row.Link);

            // Media types; LibreOffice/OpenOffice documents are ZIP based like docx.
            Assert.Equal(CommunityResourceType.Audio, (await service.CreateAsync(author, Upload("Podcast"), File("episodio.mp3", Bytes("ID3")))).Data!.ResourceType);
            Assert.Equal(CommunityResourceType.Video, (await service.CreateAsync(author, Upload("Video"), File("clase.MP4", Bytes("\0\0\0\u0018ftypmp42")))).Data!.ResourceType);
            Assert.Equal("application/vnd.oasis.opendocument.text", (await service.CreateAsync(author, Upload("Notas"), File("notas.odt", Bytes("PK\u0003\u0004")))).Data!.ContentType);
            Assert.True((await service.CreateAsync(author, Upload("Texto"), File("lectura.txt", Encoding.UTF8.GetBytes("Había una vez...")))).Valid);

            // Updating the title keeps the file even without a link.
            var updated = await service.UpdateAsync(author, pdf.Id, new CommunityResourceRequestModel { Title = "Guía de ciencias (2)", Publish = true });
            Assert.True(updated.Valid, string.Join(",", updated.Messages));
            Assert.True(updated.Data!.IsUploadedFile);
            Assert.Equal(CommunityResourceType.Document, updated.Data.ResourceType);
        }

        [Fact]
        public async Task Files_of_other_types_or_with_content_that_does_not_match_are_rejected_before_uploading()
        {
            var author = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var service = Resources(db);

            Assert.Contains(ServiceErrorCodes.ResourceFileTypeNotAllowed, (await service.CreateAsync(author, Upload("x"), File("setup.exe", Bytes("MZ")))).Messages);
            Assert.Contains(ServiceErrorCodes.ResourceFileTypeNotAllowed, (await service.CreateAsync(author, Upload("x"), File("foto.png", Bytes("\u0089PNG")))).Messages);
            Assert.Contains(ServiceErrorCodes.ResourceFileTypeNotAllowed, (await service.CreateAsync(author, Upload("x"), File("sinextension", Bytes("%PDF-")))).Messages);
            // A renamed executable: the extension is allowed but the content is not a PDF / Word file.
            Assert.Contains(ServiceErrorCodes.ResourceFileTypeNotAllowed, (await service.CreateAsync(author, Upload("x"), File("tarea.pdf", Bytes("MZ\u0090")))).Messages);
            Assert.Contains(ServiceErrorCodes.ResourceFileTypeNotAllowed, (await service.CreateAsync(author, Upload("x"), File("tarea.docx", Bytes("MZ\u0090")))).Messages);
            Assert.Contains(ServiceErrorCodes.ResourceFileTooLarge, (await service.CreateAsync(author, Upload("x"), File("vacio.pdf", Array.Empty<byte>()))).Messages);
            var tooBig = new UploadedFile(new MemoryStream(Bytes("%PDF-")), "grande.pdf", null, CommunityResourceFiles.MaxFileBytes + 1);
            Assert.Contains(ServiceErrorCodes.ResourceFileTooLarge, (await service.CreateAsync(author, Upload("x"), tooBig)).Messages);
            Assert.Empty(BlobStorage.Blobs);

            // Links: required without a file, and only http/https.
            Assert.Contains(ServiceErrorCodes.ResourceLinkRequired, (await service.CreateAsync(author, Upload("x"), null)).Messages);
            var script = Upload("x");
            script.Link = "javascript:alert(1)";
            Assert.Contains(ServiceErrorCodes.ResourceLinkInvalid, (await service.CreateAsync(author, script, null)).Messages);

            // Blob Storage down: 503 and no row.
            BlobStorage.Available = false;
            var down = await service.CreateAsync(author, Upload("Sin almacenamiento"), File("a.pdf", Bytes("%PDF-")));
            Assert.Equal(ServiceErrorKind.Unavailable, down.ErrorKind);
            Assert.Contains(ServiceErrorCodes.ResourceStorageUnavailable, down.Messages);
            Assert.False(await db.CommunityResources.AnyAsync(r => r.AuthorId == author.UserId));
        }
    }
}
