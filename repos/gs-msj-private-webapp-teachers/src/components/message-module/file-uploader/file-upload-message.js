/**
 * Created by portiz on 05/19/20.
 */
import React, { useState, useEffect } from 'react';
import classNames from 'classnames';
import Dropzone from 'react-dropzone';
import { Translate } from "react-localize-redux";
import Settings from '../../../custom-classes/settings';
import LanguageManager from '../../../custom-classes/language-manager';
import ModalGeneric from '../../shared-module-components/modal/modal-generic';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFileUpload, faPaperclip } from '@fortawesome/free-solid-svg-icons';
import { getMessageFileRules } from "../../../api/messages";
import SessionManager from "../../../custom-classes/session-manager";

const FileUploadMessage = ({updateParent}) => {
    const [error, setError] = useState(false);
    const [message, setMessage] = useState('');
    const [propsModal, setPropsModal] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [fileDetails, setFileDetails] = useState([]);
    const [supportedFilesString, setSupportedFilesString] = useState('');
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        // Call accepted files only.
        let apiCall = getMessageFileRules();
        apiCall.then((response) => {
            setFileDetails(response.data.AttachmentRuleModelList);
            
            let strFileSupportedFiles = '';
            for (let i = 0; i < response.data.AttachmentRuleModelList.length; i++) {
                if (i !== 0) {
                    strFileSupportedFiles += ', '
                }
                strFileSupportedFiles += response.data.AttachmentRuleModelList[i].FileType + ' (Max: ' + bytesToSize(response.data.AttachmentRuleModelList[i].MaxSize) + ') ';
            }
            setSupportedFilesString(strFileSupportedFiles);
        });
    }, []);

    const bytesToSize = (bytes) => {
        let sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        if (bytes == 0) return '0 Byte';
        let i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
    };

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    const onDrop = (acceptedFiles, rejectedFiles) => {
        // Only one file at the time
        if (acceptedFiles.length > 1) {
            setMessage(<Translate id="drop-zone.one-file-at-time" options={{ renderInnerHtml: true }} />);
            setError(true);
            return;
        }

        let valid = false;
        for (let i = 0; i < fileDetails.length; i++) {
            if (acceptedFiles[0].name.endsWith(fileDetails[i].FileType)) {
                valid = true;
                break;
            }
        }

        if (!valid) {
            setMessage(
                <span>
                    <Translate id="messages.file-not-supported" options={{ renderInnerHtml: true }} /><br />
                    {supportedFilesString}</span>
            );
            setError(true);
            return;
        }

        // Clean
        setMessage('');
        setError(false);

        if (!error) {
            setLoading(true);

            // for regular users
            let url = Settings.getMessageFilesUploadAPI(LanguageManager.getLanguage());

            // Attach files
            const form = new FormData();
            for (let i = 0; i < acceptedFiles.length; i++) {
                let file = acceptedFiles[i];
                form.append('files[]', file);
            }
            fetch(url, {
                method: 'POST',
                body: form,
                headers: {
                    'Accept': 'application/json',
                    'Token': SessionManager.getSession()
                }
            }).then(response => {
                setLoading(false);
                return response.json();
            }).then((data) => {
                if (!data.TransactionValid) {
                    let messageStr = '';
                    for (let i = 0; i < data.Messages.length; i++) {
                        messageStr += data.Messages[i] + '<br/>';
                    }

                    let propsModalData = {
                        bodyText: messageStr,
                        visible: true
                    };
                    setPropsModal(propsModalData);
                    setShowModal(true);
                    setLoading(false);
                }
                else {
                    setLoading(false);
                    updateParent(data.Messages[0], data.Messages[1], acceptedFiles[0].name);
                }
            }).catch(() => {
                setLoading(false);
            });
        }
    };

    return (
        <div>
            <Dropzone onDrop={onDrop}>
                {({ getRootProps, getInputProps, isDragActive }) => {
                    return (
                        <div
                            {...getRootProps()}
                            className={classNames('dropzone', { 'dropzone--isActive': isDragActive })}
                        >
                            <input {...getInputProps()} />
                            {
                                <p className="clear-p">
                                    <FontAwesomeIcon className="gray-bright-text icon-attach" icon={faPaperclip} size='4x' />{" "}<Translate id="drop-zone.attachFile" options={{ renderInnerHtml: true }} />
                                </p>
                            }
                            {
                                <br />
                            }
                            {
                                isDragActive ?
                                    <p><Translate id="drop-zone.drop-file-here" options={{ renderInnerHtml: true }} /></p> :
                                    <p><Translate id="messages.upload-instructions" options={{ renderInnerHtml: true }} />{' '} <br /><b>{supportedFilesString}</b></p>
                            }
                            {
                                error && message !== null && message !== '' ?
                                    <p className="red-text"> {message} </p> : ''
                            }
                            {
                                !error && message !== null && message !== '' ?
                                    <p className="green-text"> {message} </p> : ''
                            }
                        </div>
                    )
                }}
            </Dropzone>
            {loading ? <LoaderFullWindow /> : null}
            {showModal ?
                <ModalGeneric data={propsModal} updateParent={toggleAlertModal} /> : null}
        </div>

    );
};

export default FileUploadMessage;