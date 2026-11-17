using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Stripe;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// A webhook for Stripe. Do not modify unless you update Stripe's version.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/stripe")]
    public class StripeController : ControllerBase
    {
        // It's good practice to get these from configuration
        private readonly string _connectionString;
        private readonly string _connectionStringReader;
        private readonly string _langCode;
        private readonly string _stripeWebhookSecret;
        private readonly string _stripeApiKey;

        public StripeController()
        {
            // Initialize connection strings and secrets from environment variables
            _connectionString = AppConfiguration.Configuration["AppSettings:SubscriptionsStripeDBConnection"];
            _connectionStringReader = AppConfiguration.Configuration["AppSettings:SubscriptionsStripeDBConnectionReader"];
            _langCode = "en"; // Or get from config if dynamic
            _stripeWebhookSecret = AppConfiguration.Configuration["AppSettings:subs_stripe_webhook_secret"];
            _stripeApiKey = AppConfiguration.Configuration["AppSettings:subs_stripe_key"];
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var message = "";
            string json = await new System.IO.StreamReader(Request.Body).ReadToEndAsync(); // Read content asynchronously

            try
            {
                // *** IMPORTANT: Verify the webhook signature for security ***
                // This is the correct and secure way to parse the event.
                var stripeSignature = Request.Headers["Stripe-Signature"].FirstOrDefault();
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _stripeWebhookSecret);

                Console.WriteLine($"Received Stripe event: {stripeEvent.Type} (ID: {stripeEvent.Id})");

                // Instantiate your managers
                var subscriptionManager = new SubscriptionManager(_connectionString, _connectionStringReader, _langCode);
                var subscriptionsUpdater = new SubscriptionsUpdaterManager(_connectionString, _connectionStringReader, _langCode);
                var subscriptionReadersManager = new SubscriptionReadersManager(_connectionStringReader, _langCode);
                var plansReaderManager = new PlansReaderManager(_connectionStringReader, _langCode);


                switch (stripeEvent.Type)
                {
                    case "account.updated": // Test
                        {
                            return Ok("Account updated event received. This is a test event and does not require further processing.");

                        }
                    case "invoice.created": // "invoice.created"
                        {
                            // Get the raw JSON string from the Stripe event data
                            string rawInvoiceCreatedJson = stripeEvent.Data.ToJson();

                            if (string.IsNullOrEmpty(rawInvoiceCreatedJson))
                            {
                                message += $"Warning: Raw JSON for invoice.created event is null or empty (from ToJson()). Event ID: {stripeEvent.Id}\n";
                                break; // Exit case
                            }

                            // Parse the raw JSON into a JObject for dynamic access
                            JObject invoiceDetailsJson = JObject.Parse(rawInvoiceCreatedJson);

                            // Accessing properties using JObject/JToken
                            string invoiceId = (string)invoiceDetailsJson["id"];
                            string customerId = (string)invoiceDetailsJson["customer"]; // Customer ID is typically a direct property
                            long? amountDue = (long?)invoiceDetailsJson["amount_due"]; // amount_due is typically a long in cents
                            string subscriptionId = (string)invoiceDetailsJson["subscription"]; // Subscription ID is typically a direct property

                            message += $"Invoice Created: ID={invoiceId}, Customer={customerId}, AmountDue={amountDue}\n";

                            var invoiceLogModel = new InvoiceLogModel
                            {
                                CustomerPlatformId = customerId,
                                AmtDue = amountDue?.ToString(), // Use null-conditional operator for nullable long and convert to string
                                SubscriptionPlatformId = subscriptionId, // Use the extracted subscription ID
                                InvoicePlatformId = invoiceId
                            };
                            subscriptionManager.CreateInvoiceLog(invoiceLogModel); // Assuming subscriptionManager and CreateInvoiceLog exist
                            message += $"Invoice log created for Invoice ID: {invoiceId}\n";
                            break;
                        }

                    case "invoice.payment_succeeded": // "invoice.payment_succeeded"
                        {
                            // Get the raw JSON string from the Stripe event data
                            string rawInvoiceJsonSucceeded = stripeEvent.Data.ToJson();

                            if (string.IsNullOrEmpty(rawInvoiceJsonSucceeded))
                            {
                                message += $"Warning: Raw JSON for invoice.payment_succeeded event is null or empty (from ToJson()). Event ID: {stripeEvent.Id}\n";
                                break;
                            }

                            // Parse the raw JSON into a JObject for dynamic access
                            JObject invoiceDetailsJson = JObject.Parse(rawInvoiceJsonSucceeded);

                            // Accessing properties using JObject/JToken
                            string invoiceId = (string)invoiceDetailsJson["id"];
                            // The subscription ID is typically a direct property on the Invoice object.
                            string subscriptionIdForSucceededPayment = (string)invoiceDetailsJson["subscription"];


                            message += $"Invoice Payment Succeeded: ID={invoiceId}, Subscription={subscriptionIdForSucceededPayment}\n";

                            // For renewal, the most reliable source for the *new* period end is often the invoice's period_end
                            // or fetching the updated subscription from Stripe.
                            DateTime? newPeriodEndFromInvoice = null;
                            JToken periodEndToken = invoiceDetailsJson.SelectToken("period_end"); // Access period_end from the invoice JSON

                            if (periodEndToken != null && periodEndToken.Type != JTokenType.Null)
                            {
                                long unixTimestamp = (long)periodEndToken;
                                newPeriodEndFromInvoice = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
                                message += $"  Invoice Period End: {newPeriodEndFromInvoice?.ToShortDateString()}\n";
                            }
                            else
                            {
                                message += "  Invoice PeriodEnd is null. Attempting to fetch subscription for period end.\n";
                                // Fallback to fetching subscription if invoice.PeriodEnd is not available (less common for paid invoices)
                                try
                                {
                                    // Ensure _stripeApiKey is accessible in this scope
                                    StripeConfiguration.ApiKey = _stripeApiKey; // Assuming _stripeApiKey is defined and accessible
                                    var service = new SubscriptionService();
                                    // Use the subscription ID extracted from the JSON
                                    Subscription subscription = await service.GetAsync(subscriptionIdForSucceededPayment);
                                    if (subscription?.Items?.Data?.FirstOrDefault()?.CurrentPeriodEnd != null)
                                    {
                                        newPeriodEndFromInvoice = subscription.Items.Data.FirstOrDefault().CurrentPeriodEnd;
                                        message += $"  Fetched Subscription Period End: {newPeriodEndFromInvoice}\n";
                                    }
                                    else
                                    {
                                        message += $"  Fetched subscription {subscriptionIdForSucceededPayment} did not contain a valid CurrentPeriodEnd.\n";
                                    }
                                }
                                catch (StripeException ex)
                                {
                                    message += $"Stripe Error fetching subscription {subscriptionIdForSucceededPayment}: {ex.Message}\n";
                                }
                                catch (Exception ex)
                                {
                                    message += $"General Error fetching subscription {subscriptionIdForSucceededPayment}: {ex.Message}\n";
                                }
                            }

                            // Update the subscription details in your local DB
                            var subscriptionModelToUpdate = new SubscriptionModel
                            {
                                Status = "active", // Assuming successful payment means active
                                CurrentPeriodEnd = newPeriodEndFromInvoice?.ToString() // Use the date from Stripe
                            };
                            var subscriptionModelWhere = new SubscriptionModel
                            {
                                PlatformId = subscriptionIdForSucceededPayment
                            };

                            subscriptionsUpdater.UpdateSubscription(subscriptionModelToUpdate, subscriptionModelWhere, null);
                            message += $"Local DB updated for successful payment for subscription {subscriptionIdForSucceededPayment}.\n";

                            // TODO: Send confirmation email, grant/extend access
                            break;
                        }

                    case "invoice.payment_failed": // "invoice.payment_failed"
                        {
                            // Get the raw JSON string from the Stripe event data
                            string rawInvoiceJsonFailed = stripeEvent.Data.ToJson();

                            if (string.IsNullOrEmpty(rawInvoiceJsonFailed))
                            {
                                message += $"Warning: Raw JSON for invoice.payment_failed event is null or empty (from ToJson()). Event ID: {stripeEvent.Id}\n";
                                break;
                            }

                            // Parse the raw JSON into a JObject for dynamic access
                            JObject invoiceDetailsJson = JObject.Parse(rawInvoiceJsonFailed);

                            // Accessing properties using JObject/JToken
                            string invoiceId = (string)invoiceDetailsJson["id"];
                            // The subscription ID is typically a direct property on the Invoice object.
                            // The original code used invoicePaymentFailed.Parent.SubscriptionDetails.SubscriptionId,
                            // which might be for a very specific expanded object structure or an older Stripe.Net version.
                            // The 'subscription' field directly on the Invoice is common.
                            string subscriptionIdForFailedPayment = (string)invoiceDetailsJson["subscription"];


                            message += $"Invoice Payment Failed: ID={invoiceId}, Subscription={subscriptionIdForFailedPayment}\n";

                            // Update the subscription status in your local DB
                            var subscriptionModelToUpdate = new SubscriptionModel
                            {
                                Status = "past_due", // Or "unpaid" depending on your Stripe settings
                                                     // CurrentPeriodEnd should reflect the last paid period end, not necessarily change here
                            };
                            var subscriptionModelWhere = new SubscriptionModel
                            {
                                PlatformId = subscriptionIdForFailedPayment
                            };

                            subscriptionsUpdater.UpdateSubscription(subscriptionModelToUpdate, subscriptionModelWhere, null);
                            message += $"Local DB updated for failed payment for subscription {subscriptionIdForFailedPayment}.\n";

                            // TODO: Notify customer, initiate dunning process
                            break;
                        }

                    case "customer.subscription.updated": // "customer.subscription.updated"
                        {
                            // Get the raw JSON string from the Stripe event data
                            string rawSubscriptionJsonUpdated = stripeEvent.Data.ToJson();

                            if (string.IsNullOrEmpty(rawSubscriptionJsonUpdated))
                            {
                                message += $"Warning: Raw JSON for customer.subscription.updated event is null or empty (from ToJson()). Event ID: {stripeEvent.Id}\n";
                                break;
                            }

                            // Parse the raw JSON into a JObject for dynamic access
                            JObject subscriptionDetailsJsonUpdated = JObject.Parse(rawSubscriptionJsonUpdated);

                            // Accessing properties using JObject/JToken
                            string subscriptionIdUpdated = (string)subscriptionDetailsJsonUpdated["id"];
                            string statusUpdated = (string)subscriptionDetailsJsonUpdated["status"];

                            message += $"Subscription Updated: ID={subscriptionIdUpdated}, Status={statusUpdated}\n";

                            // Get CurrentPeriodEnd from the first subscription item
                            DateTime? currentPeriodEndUpdated = null;
                            JToken currentPeriodEndTokenUpdated = subscriptionDetailsJsonUpdated.SelectToken("items.data[0].current_period_end");
                            if (currentPeriodEndTokenUpdated != null && currentPeriodEndTokenUpdated.Type != JTokenType.Null)
                            {
                                long unixTimestamp = (long)currentPeriodEndTokenUpdated;
                                currentPeriodEndUpdated = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
                            }
                            message += $"  Period End: {currentPeriodEndUpdated?.ToShortDateString()}\n";


                            var subscriptionToUpdate = new SubscriptionModel
                            {
                                Status = statusUpdated,
                                CurrentPeriodEnd = currentPeriodEndUpdated?.ToString() // Convert DateTime? to string for your model
                            };

                            var subscriptionWhere = new SubscriptionModel
                            {
                                PlatformId = subscriptionIdUpdated
                            };

                            subscriptionsUpdater.UpdateSubscription(subscriptionToUpdate, subscriptionWhere, null);
                            message += $"Subscription basic status/period end updated.\n";

                            // If the plan changed, update PlanPlatformId in your local DB
                            // Use SelectToken for robust access to nested properties
                            string newPlanPlatformId = (string)subscriptionDetailsJsonUpdated.SelectToken("items.data[0].price.id");

                            if (!string.IsNullOrEmpty(newPlanPlatformId))
                            {
                                message += $"  Plan changed to: {newPlanPlatformId}\n";

                                var subscriptionToUpdatePlan = new SubscriptionModel
                                {
                                    PlanPlatformId = newPlanPlatformId,
                                };
                                var subscriptionWherePlan = new SubscriptionModel
                                {
                                    PlatformId = subscriptionIdUpdated
                                };
                                subscriptionsUpdater.UpdateSubscription(subscriptionToUpdatePlan, subscriptionWherePlan, null);
                                message += $"Subscription plan updated.\n";
                            }
                            else
                            {
                                message += $"  No plan change detected or Price ID not available.\n";
                            }
                            break;
                        }

                    case "customer.subscription.deleted": // "customer.subscription.deleted"
                        {
                            // This is the key change, using ToJson() to get the raw string
                            string rawSubscriptionJsonDeleted = stripeEvent.Data.ToJson();

                            if (string.IsNullOrEmpty(rawSubscriptionJsonDeleted))
                            {
                                message += $"Warning: Raw JSON for customer.subscription.deleted event is null or empty (from ToJson()). Event ID: {stripeEvent.Id}\n";
                                break;
                            }

                            // Parse the raw JSON into a JObject for dynamic access
                            JObject subscriptionDetailsJsonDeleted = JObject.Parse(rawSubscriptionJsonDeleted);

                            // Accessing properties using JObject/JToken
                            string subscriptionIdDeleted = (string)subscriptionDetailsJsonDeleted["id"];
                            string statusDeleted = (string)subscriptionDetailsJsonDeleted["status"]; // Should be "canceled" or "unpaid"

                            message += $"Subscription Deleted: ID={subscriptionIdDeleted}, Status={statusDeleted}\n";

                            // Get CurrentPeriodEnd from the first subscription item using SelectToken for robustness
                            // Note: For 'deleted' events, CurrentPeriodEnd might reflect the actual end or the original planned end.
                            // Stripe's documentation suggests using 'ended_at' for the precise end of the subscription.
                            DateTime? actualEndDate = null;
                            JToken endedAtToken = subscriptionDetailsJsonDeleted.SelectToken("ended_at"); // Often available for deleted subscriptions
                            JToken currentPeriodEndTokenDeleted = subscriptionDetailsJsonDeleted.SelectToken("items.data[0].current_period_end");

                            if (endedAtToken != null && endedAtToken.Type != JTokenType.Null)
                            {
                                long unixTimestampEndedAt = (long)endedAtToken;
                                actualEndDate = DateTimeOffset.FromUnixTimeSeconds(unixTimestampEndedAt).DateTime;
                                message += $"  Ended At Date (from 'ended_at'): {actualEndDate?.ToShortDateString()}\n";
                            }
                            else if (currentPeriodEndTokenDeleted != null && currentPeriodEndTokenDeleted.Type != JTokenType.Null)
                            {
                                // Fallback to current_period_end if ended_at is not present, though 'ended_at' is usually more accurate for deletion.
                                long unixTimestampCurrentPeriodEnd = (long)currentPeriodEndTokenDeleted;
                                actualEndDate = DateTimeOffset.FromUnixTimeSeconds(unixTimestampCurrentPeriodEnd).DateTime;
                                message += $"  Period End Date (from item current_period_end): {actualEndDate?.ToShortDateString()}\n";
                            }
                            else
                            {
                                message += "  Could not determine actual end date from 'ended_at' or 'current_period_end'.\n";
                            }


                            var subscriptionToUpdate = new SubscriptionModel
                            {
                                Status = statusDeleted, // Will be "canceled" or "unpaid"
                                CurrentPeriodEnd = actualEndDate?.ToString() // This will be the actual end date
                            };

                            var subscriptionWhere = new SubscriptionModel
                            {
                                PlatformId = subscriptionIdDeleted // Use the ID retrieved from JSON
                            };
                            subscriptionsUpdater.UpdateSubscription(subscriptionToUpdate, subscriptionWhere, null);

                            message += "Subscription updated as deleted.\n";

                            // TODO: Revoke user access, mark as inactive/deleted in your DB
                            break;
                        }

                    case "customer.subscription.created": // "customer.subscription.created"
                        {
                            message += "customer.subscription.created \n";

                            // Get the raw JSON string from the Stripe event data
                            string rawSubscriptionJson = stripeEvent.Data.ToJson();
                            message += $"rawSubscriptionJson: {rawSubscriptionJson}. \n";


                            if (string.IsNullOrEmpty(rawSubscriptionJson))
                            {
                                Console.WriteLine($"Warning: Raw JSON for customer.subscription.created event is null or empty. Event ID: {stripeEvent.Id}");
                                break;
                            }

                            // Parse the raw JSON into a JObject for dynamic access
                            JObject subscriptionDetailsJson = JObject.Parse(rawSubscriptionJson);

                            message += "customer.subscription.created: Subscription details JSON parsed. \n";

                            // Accessing properties using JObject/JToken
                            string subscriptionId = (string)subscriptionDetailsJson["id"];
                            string customerId = (string)subscriptionDetailsJson["customer"]; // This will correctly get the customer ID string
                            string status = (string)subscriptionDetailsJson["status"];

                            Console.WriteLine($"Subscription Created: ID={subscriptionId}, Status={status}");

                            // Get CurrentPeriodEnd from the first subscription item using SelectToken for robustness
                            DateTime? currentPeriodEnd = null;
                            JToken currentPeriodEndToken = subscriptionDetailsJson.SelectToken("items.data[0].current_period_end");
                            if (currentPeriodEndToken != null && currentPeriodEndToken.Type != JTokenType.Null)
                            {
                                long unixTimestamp = (long)currentPeriodEndToken;
                                currentPeriodEnd = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
                            }

                            Console.WriteLine($"  Period End: {currentPeriodEnd?.ToShortDateString()}");
                            message += "customer.subscription.created: currentPeriodEnd " + currentPeriodEnd + " \n";
                            message += "customer.subscription.created: Period End, Subscription details: " +
                                        JsonConvert.SerializeObject(subscriptionDetailsJson) + " \n";

                            message += $"starting details *********************************** \n";
                            message += $"customer.subscription.created: subscriptionId, {subscriptionId} \n";
                            message += $"customer.subscription.created: customerId, {customerId} \n";
                            message += $"customer.subscription.created: status, {status} \n";
                            message += $"customer.subscription.created: currentPeriodEnd, {currentPeriodEnd} \n";

                            // Get the PlanPlatformId (Price ID) using SelectToken
                            string planPlatformId = (string)subscriptionDetailsJson.SelectToken("items.data[0].price.id");
                            message += $"customer.subscription.created: PlanPlatformId, {planPlatformId} \n";


                            var subscriptionToCreateOrUpdate = new SubscriptionModel
                            {
                                PlatformId = subscriptionId,
                                CustomerPlatformId = customerId, // Now correctly assigned as a string
                                Status = status,
                                CurrentPeriodEnd = currentPeriodEnd?.ToString(), // Convert DateTime? to string for your model
                                PlanPlatformId = planPlatformId,
                                // Add other relevant fields like Quantity, Type, ModifiedBy if available
                            };
                            message += "customer.subscription.created: Object prepared";

                            subscriptionsUpdater.UpdateSubscription(subscriptionToCreateOrUpdate, new SubscriptionModel { PlatformId = subscriptionId }, null);
                            message += "customer.subscription.created: Subscription updated.";
                            break;
                        }

                    case "customer.subscription.trial_will_end": // "customer.subscription.trial_will_end"
                        {
                            // Get the raw JSON string from the Stripe event data
                            string rawSubscriptionJsonTrialEnd = stripeEvent.Data.ToJson();

                            if (string.IsNullOrEmpty(rawSubscriptionJsonTrialEnd))
                            {
                                message += $"Warning: Raw JSON for customer.subscription.trial_will_end event is null or empty (from ToJson()). Event ID: {stripeEvent.Id}\n";
                                break;
                            }

                            // Parse the raw JSON into a JObject for dynamic access
                            JObject subscriptionDetailsJson = JObject.Parse(rawSubscriptionJsonTrialEnd);

                            // Accessing properties using JObject/JToken
                            string subscriptionId = (string)subscriptionDetailsJson["id"];
                            string customerId = (string)subscriptionDetailsJson["customer"]; // Customer ID is typically a direct property

                            message += $"Trial Will End for Subscription: ID={subscriptionId}, Customer={customerId}\n";

                            DateTime? trialEnd = null;
                            JToken trialEndToken = subscriptionDetailsJson.SelectToken("trial_end"); // Access trial_end from the subscription JSON

                            if (trialEndToken != null && trialEndToken.Type != JTokenType.Null)
                            {
                                long unixTimestamp = (long)trialEndToken;
                                trialEnd = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
                            }
                            message += $"  Trial End Date: {trialEnd?.ToShortDateString()}\n";

                            // TODO: Send email or whatever, telling the user that the subscription trial will end and the payment will start.
                            message += "Action required: Send email/notification to user about trial ending.\n";
                            break;
                        }

                    default:
                        Console.WriteLine($"Unhandled webhook event type: {stripeEvent.Type}. Event ID: {stripeEvent.Id}");
                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                // Log Stripe-specific errors (e.g., signature mismatch, invalid event)
                Console.Error.WriteLine($"Stripe Webhook Error: {ex.Message}. HTTP Status: {ex.HttpStatusCode}. Stripe Code: {ex.StripeError?.Code}");
                return BadRequest($"Stripe Webhook Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log general unexpected errors
                Console.Error.WriteLine($"General Webhook Error: {ex.Message}. Stack Trace: {ex.StackTrace}");
                return StatusCode(500, $"Internal Server Error: {message} {ex.Message}");
            }
        }
    }
}