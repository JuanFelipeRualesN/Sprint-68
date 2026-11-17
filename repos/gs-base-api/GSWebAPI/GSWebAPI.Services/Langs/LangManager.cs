using System.Collections.Generic;
using System.Linq;
using GSWebAPI.GSWebAPI.Domain.Models.Lang;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Langs;

namespace GSWebAPI.GSWebAPI.Services.Langs
{
    /// <summary>
    /// To handle the language manager.
    /// </summary>
    public class LangManager
    {
        private readonly string _connectionStringRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="LangManager"/> class.
        /// </summary>
        /// <param name="connectionStringRead">The connection string to read.</param>
        public LangManager(string connectionStringRead)
        {
            _connectionStringRead = connectionStringRead;
        }

        /// <summary>
        /// To get the languages.
        /// </summary>
        /// <returns>Returns a list of languages.</returns>
        public List<LangModel> GetLangs()
        {
            List<LangModel> langModelList = new List<LangModel>();
            LangModel langModel = new LangModel();
            LangDAO langDAO = new LangDAO();
            langModelList = langDAO.Read(_connectionStringRead, langModel, false, false);

            if (langModelList.Count > 0)
            {
                return langModelList.OrderBy(x => x.Desc).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
