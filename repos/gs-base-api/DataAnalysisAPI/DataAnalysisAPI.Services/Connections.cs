using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.DTOs;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // for .xlsx files (Excel 2007+)
using NPOI.HSSF.UserModel; // for .xls files (Excel 97-2003)
using System.Data;
using NPOI.SS.Util;
using System.Threading.Tasks;
using DataAnalysisAPI;


namespace DataAnalysisAPI.DataAnalysisAPI.Services
{
    /// <summary>
    /// Service to handle the connections reports. 
    /// </summary>
    public static class Connections
    {
        /// <summary>
        /// To get the users connections.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user making the request is director, else false.</param>
        /// <param name="isStaff">true if the user making the request is a staff, else false.</param>
        /// <param name="dateFrom">The date from to get the connections.</param>
        /// <param name="dateTo">The date to to get the connections.</param>
        /// <param name="schoolIds">Optional: The id of the schools to filter.</param>
        /// <param name="userIds">Optional: The users' ids to filter.</param>
        /// <param name="oreId">Optional: (Required if ORE user): The ore to filter.</param>
        /// <param name="fromPage">Optional: From Page.</param>
        /// <param name="limiter">Optional: How many items per page.</param>
        /// <returns>Returns a response model with users connected etails. </returns>
        public static ResponseModel<UserSchoolModel> GetUsersConnections(int userIdRequester, bool isDirector, bool isStaff,
            DateTime? dateFrom, DateTime? dateTo, List<int> schoolIds = null,
            List<int> userIds = null, int? oreId = null, int? fromPage = null, int? limiter = null)
        {
            var users = PrepareUsersConnections(userIdRequester, isDirector, isStaff,
            dateFrom, dateTo, schoolIds,
            userIds, oreId);

            int total = users.Count;

            // Skip by pagination 
            if (total > 0)
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
        /// To get the users connections document.
        /// </summary>
        /// <param name="userIdRequester">The user that is making the request.</param>
        /// <param name="isDirector">true if director (principal), else false.</param>
        /// <param name="isStaff">true if staff, else false.</param>
        /// <param name="dateFrom">Date from to get the connections.</param>
        /// <param name="dateTo">Date to (until) to get the connections.</param>
        /// <param name="schoolIds">Optional: The schools ids. (But required ir ORE).</param>
        /// <param name="userIds">Optional: The user ids. (But required if teacher)</param>
        /// <param name="oreId">Optional: The id of the ORE. (Required if Ore admin).</param>
        /// <param name="languageCode">The language code to get the message.</param>
        /// <returns>Returns the file response model with the URL path.</returns>
        public static async Task<FileResponseModel> GetUsersConnectionsDocument(int userIdRequester, bool isDirector, bool isStaff,
            DateTime? dateFrom, DateTime? dateTo, List<int> schoolIds = null,
            List<int> userIds = null, int? oreId = null, string languageCode = "es")
        {
            var usersConnections = PrepareUsersConnections(userIdRequester, isDirector, isStaff,
               dateFrom, dateTo, schoolIds,
               userIds, oreId);

            // Get all the users general
            // If ore is not null, it should return the schools of the ore.
            List<UserSchoolModel> usersSchoolGeneral = new List<UserSchoolModel>();
            if (oreId != null && isStaff)
            {
                schoolIds = AutoFilters.GetOreSchoolIds(oreId.Value, schoolIds);
                if(schoolIds != null && schoolIds.Count > 0)
                {
                    usersSchoolGeneral = Infrastructure.Dapper.Users.GetUsers(null, null, schoolIds, userIds);
                }
            }
            else if(isStaff && oreId == null)
            {
                usersSchoolGeneral = Infrastructure.Dapper.Users.GetUsers(null, null, schoolIds, userIds);
            }
    
            // If director
            if (isDirector)
            {
                userIds = AutoFilters.GetUsersIdByDirectors(userIdRequester, userIds);
                if (userIds != null && userIds.Count > 0)
                {
                    usersSchoolGeneral = Infrastructure.Dapper.Users.GetUsers(null, null, schoolIds, userIds);
                }
            }

            // If non staff or non director
            if (!isStaff && !isDirector)
            {
                userIds = AutoFilters.GetUsersIdsByNonDirectorNonStaffUser(userIdRequester, userIds);
                if (userIds != null && userIds.Count > 0)
                {
                    usersSchoolGeneral = Infrastructure.Dapper.Users.GetUsers(null, null, schoolIds, userIds);
                }
            }

            if (usersSchoolGeneral.Count > 50000)
            {
                // Send message according to language. 
                return new FileResponseModel() { Message = AppConfiguration.Configuration["AppSettings:message_data_too_large_" + languageCode] };
            }

            List<int> distinctUserIds = usersConnections.Select(x => x.UserId).Distinct().ToList();
            List<UserSchoolModel> usersNotConnected = new List<UserSchoolModel>();
            if (usersSchoolGeneral.Count > 0)
            {
                usersNotConnected = usersSchoolGeneral.Where(user => !distinctUserIds.Any(userConnected => user.UserId == userConnected)).ToList();
            }

            var totalUsers = usersSchoolGeneral.Count;
            var totalConnected = distinctUserIds.Count;
            var totalNotConnected = usersNotConnected.Count;
            var totalPercentageOfConnectivity = 0f;
            try
            {
                totalPercentageOfConnectivity = (float.Parse(totalConnected.ToString()) / float.Parse(totalUsers.ToString()) * 100);
            }
            catch (Exception)
            {

            }

            return new FileResponseModel()
            {
                File = await CreateConnectionsDocument(totalConnected, totalUsers, totalPercentageOfConnectivity, dateFrom, dateTo,
                usersConnections, usersNotConnected)
            };
        }

        /// <summary>
        /// To prepare the document details.
        /// </summary>
        /// <param name="totalConnected">The total of users connected.</param>
        /// <param name="totalUsers">The total of users.</param>
        /// <param name="totalPercent">The total percentage.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="usersConnected">A list of users (full details) that have been connected.</param>
        /// <param name="usersNotConnected">A list of users (full details) that have not been connected. </param>
        /// <returns></returns>
        private static async Task<string> CreateConnectionsDocument(float totalConnected, float totalUsers, float totalPercent, 
            DateTime? dateFrom, DateTime? dateTo, List<UserSchoolModel> usersConnected, List<UserSchoolModel> usersNotConnected)
        {
            // Prepare the document.
            // Create a worksheet for user data
            // Create a new workbook
            IWorkbook workbook = new XSSFWorkbook();
            ISheet totalsSheet = workbook.CreateSheet("Total");

            // Create White font for header
            IFont boldFontWhite = workbook.CreateFont();
            boldFontWhite.Boldweight = (short)FontBoldWeight.Bold;
            boldFontWhite.Color = IndexedColors.White.Index;
            ICellStyle boldFontWhiteStyle = workbook.CreateCellStyle();
            boldFontWhiteStyle.SetFont(boldFontWhite);

            // Set the header style
            XSSFCellStyle headerStyleCustom = (XSSFCellStyle)workbook.CreateCellStyle();
            headerStyleCustom.WrapText = true;
            headerStyleCustom.FillForegroundColor = IndexedColors.BlueGrey.Index;
            headerStyleCustom.FillPattern = FillPattern.SolidForeground;
            headerStyleCustom.SetFont(boldFontWhite);

            ICellStyle rowStyleDetails = workbook.CreateCellStyle();
            rowStyleDetails.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightCornflowerBlue.Index;
            rowStyleDetails.FillPattern = FillPattern.SolidForeground;

            // Create headers for user data
            IRow totalDataRowHeader = totalsSheet.CreateRow(0);
            string[] headerTotals = { "Total Connected", "Total Users", "Total Percent", "Date From", "Date To" };
            for (int i = 0; i < headerTotals.Length; i++)
            {
                totalDataRowHeader.CreateCell(i).SetCellValue(headerTotals[i]);
                totalDataRowHeader.Cells[i].CellStyle = headerStyleCustom;
            }

            IRow totalDataRow = totalsSheet.CreateRow(1);
            totalDataRow.CreateCell(0).SetCellValue(totalConnected);
            totalDataRow.CreateCell(1).SetCellValue(totalUsers);
            totalDataRow.CreateCell(2).SetCellValue(totalPercent);
            totalDataRow.CreateCell(3).SetCellValue(dateFrom.Value.ToString());
            totalDataRow.CreateCell(4).SetCellValue(dateTo.Value.ToString());

            // Create the users connected information 
            ISheet usersConnectedSheet = workbook.CreateSheet("Connections");
            IRow userConnectedHeaderDataRow = usersConnectedSheet.CreateRow(0);
            string[] headerUsersConnections = { "User Id", "Username", "Email", "First Name", "Last Name", "Role",
            "School Id", "SIE (User record)", "Connection date (UTC)", "School name", "School type", "City",
            "Ore (Region)", "State ISO Code", "Country", "School Identifier"};
            for (int i = 0; i < headerUsersConnections.Length; i++)
            {
                userConnectedHeaderDataRow.CreateCell(i).SetCellValue(headerUsersConnections[i]);
                userConnectedHeaderDataRow.Cells[i].CellStyle = headerStyleCustom;
            }

            if (usersConnected.Count > 0)
            {
                int dataRowNo = 0;
                for (int i = 1; i <= usersConnected.Count; i++) // Example data for 10 users
                {
                    IRow userDataRow = usersConnectedSheet.CreateRow(i);
                    userDataRow.CreateCell(0).SetCellValue(usersConnected[dataRowNo].UserId);
                    userDataRow.CreateCell(1).SetCellValue(usersConnected[dataRowNo].Username);
                    userDataRow.CreateCell(2).SetCellValue(usersConnected[dataRowNo].Email);
                    userDataRow.CreateCell(3).SetCellValue(usersConnected[dataRowNo].FirstName);
                    userDataRow.CreateCell(4).SetCellValue(usersConnected[dataRowNo].LastName);
                    userDataRow.CreateCell(5).SetCellValue(usersConnected[dataRowNo].Role);
                    userDataRow.CreateCell(6).SetCellValue(usersConnected[dataRowNo].SchoolId.ToString());
                    userDataRow.CreateCell(7).SetCellValue(usersConnected[dataRowNo].Sie);
                    userDataRow.CreateCell(8).SetCellValue(usersConnected[dataRowNo].ConnectedDate.ToString());
                    userDataRow.CreateCell(9).SetCellValue(usersConnected[dataRowNo].SchoolName);
                    userDataRow.CreateCell(10).SetCellValue(usersConnected[dataRowNo].SchoolType);
                    userDataRow.CreateCell(11).SetCellValue(usersConnected[dataRowNo].City);
                    userDataRow.CreateCell(12).SetCellValue(usersConnected[dataRowNo].Ore);
                    userDataRow.CreateCell(13).SetCellValue(usersConnected[dataRowNo].StateIsoCode);
                    userDataRow.CreateCell(14).SetCellValue(usersConnected[dataRowNo].Country);
                    userDataRow.CreateCell(15).SetCellValue(usersConnected[dataRowNo].SchoolIdentifier);
                    if (dataRowNo % 2 == 0)
                    {
                        userDataRow.RowStyle = rowStyleDetails;
                    }
                    dataRowNo++;
                }

                // Table 
                CellRangeAddress tableRangeDetails = new CellRangeAddress(0, dataRowNo - 1, 0, headerUsersConnections.Length - 1);
                usersConnectedSheet.SetAutoFilter(tableRangeDetails);
            }

            // Create the users non connected information 
            ISheet usersNotConnectedSheet = workbook.CreateSheet("Not connected");
            IRow userNotConnectedHeaderDataRow = usersNotConnectedSheet.CreateRow(0);
            for (int i = 0; i < headerUsersConnections.Length; i++)
            {
                userNotConnectedHeaderDataRow.CreateCell(i).SetCellValue(headerUsersConnections[i]);
                userNotConnectedHeaderDataRow.Cells[i].CellStyle = headerStyleCustom;
            }

            if (usersNotConnected.Count > 0)
            {
                int dataRowNo = 0;
                for (int i = 1; i <= usersNotConnected.Count; i++) // Example data for 10 users
                {
                    IRow userDataRow = usersNotConnectedSheet.CreateRow(i);
                    userDataRow.CreateCell(0).SetCellValue(usersNotConnected[dataRowNo].UserId);
                    userDataRow.CreateCell(1).SetCellValue(usersNotConnected[dataRowNo].Username);
                    userDataRow.CreateCell(2).SetCellValue(usersNotConnected[dataRowNo].Email);
                    userDataRow.CreateCell(3).SetCellValue(usersNotConnected[dataRowNo].FirstName);
                    userDataRow.CreateCell(4).SetCellValue(usersNotConnected[dataRowNo].LastName);
                    userDataRow.CreateCell(5).SetCellValue(usersNotConnected[dataRowNo].Role);
                    userDataRow.CreateCell(6).SetCellValue(usersNotConnected[dataRowNo].SchoolId.ToString());
                    userDataRow.CreateCell(7).SetCellValue(usersNotConnected[dataRowNo].Sie);
                    userDataRow.CreateCell(8).SetCellValue("");
                    userDataRow.CreateCell(9).SetCellValue(usersNotConnected[dataRowNo].SchoolName);
                    userDataRow.CreateCell(10).SetCellValue(usersNotConnected[dataRowNo].SchoolType);
                    userDataRow.CreateCell(11).SetCellValue(usersNotConnected[dataRowNo].City);
                    userDataRow.CreateCell(12).SetCellValue(usersNotConnected[dataRowNo].Ore);
                    userDataRow.CreateCell(13).SetCellValue(usersNotConnected[dataRowNo].StateIsoCode);
                    userDataRow.CreateCell(14).SetCellValue(usersNotConnected[dataRowNo].Country);
                    userDataRow.CreateCell(15).SetCellValue(usersNotConnected[dataRowNo].SchoolIdentifier);
                    if (dataRowNo % 2 == 0)
                    {
                        userDataRow.RowStyle = rowStyleDetails;
                    }
                    dataRowNo++;
                
                    var tableRangeDetails = new CellRangeAddress(0, dataRowNo - 1, 0, headerUsersConnections.Length - 1);
                    usersNotConnectedSheet.SetAutoFilter(tableRangeDetails);
                }
            }

            // Get the full path to the file
            return await ExcelGenerator.GenerateExcel(workbook, "Connections" + System.Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        /// <summary>
        /// To prepare the general details to be applied in the other methods of this class. 
        /// The things prepared are the ORE schools if apply, the relations if apply, etc.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user making the request is director, else false.</param>
        /// <param name="isStaff">true if the user making the request is a staff, else false.</param>
        /// <param name="dateFrom">The date from to get the connections.</param>
        /// <param name="dateTo">The date to to get the connections.</param>
        /// <param name="schoolIds">Optional: The id of the schools to filter.</param>
        /// <param name="userIds">Optional: The users' ids to filter.</param>
        /// <param name="oreId">Optional: (Required if ORE user): The ore to filter.</param>
        /// <param name="fromPage">Optional: From Page.</param>
        /// <param name="limiter">Optional: How many items per page.</param>
        /// <returns>Returns a list of user schools (the user details, connection details, and school details).</returns>
        private static List<UserSchoolModel> PrepareUsersConnections(
            int userIdRequester, bool isDirector, bool isStaff,
            DateTime? dateFrom, DateTime? dateTo, List<int> schoolIds = null,
            List<int> userIds = null, int? oreId = null, int? fromPage = null, int? limiter = null)
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

            List<UserSchoolModel> connectedDatesData = Infrastructure.Dapper.Connections.GetUsersConnections(
                dateFrom, dateTo, schoolIdsFinale != null && schoolIdsFinale.Count() > 0 ? schoolIdsFinale : null,
                userIds);

            // Sort by datetime 
            connectedDatesData = connectedDatesData.OrderByDescending(x => x.ConnectedDate).ToList();

            // Limit 
            if(fromPage != null && limiter != null)
            {
                connectedDatesData = connectedDatesData.Skip(fromPage.Value * limiter.Value).Take(limiter.Value).ToList();
            }

            return connectedDatesData;
        }
    }
}