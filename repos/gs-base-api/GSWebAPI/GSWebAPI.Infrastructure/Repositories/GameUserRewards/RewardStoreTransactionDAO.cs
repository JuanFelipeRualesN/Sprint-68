using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameUserRewards
{
    class RewardStoreTransactionDAO
    {
        private MySqlConnection _connection;
        public List<RewardStoreTransactionModel> Read(string connectionString, RewardStoreTransactionModel rewardStoreTransaction, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `transaction_id`, `code`, `product_id_store`, `request_date`, `purchase_date`, `is_trial_period`, `modified_by`, `created_at`, `updated_at`  FROM gr_reward_store_transactions ";
                {
                    if (rewardStoreTransaction != null)
                    {
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.Id))
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
                            cmd.Parameters.AddWithValue("@id", rewardStoreTransaction.Id);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", rewardStoreTransaction.UserId);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.TransactionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `transaction_id` = @transactionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `transaction_id` = @transactionId ");
                            }
                            cmd.Parameters.AddWithValue("@transactionId", rewardStoreTransaction.TransactionId);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.Code))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `code` = @code ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `code` = @code ");
                            }
                            cmd.Parameters.AddWithValue("@code", rewardStoreTransaction.Code);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.ProductIdStore))
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
                            cmd.Parameters.AddWithValue("@productIdStore", rewardStoreTransaction.ProductIdStore);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.RequestDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `request_date` = @requestDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `request_date` = @requestDate ");
                            }
                            cmd.Parameters.AddWithValue("@requestDate", rewardStoreTransaction.RequestDate);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.PurchaseDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `purchase_date` = @purchaseDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `purchase_date` = @purchaseDate ");
                            }
                            cmd.Parameters.AddWithValue("@purchaseDate", rewardStoreTransaction.PurchaseDate);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.IsTrialPeriod))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `is_trial_period` = @isTrialPeriod ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `is_trial_period` = @isTrialPeriod ");
                            }
                            cmd.Parameters.AddWithValue("@isTrialPeriod", rewardStoreTransaction.IsTrialPeriod);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", rewardStoreTransaction.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", rewardStoreTransaction.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransaction.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", rewardStoreTransaction.UpdatedAt);
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
                                    rewardStoreTransaction.Id = Reader.GetValue(0).ToString();
                                else
                                    rewardStoreTransaction.Id = null;
                                if (!Reader.IsDBNull(1))
                                    rewardStoreTransaction.UserId = Reader.GetValue(1).ToString();
                                else
                                    rewardStoreTransaction.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    rewardStoreTransaction.TransactionId = Reader.GetValue(2).ToString();
                                else
                                    rewardStoreTransaction.TransactionId = null;
                                if (!Reader.IsDBNull(3))
                                    rewardStoreTransaction.Code = Reader.GetValue(3).ToString();
                                else
                                    rewardStoreTransaction.Code = null;
                                if (!Reader.IsDBNull(4))
                                    rewardStoreTransaction.ProductIdStore = Reader.GetValue(4).ToString();
                                else
                                    rewardStoreTransaction.ProductIdStore = null;
                                if (!Reader.IsDBNull(5))
                                    rewardStoreTransaction.RequestDate = Reader.GetValue(5).ToString();
                                else
                                    rewardStoreTransaction.RequestDate = null;
                                if (!Reader.IsDBNull(6))
                                    rewardStoreTransaction.PurchaseDate = Reader.GetValue(6).ToString();
                                else
                                    rewardStoreTransaction.PurchaseDate = null;
                                if (!Reader.IsDBNull(7))
                                    rewardStoreTransaction.IsTrialPeriod = Reader.GetValue(7).ToString();
                                else
                                    rewardStoreTransaction.IsTrialPeriod = null;
                                if (!Reader.IsDBNull(8))
                                    rewardStoreTransaction.ModifiedBy = Reader.GetValue(8).ToString();
                                else
                                    rewardStoreTransaction.ModifiedBy = null;
                                if (!Reader.IsDBNull(9))
                                    rewardStoreTransaction.CreatedAt = Reader.GetValue(9).ToString();
                                else
                                    rewardStoreTransaction.CreatedAt = null;
                                if (!Reader.IsDBNull(10))
                                    rewardStoreTransaction.UpdatedAt = Reader.GetValue(10).ToString();
                                else
                                    rewardStoreTransaction.UpdatedAt = null;
                                response.Add(rewardStoreTransaction);
                                rewardStoreTransaction = new RewardStoreTransactionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RewardStoreTransactionModel>();
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
                response = new List<RewardStoreTransactionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RewardStoreTransactionModel> Read(string connectionString, List<RewardStoreTransactionModel> rewardStoreTransactionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `transaction_id`, `code`, `product_id_store`, `request_date`, `purchase_date`, `is_trial_period`, `modified_by`, `created_at`, `updated_at`  FROM gr_reward_store_transactions ";
                if (rewardStoreTransactionWhere != null && rewardStoreTransactionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < rewardStoreTransactionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, rewardStoreTransactionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, rewardStoreTransactionWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].TransactionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `transaction_id` = @transactionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@transactionId" + i, rewardStoreTransactionWhere[i].TransactionId);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, rewardStoreTransactionWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].ProductIdStore))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `product_id_store` = @productIdStore" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@productIdStore" + i, rewardStoreTransactionWhere[i].ProductIdStore);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].RequestDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `request_date` = @requestDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@requestDate" + i, rewardStoreTransactionWhere[i].RequestDate);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].PurchaseDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `purchase_date` = @purchaseDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@purchaseDate" + i, rewardStoreTransactionWhere[i].PurchaseDate);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].IsTrialPeriod))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `is_trial_period` = @isTrialPeriod" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@isTrialPeriod" + i, rewardStoreTransactionWhere[i].IsTrialPeriod);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardStoreTransactionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, rewardStoreTransactionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(rewardStoreTransactionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, rewardStoreTransactionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != rewardStoreTransactionWhere.Count - 1)
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
                            RewardStoreTransactionModel rewardStoreTransaction = new RewardStoreTransactionModel(); if (!Reader.IsDBNull(0))
                                rewardStoreTransaction.Id = Reader.GetValue(0).ToString();
                            else
                                rewardStoreTransaction.Id = null;
                            if (!Reader.IsDBNull(1))
                                rewardStoreTransaction.UserId = Reader.GetValue(1).ToString();
                            else
                                rewardStoreTransaction.UserId = null;
                            if (!Reader.IsDBNull(2))
                                rewardStoreTransaction.TransactionId = Reader.GetValue(2).ToString();
                            else
                                rewardStoreTransaction.TransactionId = null;
                            if (!Reader.IsDBNull(3))
                                rewardStoreTransaction.Code = Reader.GetValue(3).ToString();
                            else
                                rewardStoreTransaction.Code = null;
                            if (!Reader.IsDBNull(4))
                                rewardStoreTransaction.ProductIdStore = Reader.GetValue(4).ToString();
                            else
                                rewardStoreTransaction.ProductIdStore = null;
                            if (!Reader.IsDBNull(5))
                                rewardStoreTransaction.RequestDate = Reader.GetValue(5).ToString();
                            else
                                rewardStoreTransaction.RequestDate = null;
                            if (!Reader.IsDBNull(6))
                                rewardStoreTransaction.PurchaseDate = Reader.GetValue(6).ToString();
                            else
                                rewardStoreTransaction.PurchaseDate = null;
                            if (!Reader.IsDBNull(7))
                                rewardStoreTransaction.IsTrialPeriod = Reader.GetValue(7).ToString();
                            else
                                rewardStoreTransaction.IsTrialPeriod = null;
                            if (!Reader.IsDBNull(8))
                                rewardStoreTransaction.ModifiedBy = Reader.GetValue(8).ToString();
                            else
                                rewardStoreTransaction.ModifiedBy = null;
                            if (!Reader.IsDBNull(9))
                                rewardStoreTransaction.CreatedAt = Reader.GetValue(9).ToString();
                            else
                                rewardStoreTransaction.CreatedAt = null;
                            if (!Reader.IsDBNull(10))
                                rewardStoreTransaction.UpdatedAt = Reader.GetValue(10).ToString();
                            else
                                rewardStoreTransaction.UpdatedAt = null;
                            response.Add(rewardStoreTransaction);
                            rewardStoreTransaction = new RewardStoreTransactionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RewardStoreTransactionModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardStoreTransactionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RewardStoreTransactionModel> Create(string connectionString, RewardStoreTransactionModel rewardStoreTransaction)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gr_reward_store_transactions (";
                string values = "(";
                if (!string.IsNullOrEmpty(rewardStoreTransaction.Id))
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
                    cmd.Parameters.AddWithValue("@id", rewardStoreTransaction.Id);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.UserId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `user_id` ");
                        values = string.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `user_id` ");
                        values = string.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", rewardStoreTransaction.UserId);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.TransactionId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `transaction_id` ");
                        values = string.Concat(values, ", @transactionId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `transaction_id` ");
                        values = string.Concat(values, " @transactionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@transactionId", rewardStoreTransaction.TransactionId);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.Code))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `code` ");
                        values = string.Concat(values, ", @code");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `code` ");
                        values = string.Concat(values, " @code");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", rewardStoreTransaction.Code);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.ProductIdStore))
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
                    cmd.Parameters.AddWithValue("@productIdStore", rewardStoreTransaction.ProductIdStore);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.RequestDate))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `request_date` ");
                        values = string.Concat(values, ", @requestDate");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `request_date` ");
                        values = string.Concat(values, " @requestDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@requestDate", rewardStoreTransaction.RequestDate);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.PurchaseDate))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `purchase_date` ");
                        values = string.Concat(values, ", @purchaseDate");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `purchase_date` ");
                        values = string.Concat(values, " @purchaseDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@purchaseDate", rewardStoreTransaction.PurchaseDate);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.IsTrialPeriod))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `is_trial_period` ");
                        values = string.Concat(values, ", @isTrialPeriod");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `is_trial_period` ");
                        values = string.Concat(values, " @isTrialPeriod");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isTrialPeriod", rewardStoreTransaction.IsTrialPeriod);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", rewardStoreTransaction.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", rewardStoreTransaction.CreatedAt);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", rewardStoreTransaction.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                rewardStoreTransaction.Id = cmd.LastInsertedId.ToString();
                response.Add(rewardStoreTransaction);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardStoreTransactionModel>();
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
        public List<RewardStoreTransactionModel> Create(string connectionString, List<RewardStoreTransactionModel> rewardStoreTransactionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardStoreTransactionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gr_reward_store_transactions (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, rewardStoreTransactionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `user_id` ");
                                        values = string.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `user_id` ");
                                        values = string.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, rewardStoreTransactionList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].TransactionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `transaction_id` ");
                                        values = string.Concat(values, ", @transactionId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `transaction_id` ");
                                        values = string.Concat(values, "@transactionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@transactionId" + i, rewardStoreTransactionList[i].TransactionId);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].Code))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `code` ");
                                        values = string.Concat(values, ", @code" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `code` ");
                                        values = string.Concat(values, "@code" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, rewardStoreTransactionList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].ProductIdStore))
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
                                    cmd.Parameters.AddWithValue("@productIdStore" + i, rewardStoreTransactionList[i].ProductIdStore);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].RequestDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `request_date` ");
                                        values = string.Concat(values, ", @requestDate" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `request_date` ");
                                        values = string.Concat(values, "@requestDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@requestDate" + i, rewardStoreTransactionList[i].RequestDate);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].PurchaseDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `purchase_date` ");
                                        values = string.Concat(values, ", @purchaseDate" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `purchase_date` ");
                                        values = string.Concat(values, "@purchaseDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@purchaseDate" + i, rewardStoreTransactionList[i].PurchaseDate);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].IsTrialPeriod))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `is_trial_period` ");
                                        values = string.Concat(values, ", @isTrialPeriod" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `is_trial_period` ");
                                        values = string.Concat(values, "@isTrialPeriod" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isTrialPeriod" + i, rewardStoreTransactionList[i].IsTrialPeriod);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardStoreTransactionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rewardStoreTransactionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rewardStoreTransactionList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(rewardStoreTransactionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardStoreTransactionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardStoreTransactionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardStoreTransactionModel>();
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

        public List<RewardStoreTransactionModel> Update(string connectionString, RewardStoreTransactionModel rewardStoreTransaction, RewardStoreTransactionModel rewardStoreTransactionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gr_reward_store_transactions SET ";
                if (!string.IsNullOrEmpty(rewardStoreTransaction.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", rewardStoreTransaction.Id);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", rewardStoreTransaction.UserId);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.TransactionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `transaction_id` = @transactionId");
                    else
                        sql = string.Concat(sql, " `transaction_id` = @transactionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@transactionId", rewardStoreTransaction.TransactionId);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.Code))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `code` = @code");
                    else
                        sql = string.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", rewardStoreTransaction.Code);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.ProductIdStore))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `product_id_store` = @productIdStore");
                    else
                        sql = string.Concat(sql, " `product_id_store` = @productIdStore");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@productIdStore", rewardStoreTransaction.ProductIdStore);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.RequestDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `request_date` = @requestDate");
                    else
                        sql = string.Concat(sql, " `request_date` = @requestDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@requestDate", rewardStoreTransaction.RequestDate);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.PurchaseDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `purchase_date` = @purchaseDate");
                    else
                        sql = string.Concat(sql, " `purchase_date` = @purchaseDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@purchaseDate", rewardStoreTransaction.PurchaseDate);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.IsTrialPeriod))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `is_trial_period` = @isTrialPeriod");
                    else
                        sql = string.Concat(sql, " `is_trial_period` = @isTrialPeriod");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@isTrialPeriod", rewardStoreTransaction.IsTrialPeriod);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", rewardStoreTransaction.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", rewardStoreTransaction.CreatedAt);
                }
                if (!string.IsNullOrEmpty(rewardStoreTransaction.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", rewardStoreTransaction.UpdatedAt);
                }
                if (rewardStoreTransactionWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", rewardStoreTransactionWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", rewardStoreTransactionWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.TransactionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `transaction_id` = @transactionIdWhere");
                        else
                            sql = string.Concat(sql, " `transaction_id` = @transactionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@transactionIdWhere", rewardStoreTransactionWhere.TransactionId);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = string.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", rewardStoreTransactionWhere.Code);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.ProductIdStore))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStoreWhere");
                        else
                            sql = string.Concat(sql, " `product_id_store` = @productIdStoreWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productIdStoreWhere", rewardStoreTransactionWhere.ProductIdStore);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.RequestDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `request_date` = @requestDateWhere");
                        else
                            sql = string.Concat(sql, " `request_date` = @requestDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@requestDateWhere", rewardStoreTransactionWhere.RequestDate);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.PurchaseDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `purchase_date` = @purchaseDateWhere");
                        else
                            sql = string.Concat(sql, " `purchase_date` = @purchaseDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@purchaseDateWhere", rewardStoreTransactionWhere.PurchaseDate);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.IsTrialPeriod))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_trial_period` = @isTrialPeriodWhere");
                        else
                            sql = string.Concat(sql, " `is_trial_period` = @isTrialPeriodWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isTrialPeriodWhere", rewardStoreTransactionWhere.IsTrialPeriod);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", rewardStoreTransactionWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", rewardStoreTransactionWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", rewardStoreTransactionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(rewardStoreTransaction);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RewardStoreTransactionModel>();
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

        public List<RewardStoreTransactionModel> Update(string connectionString, List<RewardStoreTransactionModel> rewardStoreTransactionList, List<RewardStoreTransactionModel> rewardStoreTransactionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardStoreTransactionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gr_reward_store_transactions SET ";
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, rewardStoreTransactionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, rewardStoreTransactionList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].TransactionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `transaction_id` = @transactionId" + i);
                                    else
                                        sql = string.Concat(sql, " `transaction_id` = @transactionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@transactionId" + i, rewardStoreTransactionList[i].TransactionId);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].Code))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = string.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, rewardStoreTransactionList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].ProductIdStore))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `product_id_store` = @productIdStore" + i);
                                    else
                                        sql = string.Concat(sql, " `product_id_store` = @productIdStore" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@productIdStore" + i, rewardStoreTransactionList[i].ProductIdStore);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].RequestDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `request_date` = @requestDate" + i);
                                    else
                                        sql = string.Concat(sql, " `request_date` = @requestDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@requestDate" + i, rewardStoreTransactionList[i].RequestDate);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].PurchaseDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `purchase_date` = @purchaseDate" + i);
                                    else
                                        sql = string.Concat(sql, " `purchase_date` = @purchaseDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@purchaseDate" + i, rewardStoreTransactionList[i].PurchaseDate);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].IsTrialPeriod))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `is_trial_period` = @isTrialPeriod" + i);
                                    else
                                        sql = string.Concat(sql, " `is_trial_period` = @isTrialPeriod" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@isTrialPeriod" + i, rewardStoreTransactionList[i].IsTrialPeriod);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardStoreTransactionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rewardStoreTransactionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(rewardStoreTransactionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rewardStoreTransactionList[i].UpdatedAt);
                                }
                                if (rewardStoreTransactionWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, rewardStoreTransactionWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, rewardStoreTransactionWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].TransactionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `transaction_id` = @transactionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `transaction_id` = @transactionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@transactionIdWhere" + i, rewardStoreTransactionWhereList[i].TransactionId);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, rewardStoreTransactionWhereList[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].ProductIdStore))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStoreWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id_store` = @productIdStoreWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productIdStoreWhere" + i, rewardStoreTransactionWhereList[i].ProductIdStore);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].RequestDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `request_date` = @requestDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `request_date` = @requestDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@requestDateWhere" + i, rewardStoreTransactionWhereList[i].RequestDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].PurchaseDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `purchase_date` = @purchaseDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `purchase_date` = @purchaseDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@purchaseDateWhere" + i, rewardStoreTransactionWhereList[i].PurchaseDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].IsTrialPeriod))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_trial_period` = @isTrialPeriodWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `is_trial_period` = @isTrialPeriodWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isTrialPeriodWhere" + i, rewardStoreTransactionWhereList[i].IsTrialPeriod);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, rewardStoreTransactionWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, rewardStoreTransactionWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransactionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, rewardStoreTransactionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rewardStoreTransactionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardStoreTransactionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardStoreTransactionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardStoreTransactionModel>();
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

        public List<RewardStoreTransactionModel> Delete(string connectionString, RewardStoreTransactionModel rewardStoreTransaction)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gr_reward_store_transactions  ";
                if (rewardStoreTransaction != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", rewardStoreTransaction.Id);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", rewardStoreTransaction.UserId);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.TransactionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `transaction_id` = @transactionId");
                        else
                            sql = string.Concat(sql, " `transaction_id` = @transactionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@transactionId", rewardStoreTransaction.TransactionId);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @code");
                        else
                            sql = string.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", rewardStoreTransaction.Code);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.ProductIdStore))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStore");
                        else
                            sql = string.Concat(sql, " `product_id_store` = @productIdStore");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@productIdStore", rewardStoreTransaction.ProductIdStore);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.RequestDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `request_date` = @requestDate");
                        else
                            sql = string.Concat(sql, " `request_date` = @requestDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@requestDate", rewardStoreTransaction.RequestDate);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.PurchaseDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `purchase_date` = @purchaseDate");
                        else
                            sql = string.Concat(sql, " `purchase_date` = @purchaseDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@purchaseDate", rewardStoreTransaction.PurchaseDate);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.IsTrialPeriod))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `is_trial_period` = @isTrialPeriod");
                        else
                            sql = string.Concat(sql, " `is_trial_period` = @isTrialPeriod");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@isTrialPeriod", rewardStoreTransaction.IsTrialPeriod);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", rewardStoreTransaction.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", rewardStoreTransaction.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(rewardStoreTransaction.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", rewardStoreTransaction.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(rewardStoreTransaction);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RewardStoreTransactionModel>();
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

        public List<RewardStoreTransactionModel> Delete(string connectionString, List<RewardStoreTransactionModel> rewardStoreTransaction)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardStoreTransactionModel> response = new List<RewardStoreTransactionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardStoreTransaction.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gr_reward_store_transactions  ";
                                if (rewardStoreTransaction[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, rewardStoreTransaction[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, rewardStoreTransaction[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].TransactionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `transaction_id` = @transactionId" + i);
                                        else
                                            sql = string.Concat(sql, " `transaction_id` = @transactionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@transactionId" + i, rewardStoreTransaction[i].TransactionId);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, rewardStoreTransaction[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].ProductIdStore))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `product_id_store` = @productIdStore" + i);
                                        else
                                            sql = string.Concat(sql, " `product_id_store` = @productIdStore" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@productIdStore" + i, rewardStoreTransaction[i].ProductIdStore);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].RequestDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `request_date` = @requestDate" + i);
                                        else
                                            sql = string.Concat(sql, " `request_date` = @requestDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@requestDate" + i, rewardStoreTransaction[i].RequestDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].PurchaseDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `purchase_date` = @purchaseDate" + i);
                                        else
                                            sql = string.Concat(sql, " `purchase_date` = @purchaseDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@purchaseDate" + i, rewardStoreTransaction[i].PurchaseDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].IsTrialPeriod))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `is_trial_period` = @isTrialPeriod" + i);
                                        else
                                            sql = string.Concat(sql, " `is_trial_period` = @isTrialPeriod" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@isTrialPeriod" + i, rewardStoreTransaction[i].IsTrialPeriod);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardStoreTransaction[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, rewardStoreTransaction[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(rewardStoreTransaction[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, rewardStoreTransaction[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rewardStoreTransaction[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardStoreTransactionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardStoreTransactionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardStoreTransactionModel>();
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
