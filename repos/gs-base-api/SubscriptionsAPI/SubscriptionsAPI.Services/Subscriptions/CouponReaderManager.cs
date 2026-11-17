using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;


namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// The coupon reader. This is used only for commercial purposes.
    /// </summary>
    public class CouponReaderManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code.</param>
        public CouponReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

       /// <summary>
       /// Validates the coupon by the code (when the user enters the coupon's code to receive a discount, it should be validated).
       /// </summary>
       /// <param name="coupon">The coupon model to check.</param>
       /// <param name="langCode">The language code of the user.</param>
       /// <returns>Returns a response with the coupon details (if valid or not).</returns>
        public CouponValidationResponseModel ValidateCouponByCode(CouponModel coupon, string langCode)
        {
            List<CouponModel> couponsToValidate = new CouponDAO().Read(_connectionStringRead, new CouponModel()
            {
                Code = coupon.Code,
                IntervalIntended = coupon.IntervalIntended,
                Hidden = "0", // Is not hidden
                Available = "1"
            }, false, false);

            if (couponsToValidate.Count > 0)
            {
                foreach (CouponModel couponInList in couponsToValidate)
                {
                    if (DateTime.Now < DateTime.Parse(couponInList.RedeemBy))
                    {
                        return new CouponValidationResponseModel()
                        {
                            TransactionValid = true,
                            CouponModel = couponInList
                        };
                    }
                }
            }

            // Else, return invalid coupon 
            return new CouponValidationResponseModel()
            {
                TransactionValid = false,
                Messages = new List<string>()
                {
                    // Add coupon error 
                     new MessageManager(
                        AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                        langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_coupon_invalid_code"])
                }
            };
        }

        /// <summary>
        /// Validates if the user can use the coupon again.
        /// </summary>
        /// <param name="customerPlatformId">Customer Platform ID.</param>
        /// <param name="couponPlatformId">Coupon Platform Id.</param>
        /// <param name="langCode">The user's language code.</param>
        /// <returns>Returns if the coupon was used by the user.</returns>
        public CouponValidationResponseModel ValidateIfCustomerCanUserCouponAgain(string
            customerPlatformId, string couponPlatformId, string langCode)
        {
            // Search in the receipts
            List<SubscriptionModel> subscriptionModelsToFindList = new List<SubscriptionModel>();
            subscriptionModelsToFindList = new SubscriptionDAO().Read(_connectionStringRead,
                new SubscriptionModel()
                {
                    CustomerPlatformId = customerPlatformId
                }, false, false);

            // Now find each subscriptions
            List<SubscriptionReceiptModel> subscriptionReceiptModelList = new List<SubscriptionReceiptModel>();
            foreach (SubscriptionModel subscription in subscriptionModelsToFindList)
            {
                subscriptionReceiptModelList.Add(new SubscriptionReceiptModel()
                {
                    SubscriptionId = subscription.Id,
                    CouponId = couponPlatformId
                });
            }

            List<SubscriptionReceiptModel> subsccriptionReceiptModelResultList = new List<SubscriptionReceiptModel>();
            subsccriptionReceiptModelResultList = new SubscriptionReceiptDAO().Read(_connectionStringRead,
                subscriptionReceiptModelList);

            if (subscriptionReceiptModelList.Count > 0)
            {
                return new CouponValidationResponseModel()
                {
                    TransactionValid = false,
                    Messages = new List<string>()
                    {
                            new MessageManager(
                                AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                                langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_coupon_cannot_be_used_twice"])
                    }
                };
            }
            else
                return new CouponValidationResponseModel()
                {
                    TransactionValid = true
                };
        }

        /// <summary>
        /// To return the coupons with advanced filters.
        /// </summary>
        /// <param name="couponRequestModel">The coupons with the parameters. Some contains like (name, code).</param>
        /// <param name="pageFrom">The page to start.</param>
        /// <param name="limiter">How many items per request.</param>
        /// <param name="orderByDate">To order by created date.</param>
        /// <returns>Returns the list of coupons.</returns>
        public CouponSearchResponseModel GetCouponsAdvanced(CouponModel couponRequestModel,int pageFrom, int limiter, 
            bool orderByDate)
        {

            // Prepare the response 
            CouponSearchResponseModel couponsResponseModel = new CouponSearchResponseModel
            {
                Coupons = new List<CouponModel>()
            };

            CouponModel couponModelToSearch = new CouponModel
            {

                Id = !string.IsNullOrWhiteSpace(couponRequestModel.Id) ? couponRequestModel.Id.Trim() : null,
                PlatformId = !string.IsNullOrWhiteSpace(couponRequestModel.PlatformId) ? couponRequestModel.PlatformId.Trim() : null,
                Currency = !string.IsNullOrWhiteSpace(couponRequestModel.Currency) ? couponRequestModel.Currency.Trim() : null,
                Duration = !string.IsNullOrWhiteSpace(couponRequestModel.Duration) ? couponRequestModel.Duration.Trim() : null,
                PercentOff = !string.IsNullOrWhiteSpace(couponRequestModel.PercentOff) ? couponRequestModel.PercentOff.Trim() : null,
                AmountOff = !string.IsNullOrWhiteSpace(couponRequestModel.AmountOff) ? couponRequestModel.AmountOff.Trim() : null,
                Available = couponRequestModel.Available == "1" ? "1" : null,
                Hidden = "0" // Must be always 0, the 1 are the "deleted" ones
            };

            CouponDAO couponDao = new CouponDAO();
            List<CouponModel> couponModelList = new List<CouponModel>();
            couponModelList = couponDao.Read(_connectionStringRead, couponModelToSearch, false, orderByDate);

            if (!orderByDate)
            {
                // Order by desc created date
                couponModelList = couponModelList.OrderByDescending(item => DateTime.Parse(item.CreatedAt)).ToList();
            }

            // Now check the name if not null 
            if (!string.IsNullOrWhiteSpace(couponRequestModel.Name))
            {
                List<CouponModel> couponListFiltered = new List<CouponModel>();
                couponListFiltered = couponModelList.Where(item => item.Name.ToLower().Contains(couponRequestModel.Name.ToLower())).ToList();
                couponModelList = new List<CouponModel>();
                couponModelList = couponListFiltered;
            }

            if (!string.IsNullOrWhiteSpace(couponRequestModel.Code))
            {
                List<CouponModel> couponListFiltered = new List<CouponModel>();
                couponListFiltered = couponModelList.Where(item => item.Code.ToLower().Contains(couponRequestModel.Code.ToLower())).ToList();
                couponModelList = new List<CouponModel>();
                couponModelList = couponListFiltered;
            }

            // If there is a limiter, filter the results. 
            // but first, stablish the page totals 
            couponsResponseModel.PageTotals = int.Parse(Math.Ceiling(decimal.Parse(couponModelList.Count.ToString()) / decimal.Parse(limiter.ToString())).ToString());
            if (limiter > 0)
            {
                // Set the page 
                if (pageFrom == 0)
                    pageFrom = 1;

                int startPage = pageFrom * limiter - limiter;
                int endPage = pageFrom * limiter;

                // If the list is more than the plan model list, set the start page to 0. 
                if (startPage > couponModelList.Count)
                    startPage = 0;

                // If the list is more than the end page, set the end page to the list total 
                if (endPage > couponModelList.Count)
                    endPage = couponModelList.Count;

                // Filter by limit 
                List<CouponModel> couponFilteredList = new List<CouponModel>();

                for (int i = startPage; i < endPage; i++)
                {
                    couponFilteredList.Add(couponModelList[i]);
                }

                // Assign the main list to be filtered 
                couponModelList = couponFilteredList;
            }

            if (couponModelList.Count == 0)
            {
                // return no results found
                couponsResponseModel.PageTotals = 0;
                return couponsResponseModel;
            }

            for (int i = 0; i < couponModelList.Count; i++)
            {
                couponsResponseModel.Coupons.Add(new CouponModel()
                {
                    Id = couponModelList[i].Id,
                    PlatformId = couponModelList[i].PlatformId,
                    Name = couponModelList[i].Name,
                    Currency = couponModelList[i].Currency,
                    AmountOff = couponModelList[i].AmountOff,
                    Code = couponModelList[i].Code,
                    Duration = couponModelList[i].Duration,
                    DurationInMonths = couponModelList[i].DurationInMonths,
                    Hidden = couponModelList[i].Hidden,
                    MaxRedemptions = couponModelList[i].MaxRedemptions,
                    PercentOff = couponModelList[i].PercentOff,
                    IntervalIntended = couponModelList[i]?.IntervalIntended,
                    RedeemBy = couponModelList[i].RedeemBy,
                    Available = couponModelList[i].Available,
                    ModifiedBy = couponModelList[i].ModifiedBy,
                    CreatedAt = couponModelList[i].CreatedAt,
                    UpdatedAt = couponModelList[i].UpdatedAt
                });
            }
            return couponsResponseModel;
        }

        /// <summary>
        /// Validates the coupon (if stills active, etc) by the platform id.
        /// </summary>
        /// <param name="couponPlatformId">The coupon platform id to validate.</param>
        /// <returns>Returns the coupon model if valid, else null.</returns>
        public CouponModel ValidateCoupon(string couponPlatformId)
        {
            List<CouponModel> couponsToValidate = new CouponDAO().Read(_connectionStringRead, new CouponModel()
            {
                PlatformId = couponPlatformId,
                Hidden = "0", // Is not hidden
                Available = "1"
            }, false, false);

            for (int i = 0; i < couponsToValidate.Count; i++)
            {
                if (DateTime.Now < DateTime.Parse(couponsToValidate[i].RedeemBy))
                    return couponsToValidate[i];

            }
            return null;
        }
    }
}
