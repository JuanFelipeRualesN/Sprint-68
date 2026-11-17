using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameUserRewards
{
    class RewardCodeDAO
    {
        private MySqlConnection _connection;
        public List<RewardCodeModel> Read(string connectionString, RewardCodeModel rewardCode, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `reward_code`, `reward_type`, `reward_quantity`, `multiple_use`, `start_date`, `end_date`, `coupon_quantity`, `one_use_per_customer`, `modified_by`, `created_at`, `updated_at`  FROM gr_reward_codes ";
                {
                    if (rewardCode != null)
                    {
                        if (!string.IsNullOrEmpty(rewardCode.Id))
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
                            cmd.Parameters.AddWithValue("@id", rewardCode.Id);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.RewardCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `reward_code` = @rewardCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `reward_code` = @rewardCode ");
                            }
                            cmd.Parameters.AddWithValue("@rewardCode", rewardCode.RewardCode);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.RewardType))
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
                            cmd.Parameters.AddWithValue("@rewardType", rewardCode.RewardType);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.RewardQuantity))
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
                            cmd.Parameters.AddWithValue("@rewardQuantity", rewardCode.RewardQuantity);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.MultipleUse))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `multiple_use` = @multipleUse ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `multiple_use` = @multipleUse ");
                            }
                            cmd.Parameters.AddWithValue("@multipleUse", rewardCode.MultipleUse);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.StartDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `start_date` = @startDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `start_date` = @startDate ");
                            }
                            cmd.Parameters.AddWithValue("@startDate", rewardCode.StartDate);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.EndDate))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `end_date` = @endDate ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `end_date` = @endDate ");
                            }
                            cmd.Parameters.AddWithValue("@endDate", rewardCode.EndDate);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.CouponQuantity))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `coupon_quantity` = @couponQuantity ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `coupon_quantity` = @couponQuantity ");
                            }
                            cmd.Parameters.AddWithValue("@couponQuantity", rewardCode.CouponQuantity);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.OneUsePerCustomer))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `one_use_per_customer` = @oneUsePerCustomer ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `one_use_per_customer` = @oneUsePerCustomer ");
                            }
                            cmd.Parameters.AddWithValue("@oneUsePerCustomer", rewardCode.OneUsePerCustomer);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", rewardCode.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", rewardCode.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(rewardCode.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", rewardCode.UpdatedAt);
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
                                    rewardCode.Id = Reader.GetValue(0).ToString();
                                else
                                    rewardCode.Id = null;
                                if (!Reader.IsDBNull(1))
                                    rewardCode.RewardCode = Reader.GetValue(1).ToString();
                                else
                                    rewardCode.RewardCode = null;
                                if (!Reader.IsDBNull(2))
                                    rewardCode.RewardType = Reader.GetValue(2).ToString();
                                else
                                    rewardCode.RewardType = null;
                                if (!Reader.IsDBNull(3))
                                    rewardCode.RewardQuantity = Reader.GetValue(3).ToString();
                                else
                                    rewardCode.RewardQuantity = null;
                                if (!Reader.IsDBNull(4))
                                    rewardCode.MultipleUse = Reader.GetValue(4).ToString();
                                else
                                    rewardCode.MultipleUse = null;
                                if (!Reader.IsDBNull(5))
                                    rewardCode.StartDate = Reader.GetValue(5).ToString();
                                else
                                    rewardCode.StartDate = null;
                                if (!Reader.IsDBNull(6))
                                    rewardCode.EndDate = Reader.GetValue(6).ToString();
                                else
                                    rewardCode.EndDate = null;
                                if (!Reader.IsDBNull(7))
                                    rewardCode.CouponQuantity = Reader.GetValue(7).ToString();
                                else
                                    rewardCode.CouponQuantity = null;
                                if (!Reader.IsDBNull(8))
                                    rewardCode.OneUsePerCustomer = Reader.GetValue(8).ToString();
                                else
                                    rewardCode.OneUsePerCustomer = null;
                                if (!Reader.IsDBNull(9))
                                    rewardCode.ModifiedBy = Reader.GetValue(9).ToString();
                                else
                                    rewardCode.ModifiedBy = null;
                                if (!Reader.IsDBNull(10))
                                    rewardCode.CreatedAt = Reader.GetValue(10).ToString();
                                else
                                    rewardCode.CreatedAt = null;
                                if (!Reader.IsDBNull(11))
                                    rewardCode.UpdatedAt = Reader.GetValue(11).ToString();
                                else
                                    rewardCode.UpdatedAt = null;
                                response.Add(rewardCode);
                                rewardCode = new RewardCodeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RewardCodeModel>();
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
                response = new List<RewardCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RewardCodeModel> Read(string connectionString, List<RewardCodeModel> rewardCodeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `reward_code`, `reward_type`, `reward_quantity`, `multiple_use`, `start_date`, `end_date`, `coupon_quantity`, `one_use_per_customer`, `modified_by`, `created_at`, `updated_at`  FROM gr_reward_codes ";
                if (rewardCodeWhere != null && rewardCodeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < rewardCodeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, rewardCodeWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].RewardCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reward_code` = @rewardCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rewardCode" + i, rewardCodeWhere[i].RewardCode);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].RewardType))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reward_type` = @rewardType" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rewardType" + i, rewardCodeWhere[i].RewardType);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].RewardQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `reward_quantity` = @rewardQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardCodeWhere[i].RewardQuantity);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].MultipleUse))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `multiple_use` = @multipleUse" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@multipleUse" + i, rewardCodeWhere[i].MultipleUse);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].StartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `start_date` = @startDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@startDate" + i, rewardCodeWhere[i].StartDate);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, rewardCodeWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].CouponQuantity))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `coupon_quantity` = @couponQuantity" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@couponQuantity" + i, rewardCodeWhere[i].CouponQuantity);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].OneUsePerCustomer))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `one_use_per_customer` = @oneUsePerCustomer" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@oneUsePerCustomer" + i, rewardCodeWhere[i].OneUsePerCustomer);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardCodeWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, rewardCodeWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(rewardCodeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, rewardCodeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != rewardCodeWhere.Count - 1)
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
                            RewardCodeModel rewardCode = new RewardCodeModel(); if (!Reader.IsDBNull(0))
                                rewardCode.Id = Reader.GetValue(0).ToString();
                            else
                                rewardCode.Id = null;
                            if (!Reader.IsDBNull(1))
                                rewardCode.RewardCode = Reader.GetValue(1).ToString();
                            else
                                rewardCode.RewardCode = null;
                            if (!Reader.IsDBNull(2))
                                rewardCode.RewardType = Reader.GetValue(2).ToString();
                            else
                                rewardCode.RewardType = null;
                            if (!Reader.IsDBNull(3))
                                rewardCode.RewardQuantity = Reader.GetValue(3).ToString();
                            else
                                rewardCode.RewardQuantity = null;
                            if (!Reader.IsDBNull(4))
                                rewardCode.MultipleUse = Reader.GetValue(4).ToString();
                            else
                                rewardCode.MultipleUse = null;
                            if (!Reader.IsDBNull(5))
                                rewardCode.StartDate = Reader.GetValue(5).ToString();
                            else
                                rewardCode.StartDate = null;
                            if (!Reader.IsDBNull(6))
                                rewardCode.EndDate = Reader.GetValue(6).ToString();
                            else
                                rewardCode.EndDate = null;
                            if (!Reader.IsDBNull(7))
                                rewardCode.CouponQuantity = Reader.GetValue(7).ToString();
                            else
                                rewardCode.CouponQuantity = null;
                            if (!Reader.IsDBNull(8))
                                rewardCode.OneUsePerCustomer = Reader.GetValue(8).ToString();
                            else
                                rewardCode.OneUsePerCustomer = null;
                            if (!Reader.IsDBNull(9))
                                rewardCode.ModifiedBy = Reader.GetValue(9).ToString();
                            else
                                rewardCode.ModifiedBy = null;
                            if (!Reader.IsDBNull(10))
                                rewardCode.CreatedAt = Reader.GetValue(10).ToString();
                            else
                                rewardCode.CreatedAt = null;
                            if (!Reader.IsDBNull(11))
                                rewardCode.UpdatedAt = Reader.GetValue(11).ToString();
                            else
                                rewardCode.UpdatedAt = null;
                            response.Add(rewardCode);
                            rewardCode = new RewardCodeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RewardCodeModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardCodeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RewardCodeModel> Create(string connectionString, RewardCodeModel rewardCode)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gr_reward_codes (";
                string values = "(";
                if (!string.IsNullOrEmpty(rewardCode.Id))
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
                    cmd.Parameters.AddWithValue("@id", rewardCode.Id);
                }
                if (!string.IsNullOrEmpty(rewardCode.RewardCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `reward_code` ");
                        values = string.Concat(values, ", @rewardCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `reward_code` ");
                        values = string.Concat(values, " @rewardCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardCode", rewardCode.RewardCode);
                }
                if (!string.IsNullOrEmpty(rewardCode.RewardType))
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
                    cmd.Parameters.AddWithValue("@rewardType", rewardCode.RewardType);
                }
                if (!string.IsNullOrEmpty(rewardCode.RewardQuantity))
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
                    cmd.Parameters.AddWithValue("@rewardQuantity", rewardCode.RewardQuantity);
                }
                if (!string.IsNullOrEmpty(rewardCode.MultipleUse))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `multiple_use` ");
                        values = string.Concat(values, ", @multipleUse");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `multiple_use` ");
                        values = string.Concat(values, " @multipleUse");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@multipleUse", rewardCode.MultipleUse);
                }
                if (!string.IsNullOrEmpty(rewardCode.StartDate))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `start_date` ");
                        values = string.Concat(values, ", @startDate");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `start_date` ");
                        values = string.Concat(values, " @startDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@startDate", rewardCode.StartDate);
                }
                if (!string.IsNullOrEmpty(rewardCode.EndDate))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `end_date` ");
                        values = string.Concat(values, ", @endDate");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `end_date` ");
                        values = string.Concat(values, " @endDate");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@endDate", rewardCode.EndDate);
                }
                if (!string.IsNullOrEmpty(rewardCode.CouponQuantity))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `coupon_quantity` ");
                        values = string.Concat(values, ", @couponQuantity");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `coupon_quantity` ");
                        values = string.Concat(values, " @couponQuantity");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@couponQuantity", rewardCode.CouponQuantity);
                }
                if (!string.IsNullOrEmpty(rewardCode.OneUsePerCustomer))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `one_use_per_customer` ");
                        values = string.Concat(values, ", @oneUsePerCustomer");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `one_use_per_customer` ");
                        values = string.Concat(values, " @oneUsePerCustomer");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@oneUsePerCustomer", rewardCode.OneUsePerCustomer);
                }
                if (!string.IsNullOrEmpty(rewardCode.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", rewardCode.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(rewardCode.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", rewardCode.CreatedAt);
                }
                if (!string.IsNullOrEmpty(rewardCode.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", rewardCode.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                rewardCode.Id = cmd.LastInsertedId.ToString();
                response.Add(rewardCode);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardCodeModel>();
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
        public List<RewardCodeModel> Create(string connectionString, List<RewardCodeModel> rewardCodeList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardCodeList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gr_reward_codes (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(rewardCodeList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, rewardCodeList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].RewardCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `reward_code` ");
                                        values = string.Concat(values, ", @rewardCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `reward_code` ");
                                        values = string.Concat(values, "@rewardCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardCode" + i, rewardCodeList[i].RewardCode);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].RewardType))
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
                                    cmd.Parameters.AddWithValue("@rewardType" + i, rewardCodeList[i].RewardType);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].RewardQuantity))
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
                                    cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardCodeList[i].RewardQuantity);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].MultipleUse))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `multiple_use` ");
                                        values = string.Concat(values, ", @multipleUse" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `multiple_use` ");
                                        values = string.Concat(values, "@multipleUse" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@multipleUse" + i, rewardCodeList[i].MultipleUse);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].StartDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `start_date` ");
                                        values = string.Concat(values, ", @startDate" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `start_date` ");
                                        values = string.Concat(values, "@startDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@startDate" + i, rewardCodeList[i].StartDate);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].EndDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `end_date` ");
                                        values = string.Concat(values, ", @endDate" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `end_date` ");
                                        values = string.Concat(values, "@endDate" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@endDate" + i, rewardCodeList[i].EndDate);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].CouponQuantity))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `coupon_quantity` ");
                                        values = string.Concat(values, ", @couponQuantity" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `coupon_quantity` ");
                                        values = string.Concat(values, "@couponQuantity" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@couponQuantity" + i, rewardCodeList[i].CouponQuantity);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].OneUsePerCustomer))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `one_use_per_customer` ");
                                        values = string.Concat(values, ", @oneUsePerCustomer" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `one_use_per_customer` ");
                                        values = string.Concat(values, "@oneUsePerCustomer" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@oneUsePerCustomer" + i, rewardCodeList[i].OneUsePerCustomer);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardCodeList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rewardCodeList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rewardCodeList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(rewardCodeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardCodeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardCodeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardCodeModel>();
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

        public List<RewardCodeModel> Update(string connectionString, RewardCodeModel rewardCode, RewardCodeModel rewardCodeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gr_reward_codes SET ";
                if (!string.IsNullOrEmpty(rewardCode.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", rewardCode.Id);
                }
                if (!string.IsNullOrEmpty(rewardCode.RewardCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reward_code` = @rewardCode");
                    else
                        sql = string.Concat(sql, " `reward_code` = @rewardCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardCode", rewardCode.RewardCode);
                }
                if (!string.IsNullOrEmpty(rewardCode.RewardType))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reward_type` = @rewardType");
                    else
                        sql = string.Concat(sql, " `reward_type` = @rewardType");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardType", rewardCode.RewardType);
                }
                if (!string.IsNullOrEmpty(rewardCode.RewardQuantity))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `reward_quantity` = @rewardQuantity");
                    else
                        sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rewardQuantity", rewardCode.RewardQuantity);
                }
                if (!string.IsNullOrEmpty(rewardCode.MultipleUse))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `multiple_use` = @multipleUse");
                    else
                        sql = string.Concat(sql, " `multiple_use` = @multipleUse");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@multipleUse", rewardCode.MultipleUse);
                }
                if (!string.IsNullOrEmpty(rewardCode.StartDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `start_date` = @startDate");
                    else
                        sql = string.Concat(sql, " `start_date` = @startDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@startDate", rewardCode.StartDate);
                }
                if (!string.IsNullOrEmpty(rewardCode.EndDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `end_date` = @endDate");
                    else
                        sql = string.Concat(sql, " `end_date` = @endDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@endDate", rewardCode.EndDate);
                }
                if (!string.IsNullOrEmpty(rewardCode.CouponQuantity))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `coupon_quantity` = @couponQuantity");
                    else
                        sql = string.Concat(sql, " `coupon_quantity` = @couponQuantity");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@couponQuantity", rewardCode.CouponQuantity);
                }
                if (!string.IsNullOrEmpty(rewardCode.OneUsePerCustomer))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `one_use_per_customer` = @oneUsePerCustomer");
                    else
                        sql = string.Concat(sql, " `one_use_per_customer` = @oneUsePerCustomer");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@oneUsePerCustomer", rewardCode.OneUsePerCustomer);
                }
                if (!string.IsNullOrEmpty(rewardCode.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", rewardCode.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(rewardCode.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", rewardCode.CreatedAt);
                }
                if (!string.IsNullOrEmpty(rewardCode.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", rewardCode.UpdatedAt);
                }
                if (rewardCodeWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(rewardCodeWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", rewardCodeWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.RewardCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_code` = @rewardCodeWhere");
                        else
                            sql = string.Concat(sql, " `reward_code` = @rewardCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardCodeWhere", rewardCodeWhere.RewardCode);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.RewardType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_type` = @rewardTypeWhere");
                        else
                            sql = string.Concat(sql, " `reward_type` = @rewardTypeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardTypeWhere", rewardCodeWhere.RewardType);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.RewardQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantityWhere");
                        else
                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardQuantityWhere", rewardCodeWhere.RewardQuantity);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.MultipleUse))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `multiple_use` = @multipleUseWhere");
                        else
                            sql = string.Concat(sql, " `multiple_use` = @multipleUseWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@multipleUseWhere", rewardCodeWhere.MultipleUse);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.StartDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `start_date` = @startDateWhere");
                        else
                            sql = string.Concat(sql, " `start_date` = @startDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@startDateWhere", rewardCodeWhere.StartDate);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.EndDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `end_date` = @endDateWhere");
                        else
                            sql = string.Concat(sql, " `end_date` = @endDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@endDateWhere", rewardCodeWhere.EndDate);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.CouponQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `coupon_quantity` = @couponQuantityWhere");
                        else
                            sql = string.Concat(sql, " `coupon_quantity` = @couponQuantityWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@couponQuantityWhere", rewardCodeWhere.CouponQuantity);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.OneUsePerCustomer))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `one_use_per_customer` = @oneUsePerCustomerWhere");
                        else
                            sql = string.Concat(sql, " `one_use_per_customer` = @oneUsePerCustomerWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@oneUsePerCustomerWhere", rewardCodeWhere.OneUsePerCustomer);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", rewardCodeWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", rewardCodeWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(rewardCodeWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", rewardCodeWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(rewardCode);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RewardCodeModel>();
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

        public List<RewardCodeModel> Update(string connectionString, List<RewardCodeModel> rewardCodeList, List<RewardCodeModel> rewardCodeWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardCodeList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gr_reward_codes SET ";
                                if (!string.IsNullOrEmpty(rewardCodeList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, rewardCodeList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].RewardCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reward_code` = @rewardCode" + i);
                                    else
                                        sql = string.Concat(sql, " `reward_code` = @rewardCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardCode" + i, rewardCodeList[i].RewardCode);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].RewardType))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reward_type` = @rewardType" + i);
                                    else
                                        sql = string.Concat(sql, " `reward_type` = @rewardType" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardType" + i, rewardCodeList[i].RewardType);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].RewardQuantity))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `reward_quantity` = @rewardQuantity" + i);
                                    else
                                        sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardCodeList[i].RewardQuantity);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].MultipleUse))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `multiple_use` = @multipleUse" + i);
                                    else
                                        sql = string.Concat(sql, " `multiple_use` = @multipleUse" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@multipleUse" + i, rewardCodeList[i].MultipleUse);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].StartDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `start_date` = @startDate" + i);
                                    else
                                        sql = string.Concat(sql, " `start_date` = @startDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@startDate" + i, rewardCodeList[i].StartDate);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].EndDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `end_date` = @endDate" + i);
                                    else
                                        sql = string.Concat(sql, " `end_date` = @endDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@endDate" + i, rewardCodeList[i].EndDate);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].CouponQuantity))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `coupon_quantity` = @couponQuantity" + i);
                                    else
                                        sql = string.Concat(sql, " `coupon_quantity` = @couponQuantity" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@couponQuantity" + i, rewardCodeList[i].CouponQuantity);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].OneUsePerCustomer))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `one_use_per_customer` = @oneUsePerCustomer" + i);
                                    else
                                        sql = string.Concat(sql, " `one_use_per_customer` = @oneUsePerCustomer" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@oneUsePerCustomer" + i, rewardCodeList[i].OneUsePerCustomer);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardCodeList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, rewardCodeList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(rewardCodeList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, rewardCodeList[i].UpdatedAt);
                                }
                                if (rewardCodeWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, rewardCodeWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].RewardCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_code` = @rewardCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_code` = @rewardCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardCodeWhere" + i, rewardCodeWhereList[i].RewardCode);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].RewardType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_type` = @rewardTypeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_type` = @rewardTypeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardTypeWhere" + i, rewardCodeWhereList[i].RewardType);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].RewardQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantityWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardQuantityWhere" + i, rewardCodeWhereList[i].RewardQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].MultipleUse))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `multiple_use` = @multipleUseWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `multiple_use` = @multipleUseWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@multipleUseWhere" + i, rewardCodeWhereList[i].MultipleUse);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].StartDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `start_date` = @startDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `start_date` = @startDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@startDateWhere" + i, rewardCodeWhereList[i].StartDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].EndDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `end_date` = @endDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `end_date` = @endDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@endDateWhere" + i, rewardCodeWhereList[i].EndDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].CouponQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `coupon_quantity` = @couponQuantityWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `coupon_quantity` = @couponQuantityWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@couponQuantityWhere" + i, rewardCodeWhereList[i].CouponQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].OneUsePerCustomer))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `one_use_per_customer` = @oneUsePerCustomerWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `one_use_per_customer` = @oneUsePerCustomerWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@oneUsePerCustomerWhere" + i, rewardCodeWhereList[i].OneUsePerCustomer);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, rewardCodeWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, rewardCodeWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCodeWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, rewardCodeWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rewardCodeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardCodeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardCodeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardCodeModel>();
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

        public List<RewardCodeModel> Delete(string connectionString, RewardCodeModel rewardCode)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gr_reward_codes  ";
                if (rewardCode != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(rewardCode.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", rewardCode.Id);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.RewardCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_code` = @rewardCode");
                        else
                            sql = string.Concat(sql, " `reward_code` = @rewardCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardCode", rewardCode.RewardCode);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.RewardType))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_type` = @rewardType");
                        else
                            sql = string.Concat(sql, " `reward_type` = @rewardType");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardType", rewardCode.RewardType);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.RewardQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantity");
                        else
                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rewardQuantity", rewardCode.RewardQuantity);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.MultipleUse))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `multiple_use` = @multipleUse");
                        else
                            sql = string.Concat(sql, " `multiple_use` = @multipleUse");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@multipleUse", rewardCode.MultipleUse);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.StartDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `start_date` = @startDate");
                        else
                            sql = string.Concat(sql, " `start_date` = @startDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@startDate", rewardCode.StartDate);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.EndDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `end_date` = @endDate");
                        else
                            sql = string.Concat(sql, " `end_date` = @endDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@endDate", rewardCode.EndDate);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.CouponQuantity))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `coupon_quantity` = @couponQuantity");
                        else
                            sql = string.Concat(sql, " `coupon_quantity` = @couponQuantity");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@couponQuantity", rewardCode.CouponQuantity);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.OneUsePerCustomer))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `one_use_per_customer` = @oneUsePerCustomer");
                        else
                            sql = string.Concat(sql, " `one_use_per_customer` = @oneUsePerCustomer");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@oneUsePerCustomer", rewardCode.OneUsePerCustomer);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", rewardCode.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", rewardCode.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(rewardCode.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", rewardCode.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(rewardCode);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RewardCodeModel>();
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

        public List<RewardCodeModel> Delete(string connectionString, List<RewardCodeModel> rewardCode)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RewardCodeModel> response = new List<RewardCodeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < rewardCode.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gr_reward_codes  ";
                                if (rewardCode[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(rewardCode[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, rewardCode[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].RewardCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_code` = @rewardCode" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_code` = @rewardCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardCode" + i, rewardCode[i].RewardCode);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].RewardType))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_type` = @rewardType" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_type` = @rewardType" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardType" + i, rewardCode[i].RewardType);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].RewardQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `reward_quantity` = @rewardQuantity" + i);
                                        else
                                            sql = string.Concat(sql, " `reward_quantity` = @rewardQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rewardQuantity" + i, rewardCode[i].RewardQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].MultipleUse))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `multiple_use` = @multipleUse" + i);
                                        else
                                            sql = string.Concat(sql, " `multiple_use` = @multipleUse" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@multipleUse" + i, rewardCode[i].MultipleUse);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].StartDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `start_date` = @startDate" + i);
                                        else
                                            sql = string.Concat(sql, " `start_date` = @startDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@startDate" + i, rewardCode[i].StartDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].EndDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `end_date` = @endDate" + i);
                                        else
                                            sql = string.Concat(sql, " `end_date` = @endDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@endDate" + i, rewardCode[i].EndDate);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].CouponQuantity))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `coupon_quantity` = @couponQuantity" + i);
                                        else
                                            sql = string.Concat(sql, " `coupon_quantity` = @couponQuantity" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@couponQuantity" + i, rewardCode[i].CouponQuantity);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].OneUsePerCustomer))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `one_use_per_customer` = @oneUsePerCustomer" + i);
                                        else
                                            sql = string.Concat(sql, " `one_use_per_customer` = @oneUsePerCustomer" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@oneUsePerCustomer" + i, rewardCode[i].OneUsePerCustomer);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, rewardCode[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, rewardCode[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(rewardCode[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, rewardCode[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(rewardCode[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RewardCodeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RewardCodeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RewardCodeModel>();
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
