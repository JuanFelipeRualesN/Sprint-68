import React, {useEffect, useState} from 'react';
import { useTranslation, Trans } from 'react-i18next';
import ToolModal from "./../../../teacher-module/ai-module/components/tools/tool-modal/tool-modal";
import Ic from "../../../../assets/svg/icons";
import {DEFAULT_AI_MODEL} from "../../../teacher-module/ai-module/components/tools/tool-config/tools-config";
import EventsCalendar from './calendar-events-monthly';
import {callPlanLessonsSummary, getTeacherGroupsTotal} from "../../../../api/teachers";
import {Link} from "react-router-dom";
import {callUserSchoolIds} from "../../../../api/users";
import {callCurrentPeriodTermYears} from "../../../../api/schools";
import { buildPeriods, formatDate, formatYearRange, dedupePeriods} from './utils/academic-periods'
import SessionManager from "../../../../custom-classes/session-manager";
import Settings from "../../../../custom-classes/settings";
/* Right column — Academic period + weekly plan + AI prompt */



// Tool config is built at open time so every label and prompt resolves
// through t() in the active UI language.
const buildAskTool = (t, draft) => ({
  category: "ai-assistant",
  key: "ai-assistant",
  title: t('right-column:aiAssistant.title'),
  subtitle: t('right-column:aiAssistant.subtitle'),
  defaultModel: DEFAULT_AI_MODEL,
  fields: [
    {
      "name": "request",
      "labelKey": "fields.objective",
      "type": "textarea",
      "placeholderKey": "placeholders.ai-assistant.placeholder",
      "required": false,
      "rows": 6,
      "default": draft
    },
  ],
  system: t('right-column:aiAssistant.systemPrompt'),
  buildPrompt: (f) => f.request,
});


function RightColumn({ onNavigate }) {
  const { t, i18n } = useTranslation(['right-column', 'common']);
  const [askOpen, setAskOpen] = React.useState(false);
  const [draft, setDraft] = React.useState('');
  const [weekPlan, setWeekPlan] = React.useState([]);
  const [academicYearsInfo, setAcademicYearsInfo] = React.useState([]);
  const [userRole] = useState(SessionManager.getUserRole());

  // Only show periods that actually contain "now" → drops stale/non-current ones.
  const periods = React.useMemo(
      () => buildPeriods(academicYearsInfo).filter((p) => p.isActive),
      [academicYearsInfo]
  );
  const locale = i18n.language;

  React.useEffect(() => {
    try {
      const savedPlans = JSON.parse(localStorage.getItem('gsm_teacher_plannings') || '[]');
      const lessonsThisWeek = [];
      if (savedPlans && savedPlans.length > 0) {
        for (const plan of savedPlans) {
          const lessonsList = plan.Lessons || [];
          for (const lesson of lessonsList) {
            lessonsThisWeek.push({
              // Keep raw values; missing values fall back to translation keys at render time
              subj: plan.SubjectName || null,
              unit: lesson.title || null,
              groups: plan.GroupName || null,
            });
          }
        }
      }
      if (lessonsThisWeek.length > 0) {
        setWeekPlan(lessonsThisWeek.slice(0, 5));
      }
    } catch (e) {
      console.error(e);
    }
  }, []);

  useEffect(() => {
    if(userRole != null && userRole === Settings.ROLE_TEACHER_STR) {
      getWeekLessonsFromApi();
    }
    if(userRole != null && (userRole === Settings.ROLE_TEACHER_STR ||
        userRole === Settings.ROLE_DIRECTOR_STR)) {
        getCurrentPeriods();
    }
  }, []);

  const getWeekLessonsFromApi = async () => {
    const response = await callPlanLessonsSummary();
    setWeekPlan(response.data);
  };

  const getCurrentPeriods = async () => {
    const response = await callUserSchoolIds();

    if (response?.data?.length > 0) {
      const schoolIds = response.data;
      const responseAcademicYears = await callCurrentPeriodTermYears(schoolIds);
      setAcademicYearsInfo(responseAcademicYears?.data ?? []);
    } else {
      setAcademicYearsInfo([]); // no school ids → clear, so the empty state shows
    }
  };

  // Open the modal with the current draft pre-filled
  const openAsk = () => {
    setAskOpen(buildAskTool(t, draft));
  };

  // Resolve a plan item field: stored value wins, otherwise translated fallback
  const planLabel = (value, key, fallbackKey) =>
      value || (key ? t(key) : t(fallbackKey));

  return (
      <>
        {/* Academic period */}
      {(userRole != null && (userRole === Settings.ROLE_TEACHER_STR ||
        userRole === Settings.ROLE_DIRECTOR_STR)) ?
        <div className="widget">
          <div className="w-head">
            <h3>{t('right-column:academicPeriod.title')}</h3>
          </div>

          {periods.length === 0 ? (
              <div className="period-empty">{t('right-column:academicPeriod.empty')}</div>
          ) : (
              periods.map((p) => (
                  <div className="period" key={p.id}>
                    <div className="period-top">
                      <div className="period-name">
                        {t('right-column:academicPeriod.name', { number: p.number })}
                      </div>
                      <div className="period-year">{formatYearRange(p.startFullTerm, p.endFullTerm, locale)}</div>
                    </div>
                    <div className="period-meta">
                  <span>
                    <Trans
                        i18nKey="academicPeriod.weekProgress"
                        ns="right-column"
                        values={{current: p.weekNow, total: p.weekTotal}}
                        components={{b: <b/>}}
                    />
                  </span>
                      <span>·</span>
                      <span>
                    <Trans
                        i18nKey="academicPeriod.endsOn"
                        ns="right-column"
                        values={{date: formatDate(p.endPeriod, locale)}}
                        components={{b: <b/>}}
                    />
                  </span>
                    </div>
                    <div className="period-bar">
                      <div className="period-fill" style={{width: p.pct + '%'}}/>
                    </div>
                  </div>
              ))
          )}
        </div> : null
      }
        {/*
        <br/>
                <EventsCalendar/> */}
        <br/>

        {/* This week's plan */}
        {userRole != null && (userRole === Settings.ROLE_TEACHER_STR) ?
        <div className="widget">
          <div className="w-head">
            <h3>{t('right-column:weekPlan.title')}</h3>
            <Link to="/plannings">
              {t('common:general.edit')}
            </Link>
          </div>
          {weekPlan == null || weekPlan.length === 0 ? (
              <p className="plan-no-lessons">
                {t('right-column:weekPlan.noLessonsForThisWeek')}
              </p>
          ) : (
              <ul className="plan-list">
                {weekPlan.map((p) => (
                    <li key={p.PlanLessonId} className="plan-item">
                      <Link to={"/students/lesson/" + p.LessonId + "?fromDashboard=true"}>
                        <div className="plan-subj">
                          {planLabel(p.PlanName, p.SubjectCode, 'right-column:weekPlan.fallback.planning')}
                        </div>
                        <div className="plan-unit">
                          {planLabel( t('right-column:weekPlan.unit') + " " + p.Unit + " · " +  p.LessonTitle, null, 'right-column:weekPlan.fallback.lesson')}
                        </div>
                        <div className="plan-groups">
                          {planLabel(p.GroupName, null, 'right-column:weekPlan.fallback.group')}
                        </div>
                      </Link>
                    </li>
                ))}
              </ul>
          )}
        </div> : null }
        <br/>
        {/* AI prompt */}
        {(userRole != null && (userRole === Settings.ROLE_TEACHER_STR ||
            userRole === Settings.ROLE_DIRECTOR_STR)) ?
        <div className="widget ai-ask">
          <h3>
            <span className="sparkle"><Ic.sparkle/></span> {t('right-column:aiAssistant.title')}
          </h3>
          <p>{t('right-column:aiAssistant.description')}</p>
          <div className="field">
          <textarea
              value={draft}
              onChange={(e) => setDraft(e.target.value)}
              placeholder={t('right-column:aiAssistant.requestPlaceholder')}
              onKeyDown={(e) => {
                if ((e.metaKey || e.ctrlKey) && e.key === 'Enter') openAsk();
              }}
          />
          </div>
          <button className="generate" onClick={openAsk} disabled={!draft.trim()}>
            <Ic.sparkle/> {t('right-column:aiAssistant.generate')} <Ic.arrow/>
          </button>
        </div> : null }

        {askOpen && <ToolModal tool={askOpen} onClose={() => setAskOpen(false)}/>}
      </>
  );
}

export default RightColumn;