import React, { useEffect, useState, useCallback } from 'react';
import { Button, Col, Row, } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { getMessageDetails, markMessageAsRead } from "../../../api/messages";
import FormMessageCreate from "./form-message-create";
import { BeatLoader } from "react-spinners";
import {formatUtcToLocal, getTimeZone} from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";
import LanguageManager from "../../../custom-classes/language-manager";

/**
 * Component to view the message details.
 */
const FormMessageReceived = ({ isReceived, messageId }) => {
    const [messages, setMessages] = useState([]);
    const [loading, setLoading] = useState(false);
    const [reply, setReply] = useState(false);
    const [isGeneralNotification, setIsGeneralNotification] = useState(false);
    const [language] = useState(LanguageManager.getLanguage());

    const callMessageDetails = useCallback(async (id) => {
        setLoading(true);
        try {
            const response = await getMessageDetails(id);
            setLoading(false);
            if (response.status === 200 || response.status === 201) {
                const data = response.data.EmailMessageFullDetailModel;

                const msg = data.EmailMessageModel;

                if (messages.find((m) => m.messageId === msg.Id)) return;

                const attachments = data.EmailAttachmentModelList.map((att) => (
                    <a key={att.AttachmentName} href={att.AttachmentUrl} download>
                        {att.AttachmentName}{' '}
                    </a>
                ));

                let isGeneral = "";
                if(data.EmailRecipientModelList != null) {
                    isGeneral = data.EmailRecipientModelList.find(
                        (r) => r.RecipientName === 'General Notification/Notificaci\u00f3n General'
                    );
                }

                if (isGeneral !== "") setIsGeneralNotification(true);

                let recipientsNamesString = "";
                if(isGeneral === "" && data.EmailRecipientModelList != null && data.EmailRecipientModelList.length > 0){
                   for(let i=0;i < data.EmailRecipientModelList; i++){
                       if(data.EmailAttachmentModelList[i].RecipientName !== null &&
                           data.EmailAttachmentModelList[i].RecipientName !== ""){
                           recipientsNamesString += data.EmailAttachmentModelList[i].RecipientName + ";"
                       }
                   }
               }

                const messageToAdd = {
                    messageId: msg.Id,
                    authorId: msg.AuthorId,
                    authorName: msg.UserFullName || '',
                    body: msg.Body,
                    subject: msg.Subject,
                    dateSent: msg.DateCreated,
                    parentId: msg.ParentMessageId,
                    recipientsNamesString,
                    attachments
                };

                setMessages(prev => [...prev, messageToAdd]);
                setLoading(false);
            }
        } catch {
            setLoading(false);
        }
    }, [messages]);

    const markAsRead = useCallback(async (id) => {
        try {
            markMessageAsRead(id);
        } catch {}
    }, []);

    useEffect(() => {
        callMessageDetails(messageId);
        if (isReceived) markAsRead(messageId);
    }, [callMessageDetails, markAsRead, isReceived, messageId]);

    const handleSeeMore = (e) => {
        callMessageDetails(e.currentTarget.value);
    };

    const handleReply = () => setReply(true);

    return (
        <div>
            {loading ? <BeatLoader size={10} color="#686e65" /> :
            <>
            <Row>
                <Col xs={12}>
                    {messages.map((item) => (
                        <div key={item.messageId} className="padding-20">
                            <Row className="mt--6">
                                <Col xs={12}><h4>{item.subject}</h4></Col>
                            </Row>
                            <Row className="b-bottom mb-1">
                                <Col xs={12} md={6}><b>{item.authorName}</b></Col>
                                <Col xs={12} md={6} className="text-right color-date">
                                    <small>{formatUtcToLocal(item.dateSent, language)}</small>
                                </Col>
                            </Row>
                            <Row className="mb-2 txt-strong txt-fifteen">
                                {(item.recipientsNamesString || isGeneralNotification) && (
                                    <Col xs={12}>
                                        <Translate id="messages.to" />{' '}
                                        {isGeneralNotification
                                            ? <Translate id="messages.general-notification" />
                                            : item.recipientsNamesString}
                                    </Col>
                                )}
                            </Row>
                            <Row>
                                <Col xs={12}>
                                    <div className="content" dangerouslySetInnerHTML={{ __html: item.body }} />
                                </Col>
                            </Row>
                            <Row>
                                <Col xs={12}>{item.attachments}</Col>
                            </Row>
                            {item.parentId && item.parentId !== '0' && (
                                <Row>
                                    <Col xs={12}>
                                        <Button
                                            className="btn-seemore"
                                            onClick={handleSeeMore}
                                            value={item.parentId}
                                            disabled={loading}
                                        >
                                            {loading && <BeatLoader size={10} color="#686e65" />}{' '}
                                            <Translate id="messages.see-more" />
                                        </Button>
                                    </Col>
                                </Row>
                            )}
                        </div>
                    ))}
                </Col>
            </Row>
            {isReceived && (
                <Row>
                    <Col xs={12} className="text-right">
                        <button
                            className="btn-default button-primary-custom"
                            onClick={handleReply}
                            outline
                            color="secondary"
                            value={messageId}
                        >
                            <Translate id="messages.reply" />
                        </button>
                    </Col>
                    {reply && (
                        <Col xs={12}>
                            <FormMessageCreate
                                isReply
                                recipients={messages[0]?.authorId}
                                parentId={messages[0]?.messageId}
                                subject={messages[0]?.subject}
                            />
                        </Col>
                    )}
                </Row>
            )}
            </>
            }
        </div>
    );
};

export default FormMessageReceived;