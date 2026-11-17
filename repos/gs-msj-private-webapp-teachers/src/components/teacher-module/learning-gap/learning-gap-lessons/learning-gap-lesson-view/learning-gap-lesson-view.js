import React, { useState, useEffect } from 'react';
import {
    CardHeader,
    CardBody, Row, Col, Button,
    Accordion,
    AccordionItem,
    AccordionHeader,
    AccordionBody
} from 'reactstrap';
import {PulseLoader, RingLoader} from "react-spinners";

/** Multiple Language imports **/
import { Translate, withLocalize } from "react-localize-redux";
/** End of Multiple Language imports **/
import {
    getLessonDetailsById,
    getLessonDescriptionDetails,
    getLessonInitialActivityDetails,
    getLessonDevelopmentActivityDetails,
    getLessonClosingActivityDetails,
    getLessonFullCodesDetails,
    getLessonObjectiveDetails, 
    getLessonTransversalThemesDetails, 
    getLessonStrategiesDetails, 
    getLessonPerformanceTaskDetails, 
    getLessonConceptDetails, 
    getLessonDefinitionDetails, 
    getLessonExampleDetails, 
    getLessonQuizzesDetails 
} from "../../../../../api/teachers";
import 'react-datepicker/dist/react-datepicker.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {faCaretDown, faCaretUp, faCheckCircle, faPrint} from "@fortawesome/free-solid-svg-icons";
import {getLessonPDF} from "../../../../../api/teachers";
import { fetchLessonDetails } from '../../../../../redux/actions';
import { connect } from 'react-redux';
import {renderToStaticMarkup} from "react-dom/server";
import useVersionedTranslations from '../../../../../hooks/useVersionedTranslations';
import { SendAccessLog } from '../../../../../utils/access_logger';
import SessionManager from '../../../../../custom-classes/session-manager';
import Settings from '../../../../../custom-classes/settings';


/// The MED Quindío lesson template replaces the cross-curricular themes with the
/// suggested activities, so both lists are swapped in for that version only. The other
/// versions keep the accordions they have today.
const transversalThemesDetailsAPI = [
    {
        accordionId: "8",
        titleId: "learning-gap.transversal-themes",
        state: "transversalThemes",
        apiMethod: getLessonTransversalThemesDetails,
        typeDispatch: "FETCH_TRANSVERSALTHEMES_DETAIL",
        render: data => <ul>{data.map((item, index)=><li key={index}>{item.Desc}</li>)}</ul>
    }
];

const suggestedActivitiesDetailsAPI = [
    {
      accordionId: "activity-initial",
      titleId: "learning-gap.suggested-activity-initial",
      state: "initialActivity",
      apiMethod: getLessonInitialActivityDetails,
      typeDispatch: "FETCH_INITIAL_ACTIVITY_DETAIL",
      render: data => <div dangerouslySetInnerHTML={{ __html: data }}/>
    },
    {
      accordionId: "activity-development",
      titleId: "learning-gap.suggested-activity-development",
      state: "developmentActivity",
      apiMethod: getLessonDevelopmentActivityDetails,
      typeDispatch: "FETCH_DEVELOPMENT_ACTIVITY_DETAIL",
      render: data => <div dangerouslySetInnerHTML={{ __html: data }}/>
    },
    {
      accordionId: "activity-closing",
      titleId: "learning-gap.suggested-activity-closing",
      state: "closingActivity",
      apiMethod: getLessonClosingActivityDetails,
      typeDispatch: "FETCH_CLOSING_ACTIVITY_DETAIL",
      render: data => <div dangerouslySetInnerHTML={{ __html: data }}/>
    }
];

const lessonsDetailsAPI = [
    {
      accordionId: "2",
      titleId: "learning-gap.description",
      state: "description",
      apiMethod: getLessonDescriptionDetails,
      typeDispatch: "FETCH_DESCRIPTION_DETAIL",
      render: data => <div dangerouslySetInnerHTML={{ __html: data }}/>
    },
    ...(Settings.IS_MED_QUINDIO ? suggestedActivitiesDetailsAPI : []),
    {
      accordionId: "7",
      titleId: "learning-gap.codes",
      state: "constructedCodes",
      apiMethod: getLessonFullCodesDetails,
      typeDispatch: "FETCH_FULLCODES_DETAIL",
      render: items => (
          <ul>
              {items.map((item, index) => {
                  const models = item.ConstructedCodeCodeModels;
                  const lastModel = models && models.length > 0 ? models[models.length - 1] : null;

                  return (
                      <li key={index}>
                          {item.MainCode === "1" ? (
                              <b>{item.ConstructedCodeFull}</b>
                          ) : (
                              item.ConstructedCodeFull
                          )}
                          <br/>
                          {lastModel && (
                              <span
                                  dangerouslySetInnerHTML={{__html: lastModel.CodeDescription}}
                              />
                          )}
                      </li>
                  );
              })}
          </ul>
      )
    },
    {
        accordionId: "5",
        titleId: "learning-gap.objectives",
        state: "objectives",
        apiMethod: getLessonObjectiveDetails,
        typeDispatch: "FETCH_OBJECTIVES_DETAIL",
        render: data => <ul>{data.map((item, index) => <li key={index}>{item.Desc}</li>)}</ul>
    },
    ...(Settings.IS_MED_QUINDIO ? [] : transversalThemesDetailsAPI),
    {
      accordionId: "strategies",
      titleId: "learning-gap.reforming-strategy",
      state: "strategies",
      apiMethod: getLessonStrategiesDetails,
      typeDispatch: "FETCH_STRATEGIES_DETAIL",
      render: data => <ul>{data.map((item, index)=><li key={index}>{item.Desc}</li>)}</ul>
    },
    {
      accordionId: "6",
      titleId: "learning-gap.performance-tasks",
      state: "performanceTasks",
      apiMethod: getLessonPerformanceTaskDetails,
      typeDispatch: "FETCH_PERFORMANCETASK_DETAIL",
      render: data => <ul>{data.map((item, index)=><li key={index}>{item.Desc}</li>)}</ul>
    },
    {
      accordionId: "1",
      titleId: "learning-gap.concept",
      state: "concept",
      apiMethod: getLessonConceptDetails,
      typeDispatch: "FETCH_CONCEPT_DETAIL",
      render: data => <div dangerouslySetInnerHTML={{ __html: data }}/>
    },
    {
      accordionId: "3",
      titleId: "learning-gap.definitions",
      state: "definitions",
      apiMethod: getLessonDefinitionDetails,
      typeDispatch: "FETCH_DEFINITION_DETAIL",
      render: data => (
        <ul>
          {data.map((item, index) => (
            <li key={index}>
              <b><div dangerouslySetInnerHTML={{__html:item.Name}}/></b>{" "}
              <div dangerouslySetInnerHTML={{__html:item.Desc}}/>
            </li>
          ))}
        </ul>
      )
    },
    {
      accordionId: "4",
      titleId: "learning-gap.examples",
      state: "examples",
      apiMethod: getLessonExampleDetails,
      typeDispatch: "FETCH_EXAMPLE_DETAIL",
      render: data => (
        <ul>
          {data.map((item, index) => (
            <li key={index}>
              <b><div dangerouslySetInnerHTML={{__html:item.Name}}/></b>{" "}
              <div dangerouslySetInnerHTML={{__html:item.Desc}}/>
            </li>
          ))}
        </ul>
      )
    },
    {
      accordionId: "9",
      titleId: "learning-gap.quizzes",
      state: "quizzes",
      apiMethod: getLessonQuizzesDetails,
      examType: "0",
      typeDispatch: "FETCH_PRACTICE_DETAIL",
      render: quiz => <QuizzesDropdown quizzes={quiz}/>
    },
    {
        accordionId: "10",
        titleId: "learning-gap.pre-tests",
        state: "preTests",
        apiMethod: getLessonQuizzesDetails,
        examType: "2",
        typeDispatch: "FETCH_PRETESTS_DETAIL",
        render: quiz => <QuizzesDropdown quizzes={quiz}/>
    },
    {
        accordionId: "11",
        titleId: "learning-gap.exams",
        state: "exams",
        apiMethod: getLessonQuizzesDetails,
        examType: "1",
        typeDispatch: "FETCH_EXAMS_DETAIL",
        render: quiz => <QuizzesDropdown quizzes={quiz}/>
    },
    {
        accordionId: "12",
        titleId: "learning-gap.post-tests",
        state: "postTests",
        apiMethod: getLessonQuizzesDetails,
        examType: "3",
        typeDispatch: "FETCH_POSTESTS_DETAIL",
        render: quiz => <QuizzesDropdown quizzes={quiz}/>
    }
  ];

/**
 * Lesson preview for teacher-module (Gap)
 */

/**
 * To handle the quizzes dropdown
 * @param quizzes The quizzes data
 * @returns {Element} Returns the quizzes dropdown
 * @constructor
 */
export const QuizzesDropdown = ({ quizzes }) => {
    const [selectedQuiz, setSelectedQuiz] = useState(null);
    const [selectedQuestion, setSelectedQuestion] = useState({});

    const toggleQuiz = (quizIndex) => {
        setSelectedQuiz(selectedQuiz === quizIndex ? null : quizIndex);
    };

    // Sort quizzes by IsExam (descending) and Title (ascending)
    const sortedQuizzes = [...quizzes].sort((a, b) => {
        if (a.QuizModel.IsExam !== b.QuizModel.IsExam) {
            return a.QuizModel.IsExam - b.QuizModel.IsExam; // Sort by IsExam (ascending)
        }
        return a.QuizModel.Title.localeCompare(b.QuizModel.Title); // Then by Title (alphabetical)
    });

    const toggleQuestion = (quizIndex, questionIndex) => {
        setSelectedQuestion((prevState) => ({
            ...prevState,
            [quizIndex]: prevState[quizIndex] === questionIndex ? null : questionIndex,
        }));
    };

    return (
        <div className="quizzes-container">
            {sortedQuizzes.map((quiz, quizIndex) => (
                <div key={quizIndex} className="quiz-item">
                    {/* Quiz Dropdown Header */}
                    <div
                        className="quiz-header"
                        onClick={() => toggleQuiz(quizIndex)}
                        style={{
                            display: "flex",
                            justifyContent: "space-between",
                            cursor: "pointer",
                            padding: "10px",
                            borderBottom: "1px solid #ccc",
                        }}
                    >
                        <span><b>{quiz.QuizModel.Title}</b></span>
                        <FontAwesomeIcon
                            icon={selectedQuiz === quizIndex ? faCaretUp : faCaretDown}
                        />
                    </div>

                    {/* Quiz Details Dropdown */}
                    {selectedQuiz === quizIndex && (
                        <div className="quiz-details" style={{ padding: "10px" }}>
                            <h5><Translate id="learning-gap.details"/></h5>
                            <ul>
                                <li><strong><Translate id="learning-gap.instruction" />:</strong> {quiz.QuizModel.Instruction}</li>
                                <li><strong><Translate id="learning-gap.value" />:</strong> {quiz.QuizModel.CustomValue}</li>
                            </ul>

                            <h6><Translate id="learning-gap.questions" /></h6>
                            {quiz.QuestionRequestModelList?.map((question, questionIndex) => (
                                <div key={questionIndex} style={{ marginBottom: "10px" }}>
                                    {/* Question Dropdown Header */}
                                    <div
                                        className="question-header"
                                        onClick={() => toggleQuestion(quizIndex, questionIndex)}
                                        style={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            cursor: "pointer",
                                            padding: "10px",
                                            borderBottom: "1px solid #ddd",
                                        }}
                                    >
                                        <span><Translate id="learning-gap.premise" /> {questionIndex + 1}</span>
                                        <FontAwesomeIcon
                                            icon={
                                                selectedQuestion[quizIndex] === questionIndex
                                                    ? faCaretUp
                                                    : faCaretDown
                                            }
                                        />
                                    </div>

                                    {/* Question Details Dropdown */}
                                    {selectedQuestion[quizIndex] === questionIndex && (
                                        <div
                                            className="question-details"
                                            style={{
                                                padding: "10px",
                                                backgroundColor: "#f9f9f9",
                                                border: "1px solid #ccc",
                                            }}
                                        >
                                            <div
                                                dangerouslySetInnerHTML={{
                                                    __html: question.QuizQuestionModel.Desc,
                                                }}
                                            />
                                            <h6>{question.QuizQuestionResponseModelList != null &&
                                                question.QuizQuestionResponseModelList.length > 0 ?
                                                <Translate id="learning-gap.responses" /> : null }</h6>
                                            <ul>
                                                {question.QuizQuestionResponseModelList.map(
                                                    (response, responseIndex) => (
                                                        <li key={responseIndex} style={{ display: "flex", alignItems: "center" }}>
                                                            {/* Render response text */}
                                                            <div
                                                                dangerouslySetInnerHTML={{
                                                                    __html: response.Response,
                                                                }}
                                                                style={{ marginRight: "10px" }}
                                                            />
                                                            {/* Checkmark for correct responses */}
                                                            {response.Correct === "1" && (
                                                                <FontAwesomeIcon
                                                                    icon={faCheckCircle}
                                                                    style={{ color: "green" }}
                                                                />
                                                            )}
                                                        </li>
                                                    )
                                                )}
                                            </ul>
                                        </div>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            ))}
        </div>
    );
};

/**
 * To display the lesson details
 * @param lessonId The id of the lesson.
 * @returns {Element} Returns the lesson details.
 * @constructor
 */
export const LearningGapLessonView = ({
    lessonId,
    quizId = "",
    fetchLessonDetails,
    lessonDetails,
    lessonDetailsAPI = lessonsDetailsAPI,
}) => {
    const [loading, setLoading] = useState(true);
    const [smallLoading, setSmallLoading] = useState(false);
    const [lessonTitle, setLessonTitle] = useState('');
    const [subjectCode, setSubjectCode] = useState('');
    const [levelCode, setLevelCode] = useState('');
    const [accordionOpen, setAccordionOpen] = useState('');
    const [detailLoading, setDetailLoading] = useState({});
    const sessionDetails = SessionManager.getSessionDetails();
    const planning = SessionManager.getSelectedPlanning();
    const { planningId, planningName } = planning ?? {};
    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        fetchLessonDetails({typeDispatch: "RESET_LESSON_DETAILS"});
        callLessonCustomInfo(lessonId);

    }, [lessonId]); // Dependency array ensures this only runs on component mount or when lessonId changes.

    const toggleAccordion = (id) => {

        setAccordionOpen((prev) => (prev === id ? '' : id));
        // If the accordion is already open, do nothing
        if (accordionOpen === id) return; 

        const lessonDetailsItem = lessonDetailsAPI.find(({ accordionId }) => accordionId === id);

        // If no matching lesson detail found, do nothing   
        if (!lessonDetailsItem) return; 

        const { apiMethod, state, typeDispatch, examType } = lessonDetailsItem;
        if (!lessonDetails[state]?.length) {
            setDetailLoading((detailLoad) => ({ ...detailLoad, [id]: true }));
            fetchLessonDetails({ 
                lessonId, 
                apiMethod, 
                typeDispatch,
                examType
            }).finally(() => {
                setDetailLoading((detailLoad) => ({ ...detailLoad, [id]: false }));
            });
        }
    };

    const printLesson = () => {
        setSmallLoading(true);
        getLessonPDF(lessonId, quizId).then(response => {
            setSmallLoading(false);
            try{
                SendAccessLog({
                UserName: sessionDetails.Username,
                InvitationCode: sessionDetails.InvitationCode,
                Role: sessionDetails.Roles[0],
                AreaId: "14",
                PlanId: planningId ?? null,
                PlanName: planningName ?? null,
                Details: "Downloaded Lesson PDF in planning module",
                LessonId: lessonId ?? null,
                LessonTitle: lessonTitle ?? null,
                Subject: subjectCode ?? null,
                Grade: levelCode ?? null,
                });
            } catch(error){}
            window.open(response.data);
        });
    };

    const callLessonCustomInfo = async (lessonId) => {
        setLoading(true);
        try {
            const response = await getLessonDetailsById(lessonId);
            setLoading(false);
            const lessonData = response.data;
            setLessonTitle(lessonData.LessonTitle);
            setSubjectCode(lessonData.SubjectCode);
            setLevelCode(lessonData.LevelCode);
        }
        catch(e) {
            setLoading(false);
            console.error("Failed to fetch lesson details");
        }
    };

    return (
        <div>
            {loading ?
            <div style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                width: "100%",
                height: "50vh"
            }}>
                <RingLoader size={100} color="#8bb3cc" />
            </div> :
                <>
            <CardHeader>
                <Row>
                    <Col>
                        <h3>{lessonTitle}</h3>
                        <small>{subjectCode} {levelCode}</small>
                    </Col>
                    <Col className="text-right">
                        <button
                            className='btn-default button-primary-custom'
                            onClick={printLesson}
                            disabled={smallLoading} // Disable button when smallLoading is true
                        >
                            {smallLoading ? (
                                <PulseLoader size={10} color="#395a8f" />
                            ) : (
                                <div className="print-pdf-btn">
                                    <FontAwesomeIcon icon={faPrint} /> 
                                    <Translate id="learning-gap.view-pdf" />
                                </div>
                            )}
                        </button>
                    </Col>
                </Row>
            </CardHeader>
            <CardBody>
                <Accordion open={accordionOpen} toggle={toggleAccordion}>
                {lessonDetailsAPI.map(item => (
                  <AccordionItem key={item.accordionId}>
                    <AccordionHeader targetId={item.accordionId}>
                      <Translate id={item.titleId}/> {' '}
                      <FontAwesomeIcon
                        icon={accordionOpen===item.accordionId ? faCaretUp : faCaretDown}
                        className="ms-2"
                      />
                    </AccordionHeader>
                    <AccordionBody accordionId={item.accordionId}>
                      { detailLoading[item.accordionId]
                          ? <PulseLoader size={10} />
                          : item.render(lessonDetails[item.state])
                      }
                    </AccordionBody>
                  </AccordionItem>
                ))}
                </Accordion>
            </CardBody>
                </>}
        </div>
    );
};

const mapStateToProps = state => {
    return {
        lessonDetails: state.lessonDetails,
    }
};

const actionCreators = {
    fetchLessonDetails
};

export default withLocalize(connect(mapStateToProps, actionCreators)(LearningGapLessonView));
