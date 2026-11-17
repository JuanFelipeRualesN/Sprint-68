/**
 * Created by portiz on 8/23/2018.
 */
import React, { useEffect, useState } from 'react';import {Input, Row, Col, Label, Alert} from 'reactstrap';
import Validator from '../../../custom-classes/validator';
/** Multiple Language imports **/
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * The form group to manage the phone.
 * Includes an alert to validates the phone and the main function to update the parent phoe
 */
const FormGroupPhone = ({ data, disabled, updateParent }) => {
  const [phone, setPhone] = useState(data?.phone || '');
  const [validPhone, setValidPhone] = useState(true);

  useEffect(() => {
    if (data) {
      setPhone(data.phone || '');
    }
  }, [data]);

  // Handles the phone change and send the phone to the parent
  const handlePhoneChange = (event) => {
    const value = event.currentTarget.value.replace(/\D/g, '');
    setPhone(value);

    const isValid = Validator.isPhoneValid(value);
    setValidPhone(isValid);

    updateParent({
      phone: value,
      validPhone: isValid,
      disabled,
    });
  };

    return (
        <div>
            <Row>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 6}}>
                    <Label for="phone" className="label-forms-blue">
                        * <Translate id="register.phone" options={{renderInnerHtml: true}}/>
                    </Label>
                    <Input className="mantine-like__control" disabled={disabled} value={phone} type="tel" name="phone"
                            id="phone"
                            onChange={handlePhoneChange}/>
                    <br/>
                    {
                        !validPhone ?
                            <Alert color="danger" isOpen={!validPhone}>
                                <Translate id="register.phone-error" options={{renderInnerHtml: true}}/>
                            </Alert> : null
                    }
                </Col>
            </Row>
        </div>
    )
};

export default FormGroupPhone;