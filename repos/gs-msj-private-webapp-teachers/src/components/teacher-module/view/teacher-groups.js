import React, { useState, useEffect } from 'react';
import { CardBody, Col, Row, Button } from 'reactstrap';
import { Redirect } from "react-router-dom";

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faPlus
} from '@fortawesome/free-solid-svg-icons';
import SessionManager from "../../../custom-classes/session-manager";
import ModalGroupNameCreator from "../modal/modal-group-name-creator";
import GroupsDataTable from "../datatable/groups-data-table";
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
import { SendAccessLog } from '../../../utils/access_logger';

/**
 * The component (window) to handle the teacher groups
 */
const TeacherGroups = () => {
    const userDetails = SessionManager.getSessionDetails();
    const [groupId, setGroupId] = useState(null);
    const [groupName, setGroupName] = useState('');
    const [redoSearch, setRedoSearch] = useState(false);
    const [showNewGroupModal, setShowNewGroupModal] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.remove('landing-white');
        
        try{            
            SendAccessLog({
                UserName: userDetails.Username,
                InvitationCode: userDetails.InvitationCode,
                Role: userDetails.Roles[0],
                AreaId: "2",
            })
        }
        catch (error){
            // empty catch block to avoid breaking the application
        }
    }, []);

    const handleSelectedGroup = (id, name) => {
        SessionManager.setSelectedGroup(id, name);
        setGroupId(id);
        setGroupName(name);
    };

    const openNewGroupModal = () => {
        setShowNewGroupModal(true);
        setRedoSearch(false);
    };

    const closeNewGroupModal = () => {
        setShowNewGroupModal(false);
        setRedoSearch(true);
    };

    if (groupId !== null) {
        return <Redirect to={{ pathname: '/teacher-groups-detail' }} />;
    }

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="group-management.group-students" options={{ renderInnerHtml: true }} />
                            </div>
                        </div>
                        <div className="col txt-right">
                            <Button onClick={openNewGroupModal} className="button-primary-custom" color="none" variant="none">
                                <FontAwesomeIcon icon={faPlus} />{' '}
                                <Translate id="group-management.add-new-group" options={{ renderInnerHtml: true }} />
                            </Button>
                        </div>
                    </div>
                    <Row style={{ minHeight: 600 }} className="mt-15px">
                        <Col xs={12}>
                            <div className="card-white animated delay-05s fadeIn">
                                <CardBody className="space-btn-icon danger-btn-simple big-icon second-col-center td-responsive special-top-space delete-before-5 delete-before-4 inline-table-5">
                                    <GroupsDataTable redoSearch={redoSearch} updateParent={handleSelectedGroup} />
                                </CardBody>
                            </div>
                        </Col>
                    </Row>
                </div>
            </div>
            {showNewGroupModal && (
                <ModalGroupNameCreator
                    visible={showNewGroupModal}
                    updateParent={closeNewGroupModal}
                />
            )}
        </div>
    );
};

export default withLocalize(TeacherGroups);
