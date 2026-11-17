import React, { useState } from 'react';
import {Input, Col, Row, Button, Form, InputGroup,  InputGroupText} from 'reactstrap';
import FormGroupPhoneV2 from "../form-group-phone-v2";
import {Translate} from "react-localize-redux";
import {FaMapPin} from "react-icons/all";
import LanguageManager from "../../../../custom-classes/language-manager";
import {setContactRegisterV2} from '../../../../api/users';
import LoaderFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import ModalGeneric from "../../../shared-module-components/modal/modal-generic";
import Settings from "../../../../custom-classes/settings";

/**
 * Handle the class to update user contact details
 */
const FormAdministratorContact = ({updateParent, disabled: disabledProp}) => {
  const [city, setCity] = useState("");
  const [disabled] = useState(disabledProp);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phone, setPhone] = useState("");
  const [proceedSave, setProceedSave] = useState(false);
  const [validCity, setValidCity] = useState(false);
  const [validFirstName, setValidFirstName] = useState(true);
  const [validLastName, setValidLastName] = useState(true);
  const [validPhone, setValidPhone] = useState(false);
  const [showErrorModal, setShowErrorModal] = useState(false);
  const [propsModal, setPropsModal] = useState(null);
  const [loading, setLoading] = useState(false);

  const cityTranslation = LanguageManager.getTranslation("register-v2.city", Settings.LANGUAGES);

  // Toggles alert modal
  const toggleAlertModal = () => {
    setShowErrorModal(false);
  };

  const saveContactDetailsAPI = () => {
    setLoading(true);
    const apiCall = setContactRegisterV2(firstName, lastName, city, phone);

    apiCall.then((response) => {
      if (response.status !== 200 && response.status !== 201) {
        const propsModal = {
          bodyText: response.data.Messages[0],
          visible: true
        };
        setPropsModal(propsModal);
        setShowErrorModal(true);
        setLoading(false);
      } else {
        setLoading(false);
        updateParent(true);
      }
    });
  };

  const saveName = (newValue) => {
    setFirstName(newValue.firstName);
    setLastName(newValue.lastName);
    setValidFirstName(newValue.validFirstName);
    setValidLastName(newValue.validLastName);
    evaluateSubmitButtonAvailable(
      newValue.validFirstName,
      newValue.validLastName,
      validPhone,
      validCity
    );
  };

  const saveCity = (newValue) => {
    const value = newValue.currentTarget.value;
    const valid = value.trim().length > 0;
    setCity(value);
    setValidCity(valid);
    evaluateSubmitButtonAvailable(validFirstName, validLastName, validPhone, valid);
  };

  const savePhone = (newValue) => {
    setPhone(newValue.phone);
    setValidPhone(newValue.validPhone);
    evaluateSubmitButtonAvailable(validFirstName, validLastName, newValue.validPhone, validCity);
  };

  const evaluateSubmitButtonAvailable = (first, last, phoneValid, cityValid) => {
    setProceedSave(first && last && phoneValid && cityValid);
  };

  const renderAdministratorContact = () => (
    <Form>
        <Row>
        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size:12}}>
            <h2 className="centered-text">
                <Translate id="register-v2.administrator-contact"/>
            </h2>
        </Col>
        </Row>
        <Row>
            <hr/>
        </Row>
        {
            /* <Row>
        <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size:12}}>
            <FormGroupNameTutorV2 updateParent={saveName()}/>
        </Col>
        </Row> */
        }
        <Row>
        <Col xs={{size:12}} sm={{size: 12}} md={{size: 6}} lg={{size:6}}>
            <InputGroup>
                <Input onChange={saveCity} value={city} type="text" name="city" id="city" placeholder={cityTranslation}
                maxLength="25"/>
                <div className="input-group-append">
                    <InputGroupText><FaMapPin /></InputGroupText>
                </div>
            </InputGroup>
        </Col>
        <Col xs={{size:12}} sm={{size: 12}} md={{size: 6}} lg={{size:6}}>
            <FormGroupPhoneV2 updateParent={savePhone}/>
        </Col>
        </Row>
        <Row>
            <Col xs={{size: 6}} sm={{size: 6}} md={{size: 6}} lg={{size: 6}}>
            </Col>
            <Col xs={{size: 6}} sm={{size: 6}} md={{size: 6}} lg={{size: 6}} className="text-right">
                <Button color="success"
                        disabled={!proceedSave}
                        onClick={saveContactDetailsAPI}>
                    <Translate id="register-v2.submit"
                                options={{renderInnerHtml: false}}/>
                </Button>
            </Col>
        </Row>
        {loading ? <LoaderFullWindow/> : null}
        {showErrorModal ? <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}
    </Form>
  );

    // Render the form group
    return (
        <div>
            <Row>
                <Col xs={{size:12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    {renderAdministratorContact()}
                </Col>
            </Row>
        </div>
    )
};


export default FormAdministratorContact;