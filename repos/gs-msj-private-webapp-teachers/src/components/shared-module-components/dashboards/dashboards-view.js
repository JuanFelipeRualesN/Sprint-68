import React, { useState, useMemo, useEffect, useRef } from 'react'
import { useLocation } from "react-router-dom";
import { Translate } from 'react-localize-redux';
import { withLocalize } from 'react-localize-redux';
import {
    Nav, NavItem, TabContent, TabPane, Card, CardBody, Button,
    Pagination, PaginationItem, PaginationLink, Label, Input, Table, Alert
} from "reactstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faFilePdf, faCalendarAlt, faTachometerAlt, faChartLine, faChartBar, faBookReader, faTimes, faBookOpen, faSpinner,
    faFileExcel, faEye, faDownload, faList
} from "@fortawesome/free-solid-svg-icons";
import UsageEngagementCharts from './dashboards-charts/usage-engagement/usage-engagement-view';
import { getSessionTotals, getSessionMetrics, getPerformanceMetrics, getContentMetrics, getUnitMetrics, getModuleUsage, getReadingLessonsSeries } from '../../../api/reading-api';
import PeriodSelector, { getWeekRange } from './dashboards-charts/period-selector';
import ReadingContentCharts from './dashboards-charts/reading-content/reading-content-view';
import ProgressMasteryCharts from './dashboards-charts/progress-mastery/progress-mastery-view';
import {DynamicUsageMetricsCard} from "./dashboards-charts/usage-engagement/dynamic-usage-metrics-card";
import {UsageGapsChart} from "./dashboards-charts/usage-gaps-chart";
import {DiagnosticsChart} from "./dashboards-charts/diagnostics-chart";
import AssignDatesModal from "./helpers/assign-dates-modal";
import './style.css';
import {focusAreas, languages, studentExamResults,  mockUsageMetrics,
    activitiesAndSuggestions, readingLessons, readingChartsMockedData, skillsProgressMockedData, unitsCompletedMockedData,
    averageTimeReadingMockedData, readingVolumeMockedData, readingCategoriesMockedData, averageLessonResultMockedData,
    readingKeyMetricsMockedData, diagnosticsMockedData
}
    from "./helpers/data";
import {StudentExamResultsList} from "./helpers/student-exam-results-list";
import {MultiSelectDropdown} from "./helpers/multi-select-dropdown";
import {SchoolSelectDropdown} from "./helpers/school-select-dropdown";
import {SingleSelectDropdown} from "./helpers/single-select-dropdown";
import {AverageLessonResults} from "./helpers/average-lesson-results";
import StudentDiagnosticModal from './helpers/student-diagnostic-modal';
import { PulseLoader } from 'react-spinners';
import { getLessonPDF, getTeacherGroups, getGroupDetails, getDirectorGroups } from '../../../api/teachers';
import { getProfiles } from '../../../api/users';
import { getRegions } from '../../../api/schools';
import { getClientSubscriptionsGeneralData } from '../../../api/subscriptions';
import PronunciationLessons from './helpers/pronunciation-lessons';
import { buildSkeleton, tickTranslate } from './dashboards-charts/chart-skeleton';
import { buildPerformanceMetricsSkeleton, getPerformanceRecords } from './dashboards-charts/progress-mastery/reading-level-chart';
import { buildComprehensionTrendSkeleton } from './dashboards-charts/progress-mastery/reading-comprehension-trend-chart';
import { getLanguageSection, getLanguageLabelKey } from './dashboards-charts/language-switch';
import * as XLSX from 'xlsx'
import SessionManager from "../../../custom-classes/session-manager";
import Settings, {MANUALS_AI_SPEECH_MODULE} from "../../../custom-classes/settings"
import LanguageManager from "../../../custom-classes/language-manager";
// import DiagnosticView from "./diagnostics";
import StoriesView from '../../reading-ai-module/stories';

const EXPORT_SECTION_BY_SUB_TAB = {
    "1": 'Uso y Compromiso',
    "2": 'Progreso y Dominio',
    "3": 'Lectura y Contenido',
};

// Empty states for the metrics that are returned split by story language
const emptyPerformanceMetrics = { English: [], Spanish: [] };
const emptyLanguageContentMetrics = { CompletionRate: 0, TotalTextsRead: 0, Categories: [], PagesByPeriod: [], AvgTimeByPeriod: [] };
const emptyContentMetrics = { English: emptyLanguageContentMetrics, Spanish: emptyLanguageContentMetrics };

const DashboardsView = ({ translate }) => {
    const location = useLocation();
    const query = new URLSearchParams(location.search);
    let userRole = SessionManager.getUserRole(); // Currently set to 'teacher'
    let manualUrl = MANUALS_AI_SPEECH_MODULE + LanguageManager.getLanguage() + "_" + userRole + ".pdf";
    if(userRole === "admin-ore"){
        userRole = "adminOre";
    }
    const [topLevelTab, setTopLevelTab] = useState(query.get("mainTab") || "1");
    const [dashboardSubTab, setDashboardSubTab] = useState(query.get("subTab") || "1");
    const [lessonsCardTab, setLessonsCardTab] = useState('reading');
    const [modalOpen, setModalOpen] = useState(false);
    const toggleModal = () => setModalOpen(!modalOpen);

    const [diagnosticResultsModalOpen, setDiagnosticResultsModalOpen] = useState(false);
    const [loadingLessonId, setLoadingLessonId] = useState(null);

    const [pronunciationLessonsData, setPronunciationLessonsData] = useState([]);

    const [sessionTotals, setSessionTotals] = useState(null);
    const [sessionMetrics, setSessionMetrics] = useState([]);
    const [performanceMetrics, setPerformanceMetrics] = useState(emptyPerformanceMetrics);
    const [storyLanguage, setStoryLanguage] = useState('en');
    const [contentMetrics, setContentMetrics] = useState(null);
    const [unitMetrics, setUnitMetrics] = useState([]);
    const [moduleUsage, setModuleUsage] = useState(null);
    const [readingLessonsSeries, setReadingLessonsSeries] = useState([]);
    const [sessionLoading, setSessionLoading] = useState(false);
    const [periodGranularity, setPeriodGranularity] = useState('week');
    const today = new Date();
    const initialPeriod = { period: 'week', ...getWeekRange(today) };
    const [selectedPeriod, setSelectedPeriod] = useState(initialPeriod);
    const [periodWeekDate, setPeriodWeekDate] = useState(new Date());
    const [periodMonthValue, setPeriodMonthValue] = useState(`${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}`);
    const [periodYearValue, setPeriodYearValue] = useState(String(today.getFullYear()));
    const [realStudents, setRealStudents] = useState([]);
    const [realGroups, setRealGroups] = useState([]);
    const [realRegions, setRealRegions] = useState([]);
    const currentPeriodRef = useRef(null);
    const studentsLimiter = 100;
    const groupsLimiter = 100;
    const selectedStudentsRef = useRef([]);
    const selectedSchoolIdRef = useRef(null);
    const selectedRegionIdRef = useRef(null);
    const studentsFromGroupsRef = useRef(false);

    // --- FILTER STATES ---
    const [selectedStudents, setSelectedStudents] = useState([]);
    const [selectedGroups, setSelectedGroups] = useState([]);
    const [studentsFromGroups, setStudentsFromGroups] = useState(false);
    const [selectedLanguages, setSelectedLanguages] = useState([]);
    const [selectedFocusAreas, setSelectedFocusAreas] = useState([]);
    const [selectedRegions, setSelectedRegions] = useState([]);
    const [selectedSchools, setSelectedSchools] = useState([]);
    const [selectedSchoolId, setSelectedSchoolId] = useState(null);
    const [selectedRegionId, setSelectedRegionId] = useState(null);
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');

    const [filterDependency, setFilterDependency] = useState(0);
    const [contentOpacity, setContentOpacity] = useState(1);

    // Set default filters based on user role
    useEffect(() => {
        if (userRole === 'parent') {
            setSelectedStudents(['Sofía Martínez']);
        }
    }, [userRole]);

    useEffect(() => {
        if(userRole === 'client'){
           // console.log(getClientSubscriptionsGeneralData());
        }
    }, []);

    // Fetch students for the student dropdown filter (teacher and client roles).
    useEffect(() => {
        if (userRole !== 'teacher' && userRole !== 'client') return;

        const fetchProfiles = async () => {
            try {
                const res = await getProfiles(null, null, null, 0, studentsLimiter);
                setRealStudents(res.data?.UserInfo ?? []);
            } catch (e) {
                console.error('error fetching profiles:', e);
            }
        };

        fetchProfiles();
    }, [userRole]);

    // Fetch the teacher's groups for the group dropdown filter.
    useEffect(() => {
        if (userRole !== 'teacher') return;

        const fetchGroups = async () => {
            try {
                const res = await getTeacherGroups(groupsLimiter, 0);
                setRealGroups(res.data?.TeacherGroupModelList ?? []);
            } catch (e) {
                console.error('error fetching groups:', e);
            }
        };

        fetchGroups();
    }, [userRole]);

    // Fetch the groups for a director
    useEffect(() => {
        if (userRole !== 'director') return;

        const fetchDirectorGroups = async () => {
            try {
                const res = await getDirectorGroups(groupsLimiter, 0);
                const teachers = res.data?.Teachers ?? [];
                const flatGroups = teachers.flatMap(teacher => teacher.Groups ?? []);
                setRealGroups(flatGroups);
            } catch (e) {
                console.error('error fetching director groups:', e);
            }
        };

        fetchDirectorGroups();
    }, [userRole]);

    // Fetch the regions for the region filter. For admin it returns all regions, for
    // adminOre the backend already scopes them to the ORE's region
    useEffect(() => {
        if (userRole !== 'admin' && userRole !== 'adminOre') return;

        const fetchRegions = async () => {
            try {
                const res = await getRegions();
                const regionsData = res.data ?? [];
                setRealRegions(regionsData);
                if (userRole === 'adminOre') {
                    setSelectedRegions(regionsData.map(r => r.Name));
                }
            } catch (e) {
                console.error('error fetching regions:', e);
            }
        };

        fetchRegions();
    }, [userRole]);

    // Fetch all necessary metrics data based on the selected period and filters
    const fetchMetricsData = async (period, dateFrom, dateTo, userIds, schoolId = null, regionId = null) => {
        setSessionLoading(true);
        try {
            const [totalsResult, detailsResult, performanceResult, contentResult, unitResult, moduleUsageResult, lessonsSeriesResult] = await Promise.allSettled([
                getSessionTotals(period, dateFrom, dateTo, userIds, schoolId, regionId),
                getSessionMetrics(period, dateFrom, dateTo, userIds, schoolId, regionId),
                getPerformanceMetrics(period, dateFrom, dateTo, userIds, schoolId, regionId),
                getContentMetrics(period, dateFrom, dateTo, userIds, schoolId, regionId),
                getUnitMetrics(userIds, schoolId, regionId),
                getModuleUsage(schoolId, regionId, userRole === 'director' ? userIds : null),
                getReadingLessonsSeries(period, dateFrom, dateTo, userIds, schoolId, regionId),
            ]);

            // Returns the response data for a fulfilled request
            const getData = (result, fallback, label) => {
                if (result.status === 'fulfilled') {
                    return result.value?.data ?? fallback;
                }
                console.error(`error fetching ${label}:`, result.reason);
                return fallback;
            };

            setSessionTotals(getData(totalsResult, null, 'session totals'));
            setSessionMetrics(getData(detailsResult, [], 'session metrics'));
            setPerformanceMetrics(getData(performanceResult, emptyPerformanceMetrics, 'performance metrics'));
            setContentMetrics(getData(contentResult, emptyContentMetrics, 'content metrics'));
            setUnitMetrics(getData(unitResult, [], 'unit metrics'));
            setModuleUsage(getData(moduleUsageResult, null, 'module usage'));
            setReadingLessonsSeries(getData(lessonsSeriesResult, [], 'reading lessons series'));
        } catch (e) {
            console.error('error fetching dashboard data:', e);
        } finally {
            setSessionLoading(false);
        }
    };

    // Clears all metric charts and set them to empty states. 
    // Used when a group is selected but it has no students.
    const clearMetrics = () => {
        setSessionTotals({ TotalSessions: 0, TotalActiveDays: 0, AvgTimeSeconds: 0 });
        setSessionMetrics([]);
        setPerformanceMetrics(emptyPerformanceMetrics);
        setContentMetrics(emptyContentMetrics);
        setUnitMetrics([]);
        setModuleUsage(null);
        setReadingLessonsSeries([]);
    };

    const runMetricsFetch = (period, dateFrom, dateTo) => {
        // When a group is selected but it has no students, show empty charts instead of fetching.
        if (studentsFromGroupsRef.current && selectedStudentsRef.current.length === 0) {
            clearMetrics();
            return;
        }

        fetchMetricsData(period, dateFrom, dateTo, selectedStudentsRef.current, selectedSchoolIdRef.current, selectedRegionIdRef.current);
    };

    // Fetch metrics data when the period changes
    const handlePeriodChange = ({ period, dateFrom, dateTo }) => {
        currentPeriodRef.current = { period, dateFrom, dateTo };
        setPeriodGranularity(period);
        setSelectedPeriod({ period, dateFrom, dateTo });

        if (period === 'week' && dateFrom) {
            setPeriodWeekDate(new Date(dateFrom + 'T00:00:00'));
        }
        if (period === 'month' && dateFrom) {
            setPeriodMonthValue(dateFrom.slice(0, 7));
        }
        if (period === 'year' && dateFrom) {
            setPeriodYearValue(dateFrom.slice(0, 4));
        }
        runMetricsFetch(period, dateFrom, dateTo);
    };

    useEffect(() => {
        selectedStudentsRef.current = selectedStudents;
        selectedSchoolIdRef.current = selectedSchoolId;
        selectedRegionIdRef.current = selectedRegionId;
        studentsFromGroupsRef.current = studentsFromGroups;

        const { period, dateFrom, dateTo } = currentPeriodRef.current ?? {};
        runMetricsFetch(period, dateFrom, dateTo);
    }, [selectedStudents, selectedSchoolId, selectedRegionId, studentsFromGroups]);

    const updateFilterAndRerender = (setter, value) => {
        setter(value);
        setContentOpacity(0.4);

        setTimeout(() => {
            setFilterDependency(prev => prev + 1);
        }, 300);
    };

    // Resolve the selected groups student UserIds into selectedStudents so metrics filter
    // by them. MultiSelectDropdown may pass an updater function, so resolve it against selectedGroups.
    const handleGroupsChange = async (valuesOrUpdater) => {
        const groupIds = typeof valuesOrUpdater === 'function'
            ? valuesOrUpdater(selectedGroups)
            : valuesOrUpdater;

        updateFilterAndRerender(setSelectedGroups, groupIds);

        if (!groupIds || groupIds.length === 0) {
            setStudentsFromGroups(false);
            setSelectedStudents([]);
            return;
        }

        setStudentsFromGroups(true);

        try {
            let userIds;
            if (userRole === 'director') {
                // Director groups already include their StudentIds.
                userIds = groupIds.flatMap(groupId => {
                    const group = realGroups.find(g => String(g.Id) === String(groupId));
                    return group?.StudentIds ?? [];
                });
            } else {
                const responses = await Promise.all(groupIds.map(groupId => getGroupDetails(groupId)));
                userIds = responses.flatMap(res =>
                    (res.data?.StudentDetailModelList ?? []).map(student => student.UserId)
                );
            }
            // Remove duplicates in case a student belongs to more than one selected group
            setSelectedStudents([...new Set(userIds)]);
        } catch (e) {
            console.error('error fetching group students:', e);
            setSelectedStudents([]);
        }
    };

    // Single school selection
    const handleSchoolChange = (entityId, entityName) => {
        const numericId = entityId != null ? Number(entityId) : null;
        setSelectedSchoolId(numericId);
        updateFilterAndRerender(setSelectedSchools, entityName ? [entityName] : []);
    };

    // Single region selection
    const handleRegionChange = (regionId, regionName) => {
        setSelectedRegionId(regionId != null ? regionId : null);
        updateFilterAndRerender(setSelectedRegions, regionName ? [regionName] : []);
    };

    const primarySelectedStudent = selectedStudents.length > 0 ? selectedStudents[0] : '';

    // --- ROLE-BASED VISIBILITY LOGIC ---
    const hideRegions = userRole !== "admin" && userRole !== "adminOre";
    const hideSchools = userRole !== "admin" && userRole !== "adminOre" && userRole !== "director";
    const hideStudents = userRole !== "teacher" && userRole !== "parent" && userRole !== "client";
    const hideGroups = userRole !== "teacher" && userRole !== "director";
    const hideSkill = userRole === "parent";
    const showAutomaticReportsMessages = userRole === "admin" || userRole === "adminOre";

    const disabledGroups = selectedStudents.length > 0 && !studentsFromGroups;
    const disabledStudents = userRole === "parent" || selectedGroups.length > 0;
    const disabledRegions = userRole === "adminOre" || selectedSchoolId != null;
    const disabledSchools = selectedRegionId != null;

    const [diagnosticsData, setDiagnosticsData] = useState([
        { month: 'Agosto', score: 65, status: 'completed' },
        { month: 'Diciembre', score: 85, status: 'completed' },
        { month: 'Enero', score: 0, status: 'pending' },
    ]);

    const handleAssignDate = (monthName, date) => {
        const existingIndex = diagnosticsData.findIndex(d => d.month === monthName);

        if (existingIndex > -1) {
            const newDiagnostics = [...diagnosticsData];
            newDiagnostics[existingIndex] = { ...newDiagnostics[existingIndex], status: 'pending', dateAssigned: date };
            setDiagnosticsData(newDiagnostics);
        } else {
            setDiagnosticsData([...diagnosticsData, { month: monthName, score: 0, status: 'pending', dateAssigned: date }]);
        }
        alert(`Diagnóstico asignado para el ${date}.`);
    };

    const resetFilters = () => {
        setSelectedStudents([]);
        setSelectedGroups([]);
        setStudentsFromGroups(false);
        setSelectedLanguages([]);
        setSelectedFocusAreas([]);
        if (userRole !== 'adminOre') {
            setSelectedRegions([]);
            setSelectedRegionId(null);
        }
        setSelectedSchools([]);
        setSelectedSchoolId(null);
        setStartDate('');
        setEndDate('');
        if (dashboardSubTab === "2") {
            setSelectedFocusAreas([]);
        }

        setContentOpacity(0.4);
        setTimeout(() => {
            setFilterDependency(prev => prev + 1);
        }, 300);
    };
    // EXPORT HANDLER
    // Exports what the user is currently seeing, the same data the charts render,
    // for the selected period, granularity and filters.
    const handleExportToExcel = (dataName) => {
        const filteredExamResults = filteredData.examResults; 
        let dataToExport = [];
        let fileName = `${dataName.replace(/ /g, '_')}.xlsx`;

        const colMetric = translate('dashboards.usage-and-engagement.key-usage-metrics');
        const colValue = translate('dashboards.export-value');
        const colPeriod = translate('dashboards.usage-and-engagement.days');
        const colDevice = translate('dashboards.usage-and-engagement.most-used-devices-title');
        const colPercentage = translate('dashboards.export-percentage');
        const colUsage = translate('dashboards.usage-and-engagement.usage');
        const colUsagePercent = `${translate('dashboards.usage-and-engagement.usage')} (%)`;
        const colStudent = translate('dashboards.student');
        const colGroup = translate('dashboards.group');
        const colTeacher = translate('notification-content.teacher');
        const colSchool = translate('dashboards.school');
        const colRegion = translate('dashboards.region');
        const colUnit = translate('dashboards.progress-and-mastery.unit');
        const colCategory = translate('dashboards.category');
        const colReadings = translate('dashboards.usage-and-engagement.readings-completed');
        const colTotalStudents = translate('dashboards.usage-and-engagement.total-students');
        const colActiveStudents = translate('dashboards.usage-and-engagement.used-students');
        const colModuleUsage = translate('dashboards.usage-and-engagement.usage');

        const toMinutes = (seconds) => Math.round((seconds ?? 0) / 60);

        const translateLabel = tickTranslate(translate);

        const sheetName = (translationKey) => translate(translationKey).replace(/[:\\/?*[\]]/g, '').slice(0, 31);

        const fileNameSuffix = selectedPeriod?.dateFrom ? `_${selectedPeriod.dateFrom}` : '';

        // Records the selection that produced the report
        const colFilter = translate('analytics.filters');
        const colSelection = translate('dashboards.selected');
        const filtersSheetName = sheetName('analytics.filters');

        const buildFiltersSheet = () => {
            const rows = [
                { [colFilter]: translate('dashboards.date-from'), [colSelection]: selectedPeriod?.dateFrom ?? '' },
                { [colFilter]: translate('dashboards.date-to'), [colSelection]: selectedPeriod?.dateTo ?? '' },
            ];

            if (selectedRegions.length > 0) {
                rows.push({ [colFilter]: colRegion, [colSelection]: selectedRegions.join(', ') });
            }

            if (selectedSchools.length > 0) {
                rows.push({ [colFilter]: colSchool, [colSelection]: selectedSchools.join(', ') });
            }

            if (selectedGroups.length > 0) {
                const groupNames = selectedGroups.map(groupId => {
                    const group = realGroups.find(g => String(g.Id) === String(groupId));
                    return group?.Name ?? groupId;
                });
                rows.push({ [colFilter]: colGroup, [colSelection]: groupNames.join(', ') });
            }

            // Students selected through a group are already covered by the group row.
            if (selectedStudents.length > 0 && !studentsFromGroups) {
                const studentNames = selectedStudents.map(userId => {
                    const student = realStudents.find(s => String(s.UserId) === String(userId));
                    return student ? `${student.GivenName} ${student.Surname}` : userId;
                });
                rows.push({ [colFilter]: colStudent, [colSelection]: studentNames.join(', ') });
            }

            return rows;
        };

        // Records which story language produced a report built from story results
        const buildStoryLanguageFiltersSheet = () => {
            const rows = buildFiltersSheet();
            const languageLabelKey = getLanguageLabelKey(storyLanguage);

            if (languageLabelKey) {
                rows.push({ [colFilter]: translate('dashboards.language'), [colSelection]: translate(languageLabelKey) });
            }

            return rows;
        };

        switch (dataName) {
            case `Lecciones - ${lessonsCardTab === 'actividades' ? 'Actividades' : 'Lectura'}`:
                if (lessonsCardTab === 'reading') {
                    dataToExport = pronunciationLessonsData.map(lesson => ({
                        'Título de la lección': lesson.LessonTitle,
                        'Unidades': lesson.Units,
                        'Código': lesson.ConstructedFullCode,
                        'Nivel': lesson.LevelCode,
                    }));
                    fileName = 'Reporte_Lecciones_Pronunciacion.xlsx';
                } else {
                    dataToExport = currentLessonList.map(lesson => ({
                        'Título de la lección': lesson.title,
                        'Lección recomendada para': lesson.assignedStudents,
                        'Código': lesson.standardCode,
                    }));
                    fileName = `Reporte_Lecciones_${lessonsCardTab}.xlsx`;
                }
                break;

            case 'Resultados de Exámenes/Promedios de Lecciones':
                let resultsToExport = [];
                let dataToExportSheets = {};
                let exportTitle = 'Reporte_General';
                let currentSelectedStudentName = null;

                if (primarySelectedStudent) {
                    if (Array.isArray(primarySelectedStudent) && primarySelectedStudent.length > 0) {
                        currentSelectedStudentName = primarySelectedStudent[0];
                    } else if (typeof primarySelectedStudent === 'string' && primarySelectedStudent.length > 0) {
                        currentSelectedStudentName = primarySelectedStudent;
                    }
                }

                if (userRole === 'teacher') {
                    if (currentSelectedStudentName && currentSelectedStudentName !== 'Todos los estudiantes') {
                        exportTitle = currentSelectedStudentName.replace(/\s/g, '_');

                        const studentData = filteredExamResults.find( 
                            student => student.name === currentSelectedStudentName
                        );

                        if (studentData && studentData.exams.length > 0) {
                            resultsToExport = studentData.exams.map(exam => ({
                                'Estudiante': currentSelectedStudentName,
                                'Nombre del examen': exam.examName,
                                'Puntuación': `${exam.score}%`,
                                'Tema': exam.topic,
                                'Código': exam.standardCode,
                                'Idioma': exam.language.toUpperCase(),
                                'Categoría': exam.category,
                                'Fecha de finalización': exam.completedDate,
                            }));
                        }
                    } else {
                        exportTitle = 'Todos_los_Estudiantes';
                        resultsToExport = filteredExamResults.flatMap(student => {
                            return student.exams.map(exam => ({
                                'Estudiante': student.name, 
                                'Nombre del examen': exam.examName,
                                'Puntuación': `${exam.score}%`,
                                'Tema': exam.topic,
                                'Código': exam.standardCode,
                                'Idioma': exam.language.toUpperCase(),
                                'Categoría': exam.category,
                                'Fecha de finalización': exam.completedDate,
                            }));
                        });
                    }
                    
                    if (resultsToExport.length > 0) {
                        dataToExportSheets['Resultados Detallados'] = resultsToExport;
                    }
                }

                if (userRole === 'admin' || userRole === 'director') {
                    let averageResultsReportData = [];
                    
                    if (userRole === 'admin') {
                        averageResultsReportData = averageLessonResultMockedData.map(item => ({
                            'Título de lección': item.title,
                            'Código': item.code,
                            'Promedio de puntuación (%)': item.average,
                            'Nivel de desempeño': item.level,
                        }));
                        exportTitle = 'Promedios_Admin';

                    } else { 
                        averageResultsReportData = averageLessonResultMockedData.map(item => ({
                            'Título de lección': item.title,
                            'Código': item.code,
                            'Grupo': item.group, 
                            'Promedio de puntuación (%)': item.average,
                            'Nivel de desempeño': item.level,
                        }));
                        exportTitle = 'Promedios_Director';
                    }

                    if (averageResultsReportData.length > 0) {
                        dataToExportSheets['Promedios de Grupos-Estándares'] = averageResultsReportData;
                    }
                }
                
                if (Object.keys(dataToExportSheets).length === 0) {
                    console.warn(`No se encontraron resultados de exámenes para exportar.`);
                    dataToExport = [];
                    fileName = `Reporte_Exámenes_Vacio.xlsx`;
                    break;
                }
                
                dataToExport = dataToExportSheets;
                fileName = `Reporte_Exámenes_${exportTitle}.xlsx`;
                break;

            case 'Uso y Compromiso': {
                const keyMetrics = [
                    { [colMetric]: translate('dashboards.usage-and-engagement.total-sessions'), [colValue]: sessionTotals?.TotalSessions ?? 0 },
                    { [colMetric]: translate('dashboards.usage-and-engagement.active-days'), [colValue]: sessionTotals?.TotalActiveDays ?? 0 },
                    { [colMetric]: translate('dashboards.usage-and-engagement.average-usage-time'), [colValue]: toMinutes(sessionTotals?.AvgTimeSeconds) },
                ];

                const minutesSkeleton = buildSkeleton(periodGranularity, sessionMetrics, selectedPeriod, 'TotalTimeSeconds', 'minutes');
                const sessionsSkeleton = buildSkeleton(periodGranularity, sessionMetrics, selectedPeriod, 'TotalSessions', 'sessions');

                const frequencyData = minutesSkeleton.map((item, index) => ({
                    [colPeriod]: translateLabel(item.label),
                    [translate('dashboards.usage-and-engagement.usage-time')]: toMinutes(item.minutes),
                    [translate('dashboards.usage-and-engagement.sessions')]: sessionsSkeleton[index]?.sessions ?? 0,
                }));

                const devicesData = [
                    { [colDevice]: translate('dashboards.usage-and-engagement.device-desktop'), [colPercentage]: sessionTotals?.AvgDesktopPercent ?? 0 },
                    { [colDevice]: translate('dashboards.usage-and-engagement.device-tablet'), [colPercentage]: sessionTotals?.AvgTabletPercent ?? 0 },
                    { [colDevice]: translate('dashboards.usage-and-engagement.device-mobile'), [colPercentage]: sessionTotals?.AvgMobilePercent ?? 0 },
                ];

                const schoolUsageData = [
                    { [colUsage]: translate('dashboards.usage-and-engagement.in-school-hours'), [colPercentage]: sessionTotals?.AvgOnSchoolPercent ?? 0 },
                    { [colUsage]: translate('dashboards.usage-and-engagement.off-school-hours'), [colPercentage]: sessionTotals?.AvgOffSchoolPercent ?? 0 },
                ];

                let usageGapData = [];
                let usageGapTitleKey = 'dashboards.usage-and-engagement.usage-gaps-by-school';

                if (userRole === 'teacher') {
                    usageGapTitleKey = 'dashboards.usage-and-engagement.usage-gaps-by-student';
                    usageGapData = moduleUsageData.teacher.map(student => ({
                        [colStudent]: student.name,
                        [colModuleUsage]: student.isUsing
                            ? translate('dashboards.usage-and-engagement.using-the-module')
                            : translate('dashboards.usage-and-engagement.no-use'),
                    }));
                } else if (userRole === 'director') {
                    usageGapTitleKey = 'dashboards.usage-and-engagement.usage-gaps-by-group';
                    usageGapData = moduleUsageData.director.map(group => ({
                        [colGroup]: group.name,
                        [colTeacher]: group.teacherName ?? '',
                        [colUsagePercent]: group.usage,
                        [colTotalStudents]: group.totalStudents,
                        [colActiveStudents]: group.usedStudents,
                    }));
                } else if (userRole === 'admin' || userRole === 'adminOre') {
                    const usageItems = userRole === 'admin' ? moduleUsageData.admin : moduleUsageData.adminOre;
                    const isSchoolLevel = (moduleUsage?.ByRegion ?? []).some(region => Array.isArray(region.Schools?.Items));

                    usageGapData = usageItems.map(item => ({
                        [isSchoolLevel ? colSchool : colRegion]: item.name,
                        [colUsagePercent]: item.usage,
                        [colTotalStudents]: item.totalStudents,
                        [colActiveStudents]: item.usedStudents,
                    }));
                }

                dataToExport = {
                    [filtersSheetName]: buildFiltersSheet(),
                    [sheetName('dashboards.usage-and-engagement.key-usage-metrics')]: keyMetrics,
                    [sheetName('dashboards.usage-and-engagement.frequency-usage-title')]: frequencyData,
                    [sheetName('dashboards.usage-and-engagement.most-used-devices-title')]: devicesData,
                    [sheetName('dashboards.usage-and-engagement.off-in-school-title')]: schoolUsageData,
                    [sheetName(usageGapTitleKey)]: usageGapData,
                };

                fileName = `Reporte_Uso_Compromiso${fileNameSuffix}.xlsx`;
                break;
            }

            case 'Progreso y Dominio': {
                const languageRecords = getPerformanceRecords(performanceMetrics, storyLanguage);
                const metricsSkeleton = buildPerformanceMetricsSkeleton(periodGranularity, languageRecords, selectedPeriod);

                const metricsData = metricsSkeleton.map(item => ({
                    [colPeriod]: translateLabel(item.month),
                    [translate('dashboards.progress-and-mastery.reading-metrics.precision')]: item.precision,
                    [translate('dashboards.progress-and-mastery.reading-metrics.fluency')]: item.fluency,
                    [translate('dashboards.progress-and-mastery.reading-metrics.completeness')]: item.completeness,
                    [translate('dashboards.progress-and-mastery.reading-metrics.pronunciation')]: item.pronunciation,
                }));

                const unitsDataForExport = unitMetrics.map(item => ({
                    [colUnit]: `${translate('dashboards.progress-and-mastery.unit')} ${item.LevelCode}`,
                    [colPercentage]: item.CompletionRate,
                }));

                const comprehensionTrendData = buildComprehensionTrendSkeleton(periodGranularity, readingLessonsSeries, selectedPeriod)
                    .map(item => ({
                        [colPeriod]: translateLabel(item.label),
                        [`${translate('dashboards.progress-and-mastery.reading-comprehension-result')} (${translate('dashboards.progress-and-mastery.reading-results.percentage')})`]: item.percent,
                    }));

                dataToExport = {
                    [filtersSheetName]: buildStoryLanguageFiltersSheet(),
                    [sheetName('dashboards.progress-and-mastery.reading-comprehension-trend')]: comprehensionTrendData,
                    [sheetName('dashboards.progress-and-mastery.reading-performance-metrics')]: metricsData,
                    [sheetName('dashboards.progress-and-mastery.units-completed')]: unitsDataForExport,
                };

                fileName = `Reporte_Progreso_Dominio${fileNameSuffix}.xlsx`;
                break;
            }

            case 'Lectura y Contenido': {
                const colReadingMetric = translate('dashboards.export-key-reading-metrics');

                const languageContentMetrics = getLanguageSection(contentMetrics, storyLanguage);

                const keyMetricsReading = [
                    { [colReadingMetric]: translate('dashboards.usage-and-engagement.text-completion-rate'), [colValue]: languageContentMetrics?.CompletionRate ?? 0 },
                    { [colReadingMetric]: translate('dashboards.usage-and-engagement.readings-completed'), [colValue]: languageContentMetrics?.TotalTextsRead ?? 0 },
                ];

                const avgTimeSkeleton = buildSkeleton(periodGranularity, languageContentMetrics?.AvgTimeByPeriod ?? [], selectedPeriod, 'AvgTimeSeconds', 'avgTime');
                const avgTimeData = avgTimeSkeleton.map(item => ({
                    [colPeriod]: translateLabel(item.label),
                    [translate('dashboards.progress-and-mastery.average-time')]: item.avgTime,
                }));

                const volumeSkeleton = buildSkeleton(periodGranularity, languageContentMetrics?.PagesByPeriod ?? [], selectedPeriod, 'TotalPages', 'pages');
                const volumeData = volumeSkeleton.map(item => ({
                    [colPeriod]: translateLabel(item.label),
                    [translate('dashboards.reading-and-content.pages-read')]: item.pages,
                }));

                const categoriesData = (languageContentMetrics?.Categories ?? []).map(category => {
                    const translated = translate(`dashboards.reading-and-content.categories.${category.CategoryName}`);
                    return {
                        [colCategory]: translated.startsWith('Missing') ? category.CategoryName : translated,
                        [colReadings]: category.TotalReadings,
                    };
                });

                dataToExport = {
                    [filtersSheetName]: buildStoryLanguageFiltersSheet(),
                    [sheetName('dashboards.export-key-reading-metrics')]: keyMetricsReading,
                    [sheetName('dashboards.progress-and-mastery.average-time-per-reading')]: avgTimeData,
                    [sheetName('dashboards.reading-and-content.reading-volume')]: volumeData,
                    [sheetName('dashboards.reading-and-content.reading-categories')]: categoriesData,
                };

                fileName = `Reporte_Lectura_Contenido${fileNameSuffix}.xlsx`;
                break;
            }

            default:
                console.warn("Nombre de exportación no reconocido:", dataName);
                dataToExport = [{ error: 'Datos no encontrados para este reporte.' }];
                fileName = 'Reporte_Error.xlsx';
        }

        const hasDataToExport = Array.isArray(dataToExport)
            ? dataToExport.length > 0
            : Object.keys(dataToExport ?? {}).some(sheet =>
                sheet !== filtersSheetName && dataToExport[sheet]?.length > 0
            );

        if (!hasDataToExport) {
            alert("No hay datos disponibles para exportar en esta sección.");
            return;
        }

        try {
            const wb = XLSX.utils.book_new();
            if (typeof dataToExport === 'object' && !Array.isArray(dataToExport)) {
                
                for (const sheetName in dataToExport) {
                    const dataArray = dataToExport[sheetName];

                    if (dataArray && dataArray.length > 0) {
                        const ws = XLSX.utils.json_to_sheet(dataArray); 
                        XLSX.utils.book_append_sheet(wb, ws, sheetName);
                    }
                }
            } else {
                const ws = XLSX.utils.json_to_sheet(dataToExport);
                XLSX.utils.book_append_sheet(wb, ws, 'Datos'); 
            }

            XLSX.writeFile(wb, fileName);

        } catch (error) {
            console.error("Error al exportar a Excel:", error);
            alert("Hubo un error al generar el archivo Excel.");
        }
    };

    const sumSimpleField = (data, key) => 
        data.reduce((sum, item) => sum + (item[key] || 0), 0);

    const aggregateDailyArrays = (data, key, innerKey) => {
        if (data.length === 0) return [];
        const initialArray = data[0][key].map(item => ({ day: item.day, [innerKey]: 0 }));
        return data.reduce((acc, curr) => {
            curr[key].forEach((item, index) => {
                acc[index][innerKey] += item[innerKey] || 0;
            });
            return acc;
        }, initialArray);
    };

    const averageObjectFields = (data, key) => {
        if (data.length === 0 || !data[0][key] || data[0][key].length === 0) return [{}];
        const firstItemKeys = Object.keys(data[0][key][0]);
        const totalCount = data.length;
        const summed = data.reduce((acc, curr) => {
            firstItemKeys.forEach(field => {
                acc[field] = (acc[field] || 0) + (curr[key][0][field] || 0);
            });
            return acc;
        }, {});
        const averaged = firstItemKeys.reduce((acc, field) => ({ 
            ...acc, 
            [field]: Math.round(summed[field] / totalCount) 
        }), {});
        return [averaged];
    };

    // --- DYNAMIC DATA FILTERING LOGIC ---
    const filteredData = useMemo(() => {
        const baseUsageMetrics = mockUsageMetrics[userRole] || [];

        const defaultStudentName = 'Todos los estudiantes';
        const defaultGroupName = 'Todos los grupos';
        const defaultSchoolName = 'Todas las escuelas';
        

        let nameKey;
        let defaultName;
        
        let filteredUsage = {}; 
        let filteredExams = studentExamResults;
        let dataList = [];

        if (userRole === 'teacher' || userRole === 'parent') {
            nameKey = 'studentName';
            defaultName = defaultStudentName;
        } else if (userRole === 'director') {
            nameKey = 'groupName';
            defaultName = defaultGroupName;
        } else {
            nameKey = 'schoolName';
            defaultName = defaultSchoolName;
        }

        filteredUsage = baseUsageMetrics.find(d => d[nameKey] === defaultName) || {}; 

        if (Object.keys(filteredUsage).length === 0 && baseUsageMetrics.length > 0) {
            filteredUsage = baseUsageMetrics[0]; 
        }

        const individualEntities = baseUsageMetrics.filter(d => 
            (d.studentName && d.studentName !== defaultStudentName) || 
            (d.groupName && d.groupName !== defaultGroupName) ||
            (d.schoolName && d.schoolName !== defaultSchoolName)
        );

        if (userRole === 'admin' || userRole === 'adminOre') {
            const individualSchoolMetrics = individualEntities.filter(d => d.schoolName);

            if (selectedSchools.length > 0) {
                const selectedSchoolMetrics = individualSchoolMetrics.filter(d => 
                    selectedSchools.includes(d.schoolName)
                );
                dataList = selectedSchoolMetrics;
                
                if (selectedSchoolMetrics.length === 1) {
                    filteredUsage = selectedSchoolMetrics[0];
                    
                } else if (selectedSchoolMetrics.length > 1) {
                    const count = selectedSchoolMetrics.length;

                    filteredUsage = {
                        schoolName: `Agregado (${count} Escuelas)`,
                        totalSessions: sumSimpleField(selectedSchoolMetrics, 'totalSessions'),
                        totalActiveDays: sumSimpleField(selectedSchoolMetrics, 'totalActiveDays'),
                        averageSessionDuration: Math.round(sumSimpleField(selectedSchoolMetrics, 'averageSessionDuration') / count),
                        engagementScore: Math.round(sumSimpleField(selectedSchoolMetrics, 'engagementScore') / count),
                        sessionDuration: aggregateDailyArrays(selectedSchoolMetrics, 'sessionDuration', 'duration'),
                        sessionsPerDay: aggregateDailyArrays(selectedSchoolMetrics, 'sessionsPerDay', 'sessions'),
                        devicesUsage: averageObjectFields(selectedSchoolMetrics, 'devicesUsage'),
                        OffVsInSchoolUsage: averageObjectFields(selectedSchoolMetrics, 'OffVsInSchoolUsage'),
                    };
                } else {
                    filteredUsage = {};
                    dataList = [];
                }
            } 

            else if (userRole === 'admin' && selectedRegions.length > 0) {
                
                const adminMetrics = mockUsageMetrics['admin']?.[0];
                const totalRegionsData = adminMetrics?.totalRegions || [];

                const selectedRegionMetrics = totalRegionsData.filter(d => 
                    selectedRegions.includes(d.region)
                );
                
                dataList = selectedRegionMetrics; 
                
                if (selectedRegionMetrics.length === 1) {
                    filteredUsage = selectedRegionMetrics[0];
                } 
                else if (selectedRegionMetrics.length > 1) {
                    const count = selectedRegionMetrics.length;
                    filteredUsage = {
                        schoolName: `Agregado (${count} Regiones)`,
                        totalSessions: sumSimpleField(selectedRegionMetrics, 'totalSessions'),
                        totalActiveDays: sumSimpleField(selectedRegionMetrics, 'totalActiveDays'),
                        averageSessionDuration: Math.round(sumSimpleField(selectedRegionMetrics, 'averageSessionDuration') / count),
                        engagementScore: Math.round(sumSimpleField(selectedRegionMetrics, 'engagementScore') / count),
                        sessionDuration: aggregateDailyArrays(selectedRegionMetrics, 'sessionDuration', 'duration'),
                        sessionsPerDay: aggregateDailyArrays(selectedRegionMetrics, 'sessionsPerDay', 'sessions'),
                        devicesUsage: averageObjectFields(selectedRegionMetrics, 'devicesUsage'),
                        OffVsInSchoolUsage: averageObjectFields(selectedRegionMetrics, 'OffVsInSchoolUsage'),
                    };
                } else {
                    filteredUsage = {};
                    dataList = [];
                }
            }

        }
        else if (userRole === 'teacher' || userRole === 'parent' || userRole === 'director') {
            
            const individualStudentMetrics = individualEntities.filter(d => d.studentName);
            const groupMetrics = individualEntities.filter(d => d.groupName && d.groupName !== defaultGroupName);

            if (userRole === 'parent') {
                const fixedStudent = 'Sofía Martínez'; // Este es un mock fijo.
                const entityData = individualStudentMetrics.find(d => d.studentName === fixedStudent);
                
                if (entityData) {
                    filteredUsage = entityData;
                    dataList = [entityData];
                } else {
                    filteredUsage = {};
                    dataList = [];
                }
            }

            else if (userRole === 'teacher' && selectedStudents.length > 0) {
                
                const selectedStudentMetrics = individualStudentMetrics.filter(d => 
                    selectedStudents.includes(d.studentName)
                );
                
                dataList = selectedStudentMetrics;
                
                if (selectedStudentMetrics.length === 1) {
                    filteredUsage = selectedStudentMetrics[0];
                } else if (selectedStudentMetrics.length > 1) {
                    const count = selectedStudentMetrics.length;
                    filteredUsage = {
                        studentName: `Agregado (${count} estudiantes)`,
                        totalSessions: sumSimpleField(selectedStudentMetrics, 'totalSessions'),
                        totalActiveDays: sumSimpleField(selectedStudentMetrics, 'totalActiveDays'),
                        averageSessionDuration: Math.round(sumSimpleField(selectedStudentMetrics, 'averageSessionDuration') / count),
                        engagementScore: Math.round(sumSimpleField(selectedStudentMetrics, 'engagementScore') / count),
                        sessionDuration: aggregateDailyArrays(selectedStudentMetrics, 'sessionDuration', 'duration'),
                        sessionsPerDay: aggregateDailyArrays(selectedStudentMetrics, 'sessionsPerDay', 'sessions'),
                        devicesUsage: averageObjectFields(selectedStudentMetrics, 'devicesUsage'),
                        OffVsInSchoolUsage: averageObjectFields(selectedStudentMetrics, 'OffVsInSchoolUsage'),
                    };
                } else {
                    filteredUsage = {};
                    dataList = [];
                }
            } 

            else if ((userRole === 'director' || userRole === 'teacher') && selectedGroups.length > 0) {
                
                const selectedGroupMetrics = groupMetrics.filter(d => 
                    selectedGroups.includes(d.groupName)
                );
                
                dataList = selectedGroupMetrics;
                
                if (selectedGroupMetrics.length === 1) {
                    filteredUsage = selectedGroupMetrics[0];

                } else if (selectedGroupMetrics.length > 1) {
                    const count = selectedGroupMetrics.length;

                    const totalSessions = sumSimpleField(selectedGroupMetrics, 'totalSessions');
                    const totalActiveDays = sumSimpleField(selectedGroupMetrics, 'totalActiveDays');
                    const averageSessionDuration = sumSimpleField(selectedGroupMetrics, 'averageSessionDuration') / count;
                    const engagementScore = sumSimpleField(selectedGroupMetrics, 'engagementScore') / count;
                    
                    const sessionDurationSum = aggregateDailyArrays(selectedGroupMetrics, 'sessionDuration', 'duration');
                    const sessionsPerDaySum = aggregateDailyArrays(selectedGroupMetrics, 'sessionsPerDay', 'sessions');
                    
                    const devicesUsageAvg = averageObjectFields(selectedGroupMetrics, 'devicesUsage');
                    const OffVsInSchoolUsageAvg = averageObjectFields(selectedGroupMetrics, 'OffVsInSchoolUsage');
                    
                    filteredUsage = {
                        groupName: `Agregado (${count} Grupos)`,
                        totalSessions: totalSessions,
                        totalActiveDays: totalActiveDays,
                        averageSessionDuration: Math.round(averageSessionDuration),
                        engagementScore: Math.round(engagementScore),
                        sessionDuration: sessionDurationSum,
                        sessionsPerDay: sessionsPerDaySum,
                        devicesUsage: devicesUsageAvg,
                        OffVsInSchoolUsage: OffVsInSchoolUsageAvg,
                    };
                } else {
                    filteredUsage = {};
                    dataList = [];
                }
            }
        }

        if (selectedLanguages && selectedLanguages.length > 0) {
            filteredExams = studentExamResults
                .map(student => {
                    const filteredExamsForStudent = student.exams.filter(exam => 
                        selectedLanguages.includes(exam.subject)
                    );

                    if (filteredExamsForStudent.length > 0) {
                        return {
                            ...student,
                            exams: filteredExamsForStudent,
                        };
                    }
                    return null;
                })
                .filter(student => student !== null);
        }
        
        const newDiagnostics = diagnosticsData.map(d => {
            if (d.month === 'Diciembre') {
                const regionCount = selectedRegions.length;
                let newScore = 85 - (regionCount * 5);
                newScore = Math.max(50, newScore);
                return { ...d, score: newScore, status: 'completed' };
            }
            return d;
        });

        return {
            usageMetrics: filteredUsage, 
            dataList: dataList,         
            examResults: filteredExams,
            diagnostics: newDiagnostics,
        };

    }, [userRole, selectedStudents, selectedGroups, selectedRegions, selectedSchools, studentExamResults, diagnosticsData, selectedLanguages]);
    
    const { usageMetrics, examResults, diagnostics } = filteredData;

    // Map the module-usage response into the UsageGapsChart shape per role:
    // teacher = students with a used flag;
    // admin/adminOre = region usage, or the school's usage when a school is selected.
    // director = groups with usage percentage and students summary.
    const moduleUsageData = useMemo(() => {
        const empty = { admin: [], adminOre: [], director: [], teacher: [] };
        if (!moduleUsage) return empty;

        const regions = moduleUsage.ByRegion ?? [];
        const drilledRegion = regions.find(region => Array.isArray(region.Schools?.Items));
        const schoolItems = (drilledRegion?.Schools?.Items ?? []).map(school => ({
            id: school.SchoolId,
            name: school.SchoolName,
            usage: school.UsagePercent,
            totalStudents: school.TotalStudents,
            usedStudents: school.UsedStudents,
        }));

        const regionItems = regions.map(region => ({
            id: region.RegionId,
            name: region.RegionName,
            usage: region.UsagePercent,
            totalStudents: region.TotalStudents,
            usedStudents: region.UsedStudents,
        }));

        const teacherItems = (moduleUsage.Students ?? []).map(student => ({
            id: student.UserId,
            name: student.Name,
            isUsing: student.HasUsed,
        }));

        const directorItems = (moduleUsage.Teachers ?? []).flatMap(teacher =>
            (teacher.Groups ?? []).map(group => ({
                id: group.GroupId,
                name: group.GroupName,
                teacherName: teacher.TeacherName,
                usage: group.UsagePercent,
                totalStudents: group.TotalStudents,
                usedStudents: group.UsedStudents,
            }))
        );

        // Prefer school items when a school is selected, else regions.
        const usageItems = schoolItems.length > 0 ? schoolItems : regionItems;

        if (userRole === 'teacher') return { ...empty, teacher: teacherItems };
        if (userRole === 'adminOre') return { ...empty, adminOre: usageItems };
        if (userRole === 'admin') return { ...empty, admin: usageItems };
        if (userRole === 'director') return { ...empty, director: directorItems };
        
        return empty;
    }, [moduleUsage, userRole]);

    const aggregateDiagnosticsData = (dataArray) => {
        if (!dataArray || dataArray.length === 0) {
            return [];
        }

        const aggregatedMetrics = new Map();

        dataArray.forEach(entity => {
            if (Array.isArray(entity?.diagnostics) && entity.diagnostics.length > 0) {
                entity.diagnostics.forEach(diagnostic => {
                    const month = diagnostic.month;
                    
                    if (!aggregatedMetrics.has(month)) {
                        aggregatedMetrics.set(month, { 
                            sumScore: 0, 
                            completedCount: 0,
                            hasPending: false,
                        });
                    }
                    
                    const metrics = aggregatedMetrics.get(month);
                    
                    if (diagnostic.status === 'completed') {
                        metrics.sumScore += diagnostic.score || 0;
                        metrics.completedCount++;
                    } else if (diagnostic.status === 'pending') {
                        metrics.hasPending = true;
                    }
                    aggregatedMetrics.set(month, metrics);
                });
            }
        });

        const result = Array.from(aggregatedMetrics).map(([month, metrics]) => {
            let finalStatus = 'completed';
            let finalScore = 0;
            
            if (metrics.completedCount > 0) {
                finalScore = Math.round(metrics.sumScore / metrics.completedCount);
            } else if (metrics.hasPending) {
                finalStatus = 'pending';
                finalScore = 0;
            }

            return {
                month: month,
                score: finalScore,
                status: finalStatus
            };
        }).filter(item => item.score > 0 || item.status === 'pending');

        return result;
    };

    const chartDiagnosticsData = useMemo(() => {
        
        const baseData = diagnosticsMockedData[userRole] || []; 
        let entityNameKey;
        let selectedName;

        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }
        

        if (userRole === 'teacher') {
            if (selectedStudents.length > 1) {
                const selectedStudentData = baseData.filter(d => selectedStudents.includes(d.studentName));
                return aggregateDiagnosticsData(selectedStudentData);
            } else if (selectedStudents.length === 1) {
                selectedName = selectedStudents[0];
            } 
            else if (selectedGroups.length > 1) {
                const allStudentsData = baseData.find(d => d.studentName === 'Todos los estudiantes');
                if (allStudentsData) {
                    return allStudentsData.diagnostics;
                }
            } else if (selectedGroups.length === 1) {
                selectedName = selectedGroups[0];
            }
        } 
        else if (userRole === 'director') {
            if (selectedGroups.length > 1) {
                const selectedGroupData = baseData.filter(d => selectedGroups.includes(d.groupName));
                return aggregateDiagnosticsData(selectedGroupData);
            } else if (selectedGroups.length === 1) {
                selectedName = selectedGroups[0];
            }
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateDiagnosticsData(selectedRegionData); 
            } 
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateDiagnosticsData(selectedSchoolData); 
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre') {
            if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateDiagnosticsData(selectedSchoolData);
            } else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.diagnostics) {
            return entityData.diagnostics;
        } 
        
        const defaultData = baseData.find(d => d[entityNameKey]?.startsWith('Todos los') || d[entityNameKey]?.startsWith('Todas las'));
        return defaultData?.diagnostics || [];

    }, [userRole, selectedStudents, selectedGroups, selectedSchools,selectedRegions]);

    const aggregateReadingChartsData = (dataArray) => {
        if (!dataArray || dataArray.length === 0) {
            return [];
        }

        const aggregatedSums = {};
        let validCount = 0;

        dataArray.forEach(entity => {
            if (Array.isArray(entity?.data) && entity.data.length > 0) {
                validCount++;
                
                entity.data.forEach((chartItem, index) => {
                    if (!aggregatedSums[index]) {
                        aggregatedSums[index] = {};
                    }

                    Object.keys(chartItem).forEach(key => {
                        const value = chartItem[key];
                        
                        if (typeof value === 'number') {
                            if (!aggregatedSums[index][key]) {
                                aggregatedSums[index][key] = 0;
                            }
                            aggregatedSums[index][key] += value;
                        }
                    });
                });
            }
        });
        
        if (validCount === 0) {
            return [];
        }

        const firstEntityData = dataArray.find(d => d.data && d.data.length > 0)?.data;
        if (!firstEntityData) return [];
        
        const result = firstEntityData.map((originalItem, index) => {
            const newItem = { ...originalItem };
            
            Object.keys(originalItem).forEach(key => {
                if (typeof originalItem[key] === 'number' && aggregatedSums[index] && aggregatedSums[index][key] !== undefined) {
                    // Promediar y redondear
                    newItem[key] = Math.round(aggregatedSums[index][key] / validCount);
                }
            });
            return newItem;
        });

        return result;
    };

    const chartReadingData = useMemo(() => {
        const baseData = readingChartsMockedData[userRole] || []; 
        let entityNameKey;
        let selectedName;

        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }
        
        if (userRole === 'teacher') {
            if (selectedStudents.length > 1) {
                const selectedStudentData = baseData.filter(d => selectedStudents.includes(d.studentName));
                return aggregateReadingChartsData(selectedStudentData);
            } else if (selectedStudents.length === 1) {
                selectedName = selectedStudents[0];
            } 
            else if (selectedGroups.length > 1) {
                const allStudentsData = baseData.find(d => d.studentName === 'Todos los estudiantes');

                if (allStudentsData && allStudentsData.data) {
                    return allStudentsData.data;
                }
            } else if (selectedGroups.length === 1) {
                selectedName = selectedGroups[0];
            }
        } 
        else if (userRole === 'director' && selectedGroups.length > 0) {
            if (selectedGroups.length > 1) {
                const selectedGroupData = baseData.filter(d => selectedGroups.includes(d.groupName));
                return aggregateReadingChartsData(selectedGroupData);
            }
            selectedName = selectedGroups[0];
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateReadingChartsData(selectedRegionData); 
            }
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateReadingChartsData(selectedSchoolData); 
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre') {
            if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateReadingChartsData(selectedSchoolData);
            } else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.data) {
            return entityData.data;
        } 

        const defaultData = baseData.find(d => d[entityNameKey]?.startsWith('Todos los') || d[entityNameKey]?.startsWith('Todas las'));
        return defaultData?.data || [];

    }, [userRole, selectedStudents, selectedGroups, selectedSchools,selectedRegions]);

    const aggregateSkillsProgressData = (dataArray) => {
        if (!dataArray || dataArray.length === 0) {
            return [];
        }

        const aggregatedMap = new Map();
        let validCount = 0; 

        dataArray.forEach(item => {
            if (Array.isArray(item?.data) && item.data.length > 0) {
                validCount++;
                
                item.data.forEach(metric => {
                    const skill = metric.skill;
                    const value = metric.value || 0; 
                    
                    if (!aggregatedMap.has(skill)) {
                        aggregatedMap.set(skill, 0);
                    }
                    aggregatedMap.set(skill, aggregatedMap.get(skill) + value);
                });
            }
        });

        if (validCount === 0) {
            return [];
        }

        const result = Array.from(aggregatedMap).map(([skill, sum]) => ({
            skill: skill,
            value: Math.round(sum / validCount)
        }));

        return result;
    };

    const chartSkillsData = useMemo(() => {
        const baseData = skillsProgressMockedData[userRole] || []; 
        let entityNameKey;
        let selectedName;

        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }
        
        if (userRole === 'teacher') {
            if (selectedStudents.length > 0) {
                selectedName = selectedStudents.length === 1 ? selectedStudents[0] : `Agregado (${selectedStudents.length} estudiantes)`;
            } else if (selectedGroups.length > 0) {
                selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
            }
        } 
        else if (userRole === 'director' && selectedGroups.length > 0) {
            selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateSkillsProgressData(selectedRegionData); 
            } 
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateSkillsProgressData(selectedSchoolData); 
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre') {
            if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateSkillsProgressData(selectedSchoolData);
            } else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.data) {
            return entityData.data;
        } 
        
        const defaultData = baseData.find(d => d[entityNameKey]?.startsWith('Todos los') || d[entityNameKey]?.startsWith('Todas las'));
        return defaultData ? defaultData.data : [];

    }, [userRole, selectedStudents, selectedGroups, selectedSchools,selectedRegions]);

    const aggregateUnitsCompletedData = (dataArray) => {
        if (!dataArray || dataArray.length === 0) {
            return [];
        }

        const aggregatedMap = new Map();
        let validCount = 0; 

        dataArray.forEach(item => {
            if (Array.isArray(item?.data) && item.data.length > 0) {
                validCount++;
                
                item.data.forEach(metric => {
                    const unit = metric.unit;
                    const percentageValue = metric.percentage || 0;
                    
                    if (!aggregatedMap.has(unit)) {
                        aggregatedMap.set(unit, 0);
                    }
                    aggregatedMap.set(unit, aggregatedMap.get(unit) + percentageValue);
                });
            }
        });

        if (validCount === 0) {
            return [];
        }

        const result = Array.from(aggregatedMap).map(([unit, sum]) => ({
            unit: unit,
            percentage: Math.round(sum / validCount) 
        }));

        return result;
    };

    const chartUnitsData = useMemo(() => {
        const baseData = unitsCompletedMockedData[userRole] || []; 
        let entityNameKey;
        let selectedName;
        
        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }

        if (userRole === 'teacher') {
            if (selectedStudents.length > 0) {
                selectedName = selectedStudents.length === 1 ? selectedStudents[0] : `Agregado (${selectedStudents.length} estudiantes)`;
            } else if (selectedGroups.length > 0) {
                selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
            }
        } 
        else if (userRole === 'director' && selectedGroups.length > 0) {
            selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateUnitsCompletedData(selectedRegionData); 
            } 
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateUnitsCompletedData(selectedSchoolData); 
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre') {
            if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateUnitsCompletedData(selectedSchoolData);
            } else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.data) {
            return entityData.data;
        } 
        
        const defaultData = baseData.find(d => d[entityNameKey]?.startsWith('Todos los'));
        return defaultData ? defaultData.data : [];

    }, [userRole, selectedStudents, selectedGroups, selectedSchools, selectedRegions]);

    const aggregateVolumeData = (dataArray) => {
        if (!dataArray || dataArray.length === 0) {
            return [];
        }

        const aggregatedMap = new Map();
        let validCount = 0; 

        dataArray.forEach(item => {
            if (Array.isArray(item?.data) && item.data.length > 0) {
                validCount++;
                
                item.data.forEach(metric => {
                    const session = metric.session;
                    const volumeValue = metric.volume || 0;
                    
                    if (!aggregatedMap.has(session)) {
                        aggregatedMap.set(session, 0);
                    }
                    aggregatedMap.set(session, aggregatedMap.get(session) + volumeValue);
                });
            }
        });

        if (validCount === 0) {
            return [];
        }

        const result = Array.from(aggregatedMap).map(([session, sum]) => ({
            session: session,
            volume: Math.round(sum / validCount)
        }));

        return result;
    };

    const chartVolumeData = useMemo(() => {
        const baseData = readingVolumeMockedData[userRole] || []; 
        let entityNameKey;
        let selectedName;
        
        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }
        
        if (userRole === 'teacher') {
            if (selectedStudents.length > 0) {
                selectedName = selectedStudents.length === 1 ? selectedStudents[0] : `Agregado (${selectedStudents.length} estudiantes)`;
            } else if (selectedGroups.length > 0) {
                selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
            }
        } 
        else if (userRole === 'director' && selectedGroups.length > 0) {
            selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateVolumeData(selectedRegionData); 
            } 
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateVolumeData(selectedSchoolData); 
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre') {
            if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateVolumeData(selectedSchoolData);
            } else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.data) {
            return entityData.data;
        } 
        
        const defaultData = baseData.find(d => d[entityNameKey]?.startsWith('Todos los'));
        return defaultData ? defaultData.data : [];

    }, [userRole, selectedStudents, selectedGroups, selectedSchools, selectedRegions]);


    const aggregateChartData = (dataArray) => {
        if (!dataArray || dataArray.length === 0) {
            return [];
        }

        const aggregatedMap = new Map();
        let validCount = 0; 

        dataArray.forEach(item => {
            if (Array.isArray(item?.data) && item.data.length > 0) {
                validCount++;
                
                item.data.forEach(metric => {
                    const topic = metric.topic;
                    const completedValue = metric.completed || 0; 
                    
                    if (!aggregatedMap.has(topic)) {
                        aggregatedMap.set(topic, 0);
                    }
                    aggregatedMap.set(topic, aggregatedMap.get(topic) + completedValue);
                });
            }
        });

        if (validCount === 0) {
            return [];
        }

        const result = Array.from(aggregatedMap).map(([topic, sum]) => ({
            topic: topic,
            completed: Math.round(sum / validCount) // Promedia por el conteo válido
        }));

        return result;
    };

    const chartCategoriesData = useMemo(() => {
        
        const baseData = readingCategoriesMockedData[userRole] || [];
        let entityNameKey;
        let selectedName;
        
        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }
        
        if (userRole === 'teacher') {
            if (selectedStudents.length > 0) {
                selectedName = selectedStudents.length === 1 ? selectedStudents[0] : `Agregado (${selectedStudents.length} estudiantes)`;
            } else if (selectedGroups.length > 0) {
                selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
            }
        } 
        else if (userRole === 'director' && selectedGroups.length > 0) {
            selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateChartData(selectedRegionData); 
            } 
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateChartData(selectedSchoolData); 
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre') {
            if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateChartData(selectedSchoolData);
            } else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.data) {
            return entityData.data;
        } 
        
        const defaultData = baseData.find(d => d[entityNameKey]?.startsWith('Todos los'));
        return defaultData ? defaultData.data : [];

    }, [userRole, selectedStudents, selectedGroups, selectedSchools, selectedRegions]);

    const aggregateAverageTimeData = (dataArray) => {
        if (!dataArray || dataArray.length === 0) {
            return [];
        }

        const aggregatedMap = new Map();
        let validCount = 0; 

        dataArray.forEach(item => {
            if (Array.isArray(item?.data) && item.data.length > 0) {
                validCount++;
                
                item.data.forEach(metric => {
                    const month = metric.month;
                    const levelValue = metric.level || 0;
                    
                    if (!aggregatedMap.has(month)) {
                        aggregatedMap.set(month, 0);
                    }
                    aggregatedMap.set(month, aggregatedMap.get(month) + levelValue);
                });
            }
        });

        if (validCount === 0) {
            return [];
        }

        const result = Array.from(aggregatedMap).map(([month, sum]) => ({
            month: month,
            level: Math.round(sum / validCount)
        }));

        return result;
    };

    const chartAverageTimeData = useMemo(() => {
        const baseData = averageTimeReadingMockedData[userRole] || []; 
        let entityNameKey;
        let selectedName;
        
        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }
        
        if (userRole === 'teacher') {
            if (selectedStudents.length > 0) {
                selectedName = selectedStudents.length === 1 ? selectedStudents[0] : `Agregado (${selectedStudents.length} estudiantes)`;
            } else if (selectedGroups.length > 0) {
                selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
            }
        } 
        else if (userRole === 'director' && selectedGroups.length > 0) {
            selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateAverageTimeData(selectedRegionData); 
            } 
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateAverageTimeData(selectedSchoolData); 
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre') {
            if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateAverageTimeData(selectedSchoolData);
            } else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.data) {
            return entityData.data;
        } 
        
        const defaultData = baseData.find(d => d[entityNameKey]?.startsWith('Todos los'));
        return defaultData ? defaultData.data : [];

    }, [userRole, selectedStudents, selectedGroups, selectedSchools, selectedRegions]);

    const aggregateKeyMetrics = (metricsArray) => {
        if (!metricsArray || metricsArray.length === 0) {
            return { completionRate: 0, totalReadings: 0 };
        }

        const sumRates = metricsArray.reduce((sum, current) => sum + current.data.completionRate, 0);
        const sumReadings = metricsArray.reduce((sum, current) => sum + current.data.totalReadings, 0);
        const count = metricsArray.length;

        return {
            completionRate: Math.round(sumRates / count),
            totalReadings: Math.round(sumReadings / count)
        };
    };

    const chartKeyMetricsData = useMemo(() => {
        const baseData = readingKeyMetricsMockedData[userRole] || [];
        let entityNameKey;
        let selectedName;
        
        if (userRole === 'teacher' || userRole === 'parent') {
            entityNameKey = 'studentName';
            selectedName = 'Todos los estudiantes';
        } else if (userRole === 'director') {
            entityNameKey = 'groupName';
            selectedName = 'Todos los grupos';
        } else {
            entityNameKey = 'schoolName';
            selectedName = 'Todas las escuelas';
        }
        
        if (userRole === 'teacher') {
            if (selectedStudents.length > 0) {
                selectedName = selectedStudents.length === 1 ? selectedStudents[0] : `Agregado (${selectedStudents.length} estudiantes)`;
            } else if (selectedGroups.length > 0) {
                selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
            }
        } 
        else if (userRole === 'director' && selectedGroups.length > 0) {
            selectedName = selectedGroups.length === 1 ? selectedGroups[0] : `Agregado (${selectedGroups.length} Grupos)`;
        } 
        else if (userRole === 'admin') {
            if (selectedRegions.length > 1) {
                const selectedRegionData = baseData.filter(d => selectedRegions.includes(d.schoolName));
                return aggregateKeyMetrics(selectedRegionData); // ✅ Agregación dinámica
            } 
            else if (selectedRegions.length === 1) {
                selectedName = selectedRegions[0];
            }
            else if (selectedSchools.length > 1) {
                const selectedSchoolData = baseData.filter(d => selectedSchools.includes(d.schoolName));
                return aggregateKeyMetrics(selectedSchoolData); // ✅ Agregación dinámica
            }
            else if (selectedSchools.length === 1) {
                selectedName = selectedSchools[0];
            }
        }
        else if (userRole === 'adminOre' && selectedSchools.length > 0) {
            selectedName = selectedSchools.length === 1 ? selectedSchools[0] : 'Agregado (Múltiples Escuelas)';
        }
        else if (userRole === 'parent') {
            selectedName = 'Sofía Martínez'; 
        }
        
        const entityData = baseData.find(d => d[entityNameKey] === selectedName);

        if (entityData && entityData.data) {
            return entityData.data;
        } 
        
        const defaultDataEntity = baseData.find(d => d[entityNameKey]?.startsWith('Todos los') || d[entityNameKey]?.startsWith('Todas las'));
        
        return defaultDataEntity?.data || { 
            completionRate: 0,
            totalReadings: 0,
            averageScore: 0
        };

    }, [userRole, selectedStudents, selectedGroups, selectedSchools, selectedRegions]);

    useEffect(() => {
        setContentOpacity(1);
    }, [filterDependency]);

    // --- LESSON CARD LOGIC ---
    const lessonsPerPage = 5;
    const [currentPage, setCurrentPage] = useState(1);
    const currentLessonList = lessonsCardTab === 'actividades' ? activitiesAndSuggestions : readingLessons;
    const totalPages = Math.ceil(currentLessonList.length / lessonsPerPage);

    const indexOfLastLesson = currentPage * lessonsPerPage;
    const indexOfFirstLesson = indexOfLastLesson - lessonsPerPage;
    const paginatedLessons = currentLessonList.slice(indexOfFirstLesson, indexOfLastLesson);

    const paginate = (pageNumber) => {
        if (pageNumber > 0 && pageNumber <= totalPages) {
            setCurrentPage(pageNumber);
        }
    };
    const toggleLessonsCardTab = (tab) => {
        setLessonsCardTab(tab);
        setCurrentPage(1);
    };

    const handleViewResults = () => {
        setDiagnosticResultsModalOpen(prev => !prev);
    }

    const handleViewPdf = async (lessonId) => {
        setLoadingLessonId(lessonId);
        try {
            const response = await getLessonPDF(lessonId);
            window.open(response.data, '_blank');
        } catch (error) {
            console.error("Error al abrir el PDF:", error);
        } finally {
            setLoadingLessonId(null);
        }
    };

    const renderPagination = () => {
        const pageNumbers = [];
        for (let i = 1; i <= totalPages; i++) {
            pageNumbers.push(i);
        }

        return (
            <Pagination className="mt-4 mb-0 justify-content-center" size="sm">
                <PaginationItem disabled={currentPage === 1}>
                    <PaginationLink previous onClick={() => paginate(currentPage - 1)} />
                </PaginationItem>
                {pageNumbers.map(number => (
                    <PaginationItem key={number} active={number === currentPage}>
                        <PaginationLink onClick={() => paginate(number)}>
                            {number}
                        </PaginationLink>
                    </PaginationItem>
                ))}
                <PaginationItem disabled={currentPage === totalPages}>
                    <PaginationLink next onClick={() => paginate(currentPage + 1)} />
                </PaginationItem>
            </Pagination>
        );
    };

    // --- NEW MOCK FUNCTION FOR STUDENT ASSIGNMENT ---
    const getMockAssignedStudent = (lesson, selectedStudent) => {
        if (selectedStudent) {
            return selectedStudent;
        }
        // Mock logic to show different students for different mock lessons
        if (lesson.id % 3 === 0) return "Sofía Martínez";
        if (lesson.id % 3 === 1) return "Alejandro Vargas";
        return "Grupo Seleccionado";
    };

    const ContentAndLessonsCard = useMemo(() => (
        <Card className="shadow-lg border-0 mt-2">
            <CardBody className="p-0 d-flex flex-column">
                <Nav tabs className="pt-2 px-3">
                    {/* <NavItem>
                        <nav
                            className={lessonsCardTab === "actividades"
                                ? "nav-link active bg-secondary text-white font-weight-bold"
                                : "nav-link  font-weight-bold bg-light"}
                            onClick={() => toggleLessonsCardTab("actividades")}
                            style={{cursor: 'pointer'}}
                        >
                            <Translate id="dashboards.activities-and-suggestions" />
                        </nav>
                    </NavItem> */}
                    {userRole !== "parent" ? <NavItem>
                        <nav
                            className={lessonsCardTab === "reading"
                                ? "nav-link active bg-secondary text-white font-weight-bold"
                                : "nav-link  font-weight-bold bg-light"}
                            onClick={() => toggleLessonsCardTab("reading")}
                            style={{cursor: 'pointer'}}
                        >
                            <Translate id="dashboards.reading-lessons" />
                        </nav>
                    </NavItem> : null}
                </Nav>

                <div className="p-3 flex-grow-1">
                    <TabContent activeTab={lessonsCardTab}>
                        {/* <TabPane tabId="actividades">
                            <ul className="list-group list-group-flush">
                                {paginatedLessons.map((lesson, index) => {
                                    const assignedStudent = userRole === "teacher"
                                        ? getMockAssignedStudent(lesson, primarySelectedStudent)
                                        : null;

                                    const isLoading = loadingLessonId === lesson.lessonId;

                                    return (
                                        <li key={lesson.id}
                                            className={`list-group-item list-group-item-action d-flex justify-content-between align-items-center ${index % 2 === 0 ? 'bg-white' : 'bg-light'}`}
                                        >
                                            <div>
                                                <strong>{lesson.title}</strong>
                                                <small className="d-block title-primary">
                                                    <Translate id="dashboards.code-standard" />: {lesson.standardCode}</small>

                                                {assignedStudent && (
                                                    <small className="d-block text-muted font-italic mt-1">
                                                        <Translate id="dashboards.recommendation-for" />: <span className="text-dark font-weight-bold">{assignedStudent}</span>
                                                    </small>
                                                )}

                                            </div>
                                            <button
                                                size="sm"
                                                className="btn-default button-primary-custom"
                                                outline
                                                onClick={() => handleViewPdf(lesson.lessonId)}
                                                disabled={isLoading}
                                            >
                                                {isLoading ? (
                                                    <PulseLoader size={6} color="#fff" />
                                                ) : (
                                                    <>
                                                        <FontAwesomeIcon icon={faFilePdf} className="mr-1" />
                                                        <Translate id="dashboards.view-pdf" />
                                                    </>
                                                )}
                                            </button>
                                        </li>
                                    );
                                })}
                            </ul>
                        </TabPane> */}
                        <TabPane tabId="reading">
                            <ul className="list-group list-group-flush">
                                <PronunciationLessons onLessonsChange={setPronunciationLessonsData} />
                            </ul>
                        </TabPane>
                    </TabContent>
                </div>

                <div className="border-top p-3 mt-auto d-flex justify-content-between align-items-center">
                    {/* {lessonsCardTab === 'actividades' && renderPagination()} */}
                    {/* Export Button for Content/Lessons */}
                    <Button
                        color="success"
                        outline
                        size="sm"
                        onClick={() => handleExportToExcel(`Lecciones - ${lessonsCardTab === 'actividades' ? 'Actividades' : 'Lectura'}`)}
                    >
                        <FontAwesomeIcon icon={faFileExcel} className="mr-1" />
                        <Translate id="dashboards.export-to-excel" />
                    </Button>
                </div>
            </CardBody>
        </Card>
    ), [lessonsCardTab, paginatedLessons, userRole, primarySelectedStudent, loadingLessonId]);

    const DashboardContent = (
        <>
            {/* Global Dropdowns Section (Filters) - (REMAINS THE SAME) */}
            <div className="shadow border-0 my-4 bg-light">
                <div style={{ padding: '8px 15px' }}>
                    <div className="row align-items-end">
                        {!hideRegions ?
                            <div className="col-md-2" style={{marginBottom: '4px', paddingLeft: '0px', opacity: disabledRegions ? 0.5 : 1}}>
                                <Label
                                    htmlFor="region-select"
                                    className="font-weight-bold text-dark"
                                    style={{marginBottom: '1px'}}
                                >
                                    <Translate id="dashboards.region"/>
                                </Label>
                                <SingleSelectDropdown
                                    label={translate("dashboards.region")}
                                    placeholder={translate("dashboards.select-region")}
                                    options={realRegions.map(r => ({label: r.Name, value: r.Id}))}
                                    selectedValue={userRole === "adminOre" ? (realRegions[0]?.Id ?? null) : selectedRegionId}
                                    disabled={disabledRegions}
                                    onSelect={handleRegionChange}
                                />
                            </div> : null}

                        {!hideRegions ?
                            <div className="col-md-2" style={{marginBottom: '4px', paddingLeft: '0px', opacity: disabledSchools ? 0.5 : 1}}>
                                <Label
                                    htmlFor="school-select"
                                    className="font-weight-bold text-dark"
                                    style={{marginBottom: '1px'}}
                                >
                                    <Translate id="dashboards.school"/>
                                </Label>
                                <SchoolSelectDropdown
                                    label={translate("dashboards.school")}
                                    placeholder={translate("dashboards.select-school")}
                                    translate={translate}
                                    selectedId={selectedSchoolId}
                                    selectedName={selectedSchools[0] || null}
                                    onSelect={handleSchoolChange}
                                    disabled={disabledSchools}
                                />
                            </div> : null}

                        {!hideStudents ?
                            <div className="col-md-3" style={{marginBottom: '4px', paddingLeft: '0px', opacity: disabledStudents ? 0.5 : 1}}>
                                <Label
                                    htmlFor="student-select"
                                    className="font-weight-bold text-dark"
                                    style={{marginBottom: '1px'}}
                                >
                                    <Translate id="dashboards.student"/>
                                </Label>
                                <MultiSelectDropdown
                                    label={translate("dashboards.student")}
                                    placeholder={translate("dashboards.select-student")}
                                    emptyMessage={translate("dashboards.no-students")}
                                    options={realStudents.map(student => ({label: `${student.GivenName} ${student.Surname}`, value: student.UserId }))}
                                    selectedValues={studentsFromGroups ? [] : selectedStudents}
                                    disabled={disabledStudents}
                                    setSelectedValues={(values) => {
                                        setSelectedGroups([]);
                                        setStudentsFromGroups(false);
                                        setSelectedStudents(values);
                                    }}
                                />
                            </div> : null}

                        {!hideGroups ?
                            <div className="col-md-3" style={{marginBottom: '4px', paddingLeft: '0px', opacity: disabledGroups ? 0.5 : 1}}>
                                <Label 
                                    htmlFor="group-select" 
                                    className="font-weight-bold text-dark"
                                    style={{marginBottom: '1px'}}
                                >
                                    <Translate id="dashboards.group"/>
                                </Label>
                                <MultiSelectDropdown
                                    label={translate("dashboards.group")}
                                    placeholder={translate("dashboards.select-group")}
                                    emptyMessage={translate("dashboards.no-groups")}
                                    options={realGroups.map(g => ({label: g.Name, value: g.Id}))}
                                    selectedValues={selectedGroups}
                                    disabled={disabledGroups}
                                    setSelectedValues={(values) => handleGroupsChange(values)}
                                />
                            </div> : null}

                        {/* <div className="col-md-2" style={{marginBottom: '4px'}}>
                            <Label 
                                htmlFor="start-date" 
                                className="font-weight-bold text-dark"
                                style={{marginBottom: '1px'}}
                            >
                                <Translate id="dashboards.date-from"/>
                            </Label>
                            <Input
                                type="date"
                                id="start-date"
                                className="form-control form-control-sm"
                                value={startDate}
                                onChange={(e) => {
                                    updateFilterAndRerender(setStartDate, e.target.value);
                                    setSelectedGroups([]);
                                }}
                            />
                        </div> */}
                        {/* <div className="col-md-2" style={{marginBottom: '4px'}}>
                            <Label 
                                htmlFor="end-date" 
                                className="font-weight-bold text-dark"
                                style={{marginBottom: '1px'}}
                            >
                                <Translate id="dashboards.date-to"/>
                            </Label>
                            <Input
                                type="date"
                                id="end-date"
                                className="form-control form-control-sm"
                                value={endDate}
                                onChange={(e) => {
                                    updateFilterAndRerender(setEndDate, e.target.value);
                                    setSelectedGroups([]);
                                }}
                            />
                        </div> */}

                        {/* <div className="col-md-2" style={{marginBottom: '4px'}}>
                            <Label 
                                htmlFor="language-select" 
                                className="font-weight-bold text-dark"
                                style={{marginBottom: '1px'}}
                            >
                                <Translate id="dashboards.language"/>
                            </Label>
                            <MultiSelectDropdown
                                label={translate("dashboards.language")}
                                placeholder={translate("dashboards.select-language")}
                                options={languages.map(lang => ({label: lang, value: lang}))}
                                selectedValues={selectedLanguages}
                                setSelectedValues={(values) => updateFilterAndRerender(setSelectedLanguages, values)}
                            />
                        </div> */}

                        {/* Focus Area Dropdown (Conditional Single Select remains) */}

                        {/* {!hideSkill ?
                            <div className="col-md-3" style={{marginBottom: '4px'}}>
                                <Label 
                                    htmlFor="focus-area-select" 
                                    className="font-weight-bold text-dark"
                                    style={{marginBottom: '1px'}}
                                >
                                    <Translate id="dashboards.focus-area"/>
                                </Label>
                                <MultiSelectDropdown
                                    label={translate("dashboards.focus-area")}
                                    placeholder={translate("dashboards.select-area")}
                                    options={focusAreas.map(area => ({
                                        label: `${area.name} (${area.en})`,
                                        value: area.name
                                    }))}
                                    selectedValues={selectedFocusAreas}
                                    setSelectedValues={(values) => updateFilterAndRerender(setSelectedFocusAreas, values)}
                                />
                            </div> : null } */}


                        {/* Clear Filters Button (No Change) */}
                        <div className="d-flex align-items-end"
                             style={{
                                 maxWidth: '80px',
                                 flexGrow: 0,
                                 paddingLeft: '7.5px',
                                 paddingRight: '7.5px',
                                 marginBottom: '4px'
                             }}>
                            <Button
                                color="danger"
                                outline
                                size="sm"
                                onClick={resetFilters}
                                className="w-100"
                            >
                                <FontAwesomeIcon icon={faTimes}/>
                            </Button>
                        </div>
                    </div>
                </div>
            </div>

            {/* --- CONTENT CONTAINER WITH FADE EFFECT --- */}
            <div style={{
                opacity: contentOpacity,
                transition: 'opacity 0.3s ease-in-out',
                position: 'relative',
                minHeight: '400px'
            }}>

                {contentOpacity < 1 && (
                    <div className="d-flex justify-content-center align-items-center"
                         style={{
                             position: 'absolute',
                             top: '50px', left: 0, right: 0,
                             zIndex: 10,
                         }}>
                        <FontAwesomeIcon icon={faSpinner} spin size="2x" className="title-secondary" />
                    </div>
                )}


                {/* Diagnostic and Exam Results / Average Lessons Results */}
                <div className="row mb-4">
                    {/* Hidden for now since it's using mock data */}
                    {/* <div className="col-lg-6 mb-4">
                        <Card className="shadow-sm border-0">
                            <CardBody>
                                {userRole === "teacher" ?
                                    <div className="d-flex justify-content-between align-items-center mb-3">
                                        <button onClick={toggleModal} className="btn-sm font-weight-bold btn-default button-secondary-custom">
                                            <FontAwesomeIcon icon={faCalendarAlt} className="mr-1"/> 
                                            <Translate id="dashboards.assign-diagnostic" />
                                        </button>
                                        <button className="btn-default button-primary-custom" onClick={handleViewResults} disabled={selectedStudents.length === 0}>
                                            <FontAwesomeIcon icon={faEye} className="mr-1" />
                                            <Translate id="dashboards.view-results" />
                                        </button>
                                    </div> : null }
                                <DiagnosticsChart data={chartDiagnosticsData}/>
                                {
                                    <StudentDiagnosticModal 
                                        modalOpen={diagnosticResultsModalOpen} 
                                        toggle={handleViewResults}
                                        selectedStudents={selectedStudents}
                                    />
                                }
                            </CardBody>
                        </Card>
                    </div> */}

                    {/* --- CONDITIONAL TABLE RENDERING LOGIC --- */}
                    {/* Hidden for now since it's using mock data */}
                    {/* <div className="col-lg-6 mb-4">
                        <Card className="shadow-sm border-0 h-100">
                            <div className="d-flex justify-content-end p-2 bg-light border-bottom">
                                // Export Button for Exam/Lesson Results Table
                                <Button
                                    color="success"
                                    outline
                                    size="sm"
                                    onClick={() => handleExportToExcel("Resultados de Exámenes/Promedios de Lecciones")}
                                >
                                    <FontAwesomeIcon icon={faFileExcel} className="mr-1" />
                                    <Translate id="dashboards.export-table" />
                                </Button>
                            </div>

                            {(userRole === "teacher" || userRole === "parent") ? (
                                // Teacher view: Individual student results
                                <StudentExamResultsList
                                    examsData={examResults}
                                    selectedStudent={selectedStudents}
                                    selectedFocusAreas={selectedFocusAreas}
                                    startDate={startDate}
                                    endDate={endDate}
                                />
                            ) : (
                                // Admin/Director view: Average results by lesson
                                <AverageLessonResults 
                                    data={averageLessonResultMockedData} 
                                    isDirector={userRole === "director"} 
                                    selectedLanguages={selectedLanguages}
                                    selectedFocusAreas={selectedFocusAreas}
                                />
                            )}
                        </Card>
                    </div> */}

                </div>

                {/* Sub-Navigation (REMAINS THE SAME) */}
                <Nav pills className="mb-1 rounded charts-tabs-container">
                    <NavItem className="mt-0 text-center">
                        <nav className={dashboardSubTab === "1" ? "nav-link active bg-secondary text-white font-weight-bold" : "nav-link text-muted"}
                             onClick={() => setDashboardSubTab("1")} style={{cursor: 'pointer'}}>
                            <FontAwesomeIcon icon={faChartLine} className="mr-1"/>
                            <Translate id="dashboards.usage-and-engagement.usage-and-engagement-title"/>
                        </nav>
                    </NavItem>
                    <NavItem className="mt-0 text-center">
                        <nav className={dashboardSubTab === "2" ? "nav-link active bg-secondary text-white font-weight-bold" : "nav-link text-muted"}
                             onClick={() => setDashboardSubTab("2")} style={{cursor: 'pointer'}}>
                            <FontAwesomeIcon icon={faChartBar} className="mr-1"/>
                            <Translate id="dashboards.progress-and-mastery.progress-and-mastery-title"/>
                        </nav>
                    </NavItem>
                    <NavItem className="mt-0 text-center">
                        <nav className={dashboardSubTab === "3" ? "nav-link active bg-secondary text-white font-weight-bold" : "nav-link text-muted"}
                             onClick={() => setDashboardSubTab("3")} style={{cursor: 'pointer'}}>
                            <FontAwesomeIcon icon={faBookReader} className="mr-1"/>
                            <Translate id="dashboards.reading-and-content.reading-and-content-title" />
                        </nav>
                    </NavItem>
                </Nav>

                <div className="pt-3 export-container">
                    <PeriodSelector
                        onChange={handlePeriodChange}
                        granularity={periodGranularity}
                        weekDate={periodWeekDate}
                        monthValue={periodMonthValue}
                        yearValue={periodYearValue}
                    />
                    <div className="inline text-center mb-3">
                        <Button color="success" outline size="sm" onClick={() => handleExportToExcel(EXPORT_SECTION_BY_SUB_TAB[dashboardSubTab])}
                                style={{marginRight: 10}}>
                            <FontAwesomeIcon icon={faFileExcel} className="mr-1" />
                            <Translate id="dashboards.export-section" />
                        </Button>
                    </div>
                </div>

                {/* Sub-Tab Content */}
                <TabContent activeTab={dashboardSubTab}>
                    <TabPane tabId="1">
                        <DynamicUsageMetricsCard
                            sessionTotals={sessionTotals}
                            selectedEntity={primarySelectedStudent}
                            userRole={userRole}
                        />
                        <UsageEngagementCharts
                            data={usageMetrics}
                            sessionTotals={sessionTotals}
                            sessionMetrics={sessionMetrics}
                            granularity={periodGranularity}
                            selectedPeriod={selectedPeriod}
                            sessionLoading={sessionLoading}
                        />
                        <UsageGapsChart
                            userRole={userRole}
                            data={moduleUsageData}
                            moduleUsage={moduleUsage}
                            schoolId={selectedSchoolId}
                            regionId={selectedRegionId}
                        />
                    </TabPane>
                    <TabPane tabId="2">
                        <ProgressMasteryCharts
                            readingChartsData={chartReadingData} 
                            skillsProgressData={chartSkillsData} 
                            unitMetrics={unitMetrics}
                            performanceMetrics={performanceMetrics}
                            granularity={periodGranularity}
                            selectedPeriod={selectedPeriod}
                            storyLanguage={storyLanguage}
                            onStoryLanguageChange={setStoryLanguage}
                            loading={sessionLoading}
                            readingLessonsSeries={readingLessonsSeries}
                            userRole={userRole}
                            period={selectedPeriod?.period}
                            dateFrom={selectedPeriod?.dateFrom}
                            dateTo={selectedPeriod?.dateTo}
                            userIds={selectedStudents}
                            schoolId={selectedSchoolId}
                            regionId={selectedRegionId}
                        />
                    </TabPane>
                    <TabPane tabId="3">
                        <ReadingContentCharts
                            contentMetrics={contentMetrics}
                            granularity={periodGranularity}
                            selectedPeriod={selectedPeriod}
                            loading={sessionLoading}
                            storyLanguage={storyLanguage}
                            onStoryLanguageChange={setStoryLanguage}
                        />
                    </TabPane>
                </TabContent>
            </div>
            {/* --- END CONTENT CONTAINER WITH FADE EFFECT --- */}
        </>
    );

    return (
        <div>

            <AssignDatesModal
                modal={modalOpen}
                toggle={toggleModal}
                onSubmit={handleAssignDate}
            />

            <div className="container-padding animated learning-gap-fade-in">
                <div className="wrapper-principal-container dashboards-wrapper mr-3">
                    <div className="wrapper-col mb-3">
                        <div className="col col-no-padding-left">
                            <div className="title-principal mt-3">
                                <span>
                                    <Translate id="dashboards.dashboards-title" options={{ renderInnerHtml: false }} />
                                </span>
                            </div>
                        </div>
                    </div>

                    {/* --- PRIMARY TABS (Dashboard vs Content) --- */}
                    <div className="dashboard-tabs-container justify-content-between align-items-end mb-4">
                        <Nav tabs style={{ border: 'none' }} className="rounded-lg dashboard-tabs">
                            <NavItem className="text-center" style={{marginTop: "0px"}}>
                                <nav
                                    className={topLevelTab === "1"
                                        ? "nav-link active bg-secondary text-white font-weight-bolder"
                                        : "nav-link font-weight-bold bg-light"}
                                    onClick={() => setTopLevelTab("1")}
                                    style={{ cursor: "pointer", borderRadius: "4px 0 0 0", borderRight: "1px solid #dee2e6" }}
                                >
                                    Dashboard
                                    <FontAwesomeIcon icon={faTachometerAlt} className="ml-2" />
                                </nav>
                            </NavItem>

                            {Settings.IS_MED_QUINDIO ? null :
                            <NavItem className="text-center" style={{marginTop: "0px"}}>
                                <nav
                                    className={topLevelTab === "2"
                                        ? "nav-link active bg-secondary text-white font-weight-bolder"
                                        : "nav-link font-weight-bold bg-light"}
                                    onClick={() => setTopLevelTab("2")}
                                    style={{ cursor: "pointer", borderRadius: "0 4px 0 0" }}
                                >
                                    {userRole === "parent" ? <Translate id="learning-gap.recommendations" /> : <Translate id="dashboards.content-and-lessons" />}
                                    <FontAwesomeIcon icon={faBookOpen} className="ml-2" />
                                </nav>
                            </NavItem> }
                            {/* {userRole === "teacher" ?
                            <NavItem>
                                <nav
                                    className={topLevelTab === "3"
                                        ? "nav-link active bg-secondary text-white font-weight-bolder"
                                        : "nav-link font-weight-bold bg-light"}
                                    onClick={() => setTopLevelTab("3")}
                                    style={{ cursor: "pointer", borderRadius: "0 4px 0 0" }}
                                >
                                    <Translate id="dashboards.diagnostics-creator" />
                                    <FontAwesomeIcon icon={faList} className="ml-2" />
                                </nav>
                            </NavItem> : null} */}
                            {userRole === "admin" ?
                                <NavItem className="text-center" style={{marginTop: "0px"}}>
                                    <nav
                                        className={topLevelTab === "4"
                                            ? "nav-link active bg-secondary text-white font-weight-bolder"
                                            : "nav-link font-weight-bold bg-light"}
                                        onClick={() => setTopLevelTab("4")}
                                        style={{ cursor: "pointer", borderRadius: "0 4px 0 0" }}
                                    >
                                        <Translate id="dashboards.stories-creator" />
                                        <FontAwesomeIcon icon={faList} className="ml-2" />
                                    </nav>
                                </NavItem> 
                            : null
                            }
                            
                        </Nav>

                        {/* Button aligned to the right */}
                        <a
                            href={manualUrl}
                            target="_blank"
                            rel="noopener noreferrer"
                            title=""
                        >
                            <Button color="success" className="font-weight-bold" style={{marginRight: 10}}>
                                <Translate id="dashboards.open-manual" options={{ renderInnerHtml: false }} />
                                <FontAwesomeIcon icon={faDownload} className="ml-2" />
                            </Button>
                        </a>
                    </div>

                    <TabContent activeTab={topLevelTab}>
                        {/* Tab 1: DASHBOARD (Includes Sub-Navigation) */}
                        <TabPane tabId="1">
                            {DashboardContent}
                        </TabPane>

                        {/* Tab 2: CONTENT AND LESSONS */}
                        <TabPane tabId="2">
                            {ContentAndLessonsCard}
                        </TabPane>

                        {/* <TabPane tabId="3">
                            {topLevelTab === "3" && <DiagnosticView />}
                        </TabPane> */}

                        <TabPane tabId="4">
                            {topLevelTab === "4" && <StoriesView/>}
                        </TabPane>
                    </TabContent>
                </div>
            </div>
        </div>
    )
}

export default withLocalize(DashboardsView);