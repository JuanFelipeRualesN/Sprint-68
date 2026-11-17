/**
 * Created by portiz on 9/20/18.
 */
import React, { useState, useEffect } from 'react';
import {Col, Card, CardBody, CardHeader, Container, Row, Alert, Button} from 'reactstrap';
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import LanguageManager from '../../../custom-classes/language-manager';
import SelectorStudentProduct from "../selector/selector-student-products";
import SelectorLesson from '../selector/selector-lesson';
import SelectorExam from '../selector/selector-exam';
import TableTotalTries from '../table/table-total-tries';
import RandomRGBA from '../../../custom-classes/rgba-random';
import Settings from "../../../custom-classes/settings";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faArrowAltCircleLeft, faPrint, faArrowLeft} from '@fortawesome/free-solid-svg-icons';
import ProfilesDataTable from "../datatables/profiles-data-table";
import {Link} from "react-router-dom";
import LoaderFullWindow from "../loader-windows/loader-full-window";
import "./style.css";
import {getQuizzesTries, getStudentLessonAverage} from "../../../api/gsweb";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';


/**
 * Class/component of the student average
 */
const ReportResults = ({location}) => {
    const [examId, setExamId] = useState("0");
    const [isStaff, setIsStaff] = useState(false);
    const [languages, setLanguages] = useState(Settings.LANGUAGES);
    const [loading, setLoading] = useState(false);
    const [lessonId, setLessonId] = useState(location.state?.lessonId || null);
    const [notPassedTotal, setNotPassedTotal] = useState("0");
    const [passedTotal, setPassedTotal] = useState("0");
    const [productId, setProductId] = useState(location.state?.productId || null);
    const [roleId, setRoleId] = useState(Settings.ROLE_STUDENT_ID);
    const [studentFullName, setStudentFullName] = useState(null);
    const [studentId, setStudentId] = useState(location.state?.studentId || null);
    const [triesChartData, setTriesChartData] = useState(null);
    const [triesData, setTriesData] = useState(null);
    const [backUrl, setBackUrl] = useState(typeof location !== 'undefined' && location !== null && typeof location.query !== 'undefined' &&
    location.query !== null ? location.query : '/dashboard');

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Handles the state when the component will be mounted
    useEffect(() => {
        if (location.state !== undefined) {
            if (location.state.productId !== undefined)
                setProductId(location.state.productId);

            if (location.state.studentId !== undefined)
                setStudentId(location.state.studentId);

            if (location.state.lessonId !== undefined)
                setLessonId(location.state.lessonId);

            if (lessonId !== "0" && studentId !== "0" && productId)
                callResults(); // If none of them are null/0 call the results
        }
    }, []);

    // Call the lessons of the product
    const callLessons = (newValue) => {
        setProductId(newValue);
    };

    // Call the student classes
    const callStudentProducts = (newValue) => {
        setStudentId(newValue);
        setProductId(null);
        setLessonId(null);
        setExamId("0");
        setStudentFullName(newValue.GivenName + ' ' + newValue.Surname);
    };

    // Call the user details to set everything
    const callStudentAnalysis = (newValue) => {
        setLessonId(null);
        // The exam is reset everytime we call this
        setLessonId(newValue);
        setExamId("0");
        //callResults();
    };

    // Call the results to display
    const callResults = () => {
        // Call the api of student average
        let apiCall = getStudentLessonAverage(studentId.value, lessonId.value);
        apiCall.then((response)=> {
            setPassedTotal(parseFloat(response.data));
            setNotPassedTotal(parseFloat(100 - parseFloat(response.data)).toFixed(2));
        });
    };

    // Call the user tries
    const callTries = (newValue) => {
        setLoading(true);
        setTriesData(null);
        setExamId(newValue);
        // We should fill the chart and the table
        let apiCall = getQuizzesTries(newValue, studentId.UserId);
        apiCall.then((response)=> {
            // Set the data to the table
            setTriesData(response.data);

            // Now, work with the chet
            // Work with the chart
            let labelTry = LanguageManager.getTranslation("report-student-average.try", languages);
            let labelTries = LanguageManager.getTranslation("report-student-average.tries", languages);

            // Prepare the data to the lesson
            let labels = [];
            let backgroundColors = [];
            let datasets = [{
                label: '# ' + labelTries,
                borderWidth: 1
            }];
            let dataArray = [];
            for (let i = 0; i < response.data.length; i++) {
                labels.push(labelTry + ' ' + (i + 1).toString());
                dataArray.push(response.data[i].TotalPercent);
                backgroundColors.push(RandomRGBA.randomRgba())
            }
            datasets[0].data = dataArray;
            datasets[0].fillColor = backgroundColors;
            datasets[0].borderColor = backgroundColors;

            let dataChartBar = {
                labels: labels,
                datasets: datasets
            };

            return dataChartBar;
        }).then((dataChartBar)=> {
            setTriesChartData(dataChartBar);
            setLoading(false);
        });
    };

    const showStudentBox = () => {
        setStudentId(null);
        setProductId(null);
    };

    // Render the form details
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col borderGray-bottom mb-3">
                        <div className="col col-no-padding-left">
                            <Link to={backUrl} className="btn-back">
                                <FontAwesomeIcon icon={faArrowLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link>
                            <div className="title-principal mt-3">
                                <Translate id="dashboard.results" options={{renderInnerHtml: true}}/>
                            </div>
                        </div>
                    </div>
                    <small>
                        <Translate id="report-student-average.student-lesson-average-subtitle"
                            options={{renderInnerHtml: false}}/>
                    </small>

                    {/* <Row>
                        <Col xs={{size:12}}>
                            <h2>
                                <FontAwesomeIcon icon={faPrint}/>{' '}
                                <Translate id="dashboard.results" options={{renderInnerHtml: true}}/>
                            </h2>
                        </Col>
                        <Col xs={{size:12}}>
                            <Link to={backUrl} className="btn btn-warning">
                                <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link>
                        </Col>
                    </Row>
                    <hr/> */}
                    <Row className="clear-style-card td-w-rollover td-responsive special-top-space syle-date-five flex-none pl-0 mt-3">
                                
                        <Row>
                            {
                                studentId === null || studentId === '' ?
                                    <Col>
                                        <p><b><Translate id="report-student-average.select-student"
                                                options={{renderInnerHtml: true}}/></b></p>
                                        <ProfilesDataTable
                                            value={studentId}
                                            isStaff={isStaff} roleId={roleId}
                                            updateParent={callStudentProducts}/>
                                    </Col> : null
                            }
                        </Row>
                        <Row>
                            {
                            typeof studentId !== 'undefined' && studentId !== null && studentId !== '' ?
                                <Col>
                                    <Translate id="report-student-average.selected-student"
                                            options={{renderInnerHtml: false}}/>
                                        : <span className="text-color-custom"><b> {studentFullName} </b></span> {' '}

                                        <button className="btn-default button-primary-custom" onClick={showStudentBox}>
                                            <Translate id="report-student-average.select-a-student"
                                                                   options={{renderInnerHtml: false}}/>
                                        </button>
                                </Col> : null
                            }
                        </Row>
                                
                                                       
                    </Row>

                    
                        <div className="title-principal mt-5">
                            <Translate id="report-student-average.select-class-and-lesson"
                                options={{renderInnerHtml: false}}/>
                        </div>

                        {/* SELECTORES */}
                        <div className='wrapper-col homeschooler-records pt-3 pb-3 pr-3'>
                            <div className='two-col form-group-fields__select'>
                                <p className='label-homeschooler clear-p'><Translate id="report-student-average.select-class"
                                            options={{renderInnerHtml: true}}/></p>
                                        { studentId !== null ?                                            
                                <div>
                                    <SelectorStudentProduct value={productId}
                                        data={{studentId}}
                                        updateParent={callLessons}/>
                                </div>: null }
                            </div>

                            <div className='two-col form-group-fields__select'>
                                <p className='label-homeschooler clear-p'><Translate id="syllabus.select-lesson"
                                        options={{renderInnerHtml: true}}/></p>
                                        { productId !== null ?
                                <div>
                                    <SelectorLesson value={lessonId} data={{productId}}
                                        updateParent={callStudentAnalysis}/>
                                </div> : null }
                            </div>

                            <div className='two-col form-group-fields__select'>
                                <p className='label-homeschooler clear-p'><Translate id="report-student-average.select-exam"
                                    options={{renderInnerHtml: true}}/></p>
                                    { lessonId !== null
                                    && productId !== null
                                    && studentId !== null
                                        ?
                                <div>
                                    <SelectorExam value={examId} refresh={{}}
                                        data={{lessonId}}
                                        updateParent={callTries}/>
                                </div>: null }
                            </div>
                        </div>

                        <div className="mt-3">
                            {loading ?
                                    <LoaderFullWindow/>: null
                            }
                            <div className="module-progress-exam__tries">
                                <div className="module-progress-exam__table">
                                    { examId !== "0"
                                        && productId !== null
                                        && lessonId !== null
                                        && studentId !== null
                                        && triesData !== null && triesData.length > 0 ?
                                            <TableTotalTries data={triesData}
                                                profileId={studentId.value}
                                                refresh={triesData}/>
                                        : null
                                    }
                                </div>

                                <div>
                                    { examId !== "0"
                                    && productId !== null
                                    && lessonId !== null
                                    && studentId !== null
                                    && triesData !== null && triesData.length === 0 ?
                                        <Alert>
                                            <Translate id="report-student-average.pending"
                                            options={{renderInnerHtml: true}}/>
                                        </Alert> :
                                    null}
                                </div>
                            </div>
                        </div>                               
                                              


                    {/* <Row>
                        <Col>
                                <CardHeader>
                                    <h4>
                                        <Translate id="report-student-average.select-class-and-lesson"
                                                   options={{renderInnerHtml: false}}/>
                                    </h4>
                                </CardHeader>
                                    <Row>
                                        <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                            <p><b><Translate id="report-student-average.select-class"
                                                             options={{renderInnerHtml: true}}/></b></p>
                                            { studentId !== null ?
                                                <div className="gray-dark-text">
                                                <SelectorStudentProduct value={productId}
                                                                        data={state}
                                                                        updateParent={callLessons()}/>
                                                </div>: null }
                                        </Col>
                                        <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                            <p><b><Translate id="syllabus.select-lesson"
                                                             options={{renderInnerHtml: true}}/></b></p>
                                            { productId !== null ?
                                                <div className="gray-dark-text">
                                                <SelectorLesson value={lessonId} data={state}
                                                                updateParent={callStudentAnalysis()}/>
                                                </div> : null }
                                        </Col>
                                        <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                            <p><b><Translate id="report-student-average.select-exam"
                                                             options={{renderInnerHtml: true}}/></b></p>
                                            { lessonId !== null
                                            && productId !== null
                                            && studentId !== null
                                                ?
                                                <div className="gray-dark-text">
                                                <SelectorExam value={examId} refresh={state}
                                                              data={state}
                                                              updateParent={callTries()}/>
                                                </div>: null }
                                        </Col>


                                        {loading ?
                                            <LoaderFullWindow/>: null
                                        }
                                        
                                        <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            { examId !== "0"
                                            && productId !== null
                                            && lessonId !== null
                                            && studentId !== null
                                            && triesData !== null && triesData.length > 0 ?
                                                    <TableTotalTries data={triesData}
                                                                     profileId={studentId.value}
                                                                 refresh={triesData}/>
                                                                 : null
                                            }

                                        </Col>
                                        <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            { examId !== "0"
                                            && productId !== null
                                            && lessonId !== null
                                            && studentId !== null
                                            && triesData !== null && triesData.length === 0 ?
                                                <Alert>
                                                    <Translate id="report-student-average.pending"
                                                               options={{renderInnerHtml: true}}/>
                                                </Alert> :
                                                null}

                                        </Col>
                                    </Row>
                        </Col>
                    </Row> */}
                </div>
            </div>

        </div>
    )
};

export default withLocalize(ReportResults);