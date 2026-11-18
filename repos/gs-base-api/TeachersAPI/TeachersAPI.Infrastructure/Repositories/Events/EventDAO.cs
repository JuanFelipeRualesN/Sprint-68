using System;
using System.Collections.Generic;
using System.Globalization;
using Dapper;
using MySqlConnector;
using TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses;
using TeachersAPI.Domain.Models.Events;

namespace TeachersAPI.Infrastructure.Repositories.Events
{
    class EventDAO
    {
        private MySqlConnection _connection;

        private static string ReadIsoDateTime(object rawValue)
        {
            return ((DateTime)rawValue).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }

        public Dictionary<string, EventModel> ReadD(string connectionString, EventModel eventModel, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EventModel> response = new Dictionary<string, EventModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at` FROM events ";
                {
                    if (eventModel != null)
                    {
                        if (!string.IsNullOrEmpty(eventModel.Id))
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
                            cmd.Parameters.AddWithValue("@id", eventModel.Id);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", eventModel.Title);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", eventModel.Description);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Location))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `location` = @location ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `location` = @location ");
                            }
                            cmd.Parameters.AddWithValue("@location", eventModel.Location);
                        }
                        if (!string.IsNullOrEmpty(eventModel.StartDate))
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
                            cmd.Parameters.AddWithValue("@startDate", eventModel.StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.EndDate))
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
                            cmd.Parameters.AddWithValue("@endDate", eventModel.EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TimeZoneCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `timezone_code` = @timezoneCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `timezone_code` = @timezoneCode ");
                            }
                            cmd.Parameters.AddWithValue("@timezoneCode", eventModel.TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsEventId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_event_id` = @teamsEventId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_event_id` = @teamsEventId ");
                            }
                            cmd.Parameters.AddWithValue("@teamsEventId", eventModel.TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsSyncStatus))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_sync_status` = @teamsSyncStatus ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_sync_status` = @teamsSyncStatus ");
                            }
                            cmd.Parameters.AddWithValue("@teamsSyncStatus", eventModel.TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", eventModel.Available);
                        }
                        if (!string.IsNullOrEmpty(eventModel.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", eventModel.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventModel.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", eventModel.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventModel.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", eventModel.UpdatedAt);
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
                                eventModel = new EventModel();
                                if (!reader.IsDBNull(0))
                                    eventModel.Id = reader.GetValue(0).ToString();
                                else
                                    eventModel.Id = null;
                                if (!reader.IsDBNull(1))
                                    eventModel.Title = reader.GetValue(1).ToString();
                                else
                                    eventModel.Title = null;
                                if (!reader.IsDBNull(2))
                                    eventModel.Description = reader.GetValue(2).ToString();
                                else
                                    eventModel.Description = null;
                                if (!reader.IsDBNull(3))
                                    eventModel.Location = reader.GetValue(3).ToString();
                                else
                                    eventModel.Location = null;
                                if (!reader.IsDBNull(4))
                                    eventModel.StartDate = ReadIsoDateTime(reader.GetValue(4));
                                else
                                    eventModel.StartDate = null;
                                if (!reader.IsDBNull(5))
                                    eventModel.EndDate = ReadIsoDateTime(reader.GetValue(5));
                                else
                                    eventModel.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    eventModel.TimeZoneCode = reader.GetValue(6).ToString();
                                else
                                    eventModel.TimeZoneCode = null;
                                if (!reader.IsDBNull(7))
                                    eventModel.TeamsEventId = reader.GetValue(7).ToString();
                                else
                                    eventModel.TeamsEventId = null;
                                if (!reader.IsDBNull(8))
                                    eventModel.TeamsSyncStatus = reader.GetValue(8).ToString();
                                else
                                    eventModel.TeamsSyncStatus = null;
                                if (!reader.IsDBNull(9))
                                    eventModel.Available = reader.GetValue(9).ToString();
                                else
                                    eventModel.Available = null;
                                if (!reader.IsDBNull(10))
                                    eventModel.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    eventModel.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    eventModel.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    eventModel.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    eventModel.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    eventModel.UpdatedAt = null;
                                response.Add(eventModel.Id, eventModel);
                                eventModel = new EventModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EventModel>();
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
                response = new Dictionary<string, EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public Dictionary<string, EventModel> ReadD(string connectionString, EventModel eventModel, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EventModel> response = new Dictionary<string, EventModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at` FROM events ";
                {
                    if (eventModel != null)
                    {
                        if (!string.IsNullOrEmpty(eventModel.Id))
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
                            cmd.Parameters.AddWithValue("@id", eventModel.Id);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", eventModel.Title);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", eventModel.Description);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Location))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `location` = @location ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `location` = @location ");
                            }
                            cmd.Parameters.AddWithValue("@location", eventModel.Location);
                        }
                        if (!string.IsNullOrEmpty(eventModel.StartDate))
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
                            cmd.Parameters.AddWithValue("@startDate", eventModel.StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.EndDate))
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
                            cmd.Parameters.AddWithValue("@endDate", eventModel.EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TimeZoneCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `timezone_code` = @timezoneCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `timezone_code` = @timezoneCode ");
                            }
                            cmd.Parameters.AddWithValue("@timezoneCode", eventModel.TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsEventId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_event_id` = @teamsEventId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_event_id` = @teamsEventId ");
                            }
                            cmd.Parameters.AddWithValue("@teamsEventId", eventModel.TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsSyncStatus))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_sync_status` = @teamsSyncStatus ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_sync_status` = @teamsSyncStatus ");
                            }
                            cmd.Parameters.AddWithValue("@teamsSyncStatus", eventModel.TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", eventModel.Available);
                        }
                        if (!string.IsNullOrEmpty(eventModel.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", eventModel.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventModel.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", eventModel.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventModel.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", eventModel.UpdatedAt);
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
                                eventModel = new EventModel();
                                if (!reader.IsDBNull(0))
                                    eventModel.Id = reader.GetValue(0).ToString();
                                else
                                    eventModel.Id = null;
                                if (!reader.IsDBNull(1))
                                    eventModel.Title = reader.GetValue(1).ToString();
                                else
                                    eventModel.Title = null;
                                if (!reader.IsDBNull(2))
                                    eventModel.Description = reader.GetValue(2).ToString();
                                else
                                    eventModel.Description = null;
                                if (!reader.IsDBNull(3))
                                    eventModel.Location = reader.GetValue(3).ToString();
                                else
                                    eventModel.Location = null;
                                if (!reader.IsDBNull(4))
                                    eventModel.StartDate = ReadIsoDateTime(reader.GetValue(4));
                                else
                                    eventModel.StartDate = null;
                                if (!reader.IsDBNull(5))
                                    eventModel.EndDate = ReadIsoDateTime(reader.GetValue(5));
                                else
                                    eventModel.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    eventModel.TimeZoneCode = reader.GetValue(6).ToString();
                                else
                                    eventModel.TimeZoneCode = null;
                                if (!reader.IsDBNull(7))
                                    eventModel.TeamsEventId = reader.GetValue(7).ToString();
                                else
                                    eventModel.TeamsEventId = null;
                                if (!reader.IsDBNull(8))
                                    eventModel.TeamsSyncStatus = reader.GetValue(8).ToString();
                                else
                                    eventModel.TeamsSyncStatus = null;
                                if (!reader.IsDBNull(9))
                                    eventModel.Available = reader.GetValue(9).ToString();
                                else
                                    eventModel.Available = null;
                                if (!reader.IsDBNull(10))
                                    eventModel.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    eventModel.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    eventModel.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    eventModel.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    eventModel.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    eventModel.UpdatedAt = null;
                                response.Add(eventModel.Id, eventModel);
                                eventModel = new EventModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EventModel>();
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
                response = new Dictionary<string, EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public Dictionary<string, EventModel> ReadD(string connectionString, List<EventModel> eventWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EventModel> response = new Dictionary<string, EventModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at` FROM events ";
                if (eventWhere != null && eventWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < eventWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(eventWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, eventWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, eventWhere[i].Title);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, eventWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Location))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `location` = @location" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@location" + i, eventWhere[i].Location);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].StartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `start_date` = @startDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@startDate" + i, eventWhere[i].StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, eventWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TimeZoneCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `timezone_code` = @timezoneCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timezoneCode" + i, eventWhere[i].TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsEventId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_event_id` = @teamsEventId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsEventId" + i, eventWhere[i].TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsSyncStatus))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_sync_status` = @teamsSyncStatus" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsSyncStatus" + i, eventWhere[i].TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, eventWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, eventWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, eventWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, eventWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != eventWhere.Count - 1)
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
                            EventModel eventRow = new EventModel();
                            if (!reader.IsDBNull(0))
                                eventRow.Id = reader.GetValue(0).ToString();
                            else
                                eventRow.Id = null;
                            if (!reader.IsDBNull(1))
                                eventRow.Title = reader.GetValue(1).ToString();
                            else
                                eventRow.Title = null;
                            if (!reader.IsDBNull(2))
                                eventRow.Description = reader.GetValue(2).ToString();
                            else
                                eventRow.Description = null;
                            if (!reader.IsDBNull(3))
                                eventRow.Location = reader.GetValue(3).ToString();
                            else
                                eventRow.Location = null;
                            if (!reader.IsDBNull(4))
                                eventRow.StartDate = ReadIsoDateTime(reader.GetValue(4));
                            else
                                eventRow.StartDate = null;
                            if (!reader.IsDBNull(5))
                                eventRow.EndDate = ReadIsoDateTime(reader.GetValue(5));
                            else
                                eventRow.EndDate = null;
                            if (!reader.IsDBNull(6))
                                eventRow.TimeZoneCode = reader.GetValue(6).ToString();
                            else
                                eventRow.TimeZoneCode = null;
                            if (!reader.IsDBNull(7))
                                eventRow.TeamsEventId = reader.GetValue(7).ToString();
                            else
                                eventRow.TeamsEventId = null;
                            if (!reader.IsDBNull(8))
                                eventRow.TeamsSyncStatus = reader.GetValue(8).ToString();
                            else
                                eventRow.TeamsSyncStatus = null;
                            if (!reader.IsDBNull(9))
                                eventRow.Available = reader.GetValue(9).ToString();
                            else
                                eventRow.Available = null;
                            if (!reader.IsDBNull(10))
                                eventRow.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                eventRow.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                eventRow.CreatedAt = reader.GetValue(11).ToString();
                            else
                                eventRow.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                eventRow.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                eventRow.UpdatedAt = null;
                            response.Add(eventRow.Id, eventRow);
                            eventRow = new EventModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EventModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public Dictionary<string, EventModel> ReadD(string connectionString, List<EventModel> eventWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EventModel> response = new Dictionary<string, EventModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at` FROM events ";
                if (eventWhere != null && eventWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < eventWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(eventWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, eventWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, eventWhere[i].Title);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, eventWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Location))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `location` = @location" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@location" + i, eventWhere[i].Location);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].StartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `start_date` = @startDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@startDate" + i, eventWhere[i].StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, eventWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TimeZoneCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `timezone_code` = @timezoneCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timezoneCode" + i, eventWhere[i].TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsEventId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_event_id` = @teamsEventId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsEventId" + i, eventWhere[i].TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsSyncStatus))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_sync_status` = @teamsSyncStatus" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsSyncStatus" + i, eventWhere[i].TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, eventWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, eventWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, eventWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, eventWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != eventWhere.Count - 1)
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
                            EventModel eventRow = new EventModel();
                            if (!reader.IsDBNull(0))
                                eventRow.Id = reader.GetValue(0).ToString();
                            else
                                eventRow.Id = null;
                            if (!reader.IsDBNull(1))
                                eventRow.Title = reader.GetValue(1).ToString();
                            else
                                eventRow.Title = null;
                            if (!reader.IsDBNull(2))
                                eventRow.Description = reader.GetValue(2).ToString();
                            else
                                eventRow.Description = null;
                            if (!reader.IsDBNull(3))
                                eventRow.Location = reader.GetValue(3).ToString();
                            else
                                eventRow.Location = null;
                            if (!reader.IsDBNull(4))
                                eventRow.StartDate = ReadIsoDateTime(reader.GetValue(4));
                            else
                                eventRow.StartDate = null;
                            if (!reader.IsDBNull(5))
                                eventRow.EndDate = ReadIsoDateTime(reader.GetValue(5));
                            else
                                eventRow.EndDate = null;
                            if (!reader.IsDBNull(6))
                                eventRow.TimeZoneCode = reader.GetValue(6).ToString();
                            else
                                eventRow.TimeZoneCode = null;
                            if (!reader.IsDBNull(7))
                                eventRow.TeamsEventId = reader.GetValue(7).ToString();
                            else
                                eventRow.TeamsEventId = null;
                            if (!reader.IsDBNull(8))
                                eventRow.TeamsSyncStatus = reader.GetValue(8).ToString();
                            else
                                eventRow.TeamsSyncStatus = null;
                            if (!reader.IsDBNull(9))
                                eventRow.Available = reader.GetValue(9).ToString();
                            else
                                eventRow.Available = null;
                            if (!reader.IsDBNull(10))
                                eventRow.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                eventRow.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                eventRow.CreatedAt = reader.GetValue(11).ToString();
                            else
                                eventRow.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                eventRow.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                eventRow.UpdatedAt = null;
                            response.Add(eventRow.Id, eventRow);
                            eventRow = new EventModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EventModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public List<EventModel> Read(string connectionString, EventModel eventModel, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at`, `created_by`, `teams_synced_at` FROM events ";
                {
                    if (eventModel != null)
                    {
                        if (!string.IsNullOrEmpty(eventModel.Id))
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
                            cmd.Parameters.AddWithValue("@id", eventModel.Id);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Title))
                        {
                            eventModel.Title = $"%{eventModel.Title}%";
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` LIKE @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` LIKE @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", eventModel.Title);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", eventModel.Description);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Location))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `location` = @location ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `location` = @location ");
                            }
                            cmd.Parameters.AddWithValue("@location", eventModel.Location);
                        }
                        if (!string.IsNullOrEmpty(eventModel.StartDate))
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
                            cmd.Parameters.AddWithValue("@startDate", eventModel.StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.EndDate))
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
                            cmd.Parameters.AddWithValue("@endDate", eventModel.EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TimeZoneCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `timezone_code` = @timezoneCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `timezone_code` = @timezoneCode ");
                            }
                            cmd.Parameters.AddWithValue("@timezoneCode", eventModel.TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsEventId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_event_id` = @teamsEventId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_event_id` = @teamsEventId ");
                            }
                            cmd.Parameters.AddWithValue("@teamsEventId", eventModel.TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsSyncStatus))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_sync_status` = @teamsSyncStatus ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_sync_status` = @teamsSyncStatus ");
                            }
                            cmd.Parameters.AddWithValue("@teamsSyncStatus", eventModel.TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", eventModel.Available);
                        }
                        if (!string.IsNullOrEmpty(eventModel.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", eventModel.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventModel.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where DATE(`created_at`) = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and DATE(`created_at`) = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", eventModel.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventModel.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", eventModel.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        else
                            sql = string.Concat(sql, " ORDER BY created_at DESC, id DESC");
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
                                eventModel = new EventModel();
                                if (!reader.IsDBNull(0))
                                    eventModel.Id = reader.GetValue(0).ToString();
                                else
                                    eventModel.Id = null;
                                if (!reader.IsDBNull(1))
                                    eventModel.Title = reader.GetValue(1).ToString();
                                else
                                    eventModel.Title = null;
                                if (!reader.IsDBNull(2))
                                    eventModel.Description = reader.GetValue(2).ToString();
                                else
                                    eventModel.Description = null;
                                if (!reader.IsDBNull(3))
                                    eventModel.Location = reader.GetValue(3).ToString();
                                else
                                    eventModel.Location = null;
                                if (!reader.IsDBNull(4))
                                    eventModel.StartDate = ReadIsoDateTime(reader.GetValue(4));
                                else
                                    eventModel.StartDate = null;
                                if (!reader.IsDBNull(5))
                                    eventModel.EndDate = ReadIsoDateTime(reader.GetValue(5));
                                else
                                    eventModel.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    eventModel.TimeZoneCode = reader.GetValue(6).ToString();
                                else
                                    eventModel.TimeZoneCode = null;
                                if (!reader.IsDBNull(7))
                                    eventModel.TeamsEventId = reader.GetValue(7).ToString();
                                else
                                    eventModel.TeamsEventId = null;
                                if (!reader.IsDBNull(8))
                                    eventModel.TeamsSyncStatus = reader.GetValue(8).ToString();
                                else
                                    eventModel.TeamsSyncStatus = null;
                                if (!reader.IsDBNull(9))
                                    eventModel.Available = reader.GetValue(9).ToString();
                                else
                                    eventModel.Available = null;
                                if (!reader.IsDBNull(10))
                                    eventModel.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    eventModel.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    eventModel.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    eventModel.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    eventModel.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    eventModel.UpdatedAt = null;
                                if (!reader.IsDBNull(13))
                                    eventModel.CreatedBy = reader.GetValue(13).ToString();
                                else
                                    eventModel.CreatedBy = null;
                                if (!reader.IsDBNull(14))
                                    eventModel.TeamsSyncedAt = ReadIsoDateTime(reader.GetValue(14));
                                else
                                    eventModel.TeamsSyncedAt = null;
                                response.Add(eventModel);
                                eventModel = new EventModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EventModel>();
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
                response = new List<EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public List<EventModel> Read(string connectionString, EventModel eventModel, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at` FROM events ";
                {
                    if (eventModel != null)
                    {
                        if (!string.IsNullOrEmpty(eventModel.Id))
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
                            cmd.Parameters.AddWithValue("@id", eventModel.Id);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Title))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `title` = @title ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `title` = @title ");
                            }
                            cmd.Parameters.AddWithValue("@title", eventModel.Title);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Description))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `description` = @description ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `description` = @description ");
                            }
                            cmd.Parameters.AddWithValue("@description", eventModel.Description);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Location))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `location` = @location ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `location` = @location ");
                            }
                            cmd.Parameters.AddWithValue("@location", eventModel.Location);
                        }
                        if (!string.IsNullOrEmpty(eventModel.StartDate))
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
                            cmd.Parameters.AddWithValue("@startDate", eventModel.StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.EndDate))
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
                            cmd.Parameters.AddWithValue("@endDate", eventModel.EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TimeZoneCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `timezone_code` = @timezoneCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `timezone_code` = @timezoneCode ");
                            }
                            cmd.Parameters.AddWithValue("@timezoneCode", eventModel.TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsEventId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_event_id` = @teamsEventId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_event_id` = @teamsEventId ");
                            }
                            cmd.Parameters.AddWithValue("@teamsEventId", eventModel.TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventModel.TeamsSyncStatus))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `teams_sync_status` = @teamsSyncStatus ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `teams_sync_status` = @teamsSyncStatus ");
                            }
                            cmd.Parameters.AddWithValue("@teamsSyncStatus", eventModel.TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventModel.Available))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `available` = @available ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `available` = @available ");
                            }
                            cmd.Parameters.AddWithValue("@available", eventModel.Available);
                        }
                        if (!string.IsNullOrEmpty(eventModel.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", eventModel.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventModel.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", eventModel.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventModel.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", eventModel.UpdatedAt);
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
                                eventModel = new EventModel();
                                if (!reader.IsDBNull(0))
                                    eventModel.Id = reader.GetValue(0).ToString();
                                else
                                    eventModel.Id = null;
                                if (!reader.IsDBNull(1))
                                    eventModel.Title = reader.GetValue(1).ToString();
                                else
                                    eventModel.Title = null;
                                if (!reader.IsDBNull(2))
                                    eventModel.Description = reader.GetValue(2).ToString();
                                else
                                    eventModel.Description = null;
                                if (!reader.IsDBNull(3))
                                    eventModel.Location = reader.GetValue(3).ToString();
                                else
                                    eventModel.Location = null;
                                if (!reader.IsDBNull(4))
                                    eventModel.StartDate = ReadIsoDateTime(reader.GetValue(4));
                                else
                                    eventModel.StartDate = null;
                                if (!reader.IsDBNull(5))
                                    eventModel.EndDate = ReadIsoDateTime(reader.GetValue(5));
                                else
                                    eventModel.EndDate = null;
                                if (!reader.IsDBNull(6))
                                    eventModel.TimeZoneCode = reader.GetValue(6).ToString();
                                else
                                    eventModel.TimeZoneCode = null;
                                if (!reader.IsDBNull(7))
                                    eventModel.TeamsEventId = reader.GetValue(7).ToString();
                                else
                                    eventModel.TeamsEventId = null;
                                if (!reader.IsDBNull(8))
                                    eventModel.TeamsSyncStatus = reader.GetValue(8).ToString();
                                else
                                    eventModel.TeamsSyncStatus = null;
                                if (!reader.IsDBNull(9))
                                    eventModel.Available = reader.GetValue(9).ToString();
                                else
                                    eventModel.Available = null;
                                if (!reader.IsDBNull(10))
                                    eventModel.ModifiedBy = reader.GetValue(10).ToString();
                                else
                                    eventModel.ModifiedBy = null;
                                if (!reader.IsDBNull(11))
                                    eventModel.CreatedAt = reader.GetValue(11).ToString();
                                else
                                    eventModel.CreatedAt = null;
                                if (!reader.IsDBNull(12))
                                    eventModel.UpdatedAt = reader.GetValue(12).ToString();
                                else
                                    eventModel.UpdatedAt = null;
                                response.Add(eventModel);
                                eventModel = new EventModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EventModel>();
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
                response = new List<EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public List<EventModel> Read(string connectionString, List<EventModel> eventWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at` FROM events ";
                if (eventWhere != null && eventWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < eventWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(eventWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, eventWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, eventWhere[i].Title);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, eventWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Location))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `location` = @location" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@location" + i, eventWhere[i].Location);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].StartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `start_date` = @startDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@startDate" + i, eventWhere[i].StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, eventWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TimeZoneCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `timezone_code` = @timezoneCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timezoneCode" + i, eventWhere[i].TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsEventId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_event_id` = @teamsEventId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsEventId" + i, eventWhere[i].TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsSyncStatus))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_sync_status` = @teamsSyncStatus" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsSyncStatus" + i, eventWhere[i].TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, eventWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, eventWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, eventWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, eventWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != eventWhere.Count - 1)
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
                            EventModel eventRow = new EventModel();
                            if (!reader.IsDBNull(0))
                                eventRow.Id = reader.GetValue(0).ToString();
                            else
                                eventRow.Id = null;
                            if (!reader.IsDBNull(1))
                                eventRow.Title = reader.GetValue(1).ToString();
                            else
                                eventRow.Title = null;
                            if (!reader.IsDBNull(2))
                                eventRow.Description = reader.GetValue(2).ToString();
                            else
                                eventRow.Description = null;
                            if (!reader.IsDBNull(3))
                                eventRow.Location = reader.GetValue(3).ToString();
                            else
                                eventRow.Location = null;
                            if (!reader.IsDBNull(4))
                                eventRow.StartDate = ReadIsoDateTime(reader.GetValue(4));
                            else
                                eventRow.StartDate = null;
                            if (!reader.IsDBNull(5))
                                eventRow.EndDate = ReadIsoDateTime(reader.GetValue(5));
                            else
                                eventRow.EndDate = null;
                            if (!reader.IsDBNull(6))
                                eventRow.TimeZoneCode = reader.GetValue(6).ToString();
                            else
                                eventRow.TimeZoneCode = null;
                            if (!reader.IsDBNull(7))
                                eventRow.TeamsEventId = reader.GetValue(7).ToString();
                            else
                                eventRow.TeamsEventId = null;
                            if (!reader.IsDBNull(8))
                                eventRow.TeamsSyncStatus = reader.GetValue(8).ToString();
                            else
                                eventRow.TeamsSyncStatus = null;
                            if (!reader.IsDBNull(9))
                                eventRow.Available = reader.GetValue(9).ToString();
                            else
                                eventRow.Available = null;
                            if (!reader.IsDBNull(10))
                                eventRow.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                eventRow.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                eventRow.CreatedAt = reader.GetValue(11).ToString();
                            else
                                eventRow.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                eventRow.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                eventRow.UpdatedAt = null;
                            response.Add(eventRow);
                            eventRow = new EventModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EventModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public List<EventModel> Read(string connectionString, List<EventModel> eventWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `title`, `description`, `location`, `start_date`, `end_date`, `timezone_code`, `teams_event_id`, `teams_sync_status`, `available`, `modified_by`, `created_at`, `updated_at` FROM events ";
                if (eventWhere != null && eventWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < eventWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(eventWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, eventWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Title))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `title` = @title" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@title" + i, eventWhere[i].Title);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Description))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `description` = @description" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@description" + i, eventWhere[i].Description);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Location))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `location` = @location" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@location" + i, eventWhere[i].Location);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].StartDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `start_date` = @startDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@startDate" + i, eventWhere[i].StartDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].EndDate))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `end_date` = @endDate" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@endDate" + i, eventWhere[i].EndDate);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TimeZoneCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `timezone_code` = @timezoneCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@timezoneCode" + i, eventWhere[i].TimeZoneCode);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsEventId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_event_id` = @teamsEventId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsEventId" + i, eventWhere[i].TeamsEventId);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].TeamsSyncStatus))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `teams_sync_status` = @teamsSyncStatus" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@teamsSyncStatus" + i, eventWhere[i].TeamsSyncStatus);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].Available))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `available` = @available" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@available" + i, eventWhere[i].Available);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, eventWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, eventWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(eventWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";
                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, eventWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != eventWhere.Count - 1)
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
                            EventModel eventRow = new EventModel();
                            if (!reader.IsDBNull(0))
                                eventRow.Id = reader.GetValue(0).ToString();
                            else
                                eventRow.Id = null;
                            if (!reader.IsDBNull(1))
                                eventRow.Title = reader.GetValue(1).ToString();
                            else
                                eventRow.Title = null;
                            if (!reader.IsDBNull(2))
                                eventRow.Description = reader.GetValue(2).ToString();
                            else
                                eventRow.Description = null;
                            if (!reader.IsDBNull(3))
                                eventRow.Location = reader.GetValue(3).ToString();
                            else
                                eventRow.Location = null;
                            if (!reader.IsDBNull(4))
                                eventRow.StartDate = ReadIsoDateTime(reader.GetValue(4));
                            else
                                eventRow.StartDate = null;
                            if (!reader.IsDBNull(5))
                                eventRow.EndDate = ReadIsoDateTime(reader.GetValue(5));
                            else
                                eventRow.EndDate = null;
                            if (!reader.IsDBNull(6))
                                eventRow.TimeZoneCode = reader.GetValue(6).ToString();
                            else
                                eventRow.TimeZoneCode = null;
                            if (!reader.IsDBNull(7))
                                eventRow.TeamsEventId = reader.GetValue(7).ToString();
                            else
                                eventRow.TeamsEventId = null;
                            if (!reader.IsDBNull(8))
                                eventRow.TeamsSyncStatus = reader.GetValue(8).ToString();
                            else
                                eventRow.TeamsSyncStatus = null;
                            if (!reader.IsDBNull(9))
                                eventRow.Available = reader.GetValue(9).ToString();
                            else
                                eventRow.Available = null;
                            if (!reader.IsDBNull(10))
                                eventRow.ModifiedBy = reader.GetValue(10).ToString();
                            else
                                eventRow.ModifiedBy = null;
                            if (!reader.IsDBNull(11))
                                eventRow.CreatedAt = reader.GetValue(11).ToString();
                            else
                                eventRow.CreatedAt = null;
                            if (!reader.IsDBNull(12))
                                eventRow.UpdatedAt = reader.GetValue(12).ToString();
                            else
                                eventRow.UpdatedAt = null;
                            response.Add(eventRow);
                            eventRow = new EventModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EventModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EventModel> Create(string connectionString, EventModel eventModel)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO events (";
                string values = "(";
                if (!string.IsNullOrEmpty(eventModel.Id))
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
                    cmd.Parameters.AddWithValue("@id", eventModel.Id);
                }
                if (!string.IsNullOrEmpty(eventModel.Title))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `title` ");
                        values = string.Concat(values, ", @title");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `title` ");
                        values = string.Concat(values, " @title");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", eventModel.Title);
                }
                if (!string.IsNullOrEmpty(eventModel.Description))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `description` ");
                        values = string.Concat(values, ", @description");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `description` ");
                        values = string.Concat(values, " @description");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", eventModel.Description);
                }
                if (!string.IsNullOrEmpty(eventModel.Location))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `location` ");
                        values = string.Concat(values, ", @location");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `location` ");
                        values = string.Concat(values, " @location");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@location", eventModel.Location);
                }
                if (!string.IsNullOrEmpty(eventModel.StartDate))
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
                    cmd.Parameters.AddWithValue("@startDate", eventModel.StartDate);
                }
                if (!string.IsNullOrEmpty(eventModel.EndDate))
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
                    cmd.Parameters.AddWithValue("@endDate", eventModel.EndDate);
                }
                if (!string.IsNullOrEmpty(eventModel.TimeZoneCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `timezone_code` ");
                        values = string.Concat(values, ", @timezoneCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `timezone_code` ");
                        values = string.Concat(values, " @timezoneCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@timezoneCode", eventModel.TimeZoneCode);
                }
                if (!string.IsNullOrEmpty(eventModel.TeamsEventId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `teams_event_id` ");
                        values = string.Concat(values, ", @teamsEventId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `teams_event_id` ");
                        values = string.Concat(values, " @teamsEventId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teamsEventId", eventModel.TeamsEventId);
                }
                if (!string.IsNullOrEmpty(eventModel.TeamsSyncStatus))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `teams_sync_status` ");
                        values = string.Concat(values, ", @teamsSyncStatus");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `teams_sync_status` ");
                        values = string.Concat(values, " @teamsSyncStatus");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teamsSyncStatus", eventModel.TeamsSyncStatus);
                }
                if (!string.IsNullOrEmpty(eventModel.Available))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `available` ");
                        values = string.Concat(values, ", @available");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `available` ");
                        values = string.Concat(values, " @available");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", eventModel.Available);
                }
                if (!string.IsNullOrEmpty(eventModel.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", eventModel.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(eventModel.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", eventModel.CreatedAt);
                }
                if (!string.IsNullOrEmpty(eventModel.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", eventModel.UpdatedAt);
                }
                if (!string.IsNullOrEmpty(eventModel.CreatedBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `created_by` ");
                        values = string.Concat(values, ", @createdBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `created_by` ");
                        values = string.Concat(values, " @createdBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdBy", eventModel.CreatedBy);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                response.Add(eventModel);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
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

        public List<EventModel> Update(string connectionString, EventModel eventModel, EventModel eventModelWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE events SET ";
                if (!string.IsNullOrEmpty(eventModel.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", eventModel.Id);
                }
                if (!string.IsNullOrEmpty(eventModel.Title))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `title` = @title");
                    else
                        sql = string.Concat(sql, " `title` = @title");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@title", eventModel.Title);
                }
                if (!string.IsNullOrEmpty(eventModel.Description))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `description` = @description");
                    else
                        sql = string.Concat(sql, " `description` = @description");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@description", eventModel.Description);
                }
                if (!string.IsNullOrEmpty(eventModel.Location))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `location` = @location");
                    else
                        sql = string.Concat(sql, " `location` = @location");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@location", eventModel.Location);
                }
                if (!string.IsNullOrEmpty(eventModel.StartDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `start_date` = @startDate");
                    else
                        sql = string.Concat(sql, " `start_date` = @startDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@startDate", eventModel.StartDate);
                }
                if (!string.IsNullOrEmpty(eventModel.EndDate))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `end_date` = @endDate");
                    else
                        sql = string.Concat(sql, " `end_date` = @endDate");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@endDate", eventModel.EndDate);
                }
                if (!string.IsNullOrEmpty(eventModel.TimeZoneCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `timezone_code` = @timezoneCode");
                    else
                        sql = string.Concat(sql, " `timezone_code` = @timezoneCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@timezoneCode", eventModel.TimeZoneCode);
                }
                if (!string.IsNullOrEmpty(eventModel.TeamsEventId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `teams_event_id` = @teamsEventId");
                    else
                        sql = string.Concat(sql, " `teams_event_id` = @teamsEventId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teamsEventId", eventModel.TeamsEventId);
                }
                if (!string.IsNullOrEmpty(eventModel.TeamsSyncStatus))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `teams_sync_status` = @teamsSyncStatus");
                    else
                        sql = string.Concat(sql, " `teams_sync_status` = @teamsSyncStatus");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teamsSyncStatus", eventModel.TeamsSyncStatus);
                }
                if (!string.IsNullOrEmpty(eventModel.Available))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `available` = @available");
                    else
                        sql = string.Concat(sql, " `available` = @available");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@available", eventModel.Available);
                }
                if (!string.IsNullOrEmpty(eventModel.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", eventModel.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(eventModel.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", eventModel.CreatedAt);
                }
                if (!string.IsNullOrEmpty(eventModel.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", eventModel.UpdatedAt);
                }
                if (!string.IsNullOrEmpty(eventModel.CreatedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_by` = @createdBy");
                    else
                        sql = string.Concat(sql, " `created_by` = @createdBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdBy", eventModel.CreatedBy);
                }
                if (!string.IsNullOrEmpty(eventModel.TeamsSyncedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `teams_synced_at` = @teamsSyncedAt");
                    else
                        sql = string.Concat(sql, " `teams_synced_at` = @teamsSyncedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@teamsSyncedAt", eventModel.TeamsSyncedAt);
                }
                if (eventModelWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(eventModelWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", eventModelWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.Title))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `title` = @titleWhere");
                        else
                            sql = string.Concat(sql, " `title` = @titleWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@titleWhere", eventModelWhere.Title);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @descriptionWhere");
                        else
                            sql = string.Concat(sql, " `description` = @descriptionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descriptionWhere", eventModelWhere.Description);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.Location))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `location` = @locationWhere");
                        else
                            sql = string.Concat(sql, " `location` = @locationWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@locationWhere", eventModelWhere.Location);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.StartDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `start_date` = @startDateWhere");
                        else
                            sql = string.Concat(sql, " `start_date` = @startDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@startDateWhere", eventModelWhere.StartDate);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.EndDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `end_date` = @endDateWhere");
                        else
                            sql = string.Concat(sql, " `end_date` = @endDateWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@endDateWhere", eventModelWhere.EndDate);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.TimeZoneCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `timezone_code` = @timezoneCodeWhere");
                        else
                            sql = string.Concat(sql, " `timezone_code` = @timezoneCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@timezoneCodeWhere", eventModelWhere.TimeZoneCode);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.TeamsEventId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `teams_event_id` = @teamsEventIdWhere");
                        else
                            sql = string.Concat(sql, " `teams_event_id` = @teamsEventIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teamsEventIdWhere", eventModelWhere.TeamsEventId);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.TeamsSyncStatus))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `teams_sync_status` = @teamsSyncStatusWhere");
                        else
                            sql = string.Concat(sql, " `teams_sync_status` = @teamsSyncStatusWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teamsSyncStatusWhere", eventModelWhere.TeamsSyncStatus);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @availableWhere");
                        else
                            sql = string.Concat(sql, " `available` = @availableWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@availableWhere", eventModelWhere.Available);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", eventModelWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", eventModelWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(eventModelWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", eventModelWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(eventModel);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
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

        /// <summary>
        /// Clears a dead TeamsEventId (Graph confirmed the event no longer exists) and sets the
        /// sync status, in a single direct statement. Deliberately separate from Update(), whose
        /// conditional SET builder can never write NULL to a column.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="eventId">The event id whose TeamsEventId must be cleared.</param>
        /// <param name="teamsSyncStatus">The TeamsSyncStatus value to persist alongside the clear.</param>
        public void ClearTeamsEventId(string connectionString, string eventId, string teamsSyncStatus)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection,
                    CommandText = "UPDATE events SET `teams_event_id` = NULL, `teams_sync_status` = @teamsSyncStatus WHERE `id` = @id"
                };
                cmd.Parameters.AddWithValue("@teamsSyncStatus", teamsSyncStatus);
                cmd.Parameters.AddWithValue("@id", eventId);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while clearing the Teams event id:" + e.ErrorCode + " - " + e.Message;
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
        }

        /// <summary>
        /// Records one Microsoft Graph sync attempt (create/update/delete), regardless of outcome.
        /// Insert-only, direct statement — mirrors ClearTeamsEventId's style.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="log">The sync log entry to insert.</param>
        public void InsertSyncLog(string connectionString, EventSyncLogModel log)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection,
                    CommandText = "INSERT INTO event_sync_logs (`id`, `event_id`, `action`, `status`, `error_message`) VALUES (@id, @eventId, @action, @status, @errorMessage)"
                };
                cmd.Parameters.AddWithValue("@id", System.Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@eventId", log.EventId);
                cmd.Parameters.AddWithValue("@action", log.Action);
                cmd.Parameters.AddWithValue("@status", log.Status);
                cmd.Parameters.AddWithValue("@errorMessage", (object)log.ErrorMessage ?? System.DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while inserting a sync log:" + e.ErrorCode + " - " + e.Message;
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
        }

        public List<EventModel> Delete(string connectionString, EventModel eventModel)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM events  ";
                if (eventModel != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(eventModel.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", eventModel.Id);
                    }
                    if (!string.IsNullOrEmpty(eventModel.Title))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `title` = @title");
                        else
                            sql = string.Concat(sql, " `title` = @title");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@title", eventModel.Title);
                    }
                    if (!string.IsNullOrEmpty(eventModel.Description))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `description` = @description");
                        else
                            sql = string.Concat(sql, " `description` = @description");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@description", eventModel.Description);
                    }
                    if (!string.IsNullOrEmpty(eventModel.Location))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `location` = @location");
                        else
                            sql = string.Concat(sql, " `location` = @location");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@location", eventModel.Location);
                    }
                    if (!string.IsNullOrEmpty(eventModel.StartDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `start_date` = @startDate");
                        else
                            sql = string.Concat(sql, " `start_date` = @startDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@startDate", eventModel.StartDate);
                    }
                    if (!string.IsNullOrEmpty(eventModel.EndDate))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `end_date` = @endDate");
                        else
                            sql = string.Concat(sql, " `end_date` = @endDate");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@endDate", eventModel.EndDate);
                    }
                    if (!string.IsNullOrEmpty(eventModel.TimeZoneCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `timezone_code` = @timezoneCode");
                        else
                            sql = string.Concat(sql, " `timezone_code` = @timezoneCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@timezoneCode", eventModel.TimeZoneCode);
                    }
                    if (!string.IsNullOrEmpty(eventModel.TeamsEventId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `teams_event_id` = @teamsEventId");
                        else
                            sql = string.Concat(sql, " `teams_event_id` = @teamsEventId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teamsEventId", eventModel.TeamsEventId);
                    }
                    if (!string.IsNullOrEmpty(eventModel.TeamsSyncStatus))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `teams_sync_status` = @teamsSyncStatus");
                        else
                            sql = string.Concat(sql, " `teams_sync_status` = @teamsSyncStatus");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@teamsSyncStatus", eventModel.TeamsSyncStatus);
                    }
                    if (!string.IsNullOrEmpty(eventModel.Available))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `available` = @available");
                        else
                            sql = string.Concat(sql, " `available` = @available");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@available", eventModel.Available);
                    }
                    if (!string.IsNullOrEmpty(eventModel.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", eventModel.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(eventModel.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", eventModel.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(eventModel.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", eventModel.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(eventModel);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
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

        public List<EventModel> Create(string connectionString, List<EventModel> eventModelList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
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
                        for (int i = 0; i < eventModelList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO events (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(eventModelList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, eventModelList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Title))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `title` ");
                                        values = string.Concat(values, ", @title" + i);
                                    }
                                    else
                                    {
                                        sql = string.Concat(sql, " `title` ");
                                        values = string.Concat(values, "@title" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, eventModelList[i].Title);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Description))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `description` ");
                                        values = string.Concat(values, ", @description" + i);
                                    }
                                    else
                                    {
                                        sql = string.Concat(sql, " `description` ");
                                        values = string.Concat(values, "@description" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, eventModelList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Location))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `location` ");
                                        values = string.Concat(values, ", @location" + i);
                                    }
                                    else
                                    {
                                        sql = string.Concat(sql, " `location` ");
                                        values = string.Concat(values, "@location" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@location" + i, eventModelList[i].Location);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].StartDate))
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
                                    cmd.Parameters.AddWithValue("@startDate" + i, eventModelList[i].StartDate);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].EndDate))
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
                                    cmd.Parameters.AddWithValue("@endDate" + i, eventModelList[i].EndDate);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].TimeZoneCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `timezone_code` ");
                                        values = string.Concat(values, ", @timezoneCode" + i);
                                    }
                                    else
                                    {
                                        sql = string.Concat(sql, " `timezone_code` ");
                                        values = string.Concat(values, "@timezoneCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@timezoneCode" + i, eventModelList[i].TimeZoneCode);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].TeamsEventId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `teams_event_id` ");
                                        values = string.Concat(values, ", @teamsEventId" + i);
                                    }
                                    else
                                    {
                                        sql = string.Concat(sql, " `teams_event_id` ");
                                        values = string.Concat(values, "@teamsEventId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teamsEventId" + i, eventModelList[i].TeamsEventId);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].TeamsSyncStatus))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `teams_sync_status` ");
                                        values = string.Concat(values, ", @teamsSyncStatus" + i);
                                    }
                                    else
                                    {
                                        sql = string.Concat(sql, " `teams_sync_status` ");
                                        values = string.Concat(values, "@teamsSyncStatus" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teamsSyncStatus" + i, eventModelList[i].TeamsSyncStatus);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Available))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `available` ");
                                        values = string.Concat(values, ", @available" + i);
                                    }
                                    else
                                    {
                                        sql = string.Concat(sql, " `available` ");
                                        values = string.Concat(values, "@available" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, eventModelList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, eventModelList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, eventModelList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, eventModelList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(eventModelList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EventModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EventModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
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

        public List<EventModel> Update(string connectionString, List<EventModel> eventModelList, List<EventModel> eventModelWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
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
                        for (int i = 0; i < eventModelList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE events SET ";
                                if (!string.IsNullOrEmpty(eventModelList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, eventModelList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Title))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `title` = @title" + i);
                                    else
                                        sql = string.Concat(sql, " `title` = @title" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@title" + i, eventModelList[i].Title);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Description))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `description` = @description" + i);
                                    else
                                        sql = string.Concat(sql, " `description` = @description" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@description" + i, eventModelList[i].Description);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Location))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `location` = @location" + i);
                                    else
                                        sql = string.Concat(sql, " `location` = @location" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@location" + i, eventModelList[i].Location);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].StartDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `start_date` = @startDate" + i);
                                    else
                                        sql = string.Concat(sql, " `start_date` = @startDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@startDate" + i, eventModelList[i].StartDate);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].EndDate))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `end_date` = @endDate" + i);
                                    else
                                        sql = string.Concat(sql, " `end_date` = @endDate" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@endDate" + i, eventModelList[i].EndDate);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].TimeZoneCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `timezone_code` = @timezoneCode" + i);
                                    else
                                        sql = string.Concat(sql, " `timezone_code` = @timezoneCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@timezoneCode" + i, eventModelList[i].TimeZoneCode);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].TeamsEventId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `teams_event_id` = @teamsEventId" + i);
                                    else
                                        sql = string.Concat(sql, " `teams_event_id` = @teamsEventId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teamsEventId" + i, eventModelList[i].TeamsEventId);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].TeamsSyncStatus))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `teams_sync_status` = @teamsSyncStatus" + i);
                                    else
                                        sql = string.Concat(sql, " `teams_sync_status` = @teamsSyncStatus" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@teamsSyncStatus" + i, eventModelList[i].TeamsSyncStatus);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].Available))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `available` = @available" + i);
                                    else
                                        sql = string.Concat(sql, " `available` = @available" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@available" + i, eventModelList[i].Available);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, eventModelList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, eventModelList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(eventModelList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, eventModelList[i].UpdatedAt);
                                }
                                if (eventModelWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, eventModelWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].Title))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `title` = @titleWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `title` = @titleWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@titleWhere" + i, eventModelWhereList[i].Title);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @descriptionWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @descriptionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descriptionWhere" + i, eventModelWhereList[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].Location))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `location` = @locationWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `location` = @locationWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@locationWhere" + i, eventModelWhereList[i].Location);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].StartDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `start_date` = @startDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `start_date` = @startDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@startDateWhere" + i, eventModelWhereList[i].StartDate);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].EndDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `end_date` = @endDateWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `end_date` = @endDateWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@endDateWhere" + i, eventModelWhereList[i].EndDate);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].TimeZoneCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `timezone_code` = @timezoneCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `timezone_code` = @timezoneCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@timezoneCodeWhere" + i, eventModelWhereList[i].TimeZoneCode);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].TeamsEventId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `teams_event_id` = @teamsEventIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `teams_event_id` = @teamsEventIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teamsEventIdWhere" + i, eventModelWhereList[i].TeamsEventId);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].TeamsSyncStatus))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `teams_sync_status` = @teamsSyncStatusWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `teams_sync_status` = @teamsSyncStatusWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teamsSyncStatusWhere" + i, eventModelWhereList[i].TeamsSyncStatus);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @availableWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @availableWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@availableWhere" + i, eventModelWhereList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, eventModelWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, eventModelWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, eventModelWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(eventModelList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EventModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EventModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
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

        public List<EventModel> Delete(string connectionString, List<EventModel> eventModelList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventModel> response = new List<EventModel>();
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
                        for (int i = 0; i < eventModelList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM events  ";
                                if (eventModelList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(eventModelList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, eventModelList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].Title))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `title` = @title" + i);
                                        else
                                            sql = string.Concat(sql, " `title` = @title" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@title" + i, eventModelList[i].Title);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].Description))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `description` = @description" + i);
                                        else
                                            sql = string.Concat(sql, " `description` = @description" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@description" + i, eventModelList[i].Description);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].Location))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `location` = @location" + i);
                                        else
                                            sql = string.Concat(sql, " `location` = @location" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@location" + i, eventModelList[i].Location);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].StartDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `start_date` = @startDate" + i);
                                        else
                                            sql = string.Concat(sql, " `start_date` = @startDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@startDate" + i, eventModelList[i].StartDate);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].EndDate))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `end_date` = @endDate" + i);
                                        else
                                            sql = string.Concat(sql, " `end_date` = @endDate" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@endDate" + i, eventModelList[i].EndDate);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].TimeZoneCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `timezone_code` = @timezoneCode" + i);
                                        else
                                            sql = string.Concat(sql, " `timezone_code` = @timezoneCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@timezoneCode" + i, eventModelList[i].TimeZoneCode);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].TeamsEventId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `teams_event_id` = @teamsEventId" + i);
                                        else
                                            sql = string.Concat(sql, " `teams_event_id` = @teamsEventId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teamsEventId" + i, eventModelList[i].TeamsEventId);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].TeamsSyncStatus))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `teams_sync_status` = @teamsSyncStatus" + i);
                                        else
                                            sql = string.Concat(sql, " `teams_sync_status` = @teamsSyncStatus" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@teamsSyncStatus" + i, eventModelList[i].TeamsSyncStatus);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].Available))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `available` = @available" + i);
                                        else
                                            sql = string.Concat(sql, " `available` = @available" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@available" + i, eventModelList[i].Available);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, eventModelList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, eventModelList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(eventModelList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, eventModelList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(eventModelList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EventModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EventModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventModel>();
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

        /// <summary>
        /// Replaces all of an event's relations (attachments/roles/schools/regions/users) with the
        /// given lists — delete-then-recreate per category, not a diff. Called on every create and
        /// update, so callers must always pass the full desired set, not just the changed rows.
        /// </summary>
        public void UpsertEventRelations(string connectionString, string eventId, List<EventAttachmentModel> attachments, List<EventRoleModel> roles, List<EventSchoolModel> schools, List<EventRegionModel> regions, List<EventUserModel> users)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();

                DeleteAttachments(_connection, eventId);
                CreateAttachments(_connection, eventId, attachments);

                DeleteRoles(_connection, eventId);
                CreateRoles(_connection, eventId, roles);

                DeleteSchools(_connection, eventId);
                CreateSchools(_connection, eventId, schools);

                DeleteRegions(_connection, eventId);
                CreateRegions(_connection, eventId, regions);

                DeleteUsers(_connection, eventId);
                CreateUsers(_connection, eventId, users);
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
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
        }

        /// <summary>
        /// Returns all attachments for a single event.
        /// </summary>
        public List<EventAttachmentModel> ReadAttachments(string connectionString, string eventId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventAttachmentModel> response = new List<EventAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `event_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at` FROM event_attachments WHERE `event_id` = @eventId";
                cmd.Parameters.AddWithValue("@eventId", eventId);
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            EventAttachmentModel row = new EventAttachmentModel();
                            if (!reader.IsDBNull(0))
                                row.Id = reader.GetValue(0).ToString();
                            else
                                row.Id = null;
                            if (!reader.IsDBNull(1))
                                row.EventId = reader.GetValue(1).ToString();
                            else
                                row.EventId = null;
                            if (!reader.IsDBNull(2))
                                row.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                row.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                row.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                row.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                row.CreatedAt = reader.GetValue(4).ToString();
                            else
                                row.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                row.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                row.UpdatedAt = null;
                            response.Add(row);
                            row = new EventAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EventAttachmentModel>();
                    reader.Close();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        /// <summary>
        /// Returns all role criteria (event_roles) for a single event.
        /// </summary>
        public List<EventRoleModel> ReadRoles(string connectionString, string eventId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventRoleModel> response = new List<EventRoleModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `event_id`, `role_id`, `modified_by`, `created_at`, `updated_at` FROM event_roles WHERE `event_id` = @eventId";
                cmd.Parameters.AddWithValue("@eventId", eventId);
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            EventRoleModel row = new EventRoleModel();
                            if (!reader.IsDBNull(0))
                                row.Id = reader.GetValue(0).ToString();
                            else
                                row.Id = null;
                            if (!reader.IsDBNull(1))
                                row.EventId = reader.GetValue(1).ToString();
                            else
                                row.EventId = null;
                            if (!reader.IsDBNull(2))
                                row.RoleId = reader.GetValue(2).ToString();
                            else
                                row.RoleId = null;
                            if (!reader.IsDBNull(3))
                                row.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                row.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                row.CreatedAt = reader.GetValue(4).ToString();
                            else
                                row.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                row.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                row.UpdatedAt = null;
                            response.Add(row);
                            row = new EventRoleModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EventRoleModel>();
                    reader.Close();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventRoleModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        /// <summary>
        /// Returns all school criteria (event_schools) for a single event.
        /// </summary>
        public List<EventSchoolModel> ReadSchools(string connectionString, string eventId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventSchoolModel> response = new List<EventSchoolModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `event_id`, `user_school_record`, `modified_by`, `created_at`, `updated_at` FROM event_schools WHERE `event_id` = @eventId";
                cmd.Parameters.AddWithValue("@eventId", eventId);
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            EventSchoolModel row = new EventSchoolModel();
                            if (!reader.IsDBNull(0))
                                row.Id = reader.GetValue(0).ToString();
                            else
                                row.Id = null;
                            if (!reader.IsDBNull(1))
                                row.EventId = reader.GetValue(1).ToString();
                            else
                                row.EventId = null;
                            if (!reader.IsDBNull(2))
                                row.SchoolIdentifier = reader.GetValue(2).ToString();
                            else
                                row.SchoolIdentifier = null;
                            if (!reader.IsDBNull(3))
                                row.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                row.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                row.CreatedAt = reader.GetValue(4).ToString();
                            else
                                row.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                row.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                row.UpdatedAt = null;
                            response.Add(row);
                            row = new EventSchoolModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EventSchoolModel>();
                    reader.Close();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventSchoolModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        /// <summary>
        /// Returns all region criteria (event_regions) for a single event.
        /// </summary>
        public List<EventRegionModel> ReadRegions(string connectionString, string eventId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventRegionModel> response = new List<EventRegionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `event_id`, `region_id`, `modified_by`, `created_at`, `updated_at` FROM event_regions WHERE `event_id` = @eventId";
                cmd.Parameters.AddWithValue("@eventId", eventId);
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            EventRegionModel row = new EventRegionModel();
                            if (!reader.IsDBNull(0))
                                row.Id = reader.GetValue(0).ToString();
                            else
                                row.Id = null;
                            if (!reader.IsDBNull(1))
                                row.EventId = reader.GetValue(1).ToString();
                            else
                                row.EventId = null;
                            if (!reader.IsDBNull(2))
                                row.RegionId = reader.GetValue(2).ToString();
                            else
                                row.RegionId = null;
                            if (!reader.IsDBNull(3))
                                row.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                row.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                row.CreatedAt = reader.GetValue(4).ToString();
                            else
                                row.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                row.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                row.UpdatedAt = null;
                            response.Add(row);
                            row = new EventRegionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EventRegionModel>();
                    reader.Close();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventRegionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        /// <summary>
        /// Returns all specific-user criteria (event_users) for a single event.
        /// </summary>
        public List<EventUserModel> ReadUsers(string connectionString, string eventId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EventUserModel> response = new List<EventUserModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `event_id`, `user_id`, `modified_by`, `created_at`, `updated_at` FROM event_users WHERE `event_id` = @eventId";
                cmd.Parameters.AddWithValue("@eventId", eventId);
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            EventUserModel row = new EventUserModel();
                            if (!reader.IsDBNull(0))
                                row.Id = reader.GetValue(0).ToString();
                            else
                                row.Id = null;
                            if (!reader.IsDBNull(1))
                                row.EventId = reader.GetValue(1).ToString();
                            else
                                row.EventId = null;
                            if (!reader.IsDBNull(2))
                                row.UserId = reader.GetValue(2).ToString();
                            else
                                row.UserId = null;
                            if (!reader.IsDBNull(3))
                                row.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                row.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                row.CreatedAt = reader.GetValue(4).ToString();
                            else
                                row.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                row.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                row.UpdatedAt = null;
                            response.Add(row);
                            row = new EventUserModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EventUserModel>();
                    reader.Close();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EventUserModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        /// <summary>
        /// Returns, for each event id, the set of role_id values found in event_roles.
        /// Aggregate query — one call per page, never one call per event.
        /// </summary>
        public Dictionary<string, HashSet<string>> ReadKnownRoleMembership(string connectionString, List<string> eventIds)
        {
            Dictionary<string, HashSet<string>> response = new Dictionary<string, HashSet<string>>();

            if (eventIds == null || eventIds.Count == 0)
            {
                return response;
            }

            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };

                List<string> paramNames = new List<string>();
                for (int i = 0; i < eventIds.Count; i++)
                {
                    string paramName = "@eventId" + i;
                    paramNames.Add(paramName);
                    cmd.Parameters.AddWithValue(paramName, eventIds[i]);
                }

                string sql = "SELECT `event_id`, `role_id` FROM event_roles WHERE `event_id` IN (" + string.Join(",", paramNames) + ")";
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            string eventId = reader.GetValue(0).ToString();
                            string roleId = reader.GetValue(1).ToString();
                            if (!response.ContainsKey(eventId))
                            {
                                response[eventId] = new HashSet<string>();
                            }
                            response[eventId].Add(roleId);
                        }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, HashSet<string>>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        /// <summary>
        /// Returns, for each event id, the set of region_id values found in event_regions.
        /// Aggregate query used to apply an Admin ORE's regional scope before pagination.
        /// </summary>
        public Dictionary<string, HashSet<string>> ReadKnownRegionMembership(string connectionString, List<string> eventIds)
        {
            Dictionary<string, HashSet<string>> response = new Dictionary<string, HashSet<string>>();

            if (eventIds == null || eventIds.Count == 0)
            {
                return response;
            }

            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };

                List<string> paramNames = new List<string>();
                for (int i = 0; i < eventIds.Count; i++)
                {
                    string paramName = "@eventId" + i;
                    paramNames.Add(paramName);
                    cmd.Parameters.AddWithValue(paramName, eventIds[i]);
                }

                string sql = "SELECT `event_id`, `region_id` FROM event_regions WHERE `event_id` IN (" + string.Join(",", paramNames) + ")";
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string eventId = reader.GetValue(0).ToString();
                            string regionId = reader.GetValue(1).ToString();
                            if (!response.ContainsKey(eventId))
                            {
                                response[eventId] = new HashSet<string>();
                            }
                            response[eventId].Add(regionId);
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, HashSet<string>>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        /// <summary>
        /// Returns the subset of eventIds that have at least one row in event_schools.
        /// </summary>
        public HashSet<string> ReadEventIdsWithSchools(string connectionString, List<string> eventIds)
        {
            return ReadDistinctEventIds(connectionString, "event_schools", eventIds);
        }

        /// <summary>
        /// Returns the subset of eventIds that have at least one row in event_regions.
        /// </summary>
        public HashSet<string> ReadEventIdsWithRegions(string connectionString, List<string> eventIds)
        {
            return ReadDistinctEventIds(connectionString, "event_regions", eventIds);
        }

        /// <summary>
        /// Returns the subset of eventIds that have at least one row in event_users.
        /// </summary>
        public HashSet<string> ReadEventIdsWithUsers(string connectionString, List<string> eventIds)
        {
            return ReadDistinctEventIds(connectionString, "event_users", eventIds);
        }

        /// <summary>
        /// Shared implementation for ReadEventIdsWith*. "tableName" is always one of the three
        /// fixed literals passed by the wrappers above — never external input — so string
        /// concatenation for the table name is safe here (MySQL cannot parameterize identifiers).
        /// </summary>
        private HashSet<string> ReadDistinctEventIds(string connectionString, string tableName, List<string> eventIds)
        {
            HashSet<string> response = new HashSet<string>();

            if (eventIds == null || eventIds.Count == 0)
            {
                return response;
            }

            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };

                List<string> paramNames = new List<string>();
                for (int i = 0; i < eventIds.Count; i++)
                {
                    string paramName = "@eventId" + i;
                    paramNames.Add(paramName);
                    cmd.Parameters.AddWithValue(paramName, eventIds[i]);
                }

                string sql = "SELECT DISTINCT `event_id` FROM " + tableName + " WHERE `event_id` IN (" + string.Join(",", paramNames) + ")";
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            response.Add(reader.GetValue(0).ToString());
                        }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new HashSet<string>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        private void CreateAttachments(MySqlConnection connection, string eventId, List<EventAttachmentModel> attachments)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (MySqlTransaction trans = connection.BeginTransaction())
            {
                try
                {
                    foreach (EventAttachmentModel attachment in attachments)
                    {
                        string insert = "INSERT INTO event_attachments (id, event_id, attachment_url, modified_by) VALUES (UUID(), @eventId, @attachmentUrl, @modifiedBy);";

                        parameters.Add("@eventId", eventId);
                        parameters.Add("@attachmentUrl", attachment.AttachmentUrl);
                        parameters.Add("@modifiedBy", attachment.ModifiedBy);

                        connection.Execute(insert, parameters);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void DeleteAttachments(MySqlConnection connection, string eventId)
        {
            MySqlCommand cmd = new MySqlCommand { Connection = connection };
            using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
            {
                try
                {
                    string sql = "DELETE FROM event_attachments WHERE `event_id` = @eventId;";
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void CreateRoles(MySqlConnection connection, string eventId, List<EventRoleModel> roles)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (MySqlTransaction trans = connection.BeginTransaction())
            {
                try
                {
                    foreach (EventRoleModel role in roles)
                    {
                        string insert = "INSERT INTO event_roles (id, event_id, role_id, modified_by) VALUES (UUID(), @eventId, @roleId, @modifiedBy);";

                        parameters.Add("@eventId", eventId);
                        parameters.Add("@roleId", role.RoleId);
                        parameters.Add("@modifiedBy", role.ModifiedBy);

                        connection.Execute(insert, parameters);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void DeleteRoles(MySqlConnection connection, string eventId)
        {
            MySqlCommand cmd = new MySqlCommand { Connection = connection };
            using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
            {
                try
                {
                    string sql = "DELETE FROM event_roles WHERE `event_id` = @eventId;";
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void CreateSchools(MySqlConnection connection, string eventId, List<EventSchoolModel> schools)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (MySqlTransaction trans = connection.BeginTransaction())
            {
                try
                {
                    foreach (EventSchoolModel school in schools)
                    {
                        string insert = "INSERT INTO event_schools (id, event_id, user_school_record, modified_by) VALUES (UUID(), @eventId, @schoolIdentifier, @modifiedBy);";

                        parameters.Add("@eventId", eventId);
                        parameters.Add("@schoolIdentifier", school.SchoolIdentifier);
                        parameters.Add("@modifiedBy", school.ModifiedBy);

                        connection.Execute(insert, parameters);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void DeleteSchools(MySqlConnection connection, string eventId)
        {
            MySqlCommand cmd = new MySqlCommand { Connection = connection };
            using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
            {
                try
                {
                    string sql = "DELETE FROM event_schools WHERE `event_id` = @eventId;";
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void CreateRegions(MySqlConnection connection, string eventId, List<EventRegionModel> regions)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (MySqlTransaction trans = connection.BeginTransaction())
            {
                try
                {
                    foreach (EventRegionModel region in regions)
                    {
                        string insert = "INSERT INTO event_regions (id, event_id, region_id, modified_by) VALUES (UUID(), @eventId, @regionId, @modifiedBy);";

                        parameters.Add("@eventId", eventId);
                        parameters.Add("@regionId", region.RegionId);
                        parameters.Add("@modifiedBy", region.ModifiedBy);

                        connection.Execute(insert, parameters);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void DeleteRegions(MySqlConnection connection, string eventId)
        {
            MySqlCommand cmd = new MySqlCommand { Connection = connection };
            using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
            {
                try
                {
                    string sql = "DELETE FROM event_regions WHERE `event_id` = @eventId;";
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void CreateUsers(MySqlConnection connection, string eventId, List<EventUserModel> users)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (MySqlTransaction trans = connection.BeginTransaction())
            {
                try
                {
                    foreach (EventUserModel user in users)
                    {
                        string insert = "INSERT INTO event_users (id, event_id, user_id, modified_by) VALUES (UUID(), @eventId, @userId, @modifiedBy);";

                        parameters.Add("@eventId", eventId);
                        parameters.Add("@userId", user.UserId);
                        parameters.Add("@modifiedBy", user.ModifiedBy);

                        connection.Execute(insert, parameters);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }

        private void DeleteUsers(MySqlConnection connection, string eventId)
        {
            MySqlCommand cmd = new MySqlCommand { Connection = connection };
            using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
            {
                try
                {
                    string sql = "DELETE FROM event_users WHERE `event_id` = @eventId;";
                    cmd.Parameters.AddWithValue("@eventId", eventId);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
            }
        }
    }
}
