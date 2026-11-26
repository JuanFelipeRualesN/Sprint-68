using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using WkHtmlToPdfDotNet;

namespace PDFGeneratorLibrary.Models
{
    public class TeacherPlanningPreviewPdfData
    {
        /// <summary>
        /// 
        /// </summary>
        public TeacherPlanningPreviewPdfData(DateTime open, DateTime close)
        {
            Days = DataDay.BuildDatesFromRange(open, close);
            DateRange = $"{open.ToString("MM/dd/yyyy")} al {close.ToString("MM/dd/yyyy")}";
        }
        /// <summary>
        /// 
        /// </summary>
        public int WeekNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DateRange { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string GroupStudent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TeacherFullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SubjectAndGrade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataDay> Days { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="lessonsData"></param>
        public void AddDataLessonToDay(DateTime day, DataInfo lessonsData)
        {
            AddDataToDay("DataLessons", day, lessonsData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="standardsData"></param>
        public void AddDataStandardsToDay(DateTime day, DataInfo standardsData)
        {
            AddDataToDay("DataStandards", day, standardsData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="stratergiesData"></param>
        public void AddDataStratergiesToDay(DateTime day, DataInfo stratergiesData)
        {
            AddDataToDay("DataStratergies", day, stratergiesData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="expectationsData"></param>
        public void AddDataExpectationsToDay(DateTime day, DataInfo expectationsData)
        {
            AddDataToDay("DataExpectations", day, expectationsData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="objectivesData"></param>
        public void AddDataObjetivesToDay(DateTime day, DataInfo objectivesData)
        {
            AddDataToDay("DataObjetives", day, objectivesData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="transversalThemesData"></param>
        public void AddDataTransversalThemesToDay(DateTime day, DataInfo transversalThemesData)
        {
            AddDataToDay("DataTransversalThemes", day, transversalThemesData);
        }
        /// <summary>
        /// Add introductory activity data to the specified day.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="introductoryActivityData"></param>
        public void AddDataIntroductoryActivityToDay(DateTime day, DataInfo introductoryActivityData)
        {
            AddDataToDay("DataIntroductoryActivity", day, introductoryActivityData);
        }
        /// <summary>
        /// Add development activity data to the specified day.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="developmentActivityData"></param>
        public void AddDataDevelopmentActivityToDay(DateTime day, DataInfo developmentActivityData)
        {
            AddDataToDay("DataDevelopmentActivity", day, developmentActivityData);
        }
        /// <summary>
        /// Add closing activity data to the specified day.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="closingActivityData"></param>
        public void AddDataClosingActivityToDay(DateTime day, DataInfo closingActivityData)
        {
            AddDataToDay("DataClosingActivity", day, closingActivityData);
        }
        /// <summary>
        /// Add lesson units data to be used in the PDF Header
        /// </summary>
        /// <param name="day"></param>
        /// <param name="unitsData"></param>
        public void AddDataUnitsToDay(DateTime day, DataInfo unitsData)
        {
            AddDataToDay("DataUnits", day, unitsData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="assignmentData"></param>
        public void AddDataAssignmentsToDay(DateTime day, DataInfo assignmentData)
        {
            AddDataToDay("DataAssignments", day, assignmentData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="accommodationData"></param>
        public void AddDataAccommodationsToDay(DateTime day, DataInfo accommodationData)
        {
            AddDataToDay("DataAccommodations", day, accommodationData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="evaluationData"></param>
        public void AddDataEvaluationsToDay(DateTime day, DataInfo evaluationData)
        {
            AddDataToDay("DataEvaluations", day, evaluationData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="initiativeProjectInnovationData"></param>
        public void AddDataInitiativeProjectInnovationsToDay(DateTime day, DataInfo initiativeProjectInnovationData)
        {
            AddDataToDay("DataInitiativeProjectInnovations", day, initiativeProjectInnovationData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="materialData"></param>
        public void AddDataMaterialsToDay(DateTime day, DataInfo materialData)
        {
            AddDataToDay("DataMaterials", day, materialData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="otherSubjectIntegrationData"></param>
        public void AddDataOtherSubjectIntegrationsToDay(DateTime day, DataInfo otherSubjectIntegrationData)
        {
            AddDataToDay("DataOtherSubjectIntegrations", day, otherSubjectIntegrationData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="praxisReflectionData"></param>
        public void AddDataPraxisReflectionsToDay(DateTime day, DataInfo praxisReflectionData)
        {
            AddDataToDay("DataPraxisReflections", day, praxisReflectionData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="customStrategyData"></param>
        public void AddDataCustomStrategiesToDay(DateTime day, DataInfo customStrategyData)
        {
            AddDataToDay("DataCustomStrategies", day, customStrategyData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="differentiationStrategyData"></param>
        public void AddDataDifferentiationStrategiesToDay(DateTime day, DataInfo differentiationStrategyData)
        {
            AddDataToDay("DataDifferentiationStrategies", day, differentiationStrategyData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="observationsData"></param>
        public void AddDataObservationsToDay(DateTime day, DataInfo observationsData)
        {
            AddDataToDay("DataObservations", day, observationsData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="commonExperiencesData"></param>
        public void AddDataCommonExperiencesToDay(DateTime day, DataInfo commonExperiencesData)
        {
            AddDataToDay("DataCommonExperiences", day, commonExperiencesData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="activeResponsesData"></param>
        public void AddDataActiveResponsesToDay(DateTime day, DataInfo activeResponsesData)
        {
            AddDataToDay("DataActiveResponses", day, activeResponsesData);
        }

        private void AddDataToDay(string dataName, DateTime day, DataInfo data)
        {
            var weekDay = Days.FirstOrDefault(x => x.Date.Date == day.Date);
            if (weekDay != null)
            {
                weekDay[dataName].Add(data);
            }
        }
    }

    public class DataDay
    {
        /// <summary>
        /// 
        /// </summary>
        public DataDay()
        {
            DataLessons = new List<DataInfo>();
            DataStandards = new List<DataInfo>();
            DataStratergies = new List<DataInfo>();
            DataExpectations = new List<DataInfo>();
            DataObjetives = new List<DataInfo>();
            DataTransversalThemes = new List<DataInfo>();
            DataIntroductoryActivity = new List<DataInfo>();
            DataDevelopmentActivity = new List<DataInfo>();
            DataClosingActivity = new List<DataInfo>();
            DataUnits = new List<DataInfo>();
            DataAssignments = new List<DataInfo>();
            DataAccommodations = new List<DataInfo>();
            DataEvaluations = new List<DataInfo>();
            DataInitiativeProjectInnovations = new List<DataInfo>();
            DataMaterials = new List<DataInfo>();
            DataOtherSubjectIntegrations = new List<DataInfo>();
            DataPraxisReflections = new List<DataInfo>();
            DataCustomStrategies = new List<DataInfo>();
            DataDifferentiationStrategies = new List<DataInfo>();
            DataObservations = new List<DataInfo>();
            DataCommonExperiences = new List<DataInfo>();
            DataActiveResponses = new List<DataInfo>();
        }
        /// <summary>
        /// 
        /// </summary>
        public int DayNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FormatedDate
        {
            get { return Date.ToString("MM/dd/yyyy"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataLessons { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataStandards { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataStratergies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataExpectations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataObjetives { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataTransversalThemes { get; set; }
        /// <summary>
        /// Introductory activity data
        /// </summary>
        public List<DataInfo> DataIntroductoryActivity { get; set; }
        /// <summary>
        /// Development activity data
        /// </summary>
        public List<DataInfo> DataDevelopmentActivity { get; set; }
        /// <summary>
        /// Closing activity data
        /// </summary>
        public List<DataInfo> DataClosingActivity { get; set; }
        /// <summary>
        /// Lesson units data
        /// </summary>
        public List<DataInfo> DataUnits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataAssignments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataAccommodations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataEvaluations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataInitiativeProjectInnovations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataMaterials { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataOtherSubjectIntegrations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataPraxisReflections { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataInfo> DataCustomStrategies { get; set; }

        public List<DataInfo> DataDifferentiationStrategies { get; set; }

        public List<DataInfo> DataObservations { get; set; }

        public List<DataInfo> DataCommonExperiences { get; set; }

        public List<DataInfo> DataActiveResponses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<DataInfo> this[string key]
        {
            get
            {
                switch (key)
                {
                    case "DataLessons": return DataLessons;
                    case "DataStandards": return DataStandards;
                    case "DataStratergies": return DataStratergies;
                    case "DataExpectations": return DataExpectations;
                    case "DataObjetives": return DataObjetives;
                    case "DataTransversalThemes": return DataTransversalThemes;
                    case "DataIntroductoryActivity": return DataIntroductoryActivity;
                    case "DataDevelopmentActivity": return DataDevelopmentActivity;
                    case "DataClosingActivity": return DataClosingActivity;
                    case "DataUnits": return DataUnits;
                    case "DataAssignments": return DataAssignments;
                    case "DataAccommodations": return DataAccommodations;
                    case "DataEvaluations": return DataEvaluations;
                    case "DataInitiativeProjectInnovations": return DataInitiativeProjectInnovations;
                    case "DataMaterials": return DataMaterials;
                    case "DataOtherSubjectIntegrations": return DataOtherSubjectIntegrations;
                    case "DataPraxisReflections": return DataPraxisReflections;
                    case "DataCustomStrategies": return DataCustomStrategies;
                    case "DataDifferentiationStrategies": return DataDifferentiationStrategies;
                    case "DataObservations": return DataObservations;
                    case "DataCommonExperiences": return DataCommonExperiences;
                    case "DataActiveResponses": return DataActiveResponses;
                    default: return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="open"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        public static List<DataDay> BuildDatesFromRange(DateTime open, DateTime close)
        {
            double days = (close - open).TotalDays;
            DateTime currentDate = open;
            var dataDays = new List<DataDay>();
            for (int i = 0; i <= days; i++)
            {
                dataDays.Add(new DataDay { Date = currentDate, DayNumber = i + 1 });
                currentDate = currentDate.AddDays(1);
            }

            return dataDays;
        }
    }

    public class DataInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public const string GenialLessonType = "Lección genial";
        /// <summary>
        /// 
        /// </summary>
        public const string CustomLessonType = "Lección libre";
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SecundaryDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; } = GenialLessonType;
    }
}
