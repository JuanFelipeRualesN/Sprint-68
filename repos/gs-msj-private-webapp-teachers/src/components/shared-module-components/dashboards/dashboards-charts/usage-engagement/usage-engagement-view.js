import React from 'react'
import InOffSchoolChart from './in-off-school-chart';
import AccessDevicesChart from './access-devices-chart';
import FrequencySessionsChart from './frequency-sessions-chart';
import { withLocalize, Translate } from 'react-localize-redux';

const UsageEngagementCharts = ({ data, sessionTotals, granularity, sessionMetrics, selectedPeriod, sessionLoading }) => {
  const noTotalData = !sessionTotals || sessionTotals.TotalSessions === 0;

  return (
    <div>
      <div className='primary-charts-container'>
        <div className={`full-width-chart-container ${noTotalData ? 'no-data-container' : ''}`}>
          <FrequencySessionsChart sessionMetrics={sessionMetrics} granularity={granularity} period={selectedPeriod} loading={sessionLoading} />
        </div>
        <div className='charts-container'>
          {!sessionLoading && noTotalData ? (
            <div className="no-data-container">
              <Translate id="dashboards.usage-and-engagement.no-results" />
            </div>
          ) : (
            <>
              <InOffSchoolChart sessionTotals={sessionTotals} loading={sessionLoading} />
              <AccessDevicesChart sessionTotals={sessionTotals} loading={sessionLoading} />
            </>
          )}
        </div>
      </div>
    </div>
  )
}

export default withLocalize(UsageEngagementCharts)