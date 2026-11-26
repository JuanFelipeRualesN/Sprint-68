namespace GSWebAPI.GSWebAPI.Services.Common
{
    /// <summary>Why a service call failed. Controllers map it to the HTTP status code.</summary>
    public enum ServiceErrorKind
    {
        /// <summary>The request data breaks a business rule (HTTP 400).</summary>
        Validation,

        /// <summary>The resource does not exist or is hidden from the caller (HTTP 404).</summary>
        NotFound,

        /// <summary>The caller is not allowed to perform the action (HTTP 403).</summary>
        Forbidden,

        /// <summary>The action is not allowed in the resource's current state (HTTP 409).</summary>
        Conflict,

        /// <summary>
        /// A dependency (Users API, Athenas, blob storage) could not be reached, so the rule could not be checked or the
        /// action could not be completed (HTTP 503).
        /// </summary>
        Unavailable,
    }

    /// <summary>
    /// Result of a business operation, in the same spirit as the existing response models (Valid + Messages).
    /// Messages hold error codes from <see cref="ServiceErrorCodes"/>.
    /// </summary>
    /// <typeparam name="T">Type of the data returned on success.</typeparam>
    public sealed class ServiceResult<T>
    {
        private ServiceResult()
        {
        }

        /// <summary>True when the operation succeeded; <see cref="Data"/> is then set and <see cref="ErrorKind"/> is null.</summary>
        public bool Valid { get; private init; }

        /// <summary>The operation's result on success; default on failure.</summary>
        public T? Data { get; private init; }

        /// <summary>Category of the failure, used to pick the HTTP status code; null on success.</summary>
        public ServiceErrorKind? ErrorKind { get; private init; }

        /// <summary>Error codes from <see cref="ServiceErrorCodes"/> on failure; empty on success.</summary>
        public List<string> Messages { get; private init; } = new();

        /// <summary>Creates a successful result.</summary>
        /// <param name="data">The data to return.</param>
        /// <returns>A valid result carrying <paramref name="data"/>.</returns>
        public static ServiceResult<T> Ok(T data) => new() { Valid = true, Data = data };

        /// <summary>Creates a failed result.</summary>
        /// <param name="kind">Category of the failure.</param>
        /// <param name="codes">Error codes from <see cref="ServiceErrorCodes"/>.</param>
        /// <returns>An invalid result with the given kind and codes.</returns>
        public static ServiceResult<T> Fail(ServiceErrorKind kind, params string[] codes) =>
            new() { Valid = false, ErrorKind = kind, Messages = codes.ToList() };

        /// <summary>Creates a failed result from a collected list of codes (typically several validation errors).</summary>
        /// <param name="kind">Category of the failure.</param>
        /// <param name="codes">Error codes from <see cref="ServiceErrorCodes"/>; copied into a new list.</param>
        /// <returns>An invalid result with the given kind and codes.</returns>
        public static ServiceResult<T> Fail(ServiceErrorKind kind, IEnumerable<string> codes) =>
            new() { Valid = false, ErrorKind = kind, Messages = codes.ToList() };

        /// <summary>
        /// Re-types a failed result, keeping its kind and codes. Meant for failures only: a successful result becomes a
        /// failed <see cref="ServiceErrorKind.Validation"/> result with no codes.
        /// </summary>
        /// <typeparam name="TOther">Data type of the new result.</typeparam>
        /// <returns>A failed result of <typeparamref name="TOther"/>.</returns>
        public ServiceResult<TOther> As<TOther>() => ServiceResult<TOther>.Fail(ErrorKind ?? ServiceErrorKind.Validation, Messages);
    }
}
