import React, { useMemo, useState, useEffect } from 'react';
import { useTranslation, Trans } from 'react-i18next';
import { Link } from 'react-router-dom';
import Settings from '../../../../custom-classes/settings';
import SessionManager from '../../../../custom-classes/session-manager';
import '../../../../styles/med.css';
import { LineIconAlt } from "../../line-icons/line-icon";
import { buildPeriods, formatDate, formatYearRange, dedupePeriods} from './utils/academic-periods'
import { callCurrentPeriodTermYears } from "../../../../api/schools";
import {callUserSchoolIds} from "../../../../api/users";

/* Welcome banner — greeting card + KPI cards, med-* card style */

/* accent used by the academic-period cards (kept off the palette the other KPIs use) */
const PERIOD_HEX = '#2D6CDF';

/* hex -> rgba, used for the soft icon chip background */
function rgba(hex, alpha) {
    const h = hex.replace('#', '');
    const full = h.length === 3 ? h.split('').map((c) => c + c).join('') : h;
    const n = parseInt(full, 16);
    return `rgba(${(n >> 16) & 255}, ${(n >> 8) & 255}, ${n & 255}, ${alpha})`;
}

/* bar width relative to a soft cap, so a big number never overflows */
function bar(value, cap) {
    const v = Number(value) || 0;
    return `${Math.max(6, Math.min(100, (v / cap) * 100))}%`;
}

function Welcome({
                     firstName,
                     totalMessagesUnread,
                     totalStudents,
                     totalGroups,
                     totalPendingHomework,
                     roles,
                 }) {
    const { t, i18n } = useTranslation(['welcome', 'common', 'right-column']);
    const [userRole] = useState(SessionManager.getUserRole());
    const [academicYearsInfo, setAcademicYearsInfo] = useState([]);

    const today = new Date();
    const hour = today.getHours();
    const greetingKey =
        hour < 12 ? 'greeting.morning' : hour < 19 ? 'greeting.afternoon' : 'greeting.evening';
    const greeting = t(`welcome:${greetingKey}`);

    const dateLocale = i18n.language && i18n.language.startsWith('es') ? 'es-PR' : 'en-US';
    const displayDate = new Intl.DateTimeFormat(dateLocale, {
        weekday: 'long',
        day: 'numeric',
        month: 'long',
    }).format(today);
    const formattedDate = displayDate.charAt(0).toUpperCase() + displayDate.slice(1);

    const isTeacher = Array.isArray(roles) && roles.includes(Settings.ROLE_TEACHER_STR);
    const showSummary = userRole === Settings.ROLE_TEACHER_STR;

    /* periods are only relevant for teachers/directors, same rule as RightColumn */
    const showPeriods =
        userRole === Settings.ROLE_TEACHER_STR || userRole === Settings.ROLE_DIRECTOR_STR;

    /* the greeting card's bar tracks how far along the school day is (7am–5pm) */
    const dayProgress = Math.max(0, Math.min(100, ((hour + today.getMinutes() / 60 - 7) / 10) * 100));

    /* Only keep the period(s) that actually contain "now" → drops stale ones. */
    const periods = useMemo(
        () => buildPeriods(academicYearsInfo).filter((p) => p.isActive),
        [academicYearsInfo]
    );

    /* fetch the current academic period(s) for teachers/directors */
    useEffect(() => {
        if (!showPeriods) return;
        (async () => {
            try {
                const response = await callUserSchoolIds();
                if (response?.data?.length > 0) {
                    const responseAcademicYears = await callCurrentPeriodTermYears(response.data);
                    setAcademicYearsInfo(responseAcademicYears?.data ?? []);
                } else {
                    setAcademicYearsInfo([]); // no school ids → clear, so the empty state shows
                }
            } catch (e) {
                console.error(e);
                setAcademicYearsInfo([]);
            }
        })();
    }, [showPeriods]);

    const kpis = useMemo(() => {
        const list = [];
        if (isTeacher) {
            list.push({
                key: 'toReview',
                label: t('welcome:kpis.toReview'),
                value: totalPendingHomework,
                route: '/pending-teacher-assignments',
                icon: 'check',
                hex: '#E4761B',
                bar: bar(totalPendingHomework, totalPendingHomework),
                linkText: t('welcome:kpis.toReview'),
            });
            list.push({
                key: 'activeStudents',
                label: t('welcome:kpis.activeStudents'),
                value: totalStudents,
                route: '/profile-update',
                icon: 'users',
                hex: '#2F8F83',
                bar: bar(totalStudents, totalStudents),
                linkText: t('welcome:kpis.activeStudents'),
            });
            list.push({
                key: 'groups',
                label: t('welcome:kpis.groups'),
                value: totalGroups,
                route: '/teacher-groups',
                icon: 'group',
                hex: '#5B62C4',
                bar: bar(totalGroups, totalGroups),
                linkText: t('welcome:kpis.groups'),
            });
        }
        return list;
    }, [isTeacher, t, totalPendingHomework, totalStudents, totalGroups]);

    return (
        <section className="med-welcome">
            {/* ── Greeting card ── */}
            <article className="med-card med-greet">
                <div className="med-kpi-top">
                    <span className="med-dash-date">
                        {t('welcome:eyebrow.dateWeek', { date: formattedDate })}
                    </span>
                </div>

                <h1 className="med-dash-greeting-h1">
                    {t('welcome:greeting.line', { greeting, firstName })}
                    {'.'}
                </h1>
                {showSummary && (
                    <p className="med-dash-greeting-p">
                        <Trans
                            t={t}
                            i18nKey="welcome:summary.pendingAndMessages"
                            values={{
                                pendingCount: totalPendingHomework,
                                messageCount: totalMessagesUnread,
                            }}
                            components={{ b: <b /> }}
                        />
                    </p>
                )}
                <br/>
            </article>

            {/* ── KPI band ── */}
            <div className="med-kpis">
                {kpis.map((k) => (
                    <Link key={k.key} to={k.route} className="med-card med-kpi">
                        <div className="med-kpi-top">
                            <span className="med-kpi-label">{k.label}</span>
                            <span
                                className="med-kpi-icon"
                                style={{ background: rgba(k.hex, 0.12), color: k.hex }}
                            >
                                <LineIconAlt name={k.icon} size={19} />
                            </span>
                        </div>

                        <div className="med-kpi-value">
                            <strong>{k.value}</strong>
                        </div>

                        <div className="med-kpi-bar">
                            <div style={{ width: k.bar, background: k.hex }} />
                        </div>

                        <div className="med-kpi-foot">
                            <em style={{ color: k.hex }}>{k.linkText} →</em>
                        </div>
                    </Link>
                ))}

                {/* ── Academic period card — one card, one row per active period ── */}
                {showPeriods && (
                    <div className="med-card med-kpi med-kpi-period">
                        <div className="med-kpi-top">
                            <span className="med-kpi-label">
                                {t('right-column:academicPeriod.title')}
                            </span>
                            <span
                                className="med-kpi-icon"
                                style={{ background: rgba(PERIOD_HEX, 0.12), color: PERIOD_HEX }}
                            >
                                <LineIconAlt name="calendar" size={19} />
                            </span>
                        </div>

                        {periods.length === 0 ? (
                            <div className="med-kpi-empty-text">
                                {t('right-column:academicPeriod.empty')}
                            </div>
                        ) : (
                            <div className="med-kpi-period-list">
                                {periods.map((p) => (
                                    <div key={p.id} className="med-kpi-period-row">
                                        <div className="med-kpi-period-line">
                                            <span className="med-kpi-period-name period-name">
                                                {t('right-column:academicPeriod.name', { number: p.number })}{' '}
                                            </span>
                                            <span className="med-kpi-value-sub">
                                               <b> {p.weekNow}/{p.weekTotal}</b>
                                            </span>
                                        </div>

                                        <div className="med-kpi-bar">
                                            <div style={{ width: p.pct + '%', background: PERIOD_HEX }} />
                                        </div>

                                        <div className="med-kpi-foot">
                                            <em style={{ color: PERIOD_HEX }}>
                                                <Trans
                                                    t={t}
                                                    i18nKey="right-column:academicPeriod.endsOn"
                                                    values={{ date: formatDate(p.endPeriod, i18n.language) }}
                                                    components={{ b: <b /> }}
                                                />
                                            </em>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                )}
            </div>
        </section>
    );
}

export default Welcome;