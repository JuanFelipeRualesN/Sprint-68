import React, { useState, useEffect, useContext } from 'react';
import { Card, Col, Row, CardBody } from "reactstrap";
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import { Redirect } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faExclamationTriangle,
  faEye,
  faEyeSlash,
  faArrowLeft,
} from "@fortawesome/free-solid-svg-icons";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import ModalUpdateUserDetails from "../modals/modal-update-user-details";
import FormSchoolEntities from "../forms/schools/form-school-entities";
import {
  getAcademicsDetailsByEntity,
  getEntitiesSchoolDetails,
} from "../../../api/schools";
import SchoolDataTable from "../datatables/entity-school-data-table";
import AddressDataTable from "../datatables/entity-school-data-table";
import PhoneDataTable from "../datatables/entity-school-data-table";
import UsersDataTable from "../../shared-module-components/datatables/users-data-table";
import SubscriptionDataTable from "../datatables/subscription-data-table";
import ModalSchoolAddress from "../modals/school-module/modal-address";
import ModalSchoolPhone from "../modals/school-module/modal-phone";
import ModalSchoolEntity from "../modals/school-module/modal-school-entity";
import ModalSubscription from "../modals/modal-subscription";
import Settings from "../../../custom-classes/settings";
import { isRoleAllowed } from "../../../utils/utils";
import SessionManager from "../../../custom-classes/session-manager";
import { Link } from "react-router-dom";
import AcademicDetailsTable from "../datatables/academic-details-table";
import ModalAddDetails from "../modals/modal-add-details";
import {
  saveEntitySchool,
  saveEntitySchoolAddress,
} from "../../../api/schools";
import { AppContext } from "../../../App.Context";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the user details
 */
const SchoolEntityManage = ({location}) => {

  const [academicDetails, setAcademicDetails] = useState([]);
  const [addressColumns, setAddressColumns] = useState(null);
  const [addressData, setAddressData] = useState(null);
  const [entityCode, setEntityCode] = useState("");
  const [addNew] = useState(location.addNew);
  const [entityId, setEntityId] = useState(() => {
    const params = new URLSearchParams(location.search);
    const urlEntityId = params.get('entityId');
    // If entityId is found in the URL, use it; otherwise, fall back to SessionManager
    return urlEntityId !== null ? urlEntityId : null
  });
  const [entityName, setEntityName] = useState("");
  const [entityRelationId, setEntityRelationId] = useState(null);
  const [entityType, setEntityType] = useState("1");
  const [entityTypeToOpenModal, setEntityTypeToOpenModal] = useState(null);
  const [hideProfiles, setHideProfiles] = useState(true);
  const [isAddressEdit, setIsAddressEdit] = useState(false);
  const [isRelationDelete, setIsRelationDelete] = useState(false);
  const [loading, setLoading] = useState(false);
  const [phoneColumns, setPhoneColumns] = useState(null);
  const [phoneData, setPhoneData] = useState(null);
  const [privateOrPublic, setPrivateOrPublic] = useState(0);
  const [programColumns, setProgramColumns] = useState(null);
  const [programData, setProgramData] = useState(null);
  const [propsModal, setPropsModal] = useState({ bodyText: "", visible: false });
  const [redoSearchSubscription, setRedoSearchSubscription] = useState(false);
  const [schoolColumns, setSchoolColumns] = useState(null);
  const [schoolData, setSchoolData] = useState(null);
  const [showEditModalSubscription, setShowEditModalSubscription] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [showModalAddEntityRelation, setShowModalAddEntityRelation] = useState(false);
  const [showModalAddress, setShowModalAddress] = useState(false);
  const [showModalPhone, setShowModalPhone] = useState(false);
  const [showUserModal, setShowUserModal] = useState(false);
  const [subscriptionsIds, setSubscriptionsIds] = useState(null);
  const [translateAddress] = useState(<Translate id="school-module.address" />);
  const [translateCity] = useState(<Translate id="school-module.city" />);
  const [translateCountry] = useState(<Translate id="school-module.country" />);
  const [translateNameAddress] = useState(<Translate id="school-module.address-name" />);
  const [translatePhone] = useState(<Translate id="school-module.phone" />);
  const [translateSchool] = useState(<Translate id="school-module.schools" />);
  const [translateState] = useState(<Translate id="school-module.state" />);
  const [translateZipCode] = useState(<Translate id="school-module.zip-code" />);
  const [entityAcademicDetail, setEntityAcademicDetail] = useState(null);
  const [addressId, setAddressId] = useState(null);
  const [addressLine1, setAddressLine1] = useState("");
  const [addressLine2, setAddressLine2] = useState("");
  const [city, setCity] = useState("");
  const [state, setState] = useState("");
  const [country, setCountry] = useState("");
  const [region, setRegion] = useState("");
  const [zip, setZip] = useState("");
  const [isPhoneEdit, setIsPhoneEdit] = useState(false);
  const [phoneId, setPhoneId] = useState(null);
  const [phone, setPhone] = useState("");
  const [userId, setUserId] = useState(null);
  const [subscriptionId, setSubscriptionId] = useState(null);
  const [planId, setPlanId] = useState(null);
  const [modalVisibleAddDetails, setModalVisibleAddDetails] = useState(false);
  const [redoSearchStudents, setRedoSearchStudents] = useState(false);

  const sessionDetails = SessionManager.getSessionDetails();
  const context = useContext(AppContext);

  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {
    if (typeof entityId !== "undefined" && entityId !== null && !addNew) {
        // Call the details
        handleReadDetails();
        getAcademicsDetails();
    } else {
        setAddressColumns(null);
        setAddressData(null);
        setEntityName("");
        setPhoneColumns(null);
        setPhoneData(null);
        setSchoolColumns(null);
        setSchoolData(null);
    }
  }, []);

  const toggleProfilesDataTable = () => {
    setHideProfiles(!hideProfiles);
  };

  const toggleModalPhone = () => {
    setIsPhoneEdit(false);
    setShowModalPhone(true);
    setShowModalAddress(false);
    setShowModalAddEntityRelation(false);
  };

  const toggleModalAddress = () => {
    setIsAddressEdit(false);
    setShowModalPhone(false);
    setShowModalAddress(true);
    setShowModalAddEntityRelation(false);
    setAddressId(null);
    setAddressLine1("");
    setAddressLine2("");
    setCity("");
    setState("");
    setCountry("");
    setRegion("");
    setZip("");
  };

  const toggleUpdateModalSubscription = (e) => {
    setShowEditModalSubscription(e);
    setRedoSearchSubscription(true);
  };

  const toggleModalEntitiesSchools = () => {
    setEntityRelationId(null);
    setEntityTypeToOpenModal(Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID);
    setIsAddressEdit(false);
    setIsRelationDelete(false);
    setShowModalAddEntityRelation(true);
    setShowModalAddress(false);
    setShowModalPhone(false);
  };

  const toggleModalProgram = () => {
    setEntityRelationId(null);
    setEntityTypeToOpenModal(Settings.SCHOOL_MODULE_PROGRAM_TYPE_ID);
    setIsAddressEdit(false);
    setIsRelationDelete(false);
    setRedoSearchStudents(false);
    setShowModalAddEntityRelation(true);
    setShowModalAddress(false);
    setShowModalPhone(false);
  };

  // Toggle the update plan modal
  const toggleUpdateModalStudents = (e) => {
    setShowUserModal(e);
    setRedoSearchStudents(true);
  };

  // Receives the entity details after a save
  const handleEntityBasicDetailsSave = async (detail) => {
    // if (!addressData || (addressData && !addressData.length)) {
    if (!addressData) {
      toggleModalAddress();
      setEntityName(detail.entityName);
      setEntityId(detail.entityId);
      setEntityType(detail.entityType);
      setEntityCode(detail.entityCode);
      setPrivateOrPublic(detail.privateOrPublic);
    } else {
      saveEntity(detail);
    }
  };

  const saveEntity = async (detail) => {
    setLoading(true);
    try {
      const response = await saveEntitySchool(
        detail.entityName,
        detail.entityId,
        detail.entityType,
        detail.entityCode,
        detail.privateOrPublic
      );
      const e = response.data[0];
      setEntityId(e.Id);
      setEntityType(e.EntityTypeId);
      setEntityName(e.Name);
      setEntityCode(e.Token);
      setPrivateOrPublic(e.Private);
      setLoading(false);
      SessionManager.setLastEntityId(e.Id);
      return e.Id;
    } catch (error) {
      setLoading(false);
    }
  };

  const saveDetailAddress = async (detailAddress) => {
    if (!entityId) {
      const detail = {
        entityName: entityName,
        entityId: entityId,
        entityType: entityType,
        entityCode: entityCode,
        privateOrPublic: privateOrPublic
      };
      const newEntityId = await saveEntity(detail);
      console.log("New entity Id", newEntityId);
      setEntityId(newEntityId);
      const reqAddress = {
        ...detailAddress,
        entityName: entityName,
        entityId: newEntityId,        
      };
      await saveAddress(reqAddress); 
    } else {
      saveAddress(detailAddress);
    }
  };

  const saveAddress = async (detailAddress) => {

    setLoading(true);
    try {
      const response = await saveEntitySchoolAddress(
        detailAddress.entityName,
        detailAddress.entityId,
        detailAddress.addressLine1,
        detailAddress.addressLine2,
        detailAddress.city,
        detailAddress.state,
        detailAddress.country,
        detailAddress.zip,
        detailAddress.addressId,
        detailAddress.region
      );
      let messageStr = "";
      for (let i = 0; i < response.data.Messages.length; i++) {
        messageStr += response.data.Messages[i] + " ";
      }
      if(entityId === null){
        setEntityId(detailAddress.entityId);
      }
      reloadData(true);
      setLoading(false);
      setShowModalAddress(false);
      window.location.href = `${window.location.pathname}?entityId=${detailAddress.entityId}`;
    } catch (error) {
      setLoading(false);
    }
  };

  // Toggles alert modal
  const toggleAlertModal = () => {
    setShowModal(false);
  };

  const reloadData = (e) => {
    if (e) {
      handleReadDetails();
    }
    setShowModalAddress(false);
    setShowModalPhone(false);
    setShowModalAddEntityRelation(false);
  };

  const getAcademicsDetails = async (page, pageSize) => {
    setLoading(true);
    try {
      const res = await getAcademicsDetailsByEntity(
        entityId,
        page,
        pageSize
      );
      const data = res.data;
      setLoading(false);
      setAcademicDetails(data.EntityAcademicDetailModels);
    } catch {
      setLoading(false);
    }
  };

  const handleReadDetails = () => {
    let apiCalls = getEntitiesSchoolDetails(entityId);
    apiCalls.then((response) => {
      setEntityName(response.data.Entities[0].Name);
      setEntityCode(response.data.Entities[0].Token);
      setPrivateOrPublic(response.data.Entities[0].Private);

      // Set columns
      setProgramColumns([
        {
          name: "Id",
          selector: "Id",
          sortable: true,
        },
        {
          name: translateSchool,
          selector: "Name",
          sortable: true,
        },
      ]);
      setSchoolColumns([
        {
          name: "Id",
          selector: "Id",
          sortable: true,
        },
        {
          name: translateSchool,
          selector: "Name",
          sortable: true,
        },
      ]);
      setAddressColumns([
        {
          name: translateNameAddress,
          selector: "NameAddress",
          sortable: true,
        },
        {
          name: translateAddress,
          selector: "AddressLine1",
          sortable: false,
        },
        {
          name: "",
          selector: "AddressLine2",
          sortable: false,
        },
        {
          name: translateCity,
          selector: "City",
          sortable: true,
        },
        {
          name: translateState,
          selector: "State",
          sortable: true,
        },
        {
          name: translateCountry,
          selector: "Country",
          sortable: true,
        },
        {
          name: translateZipCode,
          selector: "Zip",
          sortable: true,
        },
      ]);
      setPhoneColumns([
        {
          name: translatePhone,
          selector: "Phone",
          sortable: true,
        },
      ]);
      setPhoneData(
        response.data.Phones !== null && response.data.Phones.length > 0
          ? response.data.Phones
          : []
      );
      setProgramData(
        response.data.Programs !== null && response.data.Programs.length > 0
          ? response.data.Programs
          : []
      );
      setSchoolData(
        response.data.Schools !== null && response.data.Schools.length > 0
          ? response.data.Schools
          : []
      );
      setAddressData(
        response.data.Addresses !== null && response.data.Addresses.length > 0
          ? response.data.Addresses
          : []
      );

      if (
        response.data.Subscriptions !== null &&
        response.data.Subscriptions.length > 0
      ) {
        let arraySubscriptionsIds = [];
        for (let i = 0; i < response.data.Subscriptions.length; i++) {
          arraySubscriptionsIds.push(
            response.data.Subscriptions[i].SubscriptionId
          );
        }
        setSubscriptionsIds(arraySubscriptionsIds);
      }
    });
  };

  const handleSelectedSchoolRowAddress = (e) => {
    setAddressId(e.Id);
    setAddressLine1(e.AddressLine1);
    setAddressLine2(e.AddressLine2);
    setCity(e.City);
    setState(e.State);
    setCountry(e.Country);
    setRegion(e.RegionId);
    setZip(e.Zip);
    setIsAddressEdit(true);
    setShowModalAddress(true);
    setShowModalPhone(false);
  };

  const handleUserSelection = (e) => {
    setUserId(e.UserId);
    setShowUserModal(true);
  };

  const handleSubscriptionSelection = (e) => {
    setSubscriptionId(e.Id);
    setPlanId(e.Plan.Id);
    setUserId(e.UserId);
    setShowEditModalSubscription(true);
  };

  const handleToggleModalAddDetails = () => {
    setModalVisibleAddDetails(false);
    getAcademicsDetails();
  };

  // Handles the selected relation
  const handleSelectedRelation = (e) => {
    setEntityRelationId(e.Id);
    setIsRelationDelete(true);
    setShowModalPhone(false);
    setShowModalAddress(false);
    setShowModalAddEntityRelation(true);
  };

  const handleSelectedSchoolRowPhone = (e) => {
    setPhoneId(e.Id);
    setPhone(e.Phone);
    setIsPhoneEdit(true);
    setShowModalPhone(true);
    setShowModalAddress(false);
  };

  const handleOpenModalAddDetails = () => {
    setModalVisibleAddDetails(true);
    setEntityAcademicDetail(null);
  };

  const handleSelectedItemTable = (item) => {
    setModalVisibleAddDetails(true);
    setEntityAcademicDetail(item);
  };

    if ((typeof entityType == "undefined" || entityType == null) && Settings.IS_APP_MANAGED_BY_REQUIRED) return <Redirect to="/school-entity-search" />;

    return (
      <div>
        <div className="container-padding animated delay-05s fadeIn">
          <div className="wrapper-principal-container">
            <div className="col col-no-padding-left">
              <Link to="/school-entity-search" className="btn-back">
                <FontAwesomeIcon icon={faArrowLeft} />{" "}
                <Translate
                  id="general.back-action"
                  options={{ renderInnerHtml: true }}
                />
              </Link>
            </div>
            <div className="mb-3">
              <div className="col-no-padding-left">
                <div className="title-principal-variant mt-3 pb-1">
                  {entityType == "1" ? (
                    <div className="line-title">
                      <Translate id={((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && !context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && !context.state.variables.SchoolManagementAdminOre)) ? "school-module.manage-schools-read" : "school-module.manage-schools"} />
                    </div>
                  ) : null}
                  {entityType == "2" ? (
                    <div className="line-title">
                      <Translate id="school-module.manage-programs" />
                    </div>
                  ) : null}
                  {entityType == "3" ? (
                    <div className="line-title">
                      <Translate id="school-module.manage-institutions" />
                    </div>
                  ) : null}
                  {entityType == "4" ? (
                    <div className="line-title">
                      <Translate id="school-module.manage-companies" />
                    </div>
                  ) : null}
                </div>
              </div>
            </div>

            <Row className="clear-style-card td-responsive">
              <Col xs={12}>
                <div className="wrapper-col borderGray-bottom block-display-responsive">
                  <div className="col col-no-padding-left">
                    <div className="admin-subtitle">
                      <Translate id="school-module.school-manage-step-1" />
                    </div>
                  </div>
                </div>
                <FormSchoolEntities
                  entityId={entityId}
                  entityName={entityName}
                  entityCode={entityCode}
                  entityType={entityType}
                  initialCode={entityCode}
                  privateOrPublic={privateOrPublic}
                  disabled={((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && !context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && !context.state.variables.SchoolManagementAdminOre))}
                  updateParent={handleEntityBasicDetailsSave}
                />
              </Col>
            </Row>
            {(entityType == Settings.SCHOOL_MODULE_PROGRAM_TYPE_ID ||
              entityType ==
                Settings.SCHOOL_MODULE_INSTITUTION_TYPE_ID) &&
            typeof entityId !== "undefined" &&
            entityId !== null ? (
              <Row className="clear-style-card td-responsive module-school-step__empty admin-school-table clear-pagination">
                <Col xs={12}>
                  <CardBody>
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                      <div className="col col-no-padding-left">
                        <div className="admin-subtitle">
                          <Translate id="school-module.schools" />
                        </div>
                      </div>
                      <div className="col txt-right">
                        <button
                          className="btn-default button-primary-custom"
                          onClick={toggleModalEntitiesSchools}
                        >
                          <Translate id="school-module.add-relation-school" />
                        </button>
                      </div>
                    </div>

                    {schoolData !== null ? (
                      <SchoolDataTable
                        columns={schoolColumns}
                        data={schoolData}
                        title={translateSchool}
                        updateParent={handleSelectedRelation}
                      />
                    ) : null}
                  </CardBody>
                </Col>
              </Row>
            ) : null}


              <Row className="clear-style-card td-responsive module-school-step admin-school-table clear-pagination">


                <Col xs={12}>
                  <CardBody>
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                      <div className="col col-no-padding-left">
                        <div className="admin-subtitle">
                          <Translate id="school-module.school-manage-step-2" />
                        </div>
                      </div>
                      <div className="col txt-right">
                      {
                          entityId && isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdmin ?
                            <button
                              className="btn-default button-primary-custom"
                              onClick={toggleModalAddress}
                            >
                              <Translate id="school-module.add-address" />
                            </button> : null
                        }
                        {
                          entityId && isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdminOre ?
                            <button
                              className="btn-default button-primary-custom"
                              onClick={toggleModalAddress}
                            >
                              <Translate id="school-module.add-address" />
                            </button> : null
                        }
                      </div>
                    </div>
                    {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdminOre)) && <div className="wrapper-col block-display-responsive">
                      <p
                        className="mt-4 alert-message"
                        style={{ width: "100%" }}
                      >
                        <small>
                          <FontAwesomeIcon icon={faExclamationTriangle} />{" "}
                          <Translate
                            id="school-module.address-instructions"
                            options={{ renderInnerHtml: true }}
                          />
                        </small>
                      </p>
                    </div>}
                    {isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && <>
                      {addressData !== null &&
                        addressData.length > 0 ? (
                        <AddressDataTable
                          columns={addressColumns}
                          data={addressData}
                          updateParent={context.state.variables.SchoolManagementAdmin && handleSelectedSchoolRowAddress}
                        />
                      ) : null}
                    </>}
                    {isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && <>
                      {addressData !== null &&
                        addressData.length > 0 ? (
                        <AddressDataTable
                          columns={addressColumns}
                          data={addressData}
                          updateParent={context.state.variables.SchoolManagementAdminOre && handleSelectedSchoolRowAddress}
                        />
                      ) : null}
                    </>}
                  </CardBody>
                </Col>

              </Row>

            {addressData !== null &&
              addressData.length > 0 && (
                <div>
                    <Row className="clear-style-card td-responsive module-school-step__empty admin-school-table clear-pagination">
                      <Col xs={12}>
                        <Card>
                          <CardBody>
                            <div className="wrapper-col borderGray-bottom block-display-responsive">
                              <div className="col col-no-padding-left">
                                <div className="admin-subtitle">
                                  <Translate id="school-module.school-manage-step-3" />
                                </div>
                              </div>
                              {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdminOre)) && <div className="col txt-right">
                                <button
                                  type="button"
                                  className="btn-default button-primary-custom"
                                  onClick={handleOpenModalAddDetails}
                                >
                                  <Translate id="school-academics-details.button-add" />
                                </button>
                              </div>}                              
                            </div>
                            {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdminOre)) &&  <div className="wrapper-col block-display-responsive">
                              <p
                                className="mt-4 alert-message"
                                style={{ width: "100%" }}
                              >
                                <small>
                                  <FontAwesomeIcon
                                    icon={faExclamationTriangle}
                                  />{" "}
                                  <Translate
                                    id="school-module.academic-periods-note"
                                    options={{ renderInnerHtml: true }}
                                  />
                                </small>
                              </p>
                            </div>}

                            {academicDetails != null &&
                            academicDetails.length > 0 ? (
                              <AcademicDetailsTable
                                details={academicDetails}
                                updateParent={handleSelectedItemTable}
                                buttonTxt={((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdminOre)) ? "edit" : "see"} 
                              />
                            ) : null}
                          </CardBody>
                        </Card>
                      </Col>
                    </Row>
                    <Row className="clear-style-card td-responsive module-school-step admin-school-table clear-bg-card">
                      <Col xs={12}>
                        <CardBody>
                          <div className="wrapper-col borderGray-bottom block-display-responsive">
                            <div className="col col-no-padding-left">
                              <div className="admin-subtitle">
                                <Translate id="school-module.school-manage-step-4" />
                              </div>
                            </div>
                            {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.EnrollmentManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.EnrollmentManagementAdminOre)) && <div className="col txt-right">
                              <Link
                                className="btn-default button-primary-custom"
                                to={`/school-subscription?entityId=${entityId}`}
                              >
                                <Translate id="subscriptions.add-subscription" />
                              </Link>
                            </div>}
                          </div>

                          {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdminOre)) && <div className="wrapper-col block-display-responsive">
                            <p
                              className="mt-4 alert-message"
                              style={{ width: "100%" }}
                            >
                              <small>
                                <FontAwesomeIcon icon={faExclamationTriangle} />{" "}
                                <Translate
                                  id="school-module.enrollments-note"
                                  options={{ renderInnerHtml: true }}
                                />
                              </small>
                            </p>
                          </div>}
                          {
                          typeof subscriptionsIds !== "undefined" &&
                          subscriptionsIds !== null &&
                          subscriptionsIds.length > 0 ? (
                            <Card>
                              <CardBody>
                                {subscriptionsIds !== null ? (
                                  <SubscriptionDataTable
                                    redoSearch={redoSearchSubscription}
                                    updateParent={
                                      handleSubscriptionSelection
                                    }
                                    subscriptionsIds={
                                      subscriptionsIds
                                    }
                                  />
                                ) : null}
                              </CardBody>
                            </Card>
                          ) : null}
                        </CardBody>
                      </Col>
                    </Row>
                  {
                  typeof entityId !== "undefined" &&
                  entityId !== null ? (
                    <Row className="clear-style-card td-responsive admin-school-table module-profiles-school">
                      <>
                        <Col xs={12}>
                          <CardBody>
                            <div className="borderGray-bottom">
                              <div className="col-no-padding-left">
                                <div className="admin-subtitle--instructions">
                                  <Translate id="school-module.profiles" />:
                                  <span>
                                  <Translate id={((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && !context.state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && !context.state.variables.SchoolManagementAdminOre)) ? "school-module.profiles-instructions-general-read" : "school-module.profiles-instructions-general"} />
                                  </span>
                                </div>{" "}
                                <button
                                  className="btn-default button-secondary-custom"
                                  onClick={toggleProfilesDataTable}
                                >
                                  {hideProfiles ? (
                                    <span>
                                      <FontAwesomeIcon icon={faEye} />{" "}
                                      <Translate id="school-module.show-profiles" />
                                    </span>
                                  ) : (
                                    <span>
                                      <FontAwesomeIcon icon={faEyeSlash} />
                                      <Translate id="school-module.hide-profiles" />
                                    </span>
                                  )}
                                </button>
                              </div>
                            </div>
                            {!hideProfiles ? (
                              <Card>
                                <CardBody>
                                  {entityId !== null ? (
                                    <UsersDataTable
                                      redoSearch={redoSearchStudents}
                                      schoolId={entityId}
                                      updateParent={handleUserSelection}
                                    />
                                  ) : null}
                                </CardBody>
                              </Card>
                            ) : null}
                          </CardBody>
                        </Col>
                      </>
                    </Row>
                  ) : null}


                  {
                  typeof entityId !== "undefined" &&
                  entityId !== null ? (
                    <Row className="clear-style-card td-responsive module-school-step mt-4 clear-header admin-school-table clear-pagination">
                      <Col xs={12}>
                        <CardBody>
                          <div className="wrapper-col borderGray-bottom block-display-responsive">
                            <div className="col col-no-padding-left">
                              <div className="admin-subtitle">
                                <Translate id="school-module.school-manage-optional-1" />
                              </div>
                            </div>
                            {isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdmin && <div className="col txt-right">
                              <button
                                className="btn-default button-primary-custom"
                                onClick={toggleModalPhone}
                              >
                                <Translate id="school-module.add-telephone" />
                              </button>
                            </div>}
                            {isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.SchoolManagementAdminOre && <div className="col txt-right">
                              <button
                                className="btn-default button-primary-custom"
                                onClick={toggleModalPhone}
                              >
                                <Translate id="school-module.add-telephone" />
                              </button>
                            </div>}
                          </div>

                          {isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && <>
                            {phoneData !== null &&
                              phoneData.length > 0 ? (
                              <PhoneDataTable
                                columns={phoneColumns}
                                data={phoneData}
                                updateParent={context.state.variables.SchoolManagementAdmin && handleSelectedSchoolRowPhone}
                              />
                            ) : null}
                          </>}
                          {isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && <>
                            {phoneData !== null &&
                              phoneData.length > 0 ? (
                              <PhoneDataTable
                                columns={phoneColumns}
                                data={phoneData}
                                updateParent={context.state.variables.SchoolManagementAdminOre && handleSelectedSchoolRowPhone}
                              />
                            ) : null}
                          </>}
                        </CardBody>
                      </Col>
                    </Row>
                  ) : null}
                </div>
              )}
          </div>
        </div>
        {loading ? <LoadingFullWindow /> : null}
        {showModal ? (
          <ModalGeneric
            data={propsModal}
            updateParent={toggleAlertModal}
          />
        ) : null}
        {showModalAddress ? (
          <ModalSchoolAddress
            visibleAddress={showModalAddress}
            entityId={entityId}
            nameAddress={entityName}
            addressLine1={addressLine1}
            addressLine2={addressLine2}
            addressId={addressId}
            country={country}
            city={city}
            state={state}
            zip={zip}
            region={region}
            isAddressEdit={isAddressEdit}
            updateParent={reloadData}
            saveDetailAddress={saveDetailAddress}
          />
        ) : null}
        {showModalPhone ? (
          <ModalSchoolPhone
            visiblePhone={showModalPhone}
            entityId={entityId}
            phone={phone}
            phoneId={phoneId}
            isEdit={isPhoneEdit}
            updateParent={reloadData}
          />
        ) : null}
        {showModalAddEntityRelation ? (
          <ModalSchoolEntity
            entityRelationId={entityRelationId}
            visibleRelationEntity={showModalAddEntityRelation}
            entityId={entityId}
            entityTypeId={entityTypeToOpenModal}
            updateParent={reloadData}
            isDelete={isRelationDelete}
          />
        ) : null}
        {showUserModal ? (
          <ModalUpdateUserDetails
            userId={userId}
            isEdit={true}
            visible={showUserModal}
            updateParent={toggleUpdateModalStudents}
          />
        ) : null}
        {showEditModalSubscription ? (
          <ModalSubscription
            subscriptionId={subscriptionId}
            planId={planId}
            userId={userId}
            isEdit={true}
            isReadOnly={false}
            visible={showEditModalSubscription}
            updateParent={toggleUpdateModalSubscription}
          />
        ) : null}
        {modalVisibleAddDetails ? (
          <ModalAddDetails
            visible={modalVisibleAddDetails}
            updateParent={handleToggleModalAddDetails}
            entityId={entityId}
            entityAcademicDetail={entityAcademicDetail}
          />
        ) : null}
      </div>
    );
};


export default withLocalize(SchoolEntityManage);
