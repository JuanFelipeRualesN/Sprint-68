using Microsoft.AspNetCore.Mvc;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests.APIs;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Schools;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.DapperHandler;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// To handle the enrollments/subscriptions.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/subscriptions")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly CustomClasses.CacheHandler.CoursesCache _coursesCache;

        public SubscriptionsController(
            CustomClasses.CacheHandler.CoursesCache coursesCache)
        {
            _coursesCache = coursesCache;
        }
        /// <summary>
        /// Role ids allowed to receive the related users (SubscriptionProfiles) in GetActiveSubscriptionsRelatedToUser.
        /// RoleId comes from the Users API as a string.
        /// </summary>
        private static readonly HashSet<string> AllowedProfileRoleIdsInRelatedUserSubscriptions = new() { "1", "2", "4", "5", "6", "8" };


        /// <summary>
        /// Returns the active subscriptions/enrollments that the user is related with. 
        /// </summary>
        /// <param name="cacheOnForUserSubscriptions">Use the cache (default true).</param>
        /// <param name="includeProfiles">
        /// True to also return, per subscription, the users related to it (subst_subscription_profiles) with their names and role
        /// (<see cref="SubscriptionAcademicFullResponseModel.SubscriptionProfiles"/>). One query for all the profiles and one Users
        /// API call each for names and roles, in parallel with the rest of the reads. Default false (the response is unchanged).
        /// </param>
        /// <returns>Returns an HTTP response with the list of the subscriptions.</returns>
        [HttpGet("active/related/user")]
        public async Task<IActionResult> GetActiveSubscriptionsRelatedToUser([FromQuery] bool cacheOnForUserSubscriptions = true, [FromQuery] bool includeProfiles = false)
        {
            try
            {
                // -------- Read cache first --------
                string token = RequestHandler.GetToken(HttpContext);                         // assumes you have a Core-friendly overload
                if (cacheOnForUserSubscriptions)
                {
                    var cached = _coursesCache.GetUserRelatedSubscriptions(token, includeProfiles);
                    if (cached != null && cached.Count > 0)
                        return Ok(cached);
                }

                // -------- Token/session validation (Users API) --------
                var ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext); // adapt your helper to Core HttpRequest
                var lang = RequestHandler.GetLanguage(HttpContext);                             // adapt helper if needed

                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token,
                    ipAddress,
                    "SubscriptionUserProductRead",
                    lang
                );

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // -------- Fetch subscriptions/products --------
                var readConnStr = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"];
                var subscriptionReadersManager = new SubscriptionReadersManager(readConnStr, lang);

                // If this call stays as-is in your codebase:
                var subscriptionFullResponseModelList =
                    UserSubscriptions.ReadProfileSubscriptionsFullItems(responseSession.UserId, true);

                var subscriptionAcademicFullResponseModelList = new List<SubscriptionAcademicFullResponseModel>();

                if (subscriptionFullResponseModelList.Count == 0)
                {
                    return Ok(subscriptionAcademicFullResponseModelList);
                }

                // 1. Extract valid subscription models safely
                var validSubscriptions = subscriptionFullResponseModelList
                    .Where(s => s.SubscriptionModel != null)
                    .ToList();

                var subscriptionIds = validSubscriptions
                    .Select(s => s.SubscriptionModel.Id)
                    .ToList();

                // Related users: started now so the profiles query and the Users API calls overlap with the reads below.
                // Only the allowed roles get the profiles; everyone else gets the rest of the response as if includeProfiles were false.
                var returnProfiles = includeProfiles
                    && AllowedProfileRoleIdsInRelatedUserSubscriptions.Contains(responseSession.RoleId?.Trim());

                var profilesTask = returnProfiles && subscriptionIds.Count > 0
                    ? SubscriptionProfileUsersReader.ReadAsync(readConnStr, token, ipAddress, lang, subscriptionIds, responseSession.UserId)
                    : null;

                var examTryMaxModelsToFind = subscriptionIds
                    .Select(id => new ExamTryMaxModel { SubscriptionId = id })
                    .ToList();

                // 2. Determine if school API call is required using LINQ Any()
                bool hasSchool = validSubscriptions.Any(s => s.SubscriptionModel.Type is "S" or "T");

                // 3. Flatten product search list efficiently using SelectMany
                var subscriptionProductModelToFindList = validSubscriptions
                    .Where(s => s.ItemResponseModelList != null)
                    .SelectMany(s => s.ItemResponseModelList)
                    .Select(item => new SubscriptionProductModel { ProductId = item.Id })
                    .ToList();

                // 4. Fetch External Data
                var entitySubscriptionPeriodsList = (subscriptionIds.Count > 0 && hasSchool)
                    ? SubscriptionsAPI.Services.FeignAPIs.SchoolsDetailsHandler.GetAcademicDetailsBySubscriptionIdList(subscriptionIds)
                    : new List<EntityAcademicDetailFullResponseModel>();

                // Convert to dictionary for O(1) lightning-fast lookups in the loop below
                var academicDetailsMap = entitySubscriptionPeriodsList?
                    .Where(x => x?.EntitySubscriptionModel != null)
                    .ToDictionary(x => x.EntitySubscriptionModel.SubscriptionId) ?? new Dictionary<string, EntityAcademicDetailFullResponseModel>();

                var subscriptionProductModelListResult = subscriptionProductModelToFindList.Count > 0
                    ? new SubscriptionProductDAO().Read(readConnStr, subscriptionProductModelToFindList)
                    : new List<SubscriptionProductModel>();

                // Max allowed exam tries (if needed later)
                var examTryMaxModelsResults = new ExamTryMaxDAO().Read(readConnStr, examTryMaxModelsToFind);

                var profilesBySubscription = profilesTask != null
                    ? await profilesTask
                    : new Dictionary<string, List<SubscriptionProfileUserResponseModel>>();

                // -------- Build final response --------
                foreach (var subscription in validSubscriptions)
                {
                    var subAcademic = new SubscriptionAcademicFullResponseModel
                    {
                        SubscriptionModel = subscription.SubscriptionModel,
                        PlanModel = subscription.PlanModel,
                        SubscriptionProductModelList = subscription.ItemResponseModelList?
                            .Select(item => new SubscriptionProductModel
                            {
                                Subject = item.Subject,
                                Level = item.Level,
                                ProductId = item.Id,
                                SubjectName = item.Name,
                                Name = item.Name,
                                SubscriptionType = "S",
                                Id = item.Id
                            }).ToList() ?? new List<SubscriptionProductModel>()
                    };

                    // O(1) Dictionary Lookup instead of .Find() linear scan
                    if (academicDetailsMap.TryGetValue(subscription.SubscriptionModel.Id, out var academicDetail))
                    {
                        subAcademic.EntityAcademicDetailFullResponseModel = academicDetail;
                    }

                    if (returnProfiles)
                    {
                        subAcademic.SubscriptionProfiles = profilesBySubscription.TryGetValue(subscription.SubscriptionModel.Id, out var related)
                            ? related
                            : new List<SubscriptionProfileUserResponseModel>();
                    }

                    subscriptionAcademicFullResponseModelList.Add(subAcademic);
                }


                // -------- Cache asynchronously (optional) --------
                // -------- Cache asynchronously (Fire-and-Forget safely) --------
                if (cacheOnForUserSubscriptions)
                {
                    _ = Task.Run(async () =>
                    {
                        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                        try
                        {
                            // Note: If StoreUserRelatedSubscriptions supports Async, await it here and pass cts.Token
                            _coursesCache.StoreUserRelatedSubscriptions(token, subscriptionAcademicFullResponseModelList, includeProfiles);
                        }
                        catch (Exception ex)
                        {
                            // Log exception here so it doesn't crash silently or bring down the process
                            System.Diagnostics.Debug.WriteLine($"Caching failed: {ex.Message}");
                        }
                    });
                }

                return Ok(subscriptionAcademicFullResponseModelList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("client-creation")]
        public async Task<IActionResult> SubscribeUser(
        [FromBody] ClientSubscriptionRegisterRequestModel clientSubscriptionRegisterRequest,
        [FromQuery] bool stripeRequired = true)
        {
            try
            {
                var langCode = RequestHandler.GetLanguage(HttpContext);
                var generalResponseModel = new SubscriptionResponseModel();

                var plansReaderManager = new PlansReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], langCode);

                var planResponseModel = plansReaderManager.GetPlanDetails(
                    clientSubscriptionRegisterRequest.PlanId,
                    true,
                    true);

                if (!planResponseModel.TransactionValid)
                {
                    var response = new SubscriptionResponseModel
                    {
                        Messages = generalResponseModel.Messages
                    };
                    return BadRequest(response);
                }

                var subscriptionManagerCreator = new SubscriptionManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);

                var stripeKey = AppConfiguration.Configuration["AppSettings:SubsStripeKey"];

                var customerModel = subscriptionManagerCreator.CreateCustomerInStripe(
                    clientSubscriptionRegisterRequest.Email,
                    clientSubscriptionRegisterRequest.UserId,
                    stripeKey);

                SubscriptionsAPI.Domain.Models.Subscriptions.Responses.SubscriptionUserResponseModel responseModel;

                if (clientSubscriptionRegisterRequest.IsPromo == 0)
                {
                    responseModel = await subscriptionManagerCreator
                        .CreateSubscriptionStripeWithoutCountryValidations(
                            planResponseModel,
                            customerModel.PlatformId,
                            clientSubscriptionRegisterRequest.StripeToken,
                            clientSubscriptionRegisterRequest.StateIso,
                            clientSubscriptionRegisterRequest.CountryIsoCode,
                            clientSubscriptionRegisterRequest.SubscriptionsQuantity,
                            stripeKey,
                            clientSubscriptionRegisterRequest.UserId,
                            clientSubscriptionRegisterRequest.CouponPlatformId,
                            stripeRequired ? "H" : "S" // H because this API is for homeschoolers
                        );
                }
                else
                {
                    responseModel = await subscriptionManagerCreator.CreateUserSubscription(
                        clientSubscriptionRegisterRequest.UserId,
                        null,
                        planResponseModel.Id,
                        clientSubscriptionRegisterRequest.SubscriptionsQuantity,
                        clientSubscriptionRegisterRequest.StateIso,
                        clientSubscriptionRegisterRequest.CountryIsoCode,
                        stripeKey,
                        clientSubscriptionRegisterRequest.UserId,
                        clientSubscriptionRegisterRequest.CouponPlatformId,
                        stripeRequired ? "STR" : "PRI",
                        null,
                        stripeRequired ? "H" : "S" // H because this API is for homeschoolers
                    );
                }

                if (responseModel.TransactionValid && !string.IsNullOrWhiteSpace(responseModel.SubscriptionId))
                {
                    // Set the subscription id in the response
                    generalResponseModel.Subscriptions =
                        new List<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.SubscriptionUserResponseModel>
                        {
                    new() { SubscriptionId = responseModel.SubscriptionId }
                        };

                    // Create the max tries for exams if provided
                    if (!string.IsNullOrWhiteSpace(clientSubscriptionRegisterRequest.MaxTriesExam))
                    {
                        new ExamTryMaxDAO().Create(
                            AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                            new ExamTryMaxModel
                            {
                                SubscriptionId = responseModel.SubscriptionId,
                                TryMax = clientSubscriptionRegisterRequest.MaxTriesExam
                            });
                    }

                    // Send success email (fire-and-forget)
                    var subscriptionReadersManager = new SubscriptionReadersManager(
                        AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                        langCode);

                    var emailDetailModel = subscriptionReadersManager.GetEmailDetail(
                         AppConfiguration.Configuration["AppSettings:subr_email_success_code_v2"]);

                    emailDetailModel.Message = emailDetailModel.Message
                        .Replace("{$0}", clientSubscriptionRegisterRequest.Email)
                        .Replace("{$1}", "ORD-" + responseModel.SubscriptionId)
                        .Replace("{$2}", clientSubscriptionRegisterRequest.TemporaryPassword);

                    _ = Task.Run(() =>
                        SubscriptionsAPI.Services.Emails.EmailManager.SendEmailSimple(
                            AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                            new List<string> { clientSubscriptionRegisterRequest.Email },
                            emailDetailModel.Title,
                            emailDetailModel.Message));

                    return StatusCode(201, generalResponseModel);
                }
                else
                {
                    // Payment failed or subscription not created
                    var subscriptionMessageManager = new MessageManager(
                        AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                        langCode);

                    generalResponseModel.Messages.Add(
                        subscriptionMessageManager.GetMessageDetails(
                             AppConfiguration.Configuration["AppSettings:subr_message_payment_failed"]));

                    return BadRequest(generalResponseModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// For commercial purposes only. 
        /// To subscribe a user by a simple process (for homeschoolers). It does not include attaching the user or subscription to a school.
        /// </summary>
        /// <param name="requestModel">Request details of user to subscribe. </param>
        /// <returns>Returns the transaction’s response. For the subscription's id, use only the first element of the subscription's list.</returns>
        [HttpPost("subscribe")]
        public async Task<IActionResult> CreateClientSubscription(
           [FromBody] SubscribeCustomerRequestModel requestModel)
        {
            var generalResponseModel = new SubscriptionResponseModel();

            try
            {
                // Core-friendly helpers (adapt your helpers if they expected System.Web)
                string ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext);
                string langCode = RequestHandler.GetLanguage(HttpContext);
                string token = RequestHandler.GetToken(HttpContext);

                /* Token validation in Users API */
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ipAddress, "SubscribeUserToPlan", langCode);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();
                /* End token validation */

                // Validate coupon (optional)
                if (!string.IsNullOrWhiteSpace(requestModel.CouponPlatformId))
                {
                    var couponReaderManager = new CouponReaderManager(
                        AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                        langCode);

                    var couponValidation = couponReaderManager.ValidateCoupon(requestModel.CouponPlatformId);
                    if (couponValidation == null)
                    {
                        var messageManager = new MessageManager(
                            AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                            langCode);

                        generalResponseModel.Messages = new List<string>
                {
                    messageManager.GetMessageDetails(
                         AppConfiguration.Configuration["AppSettings:subr_message_coupon_invalid_code"])
                };

                        return BadRequest(generalResponseModel);
                    }
                }

                // Get user address (country/state)
                var userAddress = GetUserDetails.GetUserAddress(token, ipAddress);
                string countryIso = userAddress?.CountryIsoCode2;

                // Create subscription manager
                var subsMgr = new SubscriptionManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);

                // Default Type if not provided: S=School, T=Tutoring/Program, H=Homeschool
                if (string.IsNullOrWhiteSpace(requestModel.Type))
                {
                    if (!string.IsNullOrWhiteSpace(requestModel.SchoolId))
                        requestModel.Type = "S";
                    else if (!string.IsNullOrWhiteSpace(requestModel.ProgramId))
                        requestModel.Type = "T";
                    else
                        requestModel.Type = "H";
                }

                var stripeKey = AppConfiguration.Configuration["AppSettings:SubsStripeKey"];

                var responseModel = await subsMgr.CreateUserSubscription(
                    responseSession.UserId,
                    requestModel.SubTokenStripe,
                    requestModel.PlanId,
                    requestModel.Quantity,
                    userAddress?.StateIsoCode,
                    countryIso,
                    stripeKey,
                    responseSession.UserId,
                    requestModel.CouponPlatformId,
                    "STR",               // payment source
                    null,
                    requestModel.Type    // S/T/H
                );

                generalResponseModel.Messages = responseModel.Messages;

                if (responseModel.TransactionValid)
                {
                    generalResponseModel.Subscriptions =
                        new List<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.SubscriptionUserResponseModel>
                        {
                    new() { SubscriptionId = responseModel.SubscriptionId }
                        };

                    return StatusCode(201, generalResponseModel);
                }

                return BadRequest(generalResponseModel);
            }
            catch (Exception ex)
            {
                generalResponseModel.Messages ??= new List<string>();
                generalResponseModel.Messages.Add("Error " + ex);
                return BadRequest(generalResponseModel);
            }
        }

        /// <summary>
        /// To get the subscription details for the client. 
        /// Used by another API to get the information of the subscription, as how many profiles can be attached, if active or not, etc.
        /// The only required and used property is Subscription.SubscriptionId. Do not send any other property (IpAddress is optional).
        /// </summary>
        /// <param name="subscriptionRequestModel">Subscription request that include the subscription's id property to retrieve.</param>
        /// <returns>Returns the subscription details.</returns>
        [HttpPost("clients/subscription-details")]
        public IActionResult GetSubscriptionDetailsClients([FromBody] SubscriptionRequestModel subscriptionRequestModel)
        {
            try
            {
                // Resolve IP / language / token (Core-friendly helpers)
                string ipAddress = string.IsNullOrWhiteSpace(subscriptionRequestModel.IpAddress)
                    ? IPAddressDetector.GetClientIpAddress(HttpContext)
                    : subscriptionRequestModel.IpAddress;

                string langCode = RequestHandler.GetLanguage(HttpContext);
                string token = RequestHandler.GetToken(HttpContext);

                /* Token validation in Users API */
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token,
                    ipAddress,
                    "SubscriptionReadClientSubscription",
                    langCode);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();
                /* End token validation */

                // Reader manager
                var subscriptionReaderManager = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);

                // Main account owner for the subscription
                string mainAccountUserId =
                    subscriptionReaderManager.GetMainAccountUserIdBySubscriptionId(
                        subscriptionRequestModel.Subscription.SubscriptionId);

                if (string.IsNullOrWhiteSpace(mainAccountUserId))
                    return Unauthorized();

                // Email of main account
                string email = SubscriptionsAPI.Services.FeignAPIs.GetUserDetails.GetUserEmail(
                    token, ipAddress, langCode, mainAccountUserId);

                // Subscription details (use main account id when staff)
                var subscriptionDetails = subscriptionReaderManager.GetSubscriptionDetails(
                    subscriptionRequestModel.Subscription.SubscriptionId,
                    !responseSession.IsStaff ? responseSession.UserId : mainAccountUserId,
                    responseSession.IsStaff);

                var subscriptionResponseModel = new SubscriptionResponseModel
                {
                    Subscriptions = new List<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.SubscriptionUserResponseModel>
            {
                subscriptionDetails
            },
                    Email = email,
                    MainAccountUserId = mainAccountUserId
                };

                return Ok(subscriptionResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// For commercial purposes only. 
        /// To get the profiles of the most recent subscription.
        /// Used for other APIs to find the profiles' information when sending the email details (summary) of the user subscription process.
        /// </summary>
        /// <returns>Returns the profiles of the last subscription.</returns>
        [HttpGet("profiles/last")]
        public IActionResult GetSubscriptionProfilesLastSubscription()
        {
            try
            {
                string langCode = RequestHandler.GetLanguage(HttpContext);

                // Token validation (Users API)
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    RequestHandler.GetToken(HttpContext),
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    "SubscriptionsClientRead",
                    langCode);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // Readers manager
                var subscriptionReadersManager = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);

                // Get user's subscriptions
                var userSubscriptions = subscriptionReadersManager.GetCustomerSubscriptionsByUserId(responseSession.UserId);

                if (userSubscriptions == null || userSubscriptions.Count == 0)
                    return Ok(new List<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionProfileModel>());

                // Last subscription
                var userSubscription = userSubscriptions[^1];

                // Profiles attached to last subscription
                var subscriptionProfiles =
                    subscriptionReadersManager.GetSubscriptionsProfilesAttachedToSubscription(userSubscription.SubscriptionId);

                if (subscriptionProfiles == null || subscriptionProfiles.Count == 0)
                    return Ok(new List<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionProfileModel>());

                return Ok(subscriptionProfiles);
            }
            catch
            {
                // Preserve original behavior: OK with empty list on error
                return Ok(new List<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionProfileModel>());
            }
        }


        /// <summary>
        /// To update or create the tries quiz. 
        /// DO NOT USE. This is still pending to be removed once we check it is not used in any other API or client.
        /// </summary>
        /// <param name="examTryMaxModel">To create the max try for exams.</param>
        /// <returns>Returns a HTTP Response with the try max model updated or created.</returns>
        [HttpPost("tries/quiz")]
        public IActionResult SetQuizTriesBySubscription([FromBody] ExamTryMaxModel examTryMaxModel)
        {
            try
            {
                // Token validation (Users API)
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionsClientRead", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // Basic payload validation
                if (string.IsNullOrWhiteSpace(examTryMaxModel.SubscriptionId) ||
                    string.IsNullOrWhiteSpace(examTryMaxModel.TryMax))
                    return BadRequest();

                // Check ownership/permissions
                var readerMgr = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], null);

                if (!readerMgr.CheckIfUserCanHandleSubscription(responseSession.UserId, examTryMaxModel.SubscriptionId))
                    return BadRequest();

                // Upsert max try
                var dao = new ExamTryMaxDAO();
                var readerConn = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"];
                var writerConn = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"];

                var existing = dao.Read(
                    readerConn,
                    new ExamTryMaxModel { SubscriptionId = examTryMaxModel.SubscriptionId },
                    true, true);

                if (existing.Count > 0)
                {
                    var updated = dao.Update(
                        writerConn,
                        new ExamTryMaxModel
                        {
                            ModifiedBy = responseSession.UserId,
                            TryMax = examTryMaxModel.TryMax
                        },
                        new ExamTryMaxModel { SubscriptionId = examTryMaxModel.SubscriptionId });

                    return Ok(updated);
                }
                else
                {
                    var created = dao.Create(writerConn, examTryMaxModel);
                    return StatusCode(201, created);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the max tries for exams according to subscription details.
        /// DO NOT USE. Pending removal once confirmed unused.
        /// </summary>
        [HttpGet("tries/quiz/{subscriptionId}")]
        public IActionResult GetQuizTriesBySubscription(string subscriptionId)
        {
            try
            {
                // Token validation (Users API)
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionsClientRead", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // Check ownership/permissions
                var readerMgr = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], null);

                if (!readerMgr.CheckIfUserCanHandleSubscription(responseSession.UserId, subscriptionId))
                    return BadRequest();

                // Read max try
                var list = new ExamTryMaxDAO().Read(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    new ExamTryMaxModel { SubscriptionId = subscriptionId },
                    true, true);

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Get the courses available for the student (based on active enrollments).
        /// </summary>
        [HttpGet("student/courses")]
        public IActionResult GetSubscriptionCoursesForStudent()
        {
            try
            {
                // Try cache first
                var token = RequestHandler.GetToken(HttpContext);
                var cached = _coursesCache.GetUserCourses(token);
                if (cached != null)
                    return Ok(cached);

                // Fallback: token validation (Users API)
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionUserProductRead", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // Query courses (Dapper)
                var userCourses =
                    SubscriptionsAPI.Infrastructure.DapperHandler.UserSubscriptions
                        .ReadUserCourses(responseSession.UserId);

                // Cache and return
                _coursesCache.SetStudentCourses(token, userCourses);
                return Ok(userCourses);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the client general subscriptions.
        /// </summary>
        /// <returns>Returns a list of general subscriptions.</returns>
        [HttpGet("subscription/user/general")]
        public IActionResult GetSubscriptionsStatusForClient()
        {
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                   RequestHandler.GetToken(HttpContext),
                   IPAddressDetector.GetClientIpAddress(HttpContext),
                   "SubscriptionUserProductRead", RequestHandler.GetLanguage(HttpContext));

                return Ok(new SubscriptionReadersManager(
                AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).
                GetClientGeneralSubscriptions(responseSession.UserId));
            }
            catch (Exception ex)
            {
                return Ok(new List<SubscriptionModel>());
            }
        }

        [HttpGet("user/courses")]
        public IActionResult GetSubscriptionUserCourses()
        {
            try
            {
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionUserProductRead", lang);

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var courses = UserSubscriptions.ReadUserCourses(session.UserId);
                return Ok(courses);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("user/available-items")]
        public IActionResult GetUserAvailableItems([FromBody] PermissionSessionRequestModel permissionSessionRequest)
        {
            var lang = RequestHandler.GetLanguage(HttpContext);
            var token = RequestHandler.GetToken(HttpContext);
            var ip = string.IsNullOrWhiteSpace(permissionSessionRequest.IpAddress)
                            ? IPAddressDetector.GetClientIpAddress(HttpContext)
                            : permissionSessionRequest.IpAddress;

            var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                token, ip, "SubscriptionUserProductRead", lang);

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            var reader = new SubscriptionReadersManager(
                AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], lang);

            var items = reader.GetAvailableItemsBySubscriptions(false, session.UserId);
            return Ok(items);
        }

        /// <summary>
        /// Returns the available courses of the user.
        /// </summary>
        /// <returns>A list of available courses.</returns>
        [HttpGet("user/available-courses-grades")]
        public IActionResult GetUserAvailableCourses()
        {
            try
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);

                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionUserProductRead", lang);

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var reader = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], lang);

                // If role "1" means admin/staff, preserve original logic
                bool includeAll = session.RoleId == "1";
                var subscriptionProfilesItems = reader.GetAvailableItemsBySubscriptions(includeAll, session.UserId);

                var subscriptionProductsToFind = new List<SubscriptionsAPI.Domain.Models.Generals.SubscriptionProductModel>();
                foreach (var p in subscriptionProfilesItems)
                {
                    subscriptionProductsToFind.Add(new SubscriptionProductModel { ProductId = p.ItemId });
                }

                var products = new SubscriptionProductDAO().Read(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    subscriptionProductsToFind);

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        // ---------------------------------------------
        // .NET 8 versions of the requested endpoints
        // Assumes this code is inside a ControllerBase with:
        //   private readonly IConfiguration _config;
        // And your helpers (RequestHandler, IPAddressDetector, etc.) have Core-friendly overloads.
        // ---------------------------------------------

        [HttpPost("advanced/read")]
        public IActionResult SubscriptionAdvaceRead([FromBody] SubscriptionRequestModel generalRequestModel)
        {
            var generalResponseModel = new SubscriptionsAdvanceResponseModel
            {
                Subscriptions = new List<SubscriptionAdvanceResponseModel>(),
            };

            try
            {
                var ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);
                var token = RequestHandler.GetToken(HttpContext);

                // Token validation
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ipAddress, "SubscriptionAdvanceRead", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // Managers
                var plansReaderManager = new PlansReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], lang);

                var subscriptionIds = new List<string>();
                var userReportResponseModel = new UserAdvanceResponseModel();

                // Optional user filters -> resolve user IDs
                if (!string.IsNullOrWhiteSpace(generalRequestModel.Username) ||
                    !string.IsNullOrWhiteSpace(generalRequestModel.Email) ||
                    !string.IsNullOrWhiteSpace(generalRequestModel.FirstName) ||
                    !string.IsNullOrWhiteSpace(generalRequestModel.LastName))
                {
                    userReportResponseModel = GetUserDetails.FilterUsersIds(
                        token,
                        ipAddress,
                        new UserReportsFilterRequestModel
                        {
                            Username = generalRequestModel.Username,
                            Email = generalRequestModel.Email,
                            FirstName = generalRequestModel.FirstName,
                            LastName = generalRequestModel.LastName,
                            IpAddress = ipAddress,
                            Token = token
                        });

                    if (userReportResponseModel?.UserInfo == null || userReportResponseModel.UserInfo.Count == 0)
                    {
                        generalResponseModel.PageTotals = 0;
                        return Ok(generalResponseModel);
                    }
                }

                if (generalRequestModel.SubscriptionIds != null && generalRequestModel.SubscriptionIds.Count > 0)
                    subscriptionIds.AddRange(generalRequestModel.SubscriptionIds);

                if (!string.IsNullOrWhiteSpace(generalRequestModel.Subscription?.SubscriptionId))
                    subscriptionIds.Add(generalRequestModel.Subscription.SubscriptionId.Trim());

                var subscriptionReadersManager = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], lang);

                // If user filters present, pull Stripe customer ids
                var customerModels = new List<CustomerModel>();
                if (userReportResponseModel?.UserInfo != null && userReportResponseModel.UserInfo.Count > 0)
                {
                    var customerModelListToFind = new List<CustomerModel>();
                    for (int i = 0; i < userReportResponseModel.UserInfo.Count; i++)
                    {
                        customerModelListToFind.Add(new CustomerModel
                        {
                            UserId = userReportResponseModel.UserInfo[i].UserId
                        });
                    }

                    customerModels = subscriptionReadersManager.GetCustomerModelList(customerModelListToFind);
                }

                // Resolve plan platform id if Plan.Id provided
                string planPlatformId = null;
                if (!string.IsNullOrWhiteSpace(generalRequestModel.Subscription?.Plan?.Id))
                {
                    planPlatformId = plansReaderManager.GetPlanPlatformId(
                        generalRequestModel.Subscription.Plan.Id.Trim());
                }

                // Status filter
                string statusCode = null;
                if (!string.IsNullOrWhiteSpace(generalRequestModel.Subscription?.Status))
                    statusCode = generalRequestModel.Subscription.Status;

                // Build search models (AND logic)
                var subscriptionModelToFindList = new List<SubscriptionModel>();

                if (customerModels.Count > 0)
                {
                    for (int i = 0; i < customerModels.Count; i++)
                    {
                        if (subscriptionIds.Count > 0)
                        {
                            for (int j = 0; j < subscriptionIds.Count; j++)
                            {
                                subscriptionModelToFindList.Add(new SubscriptionModel
                                {
                                    CustomerPlatformId = customerModels[i].PlatformId,
                                    Id = subscriptionIds[j],
                                    PlanPlatformId = planPlatformId,
                                    Status = statusCode,
                                    Type = generalRequestModel.Type
                                });
                            }
                        }
                        else
                        {
                            subscriptionModelToFindList.Add(new SubscriptionModel
                            {
                                CustomerPlatformId = customerModels[i].PlatformId,
                                PlanPlatformId = planPlatformId,
                                Status = statusCode,
                                Type = generalRequestModel.Type
                            });
                        }
                    }
                }
                else if (customerModels.Count == 0 && subscriptionIds.Count > 0)
                {
                    for (int j = 0; j < subscriptionIds.Count; j++)
                    {
                        subscriptionModelToFindList.Add(new SubscriptionModel
                        {
                            Id = subscriptionIds[j],
                            PlanPlatformId = planPlatformId,
                            Status = statusCode,
                            Type = generalRequestModel.Type
                        });
                    }
                }
                else if (customerModels.Count == 0 && subscriptionIds.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(planPlatformId) || !string.IsNullOrWhiteSpace(statusCode))
                    {
                        subscriptionModelToFindList.Add(new SubscriptionModel
                        {
                            Type = generalRequestModel.Type,
                            PlanPlatformId = planPlatformId,
                            Status = statusCode
                        });
                    }
                }

                // Ensure Type is set if provided
                if (!string.IsNullOrWhiteSpace(generalRequestModel.Type))
                {
                    if (subscriptionModelToFindList.Count == 0)
                    {
                        subscriptionModelToFindList.Add(new SubscriptionModel
                        {
                            Type = generalRequestModel.Type
                        });
                    }
                    else
                    {
                        for (int i = 0; i < subscriptionModelToFindList.Count; i++)
                            subscriptionModelToFindList[i].Type = generalRequestModel.Type;
                    }
                }

                // Execute search
                var subscriptionResults = subscriptionReadersManager.GetSubscriptionModels(subscriptionModelToFindList);

                // Paging
                var limiter = generalRequestModel.Limiter <= 0 ? 1 : generalRequestModel.Limiter;
                generalResponseModel.PageTotals = (int)Math.Ceiling(subscriptionResults.Count / (double)limiter);

                if (subscriptionResults.Count > 0)
                {
                    int startPage = generalRequestModel.PageFrom != 0
                        ? (generalRequestModel.PageFrom * limiter)
                        : 0;
                    int endPage = startPage + limiter;

                    if (startPage > subscriptionResults.Count) startPage = 0;
                    if (endPage > subscriptionResults.Count) endPage = subscriptionResults.Count;

                    var filtered = new List<SubscriptionModel>();
                    for (int i = startPage; i < endPage; i++)
                        filtered.Add(subscriptionResults[i]);

                    subscriptionResults = filtered;

                    if (subscriptionResults.Count == 0)
                    {
                        generalResponseModel.PageTotals = 0;
                        return Ok(generalResponseModel);
                    }

                    // If we already have user info from FilterUsersIds, use it
                    if (userReportResponseModel?.UserInfo != null && userReportResponseModel.UserInfo.Count > 0)
                    {
                        var planModelsToFindList = new List<PlanModel>();

                        for (int i = 0; i < subscriptionResults.Count; i++)
                        {
                            var s = subscriptionResults[i];
                            var subscriptionAdvance = new SubscriptionAdvanceResponseModel
                            {
                                Id = s.Id,
                                CurrentPeriodEnd = s.CurrentPeriodEnd,
                                CreatedAt = s.CreatedAt,
                                CustomerPlatformId = s.CustomerPlatformId,
                                PlanPlatformId = s.PlanPlatformId,
                                PlatformId = s.PlatformId,
                                UpdatedAt = s.UpdatedAt,
                                Status = s.Status,
                                Quantity = s.Quantity,
                                ModifiedBy = s.ModifiedBy
                            };

                            planModelsToFindList.Add(new PlanModel { PlatformId = s.PlanPlatformId });

                            var customerModel = customerModels.Find(x => x.PlatformId == s.CustomerPlatformId);
                            if (customerModel != null)
                            {
                                var user = userReportResponseModel.UserInfo.Find(x => x.UserId == customerModel.UserId);
                                if (user != null)
                                {
                                    subscriptionAdvance.Username = user.Username;
                                    subscriptionAdvance.FullName = $"{user.Surname}, {user.GivenName}";
                                    subscriptionAdvance.Email = user.Email;
                                }
                                subscriptionAdvance.UserId = customerModel.UserId;
                            }

                            generalResponseModel.Subscriptions.Add(subscriptionAdvance);
                        }

                        var planModelListResult = plansReaderManager.GetPlanModels(planModelsToFindList);
                        for (int i = 0; i < generalResponseModel.Subscriptions.Count; i++)
                        {
                            generalResponseModel.Subscriptions[i].Plan =
                                planModelListResult.Find(x => x.PlatformId == generalResponseModel.Subscriptions[i].PlanPlatformId);
                        }

                        return Ok(generalResponseModel);
                    }
                    else
                    {
                        // Need to fetch customers and user details
                        var customerToFindList = new List<CustomerModel>();
                        for (int i = 0; i < subscriptionResults.Count; i++)
                        {
                            customerToFindList.Add(new CustomerModel
                            {
                                PlatformId = subscriptionResults[i].CustomerPlatformId
                            });
                        }

                        customerModels.AddRange(subscriptionReadersManager.GetCustomerModelList(customerToFindList));

                        var userIdsToCallDetails = new List<string>();
                        var planModelListToFind = new List<PlanModel>();

                        for (int i = 0; i < subscriptionResults.Count; i++)
                        {
                            var s = subscriptionResults[i];
                            var customer = customerModels.Find(x => x.PlatformId == s.CustomerPlatformId);
                            if (customer != null) userIdsToCallDetails.Add(customer.UserId);

                            planModelListToFind.Add(new PlanModel { PlatformId = s.PlanPlatformId });
                        }

                        var userReportDetails = GetUserDetails.FilterUsersIds(token, ipAddress, new UserReportsFilterRequestModel
                        {
                            UserIds = userIdsToCallDetails,
                            IpAddress = ipAddress
                        });

                        var planModelListResult = plansReaderManager.GetPlanModels(planModelListToFind);

                        for (int i = 0; i < subscriptionResults.Count; i++)
                        {
                            var s = subscriptionResults[i];
                            var subscriptionAdvance = new SubscriptionAdvanceResponseModel
                            {
                                Id = s?.Id,
                                CurrentPeriodEnd = s?.CurrentPeriodEnd,
                                CreatedAt = s?.CreatedAt,
                                CustomerPlatformId = s?.CustomerPlatformId,
                                PlanPlatformId = s?.PlanPlatformId,
                                PlatformId = s?.PlatformId,
                                UpdatedAt = s?.UpdatedAt,
                                Status = s?.Status,
                                Quantity = s?.Quantity,
                                ModifiedBy = s?.ModifiedBy
                            };

                            subscriptionAdvance.Plan = planModelListResult.Find(x => x.PlatformId == s.PlanPlatformId);

                            var customerModel = customerModels.Find(x => x.PlatformId == s.CustomerPlatformId);
                            if (userReportDetails?.UserInfo != null && userReportDetails.UserInfo.Count > 0)
                            {
                                var user = userReportDetails.UserInfo.Find(x => x.UserId == customerModel?.UserId);
                                if (user != null)
                                {
                                    subscriptionAdvance.Username = user.Username;
                                    subscriptionAdvance.FullName = $"{user.Surname}, {user.GivenName}";
                                    subscriptionAdvance.Email = user.Email;
                                    subscriptionAdvance.UserId = user.UserId;
                                }
                            }

                            generalResponseModel.Subscriptions.Add(subscriptionAdvance);
                        }

                        return Ok(generalResponseModel);
                    }
                }

                return Ok(generalResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("admin/update/plan")]
        public IActionResult UpdateSubscriptionPlan([FromBody] SubscribeCustomerRequestModel subscribeCustomerRequestModel)
        {
            try
            {
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                // Token validation
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionUpdate", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // Managers
                var updater = new SubscriptionsUpdaterManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    lang);

                var reader = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    lang);

                var planReader = new PlansReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    lang);

                // Resolve platform ids
                var subscriptionModelList = reader.GetSubscriptionModels(new List<SubscriptionModel>
        {
            new SubscriptionModel { Id = subscribeCustomerRequestModel.SubscriptionId }
        });

                var planModelList = planReader.GetPlanModels(new List<PlanModel>
        {
            new PlanModel { Id = subscribeCustomerRequestModel.PlanId }
        });

                var responseSubscription = updater.UpdateSubscription(
                    new SubscriptionModel
                    {
                        PlanPlatformId = planModelList[0].PlatformId,
                        PlatformId = subscriptionModelList[0].PlatformId,
                        Quantity = subscribeCustomerRequestModel?.Quantity.ToString()
                    },
                    new SubscriptionModel
                    {
                        PlatformId = subscribeCustomerRequestModel.SubscriptionId
                    },
                    responseSession.UserId);

                var generalResponse = new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel
                {
                    Messages = responseSubscription.Messages
                };

                if (!responseSubscription.TransactionValid)
                    return BadRequest(generalResponse);

                return Ok(generalResponse);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("admin/cancellation")]
        public IActionResult CancelSubscription([FromBody] SubscriptionCancelationRequestModel subscribeCustomerRequestModel)
        {
            try
            {
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionCancellation", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var updater = new SubscriptionsUpdaterManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    lang);

                var stripeKey = AppConfiguration.Configuration["AppSettings:SubsStripeKey"];

                var responseModel = updater.CancelSubscription(
                    subscribeCustomerRequestModel.SubscriptionId,
                    subscribeCustomerRequestModel.CancelAtPeriodEnd,
                    subscribeCustomerRequestModel.CancellationReason,
                    subscribeCustomerRequestModel.CancellationReasonId,
                    responseSession.UserId,
                    stripeKey);

                var generalResponse = new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel
                {
                    Messages = responseModel.Messages
                };

                if (!responseModel.TransactionValid)
                    return BadRequest(generalResponse);

                return Ok(generalResponse);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("admin/cancellation/{subscriptionId}")]
        public IActionResult CancelSubscriptionStatus(string subscriptionId)
        {
            try
            {
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionCancellation", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var reader = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], lang);

                var cancellationResults = reader.GetCancellationLogModels(new List<CancellationLogModel>
        {
            new CancellationLogModel { SubscriptionId = subscriptionId }
        });

                var subscriptionsList = new List<SubscriptionUserResponseModel>();
                if (cancellationResults.Count > 0)
                {
                    subscriptionsList.Add(new SubscriptionUserResponseModel
                    {
                        CancellationLogs = cancellationResults
                    });
                }

                return Ok(subscriptionsList);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the subscriptions the client in session pays for (by its Stripe customer), with their plans.
        /// </summary>
        /// <param name="includeProfiles">
        /// True to also return, per subscription, the users related to it (subst_subscription_profiles) with their names and role
        /// (<see cref="SubscriptionUserResponseModel.SubscriptionProfileUsers"/>): one query for all the profiles and one Users API
        /// call each for names and roles, in parallel. Default false (the response is unchanged).
        /// </param>
        /// <param name="includeCourses">
        /// True to also return, per subscription, the courses of its plan's product (subject code, subject name, level)
        /// (<see cref="SubscriptionUserResponseModel.SubscriptionCourses"/>): one query for all the plans. Default false.
        /// </param>
        /// <returns>200 with the subscriptions; 401 if the session or the SubscriptionsClientRead permission is not valid.</returns>
        [HttpGet("clients")]
        public async Task<IActionResult> GetSubscriptionClients([FromQuery] bool includeProfiles = false, [FromQuery] bool includeCourses = false)
        {
            try
            {
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ip, "SubscriptionsClientRead", lang);

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var readConnStr = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"];
                var reader = new SubscriptionReadersManager(readConnStr, lang);

                var subs = reader.GetCustomerSubscriptionsByUserId(responseSession.UserId);

                // Related users and courses of every subscription, in parallel: one profiles query plus two Users API calls, and
                // one courses query, in total (not per subscription).
                var subscriptionIds = subs.Where(s => !string.IsNullOrWhiteSpace(s.SubscriptionId)).Select(s => s.SubscriptionId).Distinct().ToList();
                var profilesTask = includeProfiles && subscriptionIds.Count > 0
                    ? SubscriptionProfileUsersReader.ReadAsync(readConnStr, token, ip, lang, subscriptionIds, responseSession.UserId)
                    : null;
                var coursesTask = includeCourses
                    ? Task.Run(() => UserSubscriptions.ReadCoursesByPlanProductIds(subs.Select(s => s.Plan?.ProductId)))
                    : null;

                if (coursesTask != null)
                {
                    var coursesByProduct = (await coursesTask)
                        .Where(c => c.PlanProductId != null)
                        .GroupBy(c => c.PlanProductId!)
                        .ToDictionary(g => g.Key, g => g.Select(c => new SubscriptionProductModel
                        {
                            Id = c.ItemId,
                            ProductId = c.ItemId,
                            Name = c.ItemName,
                            Subject = c.Subject,
                            SubjectName = c.SubjectName,
                            Level = c.Level,
                        }).ToList());
                    foreach (var subscription in subs)
                    {
                        subscription.SubscriptionCourses = subscription.Plan?.ProductId != null && coursesByProduct.TryGetValue(subscription.Plan.ProductId, out var courses)
                            ? courses
                            : new List<SubscriptionProductModel>();
                    }
                }

                if (profilesTask != null)
                {
                    var profilesBySubscription = await profilesTask;
                    foreach (var subscription in subs)
                    {
                        subscription.SubscriptionProfileUsers = subscription.SubscriptionId != null && profilesBySubscription.TryGetValue(subscription.SubscriptionId, out var related)
                            ? related
                            : new List<SubscriptionProfileUserResponseModel>();
                    }
                }

                return Ok(subs);
            }
            catch
            {
                return BadRequest();
            }
        }

        // Assumes this code lives inside a ControllerBase with:
        //   private readonly IConfiguration _config;  // injected via ctor
        // And your helpers (RequestHandler, IPAddressDetector, etc.) have ASP.NET Core–friendly overloads.

        [HttpPost("plan-details")]
        public IActionResult GetSubscriptionWithPlan([FromBody] SubscriptionRequestModel subscriptionRequestModel)
        {
            // Resolve original IP if not provided
            string ipAddress = string.IsNullOrWhiteSpace(subscriptionRequestModel.IpAddress)
                ? IPAddressDetector.GetClientIpAddress(HttpContext)
                : subscriptionRequestModel.IpAddress;

            // Token validation
            var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                RequestHandler.GetToken(HttpContext),
                ipAddress,
                "SubscriptionUserProductRead",
                RequestHandler.GetLanguage(HttpContext));

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            var reader = new SubscriptionReadersManager(
                AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                RequestHandler.GetLanguage(HttpContext));

            var result = reader.GetSubscriptionDetailsWithPlan(subscriptionRequestModel.SubscriptionIds);
            return Ok(result);
        }

        [HttpGet("profiles/types")]
        public IActionResult GetSubscriptionsTypesForProfile()
        {
            // Token validation
            var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                RequestHandler.GetToken(HttpContext),
                IPAddressDetector.GetClientIpAddress(HttpContext),
                "SubscriptionUserProductRead",
                RequestHandler.GetLanguage(HttpContext));

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            var types = SubscriptionsAPI.Infrastructure.DapperHandler.UserSubscriptions
                .ReadProfileSubscriptionTypes(session.UserId);

            return Ok(types);
        }

        [HttpGet("active/periods/user")]
        public IActionResult GetActivePeriodsForUser([FromQuery] string? userId = null)
        {
            // Token validation
            var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                RequestHandler.GetToken(HttpContext),
                IPAddressDetector.GetClientIpAddress(HttpContext),
                "SubscriptionUserProductRead",
                RequestHandler.GetLanguage(HttpContext));

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            var academicPeriods = new List<AcademicPeriodModel>();

            // Get active profile subscriptions (enrollments)
            var profiles = SubscriptionsAPI.Infrastructure.DapperHandler.UserSubscriptions
                .ReadProfileSubscriptions(string.IsNullOrWhiteSpace(userId) ? session.UserId : userId, true);

            if (profiles == null || profiles.Count == 0)
                return Ok(academicPeriods);

            var subscriptionIds = new List<string>(profiles.Count);
            foreach (var sp in profiles)
                subscriptionIds.Add(sp.SubscriptionId);

            // Academic details from Schools service
            var entitySubs = SubscriptionsAPI.Services.FeignAPIs.SchoolsDetailsHandler
                .GetAcademicDetailsBySubscriptionIdList(subscriptionIds);

            if (entitySubs == null || entitySubs.Count == 0)
                return Ok(academicPeriods);

            // Collect unique academic periods
            foreach (var entity in entitySubs)
            {
                if (entity?.AcademicPeriodModelList == null) continue;

                foreach (var ap in entity.AcademicPeriodModelList)
                {
                    if (ap == null) continue;

                    if (academicPeriods.Count == 0 || academicPeriods.Find(x => x.Id == ap.Id) == null)
                        academicPeriods.Add(ap);
                }
            }

            return Ok(academicPeriods);
        }

        [HttpPost("profile-subscriptions")]
        public IActionResult GetAllSubscriptionProfiles([FromBody] SubscriptionRequestModel subscriptionRequestModel)
        {
            try
            {
                var lang = RequestHandler.GetLanguage(HttpContext);

                // Token validation
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    RequestHandler.GetToken(HttpContext),
                    string.IsNullOrWhiteSpace(subscriptionRequestModel.IpAddress)
                        ? IPAddressDetector.GetClientIpAddress(HttpContext)
                        : subscriptionRequestModel.IpAddress,
                    "SubscriptionsClientRead",
                    lang);

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var reader = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    lang);

                var allowed = new List<SubscriptionProfileModel>();

                if (!session.IsStaff)
                {
                    // Non-staff: ensure the requested subscription(s) belong to the caller
                    var userSubs = reader.GetCustomerSubscriptionsByUserId(session.UserId);

                    foreach (var sub in userSubs)
                    {
                        for (int i = 0; i < subscriptionRequestModel.SubscriptionIds.Count; i++)
                        {
                            if (sub.SubscriptionId == subscriptionRequestModel.SubscriptionIds[i])
                            {
                                allowed.Add(new SubscriptionProfileModel
                                {
                                    SubscriptionId = sub.SubscriptionId
                                });
                            }
                        }
                    }
                }
                else
                {
                    // Staff can request any of the provided subscription ids
                    for (int i = 0; i < subscriptionRequestModel.SubscriptionIds.Count; i++)
                    {
                        allowed.Add(new SubscriptionProfileModel
                        {
                            SubscriptionId = subscriptionRequestModel.SubscriptionIds[i]
                        });
                    }
                }

                if (allowed.Count == 0)
                    return Ok(new List<SubscriptionProfileModel>());

                var profiles = reader.GetSubscriptionProfiles(allowed);
                return Ok(profiles);
            }
            catch
            {
                // Preserve original behavior (Unauthorized on exception)
                return Unauthorized();
            }
        }

        // Assumptions:
        // - This code is inside a ControllerBase-derived class.
        // - IConfiguration _config is injected via constructor.
        // - Your helpers (RequestHandler, IPAddressDetector, GetUserDetails, etc.) have ASP.NET Core–friendly overloads.
        // - Replace namespace/type names as needed to match your solution.

        [HttpPost("attach-profile")]
        public IActionResult AttachProfileToSubscription([FromBody] ProfileAssignProductRequestModel requestModel)
        {
            var generalResponseModel = new RegisterProfileResponseModel();

            try
            {
                // Basic payload validation
                if (string.IsNullOrWhiteSpace(requestModel.ProfileId) ||
                    string.IsNullOrWhiteSpace(requestModel.RoleId) ||
                    string.IsNullOrWhiteSpace(requestModel.SubscriptionId))
                {
                    return BadRequest();
                }

                // Resolve request context
                string ipAddress = string.IsNullOrWhiteSpace(requestModel.IpAddress)
                    ? IPAddressDetector.GetClientIpAddress(HttpContext)
                    : requestModel.IpAddress;

                string langCode = RequestHandler.GetLanguage(HttpContext);
                string token = RequestHandler.GetToken(HttpContext);

                // Token validation
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ipAddress, "AssignSeat", langCode);

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                bool isStaff = session.IsStaff;

                // Reader manager
                var reader = new SubscriptionReadersManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);

                // Main account for the subscription
                string mainAccountUserId = reader.GetMainAccountUserIdBySubscriptionId(requestModel.SubscriptionId);

                // Ensure relationship between profile and main account (no-op if already related)
                GetUserDetails.RelateProfileToMainAccount(token, mainAccountUserId, requestModel.ProfileId);

                if (!isStaff)
                    mainAccountUserId = session.UserId;

                // Subscription details (includes profiles and capacity)
                var subDetails = reader.GetSubscriptionDetails(requestModel.SubscriptionId, mainAccountUserId, isStaff);
                if (subDetails == null)
                    return BadRequest(); // cannot proceed without subscription

                // Capacity validation
                if (!isStaff)
                {
                    if (subDetails.SubscriptionProfiles != null && subDetails.SubscriptionProfiles.Count > 0)
                    {
                        // If adding +1 exceeds capacity, run special student-only logic
                        if (int.TryParse(subDetails.ProfilesTotalQuantity, out var capacity) &&
                            subDetails.SubscriptionProfiles.Count + 1 > capacity)
                        {
                            var messageManager = new MessageManager(
                                AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                                langCode);

                            // Build userRolesIds list (profile IDs)
                            var userRolesIds = new List<string>();
                            foreach (var profile in subDetails.SubscriptionProfiles)
                                userRolesIds.Add(profile.ProfileId);

                            var discountOnlyStudents = AppConfiguration.Configuration["AppSettings:subr_discount_only_students"]; // "1" or "0"
                            var roleClientChild = AppConfiguration.Configuration["AppSettings:ur_role_client_child"];
                            var errorMsgCode = AppConfiguration.Configuration["AppSettings:gwp_message_code_add_profile_error"];

                            if (discountOnlyStudents == "1" && requestModel.RoleId == roleClientChild)
                            {
                                // Count students currently attached
                                var userRoles = GetUserDetails.GetUserRoles(token, ipAddress, userRolesIds);
                                int studentTotals = 0;

                                foreach (var profile in subDetails.SubscriptionProfiles)
                                {
                                    if (userRoles.Find(x => x.UserId == profile.ProfileId && x.RoleId == roleClientChild) != null)
                                        studentTotals++;
                                }

                                if (studentTotals >= capacity)
                                {
                                    generalResponseModel.Messages ??= new List<string>();
                                    generalResponseModel.Messages.Add(messageManager.GetMessageDetails(errorMsgCode));
                                    return BadRequest(generalResponseModel);
                                }
                            }
                            else if (discountOnlyStudents == "0" && requestModel.RoleId != roleClientChild)
                            {
                                generalResponseModel.Messages ??= new List<string>();
                                generalResponseModel.Messages.Add(messageManager.GetMessageDetails(errorMsgCode));
                                return BadRequest(generalResponseModel);
                            }
                        }
                    }
                }
                // Attach the profile
                var manager = new SubscriptionManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);

                var response = manager.AssignProfilesToSubscription(
                    subDetails,
                    new List<string> { requestModel.ProfileId },
                    session.UserId,
                    true);

                generalResponseModel.Messages = response.Messages;
                // Calculate remaining space assuming +1 for the new profile
                int.TryParse(subDetails.ProfilesTotalQuantity, out var totalQty);
                var currentCount = subDetails.SubscriptionProfiles?.Count ?? 0;
                generalResponseModel.SpacesInSubscriptionLeft = totalQty > currentCount + 1;

                if (!response.TransactionValid)
                    return BadRequest(generalResponseModel);

                return Ok(generalResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("attach-profiles")]
        public IActionResult AttachProfilesToSubscription([FromBody] ProfileAssignProductRequestModel requestModel)
        {
            var generalResponseModel = new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel();

            try
            {
                string ipAddress = string.IsNullOrWhiteSpace(requestModel.IpAddress)
                    ? IPAddressDetector.GetClientIpAddress(HttpContext)
                    : requestModel.IpAddress;

                string langCode = RequestHandler.GetLanguage(HttpContext);

                // Token validation
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    RequestHandler.GetToken(HttpContext), ipAddress, "AssignSeat", langCode);

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var manager = new SubscriptionManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);

                var response = manager.AssignProfilesToSubscription(
                    requestModel.SubscriptionId,
                    requestModel.ProfileIds,
                    session.UserId,
                    true);

                generalResponseModel.Messages = response.Messages;

                if (!response.TransactionValid)
                    return BadRequest(generalResponseModel);

                return Ok(generalResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("remove-profiles")]
        public IActionResult RemoveProfileFromSubscription([FromBody] ProfileAssignProductRequestModel requestModel)
        {
            try
            {
                // Token validation
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    RequestHandler.GetToken(HttpContext),
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    "AssignSeat",
                    RequestHandler.GetLanguage(HttpContext));

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var manager = new SubscriptionManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var response = manager.RemoveProfilesFromSubscription(
                    requestModel.SubscriptionId,
                    requestModel.ProfileIds);

                return response.TransactionValid ? Ok(response) : BadRequest(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("admin/subscribe-user")]
        public async Task<IActionResult> SubscribeUserToPlan([FromBody] SubscribeCustomerRequestModel subscribeCustomerRequestModel)
        {
            try
            {
                var token = RequestHandler.GetToken(HttpContext);
                var ipAddress = IPAddressDetector.GetClientIpAddress(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);

                // Token validation
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    token, ipAddress, "SubscribeUserByEmployee", lang);

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                // User address for tax or regional validation
                var userAddressModel = GetUserDetails.GetUserAddressByEmployee(
                    token, ipAddress, subscribeCustomerRequestModel.UserId);

                // Subscription manager
                var manager = new SubscriptionManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    lang);

                // Default Type if not defined
                if (string.IsNullOrWhiteSpace(subscribeCustomerRequestModel.Type))
                {
                    if (!string.IsNullOrWhiteSpace(subscribeCustomerRequestModel.SchoolId))
                        subscribeCustomerRequestModel.Type = "S";
                    else if (!string.IsNullOrWhiteSpace(subscribeCustomerRequestModel.ProgramId))
                        subscribeCustomerRequestModel.Type = "T";
                    else
                        subscribeCustomerRequestModel.Type = "H";
                }

                // Ensure Homeschool customer exists both locally and in Stripe
                if (subscribeCustomerRequestModel.Type == "H")
                {
                    var userEmail = GetUserDetails.GetUserEmail(
                        token, ipAddress, null, subscribeCustomerRequestModel.UserId);

                    // Check local customer
                    var existing = new CustomerDAO().Read(
                        AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                        new CustomerModel { UserId = subscribeCustomerRequestModel.UserId },
                        true, true);

                    var stripeKey = AppConfiguration.Configuration["AppSettings:SubsStripeKey"];

                    if (existing.Count == 0)
                    {
                        manager.CreateCustomer(userEmail, subscribeCustomerRequestModel.UserId, stripeKey, true);
                    }
                    else
                    {
                        bool existsInStripe = manager.CheckIfCustomerExistsInStripe(existing[0].PlatformId, stripeKey);
                        if (!existsInStripe)
                            manager.CreateCustomerInStripe(userEmail, subscribeCustomerRequestModel.UserId, stripeKey);
                    }
                }

                // Create subscription
                var createResponse = await manager.CreateUserSubscription(
                    subscribeCustomerRequestModel.UserId,
                    subscribeCustomerRequestModel.SubTokenStripe,
                    subscribeCustomerRequestModel.PlanId,
                    subscribeCustomerRequestModel.Quantity,
                    userAddressModel?.StateIsoCode,
                    userAddressModel?.CountryIsoCode2,
                     AppConfiguration.Configuration["AppSettings:SubsStripeKey"],
                    session.UserId,
                    subscribeCustomerRequestModel.CouponPlatformId,
                    subscribeCustomerRequestModel.PlatformCode,
                    subscribeCustomerRequestModel?.CurrentPeriodEnd,
                    subscribeCustomerRequestModel.Type
                );

                var generalResponseModel = new SubscriptionResponseModel
                {
                    Messages = createResponse.Messages
                };

                if (createResponse.TransactionValid)
                {
                    // Include new subscription id in response
                    generalResponseModel.Subscriptions = new List<SubscriptionUserResponseModel>
            {
                new SubscriptionUserResponseModel { SubscriptionId = createResponse.SubscriptionId }
            };

                    // If school/program provided, relate subscription to entities
                    if (!string.IsNullOrWhiteSpace(subscribeCustomerRequestModel.SchoolId) ||
                        !string.IsNullOrWhiteSpace(subscribeCustomerRequestModel.ProgramId))
                    {
                        try
                        {
                            var schoolIds = new List<string>();
                            if (!string.IsNullOrWhiteSpace(subscribeCustomerRequestModel.SchoolId))
                                schoolIds.Add(subscribeCustomerRequestModel.SchoolId);

                            var programIds = new List<string>();
                            if (!string.IsNullOrWhiteSpace(subscribeCustomerRequestModel.ProgramId))
                                programIds.Add(subscribeCustomerRequestModel.ProgramId);

                            var relateReq = new SubscriptionRelateRequestModel
                            {
                                SchoolIds = schoolIds,
                                ProgramIds = programIds,
                                IpAddress = ipAddress,
                                Token = token,
                                UserId = session.UserId,
                                SubscriptionId = createResponse.SubscriptionId,
                                EntityAcademicDetailId = subscribeCustomerRequestModel.EntityAcademicDetailId
                            };

                            SubscriptionsAPI.Services.FeignAPIs.SchoolsDetailsHandler
                                .RelateSubscriptionToEntitiesSchools(token, ipAddress, relateReq);
                        }
                        catch
                        {
                            // Non-fatal: relation can be retried later
                        }
                    }

                    return StatusCode(201, generalResponseModel);
                }

                // Failure path
                return BadRequest(generalResponseModel);
            }
            catch (Exception ex)
            {
                var errorResponse = new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel
                {
                    Messages = new List<string> { ex.ToString() }
                };
                return BadRequest(errorResponse);
            }
        }

        // Assumes this is inside a ControllerBase-derived class with IConfiguration _config injected.

        [HttpGet("student/is-homeschool")]
        public IActionResult IsStudentHomeschool()
        {
            var token = RequestHandler.GetToken(HttpContext);
            var ip = IPAddressDetector.GetClientIpAddress(HttpContext);
            var lang = RequestHandler.GetLanguage(HttpContext);

            // Validate session
            var session = GetUserDetails.GetUserIdRequestOnOtherAPI(
                token, ip, "SubscriptionUserProductRead", lang);

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            var reader = new SubscriptionReadersManager(
                AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                lang);

            var isHomeschool = reader.IsStudentHomeschool(session.UserId);
            return Ok(isHomeschool);
        }

    }
}