/**
 * Created by portiz on 06/16/2020.
 */
import React, { useState, useEffect } from 'react';
import {Card, CardBody, Col, Container, Row, FormGroup, Label} from 'reactstrap';

/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import QueryString from "query-string";
import FileUploadQuiz from "../file-uploader/file-upload-quiz";
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to upload files to exam for students (Unity compliment). Created to help Jonnathan with the uploads in Unity.
 */
const ExamUpload = ({ location }) => {
    const parsedString = QueryString.parse(location.search);
    const [attachmentsURL, setAttachmentsURL] = useState([]);
    const [attachmentsHREF, setAttachmentsHREF] = useState([]);
    const [attachmentsName, setAttachmentsName] = useState([]);
    const [authorizationCode] = useState(
        typeof parsedString !== 'undefined' && typeof parsedString.authorization !== 'undefined' && parsedString.authorization !== null ? parsedString.authorization : null
    );
    const [token] = useState(
        typeof parsedString !== 'undefined' && typeof parsedString.token !== 'undefined' && parsedString.token !== null ? parsedString.token : null
    );
    const [showExpiredLinkMessage, setShowExpiredLinkMessage] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.add('exam-background');
    }, []);

    const setUploadedFile = (fileUrl, fileName) => {
        const newAttachmentNames = [...attachmentsName, fileName];
        const newAttachments = [...attachmentsURL, fileUrl];
        const newAttachmentsHREFs = [...attachmentsHREF, <a href={fileUrl}>{fileName}</a>];

        setAttachmentsName(newAttachmentNames);
        setAttachmentsURL(newAttachments);
        setAttachmentsHREF(newAttachmentsHREFs);
    };

    // Renders the main dashboard
    const listAttachmentsItems = attachmentsHREF.map((fileDetail) =>
        <li>{fileDetail}</li>
    );

    if (typeof token === 'undefined' || token === null || typeof authorizationCode === 'undefined'  || authorizationCode === null || showExpiredLinkMessage){
        return(
            <div>
                <div className="container-padding animated delay-05s fadeIn">
                    <Container className="container-wrapper">
                        <Card body className="card-white animated delay-05s fadeIn">
                            <CardBody>
                                <h3><Translate id="exam.invalid"/></h3>
                            </CardBody>
                        </Card>
                    </Container>
                </div>
            </div>
        );
    }

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <Container className="container-wrapper">
                    <Row>
                        <Col xs={12}>
                            <Card body className="card-white animated delay-05s fadeIn">
                                <CardBody>
                                    <Row>
                                        <Col xs={12}>
                                            <FormGroup row>
                                                <Label for="exampleFile" sm={2}><Translate id="messages.attach-file" options={{renderInnerHtml: true}}/></Label>
                                                <Col sm={10}>
                                                    {listAttachmentsItems}
                                                    <FileUploadQuiz updateParent={setUploadedFile} token={token}/>
                                                </Col>
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </Container>
            </div>
        </div>
    );
};

export default withLocalize(ExamUpload);