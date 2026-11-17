/**
 * Created by portiz on 1/30/20.
 */

import React, { useState, useEffect, useContext } from 'react';
import {
    Card,
    Col,
    Container,
    Row,
    CardBody,
    Form,
    Button,
    CardTitle
} from 'reactstrap';

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { Redirect } from "react-router-dom";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowAltCircleLeft, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import EntitySchoolDataTable from '../datatables/school-entity-data-table';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { renderToStaticMarkup } from "react-dom/server";
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import { isRoleAllowed } from "../../../utils/utils";
import { Link } from "react-router-dom";
import { AppContext } from "../../../App.Context";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the user details
 */
const SchoolEntitySearch = ({initialize, addTranslation}) => {
    const context = useContext(AppContext);
    
    const [didMount, setDidMount] = useState(false);
    const [entityId, setEntityId] = useState(null);
    const [entityType, setEntityType] = useState(Settings.IS_APP_MANAGED_BY_REQUIRED ? SessionManager.getLastEntityTypeId() : "1");
    const [goToAddNew, setGoToAddNew] = useState(false);
    const [loading, setLoading] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Evaluate the resize, to show the proper navbar menu style
    useEffect(() => {
        setDidMount(true);
        setEntityType(Settings.IS_APP_MANAGED_BY_REQUIRED ? SessionManager.getLastEntityTypeId() : "1");
    }, []);

    const handleEntitySelection = (e) => {
        SessionManager.setLastEntityId(e.EntityId);
        setEntityId(e.EntityId);
    };

    const handleNewEntity = () => {
        SessionManager.deleteSelectedEntity();
        setGoToAddNew(true);
    };

    // Renders the main page
    const sessionDetails = SessionManager.getSessionDetails();


    if (typeof goToAddNew !== 'undefined' && goToAddNew)
        return (<Redirect to={{
            pathname: '/school-entity-manage',
            addNew: true
        }} />);

    if (typeof entityId !== 'undefined' && entityId !== null) {
        // Create URLSearchParams object to properly format the query string
        const params = new URLSearchParams();
        params.append('entityId', entityId);

        return (
            <Redirect to={{
                pathname: '/school-entity-manage',
                search: `?${params.toString()}` // Append the search parameters
            }} />
        );
    }

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="school-module.manage-schools"/>
                            </div>
                        </div>

                        {(isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles)) ?
                            <div className="col txt-right">
                                <button className="button-primary-custom btn-default" onClick={handleNewEntity}>

                                    <Translate id="school-module.add-new"/>
                                </button>

                            </div> : null }
                    </div>

                    {/* <h2 className="font-now-bold black-text">
                        {
                            entityType == '1' ?
                            <div className="title-principal title-admin b-bottom mt-3">
                                <FontAwesomeIcon icon={faGraduationCap}/>{' '}
                                 <Translate id="school-module.manage-schools"/>
                            </div> : null
                        }
                        {
                            entityType == '2' ?
                                <div className="title-principal title-admin b-bottom mt-3">
                                    <FontAwesomeIcon icon={faTasks}/>{' '}
                                    <Translate id="school-module.manage-programs"/>
                                </div> : null
                        }
                        {
                            entityType == '3' ?
                                <div className="title-principal title-admin b-bottom mt-3">
                                    <FontAwesomeIcon icon={faBuilding}/>{' '}
                                    <Translate id="school-module.manage-institutions"/>
                                </div> : null
                        }
                        {
                            entityType == '4' ?
                                <div className="title-principal title-admin b-bottom mt-3">
                                    <FontAwesomeIcon icon={faBriefcase}/>{' '}
                                    <Translate id="school-module.manage-companies"/>
                                </div> : null
                        }
                    </h2> */}


                    <Row className="clear-style-card td-responsive mt-2">
                        <Col xs={12}>
                            <Card body>
                                {/* <CardTitle className="text-right">
                                    <button className="btn-default button-primary-custom" onClick={handleNewEntity}>
                                        <Translate id="school-module.add-new"/>
                                    </button>
                                </CardTitle> */}
                                <CardBody>
                                    <Form>
                                        <EntitySchoolDataTable
                                            entityType={entityType}
                                            redoSearch={redoSearch}
                                            updateParent={handleEntitySelection} />
                                    </Form>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                    <hr />
                </div>
            </div>

            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default withLocalize(SchoolEntitySearch);