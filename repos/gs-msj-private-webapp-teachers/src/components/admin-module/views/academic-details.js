import React, { useState, useEffect } from 'react';
import {
    Card,
    Col,
    Row,
    CardBody,
    Form,
} from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import AcademicDetailsTable from '../datatables/academic-details-table';
import { renderToStaticMarkup } from "react-dom/server";
import Settings from "../../../custom-classes/settings";
import ModalSchoolEntityFilter from "../modals/school-module/modal-school-entity-filter";
import ModalAddDetails from "../modals/modal-add-details";
import { getSchoolEntityDetails, getAcademicsDetailsByEntity } from "../../../api/schools";
import { withRouter } from 'react-router-dom';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const AcademicDetails = ({ location, history }) => {
    const [loading, setLoading] = useState(false);
    const [details, setDetails] = useState([]);
    const [modalVisible, setModalVisible] = useState(false);
    const [modalVisibleAddDetails, setModalVisibleAddDetails] = useState(false);
    const [disabledFields] = useState(true);
    const [entityCode, setEntityCode] = useState("");
    const [entityName, setEntityName] = useState("");
    const [entityId, setEntityId] = useState("");
    const [typeEntity, setTypeEntity] = useState(Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID);
    const [entityAcademicDetail, setEntityAcademicDetail] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        const params = new URLSearchParams(location.search);
        const entityIdParam = params.get('entityId');
        if (entityIdParam) {
            getSchoolEntity(entityIdParam);
            getAcademicsDetails(entityIdParam, 0, 20);
        }
    }, [location.search]);

    const getSchoolEntity = async (entityIdQuery) => {
        setLoading(true);
        try {
            const res = await getSchoolEntityDetails(entityIdQuery);
            const entity = res.data;
            setLoading(false);
            setEntityId(entity.Entities[0].Id);
            setEntityName(entity.Entities[0].Name);
            setEntityCode(entity.Entities[0].Token);
        } catch {
            setLoading(false);
        }
    };

    const getAcademicsDetails = async (entityId, page, pageSize) => {
        setLoading(true);
        try {
            const res = await getAcademicsDetailsByEntity(entityId, page, pageSize);
            const data = res.data;
            setLoading(false);
            setDetails(data.EntityAcademicDetailModels);
        } catch {
            setLoading(false);
        }
    };

    const handleSchoolModal = () => {
        setModalVisible(true);
        setTypeEntity(Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID);
    };

    const handleProgramModel = () => {
        setModalVisible(true);
        setTypeEntity(Settings.SCHOOL_MODULE_PROGRAM_TYPE_ID);
    };

    const handleSelected = (entity) => {
        if (entity) {
            const selectedEntityId = entity.EntityId;
            history.push(`/academic-details?entityId=${selectedEntityId}`);
            setEntityId(selectedEntityId);
            setEntityName(entity.EntityName);
            setEntityCode(entity.EntityIdentifier);
            getAcademicsDetails(selectedEntityId, 0, 20);
        }
        setModalVisible(false);
    };

    const handleToggleModalAddDetails = (visible) => {
        setModalVisibleAddDetails(visible);
        getAcademicsDetails(entityId, 0, 20);
    };

    const handleOpenModalAddDetails = () => {
        setModalVisibleAddDetails(true);
        setEntityAcademicDetail(null);
    };

    const handleSelectedItemTable = (item) => {
        setModalVisibleAddDetails(true);
        setEntityAcademicDetail(item);
    };

    return (
        <>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="school-academics-details.title" /></div>
                        </div>
                    </div>

                    <div className="content-wizard mt-3">
                        <span className="content-wizard--title">
                             <Translate id="wizard.step-3.sub-title" options={{ renderInnerHtml: true }} />
                        </span>
                        <br/>

                        <button className="content-wizard--w-icon mr-1" onClick={handleSchoolModal}>
                            {" "}
                            <i className="fas fa-plus-circle" />
                            <Translate id="wizard.step-3.select-school" options={{ renderInnerHtml: true }} />{" "}
                        </button>
                        {Settings.IS_APP_MANAGED_BY_REQUIRED ?
                        <button className="content-wizard--w-icon" onClick={handleProgramModel}>
                            {" "}
                            <i className="fas fa-plus-circle" />
                            <Translate id="wizard.step-3.select-program" options={{ renderInnerHtml: true }} />
                            {" "}
                        </button> : null}


                        <div className="wrapper-wizard-form mt-3">
                            <div className="wrapper-wizard-form--col">
                                <div className="form-wizard-fields">
                                    <label>*
                                        <Translate id="wizard.step-3.name" options={{ renderInnerHtml: true }} />
                                    </label>
                                    <input type="text"
                                        className="form-wizard-fields__input"
                                        value={entityName}
                                        disabled={disabledFields}
                                    />
                                </div>
                            </div>

                            <div className="wrapper-wizard-form--col">
                                <div className="form-wizard-fields">
                                    <label>*
                                        <Translate id="wizard.step-3.code" options={{ renderInnerHtml: true }} />
                                    </label>
                                    <input type="text"
                                        className="form-wizard-fields__input"
                                        value={entityCode}
                                        disabled={disabledFields}
                                    />
                                </div>
                            </div>

                            <hr />
                        </div>
                    </div>

                    {entityName && <div className="col txt-right">
                        <button type='button' className="btn-default button-primary-custom" onClick={handleOpenModalAddDetails}>
                            <Translate id="school-academics-details.button-add" />
                        </button>
                    </div>}

                    {details.length > 0 && <Row className="clear-style-card td-responsive">
                        <Col xs={12}>
                            <Card>
                                <CardBody>
                                    <Form>
                                        <AcademicDetailsTable details={details} updateParent={handleSelectedItemTable} buttonTxt={"edit"} />
                                    </Form>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>}
                </div>
            </div>
            {modalVisible ?
                <ModalSchoolEntityFilter
                    visibleRelationEntity={modalVisible}
                    entityTypeId={typeEntity === Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID ? Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID : Settings.SCHOOL_MODULE_PROGRAM_TYPE_ID}
                    updateParent={handleSelected} /> : null
            }
            {modalVisibleAddDetails ?
                <ModalAddDetails visible={modalVisibleAddDetails} updateParent={handleToggleModalAddDetails} entityId={entityId} entityAcademicDetail={entityAcademicDetail} /> : null
            }
            {loading ? <LoadingFullWindow /> : null}
        </>
    );
};

export default withRouter(withLocalize(AcademicDetails));