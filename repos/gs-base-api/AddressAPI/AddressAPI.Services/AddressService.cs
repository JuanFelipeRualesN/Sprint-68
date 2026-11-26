using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressAPI.AddressAPI.Domain.Models.Address;
using AddressAPI.AddressAPI.Infrastructure.Address;
using MySqlConnector;

namespace AddressAPI.AddressAPI.Services
{

    /// <summary>
    /// To handle details related to address.
    /// </summary>
    public class AddressService
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language's code.</param>
        public AddressService(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Returns a list of state details according to the ISO Code Alpha 3 (Iso Code 2 in the DB). This was created to evaluate the state ISOs.
        /// </summary>
        /// <param name="countryIsoAlpha">The ISO Code Alpha 3. Example: USA, PRI.</param>
        /// <param name="onlyAvailables">true to call only the available states, else, false.</param>
        /// <returns>Returns a list of the country's states.</returns>
        public List<StatesRegionsProvidenceModel> GetStatesByCountryIso2(string countryIsoAlpha, bool onlyAvailables)
        {
            if(String.IsNullOrWhiteSpace(countryIsoAlpha))
            {
                return new List<StatesRegionsProvidenceModel>();
            }

            string isoCode = countryIsoAlpha;
            if (countryIsoAlpha.Length > 2)
            {
                isoCode = GetIso1FromIso2Country(countryIsoAlpha);
            }

            var statesRegionsProvidenceModel = new StatesRegionsProvidenceModel
            {
                CountryIsoCode = isoCode,
                Available = onlyAvailables ? "1" : null
            };

            return new StatesRegionsProvidenceDAO().Read(_connectionStringRead, statesRegionsProvidenceModel, false, false);
        }

        /// <summary>
        /// Returns the country iso code 1 by iso code 2. Example: US from USA.
        /// </summary>
        /// <param name="countryIsoAlpha3">The country iso alpha 3 code. Example: USA.</param>
        public string GetIso1FromIso2Country(string countryIsoAlpha3)
        {
            return new CountryDAO().Read(
                _connectionStringRead,
                new CountryModel { IsoCode2 = countryIsoAlpha3 },
                true,
                true
            )[0].IsoCode1;
        }

        /// <summary>
        /// Returns the country standard name with the country ISO Alpha 3.
        /// </summary>
        /// <param name="countryIsoAlpha3">The country ISO alpha code 3. Example: USA. </param>
        /// <returns>Retruns a string of the country's name.</returns>
        public string GetCountryStandardName(string countryIsoAlpha3)
        {
            var countryModel = new CountryModel { IsoCode2 = countryIsoAlpha3 };
            return new CountryDAO().Read(_connectionStringRead, countryModel, true, true)[0].Name;
        }

        /// <summary>
        /// Returns a list of countries by ISOs.
        /// </summary>
        /// <param name="isoList">The iso list to filter.</param>
        /// <returns>Returns a response iso model with the country's name, ISO, if contains states, etc.</returns>
        public List<ResponseISOModel> GetCountryNamesAndIsosByIsoList(List<string> isoList)
        {
            var countryModelListToFind = new List<CountryModel>();
            for (int i = 0; i < isoList.Count; i++)
            {
                countryModelListToFind.Add(new CountryModel { IsoCode2 = isoList[i] });
            }

            var countryDAO = new CountryDAO();
            var countryModelListResult = countryDAO.Read(_connectionStringRead, countryModelListToFind);

            var statesToFindList = new List<StatesRegionsProvidenceModel>();
            var countryLangModelListToFind = new List<CountryLangModel>();
            var countryLangDAO = new CountryLangDAO();

            for (int i = 0; i < countryModelListResult.Count; i++)
            {
                countryLangModelListToFind.Add(new CountryLangModel
                {
                    CountryId = countryModelListResult[i].Id,
                    LangCode = _langCode
                });

                statesToFindList.Add(new StatesRegionsProvidenceModel
                {
                    CountryIsoCode = countryModelListResult[i].IsoCode1
                });
            }

            var statesRegionsProvidenceDAO = new StatesRegionsProvidenceDAO();
            var statesRegionsProvidenceModelResult = statesRegionsProvidenceDAO.Read(_connectionStringRead, statesToFindList);
            var countryLangModelResultList = countryLangDAO.Read(_connectionStringRead, countryLangModelListToFind);

            // Finally, return the country with the ISO
            var responseIsoModelList = new List<ResponseISOModel>();
            for (int i = 0; i < countryModelListResult.Count; i++)
            {
                responseIsoModelList.Add(new ResponseISOModel
                {
                    CountryName = countryLangModelResultList.Find(x => x.CountryId == countryModelListResult[i].Id).Desc,
                    IsoCode2 = countryModelListResult[i].IsoCode2,
                    HasStates = statesRegionsProvidenceModelResult.Find(x => x.CountryIsoCode == countryModelListResult[i].IsoCode1) != null
                });
            }

            return responseIsoModelList;
        }

        /// <summary>
        /// Returns the countries' full details (with the translations and regions).
        /// </summary>
        /// <param name="filterByLangCode">If filter by language code.</param>
        /// <param name="onlyAvailables">true to call only the available countries and regions.</param>
        /// <returns>Returns a list of countries' full details.</returns>
        public List<ResponseCountryFullDetailsModel> GetCountryFullDetails(bool filterByLangCode, bool onlyAvailables)
        {
            // First, call the countries
            var countryModels = new CountryDAO().Read(
                _connectionStringRead,
                new CountryModel { Available = onlyAvailables ? "1" : null },
                false,
                false
            );

            // Then call lang descriptions and regions
            var countryLangModelsToSearch = new List<CountryLangModel>();
            var statesRegionsToSearch = new List<StatesRegionsProvidenceModel>();

            for (int i = 0; i < countryModels.Count; i++)
            {
                countryLangModelsToSearch.Add(new CountryLangModel
                {
                    CountryId = countryModels[i].Id,
                    LangCode = filterByLangCode ? _langCode : null
                });

                statesRegionsToSearch.Add(new StatesRegionsProvidenceModel
                {
                    CountryIsoCode = countryModels[i].IsoCode1,
                    Available = onlyAvailables ? "1" : null
                });
            }

            var countryLangModelsResults = new CountryLangDAO().Read(_connectionStringRead, countryLangModelsToSearch);
            var statesRegionsProvidenceModelsResults = new StatesRegionsProvidenceDAO().Read(_connectionStringRead, statesRegionsToSearch);

            // Now attach the details
            var responseCountryFull = new List<ResponseCountryFullDetailsModel>();
            for (int i = 0; i < countryModels.Count; i++)
            {
                responseCountryFull.Add(new ResponseCountryFullDetailsModel
                {
                    Name = countryModels[i].Name,
                    Id = countryModels[i].Id,
                    Available = countryModels[i].Available,
                    CreatedAt = countryModels[i].CreatedAt,
                    Domestic = countryModels[i].Domestic,
                    IsoCode1 = countryModels[i].IsoCode1,
                    IsoCode2 = countryModels[i].IsoCode2,
                    UpdatedAt = countryModels[i].UpdatedAt,
                    CountryNameTrans = countryLangModelsResults.FindAll(x => x.CountryId == countryModels[i].Id),
                    Regions = statesRegionsProvidenceModelsResults.FindAll(x => x.CountryIsoCode == countryModels[i].IsoCode1)
                });
            }

            return responseCountryFull;
        }
    }
}
