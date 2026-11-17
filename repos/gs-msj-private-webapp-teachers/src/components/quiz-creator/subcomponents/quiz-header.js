import React, { useState, useEffect, useRef, useCallback } from 'react';
import {Card, CardBody, FormGroup, Input, Col, Row,Button, Modal, ModalHeader, ModalBody, ModalFooter, Badge, UncontrolledAlert, UncontrolledTooltip} from 'reactstrap';
import Dropdowns from '../common/drop-downs';
import { Translate } from "react-localize-redux";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { withLocalize } from 'react-localize-redux';
import { faUpload } from '@fortawesome/free-solid-svg-icons'
import MyUploader from './../common/dropzone-uploader';
import {getQuizTypes} from '../../../api/teachers';
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {Subject} from "../../../custom-classes/subject";
import Editor from '../../common/editor';
import WrapperOverlappingMedia from '../../teacher-module/wrapper-overlapping-media';
import useVersionedTranslations  from '../../../hooks/useVersionedTranslations';

const attachmentTopic = 'quiz.attachments';

/*
*   The component for the quiz header , this contain the structure to the form for create the quiz
*/
const QuizHeader = ({
    published = false, 
    Title, 
    Instruction, 
    CustomValue, 
    QuizRubricModel, 
    QuizTypeId, 
    FilesCount, 
    addQuizFile,
    onQuizChange
}) => {
    const [TitleState, setTitleState] = useState(Title || '');
    const [InstructionState, setInstructionState] = useState(Instruction || '');
    const [QuizRubricModelState, setQuizRubricModelState] = useState({
        Desc: LanguageManager.getLanguage() === "es" ? Settings.getRubricTemplateEs() : Settings.getRubricTemplateEn()
    });
    const [CustomValueState, setCustomValueState] = useState(CustomValue || '');
    const [modal, setModal] = useState(false);
    const [quizTypeModelList, setQuizTypeModelList] = useState([]);
    const [callbackMessage, setCallbackMessage] = useState('');
    const [callbackMessageType, setCallbackMessageType] = useState('warning');
    const [transUploadFile, setTransUploadFile] = useState(LanguageManager.getTranslation('quiz.upload-file', Settings.LANGUAGES));

    const instructionRef = useRef();
    const instructionEditorIdRef = useRef(null);
    const rubricInitRef = useRef(false);

    useVersionedTranslations();

    useEffect(() => {
        getQuizTypes().then(res => setQuizTypeModelList(res.data.QuizTypeModelList));

        instructionEditorIdRef.current = Subject.subscribe({
        topic: attachmentTopic,
        callback: ({type, url}) => instructionRef.current?.insertMedia(type, url)
        });

        return () => {
            Subject.unsubscribe({
                topic: attachmentTopic,
                subscriberId: instructionEditorIdRef.current
            });
        };
    }, []);

    useEffect(() => {
        setTitleState(Title || '');
        setCustomValueState(CustomValue || '');
        setInstructionState(Instruction || '');
    }, [Title, CustomValue, Instruction]);

    useEffect(() => {
    const desc = QuizRubricModel?.Desc ?? '';
    if (!rubricInitRef.current) {
        rubricInitRef.current = true;
        setQuizRubricModelState({
        Desc: desc !== ''
            ? desc
            : (LanguageManager.getLanguage() === "es"
                ? Settings.getRubricTemplateEs()
                : Settings.getRubricTemplateEn()),
        });
        return;
    }
    // only change if desc is not empty and different from current state
    if (desc !== '' && desc !== QuizRubricModelState.Desc) {
        setQuizRubricModelState({ Desc: desc });
    }

    }, [QuizRubricModel?.Desc]);

    const onRubricChangeHandler = (rubric) => {
        onQuizChange({ Rubric: rubric });
        setQuizRubricModelState({ Desc: rubric });
    };

    const handleCallbackMessage = (message, type) => {
        setCallbackMessage(message);
        setCallbackMessageType(type);
    };

    const toggleModal = () => {
        setModal(prevState => !prevState);
    };

    // Handles the focus event for the instruction editor.
    // Sets the active subscriber for the given topic with the instruction editor's ID.
    const handleInstructionFocus = useCallback(() => {
        Subject.setActiveSubscriber({
            topic: attachmentTopic,
            subscriberId: instructionEditorIdRef.current
        });
    }, []);

    const placeholderTitle = LanguageManager.getTranslation("quiz.title", Settings.LANGUAGES);
    const placeholderInstructions = LanguageManager.getTranslation("quiz.description", Settings.LANGUAGES);
    const placeholderValue = LanguageManager.getTranslation("quiz.quiz-value", Settings.LANGUAGES);
    const placeholderRubric = LanguageManager.getTranslation("quiz.rubric", Settings.LANGUAGES);
    return (
        <Card className="m-2" style={{borderTop: '12px solid orange'}}>
            <CardBody>
                <FormGroup>
                    <Row className="quiz-creator__title">
                        <Col xs="10" md="11" className="pl-0 pr-0">
                            <Translate>
                                {({ translate }) =>  <Input
                                    type="text"
                                    placeholder={placeholderTitle}
                                    onChange={e => onQuizChange({ Title: e.currentTarget.value })}
                                    value={TitleState}
                                    style={{ width: '100%' }}
                                />}
                            </Translate>
                        </Col>
                        <Col xs="2" md="1" className="pl-0 pr-0">
                            <Button color="info" id="upload" onClick={toggleModal}>
                                <FontAwesomeIcon icon={faUpload} />
                                {
                                    FilesCount > 0
                                        ?<Badge className="position-absolute" color="danger">{FilesCount}</Badge>
                                        : ''
                                }
                                <UncontrolledTooltip placement="right" target="upload" >
                                    {transUploadFile}
                                </UncontrolledTooltip>
                            </Button>
                        </Col>
                    </Row>
                </FormGroup>
                <FormGroup>
                    <WrapperOverlappingMedia>
                        <Editor
                            key={LanguageManager.getLanguage()}
                            ref={instructionRef}
                            defaultValue={InstructionState}
                            onChange={(value) => onQuizChange({ Instruction: value })}
                            placeholder={placeholderInstructions}
                            onFocus={() => handleInstructionFocus()}
                        />
                    </WrapperOverlappingMedia>
                </FormGroup>
                <Row>
                    <Col xs="6">
                        <Dropdowns
                            disabled={published}
                            optionObject ={quizTypeModelList}
                            onValueChange={(value) => onQuizChange({ QuizTypeId: value })}
                            selectedValue={QuizTypeId}
                        />
                    </Col>
                    <Col xs="6">
                        <Translate>
                            {({ translate }) =>  <Input
                                type="number"
                                min={0}
                                max={100}
                                placeholder={placeholderValue}
                                onChange={e => onQuizChange({ CustomValue: e.currentTarget.value })}
                                value={CustomValueState}
                                style={{width: '100%'}}
                            />}
                        </Translate>
                    </Col>
                </Row>
                {QuizTypeId === "3" || QuizTypeId === 3?
                <FormGroup>
                    <WrapperOverlappingMedia>
                        <Editor
                            key={LanguageManager.getLanguage()}
                            defaultValue={QuizRubricModelState.Desc}
                            onChange={onRubricChangeHandler}
                            disabled={published}
                            placeholder={placeholderRubric}
                            toolbar={{
                                showToolBar: false
                            }}
                        />
                    </WrapperOverlappingMedia>
                </FormGroup>: null}
                <Modal isOpen={modal} toggle={toggleModal}>
                    <ModalHeader toggle={toggleModal}>
                        <Translate id="quiz.upload-file" />
                    </ModalHeader>
                    <ModalBody>
                        <MyUploader handleCallbackMessage={handleCallbackMessage} category="file" addQuizFile={addQuizFile}/>
                        {
                            callbackMessage !== ''
                                ? <UncontrolledAlert className="mt-3" color={callbackMessageType} fade={true}>
                                    {callbackMessage}
                                </UncontrolledAlert>
                                : ''
                        }
                    </ModalBody>
                    <ModalFooter>
                        <Button color="primary" onClick={toggleModal}><Translate id="quiz.cancel-btn" /></Button>
                    </ModalFooter>
                </Modal>
            </CardBody>
        </Card>
    )
};

export default QuizHeader