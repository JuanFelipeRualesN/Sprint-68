/**
 * Created by portiz on 5/08/20.
 */
import React, { useState, useEffect } from 'react';
import {
    CardHeader,
    CardBody, Row, Col, Button
} from 'reactstrap';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import {getLessonDetails} from "../../../api/athenas";
import LanguageManager from '../../../custom-classes/language-manager';
import 'react-datepicker/dist/react-datepicker.css';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPrint} from "@fortawesome/free-solid-svg-icons";
import {getLessonPDF} from "../../../api/teachers";
import WrapperOverlappingMedia from '../wrapper-overlapping-media';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * Lesson preview for teacher-module
 */
const LessonPresentationTeacher = ({subjectLangCode, lessonId}) => {
    const languages = [
        {name: "English", code: "en"},
        {name: "Español", code: "es"}
    ];

    const [lessonDescription, setLessonDescription] = useState('');
    const [lessonId] = useState(lessonId ?? null);
    const [subjectLangCode] = useState(subjectLangCode ?? null);
    const [translateDefinition] = useState(LanguageManager.getTranslationByLangCode("planning.definitions", languages, subjectLangCode));
    const [translateEvaluations] = useState(LanguageManager.getTranslationByLangCode("planning.evaluations", languages, subjectLangCode));
    const [translateExamples] = useState(LanguageManager.getTranslationByLangCode("planning.examples", languages, subjectLangCode));
    const [translateDescription] = useState(LanguageManager.getTranslationByLangCode("planning.description", languages, subjectLangCode));
    const [translateConcept] = useState(LanguageManager.getTranslationByLangCode("planning.concept", languages, subjectLangCode));
    const [translateObjectives] = useState(LanguageManager.getTranslationByLangCode("planning.objectives", languages, subjectLangCode));
    const [translatePractice] = useState(LanguageManager.getTranslationByLangCode("planning.practice", languages, subjectLangCode));
    const [translateStandardCode] = useState(LanguageManager.getTranslationByLangCode("planning.standard-code", languages, subjectLangCode));
    const [translateStrategy] = useState(LanguageManager.getTranslationByLangCode("planning.strategy", languages, subjectLangCode));
    const [translateTest] = useState(LanguageManager.getTranslationByLangCode("planning.test", languages, subjectLangCode));
    const [translateGapClosing] = useState(LanguageManager.getTranslationByLangCode("planning.gap-closing", languages, subjectLangCode));
    const [translateType] = useState(LanguageManager.getTranslationByLangCode("planning.typeIsGapClosing", languages, subjectLangCode));
    const [loading, setLoading] = useState(false);
    const [syllabusInfo, setSyllabusInfo] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        callLessonDescription(lessonId);
    }, [lessonId]);

    // Opens a new tab with the PDF location.
    const printLesson = () => {
        let apiCall = getLessonPDF(lessonId);

        setLoading(true);
        apiCall.then((response) => {
            setLoading(false);
            window.open(response.data);
        });
    };

    // Returns the lesson description
    const callLessonDescription = (lessonId) => {
        setLoading(true);
        let apiSyllabus = getLessonDetails(lessonId);
        let dataSyllabus;
        apiSyllabus.then((response) => {
            let htmlToAdd = '';
            if(typeof response.data.LessonModifierRequestModel !== 'undefined' && response.data.LessonModifierRequestModel !== null)
            {
                // Lesson Title
                if(typeof response.data.LessonModifierRequestModel.LessonModel !== 'undefined' &&
                    response.data.LessonModifierRequestModel.LessonModel !== null) {
                    htmlToAdd += '<h2>' + response.data.LessonModifierRequestModel.LessonModel.LessonTitle + '</h2>';
                    htmlToAdd += '</hr>';
                }
                
                // is Gap closing
                if(typeof response.data.LessonModifierRequestModel.LessonModel !== 'undefined' &&
                    response.data.LessonModifierRequestModel.LessonModel !== null) {
                    if(response.data.LessonModifierRequestModel.LessonModel.IsGapClosing === "1") {
                        htmlToAdd += '</br>';
                        htmlToAdd += '<h3>' + translateType + '</h3>';
                        htmlToAdd += '<p>' + translateGapClosing + '</p>';
                        
                    }
                }

                // Standard codes
                if(typeof response.data.LessonModifierRequestModel.LessonStandardCodeModelList !== 'undefined'
                    && response.data.LessonModifierRequestModel.LessonStandardCodeModelList !== null
                    && response.data.LessonModifierRequestModel.LessonStandardCodeModelList.length > 0 ) {

                    htmlToAdd += '<br/>';
                    htmlToAdd += '<h3>' + translateStandardCode + "</h3>";
                    for(let i=0; i < response.data.LessonModifierRequestModel.LessonStandardCodeModelList.length; i++)
                    {
                        if(response.data.LessonModifierRequestModel.LessonStandardCodeModelList[i].StandardCode !== null)
                        {
                            htmlToAdd += response.data.LessonModifierRequestModel.LessonStandardCodeModelList[i].StandardCode;
                            htmlToAdd += '<br/>'
                        }
                    }
                }

                // Strategies
                if(typeof response.data.LessonModifierRequestModel.LessonStrategyModelList !== 'undefined'
                    && response.data.LessonModifierRequestModel.LessonStrategyModelList !== null
                    && response.data.LessonModifierRequestModel.LessonStrategyModelList.length > 0 ) {

                    htmlToAdd += '<br/>';
                    htmlToAdd += '<h3>' + translateStrategy + "</h3>";
                    for(let i=0; i < response.data.LessonModifierRequestModel.LessonStrategyModelList.length; i++)
                    {
                        if(response.data.LessonModifierRequestModel.LessonStrategyModelList[i].Desc !== null)
                        {
                            htmlToAdd += response.data.LessonModifierRequestModel.LessonStrategyModelList[i].Desc;
                            htmlToAdd += '<br/>'
                        }
                    }
                }

                // Objectives
                if(typeof response.data.LessonModifierRequestModel.LessonObjectiveModelList !== 'undefined'
                    && response.data.LessonModifierRequestModel.LessonObjectiveModelList !== null
                    && response.data.LessonModifierRequestModel.LessonObjectiveModelList.length > 0 ) {

                    htmlToAdd += '<br/>';
                    htmlToAdd += '<h3>' + translateObjectives + "</h3>";
                    for(let i=0; i < response.data.LessonModifierRequestModel.LessonObjectiveModelList.length; i++)
                    {
                        if(response.data.LessonModifierRequestModel.LessonObjectiveModelList[i].Desc !== null)
                        {
                            htmlToAdd += response.data.LessonModifierRequestModel.LessonObjectiveModelList[i].Desc;
                            htmlToAdd += '<br/>'
                        }
                    }
                }

                // Concept
                if(typeof response.data.LessonModifierRequestModel.LessonDetailModel !== 'undefined'
                    && response.data.LessonModifierRequestModel.LessonDetailModel !== null
                ) {

                    htmlToAdd += '<br/>';

                    if(response.data.LessonModifierRequestModel.LessonDetailModel.Description !== null)
                    {
                        htmlToAdd += response.data.LessonModifierRequestModel.LessonDetailModel.Description;
                        htmlToAdd += '<br/>'
                    }
                    htmlToAdd += '<h3>' + translateConcept + "</h3>";
                    if(response.data.LessonModifierRequestModel.LessonDetailModel.Concept !== null)
                    {
                        htmlToAdd += response.data.LessonModifierRequestModel.LessonDetailModel.Concept;
                        htmlToAdd += '<br/>'
                    }
                }

                // Definition
                if(typeof response.data.LessonModifierRequestModel.LessonDefinitionModelList !== 'undefined'
                    && response.data.LessonModifierRequestModel.LessonDefinitionModelList !== null
                    && response.data.LessonModifierRequestModel.LessonDefinitionModelList.length > 0 ) {

                    htmlToAdd += '<br/>';
                    htmlToAdd += '<h3>' + translateDefinition + "</h3>";

                    for(let i=0; i < response.data.LessonModifierRequestModel.LessonDefinitionModelList.length; i++)
                    {
                        if(response.data.LessonModifierRequestModel.LessonDefinitionModelList[i].Name !== null)
                        {
                            htmlToAdd += response.data.LessonModifierRequestModel.LessonDefinitionModelList[i].Name;
                        }

                        if(response.data.LessonModifierRequestModel.LessonDefinitionModelList[i].Desc !== null)
                        {
                            htmlToAdd += response.data.LessonModifierRequestModel.LessonDefinitionModelList[i].Desc;
                            htmlToAdd += '<br/>'
                        }
                    }
                }

                // Example
                if(typeof response.data.LessonModifierRequestModel.LessonExampleModelList !== 'undefined'
                    && response.data.LessonModifierRequestModel.LessonExampleModelList !== null
                    && response.data.LessonModifierRequestModel.LessonExampleModelList.length > 0) {

                    htmlToAdd += '<br/>';
                    htmlToAdd += '<h3>' + translateExamples + "</h3>";
                    for(let i=0; i < response.data.LessonModifierRequestModel.LessonExampleModelList.length; i++)
                    {
                        if(typeof response.data.LessonModifierRequestModel.LessonExampleModelList[i] !== 'undefined' &&
                            response.data.LessonModifierRequestModel.LessonExampleModelList[i] !== null) {
                            if (response.data.LessonModifierRequestModel.LessonExampleModelList[i].Name !== null) {
                                htmlToAdd += response.data.LessonModifierRequestModel.LessonExampleModelList[i].Name;
                            }

                            if (response.data.LessonModifierRequestModel.LessonExampleModelList[i].Desc !== null) {
                                htmlToAdd += response.data.LessonModifierRequestModel.LessonExampleModelList[i].Desc;
                                htmlToAdd += '<br/>'
                            }
                        }
                    }
                }

                // Quizzes
                if(typeof response.data.LessonModifierRequestModel.QuizRequestModelList !== 'undefined'
                    && response.data.LessonModifierRequestModel.QuizRequestModelList !== null
                    && response.data.LessonModifierRequestModel.QuizRequestModelList.length > 0) {

                    htmlToAdd += '<br/>';
                    htmlToAdd += '<h3>' + translateEvaluations + "</h3>";

                    for(let i=0; i < response.data.LessonModifierRequestModel.QuizRequestModelList.length; i++)
                    {
                        if(response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuizModel.IsExam == 0) {
                            htmlToAdd += '<h4>' + translatePractice + " "  + '</h4>';
                        }
                        else{
                            htmlToAdd += '<h4>'  + translateTest + " " + '</h4>';
                        }

                        if(response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuizModel !== null)
                        {
                            htmlToAdd += '<h3>' + response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuizModel.Title + '</h3>';
                            htmlToAdd += '<hr/>';
                            let quizInstruction = response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuizModel.Instruction;
                            htmlToAdd += `<br /> <div style="word-wrap: break-word; overflow-wrap: break-word;"> ${quizInstruction} </div>`;
                            htmlToAdd += '<br/>';

                            if(response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuizExtraInfoModel !== null)
                            {
                                const extraInfo = response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuizExtraInfoModel.ExtraInfo;
                                htmlToAdd += extraInfo ? `<br /> <div style="word-wrap: break-word; overflow-wrap: break-word;"> ${extraInfo} </div>` : '';
                            }
                        }

                        for(let j=0; j < response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList.length; j++)
                        {
                            if(response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionModel !== null &&
                                response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionModel.Desc !== null) {
                                let questionText = response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionModel.Desc;
                                htmlToAdd += `<br /> <div style="word-wrap: break-word; overflow-wrap: break-word;"> ${questionText} </div>`;

                            }
                            htmlToAdd += '<br/>';
                            for(let k=0; k < response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionResponseModelList.length;k++)
                            {
                                if(response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionResponseModelList[k] !== null
                                && response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionResponseModelList[k].Response !== null) {
                                    let responseText = response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionResponseModelList[k].Response;
                                    responseText = `<br /> <div style="word-wrap: break-word; overflow-wrap: break-word;"> ${responseText} </div>`;

                                    if (response.data.LessonModifierRequestModel.QuizRequestModelList[i].QuestionRequestModelList[j].QuizQuestionResponseModelList[k].Correct === "1") {

                                        htmlToAdd += '<span style="color: green">';
                                        htmlToAdd += responseText
                                        htmlToAdd += '</span>';
                                    } else {
                                        htmlToAdd += '<span style="color: darkred">';
                                        htmlToAdd += responseText;
                                        htmlToAdd += '</span>';
                                    }
                                    htmlToAdd += '<br/>';
                                }
                            }
                            htmlToAdd += '<br/>';
                        }
                    }
                }

                setLessonDescription(htmlToAdd);
                setLoading(false);
            }

            dataSyllabus = response.data.LessonModifierRequestModel.LessonDetailModel.Description;
        }).then(() => {
            setSyllabusInfo(dataSyllabus);
        });
    };

    return (
        <div>
            {loading ?
                <LoaderFullWindow/> : null
            }

            {lessonId !== null ?
                <div>
                    <CardHeader>
                        <Row>
                            <Col xs={11}>
                            <h4><Translate id="planning.lesson-details"/></h4>
                            </Col>
                            <Col xs={1}>
                                <Button color="info" onClick={printLesson}>
                                    <FontAwesomeIcon icon={faPrint}/>
                                </Button>
                            </Col>
                        </Row>
                    </CardHeader>
                        <CardBody>
                            {
                                lessonDescription !== '' ?
                                    <WrapperOverlappingMedia section={lessonDescription}>
                                        <div dangerouslySetInnerHTML={{__html: (lessonDescription)}}/>
                                    </WrapperOverlappingMedia>
                                    : null
                            }
                        </CardBody>
                </div>: null
            }
        </div>
    );
};

export default withLocalize(LessonPresentationTeacher);
