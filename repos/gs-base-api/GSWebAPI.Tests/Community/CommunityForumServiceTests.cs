using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GSWebAPI.Tests.Community
{
    [Collection(MySqlCollection.Name)]
    public sealed class CommunityForumServiceTests : CommunityTestBase
    {
        public CommunityForumServiceTests(MySqlDatabaseFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task Categories_are_seeded()
        {
            await using var db = Fixture.CreateContext();

            var categories = (await ForumService(db).ListCategoriesAsync(null)).Data!;

            var seeded = categories.Where(c => c.Id <= CommunityForumCategory.FirstStepsId).ToList();
            Assert.Equal(new[] { "general", "math", "reading_writing", "science", "organization", "legal_depr", "first_steps" }, seeded.Select(c => c.Code));
            Assert.DoesNotContain(seeded, c => c.ModeratorsOnly);
        }

        [Theory]
        [InlineData("es", "Matemáticas", "Lectura y Escritura")]
        [InlineData("es-PR", "Matemáticas", "Lectura y Escritura")]
        [InlineData("es-PR,es;q=0.9,en;q=0.8", "Matemáticas", "Lectura y Escritura")]
        [InlineData("en", "Math", "Reading and Writing")]
        [InlineData("en-US", "Math", "Reading and Writing")]
        [InlineData(null, "Math", "Reading and Writing")]
        [InlineData("fr", "Math", "Reading and Writing")] // no French translation: English
        public async Task Category_names_follow_the_session_language(string? langCode, string math, string readingWriting)
        {
            await using var db = Fixture.CreateContext();

            var categories = (await ForumService(db).ListCategoriesAsync(langCode)).Data!;

            Assert.Equal(math, categories.Single(c => c.Id == CommunityForumCategory.MathId).Name);
            Assert.Equal(readingWriting, categories.Single(c => c.Id == CommunityForumCategory.ReadingWritingId).Name);
            Assert.Equal("math", categories.Single(c => c.Id == CommunityForumCategory.MathId).Code); // the code never changes
        }

        [Fact]
        public async Task Forums_carry_the_category_name_in_the_session_language()
        {
            var author = TestUsers.User(TestUsers.NewId());
            var spanish = new RequestUser { UserId = author.UserId, RoleId = "1", Token = "test", LangCode = "es-PR" };
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);

            var forum = (await service.CreateAsync(author, null, new CommunityForumRequestModel { Title = "Experimentos", CategoryId = CommunityForumCategory.ScienceId })).Data!;

            Assert.Equal("Science", forum.CategoryName);
            Assert.Equal("Ciencias", (await service.GetAsync(spanish, forum.Id)).Data!.CategoryName);
            Assert.Equal("Ciencias", (await service.ListAsync(spanish, null, CommunityForumCategory.ScienceId, 0, 100)).Data!.Items.Single(f => f.Id == forum.Id).CategoryName);
        }

        [Fact]
        public async Task Active_members_create_forums_and_outsiders_cannot_see_them()
        {
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            var outsider = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);

            var created = await service.CreateAsync(member, group.Id, new CommunityForumRequestModel { Title = "  Math curriculum  ", Description = "Which books?" });
            Assert.True(created.Valid, string.Join(",", created.Messages));
            var forum = created.Data!;
            Assert.Equal("Math curriculum", forum.Title);
            Assert.Equal(CommunityForumCategory.GeneralId, forum.CategoryId);
            Assert.Equal("general", forum.CategoryCode);
            Assert.Equal(member.UserId, forum.AuthorId);
            Assert.Equal($"User {member.UserId}", forum.AuthorName);
            Assert.True(forum.CanEdit);

            Assert.Equal(ServiceErrorKind.Forbidden, (await service.CreateAsync(outsider, group.Id, new CommunityForumRequestModel { Title = "T" })).ErrorKind);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.ListAsync(outsider, group.Id, null, 0, 20)).ErrorKind);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.GetAsync(outsider, forum.Id)).ErrorKind);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.ListPostsAsync(outsider, forum.Id, 0, 50)).ErrorKind);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.ListAsync(member, ulong.MaxValue, null, 0, 20)).ErrorKind);

            Assert.Contains(ServiceErrorCodes.ForumTitleRequired, (await service.CreateAsync(member, group.Id, new CommunityForumRequestModel { Title = " " })).Messages);
            Assert.Contains(ServiceErrorCodes.ForumCategoryInvalid, (await service.CreateAsync(member, group.Id, new CommunityForumRequestModel { Title = "T", CategoryId = 200 })).Messages);

            // A moderators-only category: owners and moderators only.
            await EnsureModeratorsOnlyCategoryAsync();
            var announcement = new CommunityForumRequestModel { Title = "Welcome", CategoryId = TestAnnouncementsId };
            Assert.Contains(ServiceErrorCodes.ForumCategoryModeratorsOnly, (await service.CreateAsync(member, group.Id, announcement)).Messages);
            Assert.True((await service.CreateAsync(owner, group.Id, announcement)).Valid);

            var allPage = (await service.ListAsync(member, group.Id, null, 0, 20)).Data!;
            Assert.Equal(allPage.Items.Count, allPage.TotalItems);
            var all = allPage.Items;
            Assert.Equal(2, all.Count);
            var onlyAnnouncements = (await service.ListAsync(member, group.Id, TestAnnouncementsId, 0, 20)).Data!.Items;
            Assert.Equal("Welcome", Assert.Single(onlyAnnouncements).Title);
        }

        [Fact]
        public async Task Platform_forums_need_no_group_and_staff_moderate_them()
        {
            var author = TestUsers.User(TestUsers.NewId());
            var anyone = TestUsers.User(TestUsers.NewId());
            var staff = TestUsers.User(TestUsers.NewId(), isStaff: true);
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);

            var forum = (await service.CreateAsync(author, null, new CommunityForumRequestModel { Title = "Homeschool tips" })).Data!;
            Assert.Null(forum.CommunityGroupId);
            Assert.True(forum.CanEdit);

            // Everyone reads it and posts; only the author or staff edit it; only staff pin, lock or post announcements.
            Assert.False((await service.GetAsync(anyone, forum.Id)).Data!.CanEdit);
            Assert.True((await service.CreatePostAsync(anyone, forum.Id, new CommunityForumPostRequestModel { PostText = "Read aloud every day." })).Valid);
            Assert.Contains(ServiceErrorCodes.ForumNotAuthor, (await service.UpdateAsync(anyone, forum.Id, new CommunityForumRequestModel { Title = "x" })).Messages);
            Assert.Contains(ServiceErrorCodes.NotModerator, (await service.ModerateAsync(author, forum.Id, new CommunityForumModerationRequestModel { IsPinned = true })).Messages);
            Assert.True((await service.ModerateAsync(staff, forum.Id, new CommunityForumModerationRequestModel { IsPinned = true })).Valid);
            await EnsureModeratorsOnlyCategoryAsync();
            var announcement = new CommunityForumRequestModel { Title = "Welcome", CategoryId = TestAnnouncementsId };
            Assert.Contains(ServiceErrorCodes.ForumCategoryModeratorsOnly, (await service.CreateAsync(author, null, announcement)).Messages);
            Assert.True((await service.CreateAsync(staff, null, announcement)).Valid);

            // Without the filter: platform-wide forums plus those of my groups, never other groups'.
            var (owner, group) = await NewGroupAsync();
            var inGroup = (await service.CreateAsync(owner, group.Id, new CommunityForumRequestModel { Title = "Group only" })).Data!;
            var mine = (await service.ListAsync(owner, null, null, 0, 100)).Data!;
            Assert.Contains(forum.Id, mine.Items.Select(f => f.Id));
            Assert.Contains(inGroup.Id, mine.Items.Select(f => f.Id));
            Assert.DoesNotContain(inGroup.Id, (await service.ListAsync(anyone, null, null, 0, 100)).Data!.Items.Select(f => f.Id));
            Assert.Equal(new[] { inGroup.Id }, (await service.ListAsync(owner, group.Id, null, 0, 100)).Data!.Items.Select(f => f.Id));
        }

        [Fact]
        public async Task Authors_or_moderators_edit_forums_and_only_moderators_pin_or_lock_them()
        {
            var (owner, group) = await NewGroupAsync();
            var author = await NewActiveMemberAsync(group, owner);
            var other = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);
            var older = (await service.CreateAsync(author, group.Id, new CommunityForumRequestModel { Title = "Older" })).Data!;
            var forum = (await service.CreateAsync(author, group.Id, new CommunityForumRequestModel { Title = "Field trips" })).Data!;

            var change = new CommunityForumRequestModel { Title = "Field trips 2027", CategoryId = CommunityForumCategory.OrganizationId };
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.UpdateAsync(other, forum.Id, change)).ErrorKind);
            Assert.False((await service.GetAsync(other, forum.Id)).Data!.CanEdit);
            var updated = (await service.UpdateAsync(author, forum.Id, change)).Data!;
            Assert.Equal("Field trips 2027", updated.Title);
            Assert.Equal("organization", updated.CategoryCode);

            var pinAndLock = new CommunityForumModerationRequestModel { IsPinned = true, IsLocked = true };
            Assert.Contains(ServiceErrorCodes.NotModerator, (await service.ModerateAsync(author, older.Id, pinAndLock)).Messages);
            var moderated = (await service.ModerateAsync(owner, older.Id, pinAndLock)).Data!;
            Assert.True(moderated.IsPinned);
            Assert.True(moderated.IsLocked);

            // Pinned forums are listed first.
            var listed = (await service.ListAsync(other, group.Id, null, 0, 20)).Data!.Items;
            Assert.Equal(new[] { older.Id, forum.Id }, listed.Select(f => f.Id));

            // Locked: no posts or edits, except from owners/moderators.
            Assert.Contains(ServiceErrorCodes.ForumLocked, (await service.CreatePostAsync(other, older.Id, new CommunityForumPostRequestModel { PostText = "Hi" })).Messages);
            Assert.Contains(ServiceErrorCodes.ForumLocked, (await service.UpdateAsync(author, older.Id, new CommunityForumRequestModel { Title = "x" })).Messages);
            Assert.True((await service.CreatePostAsync(owner, older.Id, new CommunityForumPostRequestModel { PostText = "Closed, see the new forum." })).Valid);

            Assert.True((await service.ModerateAsync(owner, older.Id, new CommunityForumModerationRequestModel { IsLocked = false })).Data!.IsPinned);
            Assert.True((await service.CreatePostAsync(other, older.Id, new CommunityForumPostRequestModel { PostText = "Thanks" })).Valid);
        }

        [Fact]
        public async Task Replies_stay_in_their_forum_and_deleted_posts_keep_their_replies()
        {
            var (owner, group) = await NewGroupAsync();
            var alice = await NewActiveMemberAsync(group, owner);
            var bob = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);
            var forum = (await service.CreateAsync(alice, group.Id, new CommunityForumRequestModel { Title = "Reading lists" })).Data!;
            var otherForum = (await service.CreateAsync(alice, group.Id, new CommunityForumRequestModel { Title = "Other" })).Data!;

            var question = (await service.CreatePostAsync(alice, forum.Id, new CommunityForumPostRequestModel { PostText = "Any picks for 3rd grade?" })).Data!;
            var reply = (await service.CreatePostAsync(bob, forum.Id, new CommunityForumPostRequestModel { PostText = "Charlotte's Web", ParentPostId = question.Id })).Data!;
            Assert.Equal(question.Id, reply.ParentPostId);

            Assert.Contains(ServiceErrorCodes.PostParentInvalid, (await service.CreatePostAsync(bob, otherForum.Id, new CommunityForumPostRequestModel { PostText = "x", ParentPostId = question.Id })).Messages);
            Assert.Contains(ServiceErrorCodes.PostTextRequired, (await service.CreatePostAsync(bob, forum.Id, new CommunityForumPostRequestModel { PostText = "  " })).Messages);
            Assert.Contains(ServiceErrorCodes.PostTextRequired, (await service.CreatePostAsync(bob, forum.Id, new CommunityForumPostRequestModel { PostText = new string('a', 10_001) })).Messages);

            var summary = (await service.GetAsync(bob, forum.Id)).Data!;
            Assert.Equal(2, summary.PostsCount);
            Assert.NotNull(summary.LastPostAt);
            Assert.Equal(DateTimeKind.Utc, summary.LastPostAt!.Value.Kind);

            // Only the author or a moderator deletes a post.
            Assert.Contains(ServiceErrorCodes.PostNotAuthor, (await service.DeletePostAsync(bob, question.Id)).Messages);
            Assert.True((await service.DeletePostAsync(alice, question.Id)).Valid);

            // The deleted question is still listed, without author or text, because it has a reply.
            var posts = (await service.ListPostsAsync(bob, forum.Id, 0, 50)).Data!.Items;
            Assert.Equal(new[] { question.Id, reply.Id }, posts.Select(p => p.Id));
            var onePerPage = (await service.ListPostsAsync(bob, forum.Id, 1, 1)).Data!;
            Assert.Equal((2, 2), (onePerPage.TotalItems, onePerPage.TotalPages)); // the placeholder counts
            Assert.Equal(reply.Id, Assert.Single(onePerPage.Items).Id);
            var deleted = posts[0];
            Assert.True(deleted.IsDeleted);
            Assert.Null(deleted.PostText);
            Assert.Null(deleted.AuthorId);
            Assert.Equal(1, deleted.RepliesCount);
            Assert.False(deleted.CanDelete);
            Assert.Contains(ServiceErrorCodes.PostParentInvalid, (await service.CreatePostAsync(bob, forum.Id, new CommunityForumPostRequestModel { PostText = "x", ParentPostId = question.Id })).Messages);
            Assert.Equal(1, (await service.GetAsync(bob, forum.Id)).Data!.PostsCount);

            // Once its last reply is deleted (here by the owner, as moderator), the deleted post is no longer listed.
            Assert.True((await service.DeletePostAsync(owner, reply.Id)).Valid);
            Assert.Empty((await service.ListPostsAsync(bob, forum.Id, 0, 50)).Data!.Items);

            await using var check = Fixture.CreateContext();
            Assert.Equal(2, await check.CommunityForumPosts.IgnoreQueryFilters().CountAsync(p => p.CommunityForumId == forum.Id && p.DeletedAt != null));
        }

        [Fact]
        public async Task Only_the_author_edits_a_post_and_edited_at_is_set()
        {
            var (owner, group) = await NewGroupAsync();
            var author = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);
            var forum = (await service.CreateAsync(author, group.Id, new CommunityForumRequestModel { Title = "Co-op schedule" })).Data!;
            var post = (await service.CreatePostAsync(author, forum.Id, new CommunityForumPostRequestModel { PostText = "Tuesdays?" })).Data!;
            Assert.Null(post.EditedAt);
            Assert.True(post.CanEdit);

            Assert.Contains(ServiceErrorCodes.PostNotAuthor, (await service.UpdatePostAsync(owner, post.Id, new CommunityForumPostRequestModel { PostText = "Mondays" })).Messages);

            var unchanged = (await service.UpdatePostAsync(author, post.Id, new CommunityForumPostRequestModel { PostText = " Tuesdays? " })).Data!;
            Assert.Null(unchanged.EditedAt);

            var edited = (await service.UpdatePostAsync(author, post.Id, new CommunityForumPostRequestModel { PostText = "Tuesdays at 10?" })).Data!;
            Assert.Equal("Tuesdays at 10?", edited.PostText);
            Assert.NotNull(edited.EditedAt);
            Assert.Equal(DateTimeKind.Utc, edited.EditedAt!.Value.Kind);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.UpdatePostAsync(author, ulong.MaxValue, new CommunityForumPostRequestModel { PostText = "x" })).ErrorKind);
        }

        [Fact]
        public async Task Foul_language_in_English_or_Spanish_is_rejected_in_forums_and_posts()
        {
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);

            Assert.Contains(ServiceErrorCodes.ForumContentNotAllowed,
                (await service.CreateAsync(member, group.Id, new CommunityForumRequestModel { Title = "This is bullsh1t" })).Messages);
            Assert.Contains(ServiceErrorCodes.ForumContentNotAllowed,
                (await service.CreateAsync(member, group.Id, new CommunityForumRequestModel { Title = "Tareas", Description = "Qué MIERDA de tarea" })).Messages);

            var forum = (await service.CreateAsync(member, group.Id, new CommunityForumRequestModel { Title = "Clase de cálculo", Description = "Class schedule" })).Data!;
            Assert.Contains(ServiceErrorCodes.ForumContentNotAllowed,
                (await service.UpdateAsync(member, forum.Id, new CommunityForumRequestModel { Title = "Pinche clase" })).Messages);

            Assert.Contains(ServiceErrorCodes.ForumContentNotAllowed,
                (await service.CreatePostAsync(member, forum.Id, new CommunityForumPostRequestModel { PostText = "eres un pendejo" })).Messages);
            var post = (await service.CreatePostAsync(member, forum.Id, new CommunityForumPostRequestModel { PostText = "Great class, thanks!" })).Data!;
            Assert.Contains(ServiceErrorCodes.ForumContentNotAllowed,
                (await service.UpdatePostAsync(member, post.Id, new CommunityForumPostRequestModel { PostText = "f u c k this" })).Messages);
            Assert.Equal("Great class, thanks!", (await service.ListPostsAsync(member, forum.Id, 0, 50)).Data!.Items.Single().PostText);
        }

        [Fact]
        public async Task Deleting_a_forum_soft_deletes_its_posts_and_banned_members_lose_access()
        {
            var (owner, group) = await NewGroupAsync();
            var author = await NewActiveMemberAsync(group, owner);
            var other = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = ForumService(db);
            var forum = (await service.CreateAsync(author, group.Id, new CommunityForumRequestModel { Title = "Sports" })).Data!;
            var post = (await service.CreatePostAsync(other, forum.Id, new CommunityForumPostRequestModel { PostText = "Soccer league?" })).Data!;
            Assert.True((await service.CreatePostAsync(author, forum.Id, new CommunityForumPostRequestModel { PostText = "Yes!", ParentPostId = post.Id })).Valid);

            // Banned members lose access to the forums of the group.
            Assert.True((await Groups(db).BanMemberAsync(owner, group.Id, other.UserId)).Valid);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.ListPostsAsync(other, forum.Id, 0, 50)).ErrorKind);

            Assert.True((await service.DeleteAsync(author, forum.Id)).Valid);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.GetAsync(owner, forum.Id)).ErrorKind);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.CreatePostAsync(owner, forum.Id, new CommunityForumPostRequestModel { PostText = "x" })).ErrorKind);

            await using var check = Fixture.CreateContext();
            Assert.NotNull((await check.CommunityForums.IgnoreQueryFilters().SingleAsync(f => f.Id == forum.Id)).DeletedAt);
            Assert.All(await check.CommunityForumPosts.IgnoreQueryFilters().Where(p => p.CommunityForumId == forum.Id).ToListAsync(), p => Assert.NotNull(p.DeletedAt));
            Assert.Equal(2, await check.CommunityForumPosts.IgnoreQueryFilters().CountAsync(p => p.CommunityForumId == forum.Id));
        }
    }
}
