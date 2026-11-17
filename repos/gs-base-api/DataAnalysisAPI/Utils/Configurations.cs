using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySqlConnector;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses;
using DataAnalysisAPI;

namespace DataAnalysisAPI.Utils
{
    public static class Configurations
    {
        public static MySqlConnection GetMySqlConnection()
        {
            return new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:AnalysisConnectionStringReader"]
            };
        }

        public static DateTime GetMinDateForQueries()
        {
            return DateTime.Parse(AppConfiguration.Configuration["AppSettings:min_date"]);
        }

        public static DateTime GetMaxDatesForQueries()
        {
            return DateTime.Now.AddDays(-1);
        }

        public static string GetLogoUrl()
        {
            return AppConfiguration.Configuration["AppSettings:report_logo_url"];
        }

        public static string GetStudentRoleId()
        {
            return AppConfiguration.Configuration["AppSettings:role_student_id"];
        }

        public static string GetTeacherRoleId()
        {
            return AppConfiguration.Configuration["AppSettings:role_teacher_id"];
        }

        public static string GetGenericMessageOnFail(string langCode = "en")
        {
            return AppConfiguration.Configuration["AppSettings:generic_message_" + langCode];
        }

        public static string GetStorageAccountName()
        {
            return AppConfiguration.Configuration["AppSettings:azure_storage_account_name"];
        }

        public static string GetStorageAccountKey()
        {
            return AppConfiguration.Configuration["AppSettings:azure_storage_account_key"];
        }

        public static string GetStorageContainerName()
        {
            return AppConfiguration.Configuration["AppSettings:azure_storage_container_name"];
        }

        public static List<TextTranslationModel> GetPDFTextTranslations()
        {
            return new List<TextTranslationModel>()
            {
                new TextTranslationModel
                {
                    Text = "PERIODO ACADÉMICO",
                    TextCode = "academic_period",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "ACADEMIC PERIOD",
                    TextCode = "academic_period",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Periodo",
                    TextCode = "period",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Period",
                    TextCode = "period",
                    LangCode = "en",
                },
                 new TextTranslationModel
                {
                    Text = "Materia",
                    TextCode = "subject",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Subject",
                    TextCode = "subject",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Promedio por periodo",
                    TextCode = "period_average",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Average per period",
                    TextCode = "period_average",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Promedio Acumulativo",
                    TextCode = "cummulative_average",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Cummulative Average",
                    TextCode = "cummulative_average",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Acumulativo",
                    TextCode = "cummulative",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Cummulative",
                    TextCode = "cummulative",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Escalas",
                    TextCode = "scale",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Scale",
                    TextCode = "scale",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Excelente",
                    TextCode = "excellent",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Excellent",
                    TextCode = "excellent",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Bueno",
                    TextCode = "good",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Good",
                    TextCode = "good",
                    LangCode = "en",
                },
                 new TextTranslationModel
                {
                    Text = "Satisfactorio",
                    TextCode = "satisfactory",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Satisfactory",
                    TextCode = "satisfactory",
                    LangCode = "en",
                },
                 new TextTranslationModel
                {
                    Text = "Deficiente",
                    TextCode = "deficient",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Deficient",
                    TextCode = "deficient",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = "Fracaso",
                    TextCode = "fail",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = "Fail",
                    TextCode = "fail",
                    LangCode = "en",
                },
                new TextTranslationModel
                {
                    Text = " o menos ",
                    TextCode = "or_less",
                    LangCode = "es",
                },
                new TextTranslationModel
                {
                    Text = " or less ",
                    TextCode = "or_less",
                    LangCode = "en",
                },
            };
        }
    }
}