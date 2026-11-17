/**
 * Created by portiz on 9/20/18.
 */
import React, { useState, useEffect } from 'react';
import {Col, Card, CardBody, CardHeader, Container, Row, Alert} from 'reactstrap';
import {Link, Redirect} from "react-router-dom";
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/

import {getQuizzesTries, getStudentLessonAverage} from "../../../api/gsweb";
import LanguageManager from '../../../custom-classes/language-manager';
import SelectorStudentProduct from "../selector/selector-student-products";
import SelectorLesson from '../selector/selector-lesson';
import SelectorExam from '../selector/selector-exam';
import ChartAverage from '../charts/chart-average';
import ChartLessonTries from '../charts/chart-lesson-tries';
import TableTotalTries from '../table/table-total-tries';
import RandomRGBA from '../../../custom-classes/rgba-random';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faArrowAltCircleLeft, faChartBar, faArrowLeft} from '@fortawesome/free-solid-svg-icons';
import LoaderFullWindow from "../loader-windows/loader-full-window";
import "./style.css";
import Settings from "../../../custom-classes/settings";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * Class/component of the student average
 */
const ReportStudentLessonAverage = ({location, initialize, addTranslation}) => {
    const [examId, setExamId] = useState("0");
    const [languages, setLanguages] = useState(Settings.LANGUAGES); // Repeat language here, because we need it to pass it the child to create inline translation
    const [lessonId, setLessonId] = useState(location.state?.lessonId || null);
    const [loading, setLoading] = useState(false);
    const [notPassedTotal, setNotPassedTotal] = useState("0");
    const [passedTotal, setPassedTotal] = useState("0");
    const [productId, setProductId] = useState(location.state?.productId || null);
    const [studentId, setStudentId] = useState(location.state?.studentId || null);
    const [levelCode, setLevelCode] = useState(location.state?.levelCode || null);
    const [subjectCode, setSubjectCode] = useState(location.state?.subjectCode || null);
    const [studentName, setStudentName] = useState(null);
    const [triesChartData, setTriesChartData] = useState(null);
    const [triesData, setTriesData] = useState(null);

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

            if (lessonId !== null && studentId !== null && productId)
                callResults(); // If none of them are null/0 call the results
        }
    }, []);

    // Call the lessons of the product
    const callLessons = (newValue) => {
        setExamId(null);
        setProductId(null);
        setPassedTotal("0");
        setNotPassedTotal("0");
        setTriesData(null);
        setProductId(newValue);
        setLessonId(null);
        setTriesChartData(null);
    };

    // Call the student classes
    const callStudentProducts = () => (newValue) => {
        setStudentId(newValue);
        setProductId(null);
        setExamId(null);
        setTriesData(null);
    };

    // Call the user details to set everything
    const callStudentAnalysis = (newValue) => {
        setExamId(null);
        setLessonId(null);
        setTriesChartData(null);
        setPassedTotal("0");
        setNotPassedTotal("0");
        setTriesData(null);
        // The exam is reset everytime we call this
        setLessonId(newValue);
        setExamId("0");
        callResults();
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
        setExamId(newValue);
        setLoading(true);

        // We should fill the chart and the table
        let apiCall = getQuizzesTries(newValue, studentId.value);
        apiCall.then((response)=> {

            // Set the data to the table
            setTriesData(response.data);

            // Now, work with the chart
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

    if (typeof studentId === 'undefined'
        || studentId === null)
        return (<Redirect  to={{
            pathname: "/report-average"
        }}/>);

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col borderGray-bottom mb-3">
                        <div className="col col-no-padding-left">
                            <Link to="/report-average" className="btn-back">
                                <FontAwesomeIcon icon={faArrowLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link>
                            <div className="title-principal mt-3">
                                <Translate id="navbar.student-lesson-average" options={{renderInnerHtml: true}}/>
                            </div>
                        </div>
                    </div>

                    <div className="wrapper-col wrapper-col__align-top wrapper-col__wrap-responsive">
                        <div className="col-auto-size">
                            <div className="progress-lesson">
                                <Translate id="report-student-average.student" options={{renderInnerHtml: true}}/>: <b>{
                                                    studentId.GivenName
                                                    + ' ' + studentId.Surname
                                                    + ' (' + studentId.Username + ') '}</b>
                            </div>

                            {/* SELECTORES */}
                            <div className='wrapper-col homeschooler-records pt-3 pb-3'>
                                <div className='two-col form-group-fields__select'>
                                    <p className='label-homeschooler clear-p'><Translate id="report-student-average.select-class"
                                        options={{renderInnerHtml: true}}/></p>
                                        { studentId !== null ?
                                    <div>
                                        <SelectorStudentProduct value={productId} data={{studentId}}
                                            updateParent={callLessons}/>
                                    </div>: null }
                                </div>

                                <div className='two-col form-group-fields__select'>
                                    <p className='label-homeschooler clear-p'><Translate id="syllabus.select-lesson"
                                            options={{renderInnerHtml: true}}/></p>
                                            { productId !== null ?
                                    <div>
                                        <SelectorLesson value={lessonId} data={{productId: {value: productId}}} subjectCode={subjectCode} levelCode={levelCode}
                                            updateParent={callStudentAnalysis}/>
                                    </div>: null }        
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
                                            data={{lessonId: {value: lessonId}}}
                                            updateParent={callTries}/>
                                    </div>: null }

                                </div>

                            </div>
                        </div>

                        <div className="col-fixed-size-second text-center progress-border-left ml-3 col-auto-score">
                            <p className="progress-charts"><Translate id="report-student-average.lesson-score" options={{renderInnerHtml: false}}/></p>
                                { studentId !== null
                                    && productId !== null
                                    && lessonId !== null
                                    && passedTotal !== "0"
                                    && notPassedTotal !== "0"
                                    ?
                            <h1 style={{textAlign: "center"}}>{passedTotal}</h1>
                                : null }
                        </div>
                    </div>

                        {/* <Row>
                            <Col xs={{size:12}}>
                                <h2>
                                    <FontAwesomeIcon icon={faChartBar}/>{' '}
                                    <Translate id="navbar.student-lesson-average" options={{renderInnerHtml: true}}/>
                                </h2>
                            </Col>
                            <Col xs={{size:12}}>
                                <Link to="/report-average" className="btn btn-warning">
                                    <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                    <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                                </Link>
                            </Col>
                        </Row>
                        <hr/> */}
                        {/* <Row>
                            <Col sm={{size: 12, offset: 0}} lg={{size: 8, offset: 0}} md={{size: 8, offset: 0}}>
                                <Card className="card-white"> */}
                                    {/* <CardHeader>
                                        <h4>
                                            <Translate id="report-student-average.student-lesson-average"
                                                       options={{renderInnerHtml: false}}/>
                                        </h4>
                                        <small>
                                            <Translate id="report-student-average.student-lesson-average-subtitle"
                                                       options={{renderInnerHtml: false}}/>
                                        </small>
                                    </CardHeader> */}
                                    {/* <CardBody>
                                        <Row> */}
                                            {/* <Col xs={{size:12}}>
                                                <p>
                                                    <b><Translate id="report-student-average.student"
                                                                  options={{renderInnerHtml: true}}/>
                                                    </b>
                                                </p>
                                                <p>
                                                    {
                                                        this.state.studentId.GivenName
                                                        + ' ' + this.state.studentId.Surname
                                                        + ' (' + this.state.studentId.Username + ') '}
                                                </p>

                                            </Col> */}
                                            {/* <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                                <p><b><Translate id="report-student-average.select-class"
                                                                 options={{renderInnerHtml: true}}/></b></p>
                                                { this.state.studentId !== null ?
                                                    <div className="gray-dark-text">
                                                        <SelectorStudentProduct value={this.state.productId} data={this.state}
                                                                                updateParent={this.callLessons()}/>
                                                    </div>: null }
                                            </Col> */}
                                            {/* <Col xs={{size:12}} sm={{size:8}} md={{size:6}}>
                                                <p><b><Translate id="syllabus.select-lesson"
                                                                 options={{renderInnerHtml: true}}/></b></p>
                                                { this.state.productId !== null ?
                                                    <div className="gray-dark-text">
                                                        <SelectorLesson value={this.state.lessonId} data={this.state} subjectCode={this.state.subjectCode} levelCode={this.state.levelCode}
                                                                        updateParent={this.callStudentAnalysis()}/>
                                                    </div>: null }
                                            </Col> */}
                                        {/* </Row> */}
                                        {/* <Row>
                                            <Col xs={{size:12}} sm={{size:8}} md={{size:6}}>
                                                <p><b><Translate id="report-student-average.select-exam"
                                                                 options={{renderInnerHtml: true}}/></b></p>
                                                { this.state.lessonId !== null
                                                && this.state.productId !== null
                                                && this.state.studentId !== null
                                                    ?
                                                    <div className="gray-dark-text">
                                                        <SelectorExam value={this.state.examId} refresh={this.state}
                                                                      data={this.state}
                                                                      updateParent={this.callTries()}/>
                                                    </div>: null }
                                            </Col>
                                        </Row> */}
                                    {/* </CardBody>
                                </Card>
                            </Col> */}
                            {/* Columna con % en número <Col sm={{size: 12}} lg={{size: 4, offset: 0}} md={{size: 4, offset: 0}}>
                                <Card className="card-white">
                                    <CardHeader>
                                        <h4>
                                            <Translate id="report-student-average.lesson-score"
                                                       options={{renderInnerHtml: false}}/>
                                        </h4>
                                    </CardHeader>
                                    <CardBody>
                                        <Row>
                                            <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                { this.state.studentId !== null
                                                && this.state.productId !== null
                                                && this.state.lessonId !== null
                                                && this.state.passedTotal !== "0"
                                                && this.state.notPassedTotal !== "0"
                                                    ?
                                                    <h1 style={{textAlign: "center"}}>{this.state.passedTotal}</h1>
                                                    : null }
                                            </Col>
                                        </Row>
                                    </CardBody>
                                </Card>
                            </Col> */}
                        {/* </Row> */}

                        {/* <Row>
                            <Col xs={{size: 12}}>
                                <Card className="card-white">
                                    <CardHeader>
                                        <h4>
                                            <Translate id="report-student-average.average-lesson"
                                                       options={{renderInnerHtml: false}}/>
                                        </h4>
                                    </CardHeader>
                                    <CardBody>
                                        <Row>
                                            <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                { this.state.examId !== "0"
                                                && this.state.productId !== null
                                                && this.state.lessonId !== null
                                                && this.state.studentId !== null
                                                && this.state.triesChartData != null
                                                && this.state.triesChartData.labels.length > 0 ?
                                                    <ChartLessonTries data={this.state} refresh={this.state}/> : null }
                                            </Col>
                                            <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                {
                                                    this.state.examId !== "0"
                                                    && this.state.productId !== null
                                                    && this.state.lessonId !== null
                                                    && this.state.studentId !== null
                                                    && this.state.triesChartData !== null
                                                    && this.state.triesChartData.labels.length === 0 ?
                                                        <Alert color="danger">
                                                            <Translate id="report-student-average.pending"
                                                                       options={{renderInnerHtml: false}}/>
                                                        </Alert> : null}
                                            </Col>
                                        </Row>
                                    </CardBody>
                                </Card>
                            </Col>
                        </Row> */}

                    <div className="module-progress-exam">
                        <h2 className="progress-sub-title"><Translate id="report-student-average.average-lesson" options={{renderInnerHtml: false}}/></h2>
                        <div className="module-progress-exam__tries mt-5">
                            <div className="module-progress-exam__table mt-3">
                                { examId !== "0"
                                    && productId !== null
                                    && lessonId !== null
                                    && studentId !== null
                                    && triesData !== null && triesData.length > 0 ?
                                    <TableTotalTries data={triesData} profileId={studentId.value}
                                        refresh={triesData}/> : null }
                            </div>
                        </div>

                        <div className="module-progress-exam__tries mt-5">
                            {/*<div sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                { examId !== "0"
                                && productId !== null
                                && lessonId !== null
                                && studentId !== null
                                && triesChartData != null
                                && triesChartData.labels.length > 0 ?
                                    <ChartLessonTries data={state} refresh={state}/> : null }
                            </div> */}
                            <div>
                                {
                                    examId !== "0"
                                    && productId !== null
                                    && lessonId !== null
                                    && studentId !== null
                                    && triesChartData !== null
                                    && triesChartData.labels.length === 0 ?
                                        <Alert color="danger">
                                            <Translate id="report-student-average.pending"
                                                options={{renderInnerHtml: false}}/>
                                        </Alert> : null}
                            </div>

                        </div>
                    </div>

                        {/* <Row>
                            <Col lg={{size: 12}} md={{size: 12}} sm={{size: 12}}>
                                <Card className="card-white">
                                    <CardHeader>
                                        <h4>
                                            <Translate id="report-student-average.tries"
                                                       options={{renderInnerHtml: false}}/>
                                        </h4>
                                    </CardHeader>
                                    <CardBody>
                                        <Row>
                                            <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                { this.state.examId !== "0"
                                                && this.state.productId !== null
                                                && this.state.lessonId !== null
                                                && this.state.studentId !== null
                                                && this.state.triesData !== null && this.state.triesData.length > 0 ?
                                                    <TableTotalTries data={this.state.triesData} profileId={this.state.studentId.value}
                                                                     refresh={this.state.triesData}/> : null }
                                            </Col>
                                        </Row>
                                    </CardBody>
                                </Card>
                            </Col>
                        </Row> */}
                </div>
            </div>

            {loading ?
                <LoaderFullWindow/>: null
            }
        </div>
    )
};

export default withLocalize(ReportStudentLessonAverage);