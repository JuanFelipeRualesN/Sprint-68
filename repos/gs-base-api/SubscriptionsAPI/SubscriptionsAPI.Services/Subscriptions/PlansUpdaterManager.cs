using System;
using System.Collections.Generic;
using System.Configuration;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To handle the updates of a plan.
    /// </summary>
    public class PlansUpdaterManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The user's language code.</param>
        public PlansUpdaterManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }
       
        /// <summary>
        /// Updates a plan.
        /// </summary>
        /// <param name="planRequestModel">The plan to update.</param>
        /// <param name="userCreatorId">The user that is modifying the record.</param>
        /// <returns>Returns the transaction details.</returns>
        public PlanResponseModel UpdatePlan(PlanRequestModel planRequestModel, string userCreatorId)
        {
            var responseModel = new PlanResponseModel()
            {
                TransactionValid = true
            };

            var messageManager = new MessageManager(_connectionStringRead, _langCode);
            try
            {
                if (string.IsNullOrWhiteSpace(planRequestModel.Id))
                {
                    responseModel.TransactionValid = false;
                    return responseModel;
                }
                // First, let's validate the plan 
                var validator = new Validator(_connectionString, _langCode);
                var validationResponse = validator.ValidatePlan(planRequestModel);
                if (!validationResponse.TransactionValid) // If it fails, return the response with the error. 
                    return validationResponse;

                // If everything is alright, just update it 
                var planToUpdate = new PlanModel
                {
                    Name = planRequestModel.Name,
                    ProfilesQuantity = planRequestModel.ProfilesQuantity,
                    IsForSchool = planRequestModel.IsForSchool,
                    IsPrivate = planRequestModel.IsPrivate,
                    Available = planRequestModel.Available,
                    ModifiedBy = userCreatorId
                };

                var planToUpdateWhere = new PlanModel()
                {
                    Id = planRequestModel.Id
                };

                new PlanDAO().Update(_connectionString, planToUpdate, planToUpdateWhere);

                // Now update the descriptions 
                var planLangModelsToUpdate = new List<PlanLangModel>();
                var planLangModelsToUpdateWhere = new List<PlanLangModel>();

                for (int i = 0; i < planRequestModel.PlanDescriptionsByLang.Count; i++)
                {
                    planLangModelsToUpdate.Add(new PlanLangModel()
                    {
                        Name = planRequestModel.PlanDescriptionsByLang[i].Name,
                        Description = planRequestModel.PlanDescriptionsByLang[i].Description,
                        ModifiedBy = userCreatorId
                    });
                    planLangModelsToUpdateWhere.Add(new PlanLangModel()
                    {
                        LangCode = planRequestModel.PlanDescriptionsByLang[i].LangCode,
                        PlanId = planRequestModel.Id
                    });
                }
                new PlanLangDAO().Update(_connectionString, planLangModelsToUpdate, planLangModelsToUpdateWhere);

                var taxPlanDAO = new TaxPlanDAO();
                taxPlanDAO.Delete(_connectionString, new TaxPlanModel()
                {
                    PlanId = planRequestModel.Id
                });
                if (planRequestModel.PlanTaxesIds != null && planRequestModel.PlanTaxesIds.Count > 0)
                {
                    var planTaxModelList = new List<TaxPlanModel>();
                    for (int i = 0; i < planRequestModel.PlanTaxesIds.Count; i++)
                    {
                        planTaxModelList.Add(new TaxPlanModel()
                        {
                            PlanId = planRequestModel.Id,
                            TaxId = planRequestModel.PlanTaxesIds[i],
                            ModifiedBy = userCreatorId
                        });
                    }
                    if (planTaxModelList.Count > 0)
                    {

                        taxPlanDAO.Create(_connectionString, planTaxModelList);
                    }
                }

                responseModel.TransactionValid = true;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_plan_update"]));
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
