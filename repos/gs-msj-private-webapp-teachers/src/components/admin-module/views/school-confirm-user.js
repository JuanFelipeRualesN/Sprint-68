/**
 * Created by portiz on 10/08/20.
 */
import React, { useState, useEffect } from 'react';
import {
    Card,
    Col,
    Container,
    Row,
    CardBody,
    Button
} from 'reactstrap';
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import UsersDataTable from "../../shared-module-components/datatables/users-data-table";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faUserPlus} from "@fortawesome/free-solid-svg-icons";
import UserSchoolsDataTable from "../datatables/user-schools-data-table";
import {renderToStaticMarkup} from "react-dom/server";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to confirm that the user is part of a school.
 */
const SchoolConfirmUser = ({initialize, addTranslation}) => {
    const [userId, setUserId] = useState(null);
    const [userName, setUserName] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    const returnToUserSelection = () => {
        setUserId(null);
    };

    const handleUserSelection = (e) => {
        setUserId(e.UserId);
        setUserName(e.GivenName + ' ' + e.Surname);
    };

    // Renders the main page
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <h2 className="font-now-bold  black-text">
                        {/* <FontAwesomeIcon icon={faUserPlus}/>{' '} */}
                        <Translate id="school-module.add-user-school"/>
                    </h2>
                    <hr/>

                    <Row className="clear-style-card td-responsive">
                        <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>

                        {
                            userId === null ?
                                <Card>
                                    <CardBody>
                                    <UsersDataTable updateParent={handleUserSelection}/>
                                    </CardBody>
                                </Card>
                                :
                                <Card>
                                    <CardBody>
                                    <Translate id="users-schools.selected-user"/>: {userName}
                                    {' '}
                                    <button className="btn-default button-secondary-custom" onClick={returnToUserSelection}>
                                        <Translate id="users-schools.change-user"/>
                                    </button>
                                    </CardBody>
                                </Card>
                        }

                                </Col>
                    </Row>

                    <Row>
                        <Col xs={12}>
                            <Card>
                                <CardBody>
                            {userId !== null ?
                                <UserSchoolsDataTable userId={userId}/> : null
                            }
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </div>
            </div>

        </div>
    );
};

export default withLocalize(SchoolConfirmUser);