import React, { useState, useContext } from 'react';
import {
    Card,
    Col,
    Row,
    CardBody,
    Form
} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import UsersDataTable from "../../shared-module-components/datatables/users-data-table";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import { withRouter } from 'react-router-dom';
import { AppContext } from "../../../App.Context";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the user details
 */
const UsersSearch = ({ history }) => {
    const context = useContext(AppContext);
    const [redoSearch, setRedoSearch] = useState(false);
    const [userId, setUserId] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleUserSelection = (e) => {
        setUserId(e.UserId);
        history.push(`/user-manager?userId=${e.UserId}`);
    };

    const handleNewUser = () => {
        setUserId(null);
        history.push('/user-manager');
    };

    // Renders the main page
    const sessionDetails = SessionManager.getSessionDetails();
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="user-management.title" /></div>
                        </div>
                        {((isRoleAllowed([Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdmin) || (isRoleAllowed([Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.UserManagementAdminOre)) && <div className="col txt-right">
                            <button className="btn-default button-primary-custom" onClick={handleNewUser}>
                                <Translate id="user-management.add-new" />
                            </button>
                        </div>}
                    </div>

                    <Row className="clear-style-card td-responsive mt-4">
                        <Col xs={12}>
                            <Card>                                
                                <CardBody>
                                    <Form>
                                        <UsersDataTable redoSearch={redoSearch} updateParent={handleUserSelection} />
                                    </Form>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                    <hr />
                </div>
            </div>
        </div>
    );
};

export default withRouter(withLocalize(UsersSearch));