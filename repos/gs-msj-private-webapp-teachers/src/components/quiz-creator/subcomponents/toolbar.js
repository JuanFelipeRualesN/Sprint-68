/*
*   Create By: Mario Cancel
*/
import React, { useState, useEffect } from 'react';
import { ButtonGroup, Button, Modal, ModalHeader, ModalBody, ModalFooter, UncontrolledAlert, UncontrolledTooltip,
    UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import {
    faPlusCircle,
    faFileImage,
    faPhotoVideo,
    faFileAudio,
    faSave,
    faVolumeUp,
    faVideo,
    faImage
} from '@fortawesome/free-solid-svg-icons'

import Alert from 'react-s-alert';

import { validateQuiz } from '../../../utils/quiz-validation';
import { connect } from 'react-redux';
import { saveQuiz, setQuizId } from '../../../redux/actions';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import MyUploader from '../common/dropzone-uploader';
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server';
import LanguageManager from '../../../custom-classes/language-manager';
import Settings from "../../../custom-classes/settings";
import { Subject } from "../../../custom-classes/subject";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const QUESTION_TYPE_OPTIONS = [
    { typeId: '1', labelKey: 'quiz.selection' },
    { typeId: '2', labelKey: 'quiz.match' },
    { typeId: '3', labelKey: 'quiz.open' },
    { typeId: '4', labelKey: 'quiz.true-false' },
];

const ToolBar = ({ published = false, QuizRequestModel, languages, addPremise, saveQuiz, setQuizChanges, onSuccessfulSave, showMessage }) => {
    const [transButtonAddPremise] = useState(LanguageManager.getTranslation('quiz.button-add-premise', Settings.LANGUAGES));
    const [transButtonAddAudio] = useState(LanguageManager.getTranslation('quiz.button-add-audio', Settings.LANGUAGES));
    const [transButtonAddImage] = useState(LanguageManager.getTranslation('quiz.button-add-image', Settings.LANGUAGES));
    const [transButtonAddVideo] = useState(LanguageManager.getTranslation('quiz.button-add-video', Settings.LANGUAGES));
    const [transButtonSave] = useState(LanguageManager.getTranslation('quiz.button-save', Settings.LANGUAGES));
    const [transUploadFile] = useState(LanguageManager.getTranslation('quiz.upload-file', Settings.LANGUAGES));
    const [transButtonCancel] = useState(LanguageManager.getTranslation('quiz.cancel-btn', Settings.LANGUAGES));
    const [vertical, setVertical] = useState(window.innerWidth >= 768);
    const [modal, setModal] = useState(false);
    const [category, setCategory] = useState('image');
    const [callbackMessage, setCallbackMessage] = useState('');
    const [callbackMessageType, setCallbackMessageType] = useState('warning');
    const [disabled, setDisabled] = useState(true);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        const handleResize = () => setVertical(window.innerWidth >= 768);

        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);

    useEffect(() => {
        setDisabled(QuizRequestModel.QuestionRequestModelList.length === 0);
    }, [QuizRequestModel]);

    const handleCallbackMessage = (message, type) => {
        setCallbackMessage(message);
        setCallbackMessageType(type);
    };

    const toggleModalWithCategory = (category) => {
        setModal(prevState => !prevState);
        setCategory(category);
        setCallbackMessage('');
    };
    
    const toggleModal = () => {
        setModal(prevState => !prevState);
        setCallbackMessage('');
    };

    const closeModal = () => {
        setModal(false);
        setCallbackMessage('');
    };

    const addPremiseHandler = (typeId) => {
        addPremise(typeId);
    };

    const saveQuizHandler = async () => {
        const { valid, messages } = validateQuiz(QuizRequestModel, languages);
        if (!valid) {
            const alerts = messages.map((message, index) => (<li key={index}>{message}</li>));
            Alert.error(
                <div>
                    <b>{messages.length} {LanguageManager.getTranslation('quiz.warnings', languages)}:</b>
                    <ul>{alerts}</ul>
                </div>,
                {
                    position: 'top-right',
                    effect: 'slide',
                    timeout: 10000
                });
        } else {
            try {
                setDisabled(true);
                await saveQuiz(QuizRequestModel).then(quizInfo => {
                    showMessage(quizInfo.Messages);
                    if (quizInfo.Id){
                        // If the quiz was just created, we need to set the quizId in the store
                        // to be able to publish it later. The questions come along so the draft
                        // keeps the ids the server just assigned; without them a second save
                        // posts them as new and the update deletes the originals.
                        setQuizChanges(quizInfo.Id, quizInfo.QuizAttachmentModelList, quizInfo.QuestionRequestModelList);
                    }
                    if (onSuccessfulSave) {
                        onSuccessfulSave();
                    }
                    setDisabled(false);
                });
            } catch (error) {
                setDisabled(false);
                Alert.error("Failed to save the quiz. Please try again.");
            }
        }
    };

    const handleUploaderChange = (changes) => {
        Subject.publish({
            topic: 'quiz.attachments',
            data: changes,
            pushToActive: true
        });
    };


    return (
        <div>
            <ButtonGroup vertical={vertical} className={vertical ? 'position-fixed' : 'fixed-bottom'} style={vertical ? { marginTop: '45px', top: 205, right: 3 } : { marginTop: 45, bottom: 90 }}>
                {!published
                    ? <UncontrolledDropdown className="btn-group" direction={vertical ? 'left' : 'up'}>
                        <DropdownToggle id="assignment-plus-btn" className={vertical ? 'btn btn-normal' : 'btn btn-normal rounded-0'}>
                            <FontAwesomeIcon icon={faPlusCircle} />
                        </DropdownToggle>
                        <UncontrolledTooltip placement="right" target="assignment-plus-btn" >
                            {transButtonAddPremise}
                        </UncontrolledTooltip>
                        <DropdownMenu>
                            {QUESTION_TYPE_OPTIONS.map(option => (
                                <DropdownItem
                                    key={option.typeId}
                                    onClick={() => addPremiseHandler(option.typeId)}
                                >
                                    {LanguageManager.getTranslation(option.labelKey, Settings.LANGUAGES)}
                                </DropdownItem>
                            ))}
                        </DropdownMenu>
                    </UncontrolledDropdown>
                    : ''
                }
                <button id="assignment-audio-btn" className={vertical ? 'btn btn-normal' : 'btn btn-normal rounded-0'} onClick={() => toggleModalWithCategory('audio')}>
                    <FontAwesomeIcon icon={faVolumeUp} />
                    <UncontrolledTooltip trigger="hover" placement="right" target="assignment-audio-btn" >
                        {transButtonAddAudio}
                    </UncontrolledTooltip>
                </button>
                <button id="assignment-image-btn" className={vertical ? 'btn btn-normal' : 'btn btn-normal rounded-0'} onClick={() => toggleModalWithCategory('image')} >
                    <FontAwesomeIcon icon={faImage} />
                    <UncontrolledTooltip trigger="hover" placement="right" target="assignment-image-btn" >
                        {transButtonAddImage}
                    </UncontrolledTooltip>
                </button>
                <button id="assignment-video-btn" className={vertical ? 'btn btn-normal' : 'btn btn-normal rounded-0'} onClick={() => toggleModalWithCategory('video')} >
                    <FontAwesomeIcon icon={faVideo} />
                    <UncontrolledTooltip trigger="hover" placement="right" target="assignment-video-btn" >
                        {transButtonAddVideo}
                    </UncontrolledTooltip>
                </button>
                <button disabled={disabled} id="assignment-save-btn" className={vertical ? 'btn' : 'btn rounded-0'} onClick={saveQuizHandler}>
                    <FontAwesomeIcon icon={faSave} />
                    <UncontrolledTooltip placement="right" target="assignment-save-btn" >
                        {transButtonSave}
                    </UncontrolledTooltip>
                </button>
            </ButtonGroup>
            <Modal isOpen={modal} toggle={toggleModal}>
                <ModalHeader toggle={toggleModal}>
                    {transUploadFile}
                </ModalHeader>
                <ModalBody>
                    <MyUploader
                        handleCallbackMessage={handleCallbackMessage}
                        category={category}
                        onChange={handleUploaderChange} />
                    {
                        callbackMessage !== ''
                            ? <UncontrolledAlert className="mt-3" color={callbackMessageType} fade={true}>
                                {callbackMessage}
                            </UncontrolledAlert>
                            : ''
                    }
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={closeModal}>
                        {transButtonCancel}
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    )
};



export default withLocalize(ToolBar);