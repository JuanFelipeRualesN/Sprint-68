using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// Builds the .xlsx workbook of a query: a Data sheet (one row per result) and a References sheet (where the figures come from).
    /// It is computed over the JSON of the response (the one the caller sees). Every text is written as text, never as a formula.
    /// </summary>
    public static class UsageInsightsExcelExporter
    {
        /// <summary>
        /// Content type of the workbook.
        /// </summary>
        public const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// Builds the workbook.
        /// </summary>
        /// <param name="response">The response JSON.</param>
        /// <param name="language">The language code of the titles and labels.</param>
        /// <returns>Returns the bytes of the .xlsx file.</returns>
        public static byte[] BuildWorkbook(JsonNode response, string language)
        {
            using var workbook = new XSSFWorkbook();
            var styles = new Styles(workbook);
            WriteData(workbook.CreateSheet(UsageInsightsMessages.Get(language, "UsageInsightsExcelSheetData")), workbook, styles, response, language);
            WriteReferences(workbook.CreateSheet(UsageInsightsMessages.Get(language, "UsageInsightsExcelSheetReferences")), styles, response, language);
            using var stream = new MemoryStream();
            workbook.Write(stream, leaveOpen: true);
            return stream.ToArray();
        }

        private sealed class Styles
        {
            public Styles(IWorkbook workbook)
            {
                var headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerFont.Color = IndexedColors.White.Index;
                Header = workbook.CreateCellStyle();
                Header.SetFont(headerFont);
                Header.FillForegroundColor = IndexedColors.DarkBlue.Index;
                Header.FillPattern = FillPattern.SolidForeground;
                Header.WrapText = true;
                Header.VerticalAlignment = VerticalAlignment.Center;

                var boldFont = workbook.CreateFont();
                boldFont.IsBold = true;
                Bold = workbook.CreateCellStyle();
                Bold.SetFont(boldFont);
                Bold.VerticalAlignment = VerticalAlignment.Top;

                var format = workbook.CreateDataFormat();
                Text = workbook.CreateCellStyle();
                Text.DataFormat = format.GetFormat("@");
                Text.VerticalAlignment = VerticalAlignment.Top;
                Wrapped = workbook.CreateCellStyle();
                Wrapped.DataFormat = format.GetFormat("@");
                Wrapped.WrapText = true;
                Wrapped.VerticalAlignment = VerticalAlignment.Top;
                Decimal = workbook.CreateCellStyle();
                Decimal.DataFormat = format.GetFormat("0.0");
                Integer = workbook.CreateCellStyle();
                Integer.DataFormat = format.GetFormat("#,##0");
                Fraction = workbook.CreateCellStyle();
                Fraction.DataFormat = format.GetFormat("0.0##");
                LeftNumber = workbook.CreateCellStyle();
                LeftNumber.DataFormat = format.GetFormat("0.0##");
                LeftNumber.Alignment = HorizontalAlignment.Left;
                LeftNumber.VerticalAlignment = VerticalAlignment.Top;
            }

            public ICellStyle Header { get; }

            public ICellStyle Bold { get; }

            public ICellStyle Text { get; }

            public ICellStyle Wrapped { get; }

            public ICellStyle Decimal { get; }

            public ICellStyle Integer { get; }

            public ICellStyle Fraction { get; }

            public ICellStyle LeftNumber { get; }
        }

        private static void SetText(ICell cell, string value, ICellStyle style)
        {
            cell.SetCellValue(value);
            cell.CellStyle = style;
        }

        private static void WriteData(ISheet sheet, IWorkbook workbook, Styles styles, JsonNode response, string language)
        {
            var rows = response["Results"] as JsonArray ?? new JsonArray();
            var keys = new List<string>();   // order of appearance (the rows omit the nulls)
            foreach (var row in rows)
            {
                foreach (var (key, _) in UsageInsightsColumnFormatter.Flatten(row))
                {
                    if (!keys.Contains(key))
                    {
                        keys.Add(key);
                    }
                }
            }

            if (keys.Count == 0)
            {
                SetText(sheet.CreateRow(0).CreateCell(0), UsageInsightsColumnFormatter.BuildNoResultsMessage(response, language), styles.Text);
                sheet.SetColumnWidth(0, 60 * 256);
                return;
            }

            var headerRow = sheet.CreateRow(0);
            for (var column = 0; column < keys.Count; column++)
            {
                SetText(headerRow.CreateCell(column), UsageInsightsColumnFormatter.GetColumnTitle(keys[column], language), styles.Header);
            }

            var widths = keys.Select(key => UsageInsightsColumnFormatter.GetColumnTitle(key, language).Length).ToArray();
            for (var i = 0; i < rows.Count; i++)
            {
                var flat = UsageInsightsColumnFormatter.Flatten(rows[i]).ToDictionary(pair => pair.Key, pair => pair.Value);
                var excelRow = sheet.CreateRow(i + 1);
                for (var column = 0; column < keys.Count; column++)
                {
                    if (!flat.TryGetValue(keys[column], out var value))
                    {
                        continue;   // empty cell = no data (null)
                    }

                    var cell = excelRow.CreateCell(column);
                    if (UsageInsightsColumnFormatter.ToNumber(value) is { } number)
                    {
                        cell.SetCellValue(number);
                        cell.CellStyle = UsageInsightsColumnFormatter.IsPercentage(keys[column]) || keys[column].Contains("Variation", StringComparison.Ordinal)
                            ? styles.Decimal
                            : number % 1 == 0 ? styles.Integer : styles.Fraction;
                        widths[column] = Math.Max(widths[column], 10);
                    }
                    else
                    {
                        var text = UsageInsightsColumnFormatter.ToText(value, language);
                        SetText(cell, text, styles.Text);
                        if (i < 500)
                        {
                            widths[column] = Math.Max(widths[column], text.Length);
                        }
                    }
                }
            }

            for (var column = 0; column < keys.Count; column++)
            {
                sheet.SetColumnWidth(column, Math.Clamp(widths[column] + 2, 12, 60) * 256);
            }

            sheet.CreateFreezePane(0, 1);
            if (rows.Count > 0)
            {
                sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(0, rows.Count, 0, keys.Count - 1));
            }
        }

        private static void WriteReferences(ISheet sheet, Styles styles, JsonNode response, string language)
        {
            var rowIndex = 0;
            string Label(string key) => UsageInsightsMessages.Get(language, key);

            void WriteLine(string concept, string value)
            {
                var row = sheet.CreateRow(rowIndex++);
                SetText(row.CreateCell(0), concept, styles.Bold);
                SetText(row.CreateCell(1), value, styles.Wrapped);
            }

            // Numeric values (limit, exported rows, summary figures) go as NUMBERS, not as text.
            void WriteValue(string concept, JsonNode? value)
            {
                if (UsageInsightsColumnFormatter.ToNumber(value) is not { } number)
                {
                    WriteLine(concept, UsageInsightsColumnFormatter.ToText(value, language));
                    return;
                }

                var row = sheet.CreateRow(rowIndex++);
                SetText(row.CreateCell(0), concept, styles.Bold);
                var cell = row.CreateCell(1);
                cell.SetCellValue(number);
                cell.CellStyle = styles.LeftNumber;
            }

            void WriteTitle(string text)
            {
                var row = sheet.CreateRow(rowIndex++);
                SetText(row.CreateCell(0), text, styles.Header);
                SetText(row.CreateCell(1), string.Empty, styles.Header);
            }

            string FormatKeyLabel(string key) => UsageInsightsColumnFormatter.GetColumnTitle(key, language);

            var rowCount = (response["Results"] as JsonArray)?.Count ?? 0;
            WriteTitle(Label("UsageInsightsExcelReferencesTitle"));
            WriteLine(Label("UsageInsightsExcelQuery"), UsageInsightsColumnFormatter.ToText(response["Query"], language));
            WriteLine(Label("UsageInsightsExcelSource"), UsageInsightsColumnFormatter.ToText(response["Source"], language));
            WriteLine(Label("UsageInsightsExcelDateColumn"), UsageInsightsColumnFormatter.ToText(response["DateColumn"], language));
            foreach (var (key, value) in UsageInsightsColumnFormatter.Flatten(response["Period"], string.Empty))
            {
                WriteLine(key.Length == 0 ? Label("UsageInsightsExcelPeriod") : Label("UsageInsightsExcelPeriod") + " · " + key, UsageInsightsColumnFormatter.ToText(value, language));
            }

            foreach (var (key, value) in UsageInsightsColumnFormatter.Flatten(response["Filters"]))
            {
                if (key == "TestAccounts")
                {
                    WriteLine(Label("UsageInsightsExcelTestAccounts"), UsageInsightsColumnFormatter.ToText(value, language));
                }
                else
                {
                    WriteValue(Label("UsageInsightsExcelFilter") + " · " + FormatKeyLabel(key), value);
                }
            }

            if (response["Scope"] is { } scope)
            {
                WriteLine(Label("UsageInsightsExcelScope"), UsageInsightsColumnFormatter.ToText(scope, language));
            }

            WriteLine(Label("UsageInsightsExcelDataUntil"), response["DataUntil"] is { } dataUntil ? UsageInsightsColumnFormatter.ToText(dataUntil, language) : Label("UsageInsightsNotApplicable"));
            WriteValue(Label("UsageInsightsExcelRowsExported"), JsonValue.Create(rowCount));
            WriteLine(Label("UsageInsightsExcelCompleteList"), Label(UsageInsightsColumnFormatter.HasMoreRows(response) ? "UsageInsightsExcelCompleteListNo" : "UsageInsightsExcelCompleteListYes"));
            WriteLine(Label("UsageInsightsExcelGeneratedAt"), UsageInsightsColumnFormatter.ToText(response["GeneratedAt"], language));
            WriteLine(Label("UsageInsightsExcelNote"), UsageInsightsColumnFormatter.ToText(response["Note"], language) is { Length: > 0 } note ? note : Label("UsageInsightsExcelNoNote"));
            if (response["Notices"] is JsonArray { Count: > 0 } notices)
            {
                foreach (var notice in notices)
                {
                    WriteLine(Label("UsageInsightsExcelNotice"), UsageInsightsColumnFormatter.ToText(notice, language));
                }
            }

            if (response["Summary"] is JsonObject)
            {
                WriteTitle(Label("UsageInsightsExcelSummaryTitle"));
                foreach (var (key, value) in UsageInsightsColumnFormatter.Flatten(response["Summary"]))
                {
                    WriteValue(FormatKeyLabel(key), value);
                }
            }

            sheet.SetColumnWidth(0, 34 * 256);
            sheet.SetColumnWidth(1, 100 * 256);
        }
    }
}
