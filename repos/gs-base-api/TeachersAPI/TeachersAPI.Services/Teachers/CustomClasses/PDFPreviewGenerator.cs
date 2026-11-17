using NReco.PdfGenerator;
using System;
using PdfSharp;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using TeachersAPI.TeachersAPI.Services.Lessons.CustomClasses.HTMLSanitizer;
using TeachersAPI.Domain.Models.Teachers.Preview;
using System.Diagnostics;
using System.Globalization;

namespace TeachersAPI.TeachersAPI.Services.Teachers.CustomClasses
{
    /// <summary>
    /// PDF Preview Generator that supports multiple pages for long plannings
    /// </summary>
    public class PDFPreviewGenerator
    {
        /// <summary>
        /// Generates a multi-page PDF for teacher plannings
        /// </summary>
        /// <param name="teacherPlanningPreviewPdf">Data to create the planning</param>
        /// <returns>Base64 string of the PDF</returns>
        public static string GeneratePreviewPdf(TeacherPlanningPreviewPdfData teacherPlanningPreviewPdf)
        {
            try
            {
                if (teacherPlanningPreviewPdf?.Days == null || teacherPlanningPreviewPdf.Days.Count == 0)
                {
                    return "No data available for PDF generation.";
                }
            
                // Show 7 days per page
                int daysPerPage = 7;

                int totalDays = teacherPlanningPreviewPdf.Days.Count;
                int totalPages = (int)Math.Ceiling((double)totalDays / daysPerPage);

                // Find body tags position
                int bodyStart = htmlTemplate.IndexOf("<body>");
                int bodyEnd = htmlTemplate.IndexOf("</body>");

                // Get the content before the body (styles, tags)
                string htmlTemplateHeader = htmlTemplate.Substring(0, bodyStart + "<body>".Length);

                // Get the content inside the body
                string pageTemplate = htmlTemplate.Substring(bodyStart + "<body>".Length, bodyEnd - (bodyStart + "<body>".Length));

                // Get footer tags from template
                string htmlTemplateFooter = htmlTemplate.Substring(bodyEnd);

                // Create a single HTML document
                StringBuilder completeHtml = new StringBuilder();

                completeHtml.Append(htmlTemplateHeader);

                for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
                {
                    int startIndex = pageIndex * daysPerPage;
                    int endIndex = Math.Min(startIndex + daysPerPage, totalDays);

                    List<DataDay> pageDays = teacherPlanningPreviewPdf.Days.Skip(startIndex).Take(endIndex - startIndex).ToList();

                    string pageTitle = $"Página {pageIndex + 1} de {totalPages} (Días {startIndex + 1}-{endIndex})";

                    var htmlContent = pageTemplate.Replace("{{{header}}}", BuilderHeader(teacherPlanningPreviewPdf));
                    htmlContent = htmlContent.Replace("{{{tableHeader}}}", BuildTableHeaderForPage(pageDays, daysPerPage));

                    string lessonRow = BuildRowForPage(pageDays, "DataLessons", daysPerPage);
                    string standardRow = BuildRowForPage(pageDays, "DataStandards", daysPerPage);
                    string expectationsRow = BuildRowForPage(pageDays, "DataExpectations", daysPerPage);
                    string stratergiesRow = BuildRowForPage(pageDays, "DataStratergies", daysPerPage);
                    string objetivesRow = BuildRowForPage(pageDays, "DataObjetives", daysPerPage);
                    string transversalThemesRow = BuildRowForPage(pageDays, "DataTransversalThemes", daysPerPage);
                    string introductoryActivityRow = BuildRowForPage(pageDays, "DataIntroductoryActivity", daysPerPage);
                    string developmentActivityRow = BuildRowForPage(pageDays, "DataDevelopmentActivity", daysPerPage);
                    string closingActivityRow = BuildRowForPage(pageDays, "DataClosingActivity", daysPerPage);
                    string otherIntegrationMaterialsRow = BuildRowForPage(pageDays, "DataOtherSubjectIntegrations", daysPerPage);
                    string initiativeProjectInnovationsRow = BuildRowForPage(pageDays, "DataInitiativeProjectInnovations", daysPerPage);
                    string evaluationsRow = BuildRowForPage(pageDays, "DataEvaluations", daysPerPage);
                    string accommodationsRow = BuildRowForPage(pageDays, "DataAccommodations", daysPerPage);
                    string materialsRow = BuildRowForPage(pageDays, "DataMaterials", daysPerPage);
                    string assignmentsRow = BuildRowForPage(pageDays, "DataAssignments", daysPerPage);
                    string customStrategiesRow = BuildRowForPage(pageDays, "DataCustomStrategies", daysPerPage);
                    string praxisReflectionsRow = BuildRowForPage(pageDays, "DataPraxisReflections", daysPerPage);

                    bool hasContent = false;
                    foreach (var day in pageDays)
                    {
                        if ((day.DataLessons?.Count > 0) ||
                            (day.DataStandards?.Count > 0) ||
                            (day.DataExpectations?.Count > 0) ||
                            (day.DataStratergies?.Count > 0) ||
                            (day.DataObjetives?.Count > 0) ||
                            (day.DataTransversalThemes?.Count > 0) ||
                            (day.DataIntroductoryActivity?.Count > 0) ||
                            (day.DataDevelopmentActivity?.Count > 0) ||
                            (day.DataClosingActivity?.Count > 0) ||
                            (day.DataOtherSubjectIntegrations?.Count > 0) ||
                            (day.DataInitiativeProjectInnovations?.Count > 0) ||
                            (day.DataEvaluations?.Count > 0) ||
                            (day.DataAccommodations?.Count > 0) ||
                            (day.DataMaterials?.Count > 0) ||
                            (day.DataAssignments?.Count > 0) ||
                            (day.DataCustomStrategies?.Count > 0) ||
                            (day.DataPraxisReflections?.Count > 0))
                        {
                            hasContent = true;
                            break;
                        }
                    }
                    // Handle row generation when days have no data
                    if (!hasContent)
                    {
                        StringBuilder emptyRows = new StringBuilder();

                        for (int row = 0; row < 8; row++)
                        {
                            emptyRows.Append("<tr class=\"RowActions\" style=\"height: 60px;\">");
                            emptyRows.Append("<th style=\"width: 110px;\">&nbsp;</th>");

                            for (int col = 0; col < daysPerPage; col++)
                            {
                                emptyRows.Append("<td>&nbsp;</td>");
                            }

                            emptyRows.Append("</tr>");
                        }

                        htmlContent = htmlContent.Replace("{{{rows}}}", emptyRows.ToString());
                    }
                    else
                    {
                        string rows =
                            CreateTableRowIfHasContent("Lecciones", lessonRow) +
                            CreateTableRowIfHasContent("Estándares", standardRow) +
                            CreateTableRowIfHasContent("Expectativas", expectationsRow) +
                            CreateTableRowIfHasContent("Estrategia académica", stratergiesRow) +
                            CreateTableRowIfHasContent("Objetivos", objetivesRow) +
                            CreateTableRowIfHasContent("Temas Transversales", transversalThemesRow) +
                            CreateTableRowIfHasContent("Actividad de inicio", introductoryActivityRow) +
                            CreateTableRowIfHasContent("Actividad de desarrollo", developmentActivityRow) +
                            CreateTableRowIfHasContent("Actividad de cierre", closingActivityRow) +
                            CreateTableRowIfHasContent("Integración con otras materias", otherIntegrationMaterialsRow) +
                            CreateTableRowIfHasContent("Iniciativa o proyecto innovador", initiativeProjectInnovationsRow) +
                            CreateTableRowIfHasContent("Evaluaciones (Avalúos)", evaluationsRow) +
                            CreateTableRowIfHasContent("Estrategia de instrucción diferenciada", accommodationsRow) +
                            CreateTableRowIfHasContent("Materiales", materialsRow) +
                            CreateTableRowIfHasContent("Asignaciones", assignmentsRow) +
                            CreateTableRowIfHasContent("Acomodos razonables", customStrategiesRow) +
                            CreateTableRowIfHasContent("Reflexión de la praxis", praxisReflectionsRow);

                        htmlContent = htmlContent.Replace("{{{rows}}}", rows);
                    }
                    htmlContent = htmlContent.Replace("<h2 class=\"heading-h2\">Plan de aprendizaje del maestro</h2>",
                        $"<h2 class=\"heading-h2\">Plan de aprendizaje del maestro - {pageTitle}</h2>");

                    // Add the content to the final HTML
                    completeHtml.Append(htmlContent);

                    // Add a page break for each planning
                    if (pageIndex < totalPages - 1)
                    {
                        completeHtml.Append("<div style='page-break-after: always;'></div>");
                    }
                }

                // Add HTML footer tags
                completeHtml.Append(htmlTemplateFooter);

                // Generate single PDF
                try
                {
                    var htmlToPdf = new HtmlToPdfConverter();

                    htmlToPdf.Size = NReco.PdfGenerator.PageSize.A4;
                    htmlToPdf.Orientation = NReco.PdfGenerator.PageOrientation.Landscape;
                    htmlToPdf.Margins = new PageMargins { Bottom = 10, Left = 20, Right = 20, Top = 10 };

                    // Convert HTML to PDF
                    byte[] pdfBytes = htmlToPdf.GeneratePdf(completeHtml.ToString());

                    if (pdfBytes != null && pdfBytes.Length > 0)
                    {
                        return Convert.ToBase64String(pdfBytes);
                    }
                    else
                    {
                        return "Failed to generate PDF";
                    }
                }
                catch (Exception ex)
                {
                    return $"Error generating PDF: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static string BuilderHeader(TeacherPlanningPreviewPdfData pDFData)
        {
            HashSet<string> uniqueUnits = new HashSet<string>();

            // Render unique units
            foreach (var day in pDFData.Days)
            {
                if (day.DataUnits != null)
                {
                    foreach (var unit in day.DataUnits)
                    {
                        if (!string.IsNullOrEmpty(unit.Description))
                        {
                            uniqueUnits.Add(unit.Description);
                        }
                    }
                }
            }
            string unitsList = string.Join(", ", uniqueUnits);

            string header = $"<li><span>Maestro: </span>{pDFData.TeacherFullName}</li>" +
                            $"<li><span>Plan semanal, Semana #{pDFData.WeekNumber} - Rango:</span> {pDFData.DateRange}</li>" +
                            (!string.IsNullOrWhiteSpace(pDFData.GroupStudent) ? $"<li><span>Grupo de estudiantes: </span>{pDFData.GroupStudent}</li>" : "") +
                            $"<li><span>Materia y grado:</span> {pDFData.SubjectAndGrade}</li>" +
                            (!string.IsNullOrWhiteSpace(unitsList) ? $"<li><span>Unidades:</span> {unitsList}</li>" : "");
            return header;
        }

        static string BuildTableHeaderForPage(List<DataDay> days, int daysPerPage = 7)
        {
            StringBuilder stringBuilder = new StringBuilder();

            days.ToList().ForEach(x =>
            {
                string dayOfWeek = x.Date.ToString("dddd", new CultureInfo("es-MX"));
                dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);

                var html = "<th scope=\"col\">" +
                           $"<div style=\"text-align: center; padding: 5px;\">" +
                           $"<strong>{dayOfWeek}</strong> <br/>" +
                           $"{x.FormatedDate}" +
                           $"</div>" +
                           "</th>";

                stringBuilder.AppendLine(html);
            });

            // Add empty columns if the last pdf has less than 7 days
            int emptyColumnsNeeded = daysPerPage - days.Count;
            for (int i = 0; i < emptyColumnsNeeded; i++)
            {
                stringBuilder.AppendLine("<th scope=\"col\"><div style=\"text-align: center; padding: 5px;\">&nbsp;</div></th>");
            }

            return stringBuilder.ToString();
        }

        static string BuildRowForPage(List<DataDay> days, string rowData, int daysPerPage = 7)
        {
            StringBuilder stringBuilder = new StringBuilder();

            bool hasContent = false;

            days.ForEach(day =>
            {
                StringBuilder contentBuilder = new StringBuilder();

                if (day[rowData] != null && day[rowData].Count > 0)
                {
                    hasContent = true;

                    day[rowData].ForEach(data =>
                    {
                        contentBuilder.AppendLine(CreateItem(data.Title, data.Description, data.Type, data.SecundaryDescription));
                    });
                }

                stringBuilder.AppendLine(CreateRow(contentBuilder.ToString()));
            });

            // Add empty row if the last pdf has less than 7 days
            int emptyColumnsNeeded = daysPerPage - days.Count;
            for (int i = 0; i < emptyColumnsNeeded; i++)
            {
                stringBuilder.AppendLine(CreateRow(""));
            }

            return hasContent ? stringBuilder.ToString() : string.Empty;
        }

        static string CreateItem(string title, string description, string type, string secundaryDescription = null)
        {
            string secundaryDescriptionHtml = string.IsNullOrEmpty(secundaryDescription) ? "" : $"<p class=\"list--item_desc--desc list--item_secondTitle \">{secundaryDescription}</p>";
            string titleClass;
            if (type == DataInfo.CustomLessonType)
            {
                titleClass = "list--item_secondTitle";

            }
            else
            {
                titleClass = "list--item_firstTitle";
            }

            return "<li class=\"tablePDF__list--item\">" +
                                        "<div class=\"tablePDF__list--item_desc\">" +
                                            $"<p class=\"list--item_desc--title {titleClass}\">{title}</p>" +
                                            $"<p class=\"list--item_desc--desc\">{description}</p>" +
                                            secundaryDescriptionHtml +
                                        "</div>" +
                                    "</li>";
        }

        static string CreateRow(string items)
        {
            return @"<td>
                <div class=""tablePDF__container-noEdition"">
                    <ul class=""tablePDF__list"">
                        {{{items}}}
                    </ul>
                </div>
            </td>".Replace("{{{items}}}", items);
        }

        static string CreateTableRowIfHasContent(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "";
            }

            return $@"
                <tr class=""RowActions"">
                    <th>{title}</th>
                    {content}
                </tr>";
        }

        #region Template HTML

        private static string htmlTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
    <head>
        <meta charset=""UTF-8"" />
        <style>
           /* LAYOUT */

body {
    font-family: 'Montserrat', Arial, Helvetica, sans-serif;
    margin: 0;
    color: #33353d;
    font-size: 18px;
}

a {
    color: #4f57a7;
    text-decoration: none;
}
a:hover {
    color: #868686;
}

:focus {
    outline: none;
}

.globalContent {
    position: relative;
    margin-top: 5px;
    padding: 1em;
}

.heading-h1 {
    color: #1a7ba9;
    font-weight: 800;
    font-size: 22px;
}

.heading-h2 {
    color: #1a7ba9;
    font-weight: 800;
    font-size: 20px;
}

.wrapper-twoCol-highlight {
    /* CSS3: display: flex; */
    /* CSS2: Use alternative layout methods like tables or floats */
    /* In CSS2, achieving the exact behavior of flexbox requires different approaches */
    width: 100%;
    margin-bottom: 10px;
}

/* Content Planning width Sidebar */
.containerPlanning {
    display: block; /* Use block-level elements for layout */
    width: 100%;
    /* gap: 30px; Not directly supported in CSS2 */
}

.containerPlanning__content {
    /* CSS3: flex: 1 1 auto; */
    /* CSS2: Use floats or percentages to achieve similar flexible layout */
}

/* LIST INFORMATION */

.infoList {
    display: block; /* Use block-level elements for layout */
    /* flex-wrap: wrap; Not directly supported in CSS2 */
    list-style-type: none;
    margin: 0;
    padding: 0;
    font-size: 12px;
    font-weight: normal;
    color: #424242;
    margin: 8px 0;
}

.infoList li:not(:last-child)::after {
    /* CSS3: Pseudo-class :not(), :last-child, ::after */
    /* CSS2: Achieving similar effect might require manual spacing or using background images */
    display: inline;
    margin-left: 1rem;
}

.infoList span {
    font-weight: bold;
}

/* TABLE PDF */

.wrapper-tablePDF {
    /* CSS3: display: grid; */
    /* CSS2: Use alternative layout methods like tables or absolute positioning */
}

.tablePDF__scroll {
    position: relative;
    width: 100%;
    z-index: 1;
    margin: auto;
}

.tablePDF__scroll .tablePDF__content {
    width: 100%;
    /* min-width: 1280px; Not directly supported in CSS2 */
    margin: auto;
    border-collapse: separate;
    /* border-spacing: 2px; Not directly supported in CSS2 */
    font-size: 9px;
}

.tablePDF__scroll .tablePDF__content th,
.tablePDF__scroll .tablePDF__content td {
    padding: 0;
    border: 1px solid #eeefef;
    vertical-align: top;
    text-align: left;
    background-color: #dee6f2;  
    min-width: 100px; 
}

.tablePDF__scroll .tablePDF__content td {
    background-color: #fff;
}

.tablePDF__scroll .tablePDF__content td:hover {
    border-color: #4f57a7;
}

.tablePDF__scroll .tablePDF__content thead th {
    color: #384167;
    /* position: -webkit-sticky; Not supported in CSS2 */
    /* position: sticky; Not supported in CSS2 */
    /* top: 0; Not supported in CSS2 */
    width: 70px;
    padding: 5px 10px;
}
thead {
    display: table-row-group !important;
}
.tablePDF__content th:first-child {
    /* position: -webkit-sticky; Not supported in CSS2 */
    /* position: sticky; Not supported in CSS2 */
    padding: 5px 10px;
    /* left: 0; Not supported in CSS2 */
    z-index: 2;
    color: #384167;
    background: #dee6f2;
}

.tablePDF__content thead th:first-child {
    /* z-index: 5; Not supported in CSS2 */
    background-color: #dee6f2;
}

/* INISDE ATRIBUTES */
.tablePDF__infoDate {
    display: block; /* Use block-level elements for layout */
    /* justify-content: space-between; Not directly supported in CSS2 */
    /* align-items: center; Not directly supported in CSS2 */
    padding: 5px 0;
}

.tablePDF__infoDate--day {
    text-transform: uppercase;
    font-weight: bold;
    color: #000000;
}

.tablePDF__infoDate--date {
    font-weight: 500;
}

.tablePDF__container {
    /* CSS3: display: grid; */
    /* CSS2: Use alternative layout methods like tables or absolute positioning */
    height: 100%;
}

.tablePDF__container-noEdition {
    /* CSS3: display: grid; */
    /* CSS2: Use alternative layout methods like tables or absolute positioning */
    height: 100%;
    margin-top: 10px;
}

.tablePDF__list {
    margin: 0;
    padding: 0;
    list-style-type: none;
}

.tablePDF__list--item_desc {
    padding: 0 8px 8px 8px;
}

.tablePDF__list--item_desc p {
    margin: 0;
}

.list--item_desc--title {
    font-size: 10px;
    font-weight: bold;
}

.list--item_firstTitle {
    color: #c3902a;
}
.list--item_secondTitle {
    color: #809cb7;
}
.list--item_neutralTitle {
    color: #868686;
}

.list--item_desc--desc_test {
    font-weight: bold;
}
.list--item_desc--desc_test-date {
    font-size: 11px;
}

.tablePDF__container-noEdition li {
    border-bottom: 1px solid #eeefef;
    margin-bottom: 5px;
}

.tablePDF__container-noEdition li:last-child {
    border-bottom: none;
    margin-bottom: 0;
}

.list--item_desc--desc p {
    margin-bottom: 8px;
}

        </style>
    </head>

    <body>
        <div class=""globalContent"">
            <div class=""wrapper-twoCol-highlight"">
                <div class=""col""><h1 class=""heading-h1"">Planificación</h1></div>
            </div>

            <div class=""Region-no-border"">
                <nav class=""infoList"">
                    {{{header}}}
                </nav>
            </div>

            <div class=""containerPlanning"">
                <!-- Contenido central -->
                <div class=""containerPlanning__content space-margin-top-regular"">
                    <div>
                        <h2 class=""heading-h2"">Plan de aprendizaje del maestro</h2>
                    </div>

                    <div class=""wrapper-tablePDF"">
                        <div class=""tablePDF__scroll"">
                            <table class=""tablePDF__content"">
                                <thead>
                                    <tr>
                                        <th scope=""col"">
                                        {{{tableHeader}}}
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {{{rows}}}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <!-- Fin cotenido central -->
            </div>
        </div>
    </body>
</html>
";
        #endregion
    }
}