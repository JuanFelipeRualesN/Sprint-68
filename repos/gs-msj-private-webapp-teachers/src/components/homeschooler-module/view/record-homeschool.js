import React, { useState, useEffect } from "react";
import {
    Col,
    CardBody,
    Row, Card
} from "reactstrap";
import "./style.css";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server"; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faArrowLeft, faCheckCircle, faEye, faSearch
} from "@fortawesome/free-solid-svg-icons";
import ProfilesDataTable from "../../shared-module-components/datatables/profiles-data-table";
import "./style.css";
import AveragePerSubjectChart from "../charts/average-per-subject-chart";
import {getStudentOverallAverage, getStudentOverallLessonAverage} from "../../../api/gsweb";
import DataTable from "../../shared-module-components/datatables/datatable";
import {formatUtcToLocal} from "../../../utils/utils";
import ResultFilters from "../selector/results-filter";
import { getAthenasGrades, getAthenasSubjects} from "../../../api/teachers";
import { useLocation   } from "react-router-dom";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import axios from "../../../api/interceptors";
import Tooltip from '@material-ui/core/Tooltip';
import Lock from '@material-ui/icons/Lock';
import SessionManager from "../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../utils/access_logger";
/**
 * Class/component of the student average
 */
const RecordHomeschool = ({ initialize, addTranslation, history }) => {
    const [studentId, setStudentId] = useState(null);
    const [studentName, setStudentName] = useState(null);
    const [averageData, setAverageData] = useState([]);
    const [loadingAverage, setLoadingAverage] = useState(false);
    const [loadingLessons, setLoadingLessons] = useState(false);
    const [loading, setLoading] = useState(false);
    const [firstLoad, setFirstLoad] = useState(true);
    const [lessonsData, setLessonsData] = useState([]);
    const [grades, setGrades] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [filters, setFilters] = useState({
        grades: [],
    });
    const lang = LanguageManager.getLanguage();
    const location = useLocation();
    const userDetails = SessionManager.getSessionDetails();


    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("homeschool.lesson", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("homeschool.level", Settings.LANGUAGES) },
        { key: "2", name: LanguageManager.getTranslation("homeschool.average", Settings.LANGUAGES) },
        { key: "3", name: LanguageManager.getTranslation("homeschool.completed", Settings.LANGUAGES) },
        { key: "4", name: LanguageManager.getTranslation("homeschool.last-date-worked", Settings.LANGUAGES) },
        { key: "5", name: LanguageManager.getTranslation("homeschool.results", Settings.LANGUAGES) },
    ];

    const [searchFilters, setSearchFilters] = useState({
        subjects: [],
        grades: [],
    });

    const headerOptionsLessons = [...headerOptions];

    const isStaff = false;
    const roleId = Settings.ROLE_STUDENT_ID;

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        if (!studentId) return;

        fetchAverage();
        fetchLessonAverage();
    }, [studentId]);

    useEffect(() => {
        const params = new URLSearchParams(location.search);
        const studentIdParam = params.get("studentId");
        if (studentIdParam) {
            setStudentId(studentIdParam);
        }
        const studentNameParam = params.get("studentName");
        if (studentNameParam) {
            setStudentName(studentNameParam);
        }
    }, [location.search]);

    useEffect(() => {
        try {
        SendAccessLog({
            UserName: userDetails.Username,
            InvitationCode: userDetails.InvitationCode,
            Role: userDetails.Roles[0],
            AreaId: "5",
        })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    const fetchAverage = async () => {
        setLoadingAverage(true);
        // Convert date filters to MySQL format
        const dateFrom = filters != null && filters.dateFrom ? convertToMySqlDate(filters.dateFrom) : null;
        const dateTo = filters != null && filters.dateTo ? convertToMySqlDate(filters.dateTo) : null;

        try {
            const response = await getStudentOverallAverage(
                studentId,
                filters != null && filters.grades && filters.grades.length > 0 ? filters.grades[0] : null,
                dateFrom,
                dateTo);
            const data = response?.data;

            if (Array.isArray(data) && data.length > 0) {
                setAverageData(data);
            } else {
                console.warn("Empty or invalid response data:", data);
                setAverageData([]);
            }
        } catch (error) {
            console.error("Error fetching averages:", error);
            setAverageData([]);
        } finally {
            setLoadingAverage(false);
        }
    };

    const fetchLessonAverage = async () => {
        setLoadingLessons(true);

        // Convert date filters to MySQL format
        const dateFrom = filters != null && filters.dateFrom ? convertToMySqlDate(filters.dateFrom, true) : null;
        const dateTo = filters != null && filters.dateTo ? convertToMySqlDate(filters.dateTo, false) : null;

        try {
            const response = await getStudentOverallLessonAverage(studentId,
                filters != null && filters.subjects && filters.subjects.length > 0 ? filters.subjects[0] : null,
                filters != null && filters.grades && filters.grades.length > 0 ? filters.grades[0] : null,
                dateFrom,
                dateTo
                );
            convertDataLessonsToDataTableData(response?.data);
        } catch (error) {
            console.error("Error fetching averages:", error);
        } finally {
            setLoadingLessons(false);
        }
    };

    // Fetch the lessons and filters data ( subjects, grades, standards, expectations )
    useEffect(() => {
        // Fetch all the data when the component mounts
        const fetchAllData = async () => {
            setLoading(true);
            try {

                setSearchFilters({...filters});
                await Promise.all([
                    callAvailableSubjectsAndGradresToSetFilters(),
                ]);
                setFirstLoad(false);
            } finally {
                setLoading(false);
            }
        };
        fetchAllData();
    }, []);

    const callAvailableSubjectsAndGradresToSetFilters = async () => {
        try {
            const endpoint = Settings.getUserSubscriptionProductsAPI();
            const response = await axios.get(endpoint);
            const data = response.status === 200 ? response.data : [];

            // 1. Extract unique subjects by 'Subject' (code)
            const seenSubjects = new Set();
            const formattedSubjects = data
                .filter(subject => {
                    const code = subject.Subject?.trim();
                    if (!code || seenSubjects.has(code)) return false;
                    seenSubjects.add(code);
                    return true;
                })
                .map(subject => ({
                    label: subject.SubjectName,
                    code: subject.Subject,
                }))
                .sort((a, b) => a.label.localeCompare(b.label));

            setSubjects(formattedSubjects);

            // Reset subject filters
            setFilters(prev => ({
                ...prev,
                subjects: []
            }));

            // 2. Extract available levels from product data
            const availableLevels = Array.from(
                new Set(
                    data
                        .map(subject => subject.Level?.trim())
                        .filter(level => level) // Remove null/undefined/empty
                )
            );

            try {
                const responseLevels = await getAthenasGrades();

                // 3. Filter and deduplicate levels
                const seenLevels = new Set();
                const formattedGrades = responseLevels.data
                    ?.filter(level =>
                        level.Name !== "" &&
                        availableLevels.includes(level.Code) &&
                        !seenLevels.has(level.Code) &&
                        seenLevels.add(level.Code)
                    )
                    .map(level => ({
                        label: level.Name,
                        code: level.Code,
                    }));

                setGrades(formattedGrades);

                // Reset grade filters
                setFilters(prev => ({
                    ...prev,
                    grades: []
                }));
            } catch (error) {
                console.error("Error fetching levels:", error);
            }

        } catch (error) {
            console.error("Error fetching subjects:", error);
        }
    };

    const showStudentBox = () => {
        setStudentId(null);
        setAverageData([]);

        // Remove studentId from the URL
        history.push({
            pathname: location.pathname,
            search: ''
        });
    };

    const handleSetStudentDetails = (newValue) => {
        setStudentId(newValue.UserId);
        setStudentName(`${newValue.GivenName} ${newValue.Surname}`);
        window.scrollTo(0, 0);
        // Update the URL with the new studentId
        history.push(`?studentId=${newValue.UserId}&studentName=${encodeURIComponent(`${newValue.GivenName} ${newValue.Surname}`)}`);
    };

    const filterResults = () => {
        try {
            SendAccessLog({
                UserName: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "5",
                Subject: Array.isArray(filters.subjects) ? filters.subjects[0] : "",
                Grade: Array.isArray(filters.grades) ? filters.grades[0] : "",
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
        fetchLessonAverage();
        fetchAverage();
    }
    const convertToMySqlDate = (date, isDateFrom) => {
        if (!date || !(date instanceof Date) || isNaN(date)) {
            return null; // Return null or handle invalid date as per your requirement
        }

        const year = date.getFullYear();
        // getMonth() returns 0-11, so add 1
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');

        return isDateFrom ? `${year}-${month}-${day}` : `${year}-${month}-${day} ${23}:${59}:${59}`;
    }

    const viewResults = (lessonTitle, lessonId) => {
        const query = new URLSearchParams({
            studentId: studentId,
            studentName: studentName,
            lessonId: lessonId,
            lessonTitle: lessonTitle
        }).toString();
        history.push(`/record-homeschool-lesson?${query}`);
    }

    const convertDataLessonsToDataTableData = (dataLessons) => {
        const newArray = dataLessons.map((item) => ({
            LessonTitle: item.LessonTitle,
            LevelCode: item.LevelCode,
            Average: item.Average,
            Completed: item.Completed === "1" ? <FontAwesomeIcon icon={faCheckCircle} /> : null,
            LastDateWorkedOn: formatUtcToLocal(item.UpdatedAt, lang),
            Results: item.CanSeeResults ? (
                <button
                    className="btn-default button-secondary-custom"
                    onClick={() => viewResults(item.LessonTitle, item.LessonId)}
                    value={item.LessonId}
                >
                    <FontAwesomeIcon icon={faEye} />{' '}
                    <Translate id="homeschool.results" />
                </button>
            ) : (
                <Tooltip title={<Translate id="homeschool.results-cannot-be-accessed" options={{renderInnerHtml: false}}/>} placement="top" arrow>
                    <div
                        className="lesson-card-actions"
                        style={{
                            cursor: 'not-allowed',
                            color: '#A9A9A9',
                            opacity: 0.7,
                            display: 'inline-flex',
                            alignItems: 'center'
                        }}
                    >
                        <button
                            className="btn-default button-secondary-custom"
                            onClick={() => viewResults(item.LessonTitle, item.LessonId)}
                            value={item.LessonId}
                            disabled={true}
                        >
                            <Lock fontSize="small" style={{marginRight: '5px', width: '15px', color: '#616161'}}/>{' '}
                            <Translate id="homeschool.results"/>
                        </button>

                    </div>
                </Tooltip>
            ),
            value: item.LessonId
        }));

        setLessonsData(newArray);
    };

    return (
        <div>
            <div className="container-padding animated fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col mb-3">
                        <div className="col col-no-padding-left">
                            {studentId ? (
                                <>
                                    <a className="btn-back" onClick={showStudentBox}>
                                        <FontAwesomeIcon icon={faArrowLeft} />{" "}
                                        <Translate id="general.back-action" />
                                    </a>
                                    <div className="title-principal mt-3">
                                        <span className="title-principal--inside">
                                          <Translate id="progress.record-student" />:
                                        </span> {studentName}
                                    </div>
                                </>
                            ) : (
                                <div className="title-principal mt-3">
                                  <span className="title-principal--inside">
                                    <Translate id="progress.record-student" />
                                  </span>
                                </div>
                            )}
                        </div>
                    </div>

                    <Row style={{ minHeight: 600 }}>
                        <Col sm="12" md="12" lg="12">
                                <Row className="td-w-rollover td-responsive special-top-space syle-date-five">
                                    {!studentId && (
                                        <Col sm="12">
                                            <p>
                                                <b>
                                                    <Translate id="progress.select-student"
                                                               options={{renderInnerHtml: true}}/>
                                                </b>
                                            </p>
                                            <ProfilesDataTable
                                                value={studentId}
                                                isStaff={isStaff}
                                                roleId={roleId}
                                                updateParent={handleSetStudentDetails}
                                            />
                                        </Col>
                                    )}
                                </Row>
                                {studentId != null ?
                                <Row>
                                    <Col sm={4} md={12} lg={3}>
                                        {grades.length > 0 ?
                                        <ResultFilters
                                            subjects={subjects}
                                            grades={grades}
                                            filters={filters}
                                            setFilters={setFilters}
                                            filterResults={filterResults}/> : null
                                        }
                                    </Col>
                                    <Col xs={12} sm={6} md={12} lg={9} style={{height: 400}}>
                                        <div className="lesson-results-title">
                                            <span><Translate id="homeschool.average-per-subject"
                                                             options={{renderInnerHtml: true}}/></span>
                                        </div>
                                        <Card>
                                            <CardBody>
                                                {loadingAverage ? <div className="spinner-container">
                                                        <span className="spinner-button-alt"/>
                                                    </div> :
                                                    <AveragePerSubjectChart data={averageData}/>
                                                }
                                            </CardBody>
                                        </Card>
                                        <div className="lesson-results-title">
                                            <span><Translate id="homeschool.lessons-worked-by-student"
                                                             options={{renderInnerHtml: true}}/></span>
                                        </div>
                                        <Card style={{overflowY: 'auto', maxHeight: 400}}>
                                            {loadingLessons ? <div className="spinner-container">
                                                    <span className="spinner-button-alt"/>
                                                </div> :
                                                lessonsData.length === 0 ?
                                                    <div
                                                        style={{
                                                            display: 'flex',            // Enable Flexbox
                                                            flexDirection: 'column',    // Stack items vertically
                                                            justifyContent: 'center',   // Center horizontally (along the main axis, which is column here)
                                                            alignItems: 'center',       // Center vertically (along the cross axis, which is row here)
                                                            width: '100%',              // Ensure it takes full width for horizontal centering to be obvious
                                                            textAlign: 'center',        // Good for general text centering, though flexbox also helps
                                                            gap: '10px',               // Add some space between the icon and the text
                                                            paddingTop: 10
                                                    }}
                                                    >
                                                        <FontAwesomeIcon icon={faSearch}
                                                                         size="3x"/> {/* Use a larger size like 3x or 5x for emphasis */}
                                                        {/* Removed <br/> as flexbox handles vertical stacking */}
                                                        <Translate id="homeschool.there-are-no-results"
                                                                   options={{renderInnerHtml: true}}/>
                                                    </div> :
                                                    <DataTable headerOptions={headerOptions}
                                                               data={lessonsData}/>

                                            }
                                        </Card>
                                    </Col>

                                </Row>
                                    : null}
                        </Col>
                    </Row>
                </div>
            </div>
        </div>
    );
};

export default withLocalize(RecordHomeschool);
