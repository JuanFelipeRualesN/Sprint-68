using System;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson.Response;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas
{
    /// <summary>
    /// To handle the additional material's responses, with the additional material response list.
    /// </summary>
    public class AthenasAdditionalMaterialResponseModel
    {
        /// <summary>
        /// To init default.
        /// </summary>
        public AthenasAdditionalMaterialResponseModel()
        {
            AdditionalMaterials = new List<LessonAdditionalMaterialResponseModel>();
        }
        /// <summary>
        /// Gets or sets the additional material lists.
        /// </summary>
        public List<LessonAdditionalMaterialResponseModel> AdditionalMaterials { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        public int TotalItems { get; set; }
    }
}
