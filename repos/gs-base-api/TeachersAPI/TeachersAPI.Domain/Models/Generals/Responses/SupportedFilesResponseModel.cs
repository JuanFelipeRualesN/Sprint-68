using TeachersAPI.Domain.Models.Generals;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To return the response of supported files.
    /// </summary>
    public class SupportedFilesResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the list of supported files.
        /// </summary>
        public List<SupportedFileModel> SupportedFileModelList { get; set; }
    }
}