using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsAPI.CustomClasses
{
    public static class Settings
    {
        public static string CONNECTION_STRING_READ = AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"];
        public static string CONNECTION_STRING_WRITE = AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"];

        public static string SCH_SCHOOL_TYPE_ID = AppConfiguration.Configuration["AppSettings:sch_school_type_id"];
        public static string SCH_PROGRAM_TYPE_ID = AppConfiguration.Configuration["AppSettings:sch_program_type_id"];
        public static string SCH_PLATFORM_ID = AppConfiguration.Configuration["AppSettings:sch_platform_id"];

    }
}
