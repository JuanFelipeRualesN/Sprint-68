/*
*   Create By: Mario Cancel
*/
import React, { useRef, useState, useEffect } from 'react';
import 'react-dropzone-uploader/dist/styles.css'
import Dropzone from 'react-dropzone'
import Settings from '../../../custom-classes/settings';
import { getQuizFileRules } from '../../../api/teachers';
import Alert from 'react-s-alert';
import { withLocalize } from 'react-localize-redux';
import LanguageManager from '../../../custom-classes/language-manager';
import SessionManager from "../../../custom-classes/session-manager";

function MyUploader({
    handleCallbackMessage,
    category = 'file',
    onChange,
    addQuizFile = () => { }
}) {

    const langKey = LanguageManager.getLanguage();

    const [filesRules, setFilesRules] = useState([]);
    const [fileFormatStr, setFileFormatStr] = useState('');
    const [acceptFiles, setAcceptFiles] = useState('.png, .jpeg, .jpg, .pdf');
    const [formFile, setFormFile] = useState(null);
    const [fileMimeType, setFileMimeType] = useState('');

    const uploadRef = useRef(false);

    const transAllowedFiles = () => LanguageManager.getTranslation('quiz.allowed-files', Settings.LANGUAGES)


    useEffect(() => {
        const fetchData = async () => {
            const rules = await getQuizFileRules();
            let filesRulesList = rules.data.SupportedFileModelList;
            setFilesRules(filesRulesList);
        };
        fetchData();
    }, []);
    
    useEffect(() => {
        if (filesRules.length > 0) {
            handleChooseTypeFiles();
            callQuizRules(category);
        }
    }, [filesRules, category]);

    const bytesToSize = (bytes) => {
        let sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        if (bytes == 0) return '0 Byte';
        let i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
    };

    const callQuizRules = async (category) => {
        let formatTrans = LanguageManager.getTranslation('quiz.format', Settings.LANGUAGES);
        let sizeTrans = LanguageManager.getTranslation('quiz.size', Settings.LANGUAGES);

        let filesRulesList = filesRules;
        let formatText = "<table class='table'><tr><td>" + formatTrans + "</td><td>" + sizeTrans + "</td></tr>";
        for (let i = 0; i < filesRulesList.length; i++) {
            if (category === filesRulesList[i].Classification) {
                formatText += "<td>" + filesRulesList[i].Format + "</td><td>" + bytesToSize(filesRulesList[i].MaxSize) + "</td></tr>";
            }
        }
        setFileFormatStr(formatText);
    };

    //the method get the file extension accepted
    const handleChooseTypeFiles = () => {
        let acceptFilesList;
        switch (category) {

            case 'image':
                let image = filesRules.filter(m => m.Classification === category);
                acceptFilesList = image.map(accept => (
                    '.' + accept.Format
                )).join(', ').toString()
                setAcceptFiles(acceptFilesList);
                break;

            case 'video':
                let video = filesRules.filter(m => m.Classification === category);
                acceptFilesList = video.map(accept => (
                    '.' + accept.Format
                )).join(', ').toString()
                setAcceptFiles(acceptFilesList);
                break;

            case 'audio':
                let audio = filesRules.filter(m => m.Classification === category);
                acceptFilesList = audio.map(accept => (
                    '.' + accept.Format
                )).join(', ').toString()
                setAcceptFiles(acceptFilesList);
                break;

            case 'file':
                let file = filesRules.filter(m => m.Classification === category);
                acceptFilesList = file.map(accept => (
                    '.' + accept.Format
                )).join(', ').toString();
                setAcceptFiles(acceptFilesList);
                break;

            default:

                Alert.warning(LanguageManager.getTranslation("quiz.warning-upload-file", Settings.LANGUAGES), {
                    position: 'top',
                    effect: 'slide',
                    timeout: 5000
                });

                break;

        }
    }

    //method for validad falice size
    const onValidationFileSize = file => {

        // This has an error due to files names. We will be changing it. POR 2020.06.15
        // let fileExtension = file.path.split(".")[1]; //get de extesion file

        // Best fit for this by POR 2020.06.15
        let indexNoFilePath = file.path.lastIndexOf(".");
        let fileExtension = file.path.substring(indexNoFilePath + 1, file.path.length);

        let getRulesExtension = filesRules.filter(m => m.Format.toLowerCase() === fileExtension.toLowerCase());

        if (getRulesExtension.length) {
            if (file.size <= getRulesExtension[0].MaxSize) {
                handleCallbackMessage(LanguageManager.getTranslation('quiz.dropzone-correct-file-size', Settings.LANGUAGES), 'success');
                setFileMimeType(fileExtension);
                return true;

            } else {
                handleCallbackMessage(LanguageManager.getTranslation('quiz.dropzone-incorrect-file-size', Settings.LANGUAGES), 'warning');
                return false;

            }
        } else {
            handleCallbackMessage(LanguageManager.getTranslation('quiz.dropzone-file-not-allowed', Settings.LANGUAGES), 'danger');
            return false;

        }
    };

    // This method is uset for uploded
    const handleSetFile = async (form) => {
        if (uploadRef.current) return; // Prevent multiple uploads
        uploadRef.current = true;
        let response;
        let response1;
        let hasError = false;
        const fetchBody = {
            "method": "POST",
            "body": form,
            "headers": {
                "Token": SessionManager.getSession(),
            }
        };
        await fetch(Settings.getQuizFileUploadAPI(), fetchBody).then((response) => {
            if (response.status !== 200 && response.status !== 201) {
                hasError = true;
            }
            return response.json();
        })
        .then((responseData) => {
            response = responseData.Messages[0]
            response1 = responseData.Messages

        })
        .catch(err => {
            hasError = true;
            Alert.error(err.join('/n'), {
                position: 'top',
                effect: 'slide',
                timeout: 5000
            });
        })
        .finally(() => {
            uploadRef.current = false;
        });
        if (!hasError) {
            handleCallbackMessage(LanguageManager.getTranslation('quiz.dropzone-done', Settings.LANGUAGES), 'success');
            handleSubmit(response1);
        }
        else {
            handleCallbackMessage(LanguageManager.getTranslation('quiz.error-upload-file-generic', Settings.LANGUAGES), 'danger');
        }
    };

    //method for uploaded file
    const onDrop = (acceptedFilesType, rejectedFiles) => {
        handleCallbackMessage('', 'warning');
        let isValidFile = true;
        let form = new FormData();

        for (let i = 0; i < acceptedFilesType.length; i++) {
            let file = acceptedFilesType[i];
            if (onValidationFileSize(file))
                form.append('files[]', file);
            else
                isValidFile = false
        }

        if (isValidFile) {
            setFormFile(form);
            handleSetFile(form);
        }
    };

    //This is used to create the html tags to be inserted into the premise.
    const handleSubmit = (data) => {
        switch (category) {
            case 'file':
                addQuizFile((prev) => ({
                    ...prev,
                    FilesCount: prev.FilesCount + 1,
                    QuizAttachmentModelList: [...prev.QuizAttachmentModelList, { AttachmentUrl: data[1] }]
                }));
                break;
            case 'image':
            case 'video':
            case 'audio':
                onChange({ type: category, url: data[0] });
                break;
            default:
                handleCallbackMessage(LanguageManager.getTranslation('quiz.warning-upload-file', Settings.LANGUAGES), 'warning');
                break;
        }
    };

    return (
        <div>
            <div className="text-center" style={{ outline: 'none', border: '3px dashed lightgrey' }}>
                <Dropzone
                    accept={acceptFiles}
                    onDrop={onDrop}
                    timeout={180000}
                >
                    {({ getRootProps, getInputProps, isDragActive, isDragReject }) => (
                        <div {...getRootProps()} style={{ height: '100px', textAlign: 'center', padding: '40px 0' }}>
                            <input {...getInputProps()} />
                            <div style={{ margin: 'auto', width: '70%' }}>
                                {!isDragActive && LanguageManager.getTranslation("quiz.dropzone-uploaded-file", Settings.LANGUAGES)}
                                {isDragActive && !isDragReject && LanguageManager.getTranslation("quiz.dropzone-drop-file", Settings.LANGUAGES)}
                                {/* {isDragReject &&  LanguageManager.getTranslation("quiz.dropzone-not-accepted",Settings.LANGUAGES)} */}
                                {

                                    /** Modified by FG 2023.08.23 

                                    This was modified by using the drop-file translation, because the isDragReject is not working in Dropzone component. 

                                    The component was showing the same message when the drag is active. Please, do not change unless the Dropzone 

                                        component members fix this error. 

                                        **/

                                    isDragReject && LanguageManager.getTranslation("quiz.dropzone-drop-file", Settings.LANGUAGES)
                                }
                            </div>
                        </div>
                    )}
                </Dropzone>
            </div>
            {transAllowedFiles}
            <div dangerouslySetInnerHTML={{ __html: fileFormatStr }} />
        </div>
    );
}


export default withLocalize(MyUploader);