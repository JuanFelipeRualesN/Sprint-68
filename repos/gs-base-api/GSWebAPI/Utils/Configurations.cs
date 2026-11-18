using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.Utils
{
    public class Configurations
    {

        public static string GetStorageAccountName()
        {
            return AppConfiguration.Configuration["AppSettings:azure_storage_account_name"]; 
        }

        public static string GetStorageAccountKey()
        {
            return AppConfiguration.Configuration["AppSettings:AzureStorageAccountKey"];
        }

        public static string GetStorageContainerName()
        {
            return AppConfiguration.Configuration["AppSettings:azure_storage_container_name_gsweb"];
        }
    }
}
