using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To handle validations.
    /// </summary>
    public class Validator
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The user's language code.</param>
        public Validator(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To validate the subscription.
        /// </summary>
        /// <param name="subscriptionModel">The subscription model to validate.</param>
        /// <returns>Returns the response model if subscription is valid or not.</returns>
        public ResponseModel ValidateSubscription(SubscriptionModel subscriptionModel)
        {
            ResponseModel responseModel = new ResponseModel()
            {
                TransactionValid = true
            };

            List<string> errorCodes = new List<string>();
            if (!string.IsNullOrWhiteSpace(subscriptionModel.PlatformId))
            {
                responseModel.TransactionValid = false;
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_subscription_platform_id_required"]);
            }
            if (!string.IsNullOrWhiteSpace(subscriptionModel.PlanPlatformId))
            {
                responseModel.TransactionValid = false;
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_subscription_plan_platform_id_required"]);
            }

            if (errorCodes.Count > 0)
                responseModel.Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes);

            return responseModel;
        }

        /// <summary>
        /// Validates the plan to be inserted. 
        /// </summary>
        /// <param name="planRequestModel">The plan request details to be evaluated.</param>
        /// <returns>Returns the response model if plan is valid or not.</returns>
        public PlanResponseModel ValidatePlan(PlanRequestModel planRequestModel)
        {
            List<string> errorCodes = new List<string>();
            PlanResponseModel responseModel = new PlanResponseModel
            {
                TransactionValid = true
            };

            if (string.IsNullOrWhiteSpace(planRequestModel.ProductId))
            {
                // Product ID must be selected. 
                responseModel.TransactionValid = false;
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_product_required"]);
            }

            if (string.IsNullOrWhiteSpace(planRequestModel.Available))
            {
                // The availability must be sent (1 for true, 0 for false) 
                responseModel.TransactionValid = false;
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_plan_availability"]);
            }

            if (string.IsNullOrWhiteSpace(planRequestModel.Interval))
            {
                responseModel.TransactionValid = false;
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_interval"]);
            }

            if (string.IsNullOrWhiteSpace(planRequestModel.Amount) || !planRequestModel.Amount.All(char.IsDigit))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_cost"]);
                // The amount cannot be empty and has to be an integer. 
                responseModel.TransactionValid = false;
            }

            if (string.IsNullOrWhiteSpace(planRequestModel.Currency))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_currency"]);
                // The currency cannot be empty
                responseModel.TransactionValid = false;
            }

            if (string.IsNullOrWhiteSpace(planRequestModel.Name))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_plan_name"]);
                // The name cannot be empty
                responseModel.TransactionValid = false;
            }

            if (string.IsNullOrWhiteSpace(planRequestModel.ProfilesQuantity) || !planRequestModel.ProfilesQuantity.All(char.IsDigit))
            {
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_profile_quantity"]);
                // The profiles quantity has to be submitted. 
                responseModel.TransactionValid = false;
            }

            if (planRequestModel.PlanDescriptionsByLang.Count == 0)
            {
                // The descriptions by language must be subtmitted. 
                responseModel.TransactionValid = false;
                errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_description_by_language"]);
            }
            else
            {
                bool valid = true;
                // Valid each of 
                for (int i = 0; i < planRequestModel.PlanDescriptionsByLang.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(planRequestModel.PlanDescriptionsByLang[i].Name))
                    {
                        valid = false;
                        errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_description_by_language"]);
                        // The plan name must be specified
                    }

                    if (string.IsNullOrWhiteSpace(planRequestModel.PlanDescriptionsByLang[i].Description))
                    {
                        valid = false;
                        errorCodes.Add(AppConfiguration.Configuration["AppSettings:subs_message_error_description_by_language"]);
                        // The plan description must be specified. 
                    }

                    if (!valid)
                        break;
                }
                if (!valid)
                    responseModel.TransactionValid = false;
            }

           
            if (errorCodes.Count > 0)
                responseModel.Messages = new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(errorCodes);
            return responseModel;
        }
    }
}
