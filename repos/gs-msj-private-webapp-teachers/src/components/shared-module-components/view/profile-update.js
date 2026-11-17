import React, { useState, useEffect, useContext } from 'react'
import { CardBody, Col, Button, Form, FormGroup, CardHeader, Row } from 'reactstrap';
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import { Translate } from "react-localize-redux";
import FormGroupNewPassword from '../form/form-group-new-password';
import FormGroupName from '../form/form-group-name';
import ModalGeneric from '../modal/modal-generic';
import moment from 'moment';
import momentTimezone from 'moment-timezone';
import { getTimeZone } from "../../../utils/utils";
import { updateProfileInformation, updateProfilePassword, getProfileInformation } from "../../../api/users";
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import ProfilesDataTable from '../datatables/profiles-data-table';
import {
    ProfilesDataTableWithAlerts,
    ProfileSummaryCard,
    PromptSafetyAlert,
    TutorPromptHistory,
    usePromptMonitoring,
} from '../profiles-prompt-alerts';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUserPlus, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import ModalSendInvitationCode from "../modal/modal-send-invitation-code";
import UserSchoolsDataTable from "../../school-module/datatables/user-school-data-table";
import "./style.css";
import FormGroupEmail from "../form/form-group-email";
import { AppContext } from "../../../App.Context";
import LoaderFullWindow from "../loader-windows/loader-full-window";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
* The component (window) to update a profile information
*/
const ProfileUpdate = ({ roleId, withPromptAlerts }) => {
    const context = useContext(AppContext);
    const sessionDetails = SessionManager.getSessionDetails();

    // Prompt monitoring is only meaningful for staff looking at a student, so
    // the alert-aware table and the history block are gated on both the flag
    // and the viewer's role.
    const showPromptAlerts =
        withPromptAlerts &&
        isRoleAllowed(
            [Settings.ROLE_TEACHER_STR, Settings.ROLE_ADMIN_STR],
            sessionDetails ? sessionDetails.Roles : []
        );

    const [loading, setLoading] = useState(false);
    const [state, setState] = useState({
        calendarFocused: false,
        createdAt: moment(),
        dob: null,
        email: '',
        emailAvailable: true,
        firstName: '',
        isStaff: false,
        lastName: '',
        password: '',
        passwordRetype: '',
        proceedUpdateInformation: false,
        proceedUpdatePassword: false,
        propsModal: null,
        redoSearch: false,
        rendered: false,
        roles: SessionManager.getSessionDetails()?.Roles || [],
        sex: '',
        showErrorModal: false,
        studentFullName: null,
        studentObj: null,
        username: '',
        usernameAvailable: true,
        validFirstName: true,
        validLastName: true,
        validPassword: false,
        validUsername: false,
        visibleModalInvitationCode: false,
        validEmail: false,
    });
    
    // Initialize translations based on the current version
    useVersionedTranslations();

    const toggleAlertModal = () => setState(prev => ({ ...prev, showErrorModal: false }));
    const handleModalSendRequest = () => setState(prev => ({ ...prev, visibleModalInvitationCode: true, redoSearch: false }));
    const hideModalInvitationCode = () => setState(prev => ({ ...prev, visibleModalInvitationCode: false, redoSearch: true }));

    // Formats the UTC last session returned by the API into the viewer's time zone
    const formatLastSession = (lastSession) =>
        lastSession
            ? momentTimezone
                .utc(lastSession, "MM/DD/YYYY HH:mm:ss")
                .tz(getTimeZone())
                .format("MM/DD/YYYY hh:mm a")
            : "";

    const updatePassword = async () => {
        setLoading(true);
        try {
            const response = await updateProfilePassword(
                state.password,
                state.passwordRetype,
                state.studentObj?.UserId
            );

            setState(prev => ({
                ...prev,
                propsModal: { bodyText: response.data.Messages[0], visible: true },
                showErrorModal: true,
            }));
        } catch (error) {
            console.error("Error updating password:", error);
            setState(prev => ({
                ...prev,
                propsModal: { bodyText: "An error occurred while updating the password.", visible: true },
                showErrorModal: true,
            }));
        } finally {
            setLoading(false);
        }
    };

    const updateStudentInformation = async () => {
        setLoading(true);
        try {
            const response = await updateProfileInformation(
                state.firstName,
                state.lastName,
                state.sex,
                state.dob,
                state.studentObj?.UserId,
                state.email
            );

            setState(prev => ({
                ...prev,
                propsModal: { bodyText: response.data.Messages[0], visible: true },
                showErrorModal: true,
            }));
        } catch (error) {
            console.error("Error updating student information:", error);
            setState(prev => ({
                ...prev,
                propsModal: { bodyText: "An error occurred while updating the student information.", visible: true },
                showErrorModal: true,
            }));
        } finally {
            setLoading(false);
        }
    };

    // Both tables take the same props, so only the component changes.
    const ProfilesTable = showPromptAlerts ? ProfilesDataTableWithAlerts : ProfilesDataTable;

    // AI tutor prompt data for the selected student (banner count, paginated
    // history, date-range filter and period presets). Encapsulated in the hook so
    // this screen stays presentational; it resets itself when the student changes.
    const promptMonitoring = usePromptMonitoring({
        enabled: showPromptAlerts,
        userId: state.studentObj ? state.studentObj.UserId : null,
    });

    const setSubmitPasswordButtonState = () => setState(prev => ({ ...prev, proceedUpdatePassword: prev.validPassword && prev.studentObj !== null }));
    const setUpdateInformationButtonState = () => setState(prev => ({
        ...prev,
        proceedUpdateInformation:
            prev.firstName.length > 0 &&
            prev.lastName.length > 0 &&
            prev.validFirstName &&
            prev.validLastName &&
            prev.studentObj !== null,
    }));

    const saveStudentIdState = async (newValue) => {
        window.scrollTo(0, 0);
        setLoading(true); // Optional: remove if loading isn't required here

        try {
            setState(prev => ({
                ...prev,
                studentObj: newValue,
                studentFullName: `${newValue.GivenName} ${newValue.Surname}`
            }));

            const response = await getProfileInformation(newValue.UserId);

            if ([200, 201].includes(response.status)) {
                setState(prev => ({
                    ...prev,
                    email: response.data.Email,
                    firstName: response.data.GivenName,
                    lastName: response.data.Surname,
                }));
                setUpdateInformationButtonState();
            }
        } catch (error) {
            console.error("Error loading student profile:", error);
            setState(prev => ({
                ...prev,
                propsModal: { bodyText: "An error occurred while loading the student profile.", visible: true },
                showErrorModal: true,
            }));
        } finally {
            setLoading(false); // Optional: remove if not needed
        }
    };

    const savePassword = (newValue) => {
        setState(prev => ({
            ...prev,
            password: newValue.password,
            passwordRetype: newValue.passwordRetype,
            validPassword: newValue.validPassword,
        }));
        setSubmitPasswordButtonState();
    };

    const saveName = (newValue) => {
        setState(prev => ({
            ...prev,
            firstName: newValue.firstName,
            lastName: newValue.lastName,
            validFirstName: newValue.validFirstName,
            validLastName: newValue.validLastName,
        }));
        setUpdateInformationButtonState();
    };

    const saveEmail = (newValue) => {
        setState(prev => ({
            ...prev,
            email: newValue.email,
            validEmail: newValue.validEmail,
            emailAvailable: newValue.emailAvailable,
        }));
        setUpdateInformationButtonState();
    };

    const showStudentBox = () => setState(prev => ({ ...prev, studentObj: null, productId: null }));

    const renderGenericModal = () => (
        <ModalGeneric data={state.propsModal} updateParent={toggleAlertModal} />
    );

    const renderForm = () => {
        const sessionDetails = SessionManager.getSessionDetails();
        const studentObj = state.studentObj;

        return (
            <div>
                <div className="col col-no-padding-left">
                    {studentObj && (
                        <span className="btn-back" onClick={showStudentBox}>
                            <FontAwesomeIcon icon={faArrowLeft} />{' '}
                            <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                        </span>
                    )}
                </div>

                <div className="wrapper-col borderGray-bottom block-display-responsive">
                    {roleId === Settings.ROLE_STUDENT_ID && (
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="profile-update.students" />
                            </div>
                        </div>
                    )}
                    {roleId === Settings.ROLE_TEACHER_ID && (
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="profile-update.teachers" />
                            </div>
                        </div>
                    )}
                    <div className="col txt-right">
                        {isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) &&
                            context.state.variables.InvitationTeacherToStudentManagementTeacher && (
                                <button className="button-primary-custom" onClick={handleModalSendRequest}>
                                    <FontAwesomeIcon icon={faUserPlus} />{' '}
                                    <Translate id="profile-update.add-user" />
                                </button>
                            )}
                    </div>
                </div>

                <Row className="card-white">
                    <Col sm={12} md={12} lg={12} className="animated delay-05s fadeIn td-w-rollover td-responsive special-top-space syle-date-five">
                        <CardBody>
                            <Row>
                                {!studentObj && (
                                    <Col sm={12}>
                                        <ProfilesTable
                                            redoSearch={state.redoSearch}
                                            isStaff={state.isStaff}
                                            roleId={roleId}
                                            updateParent={saveStudentIdState}
                                            notSelectable={
                                                (isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles) && !context.state.variables.UserManagementPrincipal) ||
                                                (isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) && !context.state.variables.UserManagementTeacher)
                                            }
                                        />
                                    </Col>
                                )}
                            </Row>
                            {showPromptAlerts && studentObj && (
                                <Row>
                                    <Col xs={12} className="animated delay-05s fadeIn">
                                        <PromptSafetyAlert count={promptMonitoring.flaggedCount} />
                                    </Col>
                                </Row>
                            )}
                            <Row>
                                {studentObj && (
                                    <Col xs={12}>
                                        <Translate id="report-student-average.selected-student" />:{' '}
                                        <span className="text-color-custom">
                                            <b>{state.studentFullName}</b>
                                        </span>
                                    </Col>
                                )}
                            </Row>
                            {showPromptAlerts && studentObj && (
                                <Row>
                                    <Col xs={12} className="animated delay-05s fadeIn">
                                        <ProfileSummaryCard
                                            username={studentObj.Username}
                                            recordNumber={studentObj.UserSchoolRecord}
                                            invitationCode={studentObj.Token}
                                            lastSession={formatLastSession(studentObj.LastSession)}
                                        />
                                    </Col>
                                </Row>
                            )}
                        </CardBody>
                    </Col>
                </Row>

                <Row>
                    {state.roles.indexOf(Settings.ROLE_TEACHER_STR) === -1 && studentObj && (
                        <Col sm={12} md={6} lg={6} className="animated delay-05s fadeIn">
                            <div className="wrapper-general">
                                <CardHeader>
                                    <h4>
                                        <Translate id="profile-update.edit-profile" />
                                    </h4>
                                    <small>
                                        <Translate id="profile-update.edit-profile-subtitle" options={{ renderInnerHtml: true }} />
                                    </small>
                                </CardHeader>
                                <CardBody>
                                    <Form>
                                        <FormGroupName data={state} updateParent={saveName} />
                                        <FormGroupEmail data={state} updateParent={saveEmail} />
                                        <FormGroup row>
                                            <Col sm={12}>
                                                <Button
                                                    color="success"
                                                    block
                                                    disabled={!state.proceedUpdateInformation}
                                                    onClick={updateStudentInformation}
                                                >
                                                    <Translate id="profile-update.update-information" />
                                                </Button>
                                            </Col>
                                        </FormGroup>
                                    </Form>
                                </CardBody>
                            </div>
                        </Col>
                    )}

                    {studentObj && (
                        <Col sm={12} md={6} lg={6} className="animated delay-05s fadeIn">
                            <div className="wrapper-general">
                                <CardHeader>
                                <h4>
                                    <Translate id="profile-update.reset-profile-password" />
                                </h4>
                                </CardHeader>
                                <CardBody>
                                    <Form>
                                        <FormGroupNewPassword data={state} updateParent={savePassword} />
                                        <Button
                                            style={{ width: '170px' }}
                                            className='button-primary-custom'
                                            color="none"
                                            block
                                            disabled={!state.proceedUpdatePassword}
                                            onClick={updatePassword}
                                        >
                                            <Translate id="profile-update.save-password" />
                                        </Button>
                                    </Form>
                                </CardBody>
                            </div>
                        </Col>
                    )}
                </Row>

                {showPromptAlerts && studentObj && (
                    <Row>
                        <Col xs={12} className="animated delay-05s fadeIn">
                            <TutorPromptHistory
                                prompts={promptMonitoring.prompts}
                                page={promptMonitoring.page}
                                totalPages={promptMonitoring.totalPages}
                                onPageChange={promptMonitoring.setPage}
                                loading={promptMonitoring.loading}
                                from={promptMonitoring.from}
                                to={promptMonitoring.to}
                                periods={promptMonitoring.periods}
                                onDateRangeChange={promptMonitoring.setDateRange}
                            />
                        </Col>
                    </Row>
                )}

                <Row>
                    <Col xs={12}>
                        {studentObj &&
                            (state.roles.includes(Settings.ROLE_ADMIN_STR) ||
                                state.roles.includes(Settings.ROLE_EMPLOYEE_STR)) && (
                                <div className="td-responsive wrapper-general">
                                    <CardBody>
                                        <UserSchoolsDataTable userId={studentObj.UserId} />
                                    </CardBody>
                                </div>
                            )}
                    </Col>
                </Row>
            </div>
        );
    };

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    {renderForm()}
                    {state.showErrorModal && renderGenericModal()}
                    {state.visibleModalInvitationCode && (
                        <ModalSendInvitationCode
                            visible={state.visibleModalInvitationCode}
                            updateParent={hideModalInvitationCode}
                        />
                    )}
                </div>
            </div>
            {loading ?
            <LoaderFullWindow /> : null }
        </div>
    );
};

export default withLocalize(ProfileUpdate);