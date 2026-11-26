using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GSWebAPI.Tests.Community
{
    [Collection(MySqlCollection.Name)]
    public sealed class CommunityGroupServiceTests : CommunityTestBase
    {
        public CommunityGroupServiceTests(MySqlDatabaseFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task Creator_becomes_active_owner()
        {
            var (owner, group) = await NewGroupAsync();

            Assert.NotNull(group.MyMembership);
            Assert.Equal(CommunityMemberRole.Owner, group.MyMembership!.Role);
            Assert.Equal(CommunityMemberStatus.Active, group.MyMembership.Status);
            Assert.NotNull(group.MyMembership.JoinedAt);
            Assert.Equal(1, group.ActiveMembers);
            Assert.Equal(owner.UserId, group.CreatedByUserId);
            Assert.Equal($"User {owner.UserId}", group.MyMembership.UserName);
        }

        [Fact]
        public async Task Group_list_pages_carry_the_total_items_and_pages()
        {
            var user = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            for (var i = 0; i < 3; i++)
            {
                var (_, group) = await NewGroupAsync();
                Assert.True((await Groups(db).JoinAsync(user, group.Id)).Valid);
            }

            var first = (await Groups(db).ListAsync(user, onlyMine: true, page: 0, pageSize: 2)).Data!;
            Assert.All(first.Items, g => Assert.Equal($"User {user.UserId}", g.MyMembership!.UserName));
            Assert.Equal((0, 2, 3, 2), (first.Page, first.PageSize, first.TotalItems, first.TotalPages));
            Assert.Equal(2, first.Items.Count);
            var all = (await Groups(db).ListAsync(user, onlyMine: true, page: 0, pageSize: 10)).Data!.Items.Select(g => g.Id).ToList();
            Assert.Equal(all.OrderByDescending(id => id), all); // newest first

            var last = (await Groups(db).ListAsync(user, onlyMine: true, page: 1, pageSize: 2)).Data!;
            Assert.Single(last.Items);
            Assert.Empty(first.Items.Select(g => g.Id).Intersect(last.Items.Select(g => g.Id)));

            // Past the last page: no items, same totals. Out-of-range input is normalized and echoed back.
            var past = (await Groups(db).ListAsync(user, onlyMine: true, page: 5, pageSize: 2)).Data!;
            Assert.Empty(past.Items);
            Assert.Equal((3, 2), (past.TotalItems, past.TotalPages));
            var normalized = (await Groups(db).ListAsync(user, onlyMine: true, page: -1, pageSize: 500)).Data!;
            Assert.Equal((0, 100, 1), (normalized.Page, normalized.PageSize, normalized.TotalPages));

            var none = (await Groups(db).ListAsync(TestUsers.User(TestUsers.NewId()), onlyMine: true, page: 0, pageSize: 2)).Data!;
            Assert.Equal((0, 0), (none.TotalItems, none.TotalPages));
        }

        [Fact]
        public async Task Joining_a_public_group_is_active_immediately()
        {
            var (_, group) = await NewGroupAsync(CommunityGroupVisibility.Public);
            var user = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();

            var joined = await Groups(db).JoinAsync(user, group.Id);

            Assert.True(joined.Valid);
            Assert.Equal(CommunityMemberStatus.Active, joined.Data!.Status);
            Assert.Equal(CommunityMemberRole.Member, joined.Data.Role);
            Assert.Equal($"User {user.UserId}", joined.Data.UserName);
            Assert.Equal(Fixture.Time.GetUtcNow().UtcDateTime, joined.Data.JoinedAt);
            Assert.Contains(ServiceErrorCodes.AlreadyMember, (await Groups(db).JoinAsync(user, group.Id)).Messages);
        }

        [Fact]
        public async Task Private_group_join_is_pending_until_an_owner_or_moderator_approves()
        {
            var (owner, group) = await NewGroupAsync(CommunityGroupVisibility.Private);
            var applicant = TestUsers.User(TestUsers.NewId());
            var plainMember = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = Groups(db);

            var joined = await service.JoinAsync(applicant, group.Id);
            Assert.Equal(CommunityMemberStatus.Pending, joined.Data!.Status);
            Assert.Null(joined.Data.JoinedAt);

            // Pending members are not active: they cannot see group content.
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.ListMembersAsync(applicant, group.Id)).ErrorKind);

            // A plain member cannot approve.
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.ApproveMemberAsync(plainMember, group.Id, applicant.UserId)).ErrorKind);

            // A moderator can.
            Assert.True((await service.ChangeMemberRoleAsync(owner, group.Id, plainMember.UserId, CommunityMemberRole.Moderator)).Valid);
            Fixture.Time.Advance(TimeSpan.FromMinutes(5));
            var approved = await service.ApproveMemberAsync(plainMember, group.Id, applicant.UserId);
            Assert.True(approved.Valid, string.Join(",", approved.Messages));
            Assert.Equal(CommunityMemberStatus.Active, approved.Data!.Status);
            Assert.Equal(Fixture.Time.GetUtcNow().UtcDateTime, approved.Data.JoinedAt);
        }

        [Fact]
        public async Task Pending_members_are_listed_only_for_owners_and_moderators()
        {
            var (owner, group) = await NewGroupAsync(CommunityGroupVisibility.Private);
            var member = await NewActiveMemberAsync(group, owner);
            var applicant = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            await Groups(db).JoinAsync(applicant, group.Id);

            var asOwner = (await Groups(db).ListMembersAsync(owner, group.Id)).Data!;
            var asMember = (await Groups(db).ListMembersAsync(member, group.Id)).Data!;

            Assert.Contains(asOwner, m => m.UserId == applicant.UserId && m.Status == CommunityMemberStatus.Pending);
            Assert.DoesNotContain(asMember, m => m.UserId == applicant.UserId);
            Assert.All(asOwner, m => Assert.Equal($"User {m.UserId}", m.UserName));
        }

        [Fact]
        public async Task Leaving_and_joining_again_restores_the_same_membership_row()
        {
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var originalId = (await db.CommunityMembers.SingleAsync(m => m.CommunityGroupId == group.Id && m.UserId == member.UserId)).Id;

            Assert.True((await Groups(db).LeaveAsync(member, group.Id)).Valid);
            await using (var check = Fixture.CreateContext())
            {
                Assert.False(await check.CommunityMembers.AnyAsync(m => m.Id == originalId));
                Assert.NotNull((await check.CommunityMembers.IgnoreQueryFilters().SingleAsync(m => m.Id == originalId)).DeletedAt);
            }

            await using var db2 = Fixture.CreateContext();
            var rejoined = await Groups(db2).JoinAsync(member, group.Id);
            Assert.True(rejoined.Valid, string.Join(",", rejoined.Messages));
            Assert.Equal(originalId, rejoined.Data!.Id);
        }

        [Fact]
        public async Task The_last_owner_cannot_leave_and_banned_users_cannot_rejoin()
        {
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = Groups(db);

            Assert.Contains(ServiceErrorCodes.LastOwner, (await service.LeaveAsync(owner, group.Id)).Messages);
            Assert.True((await service.BanMemberAsync(owner, group.Id, member.UserId)).Valid);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.JoinAsync(member, group.Id)).ErrorKind);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.LeaveAsync(member, group.Id)).ErrorKind);
        }

        [Fact]
        public async Task Deleting_a_group_soft_deletes_it_with_its_children()
        {
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: 10);
            await using (var setup = Fixture.CreateContext())
            {
                Assert.True((await Events(setup).RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel())).Valid);
                Assert.True((await Resources(setup).CreateAsync(member, new CommunityResourceRequestModel { CommunityGroupId = group.Id, Title = "Guide", Link = "https://example.org", Publish = true }, null)).Valid);
                var forum = await ForumService(setup).CreateAsync(member, group.Id, new CommunityForumRequestModel { Title = "General", Description = "Talk" });
                Assert.True(forum.Valid, string.Join(",", forum.Messages));
                Assert.True((await ForumService(setup).CreatePostAsync(member, forum.Data!.Id, new CommunityForumPostRequestModel { PostText = "Hello" })).Valid);
            }

            await using var db = Fixture.CreateContext();
            Assert.Equal(ServiceErrorKind.Forbidden, (await Groups(db).DeleteAsync(member, group.Id)).ErrorKind);
            Assert.True((await Groups(db).DeleteAsync(owner, group.Id)).Valid);

            await using var check = Fixture.CreateContext();
            Assert.False(await check.CommunityGroups.AnyAsync(g => g.Id == group.Id));
            Assert.False(await check.CommunityMembers.AnyAsync(m => m.CommunityGroupId == group.Id));
            Assert.False(await check.CommunityEvents.AnyAsync(e => e.CommunityGroupId == group.Id));
            Assert.False(await check.CommunityEventInterests.AnyAsync(i => i.CommunityEventId == communityEvent.Id));
            Assert.False(await check.CommunityResources.AnyAsync(r => r.CommunityGroupId == group.Id));
            Assert.False(await check.CommunityForums.AnyAsync(f => f.CommunityGroupId == group.Id));
            Assert.False(await check.CommunityForumPosts.AnyAsync(p => p.Forum.CommunityGroupId == group.Id));

            // Rows are kept (soft delete).
            Assert.NotNull((await check.CommunityGroups.IgnoreQueryFilters().SingleAsync(g => g.Id == group.Id)).DeletedAt);
            Assert.All(await check.CommunityMembers.IgnoreQueryFilters().Where(m => m.CommunityGroupId == group.Id).ToListAsync(), m => Assert.NotNull(m.DeletedAt));
            Assert.All(await check.CommunityEventInterests.IgnoreQueryFilters().Where(i => i.CommunityEventId == communityEvent.Id).ToListAsync(), i => Assert.NotNull(i.DeletedAt));
            Assert.NotNull((await check.CommunityForums.IgnoreQueryFilters().SingleAsync(f => f.CommunityGroupId == group.Id)).DeletedAt);
            Assert.All(await check.CommunityForumPosts.IgnoreQueryFilters().Where(p => p.Forum.CommunityGroupId == group.Id).ToListAsync(), p => Assert.NotNull(p.DeletedAt));
            Assert.Equal(ServiceErrorKind.NotFound, (await Groups(check).GetAsync(owner, group.Id)).ErrorKind);
        }
    }
}
