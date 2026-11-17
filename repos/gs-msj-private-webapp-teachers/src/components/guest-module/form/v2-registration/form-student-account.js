import React, { useState, useEffect } from "react";
import {Col, Row, Button, Form, InputGroup, Input, InputGroupText, Alert} from 'reactstrap';
import {Translate} from "react-localize-redux";
import LanguageManager from "../../../../custom-classes/language-manager";
import FormGroupNameStudentV2 from "../form-group-name-student-v2";
import FormGroupNewPasswordStudentV2 from '../form-group-new-password-student-v2';
import {registerProfile} from '../../../../api/users';
import ModalSuccessSetupProfileV2 from "../../../shared-module-components/modal/modal-success-setup-profile-first-time-v2";
import LoaderFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import ModalGeneric from "../../../shared-module-components/modal/modal-generic";
import Settings from "../../../../custom-classes/settings";
import {FaUserAlt} from "react-icons/all";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faExclamationTriangle, faInfoCircle} from "@fortawesome/free-solid-svg-icons";
import Validator from "../../../../custom-classes/validator";
import SessionManager from "../../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../../utils/access_logger";

/**
 * Handle the class to a new student account
 */
const FormStudentAccount = (props) => {
    const userDetails = SessionManager.getSessionDetails();
    const [disabled] = useState(props.disabled);
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [password, setPassword] = useState("");
    const [proceedSubmit, setProceedSubmit] = useState(false);
    const [roleId] = useState(Settings.ROLE_STUDENT_ID);
    const [spacesLeft, setSpacesLeft] = useState(true);
    const [students, setStudents] = useState([]);
    const [studentsQuantity, setStudentsQuantity] = useState(props.studentsQuantity);
    const [subscriptionId, setSubscriptionId] = useState(props.subscriptionId);
    const [username, setUsername] = useState("");
    const [validName, setValidName] = useState(false);
    const [validPassword, setValidPassword] = useState(false);
    const [validUsername, setValidUsername] = useState(false);
    const [showErrorModal, setShowErrorModal] = useState(false);
    const [propsModal, setPropsModal] = useState(null);
    const [loading, setLoading] = useState(false);
    const [showAddNewProfileModal, setShowAddNewProfileModal] = useState(false);
    const [usernameAvailable, setUsernameAvailable] = useState(true);
    const usernameStudentTranslation = LanguageManager.getTranslation("register-v2.student-username", Settings.LANGUAGES);
    const passwordStudentTranslation = LanguageManager.getTranslation("register-v2.student-password", Settings.LANGUAGES);

    useEffect(() => {
        setStudentsQuantity(props.studentsQuantity);
        setSubscriptionId(props.subscriptionId);
    }, [props.studentsQuantity, props.subscriptionId]);

    useEffect(()=>{

        // Evaluate the submission button validity when username, password and names are changed
        setButtonSubmitStudentValid(validPassword, validUsername, usernameAvailable, validName);
    }, [validPassword, validUsername, validName]);

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowErrorModal(false);
    };

    const saveName = ({ firstName, lastName, validFirstName, validLastName }) => {
        const isValidName = validFirstName && validLastName;

        setFirstName(firstName);
        setLastName(lastName);
        setValidName(isValidName);
        setButtonSubmitStudentValid(validPassword, validUsername, usernameAvailable, isValidName);
    };

    const savePassword = (newValue) => {
        setPassword(newValue.password);
        setValidPassword(newValue.validPassword);
        setButtonSubmitStudentValid(newValue.validPassword, validUsername, usernameAvailable, validName);
    };

    const setButtonSubmitStudentValid = (validPass, validUser, usernameAvailable, validNameLastName) => {
        setProceedSubmit(validPass && validUser && usernameAvailable && validNameLastName);
    };

    const saveStudentAPI = async () => {
        setLoading(true);

        try {
            const response = await registerProfile(
                subscriptionId,
                username,
                password,
                password,
                firstName,
                lastName,
                null,
                null,
                false,
                roleId,
                null,
                null,
                true
            );

            if (response.status === 200 || response.status === 201) {
                try {
                    SendAccessLog({
                        UserName: userDetails.Username,
                        InvitationCode: userDetails.InvitationCode,
                        Role: userDetails.Roles[0],
                        AreaId: "25",
                        Details: "New student successfully created from client registration.",
                        SubscriptionId: subscriptionId,
                        ProfileInformation: {
                            Role: "student",
                            UserName: username,
                            GiveName: firstName,
                            SurName: lastName
                        }
                    })
                }
                catch (error) {
                    // empty catch block to avoid breaking the application
                }
                if (response.data.SpacesInSubscriptionLeft) {
                    setShowAddNewProfileModal(true);
                    setSpacesLeft(true);
                } else {
                    // Proceed to next page
                    props.updateParent(true);
                }
            }
        } catch (error) {
            console.error("Error saving student account:", error);
        } finally {
            setLoading(false);
        }
    };

    const toggleAlertAddNewProfile = () => {
        setStudents((prevStudents) => [
            ...prevStudents,
            <p key={`${firstName}-${lastName}`}>{`${firstName} ${lastName}`}</p>
        ]);

        setShowAddNewProfileModal(false);
        setFirstName("");
        setLastName("");
        setUsername("");
        setValidUsername(false);
        setValidName(false);
        setProceedSubmit(false); // Optional: consistent with button validity state
        // Force username input to be empty
        handleUsernameChange({ currentTarget: { value: "" } });

    };

    const handleUsernameChange = (event) => {

        const value = event.currentTarget.value;
        const isValid = Validator.isUsernameValid(value);

        setUsername(value);
        setValidUsername(isValid);

        if (!isValid) {
            setUsernameAvailable(false);
            setButtonSubmitStudentValid(validPassword, false, usernameAvailable, validName);
            return;
        }

        // Check availability asynchronously
        Validator.isUsernameAvailable(value, (isAvailable) => {
            setUsernameAvailable(isAvailable);

            // Only update button state after availability is known
            const isUsernameValidAndAvailable = isValid && isAvailable;
            setButtonSubmitStudentValid(validPassword, isUsernameValidAndAvailable, isAvailable, validName);
        });
    };

    const renderStudentAccount = () => {
        return (
            <Form>
                <Row>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size:12}}>
                        <h2 className="centered-text">
                            <Translate id="register-v2.student-account"/>
                        </h2>
                    </Col>
                </Row>
                <Row>
                    <hr/>
                </Row>
                <Row>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size:12}}>
                        <FormGroupNameStudentV2 firstName={firstName} lastName={lastName} updateParent={saveName}/>
                    </Col>
                </Row>
                <Row>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size:6}}>
                        <InputGroup>
                            <Input
                                disabled={disabled}
                                type="text"
                                name="profile_enter_usern"
                                placeholder={usernameStudentTranslation}
                                id="profile_enter_usern"
                                autoComplete="off"
                                autocomplete="off"
                                autoCorrect="off"
                                spellCheck="false"
                                onChange={handleUsernameChange}
                            />
                            <div className="input-group-append">
                                <InputGroupText><FaUserAlt /></InputGroupText>
                            </div>
                        </InputGroup>
                    </Col>
                    <Col xs={{size:12}} sm={{size: 12}} md={{size: 6}} lg={{size:6}}>
                        <FormGroupNewPasswordStudentV2 password={password} passwordRetype={password} updateParent={savePassword} />
                    </Col>
                </Row>
                    {!validUsername || !usernameAvailable ?
                        <Row>
                            <Col xs="6">
                            <Alert color="info" isOpen={!validUsername}>
                                <FontAwesomeIcon icon={faInfoCircle} />{' '}<Translate id="register.username-error"/>
                            </Alert>
                            <Alert color="danger" isOpen={!usernameAvailable}>
                                <FontAwesomeIcon icon={faExclamationTriangle} />{' '}<Translate id="register.username-availability-error"/>
                            </Alert>
                            </Col>
                        </Row> : null
                    }
                <Row>
                    <Col xs={{size: 6}} sm={{size: 6}} md={{size: 6}} lg={{size: 6}}>
                    </Col>
                    <Col xs={{size: 6}} sm={{size: 6}} md={{size: 6}} lg={{size: 6}} className="text-right">
                        <Button color="success"
                                disabled={!proceedSubmit}
                                onClick={saveStudentAPI}>
                            <Translate id="register-v2.submit"
                                       options={{renderInnerHtml: false}}/>
                        </Button>
                    </Col>
                </Row>
                {
                    studentsQuantity > 1 ?
                        <Row>
                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                <Translate id="register-v2.student-added"
                                           options={{renderInnerHtml: false}}/>
                            </Col>
                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                {students}
                            </Col>
                        </Row> : null
                }

                {loading ? <LoaderFullWindow/> : null}
                {showErrorModal ? <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}
                {showAddNewProfileModal ?
                    <ModalSuccessSetupProfileV2 visible={showAddNewProfileModal} spacesLeft={spacesLeft}
                                                updateParent={toggleAlertAddNewProfile} />  : null}
            </Form>
        )
    }

    // Render the form group
    return (
        <div>
            <Row>
                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    {renderStudentAccount()}
                </Col>
            </Row>
        </div>
    )
};

export default FormStudentAccount;