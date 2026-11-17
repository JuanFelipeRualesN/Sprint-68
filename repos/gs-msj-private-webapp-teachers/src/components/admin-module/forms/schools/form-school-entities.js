import React, { useState, useEffect } from 'react';
import { Col, Row, Button } from "reactstrap";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import { Translate } from "react-localize-redux";
import FormGroupEntityName from "./form-group-entity-name";
import ModalGeneric from "../../../shared-module-components/modal/modal-generic";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import FormGroupEntityCode from "./form-group-entity-code";
import FormGroupPublicPrivate from "./form-public-private";
import Settings from "../../../../custom-classes/settings";
import SessionManager from "../../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../../utils/utils";

import { AppContext } from "../../../../App.Context";
import useVersionedTranslations from '../../../../hooks/useVersionedTranslations';


// Component for subscriptions
const FormSchoolEntities = ({ 
    disabled, 
    entityCode, 
    entityId, 
    entityName, 
    entityType, 
    initialCode, 
    privateOrPublic,
    updateParent 
}) => {
    const [disabledState, setDisabledState] = useState(disabled);
    const [entityCodeState, setEntityCodeState] = useState(entityCode);
    const [entityIdState, setEntityIdState] = useState(entityId);
    const [entityNameState, setEntityNameState] = useState(entityName);
    const [entityTypeState, setEntityTypeState] = useState(entityType);
    const [initialCodeState, setInitialCodeState] = useState(initialCode);
    const [privateOrPublicState, setPrivateOrPublicState] = useState(privateOrPublic);
    const [loading, setLoading] = useState(false);
    const [navbarMobile, setNavbarMobile] = useState(false);

    const [showModal, setShowModal] = useState(false);
    const [propsModal, setPropsModal] = useState(null);

    const [saveButtonEnabled, setSaveButtonEnabled] = useState(false);
    const [validName, setValidName] = useState((entityId ? true : false));
    const [validEntityCode, setValidEntityCode] = useState((entityId ? true : false));

    const isEditing = !!entityId;

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        if (isEditing) {
            setSaveButtonEnabled(validName && validEntityCode);
        } else {
            setSaveButtonEnabled(validName || validEntityCode);
        }
    }, [isEditing]);

    useEffect(() => {
        if (disabled !== disabledState) {
            setDisabledState(disabled);
        }
        if (entityCode !== entityCodeState) {
            setEntityCodeState(entityCode);
        }
        if (entityId !== entityIdState) {
            setEntityIdState(entityId);
        }
        if (entityName !== entityNameState) {
            setEntityNameState(entityName);
        }
        if (entityType !== entityTypeState) {
            setEntityTypeState(entityType);
        }
        if (initialCode !== initialCodeState) {
            setInitialCodeState(initialCode);
        }
        if (privateOrPublic !== privateOrPublicState) {
            setPrivateOrPublicState(privateOrPublic);
        }
    }, [disabled, entityCode, entityId, entityName, entityType, initialCode, privateOrPublic]);

    const handleEntityName = (e) => {
        setValidName(e.validName);
        setEntityNameState(e.entityName);
        setSaveButtonEnabled(e.validName && validEntityCode);
    };

    const handleEntityCode = (e) => {
        setEntityCodeState(e.entityCode);
        setValidEntityCode(e.validCode);
        setSaveButtonEnabled(validName && e.validCode);
    };

    const handleEntityPrivateOrPublic = (e) => {
        setPrivateOrPublicState(e);
        setSaveButtonEnabled(validName && validEntityCode);
    };

    const handleSaveEntity = async () => {
        if (!entityNameState) {
            return;
        }
        const detailEntity = {
            entityName: entityNameState,
            entityId: entityIdState,
            entityType: entityTypeState,
            entityCode: entityCodeState,
            privateOrPublic: privateOrPublicState,
        };
        updateParent(detailEntity);
    };

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    // Renders the full form
    const sessionDetails = SessionManager.getSessionDetails();
    
    return (
        <div>
            <Row>
            <Col className="form-group-fields">
                <FormGroupEntityName
                    entityName={entityNameState}
                    updateParent={handleEntityName}
                    disabled={disabled}
                />
            </Col>

            <Col className="form-group-fields">
                <FormGroupEntityCode
                    initialCode={initialCodeState}
                    entityCode={entityCodeState}
                    updateParent={handleEntityCode}
                    disabled={disabled}
                />
            </Col>
            </Row>
            <Row>
                <Col xs={4}>
                    <FormGroupPublicPrivate
                        privateOrPublic={privateOrPublicState}
                        updateParent={handleEntityPrivateOrPublic}
                    />
                </Col>
            </Row>
            <Row>
                <Col xs={8} />
                <Col xs={4} style={{textAlign: "right"}}>
                    {(isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles)) ?
                        <Button className="btn-primary" onClick={handleSaveEntity}
                                disabled={!saveButtonEnabled}>
                            <Translate id="school-module.save-name" />
                        </Button> : null
                    }
                </Col>
            </Row>
            {loading ? <LoadingFullWindow /> : null}
            {showModal ? (
                <ModalGeneric
                    data={propsModal}
                    updateParent={toggleAlertModal}
                />
            ) : null}
        </div>
    );
};

export default withLocalize(FormSchoolEntities);
