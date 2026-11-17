using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses;
using Dapper;
using SchoolsAPI.CustomClasses;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.DapperHandler
{
    /// <summary>
    /// To handle details related to entities using Dapper. Dapper is used to improve performance.
    /// </summary>
    public class EntityDetails
    {
        /// <summary>
        /// To get the entities.
        /// </summary>
        /// <param name="entityId">Optional: The entity id (school's id).</param>
        /// <param name="entityTypeId">Optional: The entity type id.</param>
        /// <param name="entityName">Optional: The entity's name (school's name).</param>
        /// <param name="oreId">Optional: The school's ORE id. (region)</param>
        /// <param name="phone">Optional: The school's phone.</param>
        /// <param name="state">Optional: The school's state.</param>
        /// <param name="city">Optional: The school's city.</param>
        /// <param name="countryIsoCode">Optional: The school's country iso code.</param>
        /// <param name="privateOrPublic">Optional: If private or public.</param>
        /// <returns>Return a list of entity with general details.</returns>
        public static List<EntityGeneralDetailsResponseModel> GetEntities(string entityId = null, string entityTypeId = null, string entityName = null, string oreId = null, string phone = null, string state = null, string city = null, string countryIsoCode = null, string privateOrPublic = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT e.id as entity_id, e.name as entity_name, e.token as entity_identifier, e.entity_type_id, e.private as is_private, 
            ad.address_line_1, ad.address_line_2, ad.region_id as region, ad.state, ad.country, ad.city, ad.zip as zip_code, pho.phone
            FROM gs_schools_api_db_prod.sch_entities e
            LEFT JOIN `gs_schools_api_db_prod`.`sch_addresses` ad ON ad.entity_id = e.id 
            LEFT JOIN `gs_schools_api_db_prod`.`sch_phones` pho ON pho.entity_id = e.id ";

            DynamicParameters parameters = new DynamicParameters();
            bool addAndFirstQuery = false;

            if (!string.IsNullOrWhiteSpace(entityId))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " e.id = @entityId";
                parameters.Add("@entityId", entityId);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(entityTypeId))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " e.entity_type_id = @entityTypeId";
                parameters.Add("@entityTypeId", entityTypeId);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(entityName))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += @" e.name LIKE @entityName ";
                parameters.Add("@entityName", "%" + entityName + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(oreId))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " ad.region_id = @oreId";
                parameters.Add("@oreId", oreId);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " pho.phone LIKE @phone";
                parameters.Add("@phone", "%" + phone + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " ad.state LIKE @state";
                parameters.Add("@state", "%" + state + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " ad.city LIKE @city";
                parameters.Add("@city", "%" + city + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(countryIsoCode))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " ad.country_iso_code_2 LIKE @countryIsoCode";
                parameters.Add("@countryIsoCode", "%" + countryIsoCode + "%", System.Data.DbType.String, System.Data.ParameterDirection.Input);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(privateOrPublic))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }
                query += " e.private = @privateOrPublic";
                parameters.Add("@privateOrPublic", privateOrPublic);
                addAndFirstQuery = true;
            }

            query += " GROUP BY e.id";

            List<EntityGeneralDetailsResponseModel> items = new List<EntityGeneralDetailsResponseModel>();
            var connection = new MySqlConnection
            {
                // Change to this example, because is .Net 6: ;
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();

            try
            {

                items = connection.Query<EntityGeneralDetailsResponseModel>(query, parameters).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                items = new List<EntityGeneralDetailsResponseModel>();
            }
            finally
            {
                connection.Close();
            }

            return items;
        }
    }
}