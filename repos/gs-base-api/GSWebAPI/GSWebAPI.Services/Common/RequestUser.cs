using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;

namespace GSWebAPI.GSWebAPI.Services.Common
{
    /// <summary>
    /// The user making the request, as validated by the Users API (<c>GetUserDetails.GetUserIdRequestOnOtherAPI</c>).
    /// The token is forwarded to the other APIs (users, teachers) so they apply their own permission checks.
    /// </summary>
    public sealed class RequestUser
    {
        public ulong UserId { get; init; }

        public string? RoleId { get; init; }

        public bool IsStaff { get; init; }

        public string? Token { get; init; }

        public string? IpAddress { get; init; }

        public string? LangCode { get; init; }

        /// <summary>Builds the request user from a valid session; returns null if the session or user id is invalid.</summary>
        public static RequestUser? FromSession(ValidationResponseModel? session)
        {
            if (session == null || !session.SessionValid || !ulong.TryParse(session.UserId, out var userId) || userId == 0)
            {
                return null;
            }

            return new RequestUser
            {
                UserId = userId,
                RoleId = session.RoleId,
                IsStaff = session.IsStaff,
                Token = session.Token,
                IpAddress = session.IpAddress,
                LangCode = session.LangCode,
            };
        }
    }
}
