using System;
using System.Collections.Generic;
using System.Linq;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To handle the subscriptions/enrollments reading.
    /// </summary>
    public class SubscriptionReadersManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The user's language code.</param>
        public SubscriptionReadersManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To read the cancellation logs by filtering a cancellation log model list.
        /// </summary>
        /// <param name="cancellationLogModelsList">The cancellation log model list.</param>
        /// <returns>Returns a list of cancellation log models.</returns>
        public List<CancellationLogModel> GetCancellationLogModels(List<CancellationLogModel> cancellationLogModelsList)
        {
            return new CancellationLogDAO().Read(_connectionStringRead, cancellationLogModelsList);
        }

        /// <summary>
        /// To get the subscriptions models by filtering the subscription model.
        /// </summary>
        /// <param name="subscriptionModelList">The subscription model to filter.</param>
        /// <returns>Returns the subscription model list.</returns>
        public List<SubscriptionModel> GetSubscriptionModels(List<SubscriptionModel> subscriptionModelList)
        {
            return new SubscriptionDAO().Read(_connectionStringRead, subscriptionModelList);
        }

        /// <summary>
        /// Retrieves the customer source token of our database (not in stripe). The source tokens are a representation of 
        /// the user's payment.
        /// </summary>
        /// <param name="customerPlatformId">The customer platform id.</param>
        /// <returns>Returns the source token model.</returns>
        public CustomerSourceTokenModel GetCustomerSourceToken(string customerPlatformId)
        {
            // Check if we have a payment token already 
            List<CustomerSourceTokenModel> customerSourceTokenModels = new CustomerSourceTokenDAO().
                Read(_connectionStringRead, new CustomerSourceTokenModel
                {
                    CustomerPlatformId = customerPlatformId,
                    Active = "1"
                }, true, true);

            if (customerSourceTokenModels.Count == 0)
                return null;
            else
                return customerSourceTokenModels[0];
        }

        /// <summary>
        /// Returns the customer's platform id.
        /// </summary>
        /// <param name="userId">The user id (client).</param>
        /// <returns>Returns the customer's platform id.</returns>
        public string GetCustomerPlatformId(string userId)
        {
            CustomerDAO customerDAO = new CustomerDAO();
            CustomerModel customerModel = new CustomerModel
            {
                UserId = userId
            };
            List<CustomerModel> customerIdList = customerDAO.Read(_connectionStringRead, customerModel, true, true);
            if (customerIdList.Count == 0)
            {
                return null;
            }
            else
            {
                return customerIdList[0].PlatformId;
            }
        }

        /// <summary>
        /// To get the customer models filtering by customer model.
        /// </summary>
        /// <param name="customerModels">The customer models to filter.</param>
        /// <returns>Returns the customer models.</returns>
        public List<CustomerModel> GetCustomerModelList(List<CustomerModel> customerModels)
        {
            return new CustomerDAO().Read(_connectionStringRead, customerModels);
        }

        /// <summary>
        /// To get the subscription's details by subscription id.
        /// </summary>
        /// <param name="subscriptionId">The subscription's id.</param>
        /// <returns>Returns the subscription's details.</returns>
        public SubscriptionUserResponseModel GetSubscriptionDetails(string subscriptionId)
        {
            // First we get the customer platform id by the user id 
            List<SubscriptionModel> subscriptionModelList = new SubscriptionDAO().Read(_connectionStringRead, new SubscriptionModel
            {
                Id = subscriptionId
            }, true, true);

            // Get the User ID


            if (subscriptionModelList.Count == 0)
                return null;
            else
            {
                PlansReaderManager plansReaderManager = new PlansReaderManager(_connectionStringRead, _langCode);
                string planId = plansReaderManager.GetPlanIdByPlatformId(subscriptionModelList[0].PlanPlatformId);
                SubscriptionUserResponseModel subscriptionUserResponseModel = new SubscriptionUserResponseModel
                {
                    CurrentPeriodEnd = subscriptionModelList[0].CurrentPeriodEnd,
                    Plan = new PlansReaderManager(_connectionStringRead, _langCode).GetPlanDetails(planId, false, true),
                    Quantity = subscriptionModelList[0].Quantity,
                    Status = subscriptionModelList[0].Status,
                    SubscriptionId = subscriptionModelList[0].Id
                };

                subscriptionUserResponseModel.ProfilesTotalQuantity = (int.Parse(subscriptionModelList[0].Quantity) * int.Parse(subscriptionUserResponseModel.Plan.ProfilesQuantity)).ToString();


                // Finally we get the subscriptions attached to the profile 
                subscriptionUserResponseModel.SubscriptionProfiles = GetSubscriptionsProfilesAttachedToSubscription(subscriptionId);

                // Get the selected items 
                subscriptionUserResponseModel.SubscriptionIncludedItems = GetSubscriptionsIncludedItems(planId);
                subscriptionUserResponseModel.UserId = GetUserIdByCustomerPlatformId(subscriptionModelList[0].CustomerPlatformId);

                return subscriptionUserResponseModel;
            }
        }

        /// <summary>
        /// To retrieve the subscription general details by the subscription ids list.
        /// </summary>
        /// <param name="subscriptionIds">The subscription ids.</param>
        /// <returns>Returns the subscription general details list.</returns>
        public List<SubscriptionGeneralDetailResponseModel> GetSubscriptionDetailsWithPlan(List<string> subscriptionIds)
        {
            List<SubscriptionModel> subscriptionModelToFind = new List<SubscriptionModel>();
            foreach (string id in subscriptionIds)
            {
                subscriptionModelToFind.Add(new SubscriptionModel()
                {
                    Id = id
                });
            }
            List<SubscriptionModel> subscriptionModelResult = new SubscriptionDAO().Read(_connectionStringRead, subscriptionModelToFind);


            if (subscriptionModelResult.Count == 0)
                return new List<SubscriptionGeneralDetailResponseModel>();


            List<PlanModel> planModelsToFind = new List<PlanModel>();
            foreach (SubscriptionModel subscription in subscriptionModelResult)
            {
                planModelsToFind.Add(new PlanModel()
                {
                    PlatformId = subscription.PlanPlatformId
                });
            }

            List<PlanModel> planModelResult = new PlanDAO().Read(_connectionStringRead, planModelsToFind);
            List<SubscriptionGeneralDetailResponseModel> subscriptionGeneralDetailsList = new List<SubscriptionGeneralDetailResponseModel>();

            foreach (SubscriptionModel subscription in subscriptionModelResult)
            {
                SubscriptionGeneralDetailResponseModel subscriptionGeneralDetails = new SubscriptionGeneralDetailResponseModel()
                {
                    Subscription = subscription
                };
                PlanModel planMOdel = planModelResult.Find(x => x.PlatformId == subscription.PlanPlatformId);
                if (planMOdel != null)
                    subscriptionGeneralDetails.Plan = planMOdel;
                subscriptionGeneralDetailsList.Add(subscriptionGeneralDetails);
            }

            return subscriptionGeneralDetailsList;
        }

        /// <summary>
        /// Returns the subscription details by the subscription id and the user.
        /// </summary>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <param name="userId">The user id (client).</param>
        /// <param name="isStaff">Is staff. The staff can read any details.</param>
        /// <returns>Returns the subscription details with the plan details including products.</returns>
        public SubscriptionUserResponseModel GetSubscriptionDetails(string subscriptionId, string userId, bool isStaff)
        {
            // First we get the customer platform id by the user id 
            var subscriptionModelToFind = new SubscriptionModel()
            {
                Id = subscriptionId
            };

            if (!isStaff)
                subscriptionModelToFind.CustomerPlatformId = GetCustomerPlatformId(userId);

            List<SubscriptionModel> subscriptionModelList = new SubscriptionDAO().Read(_connectionStringRead, subscriptionModelToFind, true, true);

            if (subscriptionModelList.Count == 0)
                return null;
            else
            {
                PlansReaderManager plansReaderManager = new PlansReaderManager(_connectionStringRead, _langCode);
                string planId = plansReaderManager.GetPlanIdByPlatformId(subscriptionModelList[0].PlanPlatformId);
                SubscriptionUserResponseModel subscriptionUserResponseModel = new SubscriptionUserResponseModel
                {
                    CurrentPeriodEnd = subscriptionModelList[0].CurrentPeriodEnd,
                    Plan = new PlansReaderManager(_connectionStringRead, _langCode).GetPlanDetails(planId, false, true),
                    Quantity = subscriptionModelList[0].Quantity,
                    Status = subscriptionModelList[0].Status,
                    SubscriptionId = subscriptionModelList[0].Id
                };

                subscriptionUserResponseModel.ProfilesTotalQuantity = (int.Parse(subscriptionModelList[0].Quantity) * int.Parse(subscriptionUserResponseModel.Plan.ProfilesQuantity)).ToString();


                // Finally we get the subscriptions attached to the profile 
                subscriptionUserResponseModel.SubscriptionProfiles = GetSubscriptionsProfilesAttachedToSubscription(subscriptionId);

                // Get the selected items 
                subscriptionUserResponseModel.SubscriptionIncludedItems = GetSubscriptionsIncludedItems(planId);

                return subscriptionUserResponseModel;
            }
        }

        /// <summary>
        /// Returns the profiles attached to a subscription by the subscription id.
        /// </summary>
        /// <param name="subscriptionId">The id of the subscription.</param>
        /// <returns>Returns the subscription profiles list.</returns>
        public List<SubscriptionProfileModel> GetSubscriptionsProfilesAttachedToSubscription(string subscriptionId)
        {
            return new SubscriptionProfileDAO().Read(_connectionStringRead, new SubscriptionProfileModel
            {
                SubscriptionId = subscriptionId
            }, false, false);
        }

        /// <summary>
        /// To get the subscriptions of the user.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the subscriptions list of the user.</returns>
        public List<SubscriptionUserResponseModel> GetCustomerSubscriptionsByUserId(string userId)
        {
            // First we get the customer platform id by the user id 
            string customerPlatformId = GetCustomerPlatformId(userId);

            if (customerPlatformId == null)
            {
                return new List<SubscriptionUserResponseModel>();
            }

            // Now we get the user subscriptions 
            List<SubscriptionModel> userSubscriptions = userSubscriptions = new SubscriptionDAO().Read(_connectionStringRead, new SubscriptionModel
            {
                CustomerPlatformId = customerPlatformId
            }, false, false);

            // Now, we search the plan information 
            List<string> planPlatformList = new List<string>();
            foreach (SubscriptionModel subscription in userSubscriptions)
            {
                planPlatformList.Add(subscription.PlanPlatformId);
            }

            // Now we search the plans according to the plan platform list
            List<PlanModel> planModelsList = new PlansReaderManager(_connectionStringRead, _langCode).GetPlansByPlanPlaformIds(planPlatformList);

            // And now we get the planDetails 
            List<string> planIdList = new List<string>();

            for (int i = 0; i < planModelsList.Count; i++)
            {
                planIdList.Add(planModelsList[i].Id);
            }

            // We search the language 
            List<PlanLangModel> planLangModels = new PlansReaderManager(_connectionStringRead, _langCode).GetPlanDescriptionsByLanguageAndPlanIdList(planIdList, true);

            // Now we connect everything 
            List<SubscriptionUserResponseModel> subscriptionUserResponseModels = new List<SubscriptionUserResponseModel>();
            foreach (SubscriptionModel subscription in userSubscriptions)
            {
                // Prepare the plan we want to add 
                PlanModel planToAdd = planModelsList.Find(x => x.PlatformId == subscription.PlanPlatformId);
                PlanResponseModel planResponseModel = new PlanResponseModel
                {
                    Amount = planToAdd.Amount,
                    Available = planToAdd.Available,
                    CreatedAt = planToAdd.CreatedAt,
                    Currency = planToAdd.Currency,
                    Id = planToAdd.Id,
                    Interval = planToAdd.Interval,
                    IsForSchool = planToAdd.IsForSchool,
                    IsPrivate = planToAdd.IsPrivate,
                    Name = planToAdd.Name,
                    PlanDescriptionByLang = planLangModels.FindAll(x => x.PlanId == planToAdd.Id)
                };

                SubscriptionUserResponseModel subscriptionUserResponseModel = new SubscriptionUserResponseModel
                {
                    SubscriptionId = subscription.Id,
                    Quantity = subscription.Quantity,
                    CurrentPeriodEnd = subscription.CurrentPeriodEnd,
                    Status = subscription.Status,
                    Plan = planResponseModel,
                    ProfilesTotalQuantity = (int.Parse(planToAdd.ProfilesQuantity) * int.Parse(subscription.Quantity)).ToString(),
                    CreatedDate = subscription.CreatedAt
                };

                subscriptionUserResponseModels.Add(subscriptionUserResponseModel);
            }
            return subscriptionUserResponseModels;
        }

        /// <summary>
        /// To get the general subscriptions details of a client. (Use this just for clients)
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>Returns the subscription general details.</returns>
        public List<SubscriptionModel> GetClientGeneralSubscriptions(string userId)
        {
            string customerPlatformId = GetCustomerPlatformId(userId);
            if (string.IsNullOrEmpty(customerPlatformId))
            {
                return new List<SubscriptionModel>();
            }
            // First we retrieve the subscriptions of the customer
            return new SubscriptionDAO().Read(_connectionStringRead, new SubscriptionModel
            {
                CustomerPlatformId = customerPlatformId
            }, false, false);

        }

        /// <summary>
        /// Get the items that the user can use in their susbcriptions.
        /// </summary>
        /// <param name="isMainAccount">If is the main account, send this as true, if it's a profile (student) send it false.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>Returns a list of the items that are included in the subscription of the user.</returns>
        public List<SubscriptionProfileSelectedItemResponseModel> GetAvailableItemsBySubscriptions(bool isMainAccount, string userId)
        {
            List<SubscriptionModel> subscriptionModelResultList = new List<SubscriptionModel>();
            if (isMainAccount) // If is the main account, read according to the customer id 
            {
                // Get customer platform id 
                string customerPlatformId = GetCustomerPlatformId(userId);

                // First we retrieve the subscriptions of the customer
                subscriptionModelResultList = new SubscriptionDAO().Read(_connectionStringRead, new SubscriptionModel
                {
                    CustomerPlatformId = customerPlatformId
                }, false, false);
            }
            else // If the user is a subscribed profile instead of the main account, read this 
            {
                // First we retrieve the subscriptions of the profile. 
                List<SubscriptionProfileModel> subscriptionProfileModels = new SubscriptionProfileDAO().Read(_connectionStringRead, new SubscriptionProfileModel
                {
                    ProfileId = userId
                }, false, false);

                if(subscriptionProfileModels.Count == 0) // If there is no subscriptions attached, just returned an empty list 
                    return new List<SubscriptionProfileSelectedItemResponseModel>();

                // Second, we find the product (plans) of each subscriptions 
                // a. We find the subscriptions plans 
                List<SubscriptionModel> subscriptionModelList = new List<SubscriptionModel>();

                // Now we search the subscriptions products 
                for (int i = 0; i < subscriptionProfileModels.Count; i++)
                {
                    subscriptionModelList.Add(new SubscriptionModel { Id = subscriptionProfileModels[i].SubscriptionId });
                }
                // Get all the subscriptions details 
                subscriptionModelResultList = new SubscriptionDAO().Read(_connectionStringRead, subscriptionModelList);
            }

            if (subscriptionModelResultList.Count == 0) // If there is no subscriptions attached, just returned an empty list 
                return new List<SubscriptionProfileSelectedItemResponseModel>();

            // Else, continue evaluating the subscriptions 

            // b. We find the plan details of the active subscriptions only
            List<PlanModel> planModelToFind = new List<PlanModel>();
            DateTimeOffset dateTimeOffsetNow = DateTimeOffset.Now;
            List<SubscriptionModel> subscriptionFilteredModelList = new List<SubscriptionModel>();
            for (int i = 0; i < subscriptionModelResultList.Count; i++)
            {
                // Evaluate only the active plans (the date must be on the current period and the status must be active or trial 
                if (dateTimeOffsetNow < DateTimeOffset.Parse(subscriptionModelResultList[i].CurrentPeriodEnd) && (subscriptionModelResultList[i].Status == "active" || subscriptionModelResultList[i].Status == "trialing"))
                {
                    planModelToFind.Add(new PlanModel { PlatformId = subscriptionModelResultList[i].PlanPlatformId });
                    subscriptionFilteredModelList.Add(subscriptionModelResultList[i]);
                }
            }

            // If the subscriptions filtered list is empty, return an empty list, because there is nmothing to be shown. 
            if (subscriptionFilteredModelList.Count == 0)
                return new List<SubscriptionProfileSelectedItemResponseModel>();

            List<PlanModel> planModelResult = new PlanDAO().Read(_connectionStringRead, planModelToFind);

            if (planModelResult.Count == 0) // Return an empty list because the user doesn't have any active subscriptions 
                return new List<SubscriptionProfileSelectedItemResponseModel>();

            // Third, we find the product details 
            List<ProductItemProductModel> productItemProductModelsToFindList = new List<ProductItemProductModel>();
            for (int i = 0; i < planModelResult.Count; i++)
            {
                if (!productItemProductModelsToFindList.Any(x => x.ProductId == planModelResult[i].ProductId)) // To avoid the repeated 
                {
                    productItemProductModelsToFindList.Add(new ProductItemProductModel
                    {
                        ProductId = planModelResult[i].ProductId,
                        Available = "1" // Call only the availables
                    });
                }
            }

            List<ProductItemProductModel> productItemProductModelResultList = new ProductItemProductDAO().Read(_connectionStringRead, productItemProductModelsToFindList);
            if (productItemProductModelResultList.Count == 0)
            {
                return new List<SubscriptionProfileSelectedItemResponseModel>();
            }

            List<ItemProductModel> itemProductModelToFind = new List<ItemProductModel>();

            foreach (ProductItemProductModel product in productItemProductModelResultList)
            {
                itemProductModelToFind.Add(new ItemProductModel()
                {
                    Id = product.ItemId,
                });
            }

            // Get the item products 
            List<ItemProductModel> itemProductModelsResult = new ItemProductDAO().Read(_connectionStringRead, itemProductModelToFind);
            List<ItemResponseModel> itemResponseModelList = new List<ItemResponseModel>();
            List<SubscriptionProfileSelectedItemResponseModel> subscriptionProfileSelectedReturnedList = new List<SubscriptionProfileSelectedItemResponseModel>();
            foreach (ItemProductModel itemProduct in itemProductModelsResult)
            {
                subscriptionProfileSelectedReturnedList.Add(new SubscriptionProfileSelectedItemResponseModel { ItemId = itemProduct.Id, Name = itemProduct.Name });
            }
            return subscriptionProfileSelectedReturnedList;
        }

        /// <summary>
        /// Return the email details. Usually send when a user (client) is subscribed.
        /// </summary>
        /// <param name="code">The code to find.</param>
        /// <returns>Returns the email's details.</returns>
        public EmailDetailModel GetEmailDetail(string code)
        {
            return new EmailDetailDAO().Read(_connectionStringRead, new EmailDetailModel
            {
                Code = code
            }, true, true)?[0];
        }

       /// <summary>
       /// To get the user's id by the customer platform's id.
       /// </summary>
       /// <param name="customerPlatformId">The customer platform's id.</param>
       /// <returns>Returns user's id (string).</returns>
        public string GetUserIdByCustomerPlatformId(string customerPlatformId)
        {
            return new CustomerDAO().Read(_connectionStringRead, new CustomerModel() { PlatformId = customerPlatformId }, true, true)[0].UserId;
        }

       /// <summary>
       /// To retrieve the subscription's profiles.
       /// </summary>
       /// <param name="subscriptionProfilesToFind">The list of subscriptions to find.</param>
       /// <returns>Returns the subscription profiles model.</returns>
        public List<SubscriptionProfileModel> GetSubscriptionProfiles(List<SubscriptionProfileModel> subscriptionProfilesToFind)
        {
            return new SubscriptionProfileDAO().Read(_connectionStringRead, subscriptionProfilesToFind);
        }

        /// <summary>
        /// Returns the main user account subscription by subscription id.
        /// </summary>
        /// <param name="subscriptionId">The subscription ID to check.</param>
        /// <returns>Returns a string of main account user ID.</returns>
        public string GetMainAccountUserIdBySubscriptionId(string subscriptionId)
        {
            SubscriptionModel subscriptionModel = new SubscriptionDAO().Read(_connectionStringRead, new List<SubscriptionModel>()
            {
                new SubscriptionModel()
                {
                    Id = subscriptionId
                }
            })[0];

            CustomerModel customerModel = new CustomerDAO().Read(_connectionStringRead, new List<CustomerModel>()
            {
                new CustomerModel()
                {
                    PlatformId = subscriptionModel.CustomerPlatformId
                }
            })[0];

            return customerModel.UserId;
        }

        /// <summary>
        /// Check if user can handle subscription.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="subscriptionId">The subscription's id.</param>
        /// <returns>Returns true if user can handle the subscription, otherwise returns false.</returns>
        public bool CheckIfUserCanHandleSubscription(string userId, string subscriptionId)
        {
            string customerPlatformId = GetCustomerPlatformId(userId);
            if (string.IsNullOrWhiteSpace(customerPlatformId))
            {
                return false;
            }

            // If more han 0, can be handled.
            return new SubscriptionDAO().Read(_connectionStringRead, new SubscriptionModel()
            {
                Id = subscriptionId,
                CustomerPlatformId = customerPlatformId,
                Status = "active",
            }, true, true).Count > 0;
        }

        /// <summary>
        /// To read the cancellation reasons' models.
        /// </summary>
        /// <param name="cancellationReasonModels">Cancellation reason model list to filter.</param>
        /// <returns>Returnes the cancellations list.</returns>
        public List<CancellationReasonModel> GetCancellationResons(List<CancellationReasonModel> cancellationReasonModels)
        {
            return new CancellationReasonDAO().Read(_connectionStringRead, cancellationReasonModels);
        }

        /// <summary>
        /// To check if the student is a homeschooler based on their subscriptions.
        /// </summary>
        /// <param name="userId">The id of the user to check.</param>
        /// <returns>Returns true if the student is a homeschooler.</returns>
        public bool IsStudentHomeschool(string userId)
        {
            // Check the subscriptions of the user. 
            var userSubscriptions = new SubscriptionProfileDAO()
                .Read(_connectionStringRead, new SubscriptionProfileModel
                {
                    ProfileId = userId
                }, false, false);

            if (userSubscriptions.Count == 0)
                return false; // If there are no subscriptions, the user is not a homeschooler

            // Check if the user has a subscription with a homeschool plan
            var subscriptionsToFind = new List<SubscriptionModel>();
            foreach (var subscription in userSubscriptions)
            {
                subscriptionsToFind.Add(new SubscriptionModel
                {
                    Id = subscription.SubscriptionId,
                    Type = "H" // Homeschool type
                });
            }

            var homeschoolSubscriptions = new SubscriptionDAO()
                .Read(_connectionStringRead, subscriptionsToFind);

            // If there are any homeschool subscriptions, the user is a homeschooler
            return homeschoolSubscriptions.Count > 0;
        }

        /// <summary>
        /// Returns the items included in the plan of the subscription.
        /// </summary>
        /// <param name="planId">The id of the plan.</param>
        /// <returns>Returns the item response model with the items included in the plan.</returns>
        protected List<ItemResponseModel> GetSubscriptionsIncludedItems(string planId)
        {
            // First, we search the product id according to the plan, so we can take the items included in the product (plan) 
            List<PlanModel> planModelResult = new PlanDAO().Read(_connectionStringRead, new PlanModel { Id = planId }, true, true);


            if (planModelResult.Count == 0) // Return an empty list because the plan details were not found
                return new List<ItemResponseModel>();

            // Third, we find the product details 
            List<ProductItemProductModel> productItemProductModelsToFindList = new List<ProductItemProductModel>();
            for (int i = 0; i < planModelResult.Count; i++)
            {
                if (!productItemProductModelsToFindList.Any(x => x.ProductId == planModelResult[i].ProductId)) // To avoid the repeated 
                {
                    productItemProductModelsToFindList.Add(new ProductItemProductModel
                    {
                        ProductId = planModelResult[i].ProductId
                    });
                }
            }

            List<ProductItemProductModel> productItemProductModelResultList = new ProductItemProductDAO().Read(_connectionStringRead, productItemProductModelsToFindList);

            List<ItemProductModel> itemProductModelToFind = new List<ItemProductModel>();

            foreach (ProductItemProductModel product in productItemProductModelResultList)
            {
                itemProductModelToFind.Add(new ItemProductModel()
                {
                    Id = product.ItemId,
                });
            }

            // Get the item products 
            List<ItemProductModel> itemProductModelsResult = new ItemProductDAO().Read(_connectionStringRead, itemProductModelToFind);
            List<ItemResponseModel> itemResponseModelList = new List<ItemResponseModel>();
            foreach (ItemProductModel itemProduct in itemProductModelsResult)
            {
                itemResponseModelList.Add(new ItemResponseModel()
                {
                    Id = itemProduct.Id,
                    Available = itemProduct.Available,
                    CreatedAt = itemProduct.CreatedAt,
                    ModifiedBy = itemProduct.ModifiedBy,
                    Name = itemProduct.Name,
                    ProductId = productItemProductModelResultList.Find(x => x.ItemId == itemProduct.Id).ProductId,
                    UpdatedAt = itemProduct.UpdatedAt,
                });
            }

            return itemResponseModelList;
        }
    }
}
