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
import { getPermissionById, postPermissionUpdate, postPermissionCreate } from "../../../api/users";
import SelectorPermission from "../selectors/selector-permission";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faExclamationTriangle, faArrowAltCircleLeft, faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle permissions.
 */
const PermissionManager = () => {
    const [id, setId] = useState(null);
    const [loading, setLoading] = useState(false);
    const [isEdit, setIsEdit] = useState(false);
    const [addNew, setAddNew] = useState(false);
    const [resetBox, setResetBox] = useState(false);
    const [redirect, setRedirect] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);
    const [name, setName] = useState('');
    const [desc, setDesc] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleSelection = (e) => {
        setId(e.value);
        setIsEdit(true);
        setResetBox(false);
        setRedoSearch(false);
        readData(e.value);
    };

    const readData = (permissionId = id) => {
        if (permissionId !== null) {
            setLoading(true);
            
            let apiCall = getPermissionById(permissionId);
            apiCall.then((response) => {
                setLoading(false);

                if (response.data.length > 0) {
                    setName(response.data[0].Name);
                    setDesc(response.data[0].Desc);
                }
            });
        }
    };

    const saveChanges = () => {
        if (id === null) {
            setLoading(true);
            // Submits the registration
            let apiCall = postPermissionCreate(name, desc);
            apiCall.then((response) => {
                if (response.data.Id != null) {
                    setLoading(false);
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");
                    
                    setRedoSearch(true);
                    setId(null);
                    setName('');
                    setDesc('');
                    setResetBox(true);
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
            // Update the permission
            let apiCall = postPermissionUpdate(id, name, desc);
            apiCall.then((response) => {
                setLoading(false);

                if (response.data.Messages?.length) {
                    let message = response.data.Messages.join(" ");
                    swal("", message, "");
                }
                else {
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");

                    setRedoSearch(true);
                    setId(null);
                    setName('');
                    setDesc('');
                    setResetBox(true);
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
                            <div className="title-principal"><Translate id="permission.permission" /></div>
                        </div>
                        <div className="col txt-right">
                            <button className="btn-default button-primary-custom" onClick={handleNew}>
                                <Translate id="general.add-new" />
                            </button>
                        </div>
                    </div>

                    <Row className="clear-style-card td-responsive">
                        <p className='mt-4 alert-message' style={{ "width": "90%" }}>
                            <small>
                                <FontAwesomeIcon icon={faExclamationTriangle} />{' '}
                                <Translate id="roles.permissions-manager-instruction" options={{ renderInnerHtml: true }} />
                            </small>
                        </p>
                        <CardBody>
                            <Row className="clear-style-card td-responsive">

                                <Col xs={12} md={4}>
                                    {!addNew ?
                                        <>
                                            <Label for="permission-select">
                                                <Translate id="permission.select-to-edit" />
                                            </Label>
                                            <SelectorPermission resetBox={resetBox} redoSearch={redoSearch} disabled={addNew} updateParent={handleSelection} />
                                        </> : null}
                                </Col>

                                <Col>
                                    <Label for="permission-add"></Label>
                                </Col>
                            </Row>
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={4}>
                                    <Label for="name">
                                        * <Translate id="permission.name" />
                                    </Label>
                                    <Input maxlength="45" value={name} type="text" name="name" disabled={!isEdit && !addNew ? "disabled" : ""}
                                        id="name" onChange={handleNameChange} />
                                </Col>
                            </Row>

                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={4}>
                                    <Label for="name">
                                        * <Translate id="permission.description" />
                                    </Label>
                                    <Input value={desc} type="textarea" name="name" disabled={!isEdit && !addNew ? "disabled" : ""}
                                        id="name" onChange={handleDescriptionChange} />
                                </Col>
                            </Row>

                            <Row className="clear-style-card td-responsive mt-4">
                                <Button color="success" disabled={desc === "" || name === "" ? "disabled" : ""} onClick={saveChanges}>
                                    <Translate id="product.submit" />
                                </Button>
                            </Row>
                        </CardBody>
                    </Row>
                </div>
            </div>
            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default withLocalize(PermissionManager);