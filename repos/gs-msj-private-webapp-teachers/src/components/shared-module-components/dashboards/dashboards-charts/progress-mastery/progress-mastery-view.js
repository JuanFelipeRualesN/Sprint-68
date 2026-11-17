import React from 'react'
import ReadingLevelChart from './reading-level-chart'
import ReadingLessonsResultsTable from './reading-lessons-results-table';
import UnitsCompletedChart from './units-completed-chart';
import SkillsProgressChart from './skills-progress-chart';
import UsageGapsChart from './usage-gap-chart';
import { withLocalize } from 'react-localize-redux';

const ProgressMasteryCharts = ({readingChartsData, skillsProgressData, unitMetrics, performanceMetrics, granularity, selectedPeriod, storyLanguage, onStoryLanguageChange, loading,
                                 readingLessonsSeries, userRole, period, dateFrom, dateTo, userIds, schoolId, regionId}) => {
  return (
    <div>
      <div className='primary-charts-container'>
        <div className='full-width-chart-container'>
          <ReadingLevelChart data={readingChartsData} performanceMetrics={performanceMetrics} granularity={granularity} selectedPeriod={selectedPeriod} storyLanguage={storyLanguage} onStoryLanguageChange={onStoryLanguageChange} loading={loading}
                             readingLessonsSeries={readingLessonsSeries} />
          {/* Hidden for now since it's using mocked data */}
          {/* <SkillsProgressChart data={skillsProgressData} /> */}
        </div>
        <div className='charts-container'>
          <UnitsCompletedChart unitMetrics={unitMetrics} loading={loading} />
        </div>
      </div>
      <ReadingLessonsResultsTable
        userRole={userRole}
        period={period}
        dateFrom={dateFrom}
        dateTo={dateTo}
        userIds={userIds}
        schoolId={schoolId}
        regionId={regionId}
      />
    </div>

  )
}

export default withLocalize(ProgressMasteryCharts);