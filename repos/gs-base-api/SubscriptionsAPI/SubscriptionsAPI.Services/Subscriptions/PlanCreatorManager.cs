using System;
using System.Collections.Generic;
using System.Configuration;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;
using Stripe;
using System.Linq;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To create a plan. This is only for commercial purposes.
    /// </summary>
    public class PlanCreatorManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language's code.</param>
        public PlanCreatorManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Creates a Plan in Stripe and creates the plan local. Used for commercial purposes. The non-commercial creates automatically the plan. 
        /// </summary>
        /// <param name="planRequestModel">The plan to be created with all the details</param>
        /// <param name="stripeApiKey">The stripe API key </param>
        /// <param name="userCreatorId">The user that is creating the plan.</param>
        /// <param name="stripeRequired">If stripe is required (for commercial-purposes app) or not. true if yes, else false.</param>
        /// <returns>Returns a response telling the user if the transaction was sucessful or not and the error type</returns>
        public PlanResponseModel CreatePlan(PlanRequestModel planRequestModel, string stripeApiKey, string userCreatorId, bool stripeRequired)
        {
            var messageManager = new MessageManager(_connectionStringRead, _langCode);
            var responseModel = new PlanResponseModel { TransactionValid = true };

            // Step 1: Validate
            var validator = new Validator(_connectionString, _langCode);
            var validationResponse = validator.ValidatePlan(planRequestModel);
            if (!validationResponse.TransactionValid)
                return validationResponse;

            var productReaderManager = new ProductsReaderManager(_connectionStringRead, _langCode);

            try
            {
                var planModelCreate = new PlanModel
                {
                    Amount = planRequestModel.Amount,
                    Available = planRequestModel.Available,
                    Currency = planRequestModel.Currency,
                    Interval = planRequestModel.Interval,
                    IsForSchool = string.IsNullOrWhiteSpace(planRequestModel.IsForSchool) ? "0" : planRequestModel.IsForSchool,
                    IsPrivate = planRequestModel.IsPrivate,
                    Name = planRequestModel.Name.Trim(),
                    PlatformId = Guid.NewGuid().ToString(),
                    ProductId = planRequestModel.ProductId,
                    ProfilesQuantity = planRequestModel.ProfilesQuantity,
                    IntervalCount = planRequestModel.IntervalCount,
                    ModifiedBy = userCreatorId
                };

                if (stripeRequired)
                {
                    StripeConfiguration.ApiKey = stripeApiKey;

                    // Step 2: Get Stripe Product ID
                    var product = productReaderManager.GetProductResponseModelListByProductId(planRequestModel.ProductId, false, false);

                    // Step 3: Create Price (not Plan — plans are deprecated)
                    var priceOptions = new PriceCreateOptions
                    {
                        Product = product.PlatformId,
                        Currency = planRequestModel.Currency,
                        Recurring = new PriceRecurringOptions
                        {
                            Interval = planRequestModel.Interval,
                            IntervalCount = !string.IsNullOrEmpty(planRequestModel.IntervalCount)
                                ? long.Parse(planRequestModel.IntervalCount)
                                : 1
                        },
                        UnitAmount = long.Parse(planRequestModel.Amount),
                        Nickname = planRequestModel.Name.Trim(),
                        TaxBehavior = planRequestModel.TaxType, // or "inclusive" depending on region
                        Active = true
                    };

                    var priceService = new PriceService();
                    var createdPrice = priceService.Create(priceOptions);

                    // Save Stripe Price ID as Platform ID
                    planModelCreate.PlatformId = createdPrice.Id;
                }

                responseModel.PlatformId = planModelCreate.PlatformId;

                // Step 5: Store plan in DB
                var planDAO = new PlanDAO();
                var planCreatedModel = planDAO.Create(_connectionString, planModelCreate)[0];
                responseModel.Id = planCreatedModel.Id;

                // Step 6: Store localized descriptions
                foreach (var desc in planRequestModel.PlanDescriptionsByLang)
                {
                    desc.PlanId = planCreatedModel.Id;
                    desc.ModifiedBy = userCreatorId;
                }
                new PlanLangDAO().Create(_connectionString, planRequestModel.PlanDescriptionsByLang);
               
                // Step 7: Store tax references (for local records)
                var planTaxModelList = planRequestModel.PlanTaxesIds.Select(taxId => new TaxPlanModel
                {
                    PlanId = planCreatedModel.Id,
                    TaxId = taxId,
                    ModifiedBy = userCreatorId
                }).ToList();

                if (planTaxModelList.Any())
                {
                    var taxPlanDAO = new TaxPlanDAO();
                    taxPlanDAO.Create(_connectionString, planTaxModelList);
                }

                // Step 8: Store trial (optional)
                if (planRequestModel.TrialDays > 0)
                {
                    new PlanTrialDAO().Create(_connectionString, new PlanTrialModel
                    {
                        PlanId = planCreatedModel.Id,
                        TrialDays = planRequestModel.TrialDays.ToString(),
                        ModifiedBy = userCreatorId
                    });
                }

                // Finalize
                responseModel.TransactionValid = true;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_plan"]));
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_plan_creation"]) + " " + ex.ToString());
                return responseModel;
            }
        }
    }
}
