import React, { useContext, useEffect, useState, useCallback } from "react";
import {CardBody, CardTitle, Col, Row, Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faMailBulk,
    faPaperPlane,
    faRedo,
    faPlus
} from '@fortawesome/free-solid-svg-icons';
import SessionManager from "../../../custom-classes/session-manager";
import MessagesReceivedDataTable from "../datatables/messages-received-data-table";
import MessagesSentDataTable from "../datatables/messages-sent-data-table";
import { getTotalOfUnreadMessages } from "../../../api/messages";
import './style.css';
import Settings from "../../../custom-classes/settings";
import { isRoleAllowed } from "../../../utils/utils";
import { AppContext } from "../../../App.Context";
import FormMessageReceived from "../form/form-message-received";
import FormMessageCreate from "../form/form-message-create";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from "../../../utils/access_logger";

/**
 * The component (window) to handle the messages
 */
const Messages = ({ initialize, addTranslation }) => {
    const context = useContext(AppContext);
    const sessionDetails = SessionManager.getSessionDetails();
    const [selectedOption, setSelectedOption] = useState("0");
    const [isReceived, setIsReceived] = useState(true);
    const [redoSearch, setRedoSearch] = useState(false);
    const [messageId, setMessageId] = useState(null);
    const [showMessageDetailModal, setShowMessageDetailModal] = useState(false);
    const [showMessageCreateModal, setShowMessageCreateModal] = useState(false);
    const [totalUnreadMessages, setTotalUnreadMessages] = useState(0);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.remove("landing-white");
        callUnreadMessages();
        try{            
            SendAccessLog({
                Username: sessionDetails.Username,
                InvitationCode: sessionDetails.InvitationCode,
                Role: sessionDetails.Roles[0],
                AreaId: "8"
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    const callUnreadMessages = useCallback(async () => {
        try {
            const response = await getTotalOfUnreadMessages();
            setTotalUnreadMessages(response.data.TotalUnread || 0);
            setRedoSearch(false);
        } catch (error) {}
    }, []);

    const handleSelectedMessage = (messageId) => {
        setMessageId(messageId);
        setShowMessageDetailModal(true);
    };

    const closeShowMessageDetailModal = () => {
        setShowMessageDetailModal(false);
        setRedoSearch(true);
        callUnreadMessages();
    };

    const openNewMessage = () => setShowMessageCreateModal(true);

    const closeNewMessage = () => {
        setShowMessageCreateModal(false);
        setRedoSearch(true);
        callUnreadMessages();
    };

    const handleTabChange = (option) => {
        setSelectedOption(option);
        setIsReceived(option === "0");
        setRedoSearch(true);
        callUnreadMessages();
    };

    const refresh = () => {
        setRedoSearch(true);
        callUnreadMessages();
    };

    const canSendMessages = !(
        (isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && !context.state.variables.MessageManagementSendAdmin) ||
        (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && !context.state.variables.MessageManagementSendAdminOre) ||
        (isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles) && !context.state.variables.MessageManagementSendPrincipal) ||
        (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR], sessionDetails.Roles) && !context.state.variables.MessageManagementSendParent) ||
        (isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) && !context.state.variables.MessageManagementSendTeacher)
    );

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="student-dashboard.messages" options={{ renderInnerHtml: true }} />
                            </div>
                        </div>
                        <div className="col txt-right">
                            <button onClick={refresh} className="btn-default button-primary-custom text-uppercase mr-2">
                                <FontAwesomeIcon icon={faRedo} />{" "}
                                <Translate id="messages.refresh" options={{ renderInnerHtml: true }} />
                            </button>
                            {canSendMessages && (
                                <button onClick={openNewMessage} className="btn-default button-secondary-custom text-uppercase">
                                    <FontAwesomeIcon icon={faPlus} />{" "}
                                    <Translate id="messages.compose" options={{ renderInnerHtml: true }} />
                                </button>
                            )}
                        </div>
                    </div>

                    <Row style={{ minHeight: 600 }}>
                        <Col xs="12">
                            <div className="animated delay-05s fadeIn td-w-rollover padding-message-table tabs-style-message mt-3 td-responsive-message">
                                <CardTitle>
                                    <Row>
                                        <Col
                                            xs="6"
                                            className={selectedOption === "0" ? "tab underline-div" : "tab"}
                                            onClick={() => handleTabChange("0")}
                                        >
                                            <div>
                                                <FontAwesomeIcon icon={faMailBulk} />{" "}
                                                <Translate id="messages.received" options={{ renderInnerHtml: true }} />
                                                {totalUnreadMessages > 0 && (
                                                    <small className="notificaction-desc">
                                                        <i>
                                                            (<span className="notification-counter">{totalUnreadMessages}</span>{" "}
                                                            <Translate id="messages.no-read" options={{ renderInnerHtml: true }} />)
                                                        </i>
                                                    </small>
                                                )}
                                            </div>
                                        </Col>

                                        {canSendMessages && (
                                            <Col
                                                xs="6"
                                                className={selectedOption === "1" ? "tab underline-div" : "tab"}
                                                onClick={() => handleTabChange("1")}
                                            >
                                                <div>
                                                    <FontAwesomeIcon icon={faPaperPlane} />{" "}
                                                    <Translate id="messages.sent" options={{ renderInnerHtml: true }} />
                                                </div>
                                            </Col>
                                        )}
                                    </Row>
                                </CardTitle>

                                <CardBody className="mt--5 table-no-thead">
                                    {selectedOption === "0" && (
                                        <MessagesReceivedDataTable redoSearch={redoSearch} updateParent={handleSelectedMessage} />
                                    )}
                                    {selectedOption === "1" && (
                                        <MessagesSentDataTable redoSearch={redoSearch} updateParent={handleSelectedMessage} />
                                    )}
                                </CardBody>
                            </div>
                        </Col>
                    </Row>
                </div>
            </div>

            {showMessageDetailModal && (
                <Modal
                    isOpen={showMessageDetailModal}
                    toggle={closeShowMessageDetailModal}
                    className="modal-lg"
                >
                    <ModalHeader toggle={closeShowMessageDetailModal} />
                    <ModalBody>
                        <FormMessageReceived
                            isReceived={isReceived}
                            messageId={messageId}
                        />
                    </ModalBody>
                </Modal>
            )}

            {showMessageCreateModal && (
                <Modal
                    isOpen={showMessageCreateModal}
                    toggle={closeNewMessage}
                    className="modal-lg"
                >
                    <ModalHeader toggle={closeNewMessage}>
                        <b><Translate id="messages.new-message" options={{renderInnerHtml: true}}/></b>
                    </ModalHeader>
                        <ModalBody>
                            <FormMessageCreate updateParent={closeNewMessage}/>
                        </ModalBody>
                </Modal>
                )}
        </div>
    );
};

export default withLocalize(Messages);