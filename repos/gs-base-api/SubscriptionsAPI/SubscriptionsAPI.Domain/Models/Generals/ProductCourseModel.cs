namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals
{
    /// <summary>
    /// A course (subject and level) included in a plan's product: subst_plans.product_id → subst_product_item_products →
    /// subst_item_products → gwp_subscription_products (whose product_id holds the item id).
    /// </summary>
    public class ProductCourseModel
    {
        /// <summary>
        /// The plan's product (subst_plans.product_id = subst_product_item_products.product_id).
        /// </summary>
        public string? PlanProductId { get; set; }

        /// <summary>
        /// The item (subst_item_products.id = gwp_subscription_products.product_id).
        /// </summary>
        public string? ItemId { get; set; }

        /// <summary>
        /// The item name (subst_item_products.name).
        /// </summary>
        public string? ItemName { get; set; }

        /// <summary>
        /// Subject code (gwp_subscription_products.subject).
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Subject name (gwp_subscription_products.subject_name).
        /// </summary>
        public string? SubjectName { get; set; }

        /// <summary>
        /// Level code (gwp_subscription_products.level).
        /// </summary>
        public string? Level { get; set; }
    }
}
