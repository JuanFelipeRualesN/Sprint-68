using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the item products response
    /// </summary>
    public class ItemProductResponseModel : ItemProductModel
    {
        /// <summary>
        /// Gets or sets the subject code. 
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the level code.
        /// </summary>
        public string LevelCode { get; set; }

    }
}