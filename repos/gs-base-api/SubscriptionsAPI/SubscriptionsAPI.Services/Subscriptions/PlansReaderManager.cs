using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;
using System.Globalization;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To read the plans and the plans' details.
    /// </summary>
    public class PlansReaderManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The úser's language code.</param>
        public PlansReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the plans according to filters.
        /// </summary>
        /// <param name="planModels">The plan models to filter.</param>
        /// <returns>Returns the list of plan models.</returns>
        public List<PlanModel> GetPlanModels(List<PlanModel> planModels)
        {
            return new PlanDAO().Read(_connectionStringRead, planModels);
        }

        /// <summary>
        /// Returns the plan id by entering the platform id.
        /// </summary>
        /// <param name="planPlatformId">The plan platform id.</param>
        /// <returns>Returns the plan id.</returns>
        public string GetPlanIdByPlatformId(string planPlatformId)
        {
            return new PlanDAO().Read(_connectionStringRead, new PlanModel
            {
                PlatformId = planPlatformId
            }, true, true)[0].Id;
        }

        /// <summary>
        /// Returns the plan model details by the plan platform id list.
        /// </summary>
        /// <param name="planPlatformIdList">A list of plan platform ids.</param>
        /// <returns>Returns the plan model list.</returns>
        public List<PlanModel> GetPlansByPlanPlaformIds(List<string> planPlatformIdList)
        {
            List<PlanModel> planModelToFindList = new List<PlanModel>();
            for (int i = 0; i < planPlatformIdList.Count; i++)
            {
                PlanModel planModel = new PlanModel
                {
                    PlatformId = planPlatformIdList[i]
                };
                planModelToFindList.Add(planModel);
            }
            return new PlanDAO().Read(_connectionStringRead, planModelToFindList);
        }

        /// <summary>
        /// Returns the plan by the plan id.
        /// </summary>
        /// <param name="planId">The plan's id.</param>
        /// <returns>Returns the plans.</returns>
        public string GetPlanPlatformId(string planId)
        {
            return new PlanDAO().Read(_connectionStringRead, new PlanModel
            {
                Id = planId
            }, true, true)[0].PlatformId;
        }

        /// <summary>
        /// Returns the plans according to the filters sent in the parameters.
        /// </summary>
        /// <param name="currency">The currency (USD).</param>
        /// <param name="isForSchool">If the plan is for school or not (true/false).</param>
        /// <param name="isPrivate">If the plan is private or not (true/false).</param>
        /// <param name="onlyAvailables">To call only the availables plans or not. </param>
        /// <param name="filterByLanguage">If filter by language, or return all descriptions according to all the languages available.  </param>
        /// <param name="planId">Optional: The plan's id to call an specific plan.</param>
        /// <param name="specialPromo">Optional: To call only special promo plans.</param>
        /// <returns>Returns the plans found.</returns>
        public List<PlanResponseModel> GetPlans(string currency, bool isForSchool, bool isPrivate, bool onlyAvailables, bool filterByLanguage, string planId, bool specialPromo)
        {
            var planModelList = new PlanDAO().Read(_connectionStringRead, new PlanModel
            {
                Currency = currency,
                IsForSchool = isForSchool ? "1" : "0",
                IsPrivate = isPrivate ? "1" : "0",
                Available = onlyAvailables ? "1" : "0",
                IsSpecialPromo = specialPromo ? "1" : "0",
                Id = planId
            }, false, true);

            // Create string of plan 
            List<string> planIdList = new List<string>();
            for (int i = 0; i < planModelList.Count; i++)
            {
                planIdList.Add(planModelList[i].Id);
            }

            var planTrialModels = GetPlanTrialModelsByPlanIdList(planIdList);

            var planLangModelList = GetPlanDescriptionsByLanguageAndPlanIdList(planIdList, filterByLanguage);
            // Get the currency details
            // Set the decimals according to the currency
            var currencyManager = new Currencies.CurrencyManager(_connectionStringRead, _langCode);
            var currencyModel = currencyManager.GetCurrencyDetailsByCurrencyIso(currency);
            // Filter Plan By Iso is there is any country and state iso for the client
            var planResponseModelListToReturn = new List<PlanResponseModel>();
            // Now connect everything
            for (int i = 0; i < planModelList.Count; i++)
            {
                var planResponseModel = new PlanResponseModel
                {
                    Id = planModelList[i].Id,
                    PlanDescriptionByLang = planLangModelList.FindAll(x => x.PlanId == planModelList[i].Id),
                    Interval = planModelList[i].Interval,
                    Amount = planModelList[i].Amount,
                    ProfilesQuantity = planModelList[i].ProfilesQuantity,
                    Available = planModelList[i].Available,
                    IsForSchool = planModelList[i].IsForSchool,
                    IsPrivate = planModelList[i].IsPrivate,
                    ClientPrice = currencyModel.DecimalPlaces != "0" ? planModelList[i].Currency + " " + planModelList[i].Amount.ToString().Insert(planModelList[i].Amount.Length -
                    int.Parse(currencyModel.DecimalPlaces), ".") : planModelList[i].Amount
                };
                if (planTrialModels.Count > 0)
                {
                    var planTrial = planTrialModels.Find(v => v.PlanId == planModelList[i].Id);
                    if (planTrial != null)
                        planResponseModel.TrialDays = int.Parse(planTrial?.TrialDays);
                }
                planResponseModelListToReturn.Add(planResponseModel);
            }
            return planResponseModelListToReturn;

        }

        /// <summary>
        /// To get the plans.
        /// </summary>
        /// <param name="planRequestModel">The plan's details to filter. The filters could be if private, if for school, the interval, etc.</param>
        /// <param name="filterByLanguage">To filter by language and show the name and description by language.</param>
        /// <param name="orderByDate">To order by created date (desc).</param>
        /// <param name="pageFrom">The page from.</param>
        /// <param name="limiter">How many items per request.</param>
        /// <param name="areDetailsNeeded">If plan details are needed, such as interval, currency, etc.</param>
        /// <returns>Returns the plans found.</returns>
        public PlanSearchResponseModel GetPlans(PlanRequestModel planRequestModel,
            bool filterByLanguage,
            bool orderByDate,
            int pageFrom,
            int limiter,
            bool areDetailsNeeded)
        {

            // Prepare the response 
            PlanSearchResponseModel planSearchResponseModel = new PlanSearchResponseModel
            {
                Plans = new List<PlanResponseModel>()
            };

            List<PlanModel> planModelList = new PlanDAO().Read(_connectionStringRead, new PlanModel
            {
                Id = !string.IsNullOrWhiteSpace(planRequestModel.Id) ? planRequestModel.Id.Trim() : null,
                PlatformId = !string.IsNullOrWhiteSpace(planRequestModel.PlatformId) ? planRequestModel.PlatformId.Trim() : null,
                Currency = !string.IsNullOrWhiteSpace(planRequestModel.Currency) ? planRequestModel.Currency.Trim() : null,
                Interval = !string.IsNullOrWhiteSpace(planRequestModel.Interval) ? planRequestModel.Interval.Trim() : null,
                IsForSchool = planRequestModel.IsForSchool,
                IsPrivate = planRequestModel.IsPrivate,
                Available = planRequestModel.Available,
            }, false, orderByDate);

            if (!orderByDate)
            {
                // Order by date most recent first
                planModelList = planModelList.OrderByDescending(x => DateTime.Parse(x.CreatedAt)).ToList();
            }

            // Now check the name if not null 
            if (!string.IsNullOrWhiteSpace(planRequestModel.Name))
            {
                List<PlanModel> planModelListFilteredByName = new List<PlanModel>();
                planModelListFilteredByName = planModelList.Where(item => item.Name.ToLower().Contains(planRequestModel.Name.ToLower())).ToList();
                planModelList = new List<PlanModel>();
                planModelList = planModelListFilteredByName;
            }

            // If there is a limiter, filter the results. 
            // but first, stablish the page totals 
            planSearchResponseModel.PageTotals = int.Parse(Math.Ceiling(decimal.Parse(planModelList.Count.ToString()) / decimal.Parse(limiter.ToString())).ToString());
            if (limiter > 0)
            {
                // Set the page 
                if (pageFrom == 0)
                    pageFrom = 1;

                int startPage = pageFrom * limiter - limiter;
                int endPage = pageFrom * limiter;

                // If the list is more than the plan model list, set the start page to 0. 
                if (startPage > planModelList.Count)
                    startPage = 0;

                // If the list is more than the end page, set the end page to the list total 
                if (endPage > planModelList.Count)
                    endPage = planModelList.Count;

                // Filter by limit 
                List<PlanModel> planModelFilteredList = new List<PlanModel>();

                for (int i = startPage; i < endPage; i++)
                {
                    planModelFilteredList.Add(planModelList[i]);
                }

                // Assign the main list to be filtered 
                planModelList = planModelFilteredList;
            }

            if (planModelList.Count == 0)
            {
                // return no results found
                planSearchResponseModel.PageTotals = 0;
                return planSearchResponseModel;
            }

            // If all details are not needed, just end it here 
            if (!areDetailsNeeded)
            {
                for (int i = 0; i < planModelList.Count; i++)
                {
                    planSearchResponseModel.Plans.Add(new PlanResponseModel()
                    {
                        Id = planModelList[i].Id,
                        PlatformId = planModelList[i].PlatformId,
                        ProductId = planModelList[i].ProductId,
                        Name = planModelList[i].Name,
                        Interval = planModelList[i].Interval,
                        IntervalCount = planModelList[i].IntervalCount,
                        Currency = planModelList[i].Currency,
                        Amount = planModelList[i].Amount,
                        IsForSchool = planModelList[i].IsForSchool,
                        IsPrivate = planModelList[i].IsPrivate,
                        ProfilesQuantity = planModelList[i].ProfilesQuantity,
                        Available = planModelList[i].Available,
                        ModifiedBy = planModelList[i].ModifiedBy,
                        CreatedAt = planModelList[i].CreatedAt,
                        UpdatedAt = planModelList[i].UpdatedAt
                    });
                }
                return planSearchResponseModel;
            }

            // Else, continue to return the details 
            List<string> planIdList = new List<string>();
            for (int i = 0; i < planModelList.Count; i++)
            {
                planIdList.Add(planModelList[i].Id);
            }

            // Call the plan descriptions by language 
            List<PlanLangModel> planLangModelList = GetPlanDescriptionsByLanguageAndPlanIdList(planIdList, filterByLanguage);

            // Call the plan trials 
            List<PlanTrialModel> planTrialModels = GetPlanTrialModelsByPlanIdList(planIdList);

            // Call the plan available isos 
            List<PlanAvailableIsoModel> planAvailableIsoModels = GetPlanAvailableIsoByPlanIdList(planIdList);

            // Call the taxes planes 
            List<TaxPlanModel> planTaxesModels = GetPlanTaxesByList(planIdList);

            // Relate the data 
            for (int i = 0; i < planModelList.Count; i++)
            {
                PlanTrialModel planTrial = planTrialModels.Find(x => x.PlanId == planModelList[i].Id);
                int trialDays = 0;
                if (planTrial == null)
                {
                    trialDays = 0;
                }
                else
                {
                    trialDays = int.Parse(planTrial.TrialDays);
                }

                planSearchResponseModel.Plans.Add(new PlanResponseModel
                {
                    Id = planModelList[i].Id,
                    PlatformId = planModelList[i].PlatformId,
                    ProductId = planModelList[i].ProductId,
                    Name = planModelList[i].Name,
                    Interval = planModelList[i].Interval,
                    IntervalCount = planModelList[i].IntervalCount,
                    Currency = planModelList[i].Currency,
                    Amount = planModelList[i].Amount,
                    IsForSchool = planModelList[i].IsForSchool,
                    IsPrivate = planModelList[i].IsPrivate,
                    ProfilesQuantity = planModelList[i].ProfilesQuantity,
                    Available = planModelList[i].Available,
                    ModifiedBy = planModelList[i].ModifiedBy,
                    CreatedAt = planModelList[i].CreatedAt,
                    UpdatedAt = planModelList[i].UpdatedAt,
                    TrialDays = trialDays,
                    PlanAvailableIsos = planAvailableIsoModels.FindAll(x => x.PlanId == planModelList[i].Id),
                    PlanTaxes = planTaxesModels.FindAll(x => x.PlanId == planModelList[i].Id),
                    PlanDescriptionByLang = planLangModelList.FindAll(x => x.PlanId == planModelList[i].Id)
                });
            }

            return planSearchResponseModel;
        }

        /// <summary>
        /// Returns the plan's details by the plan id and other filters.
        /// </summary>
        /// <param name="planId">The id of the plan.</param>
        /// <param name="onlyAvailables">If call only available plan. (true/false).</param>
        /// <param name="filterByLanguage">If filter by language or get all the details for all the languages in the DB/</param>
        /// <returns>Returns the plan details.</returns>
        public PlanResponseModel GetPlanDetails(string planId, bool onlyAvailables, bool filterByLanguage)
        {
            var planResponseModel = new PlanResponseModel
            {
                TransactionValid = true
            };

            // First, we check if the plan exists 
            var planModelResult = new PlanDAO().Read(_connectionStringRead, new PlanModel
            {
                Id = planId,
                Available = onlyAvailables ? "1" : null
            }, true, true);

            // If the plan doesn't exists, return error 
            if (planModelResult.Count == 0)
            {
                var messageManager = new MessageManager(_connectionStringRead, _langCode);

                // Plan doesn't exist, or is not active anymore 
                planResponseModel.TransactionValid = false;
                planResponseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_plan_retrieve"]));
                return planResponseModel;
            }

            // Get the currency details
            // Set the decimals according to the currency
            var currencyManager = new Currencies.CurrencyManager(_connectionStringRead, _langCode);
            var currencyModel =
            currencyManager.GetCurrencyDetailsByCurrencyIso(planModelResult[0].Currency);

            // Else, continue 
            planResponseModel.PlanDescriptionByLang = GetPlanDescriptionsByPlanId(planModelResult[0].Id, filterByLanguage);
            planResponseModel.ProductDetails = new ProductsReaderManager(_connectionStringRead, _langCode).GetProductResponseModelListByProductId(planModelResult[0].ProductId, onlyAvailables, true);
            planResponseModel.Amount = planModelResult[0].Amount;
            planResponseModel.Available = planModelResult[0].Available;
            planResponseModel.CreatedAt = planModelResult[0].CreatedAt;
            planResponseModel.Currency = planModelResult[0].Currency;
            planResponseModel.Id = planModelResult[0].Id;
            planResponseModel.Interval = planModelResult[0].Interval;
            planResponseModel.IntervalCount = planModelResult[0].IntervalCount;
            planResponseModel.IsForSchool = planModelResult[0].IsForSchool;
            planResponseModel.IsPrivate = planModelResult[0].IsPrivate;
            planResponseModel.Name = planModelResult[0].Name;
            planResponseModel.PlatformId = planModelResult[0].PlatformId;
            planResponseModel.ProductId = planModelResult[0].ProductId;
            planResponseModel.UpdatedAt = planModelResult[0].UpdatedAt;
            planResponseModel.IsSpecialPromo = planModelResult[0].IsSpecialPromo;
            planResponseModel.SpecialPromoDays = planModelResult[0].SpecialPromoDays;
            planResponseModel.SpecialPromoLimit = planModelResult[0].SpecialPromoLimit;

            try
            {
                planResponseModel.ClientPrice = currencyModel.DecimalPlaces != "0" ? planModelResult[0].Currency + " " + planModelResult[0].Amount.ToString().Insert(planModelResult[0].Amount.Length -
                            int.Parse(currencyModel.DecimalPlaces), ".") : planModelResult[0].Amount;
            }
            catch
            {
                planResponseModel.ClientPrice = "0".ToString();
            }
            planResponseModel.ProfilesQuantity = planModelResult[0].ProfilesQuantity;

            // Check if plan has trial days 
            List<PlanTrialModel> planTrialModels = new PlanTrialDAO().Read(_connectionStringRead, new PlanTrialModel { PlanId = planId }, true, true);

            if (planTrialModels.Count > 0)
                planResponseModel.TrialDays = int.Parse(planTrialModels[0].TrialDays);

            return planResponseModel;
        }

        /// <summary>
        /// Get the Plan availability by ISO code.
        /// </summary>
        /// <param name="countryIsoCode">The country ISO code.</param>
        /// <param name="planModelResult">The plan result model to filter.</param>
        /// <param name="stateIsoCode">The state iso code.</param>
        /// <returns>Returns the list of plans according to the isos.</returns>
        public List<PlanAvailableIsoModel> GetPlanISOAvaialbilityModelList(string countryIsoCode, List<PlanModel> planModelResult, string stateIsoCode)
        {
            // Filter Plan By Iso is there is any country and state iso for the client 
            List<PlanAvailableIsoModel> planAvailableIsoModels = new List<PlanAvailableIsoModel>();
            if (!string.IsNullOrWhiteSpace(countryIsoCode))
            {
                List<PlanAvailableIsoModel> planAvailableIsoModelsToFind = new List<PlanAvailableIsoModel>();
                foreach (PlanModel plan in planModelResult)
                {
                    PlanAvailableIsoModel planAvailableIsoModel = new PlanAvailableIsoModel
                    {
                        PlanId = plan.Id,
                        CountryIso = countryIsoCode
                    };

                    if (!string.IsNullOrWhiteSpace(stateIsoCode))
                    {
                        planAvailableIsoModel.StateIso = stateIsoCode;
                    }
                    planAvailableIsoModelsToFind.Add(planAvailableIsoModel);
                }
                planAvailableIsoModels = new PlanAvailableIsoDAO().Read(_connectionStringRead, planAvailableIsoModelsToFind);
            }
            return planAvailableIsoModels;
        }

        /// <summary>
        /// Get the plans' taxes by plan id list.
        /// </summary>
        /// <param name="planIdList">The plan id list.</param>
        /// <returns>Returns the tax plans.</returns>
        public List<TaxPlanModel> GetPlanTaxesByList(List<string> planIdList)
        {
            List<TaxPlanModel> planTaxesModelToFindList = new List<TaxPlanModel>();
            for (int i = 0; i < planIdList.Count; i++)
            {
                planTaxesModelToFindList.Add(new TaxPlanModel()
                {
                    PlanId = planIdList[i]
                });
            }
            return new TaxPlanDAO().Read(_connectionStringRead, planTaxesModelToFindList);
        }

        /// <summary>
        /// Return the descriptions by language and plan id list 
        /// </summary>
        /// <param name="planIdList">The plan id list we want to find</param>
        /// <param name="filterByLanguage">If you want to filter by language. If true, it's going to take the langCode of the constructor</param>
        /// <returns></returns>
        public List<PlanLangModel> GetPlanDescriptionsByLanguageAndPlanIdList(List<string> planIdList, bool filterByLanguage)
        {
            List<PlanLangModel> planModelToFindList = new List<PlanLangModel>();
            for (int i = 0; i < planIdList.Count; i++)
            {
                planModelToFindList.Add(new PlanLangModel()
                {
                    PlanId = planIdList[i],
                    LangCode = filterByLanguage ? _langCode : null
                });
            }
            return new PlanLangDAO().Read(_connectionStringRead, planModelToFindList);
        }

        /// <summary>
        /// Returns the plan trials models by the plan id list.
        /// </summary>
        /// <param name="planIdList">The plan ids to filter.</param>
        /// <returns>Returns the plan trial model list.</returns>
        public List<PlanTrialModel> GetPlanTrialModelsByPlanIdList(List<string> planIdList)
        {
            List<PlanTrialModel> planTrialModelToFindList = new List<PlanTrialModel>();
            for (int i = 0; i < planIdList.Count; i++)
            {
                planTrialModelToFindList.Add(new PlanTrialModel()
                {
                    PlanId = planIdList[i]
                });
            }
            return new PlanTrialDAO().Read(_connectionStringRead, planTrialModelToFindList);
        }

        /// <summary>
        /// Returns the plan available ISOS by the plan id list.
        /// </summary>
        /// <param name="planIdList">The plan id list to filter.</param>
        /// <returns>Returns the available plans by iso list.</returns>
        public List<PlanAvailableIsoModel> GetPlanAvailableIsoByPlanIdList(List<string> planIdList)
        {
            List<PlanAvailableIsoModel> planAvailableIsoModelToFindList = new List<PlanAvailableIsoModel>();
            for (int i = 0; i < planIdList.Count; i++)
            {
                planAvailableIsoModelToFindList.Add(new PlanAvailableIsoModel()
                {
                    PlanId = planIdList[i]
                });
            }
            return new PlanAvailableIsoDAO().Read(_connectionStringRead, planAvailableIsoModelToFindList);
        }

        /// <summary>
        /// Returns the taxes that applies to the plan. 
        /// </summary>
        /// <param name="planId">The ID of the plan</param>
        /// <param name="stateIsoCode">The user's state ISO code.</param>
        /// <param name="countryIsoCode">The user's country ISO code.</param>
        /// <returns>Return the plan details with the taxes details.</returns>
        public List<TaxModel> GetTaxDetailsThatAppliesToPlan(string planId, string stateIsoCode, string countryIsoCode)
        {
            // First we get the taxes that applies to those states or countrys 
            TaxModel taxModelToFind = new TaxModel()
            {
                CountryIso = countryIsoCode,
                StateIso = stateIsoCode
            };

            List<TaxModel> taxModelResult = new List<TaxModel>();
            TaxDAO taxDAO = new TaxDAO();
            taxModelResult = taxDAO.Read(_connectionStringRead, taxModelToFind, false, false);

            // Now that we have the taxes, we check if they apply to the selected plan

            List<TaxPlanModel> taxPlanModelList = new List<TaxPlanModel>();
            TaxPlanModel taxPlanModelToFind = new TaxPlanModel
            {
                PlanId = planId
            };
            TaxPlanDAO taxPlanDAO = new TaxPlanDAO();
            taxPlanModelList = taxPlanDAO.Read(_connectionStringRead, taxPlanModelToFind, false, false);

            // Return null if there is no plan to apply
            if (taxPlanModelList.Count == 0)
                return null;

            // else, retrutn that taxes that applies 
            // Now we check if they apply and return the details 
            List<TaxModel> taxThatApplies = new List<TaxModel>();

            for (int i = 0; i < taxPlanModelList.Count; i++)
            {
                TaxModel taxModelToAdd = taxModelResult.Find(x => x.Id == taxPlanModelList[i].TaxId);
                if (taxModelToAdd != null)
                    taxThatApplies.Add(taxModelToAdd);
            }

            return taxThatApplies;
        }

        /// <summary>
        /// To calculate the plan cost (for commercial use only).
        /// </summary>
        /// <param name="planId">The id of the plan to calculate.</param>
        /// <param name="onlyAvailables">To check if the plan is available (true), else fale.</param>
        /// <param name="filterByLanguage">To filter the plan description by language.</param>
        /// <param name="quantity">The quantity that the user want to acquire. For example, 2 plans that include 1 profile, the user will have 2 profiles.</param>
        /// <param name="couponPlatformId">The coupon platform Id (the discount code if any).</param>
        /// <param name="stateIsoCode">The country iso (optional). </param>
        /// <param name="countryIsoCode">The state iso (optional). </param>
        /// <returns>Return the plan costs details.</returns>
        public PlanCostResponseModel CalculatePlanCost(string planId, bool onlyAvailables, bool filterByLanguage, int quantity, string couponPlatformId, string stateIsoCode = null, string countryIsoCode = null)
        {

            // Check if the coupon exists to apply the discount
            CouponModel couponModel = null;

            if (!string.IsNullOrWhiteSpace(couponPlatformId))
                couponModel = new CouponReaderManager(_connectionStringRead, _langCode).ValidateCoupon(couponPlatformId);

            var planResponseModel = GetPlanDetails(planId, onlyAvailables, filterByLanguage);

            if(planResponseModel.TransactionValid == false)
            {
                return new PlanCostResponseModel()
                {
                    TransactionValid = false,
                    Messages = planResponseModel.Messages,
                };
            }

            var planCostResponseModel = new PlanCostResponseModel
            {
                Amount = planResponseModel?.Amount,
                Available = planResponseModel?.Available,
                CreatedAt = planResponseModel?.CreatedAt,
                Currency = planResponseModel?.Currency,
                Id = planResponseModel?.Id,
                Interval = planResponseModel?.Interval,
                IntervalCount = planResponseModel?.IntervalCount,
                IsForSchool = planResponseModel?.IsForSchool,
                IsPrivate = planResponseModel?.IsPrivate,
                Name = planResponseModel?.Name,
                PlanAvailableForIso = planResponseModel.PlanAvailableForIso,
                PlanDescriptionByLang = planResponseModel?.PlanDescriptionByLang,
                PlatformId = planResponseModel?.PlatformId,
                ProductDetails = planResponseModel?.ProductDetails,
                ProductId = planResponseModel?.ProductId,
                ProfilesQuantity = (int.Parse(planResponseModel?.ProfilesQuantity) * quantity).ToString(),
                CouponDetails = couponModel?.Name
            };

            // Compare if the coupon is the same currency as the plan currency (only applies to an specific amount of discount, not to percentage discount, which is easier
            if (couponModel != null)
            {
                if (!string.IsNullOrWhiteSpace(couponModel.AmountOff) && !string.IsNullOrWhiteSpace(couponModel.Currency) && couponModel.Currency != planResponseModel.Currency)
                {
                    MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);
                    planCostResponseModel.TransactionValid = false;
                    planCostResponseModel.Messages.Add(messageManager.GetMessageDetails(
                        AppConfiguration.Configuration["AppSettings:subr_message_cannot_be_calculated_invalid_coupon_for_currency"])); // Cannot be calculated with this coupon
                    return planCostResponseModel;
                }
            }

            // Now get the taxes according to the state iso code and country iso code 
            if (string.IsNullOrWhiteSpace(stateIsoCode) && string.IsNullOrWhiteSpace(countryIsoCode))
                planCostResponseModel.Taxes = new List<TaxModel>(); // Empty 
            else
                planCostResponseModel.Taxes = GetTaxDetailsThatAppliesToPlan(planId, stateIsoCode, countryIsoCode);

            // Get the currency details, so we can evaluate the tax total correctly 
            // Set the decimals according to the currency
            var currencyManager = new Currencies.CurrencyManager(_connectionStringRead, _langCode);
            Domain.Models.Currencies.CurrencyModel currencyModel =
            currencyManager.GetCurrencyDetailsByCurrencyIso(planCostResponseModel.Currency);

            // Calculate the amount 
            int subTotal = quantity * int.Parse(planCostResponseModel.Amount);
            decimal subTotalWithDiscount = subTotal;

            if (couponModel != null)
            {
                if (!string.IsNullOrWhiteSpace(couponModel.AmountOff))
                    subTotalWithDiscount = subTotalWithDiscount - int.Parse(couponModel.AmountOff);

                if (!string.IsNullOrWhiteSpace(couponModel.PercentOff))
                    subTotalWithDiscount = Convert.ToInt32(subTotalWithDiscount - subTotalWithDiscount * decimal.Parse(couponModel.PercentOff) * decimal.Parse("0.01".ToString()));
            }

            // Convert according to currency decimal places so the client is clear in what she sees. 
            if (currencyModel.DecimalPlaces != "0")
            {
                try
                {
                    planCostResponseModel.SubTotal = subTotal.ToString().Insert(subTotal.ToString().Length - int.Parse(currencyModel.DecimalPlaces), ".");
                }
                catch (Exception)
                {
                    planCostResponseModel.SubTotal = subTotal.ToString();
                }
                if (couponModel != null)
                {
                    try
                    {
                        planCostResponseModel.SubTotalWithDiscount = subTotalWithDiscount.ToString().Insert(subTotalWithDiscount.ToString().Length - int.Parse(currencyModel.DecimalPlaces), ".");
                    }
                    catch (Exception)
                    {
                        planCostResponseModel.SubTotalWithDiscount = "0";
                    }
                }

            }
            else
            {
                planCostResponseModel.SubTotal = subTotal.ToString();
                if (couponModel != null)
                    planCostResponseModel.SubTotalWithDiscount = subTotalWithDiscount.ToString();
            }

            // Calculate the taxes            
            // Calculate the tax with plan cost totals with discounts if applues 
            if (planCostResponseModel.Taxes?.Count > 0 && !string.IsNullOrWhiteSpace(planCostResponseModel.SubTotalWithDiscount))
            {
                decimal taxTotalDiscounts = 0;
                planCostResponseModel.TaxTotalWithDiscount = taxTotalDiscounts.ToString();
                for (int i = 0; i < planCostResponseModel.Taxes.Count; i++)
                {
                    taxTotalDiscounts += decimal.Parse(planCostResponseModel.SubTotalWithDiscount) * (decimal.Parse(planCostResponseModel.Taxes[i].Percent) / 100);
                }

                // Round the tax total according to the decimal places of the currency 
                taxTotalDiscounts = Math.Round(taxTotalDiscounts, int.Parse(currencyModel.DecimalPlaces));
                planCostResponseModel.TaxTotalWithDiscount = taxTotalDiscounts.ToString();
            }

            // Without discounts
            if (planCostResponseModel.Taxes?.Count > 0)
            {
                decimal taxTotal = 0;
                planCostResponseModel.TaxTotal = taxTotal.ToString();
                for (int i = 0; i < planCostResponseModel.Taxes.Count; i++)
                {
                    taxTotal += decimal.Parse(planCostResponseModel.SubTotal) * (decimal.Parse(planCostResponseModel.Taxes[i].Percent) / 100);
                }

                // Round the tax total according to the decimal places of the currency 
                taxTotal = Math.Round(taxTotal, int.Parse(currencyModel.DecimalPlaces));
                planCostResponseModel.TaxTotal = taxTotal.ToString();
            }


            if (string.IsNullOrWhiteSpace(planCostResponseModel.TaxTotal))
                planCostResponseModel.TaxTotal = "0";

            // Caclulate Total and set the currency 
            decimal total = decimal.Parse(planCostResponseModel.SubTotal) + decimal.Parse(planCostResponseModel.TaxTotal);
            planCostResponseModel.TotalCost = Math.Round(total, int.Parse(currencyModel.DecimalPlaces)).ToString();
            planCostResponseModel.SubTotal = currencyModel.IsoCode + " " + planCostResponseModel.SubTotal;
            if (!string.IsNullOrWhiteSpace(planCostResponseModel.TaxTotal) && planCostResponseModel.TaxTotal != "0")
                planCostResponseModel.TaxTotal = currencyModel.IsoCode + " " + planCostResponseModel.TaxTotal;
            planCostResponseModel.TotalCost = currencyModel.IsoCode + " " + planCostResponseModel.TotalCost;

            // Calculate the total with discounts and set the currency
            if (couponModel != null)
            {
                if (string.IsNullOrWhiteSpace(planCostResponseModel.TaxTotalWithDiscount))
                    planCostResponseModel.TaxTotalWithDiscount = "0";

                decimal totalWithDiscount = decimal.Parse(planCostResponseModel.SubTotalWithDiscount) + decimal.Parse(planCostResponseModel.TaxTotalWithDiscount);
                planCostResponseModel.TotalWithDiscounts = Math.Round(totalWithDiscount, int.Parse(currencyModel.DecimalPlaces)).ToString();
                planCostResponseModel.SubTotalWithDiscount = currencyModel.IsoCode + " " + planCostResponseModel.SubTotalWithDiscount;
                if (!string.IsNullOrWhiteSpace(planCostResponseModel.TaxTotalWithDiscount) && planCostResponseModel.TaxTotalWithDiscount != "0")
                    planCostResponseModel.TaxTotalWithDiscount = currencyModel.IsoCode + " " + planCostResponseModel.TaxTotalWithDiscount;
                planCostResponseModel.TotalWithDiscounts = currencyModel.IsoCode + " " + planCostResponseModel.TotalWithDiscounts;
            }

            return planCostResponseModel;
        }


        /// <summary>
        /// Returns the billing intervals (year, month, diary)
        /// </summary>
        /// <param name="filterByLanguage">If you want to filter this by language, send true.</param>
        /// <param name="onlyAvailables">If you want to call the only availables intervals.</param>
        /// <returns>Returns the billing interval response model.</returns>
        public List<BillingIntervalResponseModel> GetBillingIntervals(bool filterByLanguage, bool onlyAvailables)
        {
            // First we retrieve the billing interval details. 
            var billingIntervalModels = new BillingIntervalDAO().Read(_connectionStringRead, new BillingIntervalModel()
            {
                Available = onlyAvailables ? "1" : null
            }, false, false);

            // Now read the languages 
            var billingIntervalLangModelToFindList = new List<BillingIntervalLangModel>();
            for (int i = 0; i < billingIntervalModels.Count; i++)
            {
                billingIntervalLangModelToFindList.Add(new BillingIntervalLangModel()
                {
                    BillingIntervalId = billingIntervalModels[i].Id,
                    LangCode = filterByLanguage ? _langCode : null
                });
            }

            var billingIntervalLangResultToFindList = new BillingIntervalLangDAO().Read(_connectionStringRead, billingIntervalLangModelToFindList);
            var billingIntervalResults = new List<BillingIntervalResponseModel>();
            // Now we relate the groups 
            for (int i = 0; i < billingIntervalModels.Count; i++)
            {
                billingIntervalResults.Add(new BillingIntervalResponseModel
                {
                    BillingInterval = billingIntervalModels[i],
                    BillingIntervalLang = billingIntervalLangResultToFindList.FindAll(x => x.BillingIntervalId == billingIntervalModels[i].Id)
                });
            }

            return billingIntervalResults;
        }

        /// <summary>
        /// To get the taxes list.
        /// </summary>
        /// <returns>Return a list of taxes.</returns>
        public List<TaxModel> GetTaxes()
        {
            return new TaxDAO().Read(_connectionStringRead, new TaxModel(), false, false);
        }


        /// <summary>
        /// Returns the plan description filtering by the plan id.
        /// </summary>
        /// <param name="planId">The id of the plan to filter.</param>
        /// <param name="filterByLanguage">true to filter the description and name by language.</param>
        /// <returns>Returns the plan language description list.</returns>
        protected List<PlanLangModel> GetPlanDescriptionsByPlanId(string planId, bool filterByLanguage)
        {
            PlanLangModel planModelToFind = new PlanLangModel
            {
                PlanId = planId,
                LangCode = filterByLanguage ? _langCode : null
            };
            PlanLangDAO planLangDAO = new PlanLangDAO();
            return planLangDAO.Read(_connectionStringRead, planModelToFind, true, true);
        }
    }
}
