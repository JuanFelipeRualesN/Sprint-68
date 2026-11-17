import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { getPrePostResultsTeacher } from "../../../api/teachers";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import {
    faCheck,
    faEye,
} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ModalQuizResults from "../../shared-module-components/modal/modal-quiz-results";
import styled from "styled-components";

const EmptyListStyled = styled.p`
  color: #ccc;
  display: block
  justify-content: center;
  text-align: center;
  margin: 3em auto;

  & img {
    display: block;
    margin: 1em auto 0.25em auto;
    font-size: 45px;
    width: 175px;
    opacity: .5;
  }

  & i {
    display: block;
    margin: 1em auto 0.25em auto;
    font-size: 45px;
    width: 175px;
    opacity: .5;
  }
`;
const ModalPrePostResults = ({ studentId, teacherPlanLessonId, visible, studentName, lessonTitle, updateParent}) => {
    const [isVisible, setIsVisible] = useState(visible);
    const [loading, setLoading] = useState(false);
    const [preTest, setPreTest] = useState(null);
    const [postTest, setPostTest] = useState(null);
    const [visibleModalQuizResult, setVisibleModalQuizResult] = useState(false);
    const [usquiUserResultId, setUsquiUserResultId] = useState(null);

    useEffect(() => {
        loadPrePost();
    }, []);

    const loadPrePost = async () => {
        setLoading(true);
        try {
            const res = await getPrePostResultsTeacher(teacherPlanLessonId, studentId);
            const data = res.data;

            if (data.length > 0) {
                setPreTest(data.filter(item => item.PrePost === '0')[0]);
                setPostTest(data.filter(item => item.PrePost === '1')[0]);
            }
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    // Hide or shows the window
    const toggle = () => {
        setIsVisible(!isVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent(!isVisible);
    };

    const toggleModalQuizResult = (visible) => {
        setVisibleModalQuizResult(visible);
    };

    const handleClickOpenModalQuizResult = (usquiUserResultId) => {
        setUsquiUserResultId(usquiUserResultId);
        setVisibleModalQuizResult(true);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                </ModalHeader>
                <ModalBody>
                    <div className="wrapper-progress-pos-pre mt--5">

                        <h1 className="progress-title"> <Translate id="progress.pre-post-results-title"
                            options={{ renderInnerHtml: true }} /></h1>
                        <div className="progress-subtitle">
                            <Translate id="report-student-average.student" options={{ renderInnerHtml: true }} />: <b>{studentName}</b>
                        </div>

                        <div className="progress-subtitle__small">
                            <Translate id="report-student-average.lesson" options={{ renderInnerHtml: true }} />: <b>{lessonTitle}</b>
                        </div>

                        <div className="container-progress-pos-pre mt-3">
                            {preTest ?
                                (<div className="progress-pos-pre__col">
                                    <div className="pos-pre__col--title-results">  <Translate id="progress.results-pretest"
                                        options={{ renderInnerHtml: true }} /></div>
                                    <div className="pos-pre__col--date"> <Translate id="progress.pre-post-completed-date"
                                        options={{ renderInnerHtml: true }} />: {preTest.CreatedAt}</div>

                                    <div className="progress-pos-pre__col_results mt-4">
                                        <div className="col_results--tries">
                                            <h1 className="result-modal">
                                                {" "}
                                                {preTest.TotalCorrect}/{preTest.TotalQuestions}{" "}
                                            </h1>
                                        </div>
                                        <div className="col_results--graph variant-color">
                                            <div className={`c100 p${Math.round(preTest.Score)} big`}>
                                                <span>{preTest.Score}%</span>
                                                <div className="slice">
                                                    <div className="bar"></div>
                                                    <div className="fill"></div>
                                                </div>
                                            </div>
                                        </div>

                                        <div className="modal-footer">
                                            <Button className="btn-see-exam" onClick={() => handleClickOpenModalQuizResult(preTest.UsquiUserResultId)}>
                                                <FontAwesomeIcon icon={faEye} />{' '}
                                                <Translate id="progress.pre-test-view-details"
                                                    options={{ renderInnerHtml: false }} />
                                            </Button>
                                        </div>

                                    </div>
                                </div>) :
                                (<div className="progress-pos-pre__col">
                                    <EmptyListStyled>
                                        <i className="fas fa-book-reader"></i><Translate id="progress.pre-post-empty-pre" options={{ renderInnerHtml: true }} />
                                    </EmptyListStyled>
                                </div>)
                            }

                            {postTest ? (
                                <div className="progress-pos-pre__col">
                                    <div className="pos-pre__col--title-results"> <Translate id="progress.results-posttest"
                                        options={{ renderInnerHtml: true }} /></div>
                                    <div className="pos-pre__col--date"> <Translate id="progress.pre-post-completed-date"
                                        options={{ renderInnerHtml: true }} />: {postTest.CreatedAt}</div>

                                    <div className="progress-pos-pre__col_results mt-4">
                                        <div className="col_results--tries">
                                            <h1 className="result-modal">
                                                {" "}
                                                {postTest.TotalCorrect}/{postTest.TotalQuestions}{" "}
                                            </h1>
                                        </div>

                                        <div className="col_results--graph">
                                            <div className={`c100 p${Math.round(postTest.Score)} big`}>
                                                <span>{postTest.Score}%</span>
                                                <div className="slice">
                                                    <div className="bar"></div>
                                                    <div className="fill"></div>
                                                </div>
                                            </div>
                                        </div>

                                        <div className="modal-footer">
                                            <Button className="btn-see-exam" onClick={() => handleClickOpenModalQuizResult(postTest.UsquiUserResultId)}>
                                                <FontAwesomeIcon icon={faEye} />{' '}
                                                <Translate id="progress.post-test-view-details"
                                                    options={{ renderInnerHtml: true }} />
                                            </Button>
                                        </div>
                                    </div>
                                </div>
                            ) :
                                (<div className="progress-pos-pre__col">
                                    <EmptyListStyled>
                                        <i className="fas fa-book-reader"></i><Translate id="progress.pre-post-empty-post" options={{ renderInnerHtml: true }} />
                                    </EmptyListStyled>
                                </div>)}
                        </div>
                    </div>

                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="general.close" />
                    </Button>
                </ModalFooter>
            </Modal>
            {loading ? <LoaderFullWindow /> : null}
            {visibleModalQuizResult ? <ModalQuizResults visible={visibleModalQuizResult} studentId={studentId} usquiUserResultId={usquiUserResultId} updateParent={toggleModalQuizResult} /> : null}
        </>
    );
};

export default ModalPrePostResults;