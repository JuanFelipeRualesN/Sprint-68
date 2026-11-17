using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP
{
    /// <summary>
    /// To handle the versions of the Unity APP.
    /// </summary>
    public class VersionResponseModel : GeneralResponseModel
    {
        /// <summary>
        /// Gets or sets the versions lists.
        /// </summary>
        public List<GameVersionModel> Versions { get; set; }
    }
}