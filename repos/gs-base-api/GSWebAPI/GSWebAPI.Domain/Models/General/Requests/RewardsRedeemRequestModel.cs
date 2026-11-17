namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// Request model to redeem rewards.
    /// </summary>
    public class RewardsRedeemRequestModel
    {
        /// <summary>
        /// Gets or sets the coins quantity.
        /// </summary>
        public int CoinsQuantity { get; set; }

        /// <summary>
        /// Gets or sets the diamonds quantity.
        /// </summary>
        public int DiamondsQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the quantity to validate is by a code.
        /// </summary>
        public bool ByCode { get; set; }

        /// <summary>
        /// Gets or sets the validation code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is by in APP Purchase (It is not used as 2020.09.10).
        /// </summary>
        public bool ByInAppPurcharse { get; set; }

        /// <summary>
        /// Gets or sets the product id in the store.
        /// </summary>
        public string ProductIdStore { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        public string RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the purchase date.
        /// </summary>
        public string PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets the Id of the transaction.
        /// </summary>
        public string TransactionId { get; set; }
    }
}