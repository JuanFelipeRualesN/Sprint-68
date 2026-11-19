using System;
using System.Collections.Generic;
using System.Linq;

namespace TeachersAPI.Domain.Models.Teachers.Preview
{
    /// <summary>
    /// 
    /// </summary>
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
}