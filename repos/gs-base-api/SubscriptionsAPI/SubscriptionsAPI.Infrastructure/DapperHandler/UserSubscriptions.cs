using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// To handle the user subscriptions (reads), using Dapper for faster reads.
    /// </summary>
    public static class UserSubscriptions
    {
        /// <summary>
        /// Read the profile subscription types.
        /// </summary>
        /// <param name="profileId">The profile's ids.</param>
        /// <returns>Returns a list of strings of the subscriptions types.</returns>
        public static List<SubscriptionProfileTryResponseModel> ReadProfileSubscriptionTypes(string profileId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var parameters = new { userId = profileId, date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
            string query = @"SELECT 
                subs.`type`,
                sup.subject, 
                sup.subject_name, 
                sup.level, 
                tr.try_max
                FROM gs_subscription_api_db_prod.subst_subscription_profiles prof 
                INNER JOIN gs_subscription_api_db_prod.subst_subscriptions subs ON subs.id = prof.subscription_id 
                INNER JOIN gs_subscription_api_db_prod.subst_plans pla ON pla.platform_id = subs.plan_platform_id
                INNER JOIN gs_subscription_api_db_prod.subst_product_item_products proi ON proi.product_id = pla.product_id 
                INNER JOIN gs_subscription_api_db_prod.subst_item_products ip ON ip.id = proi.item_id
                INNER JOIN gs_subscription_api_db_prod.gwp_subscription_products sup ON sup.product_id = ip.id
                LEFT JOIN gs_subscription_api_db_prod.gwp_exam_try_maxs tr ON tr.subscription_id = subs.id
                WHERE profile_id = @userId  AND subs.status = 'active' and STR_TO_DATE(current_period_end, '%m/%d/%Y %r') >= @date AND ip.available = 1";

            IEnumerable<SubscriptionProfileTryResponseModel> items = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"]
            };
            connection.Open();


            try
            {
                items = connection.Query<SubscriptionsAPI.Domain.Models.Generals.Responses.SubscriptionProfileTryResponseModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return items.ToList();
        }

        /// <summary>
        /// To read the courses of the user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>Returns the user courses details.</returns>
        public static SubscriptionsAPI.Domain.Models.Generals.Responses.UserCoursesResponseModel ReadUserCourses(string userId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var userCoursesResponseModel = new UserCoursesResponseModel
            {
                UserSubscribedLevelsSubjectsModelList = new List<SubscriptionsAPI.Domain.Models.Generals.Responses.SubsSubjectLevelCodesResponseModel>(),
            };

            /* string query = @"SELECT DISTINCT sup.product_id, sup.subject, sup.subject_name, sup.level, subs.type as subscription_type
                    FROM gs_subscription_api_db_prod.subst_subscription_profiles prof 
                    INNER JOIN gs_subscription_api_db_prod.subst_subscriptions subs ON subs.id = prof.subscription_id 
                    INNER JOIN gs_subscription_api_db_prod.subst_plans pla ON pla.platform_id = subs.plan_platform_id
                    INNER JOIN gs_subscription_api_db_prod.subst_product_item_products proi ON proi.product_id = pla.product_id 
                    INNER JOIN gs_subscription_api_db_prod.subst_item_products ip ON ip.id = proi.item_id
                    INNER JOIN gs_subscription_api_db_prod.gwp_subscription_products sup ON sup.product_id = ip.id
                    WHERE profile_id = @UserId  AND (subs.status = 'active' OR subs.status = 'trialing') and STR_TO_DATE(current_period_end, '%m/%d/%Y %H:%i:%s') >= @date AND ip.available = 1";
           
            var parameters = new { UserId = userId, Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };  */

            string query = @"SELECT DISTINCT sup.product_id, sup.subject, sup.subject_name, sup.level, subs.type as subscription_type
                    FROM gs_subscription_api_db_prod.subst_subscription_profiles prof 
                    INNER JOIN gs_subscription_api_db_prod.subst_subscriptions subs ON subs.id = prof.subscription_id 
                    INNER JOIN gs_subscription_api_db_prod.subst_plans pla ON pla.platform_id = subs.plan_platform_id
                    INNER JOIN gs_subscription_api_db_prod.subst_product_item_products proi ON proi.product_id = pla.product_id 
                    INNER JOIN gs_subscription_api_db_prod.subst_item_products ip ON ip.id = proi.item_id
                    INNER JOIN gs_subscription_api_db_prod.gwp_subscription_products sup ON sup.product_id = ip.id
                    WHERE profile_id = @UserId  AND (subs.status = 'active' OR subs.status = 'trialing') AND ip.available = 1";

            var parameters = new { UserId = userId };

            // Init test with dapper 
            IEnumerable<SubscriptionProductModel> itemProducts = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"]
            };
            connection.Open();
            try
            {
                itemProducts = connection.Query<SubscriptionProductModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            if (itemProducts != null)
            {
                foreach (var itemProduct in itemProducts)
                {
                    // Set the subscription
                    userCoursesResponseModel.UserSubscribedLevelsSubjectsModelList.Add(new SubsSubjectLevelCodesResponseModel
                    {
                        SubjectName = itemProduct.SubjectName,
                        Subject = itemProduct.Subject,
                        Level = itemProduct.Level,
                        ProductId = itemProduct.ProductId,
                        Name = itemProduct.Subject + " " + itemProduct.Level,
                        SubscriptionType = itemProduct.SubscriptionType
                    });
                }

                userCoursesResponseModel.SubscriptionType = userCoursesResponseModel.UserSubscribedLevelsSubjectsModelList.Count > 0 ? userCoursesResponseModel.UserSubscribedLevelsSubjectsModelList[0].SubscriptionType : null;
            }

            return userCoursesResponseModel;
        }

        /// <summary>
        /// To get the ids of the active subscriptions of the user.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="onlyActives">To call only the active subscriptions.</param>
        /// <returns>Return  the list of the subscription profile, just with the subscription id property.</returns>
        public static List<SubscriptionProfileModel> ReadProfileSubscriptions(string userId, bool onlyActives)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT DISTINCT subscription_id 
                FROM gs_subscription_api_db_prod.subst_subscription_profiles pro
                INNER JOIN gs_subscription_api_db_prod.subst_subscriptions subs ON subs.id = pro.subscription_id
                WHERE pro.profile_id = @profileId";
            parameters.Add("@profileId", userId);

            if (onlyActives)
            {
                query += " AND subs.status = 'active' AND STR_TO_DATE(current_period_end, '%m/%d/%Y')  >= NOW()";
            }
            query += ";";

            // Init test with dapper 
            IEnumerable<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionProfileModel> itemProducts = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"]
            };
            connection.Open();

            try
            {
                itemProducts = connection.Query<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionProfileModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            return itemProducts.ToList();
        }

        /// <summary>
        /// To read the profile full subscription items (courses).
        /// </summary>
        /// <param name="userId">The user id to retrieve the items.</param>
        /// <param name="onlyActives">true to call the only actives subscriptions, else false.</param>
        /// <returns>Returns the list of subscription full response (courses). </returns>
        public static List<SubscriptionFullResponseModel> ReadProfileSubscriptionsFullItems(string userId, bool onlyActives)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();

            string query = @"SELECT `subst_subscriptions`.`id`,
                `subst_subscriptions`.`platform_id`,
                `subst_subscriptions`.`platform_code`,
                `subst_subscriptions`.`customer_platform_id`,
                `subst_subscriptions`.`plan_platform_id`,
                `subst_subscriptions`.`quantity`,
                `subst_subscriptions`.`status`,
                `subst_subscriptions`.`current_period_end`,
                `subst_subscriptions`.`type`,
                `subst_subscriptions`.`modified_by`,
                `subst_subscriptions`.`created_at`,
                `subst_subscriptions`.`updated_at`
                FROM gs_subscription_api_db_prod.subst_subscription_profiles pro
                INNER JOIN gs_subscription_api_db_prod.subst_subscriptions subst_subscriptions ON subst_subscriptions.id = pro.subscription_id
                WHERE pro.profile_id = @profileId";
            parameters.Add("@profileId", userId);

            if (onlyActives)
            {
                query += " AND subst_subscriptions.status = 'active' AND STR_TO_DATE(current_period_end, '%m/%d/%Y')  >= NOW()";
            }
            query += ";";

            // Init test with dapper 
            IEnumerable<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionModel> subscriptions = null;
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"]
            };
            connection.Open();

            try
            {
                subscriptions = connection.Query<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionModel>(query, parameters);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            if (subscriptions == null || subscriptions.Count() == 0)
            {
                return new List<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionFullResponseModel>();
            }

            // For each of those plans, get the plans 
            List<string> planPlatformIdToFind = new List<string>();
            foreach (SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionModel subscription in subscriptions)
            {
                planPlatformIdToFind.Add(subscription.PlanPlatformId);
            }

            // Find the plans 
            DynamicParameters parametersPlans = new DynamicParameters();
            string queryPlan = @"SELECT * FROM gs_subscription_api_db_prod.subst_plans WHERE platform_id IN (";
            for (int i = 0; i < planPlatformIdToFind.Count; i++)
            {
                queryPlan += string.Concat(" @planPlatformIdToFind", i.ToString());
                parametersPlans.Add("@planPlatformIdToFind" + i.ToString(), planPlatformIdToFind[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                if (i < planPlatformIdToFind.Count - 1)
                {
                    queryPlan += ",";
                }
            }
            queryPlan += ") ";


            // Init test with dapper 
            IEnumerable<SubscriptionsAPI.Domain.Models.Subscriptions.PlanModel> plans = null;
            connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"]
            };
            connection.Open();

            try
            {
                plans = connection.Query<SubscriptionsAPI.Domain.Models.Subscriptions.PlanModel>(queryPlan, parametersPlans);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            // Find the item products of those plans 
            List<string> productIdsToFind = new List<string>();
            foreach (SubscriptionsAPI.Domain.Models.Subscriptions.PlanModel plan in plans)
            {
                productIdsToFind.Add(plan.ProductId);
            }

            // Find the plans 
            DynamicParameters parametersProducts = new DynamicParameters();
            string queryProduct = @"SELECT `subst_item_products`.`id`,
                `subst_item_products`.`name`,
                `subst_item_products`.`available`,
                `gsp`.`subject`,
                `gsp`.`level`,
                `subst_item_products`.`modified_by`,
                `subst_item_products`.`created_at`,
                `subst_item_products`.`updated_at`,
                `it`.`product_id`
                
            FROM `gs_subscription_api_db_prod`.`subst_item_products` subst_item_products 
            INNER JOIN `gs_subscription_api_db_prod`.`subst_product_item_products` it ON it.item_id = subst_item_products.id
            INNER JOIN `gs_subscription_api_db_prod`.`subst_products` pro ON pro.id = it.product_id
            INNER JOIN  `gs_subscription_api_db_prod`.`gwp_subscription_products` gsp ON gsp.product_id = subst_item_products.id
            WHERE it.product_id IN  (";
            for (int i = 0; i < productIdsToFind.Count; i++)
            {
                queryProduct += string.Concat(" @planPlatformIdToFind", i.ToString());
                parametersProducts.Add("@planPlatformIdToFind" + i.ToString(), productIdsToFind[i], System.Data.DbType.String, System.Data.ParameterDirection.Input);
                if (i < productIdsToFind.Count - 1)
                {
                    queryProduct += ",";
                }
            }
            queryProduct += ") ";
            if (onlyActives)
            {
                queryProduct += " AND pro.available = 1";
            }


            // Init test with dapper 
            IEnumerable<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.ItemResponseModel> itemsProducts = null;
            connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"]
            };
            connection.Open();

            try
            {
                itemsProducts = connection.Query<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.ItemResponseModel>(queryProduct, parametersProducts);
            }
            catch (Exception)
            {

            }
            finally
            {
                connection.Close();
            }


            // Now relate everything. 
            List<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionFullResponseModel> subscriptionFullResponseToReturn = new List<SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionFullResponseModel>();

            foreach (SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionModel subscription in subscriptions)
            {
                SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionFullResponseModel subscriptionToAdd = new SubscriptionFullResponseModel();
                subscriptionToAdd.SubscriptionModel = subscription;
                PlanModel planFound = plans.First(x => x.PlatformId == subscription.PlanPlatformId);
                subscriptionToAdd.PlanModel = planFound;

                if (itemsProducts != null && itemsProducts.Count() > 0)
                {
                    subscriptionToAdd.ItemResponseModelList = itemsProducts.Where(x => x.ProductId == planFound.ProductId).ToList();
                }
                subscriptionFullResponseToReturn.Add(subscriptionToAdd);
            }

            return subscriptionFullResponseToReturn;
        }
    }
}