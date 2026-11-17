/**
 * Created by portiz on 2/6/19.
 */
import React, { useState } from "react";
import classNames from "classnames";
import Dropzone from "react-dropzone";
import { Translate } from "react-localize-redux";
import Settings from "../../../custom-classes/settings";
import LanguageManager from "../../../custom-classes/language-manager";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFileUpload } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import ImgUsersUpload from "../../../assets/images/common-icons/Admin-icon-UsersUpload.png";

const FileUploader = ({ roleId, schoolId, session, subscriptionId, updateParent }) => {
    const [error, setError] = useState(false);
    const [message, setMessage] = useState("");
    const [propsModal, setPropsModal] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [loading, setLoading] = useState(false);

    // Toogles alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
        updateParent("OK");
    };

    const onDrop = (acceptedFiles, rejectedFiles) => {
        // Only one file at the time
        if (acceptedFiles.length > 1) {
            setMessage(
                <Translate
                    id="drop-zone.one-file-at-time"
                    options={{ renderInnerHtml: true }}
                />
            );
            setError(true);
            return;
        }

        // The CSV files are the only one accepted
        if (!acceptedFiles[0].name.endsWith(".csv")) {
            setMessage(
                <Translate
                    id="drop-zone.only-csv-allowed"
                    options={{ renderInnerHtml: true }}
                />
            );
            setError(true);
            return;
        }

        // Clean
        setMessage("");
        setError(false);

        if (subscriptionId !== null && !error) {
            setLoading(true);

            // for regular users
            let url = Settings.getUploadUserSchoolsBulk();

            const form = new FormData();

            for (let i = 0; i < acceptedFiles.length; i++) {
                let file = acceptedFiles[i];
                form.append("files[]", file);
            }

            form.append("Token", session);
            form.append("LangCode", LanguageManager.getLanguage());
            form.append("SubscriptionId", subscriptionId);
            form.append("RoleId", roleId);

            fetch(url, {
                method: "POST",
                body: form,
            })
                .then((response) => {
                    setLoading(false);
                    return response.json();
                })
                .then((responseData) => {
                    let data = responseData;
                    let messageStr = "";

                    let isMessageEmpty = true;

                    if (data.Messages) {
                        for (let i = 0; i < data.Messages.length; i++) {
                            if(data.Messages[i] != null && data.Messages[i].trim().length > 0) {
                                isMessageEmpty = false;
                            }
                            messageStr += data.Messages[i] + "<br/>";
                        }
                    } else if (data.UserErrors) {
                        for (let i = 0; i < data.UserErrors.length; i++) {
                            messageStr +=
                                "<br><b>" + data.UserErrors[i].Username + "</b>: ";
                            for (let j = 0; j < data.UserErrors[i].ErrorDetails.length; j++) {
                                messageStr += data.UserErrors[i].ErrorDetails[j];
                                messageStr += "<br/>";
                            }
                        }
                    }

                    if(isMessageEmpty) {
                        messageStr =
                            LanguageManager.getTranslation("drop-zone.default-message-error", Settings.LANGUAGES) + "<br/>";
                    }

                    let propsModalObj = {
                        bodyText: messageStr,
                        visible: true,
                        showModal: true,
                    };
                    setPropsModal(propsModalObj);
                    setShowModal(true);
                    setLoading(false);
                })
                .catch(() => {
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
                            className={classNames("dropzone", {
                                "dropzone--isActive": isDragActive,
                            })}
                        >
                            <input {...getInputProps()} />
                            {
                                <p>
                                    <FontAwesomeIcon
                                        className="gray-bright-text"
                                        icon={faFileUpload}
                                        size="4x"
                                    />
                                </p>
                            }
                            {<br />}
                            {isDragActive ? (
                                <p>
                                    <Translate
                                        id="drop-zone.drop-file-here"
                                        options={{ renderInnerHtml: true }}
                                    />
                                </p>
                            ) : (
                                <p>
                                    <Translate
                                        id="drop-zone.instructions"
                                        options={{ renderInnerHtml: true }}
                                    />{" "}
                                    <img
                                        src={ImgUsersUpload}
                                        style={{ width: "100%", maxWidth: 800 }}
                                    />
                                </p>
                            )}
                            {error &&
                            message !== null &&
                            message !== "" ? (
                                <h3 className="red-text"> {message} </h3>
                            ) : (
                                ""
                            )}
                            {!error &&
                            message !== null &&
                            message !== "" ? (
                                <h3 className="green-text"> {message} </h3>
                            ) : (
                                ""
                            )}
                        </div>
                    );
                }}
            </Dropzone>
            {loading ? <LoaderFullWindow /> : null}
            {showModal ? (
                <ModalGeneric
                    data={propsModal}
                    updateParent={toggleAlertModal}
                />
            ) : null}
        </div>
    );
};

export default FileUploader;
