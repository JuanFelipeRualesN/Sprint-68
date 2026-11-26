using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using Stripe;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// Handles the subscriptions/enrollments. 
    /// </summary>
    public class SubscriptionManager
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
        public SubscriptionManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To create an invoice log. This is used to log the invoices that are created in Stripe.
        /// </summary>
        /// <param name="invoiceLogModel">The invoice log model.</param>
        /// <returns>Returns the invoice log model.</returns>
        public InvoiceLogModel CreateInvoiceLog(InvoiceLogModel invoiceLogModel)
        {
            return new InvoiceLogDAO().Create(_connectionString, invoiceLogModel)?[0];
        }

        /// <summary>
        /// To create a customer.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="userId">The user's id.</param>
        /// <param name="stripeApiKey">The Stripe API Key (not required for non-commercial).</param>
        /// <param name="stripeRequired">true if stripe is required, else false.</param>
        /// <returns>Returns the transaction's details.</returns>
        public CustomerModel CreateCustomer(string email, string userId, string stripeApiKey, bool stripeRequired)
        {
            // Now insert the in the local db to have the relation between stripe customer id and user id 
            CustomerModel customerModel = new CustomerModel
            {
                UserId = userId,
                ModifiedBy = userId

            };
            if (stripeRequired)
            {
                StripeConfiguration.ApiKey = stripeApiKey;
                var options = new CustomerCreateOptions
                {
                    Email = email
                };
                var service = new CustomerService();
                var customer = service.Create(options);
                customerModel.PlatformId = customer.Id;
            }

            if (string.IsNullOrWhiteSpace(customerModel.PlatformId))
            {
                customerModel.PlatformId = Guid.NewGuid().ToString();
            }

            var customerDAO = new CustomerDAO();

            // Check if exists 
            var customerExist = customerDAO.Read(this._connectionStringRead, new CustomerModel()
            {
                UserId = userId,
            }, true, true);
            if (customerExist != null && customerExist.Count > 0)
            {
                customerDAO.Update(this._connectionString, new CustomerModel()
                {
                    PlatformId = customerModel.PlatformId,
                    ModifiedBy = userId
                }, new CustomerModel()
                {
                    UserId = userId
                });

                return customerDAO.Read(this._connectionStringRead, new CustomerModel()
                {
                    UserId = userId,
                }, true, true)[0];
            }

            return customerDAO.Create(_connectionString, customerModel)[0];
        }

        #region CommentedForNonCommercialPurposes
        /// <summary>
        /// Stores the source token for later use in Stripe
        /// </summary>
        /// <param name="email"> The user's email</param>
        /// <param name="stripeApiKey"> The Stripe API Key</param>
        /// <param name="userId"> The user's id of the cient.</param>
        /// <param name="userIdCreatedBy"> The user's id that is creating the token.</param>
        /// <param name="sourceToken">The source Token</param>
        public async Task<ResponseModel> StoreCustomerPaymentToken(string userId, string email, string sourceToken, string stripeApiKey, string userIdCreatedBy)
        {
            var responseModel = new ResponseModel
            {
                TransactionValid = true
            };

            // Read from customer source token DAO to check if it exists
            var customerDetails = new CustomerDAO().Read(_connectionStringRead, new CustomerModel { UserId = userId }, true, true);

            string customerPlatformId;
            // If does not exist, create it
            if (customerDetails == null || customerDetails.Count == 0)
            {
                var customerModel = CreateCustomer(email, userId, stripeApiKey, true);
                if (customerModel == null)
                {
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add("Failed to create customer in Stripe.");
                    return responseModel;
                }
                customerPlatformId = customerModel.PlatformId;
            }
            else
            {
                customerPlatformId = customerDetails[0].PlatformId;
            }

            // Check if the customer exists in Stripe
            StripeConfiguration.ApiKey = stripeApiKey;
            var customerService = new CustomerService();

            try
            {
                await customerService.GetAsync(customerPlatformId);
            }
            catch (StripeException ex)
            {
                // If the customer does not exist in Stripe, create it
                if (ex.StripeError?.Code == "resource_missing")
                {
                    var customerModel = CreateCustomer(email, userId, stripeApiKey, true);
                    if (customerModel == null)
                    {
                        responseModel.TransactionValid = false;
                        responseModel.Messages.Add("Failed to create customer in Stripe.");
                        return responseModel;
                    }
                    customerPlatformId = customerModel.PlatformId;
                }
                else
                {
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add($"Stripe Error: {ex.StripeError.Message}");
                    return responseModel;
                }
            }

            try
            {
                // 1. Detach all existing payment methods (cards) from the customer in Stripe
                var paymentMethodService = new PaymentMethodService();
                var existingPaymentMethods = await paymentMethodService.ListAsync(new PaymentMethodListOptions
                {
                    Customer = customerPlatformId,
                    Type = "card",
                });

                foreach (var pm in existingPaymentMethods)
                {
                    await paymentMethodService.DetachAsync(pm.Id);
                }

                // 2. sourceToken is already a PaymentMethod ID (pm_...), attach it directly
                await paymentMethodService.AttachAsync(sourceToken, new PaymentMethodAttachOptions
                {
                    Customer = customerPlatformId,
                });

                // 3. Set it as the customer's default payment method
                await customerService.UpdateAsync(customerPlatformId, new CustomerUpdateOptions
                {
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = sourceToken,
                    },
                });

                // 4. Sync the token in the local database (delete old, insert new)
                var subscriptionReaderManager = new SubscriptionReadersManager(_connectionString, _langCode);
                var customerSourceTokenModel = subscriptionReaderManager.GetCustomerSourceToken(customerPlatformId);

                var customerSourceTokenDAO = new CustomerSourceTokenDAO();
                if (customerSourceTokenModel != null)
                {
                    customerSourceTokenDAO.Delete(_connectionString, new CustomerSourceTokenModel
                    {
                        CustomerPlatformId = customerPlatformId
                    });
                }

                customerSourceTokenDAO.Create(_connectionString, new CustomerSourceTokenModel
                {
                    CustomerPlatformId = customerPlatformId,
                    Active = "1",
                    SourceToken = sourceToken,
                    ModifiedBy = userIdCreatedBy
                });

                var messageManager = new MessageManager(_connectionStringRead, _langCode);
                responseModel.Messages.Add(messageManager.GetMessageDetails(
                    AppConfiguration.Configuration["AppSettings:subr_message_subscription_payment_updated"]));

                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Messages =
                [
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(
                AppConfiguration.Configuration["AppSettings:subr_message_payment_failed"]),
        ];

                responseModel.TransactionValid = false;
                return responseModel;
            }
        }

        /// <summary>
        /// To create the customer payment method.
        /// </summary>
        /// <param name="customerId">The customer's id.</param>
        /// <param name="paymentMethodId">The payment's method id.</param>
        /// <param name="stripeApiKey">Stripe API Key.</param>
        /// <param name="userIdCreatedBy">The user that is creating the token.</param>
        /// <returns>Return a response with the transaction details.</returns>
        // --- IMPORTANT: This is how your CreateCustomerPaymentToken function SHOULD be implemented ---
        // It must attach the payment method to the customer and set it as default.
        public async Task<SubscriptionUserResponseModel> CreateCustomerPaymentToken(string customerId, string paymentMethodId, string stripeApiKey, string userIdCreatedBy)
        {
            var response = new SubscriptionUserResponseModel { TransactionValid = true, Messages = new List<string>() };
            StripeConfiguration.ApiKey = stripeApiKey;

            try
            {
                var paymentMethodService = new PaymentMethodService();
                var customerService = new CustomerService();

                // 1. Retrieve the Payment Method to verify it and check its current attachment status.
                PaymentMethod paymentMethod = null;
                try
                {
                    paymentMethod = await paymentMethodService.GetAsync(paymentMethodId);
                    Console.WriteLine($"Retrieved Payment Method: {paymentMethod.Id}");
                }
                catch (StripeException pmEx)
                {
                    response.TransactionValid = false;
                    response.Messages.Add($"Invalid Payment Method ID provided: {pmEx.Message}");
                    Console.Error.WriteLine($"Stripe Error retrieving Payment Method {paymentMethodId}: {pmEx.Message}");
                    return response;
                }

                // 2. Check if the Payment Method is already attached to the customer.
                // If it's attached to a *different* customer, you might need to detach it first.
                // For simplicity, we'll only attach if it's not attached to *this* customer.
                if (paymentMethod.CustomerId != customerId)
                {
                    Console.WriteLine($"Payment Method {paymentMethodId} not attached to customer {customerId}. Attaching now...");
                    var attachOptions = new PaymentMethodAttachOptions
                    {
                        Customer = customerId,
                    };
                    try
                    {
                        await paymentMethodService.AttachAsync(paymentMethodId, attachOptions);
                        Console.WriteLine($"Payment Method {paymentMethodId} successfully attached to customer {customerId}.");
                    }
                    catch (StripeException attachEx)
                    {
                        response.TransactionValid = false;
                        response.Messages.Add($"Failed to attach payment method to customer: {attachEx.Message}");
                        Console.Error.WriteLine($"Stripe Error attaching Payment Method {paymentMethodId} to customer {customerId}: {attachEx.Message}");
                        return response;
                    }
                }
                else
                {
                    Console.WriteLine($"Payment Method {paymentMethodId} is already attached to customer {customerId}.");
                }

                // 3. Set this Payment Method as the customer's default for future invoices/subscriptions.
                var customerUpdateOptions = new CustomerUpdateOptions
                {
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = paymentMethodId // Set the attached Payment Method as default
                    }
                };

                await customerService.UpdateAsync(customerId, customerUpdateOptions);
                Console.WriteLine($"Customer {customerId} default payment method set to {paymentMethodId}.");

                response.TransactionValid = true;
                response.Messages.Add("Payment method successfully prepared for customer.");
                return response;
            }
            catch (StripeException ex)
            {
                response.TransactionValid = false;
                response.Messages.Add($"Stripe Error in payment method preparation: {ex.StripeError?.Message ?? ex.Message}");
                Console.Error.WriteLine($"Stripe Exception in CreateCustomerPaymentToken: {ex.ToString()}");
                return response;
            }
            catch (Exception ex)
            {
                response.TransactionValid = false;
                response.Messages.Add($"An unexpected error occurred during payment method preparation: {ex.Message}");
                Console.Error.WriteLine($"General Exception in CreateCustomerPaymentToken: {ex.ToString()}");
                return response;
            }
        }

        /// <summary>
        /// Creates the source token in stripe.
        /// </summary>
        /// <param name="customerPlatformId">The customer platform id.</param>
        /// <param name="newPaymentMethodId">The source token.</param>
        /// <param name="stripeApiKey">The stripe API Key.</param>
        public async Task CreateCustomerSourceTokenStripe(string customerPlatformId, string newPaymentMethodId, string stripeApiKey)
        {
            StripeConfiguration.ApiKey = stripeApiKey;

            try
            {
                // Create services for interacting with Payment Methods and Customers
                var paymentMethodService = new PaymentMethodService();
                var customerService = new CustomerService();

                // 1. Retrieve the Payment Method to ensure it exists and to check its current attachment status.
                PaymentMethod paymentMethod = null;
                try
                {
                    paymentMethod = await paymentMethodService.GetAsync(newPaymentMethodId);
                    Console.WriteLine($"Retrieved Payment Method: {paymentMethod.Id}");
                }
                catch (StripeException pmEx)
                {
                    Console.Error.WriteLine($"Stripe Error retrieving Payment Method {newPaymentMethodId}: {pmEx.Message}");
                    // Log the error and potentially rethrow or return an error status
                    return; // Exit if payment method isn't found
                }

                // 2. Check if the Payment Method is already attached to the customer.
                // A PaymentMethod object has a 'CustomerId' property if it's attached.
                if (paymentMethod.CustomerId != customerPlatformId)
                {
                    Console.WriteLine($"Payment Method {newPaymentMethodId} is not attached to customer {customerPlatformId}. Attaching now...");
                    // 3. Attach the Payment Method to the customer.
                    var attachOptions = new PaymentMethodAttachOptions
                    {
                        Customer = customerPlatformId,
                    };
                    try
                    {
                        await paymentMethodService.AttachAsync(newPaymentMethodId, attachOptions);
                        Console.WriteLine($"Payment Method {newPaymentMethodId} successfully attached to customer {customerPlatformId}.");
                    }
                    catch (StripeException attachEx)
                    {
                        Console.Error.WriteLine($"Stripe Error attaching Payment Method {newPaymentMethodId} to customer {customerPlatformId}: {attachEx.Message}");
                        // If it fails to attach, it might already be attached to a different customer, or other issues.
                        // In a real application, you might want to handle this more gracefully (e.g., detach first if attached to another).
                        return; // Exit if attachment fails
                    }
                }
                else
                {
                    Console.WriteLine($"Payment Method {newPaymentMethodId} is already attached to customer {customerPlatformId}.");
                }

                // 4. Create the update options for the customer to set the new default.
                var customerUpdateOptions = new CustomerUpdateOptions
                {
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = newPaymentMethodId // Set the attached Payment Method as default
                    }
                };

                // 5. Perform the asynchronous update call to the Stripe Customer API.
                Customer updatedCustomer = await customerService.UpdateAsync(customerPlatformId, customerUpdateOptions);

                Console.WriteLine($"Successfully updated customer {customerPlatformId} with new default payment method {newPaymentMethodId}.");
                // You might want to return updatedCustomer or a success indicator here.
            }
            catch (StripeException ex)
            {
                // 6. Handle any Stripe-specific errors.
                Console.Error.WriteLine($"Stripe Error updating customer card: {ex.Message} (Code: {ex.StripeError?.Code})");
                // Log the error details for debugging
            }
            catch (Exception ex)
            {
                // 7. Handle any other unexpected errors.
                Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
                // Log the error details
            }
        }

        /// <summary>
        /// To check if the customer exists in Stripe.
        /// </summary>
        /// <param name="customerPlatformId">The customer platform id.</param>
        /// <param name="stripeApiKey">Stripe API key.</param>
        /// <returns>Returns true if exists, else, false</returns>
        public bool CheckIfCustomerExistsInStripe(string customerPlatformId, string stripeApiKey)
        {
            StripeConfiguration.ApiKey = stripeApiKey;

            try
            {
                var service = new CustomerService();
                Customer customer = service.Get(customerPlatformId);
                return customer != null;
            }
            catch (StripeException ex)
            {
                // If the customer does not exist in Stripe, it will throw an exception
                if (ex.StripeError?.Code == "resource_missing")
                {
                    return false;
                }
                Console.Error.WriteLine($"Stripe Error checking customer existence: {ex.StripeError.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unexpected error occurred while checking customer existence: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// To create the customer in Stripe.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="userId">The user's id.</param>
        /// <param name="stripeKey">The stripe key.</param>
        /// <returns></returns>
        public CustomerModel CreateCustomerInStripe(string email, string userId, string stripeKey)
        {
            // Set your Stripe API key for the current operation.
            // It's good practice to set it per-request or per-service instance if you manage multiple keys.
            StripeConfiguration.ApiKey = stripeKey;

            try
            {
                var service = new CustomerService();

                var options = new CustomerCreateOptions
                {
                    Email = email,
                    // It's highly recommended to store your internal user ID in Stripe's metadata.
                    // This makes it easy to link Stripe customers back to your application's users.
                    Metadata = new Dictionary<string, string>
                    {
                        { "app_userId", userId } // Use a clear key like "app_userId"
                    },
                };

                Customer customer = service.Create(options);

                string stripeCustomerId = customer.Id;

                var customerModel = new CustomerModel()
                {
                    UserId = userId,
                    PlatformId = stripeCustomerId,
                    ModifiedBy = userId,
                };

                // Create it in the customer db. 
                // Check if the customer already exists to avoid duplicates
                var existingCustomers = new CustomerDAO().Read(this._connectionStringRead, new CustomerModel() { UserId = userId }, true, true);

                if (existingCustomers != null && existingCustomers.Count > 0)
                {
                    // Update the existing customer record with the new Stripe Customer ID
                    new CustomerDAO().Update(this._connectionString, new CustomerModel()
                    {
                        PlatformId = stripeCustomerId,
                        ModifiedBy = userId
                    }, new CustomerModel()
                    {
                        UserId = userId
                    });
                    return new CustomerDAO().Read(this._connectionStringRead, new CustomerModel() { UserId = userId }, true, true)[0];
                }
                // If not exists, create it
                new CustomerDAO().Create(this._connectionString, customerModel);

                return customerModel;
            }
            catch (StripeException ex)
            {
                Console.Error.WriteLine($"Stripe Error creating customer: {ex.StripeError.Message}");
                // Log the full exception details for debugging
                // Example: _logger.LogError(ex, "Stripe API error during customer creation for email: {Email}", email);
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unexpected error occurred while creating Stripe customer: {ex.Message}");
                // Log the full exception details
                // Example: _logger.LogError(ex, "Unexpected error during customer creation for email: {Email}", email);
                return null;
            }
        }



        /// <summary>
        /// Creates the subscription in our service and in Stripe
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="planDetails"></param>
        /// <param name="customerToken"></param>
        /// <param name="paymentSourceToken"></param>
        /// <param name="stateIsoCode"></param>
        /// <param name="countryIsoCode"></param>
        /// <param name="quantityOfPlans"></param>
        /// <param name="stripeApiKey"></param>
        /// <param name="userIdCreatedBy"></param>
        /// <param name="couponPlatformId">The coupon of stripe</param>
        /// <param name="subscriptionType">T for tutoring, S for school, H for homeschooling.</param>
        /// <returns></returns>
        public async Task<SubscriptionUserResponseModel> CreateSubscriptionStripeWithoutCountryValidations(
      PlanResponseModel planDetails,
      string customerToken,
      string paymentSourceToken, // This is expected to be a Payment Method ID (pm_...)
      string stateIsoCode,
      string countryIsoCode,
      int quantityOfPlans,
      string stripeApiKey,
      string userIdCreatedBy,
      string couponPlatformId,
      string subscriptionType)
        {
            var messageManager = new MessageManager(_connectionStringRead, _langCode);
            var responseModel = new SubscriptionUserResponseModel
            {
                TransactionValid = true,
                Messages = new List<string>()
            };

            try
            {
                // Step 1: Ensure the payment method is attached to the customer
                // This call is CRITICAL and must perform the attachment logic.
                // It should return true if successful, false otherwise.
                // Make sure CreateCustomerPaymentToken is an async function and you await it.
                var responseSourceTokenCreation = await CreateCustomerPaymentToken(customerToken, paymentSourceToken, stripeApiKey, userIdCreatedBy);

                if (!responseSourceTokenCreation.TransactionValid)
                {
                    responseModel.Messages.AddRange(responseSourceTokenCreation.Messages);
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add("Failed to prepare payment method for customer.");
                    return responseModel;
                }

                StripeConfiguration.ApiKey = stripeApiKey;

                // Step 2: Prepare subscription items
                var items = new List<SubscriptionItemOptions>
            {
                new SubscriptionItemOptions
                {
                    Quantity = quantityOfPlans,
                    Price = planDetails.PlatformId, // Use Price, not Plan (Plan is deprecated)
                }
            };

                // Step 3: Build subscription create options
                var options = new SubscriptionCreateOptions
                {
                    Customer = customerToken,
                    Items = items,
                    CollectionMethod = "charge_automatically",
                    // CRITICAL FIX: Explicitly set the DefaultPaymentMethod for the subscription
                    // This ensures Stripe knows which payment method to use for the initial invoice.
                    DefaultPaymentMethod = paymentSourceToken,
                };

                // Step 3a: Add the logic for applying a coupon
                if (!string.IsNullOrWhiteSpace(couponPlatformId))
                {
                    options.Discounts = new List<SubscriptionDiscountOptions>
                {
                    new SubscriptionDiscountOptions { Coupon = couponPlatformId }
                    // If couponPlatformId is a Promotion Code ID (promo_...), use:
                    // new SubscriptionDiscountOptions { PromotionCode = couponPlatformId }
                };
                }

                // Step 4: Trial period logic
                if (planDetails.TrialDays > 0)
                {
                    options.TrialPeriodDays = planDetails.TrialDays;
                }

                var service = new SubscriptionService();
                // Create the subscription asynchronously
                Subscription subscription = await service.CreateAsync(options);

                if (subscription.Status == "active" || subscription.Status == "trialing")
                {
                    var firstSubscriptionItem = subscription.Items?.Data?.FirstOrDefault();

                    // Convert Unix timestamp (long) to DateTimeOffset, then to DateTime
                    DateTime? currentPeriodEndDateTime = null;
                    if (firstSubscriptionItem != null && firstSubscriptionItem.CurrentPeriodEnd != null)
                    {
                        currentPeriodEndDateTime = firstSubscriptionItem.CurrentPeriodEnd;
                    }

                    // Step 5: Save to DB
                    var subscriptionModelToCreate = new SubscriptionModel
                    {
                        CurrentPeriodEnd = currentPeriodEndDateTime?.ToString(), // Use null-conditional operator
                        PlatformId = subscription.Id,
                        PlatformCode = "STR",
                        CustomerPlatformId = customerToken,
                        Status = subscription.Status,
                        Quantity = quantityOfPlans.ToString(),
                        PlanPlatformId = firstSubscriptionItem?.Price?.Id,
                        ModifiedBy = userIdCreatedBy,
                        Type = subscriptionType
                    };

                    var subscriptionDAO = new SubscriptionDAO();
                    SubscriptionModel subscriptionModelInserted = subscriptionDAO.Create(_connectionString, subscriptionModelToCreate)[0];
                    responseModel.SubscriptionId = subscriptionModelInserted.Id;
                    subscriptionModelInserted.Valid = true;

                    try
                    {
                        // Step 6: Generate receipt
                        PlansReaderManager plansReaderManager = new PlansReaderManager(_connectionStringRead, _langCode);

                        PlanCostResponseModel planCostResponseModel =
                            plansReaderManager.CalculatePlanCost(planDetails.Id, false, true, quantityOfPlans, couponPlatformId, stateIsoCode, countryIsoCode);

                        SubscriptionReceiptModel subscriptionReceiptModel = new SubscriptionReceiptModel
                        {
                            SubscriptionId = subscriptionModelInserted?.Id,
                            SubTotal = planCostResponseModel?.SubTotal,
                            TaxTotal = planCostResponseModel?.TaxTotal,
                            Total = planCostResponseModel?.TotalCost,
                            Currency = planCostResponseModel?.Currency,
                            TrialDays = planDetails.TrialDays.ToString(),
                            ModifiedBy = userIdCreatedBy,
                            CouponId = !string.IsNullOrWhiteSpace(couponPlatformId) ? couponPlatformId : null
                        };

                        SubscriptionReceiptDAO subscriptionReceiptDAO = new SubscriptionReceiptDAO();
                        SubscriptionReceiptModel subscriptionReceiptModelInserted = subscriptionReceiptDAO.Create(_connectionString, subscriptionReceiptModel)[0];

                        // Step 7: Insert tax records if present
                        if (planCostResponseModel?.Taxes != null && planCostResponseModel.Taxes.Count > 0)
                        {
                            var subscriptionReceiptTaxModelList = new List<SubscriptionReceiptTaxModel>();
                            SubscriptionReceiptTaxDAO subscriptionReceiptTaxDAO = new SubscriptionReceiptTaxDAO();

                            foreach (var tax in planCostResponseModel.Taxes)
                            {
                                subscriptionReceiptTaxModelList.Add(new SubscriptionReceiptTaxModel
                                {
                                    SubscriptionReceiptId = subscriptionReceiptModelInserted.Id,
                                    TaxId = tax.Id,
                                    TaxName = tax.TaxName,
                                    TaxPercent = tax.Percent,
                                    ModifiedBy = userIdCreatedBy
                                });
                            }
                            subscriptionReceiptTaxDAO.Create(_connectionString, subscriptionReceiptTaxModelList);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error generating receipt: {ex.Message}");
                        // Decide if this error should invalidate the whole transaction or just be logged
                    }

                    responseModel.TransactionValid = true;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_subscription_creation_success"]));
                    return responseModel;
                }
                else
                {
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add("Subscription could not be created or is not active/trialing.");
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_subscription_creation_error"]));
                    return responseModel;
                }
            }
            catch (StripeException ex)
            {
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails($"Stripe Error: {ex.StripeError?.Message ?? ex.Message} (Code: {ex.StripeError?.Code})"));
                Console.Error.WriteLine($"Stripe Exception in CreateSubscriptionStripeWithoutCountryValidations: {ex.ToString()}");
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails($"An unexpected error occurred: {ex.Message}"));
                Console.Error.WriteLine($"General Exception in CreateSubscriptionStripeWithoutCountryValidations: {ex.ToString()}");
                return responseModel;
            }
        }

        #endregion CommentedForNonCommercialPurposes

        /// <summary>
        /// To create a subscription/enrollment. This is required and it is the core, to allow the profiles (students, teachers and principal), to 
        /// access to the academic content.
        /// </summary>
        /// <param name="userId">The user id (client).</param>
        /// <param name="sourceToken">The source token is when the user creates the credit card and it's stored in Stripe. Not required for non-commercial. </param>
        /// <param name="planId">The plan's id to subscribe the user.</param>
        /// <param name="quantityOfPlans">The quantity of those plans that the client is acquiring. </param>
        /// <param name="stripeApiKey">The stripe api key. Not required for non-commercial.</param>
        /// <param name="countryIsoCode">The user country ISO code to check the taxes of the country</param>
        /// <param name="stateIsoCode">The user state ISO code to check the there are taxes for the sate.</param>
        /// <param name="couponPlatformId">The coupon platform id. Not used in non-commercial.</param>
        /// <param name="currentPeriodEnd">When the subscription finalizes.</param>
        /// <param name="platformCode">The plaform code that will process the subscription. PRI for private, STR for Stripe..</param>
        /// <param name="subscriptionType">T for tutoring, S for school, H for homeschooling.</param>
        /// <param name="userIdCreatedBy">The user that is making the request.</param>
        /// <returns>Returns if the subscription was created or not</returns>
        public async Task<SubscriptionUserResponseModel> CreateUserSubscription(string userId,
            string sourceToken, string planId, int quantityOfPlans, string stateIsoCode, string countryIsoCode,
            string stripeApiKey, string userIdCreatedBy, string couponPlatformId, string platformCode, string currentPeriodEnd, string subscriptionType)
        {
            var messageManager = new MessageManager(_connectionStringRead, _langCode);
            var responseModel = new SubscriptionUserResponseModel
            {
                TransactionValid = true
            };

            // First, we get the 
            var subscriptionReadersManager = new PlansReaderManager(_connectionStringRead, _langCode);
            var subscriptionReadersManagerSubs = new SubscriptionReadersManager(_connectionStringRead, _langCode);

            var customerPlatformId = subscriptionReadersManagerSubs.GetCustomerPlatformId(userId);

            // If there is no platform id, then it is an invalid registration. Send the error. 
            if (string.IsNullOrWhiteSpace(customerPlatformId))
            {
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_registration_process_incorrect"]));
                return responseModel;
            }


            // Second check if the plan response is available
            var planResponseModel = subscriptionReadersManager.GetPlanDetails(planId, true, true);
            // If the plan is not available
            if (planResponseModel == null || planResponseModel.Available != "1")
            {
                // Return that the plan is not available 
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_plan_retrieve"]));
                return responseModel;
            }

            // Check if Special Promo 
            if (planResponseModel.IsSpecialPromo == "1" && int.Parse(planResponseModel.SpecialPromoLimit) > 0)
            {
                // Check if special promo limit has been reached
                if (new SubscriptionDAO().Read(_connectionStringRead, new SubscriptionModel() { PlanPlatformId = planResponseModel.PlatformId }, false, false).Count > int.Parse(planResponseModel.SpecialPromoLimit))
                {
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_error_plan_max_limit_reached"]));
                }
            }

            if (platformCode == "STR" && int.Parse(planResponseModel.Amount) > 0)
            {
                return await CreateSubscriptionStripeWithoutCountryValidations(
                    planResponseModel, customerPlatformId, sourceToken,
                    stateIsoCode, countryIsoCode, quantityOfPlans, stripeApiKey,
                    userIdCreatedBy, couponPlatformId, subscriptionType);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(currentPeriodEnd))
                {

                    if (string.IsNullOrWhiteSpace(planResponseModel.SpecialPromoDays) || int.Parse(planResponseModel.SpecialPromoDays) == 0)
                    {
                        // Return invalid 
                        responseModel.TransactionValid = false;
                        responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_period_time_required"]));
                        return responseModel;
                    }
                    else
                    {
                        // Set the current Period End to the promo total days. 
                        currentPeriodEnd = DateTime.Now.AddDays(int.Parse(planResponseModel.SpecialPromoDays)).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                }
                return CreateSubscriptionPrivate(planResponseModel, customerPlatformId, quantityOfPlans, userIdCreatedBy, currentPeriodEnd, subscriptionType);
            }
        }

        /// <summary>
        /// To create a private subscription.
        /// </summary>
        /// <param name="currentPeriodEnd">The date when the period of subscription is expiring.</param>
        /// <param name="customerToken">The customer's token.</param>
        /// <param name="planDetails">The details of the plan.</param>
        /// <param name="quantityOfPlans">The quantity of plans that the user is acquiring.</param>
        /// <param name="subscriptionType">T for tutoring, S for school, H for homeschooling.</param>
        /// <param name="userIdCreatedBy">The user's id that is making the request.</param>
        /// <returns>Returns the transaction's details.</returns>
        public SubscriptionUserResponseModel CreateSubscriptionPrivate(
            PlanResponseModel planDetails, string customerToken,
            int quantityOfPlans,
            string userIdCreatedBy,
            string currentPeriodEnd,
            string subscriptionType)
        {
            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);
            SubscriptionUserResponseModel responseModel = new SubscriptionUserResponseModel
            {
                TransactionValid = true
            };

            PlansReaderManager plansReaderManager = new PlansReaderManager(_connectionString, _langCode);

            // Modify date 
            DateTime dateDT = DateTime.Parse(currentPeriodEnd);
            string finalDate = dateDT.ToString("M/d/yyyy", CultureInfo.InvariantCulture) + " 11:59:59 PM";

            // Now store the service in our database 
            SubscriptionModel subscriptionModelToCreate = new SubscriptionModel
            {
                CurrentPeriodEnd = finalDate,
                PlatformId = "subgs_" + CodeBuilder.GenerateCode(),
                PlatformCode = "PRI",
                CustomerPlatformId = customerToken,
                Status = "active",
                Quantity = quantityOfPlans.ToString(), // How many quantities the user bought of that plan
                PlanPlatformId = planDetails.PlatformId, // The id of the plan in Stripe
                ModifiedBy = userIdCreatedBy,
                Type = subscriptionType
            };
            SubscriptionDAO subscriptionDAO = new SubscriptionDAO();

            SubscriptionModel subscriptionModelInserted =
                subscriptionDAO.Create(_connectionString, subscriptionModelToCreate)[0];
            responseModel.SubscriptionId = subscriptionModelInserted.Id;
            subscriptionModelInserted.Valid = true;

            // Create receipt 
            PlanCostResponseModel planCostResponseModel =
            plansReaderManager.CalculatePlanCost(planDetails.Id, false, true, quantityOfPlans, null);

            SubscriptionReceiptModel subscriptionReceiptModel = new SubscriptionReceiptModel
            {
                SubscriptionId = subscriptionModelInserted?.Id,
                SubTotal = planCostResponseModel?.SubTotal,
                TaxTotal = planCostResponseModel?.TaxTotal,
                Total = planCostResponseModel?.TotalCost,
                Currency = planCostResponseModel?.Currency,
                TrialDays = "0",
                ModifiedBy = userIdCreatedBy
            };
            SubscriptionReceiptDAO subscriptionReceiptDAO = new SubscriptionReceiptDAO();
            SubscriptionReceiptModel subscriptionReceiptModelInserted = subscriptionReceiptDAO.Create(_connectionString, subscriptionReceiptModel)[0];

            // Now insert the taxes receipt if applies 
            if (planCostResponseModel.Taxes != null && planCostResponseModel.Taxes?.Count > 0)
            {
                List<SubscriptionReceiptTaxModel> subscriptionReceiptTaxModelList = new List<SubscriptionReceiptTaxModel>();
                SubscriptionReceiptTaxDAO subscriptionReceiptTaxDAO = new SubscriptionReceiptTaxDAO();

                for (int i = 0; i < planCostResponseModel.Taxes.Count; i++)
                {
                    SubscriptionReceiptTaxModel subscriptionReceiptTaxModel = new SubscriptionReceiptTaxModel
                    {
                        SubscriptionReceiptId = subscriptionReceiptModelInserted.Id,
                        TaxId = planCostResponseModel.Taxes[i].Id,
                        TaxName = planCostResponseModel.Taxes[i].TaxName,
                        TaxPercent = planCostResponseModel.Taxes[i].Percent,
                        ModifiedBy = userIdCreatedBy
                    };
                    subscriptionReceiptTaxModelList.Add(subscriptionReceiptTaxModel);
                }
                subscriptionReceiptTaxDAO.Create(_connectionString, subscriptionReceiptTaxModelList);
            }

            responseModel.TransactionValid = true;
            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_subscription_creation_success"]));
            return responseModel;
        }

        /// <summary>
        /// Assign the profiles to subscription.
        /// </summary>
        /// <param name="subscriptionId">The subscription id (the id of our base).</param>
        /// <param name="profileIds">The profile ids we want to enroll.</param>
        /// <param name="quantityPrevalidated">If the quantity was prevalidated already and we do not need to check the user quantity to allow registration.</param>
        /// <param name="userSessionId">User session id.</param>
        /// <returns>Returns a response to the user if the transaction was sucessful or not and the error type.</returns>
        public ResponseModel AssignProfilesToSubscription(string subscriptionId, List<string> profileIds, string userSessionId, bool quantityPrevalidated)
        {
            var responseModel = new ResponseModel();
            var messageManager = new MessageManager(_connectionStringRead, _langCode);

            var subscriptionReadersManager = new SubscriptionReadersManager(_connectionStringRead, _langCode);
            // Get the subscription details 
            var subscriptionResponseModel = subscriptionReadersManager.GetSubscriptionDetails(subscriptionId);

            if (subscriptionResponseModel == null) // If the subscription doesn't exists
            {
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_subscription_not_valid"]));
                return responseModel;
            }

            // Evaluate if the profiles ids are distintcs for the subscriptions 
            var profilesToAdd = new List<SubscriptionProfileModel>();
            var profilesSame = new List<SubscriptionProfileModel>();
            var profilesToDelete = new List<SubscriptionProfileModel>();

            // We evaluate the profile ids
            for (int i = 0; i < profileIds.Count; i++)
            {
                var subscriptionProfileModel = subscriptionResponseModel.SubscriptionProfiles.Find(x => x.ProfileId == profileIds[i]);
                if (subscriptionProfileModel != null)
                    profilesSame.Add(subscriptionProfileModel);
                else
                {
                    subscriptionProfileModel = new SubscriptionProfileModel
                    {
                        SubscriptionId = subscriptionId,
                        ProfileId = profileIds[i],
                        ModifiedBy = userSessionId
                    };

                    profilesToAdd.Add(subscriptionProfileModel);
                }
            }

            // Now check if we need to delete something 
            for (int i = 0; i < subscriptionResponseModel.SubscriptionProfiles.Count; i++)
            {
                string profileId = profileIds.Find(x => x == subscriptionResponseModel.SubscriptionProfiles[i].ProfileId);
                if (string.IsNullOrWhiteSpace(profileId))
                    profilesToDelete.Add(subscriptionResponseModel.SubscriptionProfiles[i]);
            }

            // We validate if the totals that the user has in the subscriptions is the equal to the quantity of students. 
            if (!quantityPrevalidated)
            {
                if (int.Parse(subscriptionResponseModel.Quantity) * int.Parse(subscriptionResponseModel.Plan.ProfilesQuantity) < profilesToAdd.Count + profilesSame.Count - profilesToDelete.Count)
                {
                    // The quantity is not the same, send error
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_quantity_of_students_more_than_available"]));
                    return responseModel;
                }
            }

            var subscriptionProfileDAO = new SubscriptionProfileDAO();
            if (profilesToDelete.Count > 0) // Unnassign the seat 
                subscriptionProfileDAO.Delete(_connectionString, profilesToDelete);
            // Assign the new profiles  if any 
            if (profilesToAdd.Count > 0)
                subscriptionProfileDAO.Create(_connectionString, profilesToAdd);

            responseModel.TransactionValid = true;
            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_profile_added_to_subscription"]));
            return responseModel;
        }

        /// <summary>
        /// To remove profiles from a subscription.
        /// </summary>
        /// <param name="subscriptionId">The id of the subscription.</param>
        /// <param name="profileIds">The id of the profiles.</param>
        /// <returns>Returns a response model with transaction result.</returns>
        public ResponseModel RemoveProfilesFromSubscription(string subscriptionId, List<string> profileIds)
        {
            var responseModel = new ResponseModel();
            var messageManager = new MessageManager(_connectionStringRead, _langCode);

            // Prepare the SubscriptionProfileModel list to delete
            var profileSubscriptionsToDeleteList = new List<SubscriptionProfileModel>();
            foreach (var profileId in profileIds)
            {
                if (!string.IsNullOrWhiteSpace(profileId))
                {
                    profileSubscriptionsToDeleteList.Add(new SubscriptionProfileModel
                    {
                        SubscriptionId = subscriptionId,
                        ProfileId = profileId
                    });
                }
            }

            // Just return an okay. There is nothing to remove.
            if (profileSubscriptionsToDeleteList.Count == 0)
            {
                responseModel.TransactionValid = true;
                return responseModel;
            }

            var subscriptionProfileDAO = new SubscriptionProfileDAO();
            subscriptionProfileDAO.Delete(_connectionString, profileSubscriptionsToDeleteList);
            responseModel.TransactionValid = true;
            return responseModel;


        }

        /// <summary>
        /// To assign profiles to a subscription.
        /// </summary>
        /// <param name="subscriptionResponseModel">The subscription's details that we will assign the profiles. </param>
        /// <param name="profileIds">The profile's id to attach.</param>
        /// <param name="userSessionId">The user that is making the request.</param>
        /// <param name="quantityPrevalidated">If the quantity allowed to enroll were validated already.</param>
        /// <returns>Returns the response model with the transaction's details.</returns>
        public ResponseModel AssignProfilesToSubscription(SubscriptionUserResponseModel subscriptionResponseModel, List<string> profileIds, string userSessionId, bool quantityPrevalidated)
        {
            ResponseModel responseModel = new ResponseModel();
            MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

            SubscriptionReadersManager subscriptionReadersManager = new SubscriptionReadersManager(_connectionStringRead, _langCode);
            // Get the subscription details (commented because it is duplicated.
            //  Model.Responses.SubscriptionUserResponseModel subscriptionResponseModel = subscriptionReadersManager.GetSubscriptionDetails(subscriptionId, userId, isStaff);

            if (subscriptionResponseModel == null) // If the subscription doesn't exists
            {
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_subscription_not_valid"]));
                return responseModel;
            }

            // Check if the subscription stills valid 
            if (DateTimeOffset.Now > DateTimeOffset.Parse(subscriptionResponseModel.CurrentPeriodEnd))
            {
                // Cannot do the process because the invoice hasn't been renewed 
                responseModel.TransactionValid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_subscription_payment_hasnt_been_reflected"]));
                return responseModel;
            }

            // Evaluate if the profiles ids are distintcs for the subscriptions 
            List<SubscriptionProfileModel> profilesToAdd = new List<SubscriptionProfileModel>();
            List<SubscriptionProfileModel> profilesSame = new List<SubscriptionProfileModel>();
            List<SubscriptionProfileModel> profilesToDelete = new List<SubscriptionProfileModel>();

            // We evaluate the profile ids
            for (int i = 0; i < profileIds.Count; i++)
            {
                SubscriptionProfileModel subscriptionProfileModel = subscriptionResponseModel.SubscriptionProfiles.Find(x => x.ProfileId == profileIds[i]);
                if (subscriptionProfileModel != null)
                    profilesSame.Add(subscriptionProfileModel);
                else
                {
                    subscriptionProfileModel = new SubscriptionProfileModel
                    {
                        SubscriptionId = subscriptionResponseModel.SubscriptionId,
                        ProfileId = profileIds[i],
                        ModifiedBy = userSessionId
                    };

                    profilesToAdd.Add(subscriptionProfileModel);
                }
            }

            // Now check if we need to delete something 
            for (int i = 0; i < subscriptionResponseModel.SubscriptionProfiles.Count; i++)
            {
                string profileId = profileIds.Find(x => x == subscriptionResponseModel.SubscriptionProfiles[i].ProfileId);
                if (string.IsNullOrWhiteSpace(profileId))
                    profilesToDelete.Add(subscriptionResponseModel.SubscriptionProfiles[i]);
            }

            // We validate if the totals that the user has in the subscriptions is the equal to the quantity of students. 
            if (!quantityPrevalidated)
            {
                if (int.Parse(subscriptionResponseModel.Quantity) * int.Parse(subscriptionResponseModel.Plan.ProfilesQuantity) < profilesToAdd.Count + profilesSame.Count - profilesToDelete.Count)
                {
                    // The quantity is not the same, send error
                    responseModel.TransactionValid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_quantity_of_students_more_than_available"]));
                    return responseModel;
                }
            }

            SubscriptionProfileDAO subscriptionProfileDAO = new SubscriptionProfileDAO();
            if (profilesToDelete.Count > 0) // Unnassign the seat 
                subscriptionProfileDAO.Delete(_connectionString, profilesToDelete);
            // Assign the new profiles  if any 
            if (profilesToAdd.Count > 0)
                subscriptionProfileDAO.Create(_connectionString, profilesToAdd);

            responseModel.TransactionValid = true;
            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_profile_added_to_subscription"]));
            return responseModel;

        }

    }
}
