using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using Dapper;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Dapper;
using SchoolsAPI.CustomClasses;
using AcademicYearPeriodResponseModel =
    SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response.AcademicYearPeriodResponseModel;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.DapperHandler
{
    /// <summary>
    /// To handle details related to academic details using Dapper.
    /// Dapper is used to improve performance.
    /// </summary>
    public static class AcademicDetails
    {
        /// <summary>
        /// To get the general academic details.
        /// </summary>
        /// <param name="fullTermStartDate">(Optional) The start date.</param>
        /// <param name="fullTermEndDate">(Optional) The end date.</param>
        /// <param name="termId">(Optional) The term id.</param>
        /// <returns>Returns a list of academic details.</returns>
        public static List<AcademicDetailModel> GetAcademicDetails(string fullTermStartDate = null,
            string fullTermEndDate = null, string termId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();

            string query =
                @"SELECT * FROM gs_schools_api_db_prod.academic_details ";
            bool addAndFirstQuery = false;

            if (!string.IsNullOrWhiteSpace(fullTermStartDate))
            {
                query += " WHERE ";

                // Convert the date to date format
                var date = DateTime.Parse(fullTermStartDate);
                fullTermStartDate = date.ToString("yyyy-MM-dd");

                query += " full_term_start_date >= @fullTermStartDate ";
                parameters.Add("fullTermStartDate", fullTermStartDate);
                addAndFirstQuery = true;
            }

            if (!string.IsNullOrWhiteSpace(fullTermEndDate))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }

                // Convert the date to date format
                var date = DateTime.Parse(fullTermEndDate);
                fullTermEndDate = date.ToString("yyyy-MM-dd");

                query += " full_term_end_date <= @fullTermEndDate ";
                parameters.Add("fullTermEndDate", fullTermEndDate);
            }

            if (!string.IsNullOrWhiteSpace(termId))
            {
                if (addAndFirstQuery)
                {
                    query += " AND ";
                }
                else
                {
                    query += " WHERE ";
                }

                query += " term_id = @termId ";
                parameters.Add("termId", termId);
            }


            query += " ORDER BY id DESC;";

            var entityAcademicDetailModels = new List<AcademicDetailModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            try
            {
                entityAcademicDetailModels = connection.Query<AcademicDetailModel>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }

            return entityAcademicDetailModels;
        }

        /// <summary>
        /// To get the active academic periods id's
        /// </summary>
        /// <returns>Returns a list with the active academic periods id's</returns>
        public static List<int> GetActiveAcademicPeriodIds()
        {
            string query = @" SELECT id
                FROM gs_schools_api_db_prod.sch_entity_academic_details
                WHERE CURDATE() BETWEEN full_term_start_date AND full_term_end_date;
            ";

            using (var connection = new MySqlConnection(Settings.CONNECTION_STRING_READ))
            {
                connection.Open();
                return connection.Query<int>(query).ToList();
            }
        }

        /// <summary>
        /// To get the academic details by id.
        /// </summary>
        /// <param name="academicDetailId">The id of the academic detail.</param>
        /// <returns>Returns a list of academic details with full details.</returns>
        public static List<AcademicDetailsFullModel> GetEntityAcademicDetails(string academicDetailId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT 
                dt.id as `entity_academic_detail_id`, 
                dt.entity_subscription_id, 
                dt.term_id, 
                dt.period_quantity, 
                dt.days_to_close_period, 
                dt.full_term_start_date, 
                dt.full_term_end_date,
                dt.created_at as `entity_academic_detail_created_date`, 
                ap.id as `period_id`, 
                ap.period_number,
                ap.period_from, 
                ap.period_to,
                ap.created_at as `period_created_date`
                FROM gs_schools_api_db_prod.sch_entity_academic_details dt
                LEFT JOIN `gs_schools_api_db_prod`.`sch_academic_periods` ap ON ap.entity_academic_detail_id = dt.id 
                WHERE dt.id = @academicDetailId";
            var parameters = new { academicDetailId };
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            var listToReturn = connection.Query<AcademicDetailsFullModel>(query, parameters).ToList();
            connection.Close();
            return listToReturn;
        }

        /// <summary>
        /// To read the academic details by subscription ids list.
        /// </summary>
        /// <param name="subscriptionIds">Optional: The subscriptions' ids list.</param>
        /// <param name="entityId">Optional: The entity (school id).</param>
        /// <returns>Returns a list of the full entity academic details.</returns>
        public static List<EntityAcademicDetailFullResponseModel> GetAcademicDetailsWithPeriodsBySubscriptionId(List<string> subscriptionIds = null, string entityId = null)
        {
            // First, we get the subscription details according to entity subscriptions
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT `id`, `entity_id`, `subscription_id`, `platform_id`, `modified_by`, `created_at`, `updated_at` FROM `gs_schools_api_db_prod`.`sch_entity_subscriptions` ";
            DynamicParameters parameters = new DynamicParameters();
            bool addAndFirstQuery = false;
            if (subscriptionIds != null)
            {
                query += " WHERE ";
                string queryIn = " subscription_id IN (";
                for (int i = 0; i < subscriptionIds.Count; i++)
                {
                    queryIn += string.Concat("@subscriptionId", i.ToString());
                    parameters.Add(string.Concat("@subscriptionId", i.ToString()), subscriptionIds[i]);
                    if (i < subscriptionIds.Count - 1)
                    {
                        queryIn += ",";
                    }
                }
                queryIn += ") ";
                query += queryIn;
                addAndFirstQuery = true;
            }
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
                query += " entity_id = @entityId";
                parameters.Add("@entityId", entityId);
            }
            query += ";";


            List<EntitySubscriptionModel> entitySubscriptions = new List<EntitySubscriptionModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            entitySubscriptions = connection.Query<EntitySubscriptionModel>(query, parameters)?.ToList();
            connection.Close();
            if (entitySubscriptions == null || entitySubscriptions.Count == 0)
            {
                return new List<EntityAcademicDetailFullResponseModel>();
            }

            // Now get the entity subscription academic detail relations 
            string queryEntitySubscriptionRelation = @"SELECT entity_subscription_id, entity_academic_detail_id FROM gs_schools_api_db_prod.sch_entity_subscription_academic_details WHERE entity_subscription_id ";
            string queryEntity = @"SELECT `id`, `name`, `token`, `private`, `entity_type_id`, `modified_by`, `created_at`, `updated_at` FROM `gs_schools_api_db_prod`.`sch_entities` WHERE id ";

            DynamicParameters parametersEntitySubscriptionRelation = new DynamicParameters();
            DynamicParameters parametersEntity = new DynamicParameters();

            string queryEntitySubscriptionRelationIn = " IN (";
            string queryEntityIn = " IN (";
            for (int i = 0; i < entitySubscriptions.Count; i++)
            {
                queryEntitySubscriptionRelationIn += string.Concat("@entitySubscriptionId", i.ToString());
                parametersEntitySubscriptionRelation.Add(string.Concat("@entitySubscriptionId", i.ToString()), entitySubscriptions[i].Id);

                queryEntityIn += string.Concat("@entityId", i.ToString());
                parametersEntity.Add(string.Concat("@entityId", i.ToString()), entitySubscriptions[i].EntityId);

                if (i < entitySubscriptions.Count - 1)
                {
                    queryEntitySubscriptionRelationIn += ",";
                    queryEntityIn += ",";
                }
            }
            queryEntitySubscriptionRelation += queryEntitySubscriptionRelationIn + ");";
            queryEntity += queryEntityIn + ");";

            List<EntitySubscriptionAcademicDetailModel> entitySubscriptionAcademicDetails = new List<EntitySubscriptionAcademicDetailModel>();
            connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };

            connection.Open();
            entitySubscriptionAcademicDetails = connection.Query<EntitySubscriptionAcademicDetailModel>(queryEntitySubscriptionRelation, parametersEntitySubscriptionRelation).ToList();
            connection.Close();

            if (entitySubscriptionAcademicDetails == null || entitySubscriptionAcademicDetails.Count == 0)
            {
                return new List<EntityAcademicDetailFullResponseModel>();
            }

            // Now read the periods and the entity academic details 
            string queryEntityAcademicDetails = @"SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at` FROM `gs_schools_api_db_prod`.`sch_entity_academic_details` WHERE id ";
            DynamicParameters parametersEntityAcademicDetails = new DynamicParameters();
            string queryEntityAcademicDetailsIn = " IN (";
            for (int i = 0; i < entitySubscriptionAcademicDetails.Count; i++)
            {
                queryEntityAcademicDetailsIn += string.Concat("@entitySubscriptionAcademicDetailId", i.ToString());
                parametersEntityAcademicDetails.Add(string.Concat("@entitySubscriptionAcademicDetailId", i.ToString()), entitySubscriptionAcademicDetails[i].EntityAcademicDetailId);
                if (i < entitySubscriptionAcademicDetails.Count - 1)
                {
                    queryEntityAcademicDetailsIn += ",";
                }
            }
            queryEntityAcademicDetails += queryEntityAcademicDetailsIn + ");";

            List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel> entityAcademicDetailModels = new List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>();
            connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            entityAcademicDetailModels = connection.Query<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>(queryEntityAcademicDetails, parametersEntityAcademicDetails).ToList();
            connection.Close();

            if (entityAcademicDetailModels == null || entityAcademicDetailModels.Count == 0)
            {
                return new List<EntityAcademicDetailFullResponseModel>();
            }

            // Now get the academic periods
            string queryAcademics = @"SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at` FROM `gs_schools_api_db_prod`.`sch_academic_periods` WHERE entity_academic_detail_id ";
            DynamicParameters parametersEntityAcademicPeriods = new DynamicParameters();
            string queryAcademicsIn = " IN (";
            for (int i = 0; i < entityAcademicDetailModels.Count; i++)
            {
                queryAcademicsIn += string.Concat("@entityAcademicDetailId", i.ToString());
                parametersEntityAcademicPeriods.Add(string.Concat("@entityAcademicDetailId", i.ToString()), entityAcademicDetailModels[i].Id);
                if (i < entityAcademicDetailModels.Count - 1)
                {
                    queryAcademicsIn += ",";
                }
            }
            queryAcademics += queryAcademicsIn + "); ";
            List<AcademicPeriodModel> academicPeriods = new List<AcademicPeriodModel>();
            connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            academicPeriods = connection.Query<AcademicPeriodModel>(queryAcademics, parametersEntityAcademicPeriods).ToList();
            connection.Close();

            // Get the entity details
            List<EntityModel> entityModels = new List<EntityModel>();
            connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            entityModels = connection.Query<EntityModel>(queryEntity, parametersEntity).ToList();
            connection.Close();

            // Now, relate everything
            List<EntityAcademicDetailFullResponseModel> entityAcademicDetailFullResponseModels = new List<EntityAcademicDetailFullResponseModel>();
            foreach (EntitySubscriptionModel entitySubscription in entitySubscriptions)
            {

                EntityAcademicDetailFullResponseModel entityAcademicDetailFullResponse = new EntityAcademicDetailFullResponseModel();
                entityAcademicDetailFullResponse.EntitySubscriptionModel = entitySubscription;
                entityAcademicDetailFullResponse.EntityModel = entityModels.Find(x => x.Id == entitySubscription.EntityId);

                // Find the entity-subscription-academic relation
                EntitySubscriptionAcademicDetailModel entitySubscriptionAcademicDetail = entitySubscriptionAcademicDetails.Find(x => x.EntitySubscriptionId == entitySubscription.Id);
                if (entitySubscriptionAcademicDetail != null)
                {
                    entityAcademicDetailFullResponse.EntityAcademicDetailModel = entityAcademicDetailModels.Find(x => x.Id == entitySubscriptionAcademicDetail.EntityAcademicDetailId);
                    if (entityAcademicDetailFullResponse.EntityAcademicDetailModel != null)
                    {
                        entityAcademicDetailFullResponse.AcademicPeriodModelList = new List<AcademicPeriodModel>();
                        entityAcademicDetailFullResponse.AcademicPeriodModelList = academicPeriods.FindAll(x => x.EntityAcademicDetailId == entityAcademicDetailFullResponse.EntityAcademicDetailModel.Id);
                    }
                }
                entityAcademicDetailFullResponseModels.Add(entityAcademicDetailFullResponse);
            }

            return entityAcademicDetailFullResponseModels;
        }

        /// <summary>
        /// To read the academic details by entity academic detail id.
        /// </summary>
        /// <param name="entityAcademicDetailId">The entity academic detail id (relation between entity and academic details).</param>
        /// <returns>Returns the entity academic detail full response model.</returns>
        public static EntityAcademicDetailFullResponseModel GetAcademicDetails(string entityAcademicDetailId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            // Now read the periods and the entity academic details 
            string queryEntityAcademicDetails = @"SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at` FROM `gs_schools_api_db_prod`.`sch_entity_academic_details` WHERE id = @entityAcademicDetailId";
            var parameters = new { entityAcademicDetailId };
            List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel> entityAcademicDetailModels = new List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            entityAcademicDetailModels = connection.Query<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>(queryEntityAcademicDetails, parameters).ToList();
            connection.Close();

            if (entityAcademicDetailModels == null || entityAcademicDetailModels.Count == 0)
            {
                return new EntityAcademicDetailFullResponseModel();
            }

            // Now get the academic periods
            string queryAcademics = @"SELECT `id`, `entity_academic_detail_id`, `period_number`, `period_from`, `period_to`, `modified_by`, `created_at`, `updated_at` FROM `gs_schools_api_db_prod`.`sch_academic_periods` WHERE entity_academic_detail_id = @entityAcademicDetailId ";
            List<AcademicPeriodModel> academicPeriods = new List<AcademicPeriodModel>();
            connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            academicPeriods = connection.Query<AcademicPeriodModel>(queryAcademics, parameters).ToList();
            connection.Close();

            return new EntityAcademicDetailFullResponseModel()
            {
                EntityAcademicDetailModel = entityAcademicDetailModels[0],
                AcademicPeriodModelList = academicPeriods
            };
        }

        /// <summary>
        /// To get the entity academic detail models by entity id.
        /// </summary>
        /// <param name="entityId">The entity id (school id).</param>
        /// <returns>Returns a list of entity academic detail models.</returns>
        public static List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel> GetEntityAcademicModels(string entityId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, `full_term_start_date`, `full_term_end_date`, `modified_by`, `created_at`, `updated_at` FROM `gs_schools_api_db_prod`.`sch_entity_academic_details` WHERE `entity_id` = @entityId ORDER BY id DESC;";
            var parameters = new { entityId };
            List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel> entityAcademicDetailModels = new List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            try
            {
                entityAcademicDetailModels = connection.Query<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>(query, parameters).ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }
            return entityAcademicDetailModels;
        }

        /// <summary>
        /// To get the active academic models.
        /// </summary>
        /// <param name="entityIds">The schools to find.</param>
        /// <returns>Returns the list of entity academic details models.</returns>
        public static List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel> GetEntityAcademicModelsActive(List<string> entityIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"
                SELECT `id`, `entity_id`, `term_id`, `period_quantity`, `days_to_close_period`, 
                       `full_term_start_date`, `full_term_end_date`, `modified_by`, 
                       `created_at`, `updated_at`
                FROM `gs_schools_api_db_prod`.`sch_entity_academic_details`
                WHERE `entity_id` IN @entityIds 
                  AND CURDATE() BETWEEN `full_term_start_date` AND `full_term_end_date`
                ORDER BY id DESC;";

            var parameters = new { entityIds };
            List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel> entityAcademicDetailModels = new List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>();

            using (var connection = new MySqlConnection(Settings.CONNECTION_STRING_READ))
            {
                connection.Open();
                try
                {
                    entityAcademicDetailModels = connection.Query<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>(query, parameters).ToList();
                }
                catch (Exception ex)
                {
                    entityAcademicDetailModels = new List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>();
                    // Log the exception for debugging
                    Console.WriteLine($"Error fetching data: {ex.Message}");
                }
            }
            return entityAcademicDetailModels;
        }


        /// <summary>
        /// To realte an entity subscription with academic details.
        /// </summary>
        /// <param name="entitySubscriptionId">The entity subscription id (relation between subscription and entity).</param>
        /// <param name="entityAcademicDetailId">The entity academic detail id.</param>
        /// <param name="userInSession">The user making request.</param>
        public static void RelateEntitySubscriptionWithAcademicDetails(string entitySubscriptionId, string entityAcademicDetailId, string userInSession)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"INSERT INTO `gs_schools_api_db_prod`.`sch_entity_subscription_academic_details` (`entity_subscription_id`,`entity_academic_detail_id`, `modified_by`) VALUES (@entitySubscriptionId, @entityAcademicDetailId, @userId);";
            var parameters = new { entitySubscriptionId, entityAcademicDetailId, userId = userInSession };
            var connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };

            try
            {
                connection.Open();
                connection.Execute(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// To get the academic details by id.
        /// </summary>
        /// <param name="periodIdsToFindList">The period ids list.</param>
        /// <param name="entityId">The entity id to filter.</param>
        /// <returns>Returns a list of academic details with full details list.</returns>
        public static List<AcademicDetailsFullModel> GetFullDetailsPeriodsList(List<string> periodIdsToFindList, string entityId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string query = @"SELECT 
                dt.id as `entity_academic_detail_id`, 
                dt.entity_subscription_id, 
                dt.term_id, 
                dt.period_quantity, 
                dt.days_to_close_period, 
                dt.full_term_start_date, 
                dt.full_term_end_date,
                dt.created_at as `entity_academic_detail_created_date`, 
                ap.id as `period_id`, 
                ap.period_number,
                ap.period_from, 
                ap.period_to,
                ap.created_at as `period_created_date`,
                ap.modified_by as `period_modified_by`,
                ap.updated_at as `period_updated_date`
                FROM gs_schools_api_db_prod.sch_entity_academic_details dt
                LEFT JOIN `gs_schools_api_db_prod`.`sch_academic_periods` ap ON ap.entity_academic_detail_id = dt.id ";

            DynamicParameters parameters = new DynamicParameters();
            List<string> conditions = new List<string>();

            if (periodIdsToFindList != null && periodIdsToFindList.Count > 0)
            {
                conditions.Add("ap.id IN (" + string.Join(",", periodIdsToFindList.Select((_, i) => "@periodId" + i)) + ")");
                for (int i = 0; i < periodIdsToFindList.Count; i++)
                {
                    parameters.Add("@periodId" + i, periodIdsToFindList[i]);
                }
            }

            if (!string.IsNullOrEmpty(entityId))
            {
                conditions.Add("dt.entity_id = @entityId");
                parameters.Add("@entityId", entityId);
            }

            if (conditions.Any())
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            var listToReturn = connection.Query<AcademicDetailsFullModel>(query, parameters).ToList();
            connection.Close();
            return listToReturn;
        }

        /// <summary>
        /// To get the academic periods that include today's date, for the given entities.
        /// </summary>
        /// <remarks>
        /// The entity is read from sch_entity_academic_details.entity_id directly. Do not
        /// route this through sch_entity_subscriptions: that path relies on
        /// entity_academic_details.entity_subscription_id, which the schema marks as kept
        /// only for backwards compatibility and which is null on all but a handful of rows
        /// dating from 2022 - joining on it drops every current record.
        /// </remarks>
        /// <param name="entityIds">The list of entity ids.</param>
        /// <returns>Returns a list of current academic periods.</returns>
        public static List<AcademicYearPeriodResponseModel> GetCurrentAcademicPeriods(List<int> entityIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var parameters = new DynamicParameters();
            string query =
                @"SELECT pe.period_from, pe.period_to, pe.period_number,
                         ac.full_term_start_date, ac.full_term_end_date
                  FROM sch_academic_periods pe
                  INNER JOIN sch_entity_academic_details ac
                      ON ac.id = pe.entity_academic_detail_id
                  WHERE ac.entity_id IN @entityIds
                    AND CURDATE() BETWEEN pe.period_from AND pe.period_to;";
            parameters.Add("entityIds", entityIds);

            var currentAcademicPeriodModels = new List<AcademicYearPeriodResponseModel>();
            var connection = new MySqlConnection
            {
                ConnectionString = Settings.CONNECTION_STRING_READ
            };
            connection.Open();
            try
            {
                currentAcademicPeriodModels = connection.Query<AcademicYearPeriodResponseModel>(query, parameters).ToList();
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }
            return currentAcademicPeriodModels;
        }
    }
}