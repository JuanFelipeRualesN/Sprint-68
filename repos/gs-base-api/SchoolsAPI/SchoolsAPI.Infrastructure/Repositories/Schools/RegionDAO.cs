using System;
using System.Collections.Generic;
using MySqlConnector;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools
{
    class RegionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, RegionModel> ReadD(string connectionString, RegionModel region, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RegionModel> response = new Dictionary<string, RegionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                {
                    if (region != null)
                    {
                        if (!string.IsNullOrEmpty(region.Id))
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
                            cmd.Parameters.AddWithValue("@id", region.Id);
                        }
                        if (!string.IsNullOrEmpty(region.CountryIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso` = @countryIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso` = @countryIso ");
                            }
                            cmd.Parameters.AddWithValue("@countryIso", region.CountryIso);
                        }
                        if (!string.IsNullOrEmpty(region.Code))
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
                            cmd.Parameters.AddWithValue("@code", region.Code);
                        }
                        if (!string.IsNullOrEmpty(region.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", region.Name);
                        }
                        if (!string.IsNullOrEmpty(region.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", region.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(region.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", region.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(region.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", region.UpdatedAt);
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
                                    region.Id = reader.GetValue(0).ToString();
                                else
                                    region.Id = null;
                                if (!reader.IsDBNull(1))
                                    region.CountryIso = reader.GetValue(1).ToString();
                                else
                                    region.CountryIso = null;
                                if (!reader.IsDBNull(2))
                                    region.Code = reader.GetValue(2).ToString();
                                else
                                    region.Code = null;
                                if (!reader.IsDBNull(3))
                                    region.Name = reader.GetValue(3).ToString();
                                else
                                    region.Name = null;
                                if (!reader.IsDBNull(4))
                                    region.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    region.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    region.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    region.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    region.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    region.UpdatedAt = null;
                                response.Add(region.Id, region);
                                region = new RegionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, RegionModel>();
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
                response = new Dictionary<string, RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RegionModel> ReadD(string connectionString, RegionModel region, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RegionModel> response = new Dictionary<string, RegionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                {
                    if (region != null)
                    {
                        if (!string.IsNullOrEmpty(region.Id))
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
                            cmd.Parameters.AddWithValue("@id", region.Id);
                        }
                        if (!string.IsNullOrEmpty(region.CountryIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso` = @countryIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso` = @countryIso ");
                            }
                            cmd.Parameters.AddWithValue("@countryIso", region.CountryIso);
                        }
                        if (!string.IsNullOrEmpty(region.Code))
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
                            cmd.Parameters.AddWithValue("@code", region.Code);
                        }
                        if (!string.IsNullOrEmpty(region.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", region.Name);
                        }
                        if (!string.IsNullOrEmpty(region.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", region.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(region.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", region.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(region.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", region.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    region.Id = reader.GetValue(0).ToString();
                                else
                                    region.Id = null;
                                if (!reader.IsDBNull(1))
                                    region.CountryIso = reader.GetValue(1).ToString();
                                else
                                    region.CountryIso = null;
                                if (!reader.IsDBNull(2))
                                    region.Code = reader.GetValue(2).ToString();
                                else
                                    region.Code = null;
                                if (!reader.IsDBNull(3))
                                    region.Name = reader.GetValue(3).ToString();
                                else
                                    region.Name = null;
                                if (!reader.IsDBNull(4))
                                    region.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    region.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    region.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    region.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    region.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    region.UpdatedAt = null;
                                response.Add(region.Id, region);
                                region = new RegionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, RegionModel>();
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
                response = new Dictionary<string, RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RegionModel> ReadD(string connectionString, List<RegionModel> regionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RegionModel> response = new Dictionary<string, RegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                if (regionWhere != null && regionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < regionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(regionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, regionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CountryIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso` = @countryIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIso" + i, regionWhere[i].CountryIso);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, regionWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, regionWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, regionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, regionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, regionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != regionWhere.Count - 1)
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
                            RegionModel region = new RegionModel(); if (!reader.IsDBNull(0))
                                region.Id = reader.GetValue(0).ToString();
                            else
                                region.Id = null;
                            if (!reader.IsDBNull(1))
                                region.CountryIso = reader.GetValue(1).ToString();
                            else
                                region.CountryIso = null;
                            if (!reader.IsDBNull(2))
                                region.Code = reader.GetValue(2).ToString();
                            else
                                region.Code = null;
                            if (!reader.IsDBNull(3))
                                region.Name = reader.GetValue(3).ToString();
                            else
                                region.Name = null;
                            if (!reader.IsDBNull(4))
                                region.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                region.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                region.CreatedAt = reader.GetValue(5).ToString();
                            else
                                region.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                region.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                region.UpdatedAt = null;
                            response.Add(region.Id, region);
                            region = new RegionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, RegionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, RegionModel> ReadD(string connectionString, List<RegionModel> regionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, RegionModel> response = new Dictionary<string, RegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                if (regionWhere != null && regionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < regionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(regionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, regionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CountryIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso` = @countryIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIso" + i, regionWhere[i].CountryIso);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, regionWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, regionWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, regionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, regionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, regionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != regionWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn + " LIMIT @fromRow, @limit ";
                cmd.Parameters.AddWithValue("@fromRow", fromRow);
                cmd.Parameters.AddWithValue("@limit", limit);
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            RegionModel region = new RegionModel(); if (!reader.IsDBNull(0))
                                region.Id = reader.GetValue(0).ToString();
                            else
                                region.Id = null;
                            if (!reader.IsDBNull(1))
                                region.CountryIso = reader.GetValue(1).ToString();
                            else
                                region.CountryIso = null;
                            if (!reader.IsDBNull(2))
                                region.Code = reader.GetValue(2).ToString();
                            else
                                region.Code = null;
                            if (!reader.IsDBNull(3))
                                region.Name = reader.GetValue(3).ToString();
                            else
                                region.Name = null;
                            if (!reader.IsDBNull(4))
                                region.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                region.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                region.CreatedAt = reader.GetValue(5).ToString();
                            else
                                region.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                region.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                region.UpdatedAt = null;
                            response.Add(region.Id, region);
                            region = new RegionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, RegionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RegionModel> Read(string connectionString, RegionModel region, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                {
                    if (region != null)
                    {
                        if (!string.IsNullOrEmpty(region.Id))
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
                            cmd.Parameters.AddWithValue("@id", region.Id);
                        }
                        if (!string.IsNullOrEmpty(region.CountryIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso` = @countryIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso` = @countryIso ");
                            }
                            cmd.Parameters.AddWithValue("@countryIso", region.CountryIso);
                        }
                        if (!string.IsNullOrEmpty(region.Code))
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
                            cmd.Parameters.AddWithValue("@code", region.Code);
                        }
                        if (!string.IsNullOrEmpty(region.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", region.Name);
                        }
                        if (!string.IsNullOrEmpty(region.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", region.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(region.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", region.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(region.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", region.UpdatedAt);
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
                                    region.Id = reader.GetValue(0).ToString();
                                else
                                    region.Id = null;
                                if (!reader.IsDBNull(1))
                                    region.CountryIso = reader.GetValue(1).ToString();
                                else
                                    region.CountryIso = null;
                                if (!reader.IsDBNull(2))
                                    region.Code = reader.GetValue(2).ToString();
                                else
                                    region.Code = null;
                                if (!reader.IsDBNull(3))
                                    region.Name = reader.GetValue(3).ToString();
                                else
                                    region.Name = null;
                                if (!reader.IsDBNull(4))
                                    region.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    region.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    region.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    region.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    region.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    region.UpdatedAt = null;
                                response.Add(region);
                                region = new RegionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RegionModel>();
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
                response = new List<RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RegionModel> Read(string connectionString, RegionModel region, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                {
                    if (region != null)
                    {
                        if (!string.IsNullOrEmpty(region.Id))
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
                            cmd.Parameters.AddWithValue("@id", region.Id);
                        }
                        if (!string.IsNullOrEmpty(region.CountryIso))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `country_iso` = @countryIso ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `country_iso` = @countryIso ");
                            }
                            cmd.Parameters.AddWithValue("@countryIso", region.CountryIso);
                        }
                        if (!string.IsNullOrEmpty(region.Code))
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
                            cmd.Parameters.AddWithValue("@code", region.Code);
                        }
                        if (!string.IsNullOrEmpty(region.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", region.Name);
                        }
                        if (!string.IsNullOrEmpty(region.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", region.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(region.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", region.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(region.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", region.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    region.Id = reader.GetValue(0).ToString();
                                else
                                    region.Id = null;
                                if (!reader.IsDBNull(1))
                                    region.CountryIso = reader.GetValue(1).ToString();
                                else
                                    region.CountryIso = null;
                                if (!reader.IsDBNull(2))
                                    region.Code = reader.GetValue(2).ToString();
                                else
                                    region.Code = null;
                                if (!reader.IsDBNull(3))
                                    region.Name = reader.GetValue(3).ToString();
                                else
                                    region.Name = null;
                                if (!reader.IsDBNull(4))
                                    region.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    region.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    region.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    region.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    region.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    region.UpdatedAt = null;
                                response.Add(region);
                                region = new RegionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<RegionModel>();
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
                response = new List<RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RegionModel> Read(string connectionString, List<RegionModel> regionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                if (regionWhere != null && regionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < regionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(regionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, regionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CountryIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso` = @countryIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIso" + i, regionWhere[i].CountryIso);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, regionWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, regionWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, regionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, regionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, regionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != regionWhere.Count - 1)
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
                            RegionModel region = new RegionModel(); if (!reader.IsDBNull(0))
                                region.Id = reader.GetValue(0).ToString();
                            else
                                region.Id = null;
                            if (!reader.IsDBNull(1))
                                region.CountryIso = reader.GetValue(1).ToString();
                            else
                                region.CountryIso = null;
                            if (!reader.IsDBNull(2))
                                region.Code = reader.GetValue(2).ToString();
                            else
                                region.Code = null;
                            if (!reader.IsDBNull(3))
                                region.Name = reader.GetValue(3).ToString();
                            else
                                region.Name = null;
                            if (!reader.IsDBNull(4))
                                region.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                region.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                region.CreatedAt = reader.GetValue(5).ToString();
                            else
                                region.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                region.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                region.UpdatedAt = null;
                            response.Add(region);
                            region = new RegionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RegionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RegionModel> Read(string connectionString, List<RegionModel> regionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `country_iso`, `code`, `name`, `modified_by`, `created_at`, `updated_at`  FROM sch_regions ";
                if (regionWhere != null && regionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < regionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(regionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, regionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CountryIso))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `country_iso` = @countryIso" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@countryIso" + i, regionWhere[i].CountryIso);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Code))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `code` = @code" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@code" + i, regionWhere[i].Code);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, regionWhere[i].Name);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, regionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, regionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(regionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, regionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != regionWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn + " LIMIT @fromRow, @limit ";
                cmd.Parameters.AddWithValue("@fromRow", fromRow);
                cmd.Parameters.AddWithValue("@limit", limit);
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            RegionModel region = new RegionModel(); if (!reader.IsDBNull(0))
                                region.Id = reader.GetValue(0).ToString();
                            else
                                region.Id = null;
                            if (!reader.IsDBNull(1))
                                region.CountryIso = reader.GetValue(1).ToString();
                            else
                                region.CountryIso = null;
                            if (!reader.IsDBNull(2))
                                region.Code = reader.GetValue(2).ToString();
                            else
                                region.Code = null;
                            if (!reader.IsDBNull(3))
                                region.Name = reader.GetValue(3).ToString();
                            else
                                region.Name = null;
                            if (!reader.IsDBNull(4))
                                region.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                region.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                region.CreatedAt = reader.GetValue(5).ToString();
                            else
                                region.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                region.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                region.UpdatedAt = null;
                            response.Add(region);
                            region = new RegionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<RegionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<RegionModel> Create(string connectionString, RegionModel region)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO sch_regions (";
                string values = "(";
                if (!string.IsNullOrEmpty(region.Id))
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
                    cmd.Parameters.AddWithValue("@id", region.Id);
                }
                if (!string.IsNullOrEmpty(region.CountryIso))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `country_iso` ");
                        values = string.Concat(values, ", @countryIso");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `country_iso` ");
                        values = string.Concat(values, " @countryIso");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIso", region.CountryIso);
                }
                if (!string.IsNullOrEmpty(region.Code))
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
                    cmd.Parameters.AddWithValue("@code", region.Code);
                }
                if (!string.IsNullOrEmpty(region.Name))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `name` ");
                        values = string.Concat(values, ", @name");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `name` ");
                        values = string.Concat(values, " @name");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", region.Name);
                }
                if (!string.IsNullOrEmpty(region.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", region.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(region.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", region.CreatedAt);
                }
                if (!string.IsNullOrEmpty(region.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", region.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                region.Id = cmd.LastInsertedId.ToString();
                response.Add(region);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
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
        public List<RegionModel> Create(string connectionString, List<RegionModel> regionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < regionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO sch_regions (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(regionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, regionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].CountryIso))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `country_iso` ");
                                        values = string.Concat(values, ", @countryIso" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `country_iso` ");
                                        values = string.Concat(values, "@countryIso" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIso" + i, regionList[i].CountryIso);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].Code))
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
                                    cmd.Parameters.AddWithValue("@code" + i, regionList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].Name))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `name` ");
                                        values = string.Concat(values, ", @name" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `name` ");
                                        values = string.Concat(values, "@name" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, regionList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, regionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, regionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, regionList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(regionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<RegionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RegionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
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

        public List<RegionModel> Update(string connectionString, RegionModel region, RegionModel regionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE sch_regions SET ";
                if (!string.IsNullOrEmpty(region.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", region.Id);
                }
                if (!string.IsNullOrEmpty(region.CountryIso))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `country_iso` = @countryIso");
                    else
                        sql = string.Concat(sql, " `country_iso` = @countryIso");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@countryIso", region.CountryIso);
                }
                if (!string.IsNullOrEmpty(region.Code))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `code` = @code");
                    else
                        sql = string.Concat(sql, " `code` = @code");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@code", region.Code);
                }
                if (!string.IsNullOrEmpty(region.Name))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `name` = @name");
                    else
                        sql = string.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", region.Name);
                }
                if (!string.IsNullOrEmpty(region.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", region.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(region.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", region.CreatedAt);
                }
                if (!string.IsNullOrEmpty(region.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", region.UpdatedAt);
                }
                if (regionWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(regionWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", regionWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(regionWhere.CountryIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso` = @countryIsoWhere");
                        else
                            sql = string.Concat(sql, " `country_iso` = @countryIsoWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIsoWhere", regionWhere.CountryIso);
                    }
                    if (!string.IsNullOrEmpty(regionWhere.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @codeWhere");
                        else
                            sql = string.Concat(sql, " `code` = @codeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@codeWhere", regionWhere.Code);
                    }
                    if (!string.IsNullOrEmpty(regionWhere.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = string.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", regionWhere.Name);
                    }
                    if (!string.IsNullOrEmpty(regionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", regionWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(regionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", regionWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(regionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", regionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(region);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
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

        public List<RegionModel> Update(string connectionString, List<RegionModel> regionList, List<RegionModel> regionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < regionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE sch_regions SET ";
                                if (!string.IsNullOrEmpty(regionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, regionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].CountryIso))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `country_iso` = @countryIso" + i);
                                    else
                                        sql = string.Concat(sql, " `country_iso` = @countryIso" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@countryIso" + i, regionList[i].CountryIso);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].Code))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `code` = @code" + i);
                                    else
                                        sql = string.Concat(sql, " `code` = @code" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@code" + i, regionList[i].Code);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].Name))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = string.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, regionList[i].Name);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, regionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, regionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(regionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, regionList[i].UpdatedAt);
                                }
                                if (regionWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(regionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, regionWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(regionWhereList[i].CountryIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso` = @countryIsoWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso` = @countryIsoWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIsoWhere" + i, regionWhereList[i].CountryIso);
                                    }
                                    if (!string.IsNullOrEmpty(regionWhereList[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @codeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @codeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@codeWhere" + i, regionWhereList[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(regionWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, regionWhereList[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(regionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, regionWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(regionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, regionWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(regionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, regionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(regionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RegionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RegionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
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

        public List<RegionModel> Delete(string connectionString, RegionModel region)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM sch_regions  ";
                if (region != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(region.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", region.Id);
                    }
                    if (!string.IsNullOrEmpty(region.CountryIso))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `country_iso` = @countryIso");
                        else
                            sql = string.Concat(sql, " `country_iso` = @countryIso");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@countryIso", region.CountryIso);
                    }
                    if (!string.IsNullOrEmpty(region.Code))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `code` = @code");
                        else
                            sql = string.Concat(sql, " `code` = @code");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@code", region.Code);
                    }
                    if (!string.IsNullOrEmpty(region.Name))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `name` = @name");
                        else
                            sql = string.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", region.Name);
                    }
                    if (!string.IsNullOrEmpty(region.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", region.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(region.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", region.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(region.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", region.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(region);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
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

        public List<RegionModel> Delete(string connectionString, List<RegionModel> region)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<RegionModel> response = new List<RegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < region.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM sch_regions  ";
                                if (region[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(region[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, region[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(region[i].CountryIso))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `country_iso` = @countryIso" + i);
                                        else
                                            sql = string.Concat(sql, " `country_iso` = @countryIso" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@countryIso" + i, region[i].CountryIso);
                                    }
                                    if (!string.IsNullOrEmpty(region[i].Code))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `code` = @code" + i);
                                        else
                                            sql = string.Concat(sql, " `code` = @code" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@code" + i, region[i].Code);
                                    }
                                    if (!string.IsNullOrEmpty(region[i].Name))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = string.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, region[i].Name);
                                    }
                                    if (!string.IsNullOrEmpty(region[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, region[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(region[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, region[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(region[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, region[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(region[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<RegionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<RegionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<RegionModel>();
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
