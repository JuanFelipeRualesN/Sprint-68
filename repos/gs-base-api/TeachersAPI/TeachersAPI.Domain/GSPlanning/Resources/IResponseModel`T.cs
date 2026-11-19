namespace GSPlanning.Domain
{
	/// <summary>
	/// Represents a response model that contains transaction validity status, messages, and an item of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">Refers to the type of the inner content of the response object.</typeparam>
	public interface IResponseModel<T> : IResponseModel
	{
		/// <summary>
		/// The inner content of the response.
		/// </summary>
		T Item { get; set; }
	}
}
