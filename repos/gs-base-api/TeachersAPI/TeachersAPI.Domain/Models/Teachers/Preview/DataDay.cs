using System;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Teachers.Preview
{
    /// <summary>
    /// 
    /// </summary>
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
                    case "DataUnits":return DataUnits;
                    case "DataAssignments": return DataAssignments;
                    case "DataAccommodations": return DataAccommodations;
                    case "DataEvaluations": return DataEvaluations;
                    case "DataInitiativeProjectInnovations": return DataInitiativeProjectInnovations;
                    case "DataMaterials": return DataMaterials;
                    case "DataOtherSubjectIntegrations": return DataOtherSubjectIntegrations;
                    case "DataPraxisReflections": return DataPraxisReflections;
                    case "DataCustomStrategies": return DataCustomStrategies;
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
}