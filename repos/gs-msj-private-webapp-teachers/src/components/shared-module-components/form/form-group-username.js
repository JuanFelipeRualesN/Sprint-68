import React, { useEffect, useState, useRef } from 'react';
import {Input, Col, Label, Alert, Row} from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faInfoCircle, faExclamationTriangle } from '@fortawesome/free-solid-svg-icons';
import Validator from '../../../custom-classes/validator';
import {Translate} from "react-localize-redux";

/**
 * Class to handle the username for group
 * It validates the username (check if username already exists)
 * And it validates the character length and more
 */
const FormGroupUsername = ({ data, disabled, updateParent }) => {
  const [username, setUsername] = useState('');
  const [usernameAvailable, setUsernameAvailable] = useState(true);
  const [validUsername, setValidUsername] = useState(true);
  const usernameRef = useRef('');

  useEffect(() => {
    if (data) {
      setUsername(data.username);
      usernameRef.current = data.username;
    }
  }, [data]);

  // Action when the username changes
  const handleChange = (event) => {
    const value = event.currentTarget.value.replace(/[^a-zA-Z0-9]/g, '');
    setUsername(value);
    usernameRef.current = value;

    const isValid = Validator.isUsernameValid(value);
    setValidUsername(isValid);

    updateParent({
      username: value,
      validUsername: isValid,
      usernameAvailable,
      disabled
    });

    if (isValid) {
      Validator.isUsernameAvailable(value, (returned) => {
        if (usernameRef.current === value) {
          setUsernameAvailable(returned);
          updateParent({
            username: value,
            validUsername: isValid,
            usernameAvailable: returned,
            disabled
          });
        }
      });
    } else {
      updateParent({
        username: value,
        validUsername: isValid,
        usernameAvailable: false,
        disabled
      });
    }
  };

    return (
        <div>
            <Row>
            <Col xm={{size:12}} sm={{size: 12}} md={{size:12}} lg={{size:12}}>
            <Label for="username" className="label-forms-blue">
                * <Translate id="register.username"/>
            </Label>
            <Input className="mantine-like__control"
                   disabled={disabled} value={username} type="username" name="username"
                    id="username" onChange={handleChange}/>
            <br/>
            {!validUsername || !usernameAvailable ?
                <div>
                    <Alert color="info" isOpen={!validUsername}>
                        <FontAwesomeIcon icon={faInfoCircle} />{' '}<Translate id="register.username-error"/>
                    </Alert>
                    <Alert color="danger" isOpen={!usernameAvailable}>
                        <FontAwesomeIcon icon={faExclamationTriangle} />{' '}<Translate id="register.username-availability-error"/>
                    </Alert></div> : null
            }
            </Col>
                </Row>
        </div>
    )
};

export default FormGroupUsername;