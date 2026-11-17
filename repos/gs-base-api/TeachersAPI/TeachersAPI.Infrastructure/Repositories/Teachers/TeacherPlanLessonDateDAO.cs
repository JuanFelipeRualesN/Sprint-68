using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dapper;
using MySqlConnector;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers
{
    /// <summary>
    /// 
    /// </summary>
    public class TeacherPlanLessonDateDAO
    {
        private MySqlConnection _connection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="teacherPlanLessonDateModelList"></param>
        /// <returns></returns>
        public List<TeacherPlanLessonDateModel> Create(string connectionString,
            List<TeacherPlanLessonDateModel> teacherPlanLessonDateModelList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonDateModel> response = new List<TeacherPlanLessonDateModel>();
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
                        for (int i = 0; i < teacherPlanLessonDateModelList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO teacher_plan_lesson_dates (";
                                string values = "(";
                                if (teacherPlanLessonDateModelList[i].TeacherPlanCustomLessonId != 0)
                                {
                                    sql = string.Concat(sql, " `teacher_plan_lesson_id` ");
                                    values = string.Concat(values, "@TeacherPlanLessonId" + i);
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@TeacherPlanLessonId" + i,
                                        teacherPlanLessonDateModelList[i].TeacherPlanCustomLessonId);
                                }

                                if (!string.IsNullOrEmpty(teacherPlanLessonDateModelList[i].ImplementationDate))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `implementation_date` ");
                                        values = string.Concat(values, ", @Date" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `implementation_date` ");
                                        values = string.Concat(values, "@Date" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@Date" + i, teacherPlanLessonDateModelList[i].ImplementationDate);
                                }

                                if (!string.IsNullOrEmpty(teacherPlanLessonDateModelList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `modified_by` ");
                                        values = string.Concat(values, ", @ModifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `modified_by` ");
                                        values = string.Concat(values, "@ModifiedBy" + i);
                                    }

                                    cmd.Parameters.AddWithValue("@ModifiedBy" + i,
                                        teacherPlanLessonDateModelList[i].ModifiedBy);
                                }


                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(teacherPlanLessonDateModelList[i]);
                            }
                            catch (MySqlException e)
                            {
                                Debug.WriteLine("Something happends trying to save the lesson dates: {0}", e.Message);
                                response = new List<TeacherPlanLessonDateModel>();
                            }
                        }

                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        Debug.WriteLine("Something happends trying to save the lesson dates: {0}", e.Message);
                        trans.Rollback();
                        response = new List<TeacherPlanLessonDateModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("Something happends trying to save the lesson dates: {0}", e.Message);
                response = new List<TeacherPlanLessonDateModel>();
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
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="teacherPlanLessonId"></param>
        /// <returns></returns>
        public List<TeacherPlanLessonDateModel> Delete(string connectionString, int teacherPlanLessonId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<TeacherPlanLessonDateModel> response = new List<TeacherPlanLessonDateModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "DELETE FROM teacher_plan_lesson_dates WHERE teacher_plan_lesson_id = @TeacherPlanLessonId";
                cmd.Parameters.AddWithValue("@TeacherPlanLessonId", teacherPlanLessonId);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                response.Add(new TeacherPlanLessonDateModel() { Valid = true });
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("Something happends trying to delete the lesson dates: {0}", e.Message);
                response = new List<TeacherPlanLessonDateModel>();
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



        public List<TeacherPlanLessonDateModel> GetTeacherPlanLessonDates(List<int> teacherPlanLessonIds)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DynamicParameters parameters = new DynamicParameters();
            string query =
                @"SELECT id, teacher_plan_lesson_id, implementation_date, modified_by, created_at, updated_at 
                    FROM gs_teachers_web_db_prod.teacher_plan_lesson_dates 
                    WHERE teacher_plan_lesson_id IN @teacherPlanLessonId";

            parameters.Add("@teacherPlanLessonId", teacherPlanLessonIds);

            IEnumerable<TeacherPlanLessonDateModel> items = new List<TeacherPlanLessonDateModel>();
            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"]
            };
            connection.Open();
            try
            {
                items = connection.Query<TeacherPlanLessonDateModel>(query, parameters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Something happends trying to save the lesson dates: {0}", ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return items?.ToList();
        }
    }
}