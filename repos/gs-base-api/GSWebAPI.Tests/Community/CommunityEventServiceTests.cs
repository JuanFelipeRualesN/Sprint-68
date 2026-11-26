using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Catalogs;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GSWebAPI.Tests.Community
{
    [Collection(MySqlCollection.Name)]
    public sealed class CommunityEventServiceTests : CommunityTestBase
    {
        public CommunityEventServiceTests(MySqlDatabaseFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task Non_members_cannot_register_even_in_public_groups()
        {
            var (owner, group) = await NewGroupAsync(CommunityGroupVisibility.Public);
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: null);
            var outsider = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();

            var result = await Events(db).RegisterAsync(outsider, communityEvent.Id, new EventRegistrationRequestModel());

            Assert.Equal(ServiceErrorKind.Forbidden, result.ErrorKind);
            Assert.Contains(ServiceErrorCodes.NotActiveMember, result.Messages);
            Assert.False(await db.CommunityEventInterests.IgnoreQueryFilters().AnyAsync(i => i.CommunityEventId == communityEvent.Id));
        }

        [Fact]
        public async Task Platform_events_need_no_group_and_the_group_filter_is_optional()
        {
            var organizer = TestUsers.User(TestUsers.NewId());
            var anyone = TestUsers.User(TestUsers.NewId());
            var staff = TestUsers.User(TestUsers.NewId(), isStaff: true);

            // Any user creates a platform-wide event; everyone sees it and registers.
            var platform = await NewEventAsync(organizer, groupId: null, maxAllowed: 10);
            Assert.Null(platform.CommunityGroupId);
            await using var db = Fixture.CreateContext();
            var service = Events(db);
            Assert.True((await service.GetAsync(anyone, platform.Id)).Valid);
            Assert.True((await service.RegisterAsync(anyone, platform.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 2 })).Valid);

            // The organizer and staff manage it; another user cannot.
            var change = new CommunityEventRequestModel
            {
                EventTitle = "Science fair (new date)",
                MeetingFormatId = MeetingFormat.InPersonId,
                EventDateTime = new DateTime(2026, 12, 2, 14, 0, 0, DateTimeKind.Utc),
                EventTimezone = "America/Puerto_Rico",
                EventMaxAllowedPersons = 10,
            };
            Assert.Contains(ServiceErrorCodes.EventNotOrganizer, (await service.UpdateAsync(anyone, platform.Id, change)).Messages);
            Assert.True((await service.UpdateAsync(organizer, platform.Id, change)).Valid);
            Assert.True((await service.UpdateAsync(staff, platform.Id, change)).Valid);

            // Without the filter: platform-wide events plus those of my groups, never other groups'.
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            var inGroup = await NewEventAsync(owner, group.Id, maxAllowed: null);
            var mine = (await service.ListAsync(member, null, null, 0, 100)).Data!.Items.Select(e => e.Id).ToList();
            Assert.Contains(platform.Id, mine);
            Assert.Contains(inGroup.Id, mine);
            var theirs = (await service.ListAsync(anyone, null, null, 0, 100)).Data!.Items.Select(e => e.Id).ToList();
            Assert.Contains(platform.Id, theirs);
            Assert.DoesNotContain(inGroup.Id, theirs);
            Assert.Equal(new[] { inGroup.Id }, (await service.ListAsync(member, group.Id, null, 0, 100)).Data!.Items.Select(e => e.Id));

            Assert.False((await service.DeleteAsync(anyone, platform.Id)).Valid);
            Assert.True((await service.DeleteAsync(organizer, platform.Id)).Valid);
        }

        [Fact]
        public async Task Events_are_listed_most_recent_first_one_page_at_a_time()
        {
            var (owner, group) = await NewGroupAsync();
            await using var db = Fixture.CreateContext();
            var service = Events(db);
            var ids = new List<ulong>();
            foreach (var day in new[] { 3, 1, 2 })
            {
                var created = await service.CreateAsync(owner, group.Id, new CommunityEventRequestModel
                {
                    EventTitle = "Day " + day,
                    MeetingFormatId = MeetingFormat.OnlineId,
                    EventDateTime = new DateTime(2026, 12, day, 14, 0, 0, DateTimeKind.Utc),
                    EventTimezone = "America/Puerto_Rico",
                });
                Assert.True(created.Valid, string.Join(",", created.Messages));
                ids.Add(created.Data!.Id);
            }

            var first = (await service.ListAsync(owner, group.Id, null, 0, 2)).Data!;
            Assert.Equal((3, 2), (first.TotalItems, first.TotalPages));
            Assert.Equal(new[] { "Day 3", "Day 2" }, first.Items.Select(e => e.EventTitle));
            var last = (await service.ListAsync(owner, group.Id, null, 1, 2)).Data!;
            Assert.Equal("Day 1", Assert.Single(last.Items).EventTitle);

            // fromUtc still filters, and the totals follow it.
            var fromDay2 = (await service.ListAsync(owner, group.Id, new DateTime(2026, 12, 2, 0, 0, 0, DateTimeKind.Utc), 0, 20)).Data!;
            Assert.Equal(2, fromDay2.TotalItems);
        }

        [Fact]
        public async Task Pending_members_cannot_register()
        {
            var (owner, group) = await NewGroupAsync(CommunityGroupVisibility.Private);
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: null);
            var applicant = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            await Groups(db).JoinAsync(applicant, group.Id);

            var result = await Events(db).RegisterAsync(applicant, communityEvent.Id, new EventRegistrationRequestModel());

            Assert.Contains(ServiceErrorCodes.NotActiveMember, result.Messages);
        }

        [Fact]
        public async Task Going_registrations_cannot_exceed_capacity()
        {
            var (owner, group) = await NewGroupAsync();
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: 3);
            var alice = await NewActiveMemberAsync(group, owner);
            var bob = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = Events(db);

            Assert.True((await service.RegisterAsync(alice, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 2 })).Valid);
            Assert.Contains(ServiceErrorCodes.EventFull, (await service.RegisterAsync(bob, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 2 })).Messages);

            // "interested" does not take places; then 1 more fits exactly.
            Assert.True((await service.RegisterAsync(bob, communityEvent.Id, new EventRegistrationRequestModel { Response = CommunityEventResponse.Interested, QuantityOfPersonsAssisting = 5 })).Valid);
            Assert.True((await service.RegisterAsync(bob, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 1 })).Valid);

            // Alice changing her own registration does not count her current places twice.
            Assert.True((await service.RegisterAsync(alice, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 2 })).Valid);
            Assert.Contains(ServiceErrorCodes.EventFull, (await service.RegisterAsync(alice, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 3 })).Messages);

            var read = await service.GetAsync(owner, communityEvent.Id);
            Assert.Equal(3, read.Data!.GoingPersons);
            Assert.Equal(2, read.Data.Attendees!.Count);
        }

        [Fact]
        public async Task Concurrent_registrations_never_exceed_capacity()
        {
            const int capacity = 3;
            const int registrants = 12;
            var (owner, group) = await NewGroupAsync();
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: capacity);
            var members = new List<RequestUser>();
            for (var i = 0; i < registrants; i++)
            {
                members.Add(await NewActiveMemberAsync(group, owner));
            }

            // Each registration uses its own DbContext (own connection), all started at once.
            using var start = new ManualResetEventSlim(false);
            var tasks = members.Select(member => Task.Run(async () =>
            {
                start.Wait();
                await using var db = Fixture.CreateContext();
                return await Events(db).RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 1 });
            })).ToList();
            start.Set();
            var results = await Task.WhenAll(tasks);

            Assert.Equal(capacity, results.Count(r => r.Valid));
            Assert.All(results.Where(r => !r.Valid), r => Assert.Contains(ServiceErrorCodes.EventFull, r.Messages));

            await using var check = Fixture.CreateContext();
            var going = await check.CommunityEventInterests
                .Where(i => i.CommunityEventId == communityEvent.Id && i.Response == CommunityEventResponse.Going)
                .SumAsync(i => (int)i.QuantityOfPersonsAssisting);
            Assert.Equal(capacity, going);
        }

        [Fact]
        public async Task Only_open_events_accept_registrations_but_not_going_is_always_allowed()
        {
            var (owner, group) = await NewGroupAsync();
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: 5);
            var member = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = Events(db);
            Assert.True((await service.RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel())).Valid);

            var close = await service.UpdateAsync(owner, communityEvent.Id, new CommunityEventRequestModel
            {
                EventTitle = communityEvent.EventTitle,
                MeetingFormatId = communityEvent.MeetingFormatId,
                EventDateTime = communityEvent.EventDateTime,
                EventEndDateTime = communityEvent.EventEndDateTime,
                EventTimezone = communityEvent.EventTimezone,
                EventMaxAllowedPersons = communityEvent.EventMaxAllowedPersons,
                EventAvailability = CommunityEventAvailability.Closed,
            });
            Assert.True(close.Valid, string.Join(",", close.Messages));

            Assert.Contains(ServiceErrorCodes.EventNotOpen, (await service.RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 2 })).Messages);
            var notGoing = await service.RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel { Response = CommunityEventResponse.NotGoing });
            Assert.True(notGoing.Valid, string.Join(",", notGoing.Messages));
            Assert.Equal(CommunityEventResponse.NotGoing, notGoing.Data!.Response);
        }

        [Fact]
        public async Task Capacity_cannot_be_lowered_below_the_people_going()
        {
            var (owner, group) = await NewGroupAsync();
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: 5);
            var member = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = Events(db);
            await service.RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel { QuantityOfPersonsAssisting = 4 });

            var lowered = await service.UpdateAsync(owner, communityEvent.Id, new CommunityEventRequestModel
            {
                EventTitle = communityEvent.EventTitle,
                MeetingFormatId = communityEvent.MeetingFormatId,
                EventDateTime = communityEvent.EventDateTime,
                EventTimezone = communityEvent.EventTimezone,
                EventMaxAllowedPersons = 3,
            });

            Assert.Contains(ServiceErrorCodes.EventCapacityInvalid, lowered.Messages);
        }

        [Fact]
        public async Task Event_validation_and_visibility()
        {
            var (owner, group) = await NewGroupAsync();
            var member = await NewActiveMemberAsync(group, owner);
            var outsider = TestUsers.User(TestUsers.NewId());
            await using var db = Fixture.CreateContext();
            var service = Events(db);

            var invalid = await service.CreateAsync(owner, group.Id, new CommunityEventRequestModel
            {
                EventTitle = "Bad",
                MeetingFormatId = 99,
                EventDateTime = new DateTime(2026, 12, 1, 14, 0, 0, DateTimeKind.Utc),
                EventEndDateTime = new DateTime(2026, 12, 1, 13, 0, 0, DateTimeKind.Utc),
                EventTimezone = "Not/AZone",
            });
            Assert.Contains(ServiceErrorCodes.EventEndBeforeStart, invalid.Messages);
            Assert.Contains(ServiceErrorCodes.EventTimezoneInvalid, invalid.Messages);
            Assert.Contains(ServiceErrorCodes.EventMeetingFormatInvalid, invalid.Messages);

            // Plain members cannot organize; outsiders cannot see events.
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.CreateAsync(member, group.Id, new CommunityEventRequestModel
            {
                EventTitle = "Meetup", MeetingFormatId = 1, EventDateTime = DateTime.UtcNow, EventTimezone = "America/Puerto_Rico",
            })).ErrorKind);
            var created = await NewEventAsync(owner, group.Id, maxAllowed: null);
            Assert.Single((await service.ListAsync(member, group.Id, null, 0, 100)).Data!.Items);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.ListAsync(outsider, group.Id, null, 0, 100)).ErrorKind);
            Assert.Equal(ServiceErrorKind.Forbidden, (await service.GetAsync(outsider, created.Id)).ErrorKind);
            Assert.Equal(DateTimeKind.Utc, created.EventDateTime.Kind);
        }

        [Fact]
        public async Task Cancelled_registration_is_soft_deleted_and_restored_on_new_registration()
        {
            var (owner, group) = await NewGroupAsync();
            var communityEvent = await NewEventAsync(owner, group.Id, maxAllowed: 1);
            var member = await NewActiveMemberAsync(group, owner);
            var other = await NewActiveMemberAsync(group, owner);
            await using var db = Fixture.CreateContext();
            var service = Events(db);

            var first = (await service.RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel())).Data!;
            Assert.True((await service.CancelRegistrationAsync(member, communityEvent.Id)).Valid);

            // The cancelled place is free again.
            Assert.True((await service.RegisterAsync(other, communityEvent.Id, new EventRegistrationRequestModel())).Valid);
            Assert.Contains(ServiceErrorCodes.EventFull, (await service.RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel())).Messages);
            var interested = await service.RegisterAsync(member, communityEvent.Id, new EventRegistrationRequestModel { Response = CommunityEventResponse.Interested });
            Assert.Equal(first.Id, interested.Data!.Id);
        }
    }
}
