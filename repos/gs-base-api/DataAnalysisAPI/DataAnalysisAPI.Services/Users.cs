using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.DTOs;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // for .xlsx files (Excel 2007+)
using NPOI.SS.Util;
using System.Threading.Tasks;
using DataAnalysisAPI;

namespace DataAnalysisAPI.DataAnalysisAPI.Services
{
    /// <summary>
    /// To handle the details related to the user.
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// To prepare the general details to be applied in the other methods of this class. 
        /// The things prepared are the ORE schools if apply, the relations if apply, etc.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user making the request is director, else false.</param>
        /// <param name="isStaff">true if the user making the request is a staff, else false.</param>
        /// <param name="roleIds">Optional: The roles to filter.</param>
        /// <param name="sie">Optional: The sie (user record) details.</param>
        /// <param name="schoolIds">Optional: The id of the schools to filter.</param>
        /// <param name="userIds">Optional: The users' ids to filter.</param>
        /// <param name="oreId">Optional: (Required if ORE user): The ore to filter.</param>
        /// <param name="fromPage">Optional: From Page.</param>
        /// <param name="limiter">Optional: How many items per page.</param>
        /// <returns>Returns a list of user schools (the user details, and school details).</returns>
        public static ResponseModel<UserSchoolModel> GetUsers(int userIdRequester, bool isDirector, bool isStaff, string sie = null, List<int> roleIds = null,
            List<int> schoolIds = null, List<int> userIds = null, int? oreId = null, int? fromPage = null, int? limiter = null)
        {
            var users = PrepareUsersInformation(userIdRequester, isDirector, isStaff, sie,
                roleIds, schoolIds, userIds, oreId);

            int total = users.Count;

            // Skip by pagination 
            if (users.Count > 0)
            {
                users = users.Skip(fromPage.Value * limiter.Value).Take(limiter.Value).ToList();
            }

            return new ResponseModel<UserSchoolModel>()
            {
                Count = total,
                Results = users
            };
        }

        /// <summary>
        /// To get the users document.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user making the request is director, else false.</param>
        /// <param name="isStaff">true if the user making the request is a staff, else false.</param>
        /// <param name="roleIds">Optional: The roles to filter.</param>
        /// <param name="sie">Optional: The sie (user record) details.</param>
        /// <param name="schoolIds">Optional: The id of the schools to filter.</param>
        /// <param name="userIds">Optional: The users' ids to filter.</param>
        /// <param name="oreId">Optional: (Required if ORE user): The ore to filter.</param>
        /// <param name="languageCode">Optional: The language code to use.</param>
        /// <returns>Returns the users document.</returns>
        public static async Task<FileResponseModel> GetUsersDocument(int userIdRequester, bool isDirector, bool isStaff, string sie = null, List<int> roleIds = null,
        List<int> schoolIds = null, List<int> userIds = null, int? oreId = null, string languageCode = "es")
        {
            var usersData = PrepareUsersInformation(userIdRequester, isDirector, isStaff, sie,
                roleIds, schoolIds, userIds, oreId);

            if(usersData.Count == 0)
            {
                return new FileResponseModel() { Message = "No data found." };
            }

            if(usersData.Count > 50000)
            {
              // Send message according to language. 
              return new FileResponseModel() { Message = AppConfiguration.Configuration["AppSettings:message_data_too_large_" + languageCode] };
            }

            // Prepare the document.
            // Create a worksheet for user data
            // Create a new workbook
            IWorkbook workbook = new XSSFWorkbook();
            ISheet usersSheet = workbook.CreateSheet("Users");

            // Create White font for header
            IFont boldFontWhite = workbook.CreateFont();
            boldFontWhite.Boldweight = (short)FontBoldWeight.Bold;
            boldFontWhite.Color = IndexedColors.White.Index;
            ICellStyle boldFontWhiteStyle = workbook.CreateCellStyle();
            boldFontWhiteStyle.SetFont(boldFontWhite);

            // Set the header style
            XSSFCellStyle headerStyleCustom = (XSSFCellStyle)workbook.CreateCellStyle();
            headerStyleCustom.WrapText = true;
            headerStyleCustom.FillForegroundColor = IndexedColors.Grey50Percent.Index;
            headerStyleCustom.FillPattern = FillPattern.SolidForeground;
            headerStyleCustom.SetFont(boldFontWhite);

            ICellStyle rowStyleDetails = workbook.CreateCellStyle();
            rowStyleDetails.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightTurquoise.Index;
            rowStyleDetails.FillPattern = FillPattern.SolidForeground;

            // Create headers for user data
            IRow dataRowHeader = usersSheet.CreateRow(0);
            string[] headerUsers = { "User Id", "Username", "Email", "First Name", "Last Name",
            "Role", "School Id", "SIE (User record)", "School name", "City", "Ore (Region)", "State", "Country", 
            "School Identifier"};
            for (int i = 0; i < headerUsers.Length; i++)
            {
                dataRowHeader.CreateCell(i).SetCellValue(headerUsers[i]);
                dataRowHeader.Cells[i].CellStyle = headerStyleCustom;
            }

            if (usersData.Count > 0)
            {
                int dataRowNo = 0;
                for (int i = 1; i <= usersData.Count; i++) // Example data for 10 users
                {
                    IRow userDataRow = usersSheet.CreateRow(i);
                    userDataRow.CreateCell(0).SetCellValue(usersData[dataRowNo].UserId);
                    userDataRow.CreateCell(1).SetCellValue(usersData[dataRowNo].Username);
                    userDataRow.CreateCell(2).SetCellValue(usersData[dataRowNo].Email);
                    userDataRow.CreateCell(3).SetCellValue(usersData[dataRowNo].FirstName);
                    userDataRow.CreateCell(4).SetCellValue(usersData[dataRowNo].LastName);
                    userDataRow.CreateCell(5).SetCellValue(usersData[dataRowNo].Role);
                    userDataRow.CreateCell(6).SetCellValue(usersData[dataRowNo].SchoolId.ToString());
                    userDataRow.CreateCell(7).SetCellValue(usersData[dataRowNo].Sie);
                    userDataRow.CreateCell(8).SetCellValue(usersData[dataRowNo].SchoolName);
                    userDataRow.CreateCell(9).SetCellValue(usersData[dataRowNo].City);
                    userDataRow.CreateCell(10).SetCellValue(usersData[dataRowNo].Ore);
                    userDataRow.CreateCell(11).SetCellValue(usersData[dataRowNo].StateIsoCode);
                    userDataRow.CreateCell(12).SetCellValue(usersData[dataRowNo].Country);
                    userDataRow.CreateCell(13).SetCellValue(usersData[dataRowNo].SchoolIdentifier);
                    if (dataRowNo % 2 == 0)
                    {
                        userDataRow.RowStyle = rowStyleDetails;
                    }
                    dataRowNo++;
                }

                // Table 
                CellRangeAddress tableRangeDetails = new CellRangeAddress(0, dataRowNo - 1, 0, headerUsers.Length - 1);
                usersSheet.SetAutoFilter(tableRangeDetails);
            }

            // Get the full path to the file
            return new FileResponseModel() { File = await ExcelGenerator.GenerateExcel(workbook, "Users" + System.Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss")) };
        }

        /// <summary>
        /// To prepare the general details to be applied in the other methods of this class. 
        /// The things prepared are the ORE schools if apply, the relations if apply, etc.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user making the request is director, else false.</param>
        /// <param name="isStaff">true if the user making the request is a staff, else false.</param>
        /// <param name="roleIds">Optional: The roles to filter.</param>
        /// <param name="sie">Optional: The sie (user record) details.</param>
        /// <param name="schoolIds">Optional: The id of the schools to filter.</param>
        /// <param name="userIds">Optional: The users' ids to filter.</param>
        /// <param name="oreId">Optional: (Required if ORE user): The ore to filter.</param>
        /// <param name="fromPage">Optional: From Page.</param>
        /// <param name="limiter">Optional: How many items per page.</param>
        /// <returns>Returns a list of user schools (the user details, and school details).</returns>
        private static List<UserSchoolModel> PrepareUsersInformation(
            int userIdRequester, bool isDirector, bool isStaff, string sie = null, List<int> roleIds = null, 
            List<int> schoolIds = null, List<int> userIds = null, int? oreId = null, int? fromPage = null, int? limiter = null)
        {
            // If Ore is not null, call the schools first, so filter by schools. 
            List<int> schoolIdsFinale = new List<int>();

            // If ore is not null, it should return the schools of the ore. 
            if (oreId != null)
            {
                schoolIdsFinale = AutoFilters.GetOreSchoolIds(oreId.Value, schoolIds);
                if(schoolIdsFinale == null || schoolIdsFinale.Count == 0)
                {
                    return new List<UserSchoolModel>(); 
                }
            }

            // If director, get the user ids that can be handle 
            if (isDirector)
            {
                userIds = AutoFilters.GetUsersIdByDirectors(userIdRequester, userIds);

                if(userIds == null || userIds.Count == 0)
                {
                    return new List<UserSchoolModel>();
                }

            }

            // If non staff or non director
            if (!isStaff && !isDirector)
            {
                userIds = AutoFilters.GetUsersIdsByNonDirectorNonStaffUser(userIdRequester, userIds);
                if (userIds == null || userIds.Count == 0)
                {
                    return new List<UserSchoolModel>();
                }
            }

            List<UserSchoolModel> usersData = Infrastructure.Dapper.Users.GetUsers(sie, roleIds, 
                schoolIdsFinale != null && schoolIdsFinale.Count() > 0 ? schoolIdsFinale : null, userIds);

            // Sort by datetime 
            usersData = usersData.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();

            // Limit 
            if (fromPage != null && limiter != null)
            {
                usersData = usersData.Skip(fromPage.Value * limiter.Value).Take(limiter.Value).ToList();
            }

            return usersData;
        }
    }
}