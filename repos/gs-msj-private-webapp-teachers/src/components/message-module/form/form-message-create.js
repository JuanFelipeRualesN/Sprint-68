import React, { useState, useEffect } from "react";
import { Button, Col, Row, FormGroup, Input, Label } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { sendMessage, sendMessageByRole } from "../../../api/messages";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheckCircle, faPaperPlane, faTimes } from "@fortawesome/free-solid-svg-icons";
import SelectorMultiGroups from "../../teacher-module/selector/selector-multi-groups";
import FileUploadMessage from "../file-uploader/file-upload-message";
import { BeatLoader } from "react-spinners";
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import SelectorMultiUser from "../selector/selector-multi-user";
import SelectorRoles from "../../shared-module-components/selector/selector-roles";
import { getGroupDetails } from "../../../api/teachers";
import { removeDuplicates, isRoleAllowed, convertMaxLengthSting } from "../../../utils/utils";

/**
 * Component to create a new message.
 */
const FormMessageCreate = ({ isReply = false, parentId = null, recipients = [], subject: initialSubject = '' }) => {
    const sessionDetails = SessionManager.getSessionDetails();
    const roles = sessionDetails?.Roles || [];

    const [attachmentsURL, setAttachmentsURL] = useState([]);
    const [attachmentsURI, setAttachmentsURI] = useState([]);
    const [attachmentsName, setAttachmentsName] = useState([]);
    const [attachmentsHREF, setAttachmentsHREF] = useState([]);
    const [disabledSend, setDisabledSend] = useState("disabled");
    const [bodyText, setBodyText] = useState("");
    const [subject, setSubject] = useState(initialSubject);
    const [loading, setLoading] = useState(false);
    const [messageReturnedAPI, setMessageReturnedAPI] = useState("");
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [selectedRoleId, setSelectedRoleId] = useState(null);
    const [groups, setGroups] = useState([]);
    const [groupsMap, setGroupsMap] = useState(new Map());
    const [recipientsGroup, setRecipientsGroup] = useState([]);
    const [recipientIds, setRecipientIds] = useState(recipients);

    useEffect(() => {
        evaluateIfAvailableForSend();
    }, [subject, bodyText, recipientsGroup, recipientIds, selectedRoleId]);

    const evaluateIfAvailableForSend = () => {
        const canSend =
            (recipientsGroup.length > 0 || recipientIds.length > 0 || selectedRoleId !== null) &&
            bodyText.length > 0 &&
            subject.length > 0;
        setDisabledSend(canSend ? "" : "disabled");
    };

    const handleSubjectChange = (e) => {
        const value = convertMaxLengthSting(e.target.value, 100);
        setSubject(value);
    };

    const handleBodyTextMessage = (e) => {
        const value = convertMaxLengthSting(e.target.value, 500);
        setBodyText(value);
    };

    const setSelectedStudents = (users) => {
        setRecipientIds(users.map((u) => u.value));
    };

    const setSelectedRolesHandler = (roleId) => {
        setSelectedRoleId(roleId);
    };

    const setUploadedFile = (fileUrl, path, fileName) => {
        setAttachmentsURL((prev) => [...prev, fileUrl]);
        setAttachmentsURI((prev) => [...prev, path]);
        setAttachmentsName((prev) => [...prev, fileName]);
        setAttachmentsHREF((prev) => [...prev, <a key={fileName} href={fileUrl}>{fileName}</a>]);
    };

    const removeItem = (index) => {
        setAttachmentsURL((prev) => prev.filter((_, i) => i !== index));
        setAttachmentsURI((prev) => prev.filter((_, i) => i !== index));
        setAttachmentsName((prev) => prev.filter((_, i) => i !== index));
        setAttachmentsHREF((prev) => prev.filter((_, i) => i !== index));
    };

    const setSelectedGroups = async (selecteds) => {
        setGroups(selecteds);
        const newMap = new Map(groupsMap);
        let arrayValues = [];

        if (selecteds.length > 0) {
            const lastSelected = selecteds[selecteds.length - 1];
            if (!newMap.has(lastSelected.value)) {
                const members = await getMembersByGroup(lastSelected.value);
                newMap.set(lastSelected.value, members);
            }

            newMap.forEach((value) => {
                arrayValues = arrayValues.concat(value);
            });

            setGroupsMap(newMap);
            setRecipientsGroup(arrayValues);
        } else {
            setGroupsMap(new Map());
            setRecipientsGroup([]);
        }
    };

    const getMembersByGroup = async (groupId) => {
        setLoading(true);
        try {
            const response = await getGroupDetails(groupId);
            return response.data?.StudentDetailModelList || [];
        } finally {
            setLoading(false);
        }
    };

    const sendMessageHandler = () => {
        setLoading(true);

        let apiCall = null;
        const normalizedRecipientIds = Array.isArray(recipientIds) ? recipientIds : [recipientIds];

        if (selectedRoleId == null) {
            const recipientIdsMerged = removeDuplicates([
                ...recipientsGroup.map((r) => r.UserId),
                ...normalizedRecipientIds
            ]);

            apiCall = sendMessage(recipientIdsMerged, subject, bodyText, attachmentsURI, attachmentsName, parentId);
        } else {
            apiCall = sendMessageByRole(selectedRoleId, subject, bodyText, attachmentsURI, attachmentsName, parentId);
        }

        apiCall.then((response) => {
            const message = response.data.Messages?.join("<br/>") || "";

            if (response.status === 200 || response.status === 201) {
                setShowSuccessMessage(true);
                setShowErrorMessage(false);
            } else {
                setShowSuccessMessage(false);
                setShowErrorMessage(true);
            }

            setMessageReturnedAPI(message);
            setLoading(false);
        });
    };

    if (showSuccessMessage) {
        return (
            <div className="animated fadeIn text-left">
                <h3>
                    <FontAwesomeIcon icon={faCheckCircle} style={{ color: "green" }} className="float-left margin-right-10" />
                    <div className="content" dangerouslySetInnerHTML={{ __html: messageReturnedAPI }} />
                </h3>
            </div>
        );
    }

    return (
        <div>
            {!isReply && (
                <>
                    <Row>
                        <Col xs={12}>
                            {isRoleAllowed([Settings.ROLE_TEACHER_STR], roles) && (
                                <FormGroup row>
                                    <Label sm={2} className="label-forms-blue"><Translate id="messages.send-by-group" /></Label>
                                    <Col sm={10}>
                                        <SelectorMultiGroups updateParent={setSelectedGroups} />
                                    </Col>
                                </FormGroup>
                            )}
                            {isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_CLIENT_STR, Settings.ROLE_PARENT_NO_CLIENT_STR], roles) && (
                                <FormGroup row>
                                    <Label sm={2} className="label-forms-blue"><Translate id="messages.send-by-student" /></Label>
                                    <Col sm={10}>
                                        <SelectorMultiUser updateParent={setSelectedStudents} />
                                    </Col>
                                </FormGroup>
                            )}
                            {isRoleAllowed(
                                [Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_DIRECTOR_STR, Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR],
                                roles
                            ) && (
                                <FormGroup row>
                                    <Label sm={2} className="label-forms-blue"><Translate id="messages.select-role" /></Label>
                                    <Col sm={10}>
                                        <SelectorRoles roleId={selectedRoleId} updateParent={setSelectedRolesHandler} />
                                    </Col>
                                </FormGroup>
                            )}
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            <FormGroup row>
                                <Label sm={2} className="label-forms-blue"><Translate id="messages.subject" /></Label>
                                <Col sm={10}>
                                    <Input type="text" className="mantine-like__control" value={subject} onChange={handleSubjectChange} />
                                </Col>
                            </FormGroup>
                        </Col>
                    </Row>
                </>
            )}
            <Row>
                <Col xs={12}>
                    <FormGroup row>
                        <Label sm={2} className="label-forms-blue"><Translate id="messages.message" /></Label>
                        <Col sm={10}>
                            <Input type="textarea" className="mantine-like__control" rows="10" value={bodyText} onChange={handleBodyTextMessage} />
                        </Col>
                    </FormGroup>
                    <FormGroup row>
                        <Label sm={2} className="label-forms-blue"><Translate id="messages.attach-file" /></Label>
                        <Col sm={10}>
                            {attachmentsHREF.map((file, i) => (
                                <div key={i} className="files-attached-list">
                                    <FontAwesomeIcon onClick={() => removeItem(i)} icon={faTimes} /> {file}
                                </div>
                            ))}
                            <FileUploadMessage updateParent={setUploadedFile} />
                        </Col>
                    </FormGroup>
                </Col>
            </Row>
            <Row>
                {showErrorMessage && (
                    <Col xs={12} md={8} lg={8}>
                        <div className="content" dangerouslySetInnerHTML={{ __html: messageReturnedAPI }} />
                    </Col>
                )}
                <Col className="text-right">
                    <button
                        disabled={disabledSend === "disabled" || loading}
                        className="button-secondary-custom btn-default"
                        onClick={sendMessageHandler}
                    >
                        {loading ? <BeatLoader size={10} color="#fff" /> : <FontAwesomeIcon icon={faPaperPlane} />}{" "}
                        <Translate id="messages.send" />
                    </button>
                </Col>
            </Row>
        </div>
    );
};

export default FormMessageCreate;
