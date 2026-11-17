import React, { useState, useEffect } from 'react';
import { CardBody, Col, Row } from 'reactstrap';
import { Link, Redirect } from "react-router-dom";

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import SessionManager from "../../../custom-classes/session-manager";
import ProfilesInGroupDataTable from "../datatable/profiles-in-group-data-table";
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the teacher groups
 */
const TeacherGroupDetail = () => {
    const group = SessionManager.getSelectedGroup();

    const [groupId] = useState(group !== null ? group.groupId : null);
    const [groupName] = useState(group !== null ? group.groupName : '');
    const [redoSearch] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.remove('landing-white');
    }, []);

    if (!groupId) {
        return <Redirect to={{ pathname: "/teacher-groups" }} />;
    }

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col mb-3">
                        <div className="col col-no-padding-left">
                            <Link to="/teacher-groups" className="btn-back">
                                <FontAwesomeIcon icon={faArrowLeft} />{' '}
                                <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                            </Link>
                            <div className="title-principal mt-3">
                <span className="title-principal--inside">
                  <Translate id="group-management.group" options={{ renderInnerHtml: true }} />{': '}
                </span>
                                {groupName}
                            </div>
                        </div>
                    </div>
                    <Row>
                        <Col xs={12}>
                            <div className="simple-wrapper table-no-margin-top table-no-thead danger-btn-simple space-btn-icon paddingTable-responsive">
                                <CardBody>
                                    <ProfilesInGroupDataTable
                                        groupId={groupId}
                                        groupName={groupName}
                                        redoSearch={redoSearch}
                                        updateParent={null}
                                    />
                                </CardBody>
                            </div>
                        </Col>
                    </Row>
                </div>
            </div>
        </div>
    );
};

export default withLocalize(TeacherGroupDetail);