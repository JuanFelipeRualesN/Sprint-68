/**
 * Created by portiz on 9/20/18.
 */
import React, { useEffect, useState } from 'react';
import {Col, Card, CardBody, CardHeader, Container, Row, Button} from 'reactstrap';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/

import {getStudentAverage} from '../../../api/gsweb';
import SelectorStudentProduct from "../selector/selector-student-products";
import ChartAverage from '../charts/chart-average';
import TableTotalLessons from '../table/table-total-lessons';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faArrowAltCircleLeft, faChartPie, faArrowLeft} from '@fortawesome/free-solid-svg-icons';
import Settings from "../../../custom-classes/settings";
import ProfilesDataTable from "../datatables/profiles-data-table";
import {Link} from "react-router-dom";
import LoaderFullWindow from "../loader-windows/loader-full-window";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import "./style.css";
/**
 * Class/component of the student average
 */
const ReportStudentAverage = (props) => {
    const [isStaff] = useState(false);
    const [loading, setLoading] = useState(false);
    const [languages] = useState(Settings.LANGUAGES);
    const [lessonAverageCompleteInfo, setLessonAverageCompleteInfo] = useState([]);
    const [notPassedTotal, setNotPassedTotal] = useState(0);
    const [passedTotal, setPassedTotal] = useState(0);
    const [productId, setProductId] = useState(null);
    const [subjectCode, setSubjectCode] = useState(null);
    const [levelCode, setLevelCode] = useState(null);
    const [roleId] = useState(Settings.ROLE_STUDENT_ID);
    const [studentFullName, setStudentFullName] = useState(null);
    const [studentId, setStudentId] = useState(null);
    const [backUrl] = useState(
        typeof props.location !== 'undefined' && props.location !== null && typeof props.location.query !== 'undefined' &&
        props.location.query !== null ? props.location.query : '/dashboard'
    );

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Call the user details to set everything
    const callStudentAnalysis = () => (newValue) => {
        setProductId(null);
        setSubjectCode(null);
        setLevelCode(null);
        setLoading(true);
        setPassedTotal(null);
        setNotPassedTotal(null);
        setLessonAverageCompleteInfo(null);

        setProductId(newValue.value);
        setSubjectCode(newValue.subjectCode);
        setLevelCode(newValue.levelCode);
        
        // Call the api of student average
        let apiCall = getStudentAverage(studentId, newValue.subjectCode, newValue.levelCode);
        apiCall.then((response) => {
            let passedTotalValue = 0;
            let notPassedTotalValue = 0;
            if (response.data.AverageWithLessonResultDetails != null) {
                passedTotalValue = parseFloat(response.data.AverageWithLessonResultDetails.Average);
                notPassedTotalValue = parseFloat(100 - parseFloat(response.data.AverageWithLessonResultDetails.Average)).toFixed(2);
            }
            setLoading(false);
            setPassedTotal(passedTotalValue);
            setNotPassedTotal(notPassedTotalValue);
            setLessonAverageCompleteInfo(response.data.LessonAverageCompleteInfoResponseList);
        });
    };

    // Call the student classes
    const callStudentProducts = () => (newValue) => {
        setStudentId(newValue.UserId);
        setProductId(null);
        setStudentFullName(newValue.GivenName + ' ' + newValue.Surname);
    };

    const showStudentBox = () => {
        setStudentId(null);
        setProductId(null);
    };

    const getCurrentState = () => {
        return {
            backUrl, isStaff, loading, languages, lessonAverageCompleteInfo, notPassedTotal, passedTotal,
            productId, subjectCode, levelCode, roleId, studentFullName, studentId,
        };
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
                                <Translate id="navbar.student-average" options={{renderInnerHtml: true}}/>
                            </div>
                        </div>
                    </div>

                    {/* <Row>
                        <Col xs={{size:12}}>
                            <h2>
                                <FontAwesomeIcon icon={faChartPie}/>{' '}
                                <Translate id="navbar.student-average" options={{renderInnerHtml: true}}/>
                            </h2>
                        </Col>
                        <Col xs={{size:12}}>
                            <Link to={backUrl} className="btn btn-warning">
                                <FontAwesomeIcon icon={faArrowAltCircleLeft}/>{' '}
                                <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
                            </Link>
                        </Col>
                    </Row> */}
                    <Row className="clear-style-card td-w-rollover td-responsive  special-top-space syle-date-five">
                        <Col sm={{size: 12, offset: 0}} lg={{size: 12, offset: 0}}>
                            <Card>
                                <small>
                                    <Translate id="report-student-average.student-average-instructions"
                                        options={{renderInnerHtml: false}}/>
                                </small>
                                
                                <CardBody>
                                    <Row>
                                        {
                                            studentId === null || studentId === '' ?
                                            <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                                <p><b><Translate id="report-student-average.select-student"
                                                                 options={{renderInnerHtml: true}}/></b></p>
                                                <ProfilesDataTable
                                                    value={studentId}
                                                    isStaff={isStaff} roleId={roleId}
                                                    updateParent={callStudentProducts()}/>
                                            </Col> : null
                                        }
                                    </Row>
                                    <Row>
                                        {
                                            studentId !== null && studentId !== '' ?
                                                <Col xs={12}>
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
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>

                     {/* CLASE */}
                     <Row className="clear-style-card min-height-clear mt-4">
                       
                        <Col sm={{size: 12, offset: 0}} lg={{size: 12, offset: 0}}>
                            <Card>
                                <div className="title-principal">
                                    <Translate id="report-student-average.class"
                                         options={{renderInnerHtml: false}}/>
                                </div>
                                <small>
                                    <Translate id="report-student-average.class-instructions"
                                            options={{renderInnerHtml: false}}/>
                                </small>

                                <CardBody>
                                    <Row>
                                        <Col sm={{size: 12}} md={{size: 6}} lg={{size: 6}}>
                                            <p><b><Translate id="report-student-average.select-class"
                                                             options={{renderInnerHtml: true}}/></b></p>
                                            { studentId !== null ?
                                                <div className="gray-dark-text">
                                                    <SelectorStudentProduct data={getCurrentState()}
                                                                        updateParent={callStudentAnalysis()}/>
                                                </div>
                                                : null }
                                        </Col>
                                    </Row>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>

                    {/* Promedio */}
                    <Row className="clear-style-card">
                        <Col lg={{size: 12}} md={{size: 12}} sm={{size: 12}}>
                            <Card>
                                <div className='special-average-homeschooler text-right'>
                                    <Translate id="report-student-average.student-average"
                                        options={{renderInnerHtml: false}}/>
                                    <span className='pl-3'>{ studentId !== null
                                        && productId !== null
                                        && passedTotal !== 0
                                        && notPassedTotal !== 0 ?
                                    <b>{passedTotal}%</b> : null }</span>
                                </div>
                                {/* <Row>
                                    <Col xs={9}><b>
                                        <Translate id="report-student-average.student-average"
                                            options={{renderInnerHtml: false}}/></b>
                                    </Col>
                                    <Col xs={3}>{ studentId !== null
                                    && productId !== null
                                    && passedTotal !== 0
                                    && notPassedTotal !== 0 ?
                                        <b>{passedTotal}%</b> : null }</Col>

                                </Row> */}
                                <CardBody>
                                    <Row className="content-table-data td-responsive adjustment-font-table">
                                        <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                            {
                                                productId !== null &&
                                                lessonAverageCompleteInfo !== null &&
                                                typeof lessonAverageCompleteInfo !== 'undefined' &&
                                                lessonAverageCompleteInfo.length > 0 ?
                                                    <TableTotalLessons data={getCurrentState()}/> : null }
                                        </Col>
                                    </Row>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </div>
            </div>

            {loading ?
                <LoaderFullWindow/>: null
            }
        </div>
    );
};

export default withLocalize(ReportStudentAverage);