using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests
{
    public class GeneralRequestModel
    {

        /// <summary>
        /// Gets or sets the Ip Address. Used on some calls that the API to API feed is available.
        /// </summary>
        public string IpAddress { get; set; }
    }
}