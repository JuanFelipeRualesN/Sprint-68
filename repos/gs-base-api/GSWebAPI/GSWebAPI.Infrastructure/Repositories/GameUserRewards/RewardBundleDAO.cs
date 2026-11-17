using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameUserRewards
{
    class RewardBundleDAO
    {
        private MySqlConnection _connection;
        public List<RewardBundleModel> Read(string connectionString, RewardBundleModel rewardBundle, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `product_id_store`, `reward_type`, `reward_quantity`, `modified_by`, `created_at`, `updated_at`  FROM gr_reward_bundles ";
                {
                    if (rewardBundle != null)
                    {
                        if (!string.IsNullOrEmpty(rewardBundle.Id))
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
                            cmd.Parameters.AddWithValue("@id", rewardBundle.Id);
                        }
                        if (!string.IsNullOrEmpty(rewardBundle.ProductIdStore))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `product_id_store` = @productIdStore ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `product_id_store` = @productIdStore ");
                            }
                            cmd.Parameters.AddWithValue("@productIdStore", rewardBundle.ProductIdStore);
                        }
                        if (!string.IsNullOrEmpty(rewardBundle.RewardType))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reward_type` = @rewardType ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reward_type` = @rewardType ");
                            }
                            cmd.Parameters.AddWithValue("@rewardType", rewardBundle.RewardType);
                        }
                        if (!string.IsNullOrEmpty(rewardBundle.RewardQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reward_quantity` = @rewardQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reward_quantity` = @rewardQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@rewardQuantity", rewardBundle.RewardQuantity);
                        }
                        if (!string.IsNullOrEmpty(rewardBundle.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", rewardBundle.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(rewardBundle.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", rewardBundle.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(rewardBundle.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", rewardBundle.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader Reader = cmd.ExecuteReader();
                    try
                    {
                        if (Reader.HasRows)
                            while (Reader.Read())
                            {
                                if (!Reader.IsDBNull(0))
                                    rewardBundle.Id = Reader.GetValue(0).ToString();
                                else
                                    rewardBundle.Id = null;
                                if (!Reader.IsDBNull(1))
                                    rewardBundle.ProductIdStore = Reader.GetValue(1).ToString();
                                else
                                    rewardBundle.ProductIdStore = null;
                                if (!Reader.IsDBNull(2))
                                    rewardBundle.RewardType = Reader.GetValue(2).ToString();
                                else
                                    rewardBundle.RewardType = null;
                                if (!Reader.IsDBNull(3))
                                    rewardBundle.RewardQuantity = Reader.GetValue(3).ToString();
                                else
                                    rewardBundle.RewardQuantity = null;
                                if (!Reader.IsDBNull(4))
                                    rewardBundle.ModifiedBy = Reader.GetValue(4).ToString();
                                else
                                    rewardBundle.ModifiedBy = null;
                                if (!Reader.IsDBNull(5))
                                    rewardBundle.CreatedAt = Reader.GetValue(5).ToString();
                                else
                                    rewardBundle.CreatedAt = null;
                                if (!Reader.IsDBNull(6))
                                    rewardBundle.UpdatedAt = Reader.GetValue(6).ToString();
                                else
                                    rewardBundle.UpdatedAt = null;
                                response.Add(rewardBundle);
                                rewardBundle = new RewardBundleModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RewardBundleModel>();
                        Reader.Close();
                    }
                    finally
                    {
                        if (Reader != null)
                            Reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RewardBundleModel> Read(string connectionString, List<RewardBundleModel> rewardBundleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `product_id_store`, `reward_type`, `reward_quantity`, `modified_by`, `created_at`, `updated_at`  FROM gr_reward_bundles ";
                if (rewardBundleWhere != null && rewardBundleWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < rewardBundleWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(rewardBundleWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, rewardBundleWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(rewardBundleWhere[i].ProductIdStore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id_store` = @productIdStore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productIdStore" + i, rewardBundleWhere[i].ProductIdStore);
                        }
                        if (!string.IsNullOrEmpty(rewardBundleWhere[i].RewardType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reward_type` = @rewardType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rewardType" + i, rewardBundleWhere[i].RewardType);
                        }
                        if (!string.IsNullOrEmpty(rewardBundleWhere[i].RewardQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reward_quantity` = @rewardQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardBundleWhere[i].RewardQuantity);
                        }
                        if (!string.IsNullOrEmpty(rewardBundleWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardBundleWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(rewardBundleWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, rewardBundleWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(rewardBundleWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, rewardBundleWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != rewardBundleWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn;
                MySqlDataReader Reader = cmd.ExecuteReader();
                try
                {
                    if (Reader.HasRows)
                        while (Reader.Read())
                        {
                            RewardBundleModel rewardBundle = new RewardBundleModel(); if (!Reader.IsDBNull(0))
                                rewardBundle.Id = Reader.GetValue(0).ToString();
                            else
                                rewardBundle.Id = null;
                            if (!Reader.IsDBNull(1))
                                rewardBundle.ProductIdStore = Reader.GetValue(1).ToString();
                            else
                                rewardBundle.ProductIdStore = null;
                            if (!Reader.IsDBNull(2))
                                rewardBundle.RewardType = Reader.GetValue(2).ToString();
                            else
                                rewardBundle.RewardType = null;
                            if (!Reader.IsDBNull(3))
                                rewardBundle.RewardQuantity = Reader.GetValue(3).ToString();
                            else
                                rewardBundle.RewardQuantity = null;
                            if (!Reader.IsDBNull(4))
                                rewardBundle.ModifiedBy = Reader.GetValue(4).ToString();
                            else
                                rewardBundle.ModifiedBy = null;
                            if (!Reader.IsDBNull(5))
                                rewardBundle.CreatedAt = Reader.GetValue(5).ToString();
                            else
                                rewardBundle.CreatedAt = null;
                            if (!Reader.IsDBNull(6))
                                rewardBundle.UpdatedAt = Reader.GetValue(6).ToString();
                            else
                                rewardBundle.UpdatedAt = null;
                            response.Add(rewardBundle);
                            rewardBundle = new RewardBundleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RewardBundleModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RewardBundleModel> Create(string connectionString, RewardBundleModel rewardBundle)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gr_reward_bundles (";
                string values = "(";
                if (!string.IsNullOrEmpty(rewardBundle.Id))
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
                    cmd.Parameters.AddWithValue("@id", rewardBundle.Id);
                }
                if (!string.IsNullOrEmpty(rewardBundle.ProductIdStore))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `product_id_store` ");
                        values = string.Concat(values, ", @productIdStore");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `product_id_store` ");
                        values = string.Concat(values, " @productIdStore");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@productIdStore", rewardBundle.ProductIdStore);
                }
                if (!string.IsNullOrEmpty(rewardBundle.RewardType))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `reward_type` ");
                        values = string.Concat(values, ", @rewardType");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `reward_type` ");
                        values = string.Concat(values, " @rewardType");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardType", rewardBundle.RewardType);
                }
                if (!string.IsNullOrEmpty(rewardBundle.RewardQuantity))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `reward_quantity` ");
                        values = string.Concat(values, ", @rewardQuantity");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `reward_quantity` ");
                        values = string.Concat(values, " @rewardQuantity");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardQuantity", rewardBundle.RewardQuantity);
                }
                if (!string.IsNullOrEmpty(rewardBundle.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", rewardBundle.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(rewardBundle.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", rewardBundle.CreatedAt);
                }
                if (!string.IsNullOrEmpty(rewardBundle.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", rewardBundle.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                rewardBundle.Id = cmd.LastInsertedId.ToString();
                response.Add(rewardBundle);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
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
        public List<RewardBundleModel> Create(string connectionString, List<RewardBundleModel> rewardBundleList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardBundleList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gr_reward_bundles (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(rewardBundleList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, rewardBundleList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].ProductIdStore))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `product_id_store` ");
                                        values = string.Concat(values, ", @productIdStore" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `product_id_store` ");
                                        values = string.Concat(values, "@productIdStore" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@productIdStore" + i, rewardBundleList[i].ProductIdStore);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].RewardType))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `reward_type` ");
                                        values = string.Concat(values, ", @rewardType" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `reward_type` ");
                                        values = string.Concat(values, "@rewardType" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardType" + i, rewardBundleList[i].RewardType);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].RewardQuantity))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `reward_quantity` ");
                                        values = string.Concat(values, ", @rewardQuantity" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `reward_quantity` ");
                                        values = string.Concat(values, "@rewardQuantity" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardBundleList[i].RewardQuantity);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardBundleList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rewardBundleList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rewardBundleList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(rewardBundleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardBundleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardBundleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
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

        public List<RewardBundleModel> Update(string connectionString, RewardBundleModel rewardBundle, RewardBundleModel rewardBundleWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gr_reward_bundles SET ";
                if (!string.IsNullOrEmpty(rewardBundle.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", rewardBundle.Id);
                }
                if (!string.IsNullOrEmpty(rewardBundle.ProductIdStore))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `product_id_store` = @productIdStore");
                    else
                        sql = string.Concat(sql, " `product_id_store` = @productIdStore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@productIdStore", rewardBundle.ProductIdStore);
                }
                if (!string.IsNullOrEmpty(rewardBundle.RewardType))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reward_type` = @rewardType");
                    else
                        sql = string.Concat(sql, " `reward_type` = @rewardType");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardType", rewardBundle.RewardType);
                }
                if (!string.IsNullOrEmpty(rewardBundle.RewardQuantity))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reward_quantity` = @rewardQuantity");
                    else
                        sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardQuantity", rewardBundle.RewardQuantity);
                }
                if (!string.IsNullOrEmpty(rewardBundle.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", rewardBundle.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(rewardBundle.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", rewardBundle.CreatedAt);
                }
                if (!string.IsNullOrEmpty(rewardBundle.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", rewardBundle.UpdatedAt);
                }
                if (rewardBundleWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(rewardBundleWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", rewardBundleWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(rewardBundleWhere.ProductIdStore))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStoreWhere");
                        else
                            sql = string.Concat(sql, " `product_id_store` = @productIdStoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productIdStoreWhere", rewardBundleWhere.ProductIdStore);
                    }
                    if (!string.IsNullOrEmpty(rewardBundleWhere.RewardType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_type` = @rewardTypeWhere");
                        else
                            sql = string.Concat(sql, " `reward_type` = @rewardTypeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardTypeWhere", rewardBundleWhere.RewardType);
                    }
                    if (!string.IsNullOrEmpty(rewardBundleWhere.RewardQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantityWhere");
                        else
                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardQuantityWhere", rewardBundleWhere.RewardQuantity);
                    }
                    if (!string.IsNullOrEmpty(rewardBundleWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", rewardBundleWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(rewardBundleWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", rewardBundleWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(rewardBundleWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", rewardBundleWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(rewardBundle);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
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

        public List<RewardBundleModel> Update(string connectionString, List<RewardBundleModel> rewardBundleList, List<RewardBundleModel> rewardBundleWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardBundleList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gr_reward_bundles SET ";
                                if (!string.IsNullOrEmpty(rewardBundleList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, rewardBundleList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].ProductIdStore))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `product_id_store` = @productIdStore" + i);
                                    else
                                        sql = string.Concat(sql, " `product_id_store` = @productIdStore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@productIdStore" + i, rewardBundleList[i].ProductIdStore);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].RewardType))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reward_type` = @rewardType" + i);
                                    else
                                        sql = string.Concat(sql, " `reward_type` = @rewardType" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardType" + i, rewardBundleList[i].RewardType);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].RewardQuantity))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reward_quantity` = @rewardQuantity" + i);
                                    else
                                        sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardBundleList[i].RewardQuantity);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardBundleList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rewardBundleList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(rewardBundleList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rewardBundleList[i].UpdatedAt);
                                }
                                if (rewardBundleWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(rewardBundleWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, rewardBundleWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundleWhereList[i].ProductIdStore))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStoreWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id_store` = @productIdStoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productIdStoreWhere" + i, rewardBundleWhereList[i].ProductIdStore);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundleWhereList[i].RewardType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_type` = @rewardTypeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_type` = @rewardTypeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardTypeWhere" + i, rewardBundleWhereList[i].RewardType);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundleWhereList[i].RewardQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantityWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardQuantityWhere" + i, rewardBundleWhereList[i].RewardQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundleWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, rewardBundleWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundleWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, rewardBundleWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundleWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, rewardBundleWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rewardBundleList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardBundleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardBundleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
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

        public List<RewardBundleModel> Delete(string connectionString, RewardBundleModel rewardBundle)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gr_reward_bundles  ";
                if (rewardBundle != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(rewardBundle.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", rewardBundle.Id);
                    }
                    if (!string.IsNullOrEmpty(rewardBundle.ProductIdStore))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStore");
                        else
                            sql = string.Concat(sql, " `product_id_store` = @productIdStore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productIdStore", rewardBundle.ProductIdStore);
                    }
                    if (!string.IsNullOrEmpty(rewardBundle.RewardType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_type` = @rewardType");
                        else
                            sql = string.Concat(sql, " `reward_type` = @rewardType");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardType", rewardBundle.RewardType);
                    }
                    if (!string.IsNullOrEmpty(rewardBundle.RewardQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantity");
                        else
                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardQuantity", rewardBundle.RewardQuantity);
                    }
                    if (!string.IsNullOrEmpty(rewardBundle.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", rewardBundle.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(rewardBundle.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", rewardBundle.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(rewardBundle.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", rewardBundle.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(rewardBundle);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
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

        public List<RewardBundleModel> Delete(string connectionString, List<RewardBundleModel> rewardBundle)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardBundleModel> response = new List<RewardBundleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardBundle.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gr_reward_bundles  ";
                                if (rewardBundle[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(rewardBundle[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, rewardBundle[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundle[i].ProductIdStore))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStore" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id_store` = @productIdStore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productIdStore" + i, rewardBundle[i].ProductIdStore);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundle[i].RewardType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_type` = @rewardType" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_type` = @rewardType" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardType" + i, rewardBundle[i].RewardType);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundle[i].RewardQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantity" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardBundle[i].RewardQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundle[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardBundle[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundle[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, rewardBundle[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(rewardBundle[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, rewardBundle[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rewardBundle[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardBundleModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardBundleModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardBundleModel>();
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
