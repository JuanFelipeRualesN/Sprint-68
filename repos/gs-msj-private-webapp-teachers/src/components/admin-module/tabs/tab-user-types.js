import React, {useCallback, useContext, useEffect, useState} from 'react';
import {
  TabContent,
  TabPane,
  Nav,
  NavItem,
  NavLink,
  Card,
  Row,
  Col,
  Form, ModalHeader, Modal, ModalBody, ModalFooter,
} from "reactstrap";

import { Translate } from "react-localize-redux";
import FileUploader from "../fileuploader/drop-zone";
import SessionManager from "../../../custom-classes/session-manager";
import ProfilesDataTable from "../../shared-module-components/datatables/profiles-data-table";
import ModalUpdateUserDetails from "../modals/modal-update-user-details";
import ModalAttachProfileByEmployee from "../modals/modal-attach-profile-by-employee";
import classNames from "classnames";
import Settings from "../../../custom-classes/settings";
import { isRoleAllowed } from "../../../utils/utils";
import { AppContext } from "../../../App.Context";
import {removeProfileFromSubscription} from "../../../api/subscriptions";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";

const TabUserTypes = ({ addRemoveButton = false, roleId: propRoleId, schoolId: propSchoolId, showTeachers: propShowTeachers, subscriptionId: propSubscriptionId }) => {
  const context = useContext(AppContext);

  const [activeTab, setActiveTab] = useState("1");
  const [redoSearch, setRedoSearch] = useState(true);
  const [roleId, setRoleId] = useState(propRoleId !== undefined && propRoleId != null && propRoleId !== "" ? propRoleId : null);
  const [schoolId, setSchoolId] = useState(propSchoolId !== undefined && propSchoolId != null && propSchoolId !== "" ? propSchoolId : null);
  const [showModalAttachProfiles, setShowModalAttachProfiles] = useState(false);
  const [showTeachers, setShowTeachers] = useState(propShowTeachers);
  const [showUserModal, setShowUserModal] = useState(false);
  const [subscriptionId, setSubscriptionId] = useState(propSubscriptionId);
  const [profileId, setProfileId] = useState(null);
  const [showModalRemoveUser, setShowModalRemoveUser] = useState(false);
  const [userToRemove, setUserToRemove] = useState(null);
  const [refreshKey, setRefreshKey] = useState(0); // Initialize key to 0
    const [isLoading, setIsLoading] = useState(false);


  // Force redo search by incrementing the key
  const forceRefreshDataTable = useCallback(() => {
    setRefreshKey(prevKey => prevKey + 1);
  }, []);

  // Hide or shows the window (for tabs)
  const toggle = useCallback((tabNo, newRoleId) => {
    setActiveTab(tabNo);
    setRoleId(newRoleId);
    forceRefreshDataTable(); // Force re-search when tab/role changes
  }, [forceRefreshDataTable]);

  // Force redo.
  const redoSearchAction = () => {
    setRedoSearch(false);
    setTimeout(() => {
      setRedoSearch(true);
    }, 0);
  };

  const handleExistingAccountSelected = (e) => {
    setProfileId(e.UserId);
    setShowUserModal(true);
  };

  const toggleUpdateModal = useCallback((e) => {
    setShowUserModal(e); // 'e' is typically boolean: true to show, false to hide
    if (!e) { // If modal is being closed
      forceRefreshDataTable(); // Force re-search after user update
    }
  }, [forceRefreshDataTable]);

  const showModalAttachProfilesHandler = () => {
    setShowModalAttachProfiles(true);
  };

  const closeModalAttachProfiles = useCallback(() => {
    setShowModalAttachProfiles(false);
    forceRefreshDataTable(); // Force re-search after attaching profiles
  }, [forceRefreshDataTable]);

  const removeUserShowModal = useCallback((e)=>{
    setUserToRemove(e);
    setShowModalRemoveUser(true);
  }, []);

  const onConfirmRemove = useCallback(async () => {
    setIsLoading(true); // Start loading

    try {

      await removeProfileFromSubscription(userToRemove.UserId, subscriptionId);

      // If successful:
      setShowModalRemoveUser(false); // Close the modal
      setUserToRemove(null);       // Clear user to remove
      forceRefreshDataTable();     // Force re-search after removal

    } catch (error) {
      // Handle any errors that occur during the removal process
      console.error("Error removing profile from subscription:", error);
    } finally {
      setIsLoading(false); // Always stop loading, regardless of success or failure
    }
  }, [userToRemove, subscriptionId, forceRefreshDataTable]);

  const renderProfiles = () => {
    const sessionDetails = SessionManager.getSessionDetails();
    return (
      <div>
        {roleId !== Settings.ROLE_DIRECTOR_ID &&
          typeof schoolId !== "undefined" &&
          schoolId != null &&
          schoolId !== "" ? (
          <Row>
            {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.EnrollmentManagementAdmin) ||
              (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.EnrollmentManagementAdminOre)) && (
                <Col xs={12}>
                  <Card body>
                    <Form>
                      <h2>
                        <b>
                          <Translate id="plan-creator.upload-users" />
                        </b>
                      </h2>
                      <hr />
                      <FileUploader
                        schoolId={schoolId}
                        subscriptionId={subscriptionId}
                        session={SessionManager.getSession()}
                        roleId={roleId}
                        updateParent={redoSearchAction}
                      />
                    </Form>
                  </Card>
                </Col>
              )}
          </Row>
        ) : null}
        <Row style={{ marginTop: 30 }}>
          <Col xs={12}>
            <b>
              <Translate id="subscriptions.attached-profiles" />
            </b>{" "}
            {subscriptionId &&
              ((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && context.state.variables.EnrollmentManagementAdmin) ||
                (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && context.state.variables.EnrollmentManagementAdminOre)) ? (
              <button className="btn-default button-secondary-custom" onClick={showModalAttachProfilesHandler}>
                <Translate id="subscriptions.see-profiles" />
              </button>
            ) : null}
          </Col>
          <Col xs={12}>
            {roleId !== null && redoSearch ? (
              <ProfilesDataTable
                  key={refreshKey}
                  addRemoveButton={addRemoveButton}
                  refreshKey={refreshKey}
                isStaff={true}
                roleId={roleId}
                subscriptionId={subscriptionId}
                redoSearch={redoSearch}
                  removeUser={removeUserShowModal}
                updateParent={handleExistingAccountSelected}
              />
            ) : null}
          </Col>
        </Row>
      </div>
    );
  };

  useEffect(() => {
    toggle("1", Settings.ROLE_STUDENT_ID);
  }, []);

  useEffect(() => {
    setSchoolId(
      propSchoolId !== undefined && propSchoolId != null && propSchoolId !== ""
        ? propSchoolId
        : null
    );
    setShowTeachers(propShowTeachers);
    setSubscriptionId(propSubscriptionId);
  }, [propSchoolId, propShowTeachers, propSubscriptionId]);

  return (
    <div>
      <Nav tabs>
        <NavItem>
          <NavLink
            className={classNames({ active: activeTab === "1" })}
            onClick={() => {
              toggle("1", Settings.ROLE_STUDENT_ID);
            }}
          >
            <Translate id="user-management.students" />
          </NavLink>
        </NavItem>
        {showTeachers ? (
          <NavItem>
            <NavLink
              className={classNames({ active: activeTab === "2" })}
              onClick={() => {
                toggle("2", Settings.ROLE_TEACHER_ID);
              }}
            >
              <Translate id="user-management.teachers" />
            </NavLink>
          </NavItem>
        ) : null}
        {showTeachers ? (
          <NavItem>
            <NavLink
              className={classNames({ active: activeTab === "3" })}
              onClick={() => {
                toggle("3", Settings.ROLE_DIRECTOR_ID);
              }}
            >
              <Translate id="user-management.principal" />
            </NavLink>
          </NavItem>
        ) : null}
      </Nav>
      <TabContent activeTab={activeTab}>
        <TabPane tabId="1">
          {activeTab === "1" ? renderProfiles() : null}
        </TabPane>
        <TabPane tabId="2">
          {activeTab === "2" ? renderProfiles() : null}
        </TabPane>
        <TabPane tabId="3">
          {activeTab === "3" ? renderProfiles() : null}
        </TabPane>
      </TabContent>
      {showUserModal ? (
        <ModalUpdateUserDetails
          userId={profileId}
          isEdit={true}
          visible={showUserModal}
          updateParent={toggleUpdateModal}
        />
      ) : null}
      {showModalAttachProfiles ? (
        <ModalAttachProfileByEmployee
          schoolId={schoolId}
          roleId={roleId}
          visible={showModalAttachProfiles}
          subscriptionId={subscriptionId}
          updateParent={closeModalAttachProfiles}
        />
      ) : null}
      {showModalRemoveUser && userToRemove != null ?
          (<Modal isOpen={showModalRemoveUser} toggle={()=>{setShowModalRemoveUser(!showModalRemoveUser)}} centered>
            <ModalHeader toggle={()=>{setShowModalRemoveUser(!showModalRemoveUser)}} >
              <Translate id="users-datatable.remove-profile-title" defaultMessage="Remove Profile" />
            </ModalHeader>

            <ModalBody>
              <p>
                <Translate
                    id="users-datatable.remove-profile-warning"
                    defaultMessage={`Are you sure you want to remove the profile "${userToRemove?.GivenName} ${userToRemove?.Surname}" from the subscription? This action cannot be undone.`}
                />
              </p>
              <p>
                {userToRemove?.GivenName} {userToRemove?.Surname}
              </p>
            </ModalBody>

            <ModalFooter>
              <button className="btn-info" onClick={()=>{setShowModalRemoveUser(!showModalRemoveUser)}}>
                <Translate id="users-datatable.cancel" defaultMessage="Cancel" />
              </button>
              <button className="btn-info btn-danger" onClick={() => onConfirmRemove(userToRemove)}>
                <Translate id="users-datatable.confirm" defaultMessage="Remove" />
              </button>
            </ModalFooter>
          </Modal> ) : null}

      {isLoading ? <LoadingFullWindow /> : null}
    </div>
  );
};

export default TabUserTypes;
