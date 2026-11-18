namespace GSPlanning.Domain
{
	/// <inheritdoc cref="IResponseModel{T}"/>
	public class ResponseModel<T> : ResponseModel, IResponseModel<T>
	{
		/// <inheritdoc/>
		public T Item { get; set; }

		/// <summary>
		/// Initialize a new instance of <see cref="ResponseModel{T}"/> class.
		/// </summary>
		/// <param name="item">The items that the response will contain.</param>
		public ResponseModel(T item)
		{
			TransactionValid = true;
			Item = item;
		}
	}
}
