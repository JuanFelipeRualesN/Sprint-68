using System;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights
{
    /// <summary>
    /// A query parameter is missing or invalid; the controller answers 400. The reason is a code that the message catalog translates.
    /// </summary>
    public class InvalidQueryParameterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidQueryParameterException"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the offending parameter.</param>
        /// <param name="reasonCode">Reason code, for example Missing, DateFormat or LimitRange.</param>
        /// <param name="detail">Optional value used by the message (a limit, a school name).</param>
        public InvalidQueryParameterException(string parameterName, string reasonCode, string? detail = null)
            : base($"{parameterName}: {reasonCode}")
        {
            ParameterName = parameterName;
            ReasonCode = reasonCode;
            Detail = detail;
        }

        /// <summary>
        /// Gets the name of the offending parameter.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Gets the reason code.
        /// </summary>
        public string ReasonCode { get; }

        /// <summary>
        /// Gets the optional detail value.
        /// </summary>
        public string? Detail { get; }
    }
}
