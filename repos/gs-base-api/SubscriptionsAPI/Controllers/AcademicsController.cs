using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Schools;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.DapperHandler;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// To handle the academic details according to the user's enrollment. 
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/academics")]
    public class AcademicsController : ControllerBase
    {
        /// <summary>
        /// Returns the user academic years without taking in consideration if the subscription stills active or not.
        /// </summary>
        /// <param name="userId">Optional: The user's id.</param>
        /// <returns>Returns a HTTP response with the list of the academic years.</returns>
        [HttpGet("user/years")]
        public async Task<ActionResult<List<EntityAcademicDetailModel>>> GetUserAcademicYears([FromQuery] string userId = null)
        {
            /* Token validation in Users API */
            // Since this call is available from API to API feed, validate the original IP Address.
            ValidationResponseModel responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                RequestHandler.GetToken(HttpContext),
                IPAddressDetector.GetClientIpAddress(HttpContext),
                "SubscriptionUserProductRead", RequestHandler.GetLanguage(HttpContext));

            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            List<EntityAcademicDetailModel> academicYears = new List<EntityAcademicDetailModel>();

            // First, call the subscriptions 
            List<SubscriptionProfileModel> subscriptionProfiles =
                UserSubscriptions.ReadProfileSubscriptions(!string.IsNullOrWhiteSpace(userId) ? userId : responseSession.UserId, false);
            if (subscriptionProfiles == null || subscriptionProfiles.Count == 0)
            {
                return Ok(academicYears);
            }

            List<string> subscriptionIds = new List<string>();
            foreach (SubscriptionProfileModel subscriptionProfile in subscriptionProfiles)
            {
                subscriptionIds.Add(subscriptionProfile.SubscriptionId);
            }

            // Now get the academic details 
            List<EntityAcademicDetailFullResponseModel> entitySubscriptionPeriodsList =
                SchoolsDetailsHandler.GetAcademicDetailsBySubscriptionIdList(subscriptionIds);

            if (entitySubscriptionPeriodsList == null || entitySubscriptionPeriodsList.Count == 0)
            {
                return Ok(academicYears);
            }

            // Return just the AcademicPeriodModels 
            foreach (EntityAcademicDetailFullResponseModel entityAcademic in entitySubscriptionPeriodsList)
            {
                if (academicYears.Count > 0)
                {
                    EntityAcademicDetailModel academicExists = academicYears.Find(x => x.Id == entityAcademic.EntityAcademicDetailModel?.Id);
                    if (academicExists == null)
                    {
                        academicYears.Add(entityAcademic.EntityAcademicDetailModel);
                    }
                }
                else
                {
                    academicYears.Add(entityAcademic.EntityAcademicDetailModel);
                }
            }
            return Ok(academicYears);
        }
    }
}
