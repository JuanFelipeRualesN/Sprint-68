using System;
using System.Collections.Generic;
using System.Configuration;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals;
using GSWebAPI.CustomClasses;

namespace GSWebAPI.GSWebAPI.Services.Generals
{

    /// <summary>
    /// Handle details related to the app (Unity) version.
    /// </summary>
    public class VersionManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="connectionStringRead">The connection string to read.</param>
        /// <param name="langCode">Language code.</param>
        public VersionManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the app versions.
        /// </summary>
        /// <param name="gameVersion">The version to read.</param>
        /// <returns>Returns the model list of game versions.</returns>
        public List<GameVersionModel> GetAppVersions(GameVersionModel gameVersion)
        {
            return new GameVersionDAO().Read(_connectionStringRead, gameVersion, false, true);
        }

        /// <summary>
        /// Add a new version. Updates the old one.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="dateStart">The date where the new version starts.</param>
        /// <param name="description">The description of the version.</param>
        /// <returns>Returns a general response with the transaction details.</returns>
        public GeneralResponseModel AddVersion(string version, string dateStart, string description)
        {
            // Validate if the date is correct.
            DateTime dateTryValid;
            if (DateTime.TryParse(dateStart, out dateTryValid))
            {
                dateTryValid = DateTime.Parse(dateStart);
            }
            else
            {
                // Invalid date
                return new GeneralResponseModel()
                {
                    TransactionValid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gwp_message_code_invalid_date_format"]),
                    },
                };
            }

            string dateTryValidString = dateTryValid.ToString("yyyy-MM-dd HH:mm:ss");

            GameVersionDAO gameVersionDAO = new GameVersionDAO();

            // Get the actual versions
            List<GameVersionModel> gameVersionModelList = gameVersionDAO.Read(_connectionStringRead, new GameVersionModel(), false, true);

            // First, we check if the version name already exists
            foreach (GameVersionModel gameVersion in gameVersionModelList)
            {
                // If the name of the version exists, return error.
                if (gameVersion.Version == version)
                {
                    return new GeneralResponseModel()
                    {
                        TransactionValid = false,
                        Messages = new List<string>()
                        {
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gwp_message_code_version_already_exists"]),
                        },
                    };
                }
            }

            // Else, continue with the update of the previous version

            // Update the version dates to stablish this new version release date as the end date of previous version
            gameVersionDAO.Update(_connectionString, new GameVersionModel { EndDate = dateTryValidString }, new GameVersionModel { Id = gameVersionModelList[0].Id });

            // Now create the new version
            gameVersionDAO.Create(_connectionString, new GameVersionModel()
            {
                Version = version,
                EndDate = "2099-12-31 00:00:00",
                ReleaseDate = dateTryValidString,
                Description = description,
            });

            return new GeneralResponseModel()
            {
                TransactionValid = true,
                Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:gwp_message_code_version_updated"]),
                    },
            };
        }
    }
}