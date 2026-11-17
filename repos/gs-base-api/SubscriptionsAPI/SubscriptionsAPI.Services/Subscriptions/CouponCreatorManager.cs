using System;
using System.Collections.Generic;
using Stripe;
using System.Configuration;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// To handle the coupons. This is used only for commercial purposes.
    /// </summary>
    public class CouponCreatorManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language code.</param>
        public CouponCreatorManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Create or "Updates" a coupon. A coupon cannot be updated because it will 
        /// change the values on Stripe. So instead, we made a fake "Update" creating one
        /// with the same code and possible the same values (instead the ones that are changing) 
        /// </summary>
        /// <param name="couponModel">The model of the coupon</param>
        /// <param name="stripeApiKey">The stripe API Key.</param>
        /// <returns>Returns the transaction details.</returns>
        public ResponseModel CreateCoupon(CouponModel couponModel, string stripeApiKey)
        {
            List<string> errorCodesList = new List<string>();
            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);
            ResponseModel responseModel = new ResponseModel
            {
                TransactionValid = true
            };

            // First, validate the code 

            // THe only change allowed to change to an existing coupon is the redeemby
            if (string.IsNullOrWhiteSpace(couponModel.Id))
            {
                if (string.IsNullOrWhiteSpace(couponModel.AmountOff) && string.IsNullOrWhiteSpace(couponModel.PercentOff))
                {
                    // Amount or percent off are required
                    responseModel.TransactionValid = false;
                    errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_amount_percent_required"]);
                }

                if (!string.IsNullOrWhiteSpace(couponModel.PercentOff))
                {
                    try
                    {
                        int parsePercentInt = int.Parse(couponModel.PercentOff);
                    }
                    catch
                    {
                        errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_percent_invalid"]);
                        responseModel.TransactionValid = false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(couponModel.AmountOff))
                {
                    try
                    {
                        int parsePercentInt = int.Parse(couponModel.AmountOff);
                    }
                    catch
                    {
                        errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_amount_invalid"]);
                        responseModel.TransactionValid = false;
                    }
                }

                if (string.IsNullOrWhiteSpace(couponModel.Duration))
                {
                    // Duration is required
                    responseModel.TransactionValid = false;
                    errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_duration_required"]);
                }

                if (string.IsNullOrWhiteSpace(couponModel.Name))
                {
                    // Name is required for reference
                    responseModel.TransactionValid = false;
                    errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_name_required"]);
                }

                if (!string.IsNullOrWhiteSpace(couponModel.AmountOff) && string.IsNullOrWhiteSpace(couponModel.Currency))
                {
                    // Currency is required for the amount
                    responseModel.TransactionValid = false;
                    errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_currency_amount_required"]);
                }


                if (string.IsNullOrWhiteSpace(couponModel.Code))
                {
                    // The code is required
                    responseModel.TransactionValid = false;
                    errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_code_required"]);
                }
            }

            if (!string.IsNullOrWhiteSpace(couponModel.RedeemBy))
            {
                // Try to parse as timestamp 
                try
                {
                    DateTime.Parse(couponModel.RedeemBy);

                }
                catch (Exception)
                {
                    // Show error 
                    responseModel.TransactionValid = false;
                    errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_date_by"]);
                }
            }
            else
            {
                responseModel.TransactionValid = false;
                errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_date_by"]);
            }

            if (string.IsNullOrWhiteSpace(couponModel.IntervalIntended))
            {
                responseModel.TransactionValid = false;
                errorCodesList.Add(AppConfiguration.Configuration["AppSettings:subr_message_error_interval_time_required"]);
            }

            if (!responseModel.TransactionValid)
            {
                // Return errors 
                responseModel.Messages = messageManager.GetMessageDetails(errorCodesList);
                return responseModel;
            }

            // Else, continue checking
            // Check if the coupon code already exists 
            List<CouponModel> couponsWithTheSameCode = new CouponDAO().Read(_connectionStringRead, new CouponModel() { Code = couponModel.Code }, false, false);

            // If exists, cancel the old coupon  (set hidden on) 
            if (couponsWithTheSameCode.Count > 0)
            {
                List<CouponModel> couponsToUpdate = new List<CouponModel>();
                List<CouponModel> couponsWhere = new List<CouponModel>();
                for (int i = 0; i < couponsWithTheSameCode.Count; i++)
                {
                    couponsToUpdate.Add(new CouponModel()
                    {
                        Hidden = "1",
                        Available = "0"
                    });
                    couponsWhere.Add(new CouponModel()
                    {
                        Id = couponsWithTheSameCode[i].Id
                    });
                }
                new CouponDAO().Update(_connectionString, couponsToUpdate, couponsWhere);
            }

            // Now create the new one
            CreateCouponStripe(couponModel, stripeApiKey);
            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_coupon_code"]));
            return responseModel;
        }

        
        /// <summary>
        /// Creates the coupon in Stripe
        /// </summary>
        /// <param name="couponModel">The coupon model</param>
        /// <param name="stripeApiKey">The stripe api Key</param>
        /// <returns></returns>
        public ResponseModel CreateCouponStripe(CouponModel couponModel, string stripeApiKey)
        {
            // Add the new coupon 
            StripeConfiguration.ApiKey = stripeApiKey;
            couponModel.PlatformId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + "_" + couponModel.Code;
            couponModel.Hidden = "0";
            var options = new CouponCreateOptions
            {
                Id = couponModel.PlatformId,
                Duration = couponModel.Duration
            };

            if (!string.IsNullOrWhiteSpace(couponModel.PercentOff))
                options.PercentOff = decimal.Parse(couponModel.PercentOff);

            if (!string.IsNullOrWhiteSpace(couponModel.AmountOff))
                options.AmountOff = int.Parse(couponModel.AmountOff);

            if (!string.IsNullOrWhiteSpace(couponModel.Currency))
                options.Currency = couponModel.Currency;

            if (!string.IsNullOrWhiteSpace(couponModel.DurationInMonths))
                options.DurationInMonths = int.Parse(couponModel.DurationInMonths);

            if (!string.IsNullOrWhiteSpace(couponModel.MaxRedemptions))
                options.MaxRedemptions = int.Parse(couponModel.MaxRedemptions);

            if (!string.IsNullOrWhiteSpace(couponModel.RedeemBy))
                options.RedeemBy = DateTime.Parse(couponModel.RedeemBy);

            // Create the coupon in Stripe
            var service = new CouponService();
            Coupon coupon = service.Create(options);

            // Create coupon
            new CouponDAO().Create(_connectionString, couponModel);
            return new ResponseModel()
            {
                TransactionValid = true,
                Messages = new List<string>(){
                    new MessageManager(_connectionStringRead, _langCode).
                    GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_success_coupon_code"])
                }
            };
        } 
    }
}
