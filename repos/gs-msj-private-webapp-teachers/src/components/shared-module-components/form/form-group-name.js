import React, { useEffect, useState } from 'react';
import {Input, Col, Label, Alert, Row} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons';
import {Translate} from "react-localize-redux";

/**
 * Handles the form group of the person name
 */
const FormGroupNewName = ({ data, disabled, updateParent }) => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [validFirstName, setValidFirstName] = useState(true);
  const [validLastName, setValidLastName] = useState(true);

  useEffect(() => {
    if (data) {
      setFirstName(data.firstName);
      setLastName(data.lastName);
    }
  }, [data]);

  // Action when the first name is typed
  const handleFirstNameChange = (event) => {
    const value = event.currentTarget.value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]/g, '');
    setFirstName(value);

    const isValidFirst = Validator.isFirstNameValid(value);
    const isValidLast = Validator.isLastNameValid(lastName);

    setValidFirstName(isValidFirst);
    setValidLastName(isValidLast);

    updateParent({
      firstName: value,
      lastName,
      validFirstName: isValidFirst,
      validLastName: isValidLast,
      disabled,
    });
  };

  // Action when the last name is typed
  const handleLastNameChange = (event) => {
    const value = event.currentTarget.value.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]/g, '');
    setLastName(value);
    const isValidFirst = Validator.isFirstNameValid(firstName);
    const isValidLast = Validator.isLastNameValid(value);

    setValidFirstName(isValidFirst);
    setValidLastName(isValidLast);

    updateParent({
      firstName,
      lastName: value,
      validFirstName: isValidFirst,
      validLastName: isValidLast,
      disabled,
    });
  };

  return (
    <div>
      <Row>
        <Col sm="12" md="6" lg="6">
          <Label for="firstname" className="label-forms-blue">
            * <Translate id="register.firstname" />
          </Label>
          <Input
            className="mantine-like__control"
            disabled={disabled}
            value={firstName}
            type="text"
            name="firstname"
            id="firstname"
            onChange={handleFirstNameChange}
          />
          <br />
          {!validFirstName && (
            <Alert color="info">
              <FontAwesomeIcon icon={faInfoCircle} />{' '}
              <Translate id="register.firstname-error" />
            </Alert>
          )}
        </Col>

        <Col sm="12" md="6" lg="6">
          <Label for="lastname" className="label-forms-blue">
            * <Translate id="register.lastname" />
          </Label>
          <Input
              className="mantine-like__control"
            disabled={disabled}
            value={lastName}
            type="text"
            name="lastname"
            id="lastname"
            onChange={handleLastNameChange}
          />
          <br />
          {!validLastName && (
            <Alert color="info">
              <FontAwesomeIcon icon={faInfoCircle} />{' '}
              <Translate id="register.lastname-error" />
            </Alert>
          )}
        </Col>
      </Row>
    </div>
  );
};

export default FormGroupNewName;