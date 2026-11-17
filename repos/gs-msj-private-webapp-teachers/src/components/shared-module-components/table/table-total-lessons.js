/**
 * Created by portiz on 9/23/18.
 */
import React, { useState, useEffect } from 'react';
import {Table} from 'reactstrap';
import { Link } from "react-router-dom";
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faEye} from "@fortawesome/free-solid-svg-icons";
/** End of Multiple Language imports **/

/**
 * Class that handles the student scores by the lessons
 */
const TableTotalLessons = ({ data }) => {
    const [languages, setLanguages] = useState(data.languages);
    const [lessonAverageCompleteInfo, setLessonAverageCompleteInfo] = useState(data.lessonAverageCompleteInfo);
    const [modal, setModal] = useState(false);
    const [productId, setProductId] = useState(data.productId);
    const [subjectCode, setSubjectCode] = useState(data.subjectCode);
    const [levelCode, setLevelCode] = useState(data.levelCode);
    const [rows, setRows] = useState([]);
    const [studentId, setStudentId] = useState(data.studentId);

    const toggle = () => {
        setModal(!modal);
    };

    const mountData = () => {
        let lessonDetails = [];
        let lessonList = lessonAverageCompleteInfo;

        for (let i = 0; i < lessonList?.length; i++) {
            let row;

            if(lessonList[i].Average !== null && lessonList[i].Average.length > 0) {
                row =
                    <tr key={lessonList[i].LessonId}>
                        <td>{lessonList[i].LessonTitle}</td>
                        <td>{lessonList[i].Average}</td>
                        <td>
                            <Link to={{pathname: '/report-lesson-average', state:{
                                productId: productId,
                                subjectCode: subjectCode,
                                levelCode: levelCode,
                                studentId: studentId,
                                lessonId: {value: lessonList[i].LessonId, label: lessonList[i].LessonTitle }
                            }}} className="btn-default button-secondary-custom">
                                <FontAwesomeIcon icon={faEye}/>{' '}
                                <Translate id="report-student-average.see-details" options={{renderInnerHtml: false}}/>
                            </Link>
                        </td>
                    </tr>;
            }
            else
            {
                row =
                    <tr key={lessonList[i].LessonId}>
                        <td>{lessonList[i].LessonTitle}</td>
                        <td>{lessonList[i].Average}</td>
                        <td>
                            <span className='pending-text'>
                                <Translate id="report-student-average.pending" options={{renderInnerHtml: false}}/>
                            </span>
                        </td>
                    </tr>;
            }
            lessonDetails.push(row);
        }

        setRows(lessonDetails);
    };

    useEffect(() => {
        setLessonAverageCompleteInfo(data.lessonAverageCompleteInfo);
        setLanguages(data.languages);
        setStudentId(data.studentId);
        setSubjectCode(data.subjectCode);
        setLevelCode(data.levelCode);
        setProductId(data.productId);
    }, [data]);

    useEffect(() => {
        mountData();
    }, [lessonAverageCompleteInfo, productId, subjectCode, levelCode, studentId]);

    return (
        <div>
            <Table responsive>
                <thead>
                <tr>
                    <th><Translate id="report-student-average.lesson" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="report-student-average.score" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="report-student-average.see-details" options={{renderInnerHtml: false}}/></th>
                </tr>
                </thead>
                <tbody>
                { rows !== "0" ? rows : null }
                </tbody>
            </Table>
        </div>
    );
};

export default TableTotalLessons;