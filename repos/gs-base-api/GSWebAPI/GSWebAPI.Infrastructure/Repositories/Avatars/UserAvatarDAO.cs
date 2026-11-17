using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.Avatar;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.Avatars
{
    class UserAvatarDAO
    {
        private MySqlConnection _connection;
        public List<UserAvatarModel> Read(string connectionString, UserAvatarModel userAvatar, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `base`, `base_hex`, `sex`, `back_accesory`, `eyebrows`, `eyes`, `eyes_hex`, `face_paint`, `glasses`, `hair`, `hair_hex`, `hat`, `jacket`, `mask`, `mouth`, `nose`, `pants`, `shirt`, `shoes`, `socks`, `modified_by`, `created_at`, `updated_at`  FROM av_user_avatars ";
                {
                    if (userAvatar != null)
                    {
                        if (!string.IsNullOrEmpty(userAvatar.Id))
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
                            cmd.Parameters.AddWithValue("@id", userAvatar.Id);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userAvatar.UserId);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Base))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `base` = @base ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `base` = @base ");
                            }
                            cmd.Parameters.AddWithValue("@base", userAvatar.Base);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.BaseHex))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `base_hex` = @baseHex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `base_hex` = @baseHex ");
                            }
                            cmd.Parameters.AddWithValue("@baseHex", userAvatar.BaseHex);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Sex))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `sex` = @sex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `sex` = @sex ");
                            }
                            cmd.Parameters.AddWithValue("@sex", userAvatar.Sex);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.BackAccesory))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `back_accesory` = @backAccesory ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `back_accesory` = @backAccesory ");
                            }
                            cmd.Parameters.AddWithValue("@backAccesory", userAvatar.BackAccesory);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Eyebrows))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `eyebrows` = @eyebrows ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `eyebrows` = @eyebrows ");
                            }
                            cmd.Parameters.AddWithValue("@eyebrows", userAvatar.Eyebrows);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Eyes))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `eyes` = @eyes ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `eyes` = @eyes ");
                            }
                            cmd.Parameters.AddWithValue("@eyes", userAvatar.Eyes);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.EyesHex))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `eyes_hex` = @eyesHex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `eyes_hex` = @eyesHex ");
                            }
                            cmd.Parameters.AddWithValue("@eyesHex", userAvatar.EyesHex);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.FacePaint))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `face_paint` = @facePaint ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `face_paint` = @facePaint ");
                            }
                            cmd.Parameters.AddWithValue("@facePaint", userAvatar.FacePaint);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Glasses))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `glasses` = @glasses ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `glasses` = @glasses ");
                            }
                            cmd.Parameters.AddWithValue("@glasses", userAvatar.Glasses);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Hair))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `hair` = @hair ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `hair` = @hair ");
                            }
                            cmd.Parameters.AddWithValue("@hair", userAvatar.Hair);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.HairHex))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `hair_hex` = @hairHex ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `hair_hex` = @hairHex ");
                            }
                            cmd.Parameters.AddWithValue("@hairHex", userAvatar.HairHex);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Hat))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `hat` = @hat ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `hat` = @hat ");
                            }
                            cmd.Parameters.AddWithValue("@hat", userAvatar.Hat);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Jacket))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `jacket` = @jacket ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `jacket` = @jacket ");
                            }
                            cmd.Parameters.AddWithValue("@jacket", userAvatar.Jacket);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Mask))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `mask` = @mask ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `mask` = @mask ");
                            }
                            cmd.Parameters.AddWithValue("@mask", userAvatar.Mask);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Mouth))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `mouth` = @mouth ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `mouth` = @mouth ");
                            }
                            cmd.Parameters.AddWithValue("@mouth", userAvatar.Mouth);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Nose))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `nose` = @nose ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `nose` = @nose ");
                            }
                            cmd.Parameters.AddWithValue("@nose", userAvatar.Nose);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Pants))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `pants` = @pants ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `pants` = @pants ");
                            }
                            cmd.Parameters.AddWithValue("@pants", userAvatar.Pants);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Shirt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `shirt` = @shirt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `shirt` = @shirt ");
                            }
                            cmd.Parameters.AddWithValue("@shirt", userAvatar.Shirt);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Shoes))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `shoes` = @shoes ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `shoes` = @shoes ");
                            }
                            cmd.Parameters.AddWithValue("@shoes", userAvatar.Shoes);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.Socks))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `socks` = @socks ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `socks` = @socks ");
                            }
                            cmd.Parameters.AddWithValue("@socks", userAvatar.Socks);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userAvatar.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userAvatar.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userAvatar.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userAvatar.UpdatedAt);
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
                                    userAvatar.Id = Reader.GetValue(0).ToString();
                                else
                                    userAvatar.Id = null;
                                if (!Reader.IsDBNull(1))
                                    userAvatar.UserId = Reader.GetValue(1).ToString();
                                else
                                    userAvatar.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    userAvatar.Base = Reader.GetValue(2).ToString();
                                else
                                    userAvatar.Base = null;
                                if (!Reader.IsDBNull(3))
                                    userAvatar.BaseHex = Reader.GetValue(3).ToString();
                                else
                                    userAvatar.BaseHex = null;
                                if (!Reader.IsDBNull(4))
                                    userAvatar.Sex = Reader.GetValue(4).ToString();
                                else
                                    userAvatar.Sex = null;
                                if (!Reader.IsDBNull(5))
                                    userAvatar.BackAccesory = Reader.GetValue(5).ToString();
                                else
                                    userAvatar.BackAccesory = null;
                                if (!Reader.IsDBNull(6))
                                    userAvatar.Eyebrows = Reader.GetValue(6).ToString();
                                else
                                    userAvatar.Eyebrows = null;
                                if (!Reader.IsDBNull(7))
                                    userAvatar.Eyes = Reader.GetValue(7).ToString();
                                else
                                    userAvatar.Eyes = null;
                                if (!Reader.IsDBNull(8))
                                    userAvatar.EyesHex = Reader.GetValue(8).ToString();
                                else
                                    userAvatar.EyesHex = null;
                                if (!Reader.IsDBNull(9))
                                    userAvatar.FacePaint = Reader.GetValue(9).ToString();
                                else
                                    userAvatar.FacePaint = null;
                                if (!Reader.IsDBNull(10))
                                    userAvatar.Glasses = Reader.GetValue(10).ToString();
                                else
                                    userAvatar.Glasses = null;
                                if (!Reader.IsDBNull(11))
                                    userAvatar.Hair = Reader.GetValue(11).ToString();
                                else
                                    userAvatar.Hair = null;
                                if (!Reader.IsDBNull(12))
                                    userAvatar.HairHex = Reader.GetValue(12).ToString();
                                else
                                    userAvatar.HairHex = null;
                                if (!Reader.IsDBNull(13))
                                    userAvatar.Hat = Reader.GetValue(13).ToString();
                                else
                                    userAvatar.Hat = null;
                                if (!Reader.IsDBNull(14))
                                    userAvatar.Jacket = Reader.GetValue(14).ToString();
                                else
                                    userAvatar.Jacket = null;
                                if (!Reader.IsDBNull(15))
                                    userAvatar.Mask = Reader.GetValue(15).ToString();
                                else
                                    userAvatar.Mask = null;
                                if (!Reader.IsDBNull(16))
                                    userAvatar.Mouth = Reader.GetValue(16).ToString();
                                else
                                    userAvatar.Mouth = null;
                                if (!Reader.IsDBNull(17))
                                    userAvatar.Nose = Reader.GetValue(17).ToString();
                                else
                                    userAvatar.Nose = null;
                                if (!Reader.IsDBNull(18))
                                    userAvatar.Pants = Reader.GetValue(18).ToString();
                                else
                                    userAvatar.Pants = null;
                                if (!Reader.IsDBNull(19))
                                    userAvatar.Shirt = Reader.GetValue(19).ToString();
                                else
                                    userAvatar.Shirt = null;
                                if (!Reader.IsDBNull(20))
                                    userAvatar.Shoes = Reader.GetValue(20).ToString();
                                else
                                    userAvatar.Shoes = null;
                                if (!Reader.IsDBNull(21))
                                    userAvatar.Socks = Reader.GetValue(21).ToString();
                                else
                                    userAvatar.Socks = null;
                                if (!Reader.IsDBNull(22))
                                    userAvatar.ModifiedBy = Reader.GetValue(22).ToString();
                                else
                                    userAvatar.ModifiedBy = null;
                                if (!Reader.IsDBNull(23))
                                    userAvatar.CreatedAt = Reader.GetValue(23).ToString();
                                else
                                    userAvatar.CreatedAt = null;
                                if (!Reader.IsDBNull(24))
                                    userAvatar.UpdatedAt = Reader.GetValue(24).ToString();
                                else
                                    userAvatar.UpdatedAt = null;
                                response.Add(userAvatar);
                                userAvatar = new UserAvatarModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserAvatarModel>();
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
                response = new List<UserAvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserAvatarModel> Read(string connectionString, List<UserAvatarModel> userAvatarWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `base`, `base_hex`, `sex`, `back_accesory`, `eyebrows`, `eyes`, `eyes_hex`, `face_paint`, `glasses`, `hair`, `hair_hex`, `hat`, `jacket`, `mask`, `mouth`, `nose`, `pants`, `shirt`, `shoes`, `socks`, `modified_by`, `created_at`, `updated_at`  FROM av_user_avatars ";
                if (userAvatarWhere != null && userAvatarWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userAvatarWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userAvatarWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userAvatarWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Base))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `base` = @base" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@base" + i, userAvatarWhere[i].Base);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].BaseHex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `base_hex` = @baseHex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@baseHex" + i, userAvatarWhere[i].BaseHex);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Sex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `sex` = @sex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@sex" + i, userAvatarWhere[i].Sex);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].BackAccesory))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `back_accesory` = @backAccesory" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@backAccesory" + i, userAvatarWhere[i].BackAccesory);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Eyebrows))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `eyebrows` = @eyebrows" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@eyebrows" + i, userAvatarWhere[i].Eyebrows);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Eyes))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `eyes` = @eyes" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@eyes" + i, userAvatarWhere[i].Eyes);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].EyesHex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `eyes_hex` = @eyesHex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@eyesHex" + i, userAvatarWhere[i].EyesHex);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].FacePaint))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `face_paint` = @facePaint" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@facePaint" + i, userAvatarWhere[i].FacePaint);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Glasses))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `glasses` = @glasses" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@glasses" + i, userAvatarWhere[i].Glasses);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Hair))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `hair` = @hair" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@hair" + i, userAvatarWhere[i].Hair);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].HairHex))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `hair_hex` = @hairHex" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@hairHex" + i, userAvatarWhere[i].HairHex);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Hat))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `hat` = @hat" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@hat" + i, userAvatarWhere[i].Hat);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Jacket))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `jacket` = @jacket" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@jacket" + i, userAvatarWhere[i].Jacket);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Mask))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `mask` = @mask" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mask" + i, userAvatarWhere[i].Mask);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Mouth))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `mouth` = @mouth" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@mouth" + i, userAvatarWhere[i].Mouth);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Nose))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `nose` = @nose" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@nose" + i, userAvatarWhere[i].Nose);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Pants))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `pants` = @pants" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@pants" + i, userAvatarWhere[i].Pants);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Shirt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `shirt` = @shirt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@shirt" + i, userAvatarWhere[i].Shirt);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Shoes))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `shoes` = @shoes" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@shoes" + i, userAvatarWhere[i].Shoes);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].Socks))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `socks` = @socks" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@socks" + i, userAvatarWhere[i].Socks);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userAvatarWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userAvatarWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userAvatarWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userAvatarWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userAvatarWhere.Count - 1)
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
                            UserAvatarModel userAvatar = new UserAvatarModel(); if (!Reader.IsDBNull(0))
                                userAvatar.Id = Reader.GetValue(0).ToString();
                            else
                                userAvatar.Id = null;
                            if (!Reader.IsDBNull(1))
                                userAvatar.UserId = Reader.GetValue(1).ToString();
                            else
                                userAvatar.UserId = null;
                            if (!Reader.IsDBNull(2))
                                userAvatar.Base = Reader.GetValue(2).ToString();
                            else
                                userAvatar.Base = null;
                            if (!Reader.IsDBNull(3))
                                userAvatar.BaseHex = Reader.GetValue(3).ToString();
                            else
                                userAvatar.BaseHex = null;
                            if (!Reader.IsDBNull(4))
                                userAvatar.Sex = Reader.GetValue(4).ToString();
                            else
                                userAvatar.Sex = null;
                            if (!Reader.IsDBNull(5))
                                userAvatar.BackAccesory = Reader.GetValue(5).ToString();
                            else
                                userAvatar.BackAccesory = null;
                            if (!Reader.IsDBNull(6))
                                userAvatar.Eyebrows = Reader.GetValue(6).ToString();
                            else
                                userAvatar.Eyebrows = null;
                            if (!Reader.IsDBNull(7))
                                userAvatar.Eyes = Reader.GetValue(7).ToString();
                            else
                                userAvatar.Eyes = null;
                            if (!Reader.IsDBNull(8))
                                userAvatar.EyesHex = Reader.GetValue(8).ToString();
                            else
                                userAvatar.EyesHex = null;
                            if (!Reader.IsDBNull(9))
                                userAvatar.FacePaint = Reader.GetValue(9).ToString();
                            else
                                userAvatar.FacePaint = null;
                            if (!Reader.IsDBNull(10))
                                userAvatar.Glasses = Reader.GetValue(10).ToString();
                            else
                                userAvatar.Glasses = null;
                            if (!Reader.IsDBNull(11))
                                userAvatar.Hair = Reader.GetValue(11).ToString();
                            else
                                userAvatar.Hair = null;
                            if (!Reader.IsDBNull(12))
                                userAvatar.HairHex = Reader.GetValue(12).ToString();
                            else
                                userAvatar.HairHex = null;
                            if (!Reader.IsDBNull(13))
                                userAvatar.Hat = Reader.GetValue(13).ToString();
                            else
                                userAvatar.Hat = null;
                            if (!Reader.IsDBNull(14))
                                userAvatar.Jacket = Reader.GetValue(14).ToString();
                            else
                                userAvatar.Jacket = null;
                            if (!Reader.IsDBNull(15))
                                userAvatar.Mask = Reader.GetValue(15).ToString();
                            else
                                userAvatar.Mask = null;
                            if (!Reader.IsDBNull(16))
                                userAvatar.Mouth = Reader.GetValue(16).ToString();
                            else
                                userAvatar.Mouth = null;
                            if (!Reader.IsDBNull(17))
                                userAvatar.Nose = Reader.GetValue(17).ToString();
                            else
                                userAvatar.Nose = null;
                            if (!Reader.IsDBNull(18))
                                userAvatar.Pants = Reader.GetValue(18).ToString();
                            else
                                userAvatar.Pants = null;
                            if (!Reader.IsDBNull(19))
                                userAvatar.Shirt = Reader.GetValue(19).ToString();
                            else
                                userAvatar.Shirt = null;
                            if (!Reader.IsDBNull(20))
                                userAvatar.Shoes = Reader.GetValue(20).ToString();
                            else
                                userAvatar.Shoes = null;
                            if (!Reader.IsDBNull(21))
                                userAvatar.Socks = Reader.GetValue(21).ToString();
                            else
                                userAvatar.Socks = null;
                            if (!Reader.IsDBNull(22))
                                userAvatar.ModifiedBy = Reader.GetValue(22).ToString();
                            else
                                userAvatar.ModifiedBy = null;
                            if (!Reader.IsDBNull(23))
                                userAvatar.CreatedAt = Reader.GetValue(23).ToString();
                            else
                                userAvatar.CreatedAt = null;
                            if (!Reader.IsDBNull(24))
                                userAvatar.UpdatedAt = Reader.GetValue(24).ToString();
                            else
                                userAvatar.UpdatedAt = null;
                            response.Add(userAvatar);
                            userAvatar = new UserAvatarModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserAvatarModel>();
                    Reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAvatarModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserAvatarModel> Create(string connectionString, UserAvatarModel userAvatar)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO av_user_avatars (";
                string values = "(";
                if (!string.IsNullOrEmpty(userAvatar.Id))
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
                    cmd.Parameters.AddWithValue("@id", userAvatar.Id);
                }
                if (!string.IsNullOrEmpty(userAvatar.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userAvatar.UserId);
                }
                if (!string.IsNullOrEmpty(userAvatar.Base))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `base` ");
                        values = string.Concat(values, ", @base");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `base` ");
                        values = string.Concat(values, " @base");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@base", userAvatar.Base);
                }
                if (!string.IsNullOrEmpty(userAvatar.BaseHex))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `base_hex` ");
                        values = string.Concat(values, ", @baseHex");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `base_hex` ");
                        values = string.Concat(values, " @baseHex");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@baseHex", userAvatar.BaseHex);
                }
                if (!string.IsNullOrEmpty(userAvatar.Sex))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `sex` ");
                        values = string.Concat(values, ", @sex");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `sex` ");
                        values = string.Concat(values, " @sex");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@sex", userAvatar.Sex);
                }
                if (!string.IsNullOrEmpty(userAvatar.BackAccesory))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `back_accesory` ");
                        values = string.Concat(values, ", @backAccesory");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `back_accesory` ");
                        values = string.Concat(values, " @backAccesory");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@backAccesory", userAvatar.BackAccesory);
                }
                if (!string.IsNullOrEmpty(userAvatar.Eyebrows))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `eyebrows` ");
                        values = string.Concat(values, ", @eyebrows");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `eyebrows` ");
                        values = string.Concat(values, " @eyebrows");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@eyebrows", userAvatar.Eyebrows);
                }
                if (!string.IsNullOrEmpty(userAvatar.Eyes))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `eyes` ");
                        values = string.Concat(values, ", @eyes");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `eyes` ");
                        values = string.Concat(values, " @eyes");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@eyes", userAvatar.Eyes);
                }
                if (!string.IsNullOrEmpty(userAvatar.EyesHex))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `eyes_hex` ");
                        values = string.Concat(values, ", @eyesHex");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `eyes_hex` ");
                        values = string.Concat(values, " @eyesHex");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@eyesHex", userAvatar.EyesHex);
                }
                if (!string.IsNullOrEmpty(userAvatar.FacePaint))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `face_paint` ");
                        values = string.Concat(values, ", @facePaint");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `face_paint` ");
                        values = string.Concat(values, " @facePaint");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@facePaint", userAvatar.FacePaint);
                }
                if (!string.IsNullOrEmpty(userAvatar.Glasses))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `glasses` ");
                        values = string.Concat(values, ", @glasses");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `glasses` ");
                        values = string.Concat(values, " @glasses");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@glasses", userAvatar.Glasses);
                }
                if (!string.IsNullOrEmpty(userAvatar.Hair))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `hair` ");
                        values = string.Concat(values, ", @hair");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `hair` ");
                        values = string.Concat(values, " @hair");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hair", userAvatar.Hair);
                }
                if (!string.IsNullOrEmpty(userAvatar.HairHex))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `hair_hex` ");
                        values = string.Concat(values, ", @hairHex");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `hair_hex` ");
                        values = string.Concat(values, " @hairHex");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hairHex", userAvatar.HairHex);
                }
                if (!string.IsNullOrEmpty(userAvatar.Hat))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `hat` ");
                        values = string.Concat(values, ", @hat");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `hat` ");
                        values = string.Concat(values, " @hat");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hat", userAvatar.Hat);
                }
                if (!string.IsNullOrEmpty(userAvatar.Jacket))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `jacket` ");
                        values = string.Concat(values, ", @jacket");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `jacket` ");
                        values = string.Concat(values, " @jacket");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@jacket", userAvatar.Jacket);
                }
                if (!string.IsNullOrEmpty(userAvatar.Mask))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `mask` ");
                        values = string.Concat(values, ", @mask");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `mask` ");
                        values = string.Concat(values, " @mask");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mask", userAvatar.Mask);
                }
                if (!string.IsNullOrEmpty(userAvatar.Mouth))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `mouth` ");
                        values = string.Concat(values, ", @mouth");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `mouth` ");
                        values = string.Concat(values, " @mouth");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mouth", userAvatar.Mouth);
                }
                if (!string.IsNullOrEmpty(userAvatar.Nose))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `nose` ");
                        values = string.Concat(values, ", @nose");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `nose` ");
                        values = string.Concat(values, " @nose");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@nose", userAvatar.Nose);
                }
                if (!string.IsNullOrEmpty(userAvatar.Pants))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `pants` ");
                        values = string.Concat(values, ", @pants");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `pants` ");
                        values = string.Concat(values, " @pants");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@pants", userAvatar.Pants);
                }
                if (!string.IsNullOrEmpty(userAvatar.Shirt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `shirt` ");
                        values = string.Concat(values, ", @shirt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `shirt` ");
                        values = string.Concat(values, " @shirt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@shirt", userAvatar.Shirt);
                }
                if (!string.IsNullOrEmpty(userAvatar.Shoes))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `shoes` ");
                        values = string.Concat(values, ", @shoes");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `shoes` ");
                        values = string.Concat(values, " @shoes");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@shoes", userAvatar.Shoes);
                }
                if (!string.IsNullOrEmpty(userAvatar.Socks))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `socks` ");
                        values = string.Concat(values, ", @socks");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `socks` ");
                        values = string.Concat(values, " @socks");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@socks", userAvatar.Socks);
                }
                if (!string.IsNullOrEmpty(userAvatar.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userAvatar.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userAvatar.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userAvatar.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userAvatar.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userAvatar.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userAvatar.Id = cmd.LastInsertedId.ToString();
                response.Add(userAvatar);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAvatarModel>();
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
        public List<UserAvatarModel> Create(string connectionString, List<UserAvatarModel> userAvatarList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userAvatarList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO av_user_avatars (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userAvatarList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userAvatarList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userAvatarList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Base))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `base` ");
                                        values = string.Concat(values, ", @base" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `base` ");
                                        values = string.Concat(values, "@base" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@base" + i, userAvatarList[i].Base);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].BaseHex))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `base_hex` ");
                                        values = string.Concat(values, ", @baseHex" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `base_hex` ");
                                        values = string.Concat(values, "@baseHex" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@baseHex" + i, userAvatarList[i].BaseHex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Sex))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `sex` ");
                                        values = string.Concat(values, ", @sex" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `sex` ");
                                        values = string.Concat(values, "@sex" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@sex" + i, userAvatarList[i].Sex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].BackAccesory))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `back_accesory` ");
                                        values = string.Concat(values, ", @backAccesory" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `back_accesory` ");
                                        values = string.Concat(values, "@backAccesory" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@backAccesory" + i, userAvatarList[i].BackAccesory);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Eyebrows))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `eyebrows` ");
                                        values = string.Concat(values, ", @eyebrows" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `eyebrows` ");
                                        values = string.Concat(values, "@eyebrows" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@eyebrows" + i, userAvatarList[i].Eyebrows);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Eyes))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `eyes` ");
                                        values = string.Concat(values, ", @eyes" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `eyes` ");
                                        values = string.Concat(values, "@eyes" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@eyes" + i, userAvatarList[i].Eyes);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].EyesHex))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `eyes_hex` ");
                                        values = string.Concat(values, ", @eyesHex" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `eyes_hex` ");
                                        values = string.Concat(values, "@eyesHex" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@eyesHex" + i, userAvatarList[i].EyesHex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].FacePaint))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `face_paint` ");
                                        values = string.Concat(values, ", @facePaint" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `face_paint` ");
                                        values = string.Concat(values, "@facePaint" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@facePaint" + i, userAvatarList[i].FacePaint);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Glasses))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `glasses` ");
                                        values = string.Concat(values, ", @glasses" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `glasses` ");
                                        values = string.Concat(values, "@glasses" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@glasses" + i, userAvatarList[i].Glasses);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Hair))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `hair` ");
                                        values = string.Concat(values, ", @hair" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `hair` ");
                                        values = string.Concat(values, "@hair" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hair" + i, userAvatarList[i].Hair);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].HairHex))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `hair_hex` ");
                                        values = string.Concat(values, ", @hairHex" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `hair_hex` ");
                                        values = string.Concat(values, "@hairHex" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hairHex" + i, userAvatarList[i].HairHex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Hat))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `hat` ");
                                        values = string.Concat(values, ", @hat" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `hat` ");
                                        values = string.Concat(values, "@hat" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hat" + i, userAvatarList[i].Hat);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Jacket))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `jacket` ");
                                        values = string.Concat(values, ", @jacket" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `jacket` ");
                                        values = string.Concat(values, "@jacket" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@jacket" + i, userAvatarList[i].Jacket);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Mask))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `mask` ");
                                        values = string.Concat(values, ", @mask" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `mask` ");
                                        values = string.Concat(values, "@mask" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mask" + i, userAvatarList[i].Mask);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Mouth))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `mouth` ");
                                        values = string.Concat(values, ", @mouth" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `mouth` ");
                                        values = string.Concat(values, "@mouth" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mouth" + i, userAvatarList[i].Mouth);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Nose))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `nose` ");
                                        values = string.Concat(values, ", @nose" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `nose` ");
                                        values = string.Concat(values, "@nose" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@nose" + i, userAvatarList[i].Nose);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Pants))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `pants` ");
                                        values = string.Concat(values, ", @pants" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `pants` ");
                                        values = string.Concat(values, "@pants" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@pants" + i, userAvatarList[i].Pants);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Shirt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `shirt` ");
                                        values = string.Concat(values, ", @shirt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `shirt` ");
                                        values = string.Concat(values, "@shirt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@shirt" + i, userAvatarList[i].Shirt);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Shoes))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `shoes` ");
                                        values = string.Concat(values, ", @shoes" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `shoes` ");
                                        values = string.Concat(values, "@shoes" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@shoes" + i, userAvatarList[i].Shoes);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Socks))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `socks` ");
                                        values = string.Concat(values, ", @socks" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `socks` ");
                                        values = string.Concat(values, "@socks" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@socks" + i, userAvatarList[i].Socks);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userAvatarList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userAvatarList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userAvatarList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userAvatarList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserAvatarModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserAvatarModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAvatarModel>();
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

        public List<UserAvatarModel> Update(string connectionString, UserAvatarModel userAvatar, UserAvatarModel userAvatarWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE av_user_avatars SET ";
                if (!string.IsNullOrEmpty(userAvatar.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userAvatar.Id);
                }
                if (!string.IsNullOrEmpty(userAvatar.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userAvatar.UserId);
                }
                if (!string.IsNullOrEmpty(userAvatar.Base))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `base` = @base");
                    else
                        sql = string.Concat(sql, " `base` = @base");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@base", userAvatar.Base);
                }
                if (!string.IsNullOrEmpty(userAvatar.BaseHex))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `base_hex` = @baseHex");
                    else
                        sql = string.Concat(sql, " `base_hex` = @baseHex");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@baseHex", userAvatar.BaseHex);
                }
                if (!string.IsNullOrEmpty(userAvatar.Sex))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `sex` = @sex");
                    else
                        sql = string.Concat(sql, " `sex` = @sex");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@sex", userAvatar.Sex);
                }
                if (!string.IsNullOrEmpty(userAvatar.BackAccesory))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `back_accesory` = @backAccesory");
                    else
                        sql = string.Concat(sql, " `back_accesory` = @backAccesory");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@backAccesory", userAvatar.BackAccesory);
                }
                if (!string.IsNullOrEmpty(userAvatar.Eyebrows))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `eyebrows` = @eyebrows");
                    else
                        sql = string.Concat(sql, " `eyebrows` = @eyebrows");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@eyebrows", userAvatar.Eyebrows);
                }
                if (!string.IsNullOrEmpty(userAvatar.Eyes))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `eyes` = @eyes");
                    else
                        sql = string.Concat(sql, " `eyes` = @eyes");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@eyes", userAvatar.Eyes);
                }
                if (!string.IsNullOrEmpty(userAvatar.EyesHex))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `eyes_hex` = @eyesHex");
                    else
                        sql = string.Concat(sql, " `eyes_hex` = @eyesHex");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@eyesHex", userAvatar.EyesHex);
                }
                if (!string.IsNullOrEmpty(userAvatar.FacePaint))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `face_paint` = @facePaint");
                    else
                        sql = string.Concat(sql, " `face_paint` = @facePaint");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@facePaint", userAvatar.FacePaint);
                }
                if (!string.IsNullOrEmpty(userAvatar.Glasses))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `glasses` = @glasses");
                    else
                        sql = string.Concat(sql, " `glasses` = @glasses");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@glasses", userAvatar.Glasses);
                }
                if (!string.IsNullOrEmpty(userAvatar.Hair))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `hair` = @hair");
                    else
                        sql = string.Concat(sql, " `hair` = @hair");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hair", userAvatar.Hair);
                }
                if (!string.IsNullOrEmpty(userAvatar.HairHex))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `hair_hex` = @hairHex");
                    else
                        sql = string.Concat(sql, " `hair_hex` = @hairHex");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hairHex", userAvatar.HairHex);
                }
                if (!string.IsNullOrEmpty(userAvatar.Hat))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `hat` = @hat");
                    else
                        sql = string.Concat(sql, " `hat` = @hat");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@hat", userAvatar.Hat);
                }
                if (!string.IsNullOrEmpty(userAvatar.Jacket))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `jacket` = @jacket");
                    else
                        sql = string.Concat(sql, " `jacket` = @jacket");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@jacket", userAvatar.Jacket);
                }
                if (!string.IsNullOrEmpty(userAvatar.Mask))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `mask` = @mask");
                    else
                        sql = string.Concat(sql, " `mask` = @mask");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mask", userAvatar.Mask);
                }
                if (!string.IsNullOrEmpty(userAvatar.Mouth))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `mouth` = @mouth");
                    else
                        sql = string.Concat(sql, " `mouth` = @mouth");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@mouth", userAvatar.Mouth);
                }
                if (!string.IsNullOrEmpty(userAvatar.Nose))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `nose` = @nose");
                    else
                        sql = string.Concat(sql, " `nose` = @nose");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@nose", userAvatar.Nose);
                }
                if (!string.IsNullOrEmpty(userAvatar.Pants))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `pants` = @pants");
                    else
                        sql = string.Concat(sql, " `pants` = @pants");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@pants", userAvatar.Pants);
                }
                if (!string.IsNullOrEmpty(userAvatar.Shirt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `shirt` = @shirt");
                    else
                        sql = string.Concat(sql, " `shirt` = @shirt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@shirt", userAvatar.Shirt);
                }
                if (!string.IsNullOrEmpty(userAvatar.Shoes))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `shoes` = @shoes");
                    else
                        sql = string.Concat(sql, " `shoes` = @shoes");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@shoes", userAvatar.Shoes);
                }
                if (!string.IsNullOrEmpty(userAvatar.Socks))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `socks` = @socks");
                    else
                        sql = string.Concat(sql, " `socks` = @socks");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@socks", userAvatar.Socks);
                }
                if (!string.IsNullOrEmpty(userAvatar.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userAvatar.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userAvatar.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userAvatar.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userAvatar.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userAvatar.UpdatedAt);
                }
                if (userAvatarWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userAvatarWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userAvatarWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userAvatarWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Base))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `base` = @baseWhere");
                        else
                            sql = string.Concat(sql, " `base` = @baseWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@baseWhere", userAvatarWhere.Base);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.BaseHex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `base_hex` = @baseHexWhere");
                        else
                            sql = string.Concat(sql, " `base_hex` = @baseHexWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@baseHexWhere", userAvatarWhere.BaseHex);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Sex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sex` = @sexWhere");
                        else
                            sql = string.Concat(sql, " `sex` = @sexWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@sexWhere", userAvatarWhere.Sex);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.BackAccesory))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `back_accesory` = @backAccesoryWhere");
                        else
                            sql = string.Concat(sql, " `back_accesory` = @backAccesoryWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@backAccesoryWhere", userAvatarWhere.BackAccesory);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Eyebrows))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `eyebrows` = @eyebrowsWhere");
                        else
                            sql = string.Concat(sql, " `eyebrows` = @eyebrowsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@eyebrowsWhere", userAvatarWhere.Eyebrows);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Eyes))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `eyes` = @eyesWhere");
                        else
                            sql = string.Concat(sql, " `eyes` = @eyesWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@eyesWhere", userAvatarWhere.Eyes);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.EyesHex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `eyes_hex` = @eyesHexWhere");
                        else
                            sql = string.Concat(sql, " `eyes_hex` = @eyesHexWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@eyesHexWhere", userAvatarWhere.EyesHex);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.FacePaint))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `face_paint` = @facePaintWhere");
                        else
                            sql = string.Concat(sql, " `face_paint` = @facePaintWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@facePaintWhere", userAvatarWhere.FacePaint);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Glasses))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `glasses` = @glassesWhere");
                        else
                            sql = string.Concat(sql, " `glasses` = @glassesWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@glassesWhere", userAvatarWhere.Glasses);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Hair))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hair` = @hairWhere");
                        else
                            sql = string.Concat(sql, " `hair` = @hairWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hairWhere", userAvatarWhere.Hair);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.HairHex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hair_hex` = @hairHexWhere");
                        else
                            sql = string.Concat(sql, " `hair_hex` = @hairHexWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hairHexWhere", userAvatarWhere.HairHex);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Hat))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hat` = @hatWhere");
                        else
                            sql = string.Concat(sql, " `hat` = @hatWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hatWhere", userAvatarWhere.Hat);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Jacket))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `jacket` = @jacketWhere");
                        else
                            sql = string.Concat(sql, " `jacket` = @jacketWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@jacketWhere", userAvatarWhere.Jacket);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Mask))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `mask` = @maskWhere");
                        else
                            sql = string.Concat(sql, " `mask` = @maskWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@maskWhere", userAvatarWhere.Mask);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Mouth))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `mouth` = @mouthWhere");
                        else
                            sql = string.Concat(sql, " `mouth` = @mouthWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mouthWhere", userAvatarWhere.Mouth);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Nose))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `nose` = @noseWhere");
                        else
                            sql = string.Concat(sql, " `nose` = @noseWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@noseWhere", userAvatarWhere.Nose);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Pants))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `pants` = @pantsWhere");
                        else
                            sql = string.Concat(sql, " `pants` = @pantsWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@pantsWhere", userAvatarWhere.Pants);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Shirt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `shirt` = @shirtWhere");
                        else
                            sql = string.Concat(sql, " `shirt` = @shirtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@shirtWhere", userAvatarWhere.Shirt);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Shoes))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `shoes` = @shoesWhere");
                        else
                            sql = string.Concat(sql, " `shoes` = @shoesWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@shoesWhere", userAvatarWhere.Shoes);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.Socks))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `socks` = @socksWhere");
                        else
                            sql = string.Concat(sql, " `socks` = @socksWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@socksWhere", userAvatarWhere.Socks);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userAvatarWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userAvatarWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userAvatarWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userAvatarWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userAvatar);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserAvatarModel>();
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

        public List<UserAvatarModel> Update(string connectionString, List<UserAvatarModel> userAvatarList, List<UserAvatarModel> userAvatarWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userAvatarList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE av_user_avatars SET ";
                                if (!string.IsNullOrEmpty(userAvatarList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userAvatarList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userAvatarList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Base))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `base` = @base" + i);
                                    else
                                        sql = string.Concat(sql, " `base` = @base" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@base" + i, userAvatarList[i].Base);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].BaseHex))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `base_hex` = @baseHex" + i);
                                    else
                                        sql = string.Concat(sql, " `base_hex` = @baseHex" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@baseHex" + i, userAvatarList[i].BaseHex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Sex))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `sex` = @sex" + i);
                                    else
                                        sql = string.Concat(sql, " `sex` = @sex" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@sex" + i, userAvatarList[i].Sex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].BackAccesory))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `back_accesory` = @backAccesory" + i);
                                    else
                                        sql = string.Concat(sql, " `back_accesory` = @backAccesory" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@backAccesory" + i, userAvatarList[i].BackAccesory);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Eyebrows))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `eyebrows` = @eyebrows" + i);
                                    else
                                        sql = string.Concat(sql, " `eyebrows` = @eyebrows" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@eyebrows" + i, userAvatarList[i].Eyebrows);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Eyes))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `eyes` = @eyes" + i);
                                    else
                                        sql = string.Concat(sql, " `eyes` = @eyes" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@eyes" + i, userAvatarList[i].Eyes);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].EyesHex))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `eyes_hex` = @eyesHex" + i);
                                    else
                                        sql = string.Concat(sql, " `eyes_hex` = @eyesHex" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@eyesHex" + i, userAvatarList[i].EyesHex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].FacePaint))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `face_paint` = @facePaint" + i);
                                    else
                                        sql = string.Concat(sql, " `face_paint` = @facePaint" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@facePaint" + i, userAvatarList[i].FacePaint);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Glasses))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `glasses` = @glasses" + i);
                                    else
                                        sql = string.Concat(sql, " `glasses` = @glasses" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@glasses" + i, userAvatarList[i].Glasses);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Hair))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `hair` = @hair" + i);
                                    else
                                        sql = string.Concat(sql, " `hair` = @hair" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hair" + i, userAvatarList[i].Hair);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].HairHex))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `hair_hex` = @hairHex" + i);
                                    else
                                        sql = string.Concat(sql, " `hair_hex` = @hairHex" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hairHex" + i, userAvatarList[i].HairHex);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Hat))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `hat` = @hat" + i);
                                    else
                                        sql = string.Concat(sql, " `hat` = @hat" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@hat" + i, userAvatarList[i].Hat);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Jacket))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `jacket` = @jacket" + i);
                                    else
                                        sql = string.Concat(sql, " `jacket` = @jacket" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@jacket" + i, userAvatarList[i].Jacket);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Mask))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `mask` = @mask" + i);
                                    else
                                        sql = string.Concat(sql, " `mask` = @mask" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mask" + i, userAvatarList[i].Mask);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Mouth))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `mouth` = @mouth" + i);
                                    else
                                        sql = string.Concat(sql, " `mouth` = @mouth" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@mouth" + i, userAvatarList[i].Mouth);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Nose))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `nose` = @nose" + i);
                                    else
                                        sql = string.Concat(sql, " `nose` = @nose" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@nose" + i, userAvatarList[i].Nose);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Pants))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `pants` = @pants" + i);
                                    else
                                        sql = string.Concat(sql, " `pants` = @pants" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@pants" + i, userAvatarList[i].Pants);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Shirt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `shirt` = @shirt" + i);
                                    else
                                        sql = string.Concat(sql, " `shirt` = @shirt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@shirt" + i, userAvatarList[i].Shirt);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Shoes))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `shoes` = @shoes" + i);
                                    else
                                        sql = string.Concat(sql, " `shoes` = @shoes" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@shoes" + i, userAvatarList[i].Shoes);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].Socks))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `socks` = @socks" + i);
                                    else
                                        sql = string.Concat(sql, " `socks` = @socks" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@socks" + i, userAvatarList[i].Socks);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userAvatarList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userAvatarList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userAvatarList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userAvatarList[i].UpdatedAt);
                                }
                                if (userAvatarWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userAvatarWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userAvatarWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Base))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `base` = @baseWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `base` = @baseWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@baseWhere" + i, userAvatarWhereList[i].Base);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].BaseHex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `base_hex` = @baseHexWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `base_hex` = @baseHexWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@baseHexWhere" + i, userAvatarWhereList[i].BaseHex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Sex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sex` = @sexWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `sex` = @sexWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@sexWhere" + i, userAvatarWhereList[i].Sex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].BackAccesory))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `back_accesory` = @backAccesoryWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `back_accesory` = @backAccesoryWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@backAccesoryWhere" + i, userAvatarWhereList[i].BackAccesory);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Eyebrows))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `eyebrows` = @eyebrowsWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `eyebrows` = @eyebrowsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@eyebrowsWhere" + i, userAvatarWhereList[i].Eyebrows);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Eyes))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `eyes` = @eyesWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `eyes` = @eyesWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@eyesWhere" + i, userAvatarWhereList[i].Eyes);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].EyesHex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `eyes_hex` = @eyesHexWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `eyes_hex` = @eyesHexWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@eyesHexWhere" + i, userAvatarWhereList[i].EyesHex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].FacePaint))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `face_paint` = @facePaintWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `face_paint` = @facePaintWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@facePaintWhere" + i, userAvatarWhereList[i].FacePaint);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Glasses))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `glasses` = @glassesWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `glasses` = @glassesWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@glassesWhere" + i, userAvatarWhereList[i].Glasses);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Hair))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hair` = @hairWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `hair` = @hairWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hairWhere" + i, userAvatarWhereList[i].Hair);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].HairHex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hair_hex` = @hairHexWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `hair_hex` = @hairHexWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hairHexWhere" + i, userAvatarWhereList[i].HairHex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Hat))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hat` = @hatWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `hat` = @hatWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hatWhere" + i, userAvatarWhereList[i].Hat);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Jacket))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `jacket` = @jacketWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `jacket` = @jacketWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@jacketWhere" + i, userAvatarWhereList[i].Jacket);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Mask))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `mask` = @maskWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `mask` = @maskWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@maskWhere" + i, userAvatarWhereList[i].Mask);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Mouth))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `mouth` = @mouthWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `mouth` = @mouthWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mouthWhere" + i, userAvatarWhereList[i].Mouth);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Nose))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `nose` = @noseWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `nose` = @noseWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@noseWhere" + i, userAvatarWhereList[i].Nose);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Pants))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `pants` = @pantsWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `pants` = @pantsWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@pantsWhere" + i, userAvatarWhereList[i].Pants);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Shirt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `shirt` = @shirtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `shirt` = @shirtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@shirtWhere" + i, userAvatarWhereList[i].Shirt);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Shoes))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `shoes` = @shoesWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `shoes` = @shoesWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@shoesWhere" + i, userAvatarWhereList[i].Shoes);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].Socks))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `socks` = @socksWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `socks` = @socksWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@socksWhere" + i, userAvatarWhereList[i].Socks);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userAvatarWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userAvatarWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatarWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userAvatarWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userAvatarList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserAvatarModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserAvatarModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAvatarModel>();
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

        public List<UserAvatarModel> Delete(string connectionString, UserAvatarModel userAvatar)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM av_user_avatars  ";
                if (userAvatar != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userAvatar.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userAvatar.Id);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userAvatar.UserId);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Base))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `base` = @base");
                        else
                            sql = string.Concat(sql, " `base` = @base");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@base", userAvatar.Base);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.BaseHex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `base_hex` = @baseHex");
                        else
                            sql = string.Concat(sql, " `base_hex` = @baseHex");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@baseHex", userAvatar.BaseHex);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Sex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `sex` = @sex");
                        else
                            sql = string.Concat(sql, " `sex` = @sex");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@sex", userAvatar.Sex);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.BackAccesory))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `back_accesory` = @backAccesory");
                        else
                            sql = string.Concat(sql, " `back_accesory` = @backAccesory");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@backAccesory", userAvatar.BackAccesory);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Eyebrows))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `eyebrows` = @eyebrows");
                        else
                            sql = string.Concat(sql, " `eyebrows` = @eyebrows");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@eyebrows", userAvatar.Eyebrows);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Eyes))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `eyes` = @eyes");
                        else
                            sql = string.Concat(sql, " `eyes` = @eyes");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@eyes", userAvatar.Eyes);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.EyesHex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `eyes_hex` = @eyesHex");
                        else
                            sql = string.Concat(sql, " `eyes_hex` = @eyesHex");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@eyesHex", userAvatar.EyesHex);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.FacePaint))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `face_paint` = @facePaint");
                        else
                            sql = string.Concat(sql, " `face_paint` = @facePaint");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@facePaint", userAvatar.FacePaint);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Glasses))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `glasses` = @glasses");
                        else
                            sql = string.Concat(sql, " `glasses` = @glasses");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@glasses", userAvatar.Glasses);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Hair))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hair` = @hair");
                        else
                            sql = string.Concat(sql, " `hair` = @hair");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hair", userAvatar.Hair);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.HairHex))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hair_hex` = @hairHex");
                        else
                            sql = string.Concat(sql, " `hair_hex` = @hairHex");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hairHex", userAvatar.HairHex);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Hat))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `hat` = @hat");
                        else
                            sql = string.Concat(sql, " `hat` = @hat");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@hat", userAvatar.Hat);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Jacket))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `jacket` = @jacket");
                        else
                            sql = string.Concat(sql, " `jacket` = @jacket");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@jacket", userAvatar.Jacket);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Mask))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `mask` = @mask");
                        else
                            sql = string.Concat(sql, " `mask` = @mask");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mask", userAvatar.Mask);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Mouth))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `mouth` = @mouth");
                        else
                            sql = string.Concat(sql, " `mouth` = @mouth");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@mouth", userAvatar.Mouth);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Nose))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `nose` = @nose");
                        else
                            sql = string.Concat(sql, " `nose` = @nose");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nose", userAvatar.Nose);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Pants))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `pants` = @pants");
                        else
                            sql = string.Concat(sql, " `pants` = @pants");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@pants", userAvatar.Pants);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Shirt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `shirt` = @shirt");
                        else
                            sql = string.Concat(sql, " `shirt` = @shirt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@shirt", userAvatar.Shirt);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Shoes))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `shoes` = @shoes");
                        else
                            sql = string.Concat(sql, " `shoes` = @shoes");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@shoes", userAvatar.Shoes);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.Socks))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `socks` = @socks");
                        else
                            sql = string.Concat(sql, " `socks` = @socks");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@socks", userAvatar.Socks);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userAvatar.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userAvatar.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userAvatar.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userAvatar.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userAvatar);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserAvatarModel>();
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

        public List<UserAvatarModel> Delete(string connectionString, List<UserAvatarModel> userAvatar)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserAvatarModel> response = new List<UserAvatarModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userAvatar.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM av_user_avatars  ";
                                if (userAvatar[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userAvatar[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userAvatar[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userAvatar[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Base))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `base` = @base" + i);
                                        else
                                            sql = string.Concat(sql, " `base` = @base" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@base" + i, userAvatar[i].Base);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].BaseHex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `base_hex` = @baseHex" + i);
                                        else
                                            sql = string.Concat(sql, " `base_hex` = @baseHex" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@baseHex" + i, userAvatar[i].BaseHex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Sex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `sex` = @sex" + i);
                                        else
                                            sql = string.Concat(sql, " `sex` = @sex" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@sex" + i, userAvatar[i].Sex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].BackAccesory))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `back_accesory` = @backAccesory" + i);
                                        else
                                            sql = string.Concat(sql, " `back_accesory` = @backAccesory" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@backAccesory" + i, userAvatar[i].BackAccesory);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Eyebrows))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `eyebrows` = @eyebrows" + i);
                                        else
                                            sql = string.Concat(sql, " `eyebrows` = @eyebrows" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@eyebrows" + i, userAvatar[i].Eyebrows);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Eyes))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `eyes` = @eyes" + i);
                                        else
                                            sql = string.Concat(sql, " `eyes` = @eyes" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@eyes" + i, userAvatar[i].Eyes);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].EyesHex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `eyes_hex` = @eyesHex" + i);
                                        else
                                            sql = string.Concat(sql, " `eyes_hex` = @eyesHex" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@eyesHex" + i, userAvatar[i].EyesHex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].FacePaint))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `face_paint` = @facePaint" + i);
                                        else
                                            sql = string.Concat(sql, " `face_paint` = @facePaint" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@facePaint" + i, userAvatar[i].FacePaint);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Glasses))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `glasses` = @glasses" + i);
                                        else
                                            sql = string.Concat(sql, " `glasses` = @glasses" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@glasses" + i, userAvatar[i].Glasses);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Hair))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hair` = @hair" + i);
                                        else
                                            sql = string.Concat(sql, " `hair` = @hair" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hair" + i, userAvatar[i].Hair);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].HairHex))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hair_hex` = @hairHex" + i);
                                        else
                                            sql = string.Concat(sql, " `hair_hex` = @hairHex" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hairHex" + i, userAvatar[i].HairHex);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Hat))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `hat` = @hat" + i);
                                        else
                                            sql = string.Concat(sql, " `hat` = @hat" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@hat" + i, userAvatar[i].Hat);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Jacket))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `jacket` = @jacket" + i);
                                        else
                                            sql = string.Concat(sql, " `jacket` = @jacket" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@jacket" + i, userAvatar[i].Jacket);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Mask))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `mask` = @mask" + i);
                                        else
                                            sql = string.Concat(sql, " `mask` = @mask" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mask" + i, userAvatar[i].Mask);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Mouth))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `mouth` = @mouth" + i);
                                        else
                                            sql = string.Concat(sql, " `mouth` = @mouth" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@mouth" + i, userAvatar[i].Mouth);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Nose))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `nose` = @nose" + i);
                                        else
                                            sql = string.Concat(sql, " `nose` = @nose" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nose" + i, userAvatar[i].Nose);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Pants))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `pants` = @pants" + i);
                                        else
                                            sql = string.Concat(sql, " `pants` = @pants" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@pants" + i, userAvatar[i].Pants);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Shirt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `shirt` = @shirt" + i);
                                        else
                                            sql = string.Concat(sql, " `shirt` = @shirt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@shirt" + i, userAvatar[i].Shirt);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Shoes))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `shoes` = @shoes" + i);
                                        else
                                            sql = string.Concat(sql, " `shoes` = @shoes" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@shoes" + i, userAvatar[i].Shoes);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].Socks))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `socks` = @socks" + i);
                                        else
                                            sql = string.Concat(sql, " `socks` = @socks" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@socks" + i, userAvatar[i].Socks);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userAvatar[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userAvatar[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userAvatar[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userAvatar[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userAvatar[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserAvatarModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserAvatarModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserAvatarModel>();
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
