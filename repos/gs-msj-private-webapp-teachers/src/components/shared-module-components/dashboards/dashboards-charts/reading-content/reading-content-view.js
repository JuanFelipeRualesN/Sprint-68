import React from 'react'
import { withLocalize } from 'react-localize-redux';
import ReadingCategoriesChart from './reading-categories-chart';
import AverageTimeReadingChart from './average-time-reading-chart';
import ReadingVolumeChart from './reading-volume-chart';
import MetricsCard from '../usage-engagement/metrics-card';
import { BookCheck } from 'lucide-react';
import { BookOpen } from 'lucide-react';
import { Translate } from 'react-localize-redux';
import LanguageSwitch, { getLanguageSection } from '../language-switch';

const ReadingContentCharts = ({ contentMetrics, granularity, selectedPeriod, loading, storyLanguage = 'en', onStoryLanguageChange = () => {} }) => {
  const languageMetrics = getLanguageSection(contentMetrics, storyLanguage);
  const completionRate = languageMetrics?.CompletionRate ?? null;
  const totalReadings  = languageMetrics?.TotalTextsRead ?? null;

  return (
    <div>
      <div style={{ marginBottom: 14 }}>
        <LanguageSwitch language={storyLanguage} onChange={onStoryLanguageChange} />
      </div>
      <div className="cards-metrics-container">
        {/* Text Completion Rate */}
        <MetricsCard 
          title={<Translate id="dashboards.usage-and-engagement.text-completion-rate" />} 
          icon={<BookCheck size={18} />} 
          color="#93e24aff" 
          badgeColor={"rgba(147, 226, 74, .12)"}
          unit={"%"}
          value={completionRate} 
        />
        {/* Total Readings Completed */}
        <MetricsCard
          title={<Translate id="dashboards.usage-and-engagement.readings-completed" />} 
          icon={<BookOpen size={18} />} 
          color="#7033b6ff" 
          badgeColor={"rgba(112, 51, 182, .12)"} 
          value={totalReadings} 
        />        
      </div>
      <div className='primary-charts-container'>
        <div className='full-width-chart-container'>
          <AverageTimeReadingChart contentMetrics={languageMetrics} granularity={granularity} selectedPeriod={selectedPeriod} loading={loading} />
          <ReadingVolumeChart contentMetrics={languageMetrics} granularity={granularity} selectedPeriod={selectedPeriod} loading={loading} />
        </div>
        <div className='charts-container'>
          <ReadingCategoriesChart categories={languageMetrics?.Categories} loading={loading} />
        </div>
      </div>
    </div>
  )
}

export default withLocalize(ReadingContentCharts)