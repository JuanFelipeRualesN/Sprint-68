using System;
using System.Collections.Generic;
using System.Configuration;
using Stripe;
//using Stripe;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To handle the subscriptions updates.
    /// </summary>
    public class SubscriptionsUpdaterManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The user's language code.</param>
        public SubscriptionsUpdaterManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To cancel a subscription/enrollment.
        /// </summary>
        /// <param name="subscriptionId">The id of the subscription.</param>
        /// <param name="cancelAtPeriodEnd">true if cancel when the period end, else, false.</param>
        /// <param name="cancelledReason">The reason of the cancellation (text of user's explenation).</param>
        /// <param name="cancellationReasonId">The cancellation reason's id. (The cancellation reason's id).</param> 
        /// <param name="userSessionId">The id of the user that is making the request.</param>
        ///         /// <param name="stripeApiKey"> The Stripe API key for authentication.</param>
        /// <returns>Returns a response model of the cancellation process.</returns>

        public ResponseModel CancelSubscription(string subscriptionId, bool cancelAtPeriodEnd, string cancelledReason, string cancellationReasonId, string userSessionId, string stripeApiKey)
        {
            var subscriptionToCancel = new SubscriptionDAO().Read(_connectionStringRead, new SubscriptionModel() { Id = subscriptionId }, true, true)[0];
            var responseModel = new ResponseModel()
            {
                TransactionValid = true,
            };
            var messageManager = new MessageManager(_connectionStringRead, _langCode);

            // Get the subscription stripe id by the subscription id 
            string subscriptionPlatformId = subscriptionToCancel.PlatformId;

            // Check if Stripe, then cancelled with Stripe
            if (subscriptionToCancel.PlatformCode == "STR") // "STR" indicates a Stripe subscription
            {
                try
                {
                    StripeConfiguration.ApiKey = stripeApiKey;
                    var service = new SubscriptionService();

                    if (cancelAtPeriodEnd)
                    {
                        var subscriptionUpdateOptions = new SubscriptionUpdateOptions
                        {
                            CancelAtPeriodEnd = true // Set to true to cancel at the end of the current billing period
                        };

                        // Update the Stripe subscription to cancel at period end
                        var updatedSubscription = service.Update(subscriptionPlatformId, subscriptionUpdateOptions);
                    }
                    else
                    {
                        // Cancel NOW (immediately)
                        var cancelledSubscription = service.Cancel(subscriptionPlatformId, new SubscriptionCancelOptions());

                        // Update your local database to "canceled"
                        new SubscriptionDAO().Update(_connectionString,
                            new SubscriptionModel()
                            {
                                Status = "canceled", // Stripe's status will also be "canceled" immediately
                                ModifiedBy = userSessionId,
                            },
                           new SubscriptionModel()
                           {
                               Id = subscriptionId
                           });
                    }
                }
                catch (StripeException stripeEx)
                {
                    // Log the Stripe error for debugging
                    // Console.WriteLine($"Stripe Error: {stripeEx.StripeCode} - {stripeEx.Message}");
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_stripe_api_call"]) + " Stripe Error: " + stripeEx.Message);
                    return responseModel;
                }
                catch (Exception ex)
                {
                    // Handle other unexpected errors
                    // Console.WriteLine($"General Error: {ex.Message}");
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_general"]) + " " + ex.ToString());
                    return responseModel;
                }
            }
            else if (subscriptionToCancel.PlatformCode == "PRI")
            { 
                new SubscriptionDAO().Update(_connectionString,
                       new SubscriptionModel()
                       {
                           Status = "canceled",
                           ModifiedBy = userSessionId
                       },
                      new SubscriptionModel()
                      {
                          Id = subscriptionId
                      });
            }


            // Now, enter the cancel reasons 
            new CancellationLogDAO().Create(_connectionString, new CancellationLogModel()
            {
                CancellationNow = cancelAtPeriodEnd ? "0" : "1",
                CancellationReasonId = cancellationReasonId,
                Reason = cancelledReason,
                SubscriptionId = subscriptionId,
                ModifiedBy = userSessionId
            });

            // Return response 
            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_canceled_success"]));
            return responseModel;
        }

        /// <summary>
        /// To update a subscription (changing details such a plan).
        /// </summary>
        /// <param name="subscriptionModel">The subscription model to update.</param>
        /// <param name="subscriptionModelWhere">The subscription model where.</param>
        /// <param name="userModifiedBy">The user that is modifying the record.</param>
        /// <returns>Returns the response with transaction details.</returns>
        public ResponseModel UpdateSubscription(SubscriptionModel subscriptionModel, SubscriptionModel subscriptionModelWhere, string userModifiedBy)
        {
            // First, lets validate the subscription 
            Validator validator = new Validator(_connectionStringRead, _langCode);
            ResponseModel response = new ResponseModel()
            {
                TransactionValid = true,
                Messages = new List<string>()
            };
            response = validator.ValidateSubscription(subscriptionModel);

            if (!response.TransactionValid)
                return response;

            subscriptionModel.ModifiedBy = userModifiedBy;
            subscriptionModel.CreatedAt = null;
            subscriptionModel.UpdatedAt = null;

            // Update with stripe
            /*StripeConfiguration.ApiKey = AppConfiguration.Configuration["AppSettings:subs_stripe_key"];
            var service = new SubscriptionService();
            Subscription subscription = service.Get(subscriptionModel.PlatformId);

            SubscriptionItemUpdateOption subscriptionItemUpdateOption = new SubscriptionItemUpdateOption()
            {
                Id = subscription.Items.Data[0].Id,
                PlanId = subscriptionModel.PlanPlatformId
            };

            if (subscription.Quantity > 0)
                subscriptionItemUpdateOption.Quantity = subscription.Quantity;

            var items = new List<SubscriptionItemUpdateOption> {
                   subscriptionItemUpdateOption
                };
            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = false,
                Items = items,
            };

            service.Update(subscription.Items.Data[0].Id, options); */
            new SubscriptionDAO().Update(_connectionString, subscriptionModel, subscriptionModelWhere);

            response.Messages.Add(AppConfiguration.Configuration["AppSettings:subr_message_success_transaction"]);
            return response;
        }
    }
}
