/**
 * Created by Pedro Ortiz
 * 2021
 */
import React, { useState, useEffect } from 'react';
import {
    Card,
    Col,
    Container,
    Row,
    CardBody,
    Button, CardHeader, Label, Input
} from 'reactstrap';

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

import { renderToStaticMarkup } from "react-dom/server";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import swal from "sweetalert";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { getRoleById, postRolesUpdate, postRolesCreate } from "../../../api/users";
import MultiSelectorPermission from "../selectors/multi-select-permission";
import SelectorRoles from "../../shared-module-components/selector/selector-roles";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faExclamationTriangle, faSearch, faArrowAltCircleLeft, faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle roles.
 */
const RoleManager = () => {
    const [id, setId] = useState(null);
    const [loading, setLoading] = useState(false);
    const [isEdit, setIsEdit] = useState(false);
    const [addNew, setAddNew] = useState(false);
    const [resetBox, setResetBox] = useState(false);
    const [redirect, setRedirect] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);
    const [selectedPermissions, setSelectedPermissions] = useState([]);
    const [name, setName] = useState('');
    const [desc, setDesc] = useState('');
    const [permissionDesc, setPermissionDesc] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleSelection = (e) => {
        if (typeof e !== 'undefined') {
            setId(e);
            setIsEdit(true);
            setResetBox(false);
            setRedoSearch(false);
            readData(e);
        }
        else {
            setRedoSearch(false);
            setId(null);
            setName('');
            setDesc('');
            setResetBox(true);
            setPermissionDesc('');
            setSelectedPermissions([]);
        }
    };

    const handlePermissionSelection = (e) => {
        setSelectedPermissions(e);
        if (e.length > 0) {
            setPermissionDesc(e[e.length - 1].Desc);
        }
    };

    const readData = (roleId = id) => {
        if (roleId !== null) {
            setLoading(true);
            
            let apiCall = getRoleById(roleId);
            apiCall.then((response) => {
                setLoading(false);
                setName(response.data.Name);
                setDesc(response.data.Desc);

                let selected = [];
                for (let i = 0; i < response.data.PermissionModelList.length; i++) {
                    selected.push({
                        value: response.data.PermissionModelList[i].Id,
                        label: response.data.PermissionModelList[i].Name,
                        desc: response.data.PermissionModelList[i].Desc,
                    });
                }
                setSelectedPermissions(selected);
            });
        }
    };

    const saveChanges = () => {
        if (id === null) {
            setLoading(true);
            // Create a new role
            let apiCall = postRolesCreate(name, desc, selectedPermissions);
            apiCall.then((response) => {
                if (response.data.Id != null) {
                    setLoading(false);
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");
                    
                    setRedoSearch(prev => !prev);
                    setId(null);
                    setName('');
                    setDesc('');
                    setResetBox(true);
                    setPermissionDesc('');
                    setSelectedPermissions([]);
                }
                else {
                    if (response.data.Messages?.length) {
                        let message = response.data.Messages.join(" ");
                        swal("", message, "");
                    }
                }
            });
        }
        else {
            setLoading(true);
            // Update an existing role
            let apiCall = postRolesUpdate(id, name, desc, selectedPermissions);
            apiCall.then((response) => {
                setLoading(false);

                if (response.data.Messages?.length) {
                    let message = response.data.Messages.join(" ");
                    swal("", message, "");
                }
                else {
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");
                    
                    setRedoSearch(prev => !prev);
                    setId(null);
                    setName('');
                    setDesc('');
                    setPermissionDesc('');
                    setResetBox(true);
                    setSelectedPermissions([]);
                }
            });
        }
    };

    const handleNew = () => {
        setAddNew(true);
        setId(null);
        setName('');
        setDesc('');
        setResetBox(true);
        setPermissionDesc('');
        setSelectedPermissions([]);
    };

    const handleNameChange = (e) => {
        setName(e.currentTarget.value);
        setRedoSearch(false);
    };

    const handleDescriptionChange = (e) => {
        setDesc(e.currentTarget.value);
        setRedoSearch(false);
    };

    // Renders the main page
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="col col-no-padding-left">
                        <Link to="/access" className="btn-back">
                            <FontAwesomeIcon icon={faArrowLeft} />{' '}
                            <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                        </Link>
                    </div>

                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="roles.roles-manager" /></div>
                        </div>
                        <div className="col txt-right">
                            <button className="btn-default button-primary-custom" onClick={handleNew}>
                                <Translate id="general.add-new" />
                            </button>
                        </div>
                    </div>
                    <p className='mt-4 alert-message' style={{ "width": "90%" }}>
                        <small>
                            <FontAwesomeIcon icon={faExclamationTriangle} />{' '}
                            <Translate id="roles.roles-manager-instruction" options={{ renderInnerHtml: true }} />
                        </small>
                    </p>

                    <Row className="clear-style-card td-responsive">
                        <CardBody>
                            {!addNew ?
                                <Row>
                                    <Col xs={12} md={4}>
                                        <>
                                            <Label for="roles-select">
                                                <Translate id="roles.select-to-edit" />
                                            </Label>
                                            <SelectorRoles redoSearch={redoSearch} roleId={id} updateParent={handleSelection} />
                                        </>
                                    </Col>
                                </Row> : null}
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={4}>
                                    <Label for="name">
                                        * <Translate id="roles.name" />
                                    </Label>
                                    <Input maxlength="20" value={name} type="text" name="name" disabled={!isEdit && !addNew ? "disabled" : ""}
                                        id="name" onChange={handleNameChange} />
                                </Col>
                            </Row>

                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={4}>
                                    <Label for="desc">
                                        * <Translate id="roles.description" />
                                    </Label>
                                    <Input value={desc} type="textarea" name="desc" disabled={!isEdit && !addNew ? "disabled" : ""}
                                        id="desc" onChange={handleDescriptionChange} />
                                </Col>
                            </Row>
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={10}>
                                    <Label for="select-permissions">
                                        * <Translate id="roles.select-permissions" />
                                    </Label>
                                    <MultiSelectorPermission value={selectedPermissions} resetBox={resetBox} redoSearch={false} disabled={addNew} updateParent={handlePermissionSelection} />
                                </Col>
                            </Row>
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={10}>
                                    <b>{permissionDesc}</b>
                                </Col>
                            </Row>

                            <Row className="clear-style-card td-responsive mt-4">
                                <Button color="success" disabled={desc === "" || name === "" || selectedPermissions.length === 0 ? "disabled" : ""} onClick={saveChanges}>
                                    <Translate id="roles.submit" />
                                </Button>
                            </Row>
                        </CardBody>
                    </Row>
                    <hr />
                </div>
            </div>
            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default withLocalize(RoleManager);