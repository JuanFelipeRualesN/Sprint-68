/**
 * Created by Jonnathan Aponte and Pedro Ortiz
 * 2020 nov
 */
import React, { useState, useEffect } from 'react';
import {
    Card,
    Col,
    Container,
    Row,
    CardBody,
    Button, CardHeader, Label, Input, Alert
} from 'reactstrap';

/** Multiple Language imports **/
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

import { renderToStaticMarkup } from "react-dom/server";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import SelectorItemProduct from "../selectors/selector-item-product";
import Checkbox from "@material-ui/core/Checkbox";
import { postUpdateItemsProductsAPI, getReadSpecificItemProductAPI, postItemsProductsAPI } from "../../../api/subscriptions";
import swal from "sweetalert";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faExclamationTriangle,
    faArrowAltCircleLeft,
    faArrowLeft,
    faInfoCircle
} from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) to handle item products
 */
const ItemsProductManager = () => {
    const [id, setId] = useState(null);
    const [availableTranslation, setAvailableTranslation] = useState(LanguageManager.getTranslation("product.available", Settings.LANGUAGES));
    const [available, setAvailable] = useState(true);
    const [loading, setLoading] = useState(false);
    const [isEdit, setIsEdit] = useState(false);
    const [addNew, setAddNew] = useState(false);
    const [resetBox, setResetBox] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);
    const [subjectCode, setSubjectCode] = useState('');
    const [levelCode, setLevelCode] = useState('');
    const [name, setName] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleItemProductSelection = (e) => {
        setId(e.value);
        setIsEdit(true);
        setResetBox(false);
        readData(e.value);
    };

    const readData = (productId = id) => {
        if (productId !== null) {
            setLoading(true);
            
            let apiCall = getReadSpecificItemProductAPI(productId);
            apiCall.then((response) => {
                setLoading(false);

                if (response.data.length > 0) {
                    setSubjectCode(response.data[0].SubjectCode);
                    setLevelCode(response.data[0].LevelCode);
                    setName(response.data[0].Name);
                    setAvailable(response.data[0].Available === "1");
                }
            });
        }
    };

    const saveChanges = () => {
        const cleanedName = trimAndNormalizeText(name);
        const cleanedSubjectCode = trimAndNormalizeText(subjectCode);
        const cleanedLevelCode = trimAndNormalizeText(levelCode);

        if (id === null) {
            setLoading(true);
            
            let apiCall = postItemsProductsAPI(cleanedName, available, cleanedSubjectCode, cleanedLevelCode);
            apiCall.then((response) => {
                if (typeof response.data.Id !== 'undefined' && response.data.Id !== null) {
                    setLoading(false);
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");
                    
                    setRedoSearch(false);
                    setId(null);
                    setSubjectCode('');
                    setLevelCode('');
                    setName('');
                    setResetBox(true);
                }
                else {
                    if (typeof response.data.Messages !== 'undefined' && response.data.Messages !== null) {
                        let message = "";
                        for (let i = 0; i < response.data.Messages.length; i++) {
                            message += response.data.Messages[i];
                        }
                        swal("", message, "");
                    }
                }
            });
        }
        else {
            setLoading(true);
            
            let apiCall = postUpdateItemsProductsAPI(id, cleanedSubjectCode, cleanedLevelCode, cleanedName, available);
            apiCall.then((response) => {
                setLoading(false);

                if (typeof response.data.Messages !== 'undefined' && response.data.Messages !== null) {
                    let message = "";
                    for (let i = 0; i < response.data.Messages.length; i++) {
                        message += response.data.Messages[i];
                    }
                    swal("", message, "");
                }
                else {
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");
                    
                    setRedoSearch(false);
                    setId(null);
                    setSubjectCode('');
                    setLevelCode('');
                    setName('');
                    setResetBox(true);
                }
            });
        }
    };

    const handleNewProduct = () => {
        setAddNew(true);
        setId(null);
        setSubjectCode('');
        setLevelCode('');
        setName('');
        setResetBox(true);
    };

    const handleNameChange = (e) => {
        setName(e.currentTarget.value);
    };

    const handleSubjectChange = (e) => {
        setSubjectCode(e.currentTarget.value);
    };

    const handleCheckboxChange = (e) => {
        setAvailable(e.currentTarget.checked);
    };

    const handleLevelChange = (e) => {
        setLevelCode(e.currentTarget.value);
    };

    const trimAndNormalizeText = (text) => {
        return text.trim().replace(/\s{2,}/g, ' '); 
    };

    const isValidName = name && typeof name === 'string' && name.trim().length > 0 && name.length <= 100;
    // Renders the main page
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="col col-no-padding-left">
                        <Link to="/curriculum-module" className="btn-back">
                            <FontAwesomeIcon icon={faArrowLeft} />{' '}
                            <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                        </Link>
                    </div>
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="product.manage-item-product" /></div>
                        </div>
                        <div className="col txt-right">
                            <button className="btn-default button-primary-custom" onClick={handleNewProduct}>
                                <Translate id="user-management.add-new" />
                            </button>
                        </div>
                    </div>
                    <p className='mt-4 alert-message' style={{ "width": "90%" }}>
                        <small>
                            <FontAwesomeIcon icon={faExclamationTriangle} />{' '}
                            <Translate id="product.product-manager-instruction" options={{ renderInnerHtml: true }} />
                        </small>
                    </p>
                    <Row className="clear-style-card td-responsive mt-4">
                        <CardBody>
                            {!addNew ?
                                <Row className="clear-style-card td-responsive">
                                    <Col xs={12} md={4}>
                                        <Label for="subjects-grades">
                                            * <Translate id="product.subjects-grades" />
                                        </Label>
                                        <SelectorItemProduct resetBox={resetBox} redoSearch={redoSearch} disabled={addNew} updateParent={handleItemProductSelection} />
                                    </Col>

                                    <Col>
                                        <Label for="subjects-grades">
                                        </Label>
                                    </Col>
                                </Row> : null}
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={4}>
                                    <Label for="name">
                                        * <Translate id="product.name" />
                                    </Label>
                                    <Input value={name} type="text" name="name" disabled={!isEdit && !addNew ? "disabled" : ""}
                                        id="name" onChange={handleNameChange} />
                                </Col>
                            </Row>
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={4}>
                                    <Label for="name">
                                        * <Translate id="product.available" />
                                    </Label>
                                    <Checkbox name={availableTranslation} disabled={!isEdit && !addNew ? "disabled" : ""}
                                        checked={available}
                                        onChange={handleCheckboxChange}
                                    />
                                </Col>
                            </Row>
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={12}>
                                    <Label for="name">
                                        * <Translate id="product.subject-code" />
                                        <br />
                                        <small><Translate id="product.subject-code-exp" /></small>
                                    </Label>
                                </Col>
                                <Col xs={12} md={4}>
                                    <Input value={subjectCode} type="text" name="subject"
                                           disabled={!addNew ? "disabled" : ""}
                                           id="subject" onChange={handleSubjectChange} />

                                </Col>
                                { isEdit && !addNew ?
                                  <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                      <Alert color="info">
                                          <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="product.message-info-edit-not-allow"/>
                                      </Alert>
                                  </Col>
                                  : null}
                            </Row>
                            <Row className="clear-style-card td-responsive">
                                <Col xs={12} md={12}>
                                    <Label for="name">
                                        * <Translate id="product.level-code" />
                                        <br />
                                        <small><Translate id="product.level-code-exp" /></small>
                                    </Label>
                                </Col>
                                <Col xs={12} md={4}>
                                    <Input value={levelCode} type="text" name="level"
                                           id="level" onChange={handleLevelChange}
                                           disabled={!addNew ? "disabled" : ""}
                                    />
                                </Col>
                                { isEdit && !addNew ?
                                  <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                      <Alert color="info">
                                          <FontAwesomeIcon icon={faInfoCircle} /> {' '}<Translate id="product.message-info-edit-not-allow"/>
                                      </Alert>
                                  </Col>
                                  : null}
                            </Row>
                                  <Row className="clear-style-card td-responsive mt-4">
                                <Button
                                    color="info" 
                                    disabled={name === "" || !isValidName || subjectCode === "" || levelCode === "" ? "disabled" : ""} 
                                    onClick={saveChanges}
                                >
                                    <Translate id="product.submit" />
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

export default withLocalize(ItemsProductManager);
