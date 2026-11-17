import React, { useState, useEffect, useContext } from "react";
import Settings from '../../../../custom-classes/settings';
import SelectorCountry from '../../../shared-module-components/selector/selector-country';
import ModalSchoolEntityFilter from "../../modals/school-module/modal-school-entity-filter";
import ModalGeneric from "../../../shared-module-components/modal/modal-generic";
import { useHistory, useLocation } from "react-router-dom";
import { getStateProvinces } from "../../../../api/address";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import { getEducationalRegions } from "../../../../api/schools";
import { getSubscriptionSchoolEntitiesRelatedAdvanced } from "../../../../api/subscriptions";
import Select from "react-select";
import { Translate } from "react-localize-redux";
import styled from "styled-components";
import { getSchoolEntityDetails } from "../../../../api/schools";
import { WizardContext } from "../../modals/modal-subscription";

const TextDangerStyled = styled.p`
  color: #fa2626;
  font-size: 11px;
  margin: auto;
  padding: 0 0 0.25em 0;
  text-align: right;
`;

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const Step3 = ({ stepCompleted, setTypeEntityToWizard, setEntityIdToWizard, isSchool }) => {

  const { data } = useContext(WizardContext);
  const history = useHistory();
  let location = useLocation();
  let query = useQuery();
  let planId = query.get("planId");
  let userId = query.get("userId");
  let entityIdQuery = query.get("entityId");

  const [modalVisible, setModalVisible] = useState(false);
  const [typeEntity, setTypeEntity] = useState(Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID);
  const [loading, setLoading] = useState(false);
  const [viewEntity, setViewEntity] = useState(false);
  const [privateOrPublic, setPrivateOrPublic] = useState(null);
  const [addressLine1, setAddressLine1] = useState("");
  const [addressLine2, setAddressLine2] = useState("");
  const [addressPhone, setAddressPhone] = useState("");
  const [addressCity, setAddressCity] = useState("");
  const [addressZip, setAddressZip] = useState("");
  const [addressState, setAddressState] = useState("");
  const [addressCountry, setAddressCountry] = useState("");
  const [entityName, setEntityName] = useState("");
  const [regionsOptions, setRegionsOptions] = useState([]);
  const [educationalRegion, setEducationalRegion] = useState("");
  const [disabledFields, setDisabledFields] = useState(false);
  const [visibleStatesSelect, setVisibleStatesSelect] = useState(false);
  const [states, setStates] = useState([]);
  const [entityCode, setEntityCode] = useState("");

  const [validAddress, setValidAddress] = useState(true);
  const [validEntityName, setValidEntityName] = useState(true);
  const [validEntityCode, setValidEntityCode] = useState(true);
  const [validEducationalRegion, setValidEducationalRegion] = useState(true);
  const [validCity, setValidCity] = useState(true);
  const [validPhone, setValidPhone] = useState(true);
  const [validState, setValidState] = useState(true);
  const [validEntityType, setValidEntityType] = useState(true);
  const [validCountry, setValidCountry] = useState(true);
  const [validZipCode, setValidZipCode] = useState(true);

  const [messagesModal, setMessagesModal] = useState({
    visible: false,
    bodyText: ""
  });

  useEffect(() => {
    if (data && data.subscriptionId) {
      getSubscriptionEntitiesRelated(data.subscriptionId);
    } else if (entityIdQuery) {
      getSchoolEntity(entityIdQuery);
    }
  }, []);

  const getSubscriptionEntitiesRelated = async (subscriptionId) => {
    setLoading(true);
    try {
      const res = await getSubscriptionSchoolEntitiesRelatedAdvanced(subscriptionId);
      const entityId = res.data[0].Id;
      await getSchoolEntity(entityId);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const getSchoolEntity = async (entityIdQuery) => {
    setLoading(true);
    try {
      const res = await getSchoolEntityDetails(entityIdQuery);
      const entity = res.data;
      setEntitiesValues(entity);
      setAddressEntity(entity);
      setDisabledFields(true);
      setViewEntity(true);
      setEntityIdToWizard(entity.Entities[0].Id);
      stepCompleted(true);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const handleSchoolModal = () => {
    setModalVisible(true);
    setTypeEntity(Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID);
  }

  const handleProgramModel = () => {
    setModalVisible(true);
    setTypeEntity(Settings.SCHOOL_MODULE_PROGRAM_TYPE_ID);
  }

  const toggleAlertModal = () => {
    setMessagesModal({
      visible: false
    })
  }

  const handleSelected = (entity) => {
    if (entity) {
      pushParamsToUrl(entity.Entities[0].Id);
      setEntitiesValues(entity);
      setAddressEntity(entity);
      setDisabledFields(true);
      setViewEntity(true);
      setEntityIdToWizard(entity.Entities[0].Id);
      stepCompleted(true);
    }
    setModalVisible(false);
  }

  const pushParamsToUrl = (entityId) => {
    history.push(`${location.pathname}?userId=${userId}&planId=${planId}&entityId=${entityId}&subscriptionType=${!isSchool ? "H" : "S"}`);
  }

  const setAddressEntity = (entity) => {
    setAddressLine1(entity.Addresses ? entity.Addresses[0].AddressLine1 : "");
    setAddressLine2(entity.Addresses ? entity.Addresses[0].AddressLine2 : "");
    setAddressPhone(entity.Addresses ? entity.Addresses[0].AddressPhone : "");
    setAddressState(entity.Addresses ? entity.Addresses[0].State : "");
    setAddressZip(entity.Addresses ? entity.Addresses[0].Zip : "");
    setAddressCity(entity.Addresses ? entity.Addresses[0].City : "");
    setAddressCountry(entity.Addresses ? entity.Addresses[0].Country : "");
  }

  const loadStates = async () => {
    try {
      const res = await getStateProvinces("USA");
      const states = res.data.map((elem) => ({
        value: elem.IsoCode,
        label: elem.Name,
      }));
      setStates(states);
      return states;
    } catch {

    }
  }

  const handleOnChangeCountry = (value) => {

    searchEducationalRegions(value);
    setAddressCountry(value);

    if (value === "USA") {
      if (states.length < 1) {
        loadStates();
      }
      setVisibleStatesSelect(true);
    } else {
      setVisibleStatesSelect(false);
    }
  }

  const handleOnChangeState = (value) => {
    setAddressState(value);
  }
  const handleOnChangeName = (event) => {
    setEntityName(event.currentTarget.value)
  }

  const handleOnChangeCode = (event) => {
    setEntityCode(event.currentTarget.value)
  }

  const handleOnChangeAddressLine1 = (event) => {
    setAddressLine1(event.currentTarget.value)
  }

  const handleOnChangeAddressLine2 = (event) => {
    setAddressLine2(event.currentTarget.value)
  }

  const handleOnChangeCity = (event) => {
    setAddressCity(event.currentTarget.value)
  }

  const handleOnChangeZipCode = (event) => {
    setAddressZip(event.currentTarget.value)
  }

  const handleOnChangeAddressPhone = (event) => {
    setAddressPhone(event.currentTarget.value)
  }

  const setEntitiesValues = (entity) => {
    setEntityName(entity.Entities[0].Name);
    setEntityCode(entity.Entities[0].Token);
    setPrivateOrPublic(entity.Entities[0].Private);
    setTypeEntity(parseInt(entity.Entities[0].EntityTypeId));
    setTypeEntityToWizard(parseInt(entity.Entities[0].EntityTypeId));
  }

  const handleEducationalRegion = (selected) => {
    setEducationalRegion(selected);
  }

  const searchEducationalRegions = (country) => {
    getEducationalRegions(country).then((response) => {
      let regions = response.data,
        options = [];

      for (var i = 0; i < regions.length; i++) {
        options.push({
          label: regions[i].Name,
          value: regions[i].Id,
        })
      }
      setRegionsOptions(options);
    });
  }

  const handleCheckedPrivateOrPublic = (event) => {
    setPrivateOrPublic(event.currentTarget.value)
  }

  return (
    <>
      <div className="content-wizard">
        <h1 className="content-wizard--title">
          {isSchool ?
              <Translate id="wizard.step-3.title" options={{ renderInnerHtml: true }} /> : <Translate id="wizard.step-3.title-dpr" options={{ renderInnerHtml: true }} />}
        </h1>
        {/*data && data.subscriptionId || !Settings.IS_APP_MANAGED_BY_REQUIRED ? null : <><button className="content-wizard--w-icon mr-1" onClick={handleSchoolModal}>
          {" "}
          <i className="fas fa-plus-circle" />
          <Translate id="wizard.step-3.select-school" options={{ renderInnerHtml: true }} />{" "}
        </button>
          {Settings.IS_APP_MANAGED_BY_REQUIRED ?
            <button className="content-wizard--w-icon" onClick={handleProgramModel}>
              {" "}
              <i className="fas fa-plus-circle" />
              <Translate id="wizard.step-3.select-program" options={{ renderInnerHtml: true }} />
              {" "}
            </button> : null}</>*/}


        <div className="wrapper-wizard-form mt-3 input-responsive">
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*
                <Translate id="wizard.step-3.name" options={{ renderInnerHtml: true }} />
              </label>
              <input type="text"
                className="form-wizard-fields__input"
                value={entityName}
                disabled={disabledFields}
                onChange={handleOnChangeName} />
              {!validEntityName &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.name"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>

          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*
                <Translate id="wizard.step-3.code" options={{ renderInnerHtml: true }} />
              </label>
              <input type="text"
                className="form-wizard-fields__input"
                value={entityCode}
                disabled={disabledFields}
                onChange={handleOnChangeCode} />
              {!validEntityCode &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.code"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>

          <hr />

          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*
                <Translate id="wizard.step-3.address" options={{ renderInnerHtml: true }} />
              </label>
              <input type="text"
                className="form-wizard-fields__input"
                value={addressLine1}
                disabled={disabledFields}
                onChange={handleOnChangeAddressLine1} />
              {!validAddress &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.address"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
              <input type="text"
                className="form-wizard-fields__input"
                value={addressLine2}
                disabled={disabledFields}
                onChange={handleOnChangeAddressLine2} />
            </div>
          </div>
        </div>

        <div className="wrapper-wizard-form">
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*
                <Translate id="wizard.step-3.city" options={{ renderInnerHtml: true }} />
              </label>
              <input type="text"
                className="form-wizard-fields__input"
                value={addressCity}
                onChange={handleOnChangeCity}
                disabled={disabledFields} />
              {!validCity &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.city"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>
          {visibleStatesSelect &&
            <div className="wrapper-wizard-form--col">
              <div className="form-wizard-fields">
                <label>*
                  <Translate id="wizard.step-3.state" options={{ renderInnerHtml: true }} />
                </label>
                <Select options={states}
                        classNamePrefix="mantine-like"
                        className="mantine-like__control"
                  disabled={disabledFields}
                  value={addressState}
                  onChange={handleOnChangeState} />
                {!validState &&
                  <TextDangerStyled>
                    {<Translate id="wizard.step-3.errors.state"
                      options={{ renderInnerHtml: true }} />}
                  </TextDangerStyled>
                }
              </div>
            </div>
          }
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields wizard-fields__select">
              <label>*
                <Translate id="wizard.step-3.country" options={{ renderInnerHtml: true }} />
              </label>
              {<SelectorCountry disabled={disabledFields}
                value={addressCountry}
                updateParent={handleOnChangeCountry} />}
              {!validCountry &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.country"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields wizard-fields__select">
              <label>*
                <Translate id="wizard.step-3.educational-region" options={{ renderInnerHtml: true }} />
              </label>
              <Select options={regionsOptions}
                value={educationalRegion}
                onChange={handleEducationalRegion}
                isDisabled={disabledFields}
                      classNamePrefix="mantine-like"
                      className="mantine-like__control" />
              {!validEducationalRegion &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.educational-region"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*
                <Translate id="wizard.step-3.zipcode" options={{ renderInnerHtml: true }} />
              </label>
              <input type="text"
                className="form-wizard-fields__input"
                value={addressZip}
                disabled={disabledFields}
                onChange={handleOnChangeZipCode} />
              {!validZipCode &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.zipcode"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>

          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*
                <Translate id="wizard.step-3.phone" options={{ renderInnerHtml: true }} />
              </label>
              <input type="text"
                className="form-wizard-fields__input"
                value={addressPhone}
                disabled={disabledFields}
                onChange={handleOnChangeAddressPhone} />
              {!validPhone &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-3.errors.phone"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>
          {Settings.IS_APP_MANAGED_BY_REQUIRED && <div className="wrapper-wizard-form--col">
            <div className="wrapper-wizard-form--col">
              <span className="txt-label">*
                <Translate id="wizard.step-3.school-type" options={{ renderInnerHtml: true }} />
              </span>
              <div className="form-wizard-fields">
                <div className="form-wizard-fields form-wizard-fields__box special-space">
                  <input type="radio"
                    id="privado"
                    name="privado"
                    checked={privateOrPublic === "1"}
                    disabled={disabledFields}
                    value={"1"}
                    onChange={handleCheckedPrivateOrPublic} />
                  <label for="privado">
                    <Translate id="wizard.step-2.plan-type-check-options.private" options={{ renderInnerHtml: true }} />
                  </label>
                  <input type="radio"
                    id="publico"
                    name="publico"
                    checked={privateOrPublic === "0"}
                    value={"0"}
                    disabled={disabledFields}
                    onChange={handleCheckedPrivateOrPublic} />
                  <label for="publico">
                    <Translate id="wizard.step-2.plan-type-check-options.public" options={{ renderInnerHtml: true }} />
                  </label>
                  {!validEntityType &&
                    <TextDangerStyled>
                      {<Translate id="wizard.step-3.errors.school-type"
                        options={{ renderInnerHtml: true }} />}
                    </TextDangerStyled>
                  }
                </div>
              </div>
            </div>
          </div>}
        </div>
      </div>
      <div className="content-wizard__footer"></div>
      {modalVisible ?
        <ModalSchoolEntityFilter
          visibleRelationEntity={modalVisible}
          entityTypeId={typeEntity === Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID ? Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID : Settings.SCHOOL_MODULE_PROGRAM_TYPE_ID}
          updateParent={handleSelected} /> : null
      }
      {messagesModal.visible && <ModalGeneric data={messagesModal}
        updateParent={toggleAlertModal} />}
      {loading ? <LoadingFullWindow /> : null}
    </>
  );
};

export default Step3;
