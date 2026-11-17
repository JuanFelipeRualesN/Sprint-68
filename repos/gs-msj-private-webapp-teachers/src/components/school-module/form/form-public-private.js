/**
 * Created by portiz on 4/15/2020.
 */
import React, { useEffect, useState } from 'react';
import { Input, Label, FormGroup, Alert } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import Settings from '../../../custom-classes/settings';
/** End of Multiple Language imports **/

/**
 * Handles the form group of the person name
 */
const FormGroupPublicPrivate = ({ disabled, isRegisterForm: propIsRegisterForm, privateOrPublic: propPrivateOrPublic, updateParent }) => {
  const [privateOrPublic, setPrivateOrPublic] = useState( propPrivateOrPublic ?? 0);
  const [isRegisterForm] = useState( propIsRegisterForm ?? false);
  const [showRegisterHomeSchoolError, setShowRegisterHomeSchoolError] = useState(false);
  const [validSelection, setValidSelection] = useState(true);

  useEffect(() => {
    if (typeof propPrivateOrPublic !== 'undefined' && propPrivateOrPublic !== null) {
      setPrivateOrPublic(propPrivateOrPublic);
    }
  }, [propPrivateOrPublic]);

  // Action when the first name is typed
  const handleOnChange = (event) => {
    if (event.currentTarget.value === "2") {
      setPrivateOrPublic(event.currentTarget.value);
      setShowRegisterHomeSchoolError(true);
      setValidSelection(false);
      updateParent(event.currentTarget.value, false);
    } else {
      setPrivateOrPublic(event.currentTarget.value);
      setShowRegisterHomeSchoolError(false);
      setValidSelection(true);
      updateParent(event.currentTarget.value, true);
    }
  };

  // Render the form group
    return (
            <FormGroup check>
                {Settings.IS_APP_MANAGED_BY_REQUIRED && (<Label check>
                    <Input type="radio" name="private-public" value={"1"}
                        checked={privateOrPublic === "1"}
                        onChange={handleOnChange} />{' '}
                    <Translate id="school-module.private" />
                </Label>)}
                <br />
                <Label check>
                    <Input type="radio" checked={privateOrPublic === "0"} name="private-public" value={"0"} onChange={handleOnChange} />{' '}
                    <Translate id="school-module.public" />
                </Label>
                <br />
                {isRegisterForm && Settings.IS_APP_MANAGED_BY_REQUIRED ?
                    <Label check>
                        <Input type="radio" name="private-public" value={"2"} onChange={handleOnChange} />{' '}
                        <Translate id="school-module.homeschooling" />
                    </Label>
                    : null}
                <br />
                {
                    showRegisterHomeSchoolError ?
                        <Alert color="primary">
                            <Translate id="school-module.homeschooling-note" options={{ renderInnerHtml: true }} />
                        </Alert> : null
                }
            </FormGroup>
        )
};

export default FormGroupPublicPrivate;