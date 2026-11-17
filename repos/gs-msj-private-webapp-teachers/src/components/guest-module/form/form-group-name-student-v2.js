import  React, { useState, useEffect }  from 'react';
import {Input, Col, Row, InputGroup,  InputGroupText} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import {FaGraduationCap} from "react-icons/all";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";

/**
 * Handles the form group of the person name
 */
const FormGroupNameStudentV2 = ({ disabled, firstName: initialFirstName, lastName: initialLastName, updateParent }) => {
    const [firstName, setFirstName] = useState(initialFirstName || '');
    const [lastName, setLastName] = useState(initialLastName || '');
    const [validFirstName, setValidFirstName] = useState(true);
    const [validLastName, setValidLastName] = useState(true);
    const [isDisabled, setIsDisabled] = useState(disabled);

    const nameStudentTranslation = LanguageManager.getTranslation("register-v2.student-name", Settings.LANGUAGES);
    const lastNameStudentTranslation = LanguageManager.getTranslation("register-v2.student-last-name", Settings.LANGUAGES);

    useEffect(() => {
        setIsDisabled(disabled);
    }, [disabled]);

    useEffect(() => {
        if (initialFirstName !== undefined) {
            setFirstName(initialFirstName);
            setLastName(initialLastName);
        }
    }, [initialFirstName, initialLastName]);

    const handleFirstNameChange = (event) => {
        const rawValue = event.currentTarget.value;

        // Allow only letters (with accents), spaces, apostrophes, and hyphens
        const filteredValue = rawValue.replace(/[^A-Za-zÀ-ÖØ-öø-ÿ\s'-]/g, '');

        setFirstName(filteredValue);

        const isValidFirst = Validator.isFirstNameValid(filteredValue);
        const isValidLast = Validator.isLastNameValid(lastName);

        setValidFirstName(isValidFirst);
        setValidLastName(isValidLast);

        updateParent({ firstName: filteredValue, lastName, validFirstName: isValidFirst, validLastName: isValidLast });
    };

    const handleLastNameChange = (event) => {
        const rawValue = event.currentTarget.value;

        // Allow only letters (including accents), spaces, apostrophes, and hyphens
        const filteredValue = rawValue.replace(/[^A-Za-zÀ-ÖØ-öø-ÿ\s'-]/g, '');

        setLastName(filteredValue);

        const isValidFirst = Validator.isFirstNameValid(firstName);
        const isValidLast = Validator.isLastNameValid(filteredValue);

        setValidFirstName(isValidFirst);
        setValidLastName(isValidLast);

        updateParent({ firstName, lastName: filteredValue, validFirstName: isValidFirst, validLastName: isValidLast });
    };

    return (
        <div>
            <Row>
                <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 6 }}>
                    <InputGroup>
                        <Input
                            disabled={isDisabled}
                            maxLength="40"
                            value={firstName}
                            type="text"
                            name="firstname"
                            id="firstname"
                            placeholder={nameStudentTranslation}
                            onChange={handleFirstNameChange}
                        />
                        <div className="input-group-append">
                            <InputGroupText><FaGraduationCap /></InputGroupText>
                        </div>
                    </InputGroup>
                </Col>
                <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 6 }}>
                    <InputGroup>
                        <Input
                            disabled={isDisabled}
                            maxLength="40"
                            value={lastName}
                            type="text"
                            name="lastname"
                            id="lastname"
                            placeholder={lastNameStudentTranslation}
                            onChange={handleLastNameChange}
                        />
                        <div className="input-group-append">
                            <InputGroupText><FaGraduationCap /></InputGroupText>
                        </div>
                    </InputGroup>
                </Col>
            </Row>
        </div>
    );
};

export default FormGroupNameStudentV2;
