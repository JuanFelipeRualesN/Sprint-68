using System;
using System.Collections.Generic;
using MySqlConnector;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions.CustomClasses;

namespace SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions
{
    class SubscriptionProfileDAO
    {
        private MySqlConnection _connection;
        public List<SubscriptionProfileModel> Read(string connectionString, SubscriptionProfileModel subscriptionProfile, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `subscription_id`, `profile_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_profiles ";
                {
                    if (subscriptionProfile != null)
                    {
                        if (!string.IsNullOrEmpty(subscriptionProfile.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", subscriptionProfile.Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfile.SubscriptionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subscription_id` = @subscriptionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subscription_id` = @subscriptionId ");
                            }
                            cmd.Parameters.AddWithValue("@subscriptionId", subscriptionProfile.SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfile.ProfileId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `profile_id` = @profileId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `profile_id` = @profileId ");
                            }
                            cmd.Parameters.AddWithValue("@profileId", subscriptionProfile.ProfileId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfile.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProfile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfile.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", subscriptionProfile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfile.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", subscriptionProfile.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    subscriptionProfile.Id = reader.GetValue(0).ToString();
                                else
                                    subscriptionProfile.Id = null;
                                if (!reader.IsDBNull(1))
                                    subscriptionProfile.SubscriptionId = reader.GetValue(1).ToString();
                                else
                                    subscriptionProfile.SubscriptionId = null;
                                if (!reader.IsDBNull(2))
                                    subscriptionProfile.ProfileId = reader.GetValue(2).ToString();
                                else
                                    subscriptionProfile.ProfileId = null;
                                if (!reader.IsDBNull(3))
                                    subscriptionProfile.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    subscriptionProfile.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    subscriptionProfile.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    subscriptionProfile.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    subscriptionProfile.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    subscriptionProfile.UpdatedAt = null;
                                response.Add(subscriptionProfile);
                                subscriptionProfile = new SubscriptionProfileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<SubscriptionProfileModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionProfileModel> Read(string connectionString, List<SubscriptionProfileModel> subscriptionProfileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `subscription_id`, `profile_id`, `modified_by`, `created_at`, `updated_at`  FROM subst_subscription_profiles ";
                if (subscriptionProfileWhere != null && subscriptionProfileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < subscriptionProfileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(subscriptionProfileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, subscriptionProfileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfileWhere[i].SubscriptionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subscription_id` = @subscriptionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionProfileWhere[i].SubscriptionId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfileWhere[i].ProfileId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `profile_id` = @profileId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@profileId" + i, subscriptionProfileWhere[i].ProfileId);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProfileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProfileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(subscriptionProfileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProfileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != subscriptionProfileWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            SubscriptionProfileModel subscriptionProfile = new SubscriptionProfileModel(); if (!reader.IsDBNull(0))
                                subscriptionProfile.Id = reader.GetValue(0).ToString();
                            else
                                subscriptionProfile.Id = null;
                            if (!reader.IsDBNull(1))
                                subscriptionProfile.SubscriptionId = reader.GetValue(1).ToString();
                            else
                                subscriptionProfile.SubscriptionId = null;
                            if (!reader.IsDBNull(2))
                                subscriptionProfile.ProfileId = reader.GetValue(2).ToString();
                            else
                                subscriptionProfile.ProfileId = null;
                            if (!reader.IsDBNull(3))
                                subscriptionProfile.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                subscriptionProfile.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                subscriptionProfile.CreatedAt = reader.GetValue(4).ToString();
                            else
                                subscriptionProfile.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                subscriptionProfile.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                subscriptionProfile.UpdatedAt = null;
                            response.Add(subscriptionProfile);
                            subscriptionProfile = new SubscriptionProfileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<SubscriptionProfileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<SubscriptionProfileModel> Create(string connectionString, SubscriptionProfileModel subscriptionProfile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO subst_subscription_profiles (";
                string values = "(";
                if (!string.IsNullOrEmpty(subscriptionProfile.Id))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `id` ");
                        values = string.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `id` ");
                        values = string.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", subscriptionProfile.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.SubscriptionId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subscription_id` ");
                        values = string.Concat(values, ", @subscriptionId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subscription_id` ");
                        values = string.Concat(values, " @subscriptionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", subscriptionProfile.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.ProfileId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `profile_id` ");
                        values = string.Concat(values, ", @profileId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `profile_id` ");
                        values = string.Concat(values, " @profileId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@profileId", subscriptionProfile.ProfileId);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `modified_by` ");
                        values = string.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `modified_by` ");
                        values = string.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProfile.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `created_at` ");
                        values = string.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `created_at` ");
                        values = string.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionProfile.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `updated_at` ");
                        values = string.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `updated_at` ");
                        values = string.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionProfile.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                subscriptionProfile.Id = cmd.LastInsertedId.ToString();
                response.Add(subscriptionProfile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }
        public List<SubscriptionProfileModel> Create(string connectionString, List<SubscriptionProfileModel> subscriptionProfileList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < subscriptionProfileList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO subst_subscription_profiles (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `id` ");
                                        values = string.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `id` ");
                                        values = string.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionProfileList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subscription_id` ");
                                        values = string.Concat(values, ", @subscriptionId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subscription_id` ");
                                        values = string.Concat(values, "@subscriptionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionProfileList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].ProfileId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `profile_id` ");
                                        values = string.Concat(values, ", @profileId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `profile_id` ");
                                        values = string.Concat(values, "@profileId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@profileId" + i, subscriptionProfileList[i].ProfileId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `modified_by` ");
                                        values = string.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `modified_by` ");
                                        values = string.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProfileList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `created_at` ");
                                        values = string.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `created_at` ");
                                        values = string.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProfileList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `updated_at` ");
                                        values = string.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `updated_at` ");
                                        values = string.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProfileList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(subscriptionProfileList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionProfileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionProfileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<SubscriptionProfileModel> Update(string connectionString, SubscriptionProfileModel subscriptionProfile, SubscriptionProfileModel subscriptionProfileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE subst_subscription_profiles SET ";
                if (!string.IsNullOrEmpty(subscriptionProfile.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", subscriptionProfile.Id);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.SubscriptionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId");
                    else
                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subscriptionId", subscriptionProfile.SubscriptionId);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.ProfileId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `profile_id` = @profileId");
                    else
                        sql = string.Concat(sql, " `profile_id` = @profileId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@profileId", subscriptionProfile.ProfileId);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProfile.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", subscriptionProfile.CreatedAt);
                }
                if (!string.IsNullOrEmpty(subscriptionProfile.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", subscriptionProfile.UpdatedAt);
                }
                if (subscriptionProfileWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(subscriptionProfileWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", subscriptionProfileWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfileWhere.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionIdWhere", subscriptionProfileWhere.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfileWhere.ProfileId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `profile_id` = @profileIdWhere");
                        else
                            sql = string.Concat(sql, " `profile_id` = @profileIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@profileIdWhere", subscriptionProfileWhere.ProfileId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfileWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", subscriptionProfileWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfileWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", subscriptionProfileWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfileWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", subscriptionProfileWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(subscriptionProfile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<SubscriptionProfileModel> Update(string connectionString, List<SubscriptionProfileModel> subscriptionProfileList, List<SubscriptionProfileModel> subscriptionProfileWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < subscriptionProfileList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE subst_subscription_profiles SET ";
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, subscriptionProfileList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].SubscriptionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subscription_id` = @subscriptionId" + i);
                                    else
                                        sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionProfileList[i].SubscriptionId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].ProfileId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `profile_id` = @profileId" + i);
                                    else
                                        sql = string.Concat(sql, " `profile_id` = @profileId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@profileId" + i, subscriptionProfileList[i].ProfileId);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProfileList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProfileList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(subscriptionProfileList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProfileList[i].UpdatedAt);
                                }
                                if (subscriptionProfileWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(subscriptionProfileWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, subscriptionProfileWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfileWhereList[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionIdWhere" + i, subscriptionProfileWhereList[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfileWhereList[i].ProfileId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `profile_id` = @profileIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `profile_id` = @profileIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@profileIdWhere" + i, subscriptionProfileWhereList[i].ProfileId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfileWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, subscriptionProfileWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfileWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, subscriptionProfileWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfileWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, subscriptionProfileWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionProfileList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionProfileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionProfileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                {
                    if (_connection != null)
                    {
                        dbConn.CloseConnection(_connection);
                    }
                }
            }
            return response;
        }

        public List<SubscriptionProfileModel> Delete(string connectionString, SubscriptionProfileModel subscriptionProfile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM subst_subscription_profiles  ";
                if (subscriptionProfile != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(subscriptionProfile.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", subscriptionProfile.Id);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfile.SubscriptionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId");
                        else
                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subscriptionId", subscriptionProfile.SubscriptionId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfile.ProfileId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `profile_id` = @profileId");
                        else
                            sql = string.Concat(sql, " `profile_id` = @profileId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@profileId", subscriptionProfile.ProfileId);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfile.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", subscriptionProfile.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfile.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", subscriptionProfile.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(subscriptionProfile.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", subscriptionProfile.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(subscriptionProfile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<SubscriptionProfileModel> Delete(string connectionString, List<SubscriptionProfileModel> subscriptionProfile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<SubscriptionProfileModel> response = new List<SubscriptionProfileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < subscriptionProfile.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM subst_subscription_profiles  ";
                                if (subscriptionProfile[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(subscriptionProfile[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, subscriptionProfile[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfile[i].SubscriptionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subscription_id` = @subscriptionId" + i);
                                        else
                                            sql = string.Concat(sql, " `subscription_id` = @subscriptionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subscriptionId" + i, subscriptionProfile[i].SubscriptionId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfile[i].ProfileId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `profile_id` = @profileId" + i);
                                        else
                                            sql = string.Concat(sql, " `profile_id` = @profileId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@profileId" + i, subscriptionProfile[i].ProfileId);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfile[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, subscriptionProfile[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfile[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, subscriptionProfile[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(subscriptionProfile[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, subscriptionProfile[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(subscriptionProfile[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<SubscriptionProfileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<SubscriptionProfileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<SubscriptionProfileModel>();
            }
            finally
            {
                {
                    if (_connection != null)
                    {
                        dbConn.CloseConnection(_connection);
                    }
                }
            }
            return response;
        }
    }
}
