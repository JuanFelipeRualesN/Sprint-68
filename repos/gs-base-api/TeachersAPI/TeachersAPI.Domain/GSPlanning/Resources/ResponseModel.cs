using System.Collections.Generic;

namespace GSPlanning.Domain
{
	/// <inheritdoc/>
	public class ResponseModel : IResponseModel
	{
		/// <inheritdoc/>
		public bool TransactionValid { get; set; }
		/// <inheritdoc/>
		public IEnumerable<string> Messages { get; set; } = new List<string>();

		/// <summary>
		/// Initialize a new instance of <see cref="ResponseModel"/> class.
		/// </summary>
		public ResponseModel()
		{
			TransactionValid = true;
		}

		/// <summary>
		/// Initialize a new instance of <see cref="ResponseModel"/> class.
		/// </summary>
		/// <param name="message">The items that the response will contain.</param>
		public ResponseModel(string message)
		{
			TransactionValid = true;
			Messages = new []{ message};
		}
	}
}
