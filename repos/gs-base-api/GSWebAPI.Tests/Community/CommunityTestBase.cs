using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Catalogs;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.Community;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GSWebAPI.Tests.Community
{
    public abstract class CommunityTestBase
    {
        protected CommunityTestBase(MySqlDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        protected MySqlDatabaseFixture Fixture { get; }

        protected FakeUserDirectory Users { get; } = new();

        protected CommunityGroupService Groups(GSWebDbContext db) => new(db, Users, Fixture.Time);

        protected CommunityEventService Events(GSWebDbContext db) => new(db, Users);

        /// <summary>In-memory Blob Storage for uploaded resource files.</summary>
        protected FakeBlobStorage BlobStorage { get; } = new();

        protected CommunityResourceService Resources(GSWebDbContext db) => new(db, Users, Fixture.Time, BlobStorage);

        protected CommunityForumService ForumService(GSWebDbContext db) => new(db, Users, Fixture.Time, new ProfanityFilter());

        /// <summary>A moderators-only category that exists only in the test database (production has none).</summary>
        protected const byte TestAnnouncementsId = 250;

        /// <summary>Adds the <see cref="TestAnnouncementsId"/> category (moderators only) if it is not there yet.</summary>
        protected async Task EnsureModeratorsOnlyCategoryAsync()
        {
            await using var db = Fixture.CreateContext();
            if (!await db.CommunityForumCategories.AnyAsync(c => c.Id == TestAnnouncementsId))
            {
                db.CommunityForumCategories.Add(new CommunityForumCategory { Id = TestAnnouncementsId, Code = "test_announcements", Name = "Test announcements", ModeratorsOnly = true });
                await db.SaveChangesAsync();
            }
        }

        /// <summary>Creates a group owned by a new user and returns both.</summary>
        protected async Task<(RequestUser Owner, CommunityGroupResponseModel Group)> NewGroupAsync(CommunityGroupVisibility visibility = CommunityGroupVisibility.Public)
        {
            var owner = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var result = await Groups(db).CreateAsync(owner, new CommunityGroupRequestModel
            {
                CommunityName = "Homeschool families " + owner.UserId,
                CommunityLocation = "San Juan",
                Visibility = visibility,
            });
            Assert.True(result.Valid, string.Join(",", result.Messages));
            return (owner, result.Data!);
        }

        /// <summary>A new user who is an active member of the group.</summary>
        protected async Task<RequestUser> NewActiveMemberAsync(CommunityGroupResponseModel group, RequestUser owner)
        {
            var user = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var service = Groups(db);
            var joined = await service.JoinAsync(user, group.Id);
            Assert.True(joined.Valid, string.Join(",", joined.Messages));
            if (joined.Data!.Status == CommunityMemberStatus.Pending)
            {
                Assert.True((await service.ApproveMemberAsync(owner, group.Id, user.UserId)).Valid);
            }

            return user;
        }

        protected async Task<CommunityEventResponseModel> NewEventAsync(RequestUser owner, ulong? groupId, ushort? maxAllowed)
        {
            await using var db = Fixture.CreateContext();
            var result = await Events(db).CreateAsync(owner, groupId, new CommunityEventRequestModel
            {
                EventTitle = "Science fair",
                MeetingFormatId = MeetingFormat.InPersonId,
                EventLocation = "Parque Central",
                EventDateTime = new DateTime(2026, 12, 1, 14, 0, 0, DateTimeKind.Utc),
                EventEndDateTime = new DateTime(2026, 12, 1, 16, 0, 0, DateTimeKind.Utc),
                EventTimezone = "America/Puerto_Rico",
                EventMaxAllowedPersons = maxAllowed,
            });
            Assert.True(result.Valid, string.Join(",", result.Messages));
            return result.Data!;
        }
    }
}
