using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.DTOs;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.Responses;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using DataAnalysisAPI.Utils;
using DataAnalysisAPI;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnalysisAPI.DataAnalysisAPI.Services
{
    /// <summary>
    /// To handle the details related to the results.
    /// </summary>
    public static class Results
    {
        /// <summary>
        /// To get the results of the students. 
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user is a director (principal), else false.</param>
        /// <param name="isStaff">true if the user is staff (admin, admin ore), else false.</param>
        /// <param name="isStudent">true if the user making the request is a student, else false.</param>
        /// <param name="isTeacher">true if the user making the request is a teacher, else false.</param>
        /// <param name="sie">Optional: The SIE of the student.</param>
        /// <param name="schoolIds">Optional: A list of schools.</param>
        /// <param name="userIds">Optional: A list of users.</param>
        /// <param name="oreId">Optional: The ORE Id.</param>
        /// <param name="subjectCodes">Optional: The subject codes.</param>
        /// <param name="levelCodes">Optional: The level codes.</param>
        /// <param name="fromDatePeriod">The from date for the period.</param>
        /// <param name="toDatePeriod">The to date for the period.</param>
        /// <param name="fromPage">The from page.</param>
        /// <param name="limiter">How many items per page.</param>
        /// <returns>Returns the response model with the result generic.</returns>
        public static ResponseModel<ResultGenericModel> GetGradeSubjectTotals(int userIdRequester, bool isDirector, 
            bool isStaff, bool isStudent, bool isTeacher, string sie = null,
            List<int> schoolIds = null, List<int> userIds = null, int? oreId = null,
            List<string> subjectCodes = null, List<string> levelCodes = null,
            DateTime? fromDatePeriod = null, DateTime? toDatePeriod = null, 
            int? fromPage = 0, int? limiter = 10)
        {
            var results = PrepareDataForResults(userIdRequester, isDirector, isStaff, isStudent, isTeacher, sie, schoolIds, userIds, oreId, 
                subjectCodes, levelCodes, fromDatePeriod, toDatePeriod);
            int total = results.Count;

            // Skip by pagination 
            if (results.Count > 0)
            {
                results = results.Skip(fromPage.Value * limiter.Value).Take(limiter.Value).ToList();
            }

            return new ResponseModel<ResultGenericModel>()
            {
                Count = total,
                Results = results
            };
        }

        /// <summary>
        /// To get the results of the students (the excel document).
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user is a director (principal), else false.</param>
        /// <param name="isStaff">true if the user is staff (admin, admin ore), else false.</param>
        /// <param name="isStudent">true if the user making the request is a student, else false.</param>
        /// <param name="isTeacher">true if the user making the request is a teacher, else false.</param>
        /// <param name="sie">Optional: The SIE of the student.</param>
        /// <param name="schoolIds">Optional: A list of schools.</param>
        /// <param name="userIds">Optional: A list of users.</param>
        /// <param name="oreId">Optional: The ORE Id.</param>
        /// <param name="subjectCodes">Optional: The subject codes.</param>
        /// <param name="levelCodes">Optional: The level codes.</param>
        /// <param name="fromDatePeriod">The from date for the period.</param>
        /// <param name="toDatePeriod">The to date for the period.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>Returns the response model with the result generic.</returns>
        public static async Task<FileResponseModel> GetGradeSubjectTotalsDocument(int userIdRequester, bool isDirector,
        bool isStaff, bool isStudent, bool isTeacher, string sie = null,
        List<int> schoolIds = null, List<int> userIds = null, int? oreId = null,
        List<string> subjectCodes = null, List<string> levelCodes = null,
        DateTime? fromDatePeriod = null, DateTime? toDatePeriod = null, string languageCode = "es")
        {
            // Set the generic results.
            var results = PrepareDataForResults(userIdRequester, isDirector, isStaff, isStudent, isTeacher, sie, schoolIds, userIds, oreId,
                subjectCodes, levelCodes, fromDatePeriod, toDatePeriod);

            if (results.Count > 50000)
            {
                // Send message according to language. 
                return new FileResponseModel() { Message = AppConfiguration.Configuration["AppSettings:message_data_too_large_" + languageCode] };
            }

            // Set the averages by each student, first name, last name, subject, level and period.
            var averageScores = results
            .GroupBy(sp => new { sp.UserId, sp.Sie, sp.FirstName, sp.LastName, sp.SchoolIdentifier, sp.SchoolName,
                sp.City, sp.Ore, sp.StateIsoCode, sp.CountryIso, 
                sp.SubjectCode, sp.LevelCode, sp.PeriodId, sp.PeriodNo, sp.PeriodFrom, sp.PeriodTo, sp.AcademicYearStart, sp.AcademicYearEnd })
            .Select(group => new
            {
                group.Key.UserId,
                group.Key.Sie,
                group.Key.FirstName,
                group.Key.LastName,
                group.Key.SchoolIdentifier,
                group.Key.SchoolName,
                group.Key.City, 
                group.Key.Ore,
                group.Key.StateIsoCode,
                group.Key.CountryIso,
                group.Key.SubjectCode,
                group.Key.LevelCode,
                group.Key.PeriodId,
                group.Key.PeriodFrom,
                group.Key.PeriodTo,
                group.Key.PeriodNo,
                group.Key.AcademicYearStart,
                group.Key.AcademicYearEnd,
                AverageScore = group.Average(sp => sp.TotalPercentFullQuiz)
            });

            averageScores = averageScores.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.AcademicYearStart).ThenBy(x => x.PeriodNo);

            // Prepare the document.
            // Create a worksheet for user data
            // Create a new workbook
            IWorkbook workbook = new XSSFWorkbook();

            #region averages
            XSSFSheet totalsSheet = workbook.CreateSheet("Averages") as XSSFSheet;

            // Create White font for header
            IFont boldFontWhite = workbook.CreateFont();
            boldFontWhite.Boldweight = (short)FontBoldWeight.Bold;
            boldFontWhite.Color = IndexedColors.White.Index;
            ICellStyle boldFontWhiteStyle = workbook.CreateCellStyle();
            boldFontWhiteStyle.SetFont(boldFontWhite);

            // Set the header style
            XSSFCellStyle headerStyleCustom = (XSSFCellStyle)workbook.CreateCellStyle();
            headerStyleCustom.WrapText = true;
            headerStyleCustom.FillForegroundColor = IndexedColors.DarkGreen.Index;
            headerStyleCustom.FillPattern = FillPattern.SolidForeground;
            headerStyleCustom.SetFont(boldFontWhite);

            // Set the cell style (gray)
            ICellStyle grayRowStyle = workbook.CreateCellStyle();
            grayRowStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            grayRowStyle.FillPattern = FillPattern.SolidForeground;

            string[] headerValues = { "SIE (Student Record)", "First Name", "Last Name", "School Identifier", "School Name", 
            "City", "Ore", "State", "Country", "SubjectCode", "LevelCode", "Period No.", "Period From", "Period To",
            "Academic Year Start", "Academic Year End", "Average"};

            // Create headers for user data
            IRow totalDataRowHeader = totalsSheet.CreateRow(0);
            for(int i=0; i < headerValues.Length; i++)
            {
                totalDataRowHeader.CreateCell(i).SetCellValue(headerValues[i]);
                totalDataRowHeader.Cells[i].CellStyle = headerStyleCustom;
            }

            if (averageScores.Count() > 0)
            {
                int rowSheetNumber = 1; // 1, because the 0 is for the header
                foreach (var keyAndAverage in averageScores)
                {
                    IRow dataRow = totalsSheet.CreateRow(rowSheetNumber);
                    dataRow.CreateCell(0).SetCellValue(keyAndAverage.Sie);
                    dataRow.CreateCell(1).SetCellValue(keyAndAverage.FirstName);
                    dataRow.CreateCell(2).SetCellValue(keyAndAverage.LastName);
                    dataRow.CreateCell(3).SetCellValue(keyAndAverage.SchoolIdentifier);
                    dataRow.CreateCell(4).SetCellValue(keyAndAverage.SchoolName);
                    dataRow.CreateCell(5).SetCellValue(keyAndAverage.City);
                    dataRow.CreateCell(6).SetCellValue(keyAndAverage.Ore);
                    dataRow.CreateCell(7).SetCellValue(keyAndAverage.StateIsoCode);
                    dataRow.CreateCell(8).SetCellValue(keyAndAverage.CountryIso);
                    dataRow.CreateCell(9).SetCellValue(keyAndAverage.SubjectCode);
                    dataRow.CreateCell(10).SetCellValue(keyAndAverage.LevelCode);
                    dataRow.CreateCell(11).SetCellValue(keyAndAverage.PeriodNo.ToString());
                    dataRow.CreateCell(12).SetCellValue(keyAndAverage.PeriodFrom.ToString());
                    dataRow.CreateCell(13).SetCellValue(keyAndAverage.PeriodTo.ToString());
                    dataRow.CreateCell(14).SetCellValue(keyAndAverage.AcademicYearStart.ToString());
                    dataRow.CreateCell(15).SetCellValue(keyAndAverage.AcademicYearEnd.ToString());
                    ICell cell = dataRow.CreateCell(16);
                    cell.SetCellValue(double.Parse(double.Parse(keyAndAverage.AverageScore.ToString()).ToString("0.00")));
                    if (rowSheetNumber % 2 == 0)
                    {
                        dataRow.RowStyle = grayRowStyle;
                    }

                    rowSheetNumber++;
                }


                // Table 
                int rowCount = rowSheetNumber; // Include the header row
                int columnCount = headerValues.Length;
                CellRangeAddress tableRange = new CellRangeAddress(0, rowCount - 1, 0, columnCount - 1);
                totalsSheet.SetAutoFilter(tableRange);
            }
            #endregion averages

            #region details
            XSSFSheet detailsSheet = workbook.CreateSheet("Details") as XSSFSheet;

            // Set the header style
            XSSFCellStyle headerStyleCustomDetails = (XSSFCellStyle)workbook.CreateCellStyle();
            headerStyleCustomDetails.WrapText = true;
            headerStyleCustomDetails.FillForegroundColor = IndexedColors.DarkTeal.Index;
            headerStyleCustomDetails.FillPattern = FillPattern.SolidForeground;
            headerStyleCustomDetails.SetFont(boldFontWhite);

            ICellStyle rowStyleDetails = workbook.CreateCellStyle();
            rowStyleDetails.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightCornflowerBlue.Index;
            rowStyleDetails.FillPattern = FillPattern.SolidForeground;

           
            string[] headerDetails = { "Sie", "First Name", "Last Name", "School Identifier", "School Name", "City", "Ore", "State",
                "Country", "Subject Code", "Level Code", "Assignment Title", "Assignment Taken Date", "Assignment Due Date", 
                "Assignment Assigned Date", "Score", "Plan Name", "Plan Closed Date",
                "Period From", "Period To", "Period No", "Academic Year Start", "Academic Year End", "Note" };

            // Create headers for user data
            IRow dataRowHeaderDetails = detailsSheet.CreateRow(0);
            for (int i = 0; i < headerDetails.Length; i++)
            {
                dataRowHeaderDetails.CreateCell(i).SetCellValue(headerDetails[i]);
                dataRowHeaderDetails.Cells[i].CellStyle = headerStyleCustomDetails;
            }

            if (results.Count() > 0)
            {
                int rowSheetNumberDetails = 1; // 1, because the 0 is for the header
                foreach (var resultDetail in results)
                {
                    IRow dataRow = detailsSheet.CreateRow(rowSheetNumberDetails);
                    dataRow.CreateCell(0).SetCellValue(resultDetail.Sie);
                    dataRow.CreateCell(1).SetCellValue(resultDetail.FirstName);
                    dataRow.CreateCell(2).SetCellValue(resultDetail.LastName);
                    dataRow.CreateCell(3).SetCellValue(resultDetail.SchoolIdentifier);
                    dataRow.CreateCell(4).SetCellValue(resultDetail.SchoolName);
                    dataRow.CreateCell(5).SetCellValue(resultDetail.City);
                    dataRow.CreateCell(6).SetCellValue(resultDetail.Ore);
                    dataRow.CreateCell(7).SetCellValue(resultDetail.StateIsoCode);
                    dataRow.CreateCell(8).SetCellValue(resultDetail.CountryIso);
                    dataRow.CreateCell(9).SetCellValue(resultDetail.SubjectCode);
                    dataRow.CreateCell(10).SetCellValue(resultDetail.LevelCode);
                    dataRow.CreateCell(11).SetCellValue(resultDetail.QuizTitle);
                    dataRow.CreateCell(12).SetCellValue(resultDetail.QuizTakenDate.ToString());
                    dataRow.CreateCell(13).SetCellValue(resultDetail.QuizDueDate.ToString());
                    dataRow.CreateCell(14).SetCellValue(resultDetail.QuizAssignedToPlanDate.ToString());
                    dataRow.CreateCell(15).SetCellValue(double.Parse(resultDetail.TotalPercentFullQuiz.ToString()));
                    dataRow.CreateCell(16).SetCellValue(resultDetail.PlanName);
                    dataRow.CreateCell(17).SetCellValue(resultDetail.PlanClosedDate.ToString());
                    dataRow.CreateCell(18).SetCellValue(resultDetail.PeriodFrom.ToString());
                    dataRow.CreateCell(19).SetCellValue(resultDetail.PeriodTo.ToString());
                    dataRow.CreateCell(20).SetCellValue(resultDetail.PeriodNo.ToString());
                    dataRow.CreateCell(21).SetCellValue(resultDetail.AcademicYearStart.ToString());
                    dataRow.CreateCell(22).SetCellValue(resultDetail.AcademicYearEnd.ToString());
                    dataRow.CreateCell(23).SetCellValue(resultDetail.Note);

                    if (rowSheetNumberDetails % 2 == 0)
                    {
                        dataRow.RowStyle = rowStyleDetails;
                    }

                    rowSheetNumberDetails++;
                }

                // Table 
                int rowCountDetails = rowSheetNumberDetails; // Include the header row
                int columnCountDetails = headerDetails.Length;
                CellRangeAddress tableRangeDetails = new CellRangeAddress(0, rowCountDetails - 1, 0, columnCountDetails - 1);
                detailsSheet.SetAutoFilter(tableRangeDetails);
            }
            #endregion details 


            return new FileResponseModel()
            {
                File = await ExcelGenerator.GenerateExcel(workbook, "Average" + System.Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss"))
            };

        }

        /// <summary>
        /// To prepare the annual score.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">If the user making the request is a principal (director).</param>
        /// <param name="isStaff">If the user making the request is a Staff.</param>
        /// <param name="isStudent">If the user making the request is a student.</param>
        /// <param name="studentId">The student that we want to get the results.</param>
        /// <param name="academicYearId">The academic year id.</param>
        /// <returns>Returns the student score report.</returns>
        public static StudentScoreReportModel PrepareStudentAnnualScore(int userIdRequester, bool isDirector,
        bool isStaff, bool isStudent, int studentId, int academicYearId)
        {
            // Set the generic results.
            var results = PrepareDataForResults(userIdRequester, isDirector, isStaff, isStudent, false, null, null, new List<int> { studentId },
                null, null, null, null, null, academicYearId);

            // Set the averages by each student, first name, last name, subject, level and period.
            var averageScores = results
            .GroupBy(sp => new
            {
                sp.UserId,
                sp.Sie,
                sp.FirstName,
                sp.LastName,
                sp.SchoolIdentifier,
                sp.SchoolName,
                sp.City,
                sp.Ore,
                sp.StateIsoCode,
                sp.CountryIso,
                sp.SubjectCode,
                sp.LevelCode,
                sp.PeriodId,
                sp.PeriodNo,
                sp.PeriodFrom,
                sp.PeriodTo,
                sp.AcademicYearStart,
                sp.AcademicYearEnd,
                sp.AcademicYearId
            })
            .Select(group => new
            {
                group.Key.UserId,
                group.Key.Sie,
                group.Key.FirstName,
                group.Key.LastName,
                group.Key.SchoolIdentifier,
                group.Key.SchoolName,
                group.Key.City,
                group.Key.Ore,
                group.Key.StateIsoCode,
                group.Key.CountryIso,
                group.Key.SubjectCode,
                group.Key.LevelCode,
                group.Key.PeriodId,
                group.Key.PeriodFrom,
                group.Key.PeriodTo,
                group.Key.PeriodNo,
                group.Key.AcademicYearStart,
                group.Key.AcademicYearEnd,
                group.Key.AcademicYearId,
                AverageScore = group.Average(sp => sp.TotalPercentFullQuiz)
            });

            // Prepare the results in a general object, that includes the subject name, level code, 
            // first name, last name, all the previous keys, etc...
            StudentScoreReportModel studentScoreReportModel = new StudentScoreReportModel()
            {
                PeriodDetails = new List<PeriodDetailsModel>(),
                Cummulative = new List<CummulativeModel>()
            };

            try
            {
                studentScoreReportModel.FullName = results[0].FirstName + " " + results[0].LastName;
            }
            catch (Exception)
            {

            }
            try
            {
                studentScoreReportModel.SchoolName = results[0].SchoolName;
                studentScoreReportModel.SchoolIdentifier = results[0].SchoolIdentifier;
                studentScoreReportModel.City = results[0].City;
                studentScoreReportModel.Country = results[0].Country;
                studentScoreReportModel.StateIsoCode = results[0].StateIsoCode;
                studentScoreReportModel.Ore = results[0].Ore;
                studentScoreReportModel.AcademicYearStart = results[0].AcademicYearStart.Value;
                studentScoreReportModel.AcademicYearEnd = results[0].AcademicYearEnd.Value;
                studentScoreReportModel.Sie = results[0].Sie;

            }
            catch (Exception)
            {

            }

            List<SubjectModel> subjects = Infrastructure.Dapper.Generics.GetSubjects();

            foreach (var averageDetail in averageScores)
            {
                studentScoreReportModel.PeriodDetails.Add(new PeriodDetailsModel()
                {
                    AcademicYearId = averageDetail.AcademicYearId.Value,
                    UserId = averageDetail.UserId,
                    SubjectCode = averageDetail.SubjectCode,
                    SubjectName = subjects.Find(x => x.SubjectCode == averageDetail.SubjectCode).SubjectName,
                    LevelCode = averageDetail.LevelCode,
                    PeriodNo = averageDetail.PeriodNo.Value,
                    Score = double.Parse(averageDetail.AverageScore.ToString()),
                    Grade = GetGrade(averageDetail.AverageScore)
                });
            }

            // Calculate cummulative 
            // First, calculate the average 
            // Set the averages by each student, first name, last name, subject, level and period.
            if (studentScoreReportModel.PeriodDetails != null && studentScoreReportModel.PeriodDetails.Count > 0)
            {
                // First, calculate the averages
                var averageScoresCumulative = studentScoreReportModel.PeriodDetails
                .GroupBy(sp => new
                {
                    sp.UserId,
                    sp.SubjectCode,
                    sp.LevelCode,
                    sp.SubjectName,
                })
                .Select(group => new
                {
                    group.Key.UserId,
                    group.Key.SubjectCode,
                    group.Key.SubjectName,
                    group.Key.LevelCode,
                    AverageScore = group.Average(sp => sp.Score)
                });

                foreach (var averageDetail in averageScoresCumulative)
                {
                    studentScoreReportModel.Cummulative.Add(new CummulativeModel()
                    {
                        Score = averageDetail.AverageScore,
                        SubjectName = averageDetail.SubjectName,
                        SubjectCode = averageDetail.SubjectCode,
                        LevelCode = averageDetail.LevelCode,
                        Grade = GetGrade(float.Parse(averageDetail.AverageScore.ToString()))
                    });
                }
            }

            return studentScoreReportModel;
        }

        /// <summary>
        /// To generate a PDF with the user schools.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">If the user making the request is a principal (director).</param>
        /// <param name="isStaff">If the user making the request is a Staff.</param>
        /// <param name="isStudent">If the user making the request is a student.</param>
        /// <param name="studentId">The student that we want to get the results.</param>
        /// <param name="academicYearId">The academic year id.</param>
        /// <param name="logoUrl">The URL for the annual report.</param>
        /// <param name="langCode">The language code.</param>
        /// <returns>Returns the file response with the document's URL.</returns>
        public static async Task<FileResponseModel> GetStudentAnnualScoreDocument(int userIdRequester, bool isDirector,
        bool isStaff, bool isStudent, int studentId, int academicYearId, string logoUrl, string langCode)
        {
            var studentScoreReportModel = PrepareStudentAnnualScore(userIdRequester, isDirector, isStaff, isStudent, studentId, academicYearId);

            // Now, create the PDF. 
            string cssStyleSheet = "";

            cssStyleSheet += "<style type=\"text/css\">.average-header { display: -webkit-box; display: -ms-flexbox; display: flex; } ";
            cssStyleSheet += ".average-header__title { flex:1 1 auto; font-family: Roboto, Arial, 'Times New Roman', Times, serif; font-weight: normal; text-align: right; margin-top: 10px; color: #222; } ";
            cssStyleSheet += ".average-header__title p { margin: 0.25em; font-size: 11px; } .average-header__title h1 { margin: 0; font-size: 14px; text-transform: uppercase; } ";
            cssStyleSheet += ".average-header__logo { flex:0 1 auto; } .average-header__logo img { width: 125px; } ";
            cssStyleSheet += ".average-title { padding: 10px 0; margin: 30px auto 50px auto; text-align: center; border-bottom: 1px solid #DDD; font-size: 15px; font-family: Roboto, Arial, 'Times New Roman', Times, serif; color: #222; } ";
            cssStyleSheet += ".average-container { font-size: 12px; font-family: Roboto, Arial, 'Times New Roman', Times, serif; color: #222; } ";
            cssStyleSheet += ".average-container__table { border-collapse: collapse; border-bottom:1px solid #404040; width: 100%; margin-bottom: 20px; } ";
            cssStyleSheet += ".average-container__tit { background:#404040; text-align: left; color: white; font-size: 11px; font-family: Roboto, Arial, 'Times New Roman', Times, serif; font-weight: normal; } ";
            cssStyleSheet += ".average-container__table td { vertical-align: top; font-size: 12px; } .average-container__table td, .average-container__table th { border-bottom:1px solid #404040; padding: 10px 5px; } ";
            cssStyleSheet += ".average-container__table tfoot td { background: #CCC; padding: 15px 5px; } .average-container__table--dataEnd { border-collapse: collapse; width: 100%; } ";
            cssStyleSheet += ".average-container__table--scale { border-collapse: collapse; border:1px solid #404040; width: 100%; margin-bottom: 20px; } ";
            cssStyleSheet += ".average-container__table--scale td { vertical-align: top; font-size: 11px; } .average-container__table--scale td, .average-container__table--scale th { border-bottom:1px solid #404040; padding: 5px; } ";
            cssStyleSheet += ".average-container__subtit { font-weight: bold; text-transform: uppercase; }</style>";

            string htmlToGet = "<html>";

            // body
            htmlToGet += "<body>";
            if (!string.IsNullOrWhiteSpace(logoUrl))
            {
                htmlToGet += "<div class='average-header'>";
                htmlToGet += "<div class='average-header__logo'><img src='" + logoUrl + "'/></div>";
            }

            // Header title 
            htmlToGet += "<div class='average-header__title' style='position:absolute;right:0px;'>";
            htmlToGet += "<h1>" + studentScoreReportModel.FullName + "</h1>";
            htmlToGet += "<p>" + studentScoreReportModel.SchoolName + " (" + studentScoreReportModel.SchoolIdentifier + ")</p>";
            htmlToGet += "<p><small>" + studentScoreReportModel.City + " " + studentScoreReportModel.Country + " | " + 
                studentScoreReportModel.Ore + "</small></p>";
            htmlToGet += "<p><strong>" + studentScoreReportModel.Sie + "</strong></p>";
            htmlToGet += "<p><i>" + DateTime.Now.ToString() + "</i></p>";
            htmlToGet += "</div>";
            htmlToGet += "</div>";

            // Average title 
            htmlToGet += "<div class='average-title'>" + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "academic_period" && x.LangCode == langCode)?.Text + " <strong>" + studentScoreReportModel.AcademicYearStart.ToShortDateString() + "</strong> ";
            htmlToGet += "-" + " <strong>" + studentScoreReportModel.AcademicYearEnd.ToShortDateString() + "</strong> </div>";

            // Average container 
            htmlToGet += "<div class='average-container'>";
            htmlToGet += "<p>" + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "period_average" && x.LangCode == langCode)?.Text + "</p>";

            // First, we find each subject and add the period details (Score and period number).
            List<PDFSubjectScoreModel> scoreList = new List<PDFSubjectScoreModel>();
            foreach (var cummulative in studentScoreReportModel.Cummulative)
            {
                // Identify the subject and levels. 
                List<PeriodDetailsModel> subjectLevelsToAdd = studentScoreReportModel.PeriodDetails.FindAll(x => x.SubjectCode == cummulative.SubjectCode
                && x.LevelCode == cummulative.LevelCode);

                var scoreDetailsToAdd = new PDFSubjectScoreModel()
                {
                    SubjectLevel = cummulative.SubjectName + " " + cummulative.LevelCode,
                    PeriodScores = new List<PDFPeriodScoreModel>()
                };

                // Add the period details 
                foreach(var subjectLevelToAdd in subjectLevelsToAdd)
                {
                    scoreDetailsToAdd.PeriodScores.Add(new PDFPeriodScoreModel()
                    {
                        PeriodNo = subjectLevelToAdd.PeriodNo,
                        Score = subjectLevelToAdd.Score,
                        Grade = subjectLevelToAdd.Grade,
                    });
                }

                scoreList.Add(scoreDetailsToAdd);
            }
            string htmlAveragePerPeriod = "<table class='average-container__table'>";

            // Now, for each period
            // Prepare the period.
            int periodNo = 1;

            // Find the max period. 
            int maxPeriod = studentScoreReportModel.PeriodDetails.Max(x => x.PeriodNo);

            // Set the header 
            htmlAveragePerPeriod += "<thead>";
            htmlAveragePerPeriod += "<tr>";
            htmlAveragePerPeriod += "<th style='background-color:#404040;color:white;' align='left'> " + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "subject" && x.LangCode == langCode)?.Text +  " </th>";
            for (int i=1;i<=maxPeriod;i++)
            {
                htmlAveragePerPeriod += "<th style='background-color:#404040;color:white;' align='right'> " + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "period" && x.LangCode == langCode)?.Text + " " + i.ToString() + "</th>";
            }
            htmlAveragePerPeriod += "</tr></thead>";

            // Set the body 
            htmlAveragePerPeriod += "<tbody>";

            foreach (var subjectScorePeriod in scoreList)
            {
                htmlAveragePerPeriod += "<tr>";
                htmlAveragePerPeriod += "<td class='average-container__subtit'>";
                htmlAveragePerPeriod += subjectScorePeriod.SubjectLevel;
                htmlAveragePerPeriod += "</td>";

                while (periodNo <= maxPeriod)
                {
                    htmlAveragePerPeriod += "<td align='right'>";

                    // Find the period
                    var period = subjectScorePeriod.PeriodScores.Find(x => x.PeriodNo == periodNo);

                    if(period != null)
                    {
                        htmlAveragePerPeriod += period.Score.ToString("0.00") + "% <span>" + period.Grade + "</span>";
                    }
                    else
                    {
                        htmlAveragePerPeriod += "-"; 
                    }

                    htmlAveragePerPeriod += "</td>";
                    periodNo++;

                }

                // Reset period no to get the other subject details
                periodNo = 1;
                htmlAveragePerPeriod += "</tr>";
            }
            htmlAveragePerPeriod += "</tbody>";
            htmlAveragePerPeriod += "</table>";
            htmlToGet += htmlAveragePerPeriod;


            // Prepare the cummulative 
            string htmlCummulative = "<table class='average-container__table--dataEnd'>";
            htmlCummulative += "<tr><td><p> " + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "cummulative_average" && x.LangCode == langCode)?.Text + " </p>";
            htmlCummulative += "<table class='average-container__table'> <thead>";
            htmlCummulative += "<tr> <th style='background-color:#404040;color:white;' align='left'>" + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "subject" && x.LangCode == langCode)?.Text +"</th>";
            htmlCummulative += "<th style='background-color:#404040;color:white;' align='right'>" + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "cummulative" && x.LangCode == langCode)?.Text + "</th>";
            htmlCummulative += "</tr></thead>";
            htmlCummulative += "<tbody>";

            foreach (var cummulative in studentScoreReportModel.Cummulative)
            {
                htmlCummulative += "<tr>";
                htmlCummulative += "<td class='average-container__subtit'>" + cummulative.SubjectName + " " + cummulative.LevelCode + "</td>";
                htmlCummulative += "<td align='right'>" + cummulative.Score.ToString("0.00") + "%<span> " + cummulative.Grade.ToString() + "</span></td>";
                htmlCummulative += "</tr>";
            }
            htmlCummulative += "</tfoot></table>";
            htmlCummulative += "</td> <td width='15%'></td> <td valign='bottom' width='200'>";

            htmlToGet += htmlCummulative;

            // Footer overall grades 
            htmlToGet += "<table class='average-container__table--scale'> <thead> ";
            htmlToGet += "<tr>";
            htmlToGet += "<th style='background-color:#404040;color:white;' colspan='3'>" + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "scale" && x.LangCode == langCode)?.Text + "</th> </tr> </thead> ";
            htmlToGet += "<tbody> <tr> <td>A =</td> <td>100% - 90%</td> <td> " + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "excellent" && x.LangCode == langCode)?.Text + " </td> </tr>";
            htmlToGet += "<tr> <td>B =</td> <td>89% - 80%</td> <td> " + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "good" && x.LangCode == langCode)?.Text + " </td> </tr>";
            htmlToGet += "<tr> <td>C =</td> <td>79% - 70%</td> <td> " + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "satisfactory" && x.LangCode == langCode)?.Text + "</td> </tr>";
            htmlToGet += "<tr> <td>D =</td> <td>69% - 60%</td> <td>" + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "deficient" && x.LangCode == langCode)?.Text + "</td> </tr>";
            htmlToGet += "<tr> <td>F =</td> <td>59% o menos</td> <td>" + Configurations.GetPDFTextTranslations().Find(x => x.TextCode == "fail" && x.LangCode == langCode)?.Text + " </td> </tr>";
            htmlToGet += "</tbody> </table> </td> </tr> </table>";
            htmlToGet += "</div> </body> </html>";

            return new FileResponseModel()
            {
                File = await PdfHtmlGenerator.GeneratePdfAsync(htmlToGet, cssStyleSheet, studentScoreReportModel.Sie + "_" + System.Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss"))
            };
        }

        /// <summary>
        /// To get the grades by the score.
        /// </summary>
        /// <param name="score">The score to evaluate.</param>
        /// <returns>Returns the grade.</returns>
        private static string GetGrade(float? score)
        { 
            if(score == null)
            {
                return "-";
            }

            if (score.Value >= 90)
            {
                return "A";
            }
            else if (score.Value >= 80)
            {
                return "B";
            }
            else if (score.Value >= 70)
            {
                return "C";
            }
            else if (score.Value >= 60)
            {
                return "D";
            }
            else
            {
                return "F";
            }
        }

        /// <summary>
        /// To prepare the base data for the results.
        /// </summary>
        /// <param name="userIdRequester">The user id that is making the request.</param>
        /// <param name="isDirector">true if the user is a director (principal), else false.</param>
        /// <param name="isStaff">true if the user is staff (admin, admin ore), else false.</param>
        /// <param name="isStudent">If is is an student.</param>
        /// <param name="isTeacher">If the user making the request is a teacher, then this is true, else, false.</param>
        /// <param name="sie">Optional: The SIE of the student.</param>
        /// <param name="schoolIds">Optional: A list of schools.</param>
        /// <param name="userIds">Optional: A list of users.</param>
        /// <param name="oreId">Optional: The ORE Id.</param>
        /// <param name="subjectCodes">Optional: The subject codes.</param>
        /// <param name="levelCodes">Optional: The level codes.</param>
        /// <param name="fromDatePeriod">The from date for the period.</param>
        /// <param name="toDatePeriod">The to date for the period.</param>
        /// <param name="academicYearId">The id of the academic year.</param>
        /// <returns>Returns a list of results with the user full info.</returns>
        private static List<ResultGenericModel> PrepareDataForResults(int userIdRequester, bool isDirector, bool isStaff, bool isStudent, 
            bool isTeacher, string sie = null,
            List<int> schoolIds = null, List<int> userIds = null, int? oreId = null, 
            List<string> subjectCodes = null, List<string> levelCodes = null,
            DateTime? fromDatePeriod = null, DateTime? toDatePeriod = null, int? academicYearId = null)
        {
            if (oreId != null)
            {
                schoolIds = AutoFilters.GetOreSchoolIds(oreId.Value, schoolIds);
                if (schoolIds == null || schoolIds.Count == 0)
                {
                    return new List<ResultGenericModel>();
                }
            }

            // If director
            if (isDirector)
            {
                userIds = AutoFilters.GetUsersIdByDirectors(userIdRequester, userIds);
                if (userIds == null || userIds.Count == 0)
                {
                    return new List<ResultGenericModel>();
                }
            }

            // If non staff or non director
            if (!isStaff && !isDirector && !isStudent)
            {
                userIds = AutoFilters.GetUsersIdsByNonDirectorNonStaffUser(userIdRequester, userIds);
                if (userIds == null || userIds.Count == 0)
                {
                    return new List<ResultGenericModel>();
                }
            }

            // The student can call just their data.
            if (isStudent)
            {
                userIds = new List<int> { userIdRequester };
            }

            // Now call the results 
            if (isTeacher)
            {
                return DataAnalysisAPI.Infrastructure.Dapper.Results.GetResultGenerics(fromDatePeriod,
                    toDatePeriod, schoolIds, userIds, subjectCodes, levelCodes, sie, academicYearId, userIdRequester);
            }
            else
            {
                return DataAnalysisAPI.Infrastructure.Dapper.Results.GetResultGenerics(fromDatePeriod,
                    toDatePeriod, schoolIds, userIds, subjectCodes, levelCodes, sie, academicYearId);
            }
        }
    }
}