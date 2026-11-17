using NPOI.SS.UserModel;
using System.IO;
using System.Threading.Tasks;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// To handle excels generator.
    /// </summary>
    public static class ExcelGenerator
    {
        /// <summary>
        /// To generate an excel file.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        /// <param name="fileName">The file's name.</param>
        /// <returns>Returns the file URL.</returns>
        public async static Task<string> GenerateExcel(IWorkbook workbook, string fileName)
        {
            var ms = new MemoryStream();
            workbook.Write(ms, true);
            return await AzureAssets.UploadFileToAzureStorage(ms, fileName + ".xlsx");
        }
    }
}