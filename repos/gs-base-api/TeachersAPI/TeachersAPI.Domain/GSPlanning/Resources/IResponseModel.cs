using System.Collections.Generic;

namespace GSPlanning.Domain
{
	/// <summary>
	/// Represents a response model that contains transaction validity status and messages.
	/// </summary>
	public interface IResponseModel
	{
		/// <summary>
		/// Refers to the transaction validity.
		/// </summary>
		/// <remarks>True means that the operation was performed as expected, otherwise not.</remarks>
		bool TransactionValid { get; set; }
		/// <summary>
		/// The messages that provide a meaningful context to the response.
		/// </summary>
		IEnumerable<string> Messages { get; set; }
	}
}
