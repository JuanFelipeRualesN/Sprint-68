import React, { useState, useEffect, useContext } from "react";
import ModalPlanSelection from "../../modals/modal-plan-selection";
import CheckboxTaxes from '../../selectors/multi-selector-taxes';
import SelectorPlanProduct from '../../selectors/selector-plan-product';
import SelectorBillingInterval from "../../selectors/selector-billing-interval";
import { getPlansAdvanced, insertPlan } from "../../../../api/subscriptions";
import { getLanguages } from "../../../../api/gsweb";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import PlanDescriptionEditorSN from '../../html-editor/plan-description-editor-sn';
import ModalGeneric from "../../../shared-module-components/modal/modal-generic";
import SelectorCurrency from "../../selectors/selector-currency";
import { useHistory, useLocation } from "react-router-dom";
import { withLocalize } from "react-localize-redux";
import { Translate } from "react-localize-redux";
import styled from "styled-components";
import { WizardContext } from "../../modals/modal-subscription";
import { AppContext } from "../../../../App.Context";
import Settings from "../../../../custom-classes/settings";
import SessionManager from "../../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../../utils/utils";


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

const Step2 = ({ stepCompleted, setType, setPlanIdToWizard, isSchool, translate }) => {

  const { data } = useContext(WizardContext);
  const { state } = useContext(AppContext);
  const history = useHistory();
  let location = useLocation();
  let query = useQuery();
  let planIdfromQuery = query.get("planId") || (data ? data.planId : undefined);
  let clientId = query.get("userId");
  let entityIdQuery = query.get("entityId");
  const sessionDetails = SessionManager.getSessionDetails();

  const [modalPlanVisible, setModalPlanVisible] = useState(false);
  const [disableFields, setDisabledFields] = useState(false);
  const [loading, setLoading] = useState(false);
  const [languages, setLanguages] = useState([]);

  const [currentLangCode, setCurrentLangCode] = useState("");
  const [currentDescription, setCurrentDescription] = useState("");
  const [currentTitle, setCurrentTitle] = useState("");
  const [currentLangName, setCurrentLangName] = useState("");
  const [displayEditor, setDisplayEditor] = useState(false);
  const [validDescriptions, setValidDescriptions] = useState(false);
  const [interval, setInterval] = useState(null);
  const [viewPlan, setViewPlan] = useState(false);
  const [available, setAvailable] = useState(true);
  const [intervalCount, setIntervalCount] = useState(null);
  const [profilesQuantity, setProfilesQuantity] = useState(null);
  const [currency, setCurrency] = useState(null);
  const [name, setName] = useState(null);
  const [planId, setPlanId] = useState(null);
  const [planTaxesIds, setPlanTaxesIds] = useState([]);
  const [planDescriptionByLang, setPlanDescriptionsByLang] = useState([]);
  const [amount, setAmount] = useState(null);
  const [trialDays, setTrialDays] = useState(null);
  const [productId, setProductId] = useState(0);
  const [taxesSelected, setTaxesSelected] = useState([]);

  const [validProfilesQuantity, setValidProfilesQuantity] = useState(false);
  const [validAmount, setValidAmount] = useState(false);
  const [validCurrency, setValidCurrency] = useState(false);
  const [validIntervalBilling, setValidIntervalBilling] = useState(false);
  const [validPlanName, setValidPlanName] = useState(false);
  const [validTrialDays, setValidTrialDays] = useState(false);
  const [descriptionByLangValid, setDescriptionByLangValid] = useState(true);
  const [validPlanType, setValidPlanType] = useState(true);
  const [validProductId, setValidProductId] = useState(false);
  const [validIntervalCount, setValidIntervalCount] = useState(false);

  const [checkedPrivate, setCheckedPrivate] = useState(true);
  const [checkedPublic, setCheckedPublic] = useState(false);
  const [typePlan, setTypePlan] = useState(isSchool ? "1": "0");
  const [validSubmitButton, setValidSubmitButton] = useState(false);

  //when creating a plan we should display this modal
  const [messagesModal, setMessagesModal] = useState({
    visible: false,
    bodyText: ""
  });

  useEffect(() => {
    setValidSubmitButton(validProductId && validAmount && validCurrency && validDescriptions &&
        validPlanName && validIntervalBilling && validPlanType && validTrialDays && validProfilesQuantity && validIntervalCount)
  }, [validProductId, validAmount, validCurrency, validDescriptions, validPlanName, 
    validIntervalBilling, validPlanType, validTrialDays, validProfilesQuantity, validIntervalCount])

  useEffect(() => {
    if (isSchool) {
      setDisabledFields(true);
    }
    //language options for descriptions of plan
    searchLanguages();
    //in case the user has reloaded the page
    if (planIdfromQuery) {
      searchPlan();
    }
  }, []);

  const searchPlan = async () => {
    let plan = await getPlanDetails(planIdfromQuery)
    setValuePlan(plan, plan);
  }
  //function for searching languages for option buttons
  const searchLanguages = () => {
    let apiCall = getLanguages();
    apiCall.then((response) => {
      let options = [];
      response.data.forEach(lang => {
        options.push({
          name: lang.Desc,
          key: lang.Code,
          label: lang.Code
        })
      });
      //after searching languages, set to the state.
      setLanguages(options);
    });
  }
  //handlers
  const handleSelectedPlan = async (plan) => {

    setModalPlanVisible(false);
    if (plan) {
      let details = await getPlanDetails(plan.Id);
      setViewPlan(true);
      setValuePlan(plan, details);

      if (entityIdQuery) {
        history.push(`${location.pathname}?userId=${clientId}&planId=${plan.Id}&entityId=${entityIdQuery}&subscriptionType=${!isSchool ? "H" : "S"}`);
      } else {
        history.push(`${location.pathname}?userId=${clientId}&planId=${plan.Id}&subscriptionType=${!isSchool ? "H" : "S"}`);
      }
    }
  }
  const handleAmount = (event) => {
    const filteredAmount = event.currentTarget.value.replace(/[^0-9]/g, '');
    setAmount(filteredAmount);

    if (!isSchool) {
      const isValid = filteredAmount !== ""
      setValidAmount(isValid);
    } else {
      setValidAmount(true);
    }
  }
  const handleTrialDays = (event) => {
    const filteredTrialDays = event.currentTarget.value.replace(/[^0-9]/g, '');
    setTrialDays(filteredTrialDays);

    if (!isSchool) {
      const isValid = filteredTrialDays !== ""
      setValidTrialDays(isValid);
    } else {
      setValidTrialDays(true);
    }
  }
  const handleOnClickSearchPlan = () => {
    setModalPlanVisible(true);
  }
  const handleProduct = (value) => {
    setProductId(value)
    const isValid = value !== null
    setValidProductId(isValid);
  }
  const handleInterval = (value) => {
    setInterval(value)
    if (!isSchool) {
      const isValid = value !== null
      setValidIntervalBilling(isValid);
    } else {
      setValidIntervalBilling(true);
    }
  }
  const handleIntervalCount = (event) => {
    const filteredIntervalCount = event.currentTarget.value.replace(/[^0-9]/g, '');
    setIntervalCount(filteredIntervalCount);

    if (!isSchool) {
      const isValid = filteredIntervalCount !== ""
      setValidIntervalCount(isValid);
    } else {
      setValidIntervalCount(true);
    }
  }
  const handleCheckedTaxes = (event) => {
    setPlanTaxesIds(event);
  }

  const handleCheckedTypePlan = (e) => {
    const value = e.target.value;
    setTypePlan(value);
    setType(value);
  }

  const handleDescriptionSection = (e) => {

    let langName = languages.find(x => x.key === e.currentTarget.value).name
    setCurrentDescription(searchDescriptionTextByLangCode(e.currentTarget.value, planDescriptionByLang));
    setCurrentLangCode(e.currentTarget.value);
    setCurrentLangName(langName);
    setCurrentTitle(searchTitleTextByLangCode(e.currentTarget.value, planDescriptionByLang));

    //display editor
    setDisplayEditor(true)

  }
  const searchDescriptionTextByLangCode = (langCode, objectArray) => {
    for (let i = 0; i < objectArray.length; i++) {
      if (objectArray[i].LangCode === langCode)
        return objectArray[i].Description;
    }
    return '';
  }
  const searchTitleTextByLangCode = (langCode, objectArray) => {
    for (let i = 0; i < objectArray.length; i++) {
      if (objectArray[i].LangCode === langCode)
        return objectArray[i].Name;
    }
    return '';
  }
  const handleChangeDescriptionEditor = (data) => {
    const { languageCode, title, description } = data;
    updateDescriptionText(languageCode, title, description, () => {
      validateDescriptions(languages, planDescriptionByLang)
    })
  }
  const handleStudentQuantity = (event) => {
    const filteredStudentQuantity = event.currentTarget.value.replace(/[^0-9]/g, '');
    setProfilesQuantity(filteredStudentQuantity);

    if (!isSchool) {
      const isValid = filteredStudentQuantity !== ""
      setValidProfilesQuantity(isValid);
    } else {
      setValidProfilesQuantity(true);
    }
  }

  const validateDescriptions = (languages, planDescriptions) => {
    let valid = true;
    setValidDescriptions(true);
    if (planDescriptions.length < languages.length) {
      setValidDescriptions(false);
      valid = false;
    }

    for (let i = 0; i < planDescriptions.length; i++) {
      if (typeof planDescriptions[i].Description === 'undefined'
        || planDescriptions[i].Description === null
        || planDescriptions[i].Description.trim().length === 0
        || planDescriptions[i].Description === "<p>&nbsp;</p>"
        || typeof planDescriptions[i].Name === 'undefined'
        || planDescriptions[i].Name === null
        || planDescriptions[i].Name.trim().length === 0
        || planDescriptions[i].Name === "<p>&nbsp;</p>"
      ) {
        setValidDescriptions(false);
        valid = false;
        break;
      }
    }

    return valid;
  }
  const updateDescriptionText = (langCode, titleText, description, callback) => {
    var i = 0;
    var found = false;

    while (i < planDescriptionByLang.length && !found) {
      if (planDescriptionByLang[i].LangCode === langCode) {
        let planDescriptionByLangArray = [...planDescriptionByLang];

        planDescriptionByLangArray[i].Description = description;
        planDescriptionByLangArray[i].Name = titleText;

        setPlanDescriptionsByLang(planDescriptionByLangArray);
        found = true;
      }
      i++;
    }
    if (!found) {
      let DescriptionByLangItem = {
        LangCode: langCode,
        Name: titleText,
        Description: description
      };
      //add new description to Array.
      setPlanDescriptionsByLang(planDescriptionByLang => [...planDescriptionByLang, DescriptionByLangItem]);
    }
    setCurrentTitle(titleText);
    setCurrentDescription(description);

    callback();
  }
  const handleCurrency = (value) => {
    setCurrency(value)
    if (!isSchool) {
      const isValid = value !== null
      setValidCurrency(isValid);
    } else {
      setValidCurrency(true);
    }
  }
  const getPlanDetails = async (planId) => {
    setLoading(true);
    let apiCall = await getPlansAdvanced(planId, null, null, 1, 1, true,
        !isSchool);
    setLoading(false);
    return apiCall.data.Plans[0];
  }
  //most of the code was reused of existing module but this is not cool
  const setValuePlan = async (Plan, Details) => {

    if(typeof Plan === 'undefined' || Plan === null)
      return;

    if (Plan?.IsPrivate === "1") {
      setCheckedPrivate(true)
      setCheckedPublic(false)
    } else {
      setCheckedPublic(true)
      setCheckedPrivate(false)
    }

    setInterval(Plan.Interval);
    setName(Plan.Name);
    setPlanId(Plan.Id);
    setIntervalCount(Plan.IntervalCount);
    setCurrency(Plan.Currency);
    setProductId(Plan.ProductId);
    setPlanDescriptionsByLang(Details.PlanDescriptionByLang);
    setValidDescriptions(true);
    setPlanTaxesIds(Details.PlanTaxes);
    setProfilesQuantity(Details.ProfilesQuantity);
    setAmount(Plan.Amount);
    setTrialDays(Details.TrialDays);
    setAvailable((Plan.Available === "1"))
    setTypePlan(Plan.IsForSchool);
    setType(Plan.IsForSchool);

    setValidProductId(true);
    setValidPlanName(true);
    setValidAmount(true);
    setValidCurrency(true);
    setValidIntervalBilling(true);
    setValidIntervalCount(true);
    setValidTrialDays(true);
    setValidProfilesQuantity(true);
    setValidDescriptions(true);
    setValidPlanType(true);

    //disable fields and set
    setDisabledFields(true);
    stepCompleted(true);

    //send plan id to wizard
    setPlanIdToWizard(Plan.Id);
  }
  const validateFullPlan = (newPlan) => {
    //validate all fields
    let isCurrencyValid =  !isSchool ? !(currency === null) : true,
      isIntervalBillingValid =  !isSchool ? !(interval === null) : true,
      isAmountValid = !isSchool ? !(amount === null) : true,
      isPlanNameValid = !(name === null || name === ""),
      isProductIdValid = !(productId === 0 || productId === null),
      isTrialDaysValid = !isSchool ? (trialDays >= 0 && trialDays !== null) : true,
      isDescriptionsByLangValid = !isSchool ? validDescriptions : true,
      isValidPlanType =  !isSchool ? (typePlan ? true : false) : true,
      isProfilesQuantityValid = !isSchool ? !(profilesQuantity === 0 || profilesQuantity === null) : true;

    setValidCurrency(isCurrencyValid);
    setValidIntervalBilling(isIntervalBillingValid);
    setValidAmount(isAmountValid);
    setValidPlanName(isPlanNameValid);
    setValidProductId(isProductIdValid);
    setValidTrialDays(isTrialDaysValid);
    setDescriptionByLangValid(isDescriptionsByLangValid);
    setValidProfilesQuantity(isProfilesQuantityValid);
    setValidPlanType(isValidPlanType);


    const isPlanValid = (isCurrencyValid && isIntervalBillingValid && isAmountValid &&
      isPlanNameValid &&
      isProductIdValid &&
      isTrialDaysValid &&
      isProfilesQuantityValid &&
      isValidPlanType)

    return isPlanValid;
  }
  const submitPlan = async () => {
    if (validateFullPlan()) {
      setLoading(true);
      insertPlan(buildPlanModel(), !isSchool).then((response) => {
        let plan = response.data.Plans[0],
          messages = response.data.Messages;

        setViewPlan(true);
        setDisplayEditor(false);

        let messageToShow = '';
        messages.forEach((message) => {
          messageToShow += message + '<br/>';
        });
        //display messages into modal
        setMessagesModal({
          visible: true,
          bodyText: messageToShow
        });

        setPlanId(plan.Id)
        setPlanIdToWizard(plan.Id);
        stepCompleted(true);
        setLoading(false);
        setDisabledFields(true);

        if (entityIdQuery) {
          history.push(`${location.pathname}?userId=${clientId}&planId=${plan.Id}&entityId=${entityIdQuery}&subscriptionType=${!isSchool ? "H" : "S"}`);
        } else {

          history.push(`${location.pathname}?userId=${clientId}&planId=${plan.Id}&subscriptionType=${!isSchool ? "H" : "S"}`);
        }
      }).catch((error) => {
        let messages = error.data.Messages;
        setMessagesModal({
          visible: true,
          bodyText: messages[0]
        });
        setLoading(false);
      }).finally(() => {
        setLoading(false);
      });
    }
  }

  const buildPlanModel = () => {
    const cleanedPlanName = trimAndNormalizeText(name);

    return {
      ProductId: productId,
      Name: cleanedPlanName,
      Interval:  !isSchool ? interval : "year",
      IntervalCount: !isSchool ? intervalCount : "1",
      Currency:  !isSchool ? currency : "USD",
      Amount: !isSchool ? amount : "199",
      IsForSchool:  !isSchool ? typePlan : "1",
      IsPrivate:  !isSchool ? checkedPrivate ? "1" : "0" : "1",
      ProfilesQuantity: !isSchool ? profilesQuantity : "999999",
      Available: available ? "1" : "0",
      PlanTaxesIds: !isSchool ? taxesSelected : [],
      TrialDays:  !isSchool ? trialDays : 0,
      PlanDescriptionsByLang: !isSchool ? planDescriptionByLang : [{ "LangCode": "en", "Name": name, "Description": name }, { "LangCode": "es", "Name": name, "Description": name }],
      Id: planId
    };
  };

  const handlePlanName = (event) => {
    const planName = event.currentTarget.value;
    setName(planName);

    const isValid = planName !== "";
    setValidPlanName(isValid);
  }
  const handleCheckPlanAvailable = () => {
    setAvailable(!available)
  }
  const toggleAlertModal = () => {
    setMessagesModal({
      visible: false
    })
  }

  const trimAndNormalizeText = (text) => {
      return text.trim().replace(/\s{2,}/g, ' '); 
  };

  return (
    <>
      <div className="content-wizard">
        <h1 className="content-wizard--title">
        {!isSchool ? <Translate id="wizard.step-2.title" options={{ renderInnerHtml: true }} /> : <Translate id={((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && !state.variables.EnrollmentManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && !state.variables.EnrollmentManagementAdmin)) ? "wizard.step-2.title-dpr-read" : "wizard.step-2.title-dpr"} options={{ renderInnerHtml: true }} />}
        </h1>
        {data && data.planId ? null : (
            <button className="content-wizard--w-icon" type="button" onClick={handleOnClickSearchPlan}>
          {" "}
          <i className="fas fa-plus-circle" />
          {!isSchool ? <Translate id="wizard.step-2.select-existent-plan" options={{ renderInnerHtml: true }} /> : <Translate id="wizard.step-2.select-existent-plan-dpr" options={{ renderInnerHtml: true }} />}
          {" "}
        </button>)}
        <div className="wrapper-wizard-col mt-3 input-responsive">
          <div className="wrapper-wizard-col--col">
            <div className="wrapper-wizard-form">
              <div className="wrapper-wizard-form--col">
                <div className="form-wizard-fields wizard-fields__select">
                  <label>*
                    {!isSchool ? <Translate id="wizard.step-2.product" options={{ renderInnerHtml: true }} /> : <Translate id="wizard.step-2.product-dpr" options={{ renderInnerHtml: true }} />}
                  </label>
                  <SelectorPlanProduct disabled={disableFields} value={productId}
                    name="product" id="product" updateParent={handleProduct} isCommercial={!isSchool} />
                  {!validProductId &&
                    <TextDangerStyled>
                      {<Translate id="wizard.step-2.errors.product"
                        options={{ renderInnerHtml: true }} />}
                    </TextDangerStyled>
                  }
                </div>
              </div>

              <div className="wrapper-wizard-form--col">
                <div className="form-wizard-fields">
                  {!isSchool ? <label>*<Translate id="wizard.step-2.plan-name" options={{ renderInnerHtml: true }} />{" "}<small><Translate id="wizard.step-2.plan-name-example" options={{ renderInnerHtml: true }} /></small></label> : <label>*<Translate id="wizard.step-2.plan-name-dpr" options={{ renderInnerHtml: true }} /></label>}
                  <input type="text"
                    className="form-wizard-fields__input"
                    disabled={disableFields}
                    value={name}
                    onChange={handlePlanName} 
                    maxLength={100}
                    />
                  {!validPlanName &&
                    <TextDangerStyled>
                      {<Translate id="wizard.step-2.errors.plan-name"
                        options={{ renderInnerHtml: true }} />}
                    </TextDangerStyled>
                  }
                </div>
              </div>
            </div>

            {!isSchool && <div className="wrapper-wizard-form mt-2">
              <div className="form-wizard-fields form-wizard-fields__box">
                <input
                  type="checkbox"
                  id="plan-disponible"
                  name="plan-disponible"
                  checked={available}
                  disabled={disableFields}
                  onChange={handleCheckPlanAvailable}
                />
                <label for="plan-disponible">
                  {" "}
                  <Translate id="wizard.step-2.plan-available" options={{ renderInnerHtml: true }} />
                </label>
              </div>
            </div>}

            {!isSchool ?
              <div className="mt-4 clear-p">
                <p>
                  <Translate id="wizard.step-2.plan-type" options={{ renderInnerHtml: true }} />
                </p>
                <div className="wrapper-wizard-form">
                  <div className="form-wizard-fields form-wizard-fields__box">
                    <input
                      type="radio"
                      name="type_plan"
                      onChange={handleCheckedTypePlan}
                      value="1"
                      checked={typePlan === "1"}
                      disabled={true}
                    />
                    <label for="escuela">
                      <Translate id="wizard.step-2.plan-type-options.school" options={{ renderInnerHtml: true }} />
                    </label>
                  </div>
                  <div className="form-wizard-fields form-wizard-fields__box">
                    <input
                      type="radio"
                      name="type_plan"
                      onChange={handleCheckedTypePlan}
                      value="0"
                      checked={typePlan === "0"}
                      disabled={true}
                    />
                    <label for="cliente-regular">
                      <Translate id="wizard.step-2.plan-type-options.regular-client" options={{ renderInnerHtml: true }} />
                    </label>
                  </div>
                  {!validPlanType &&
                    <TextDangerStyled>
                      {<Translate id="wizard.step-2.errors.plan-type"
                        options={{ renderInnerHtml: true }} />}
                    </TextDangerStyled>
                  }
                </div>
              </div> : null}

            {!isSchool ?
              <div className="mt-4 clear-p">
                <p>
                  <Translate id="wizard.step-2.language-description" options={{ renderInnerHtml: true }} />
                </p>
                {
                  languages.map(item => (
                    <button className="content-wizard--w-icon mr-1" value={item.key} disabled={disableFields}
                      onClick={handleDescriptionSection}>
                      {item.name}</button>
                  ))
                }
                {!validDescriptions &&
                  <TextDangerStyled>
                    {<Translate id="wizard.step-2.errors.description-by-lang"
                      options={{ renderInnerHtml: true }} />}
                  </TextDangerStyled>
                }
              </div> : null}
            <div>
              {
                displayEditor ?
                  <PlanDescriptionEditorSN disabled={disableFields}
                    languageName={currentLangName}
                    title={currentTitle}
                    description={currentDescription}
                    languageCode={currentLangCode}
                    updateParent={handleChangeDescriptionEditor} />

                  : <></>
              }
            </div>
          </div>

          {!isSchool ?
            <div className="wrapper-wizard-col--col">
              <div className="wrapper-wizard-col--col__shadowCol">
                <div className="wrapper-wizard-form">
                  <div className="wrapper-wizard-form--col">
                    <div className="form-wizard-fields wizard-fields__select">
                      <label>*
                        <Translate id="wizard.step-2.currency" options={{ renderInnerHtml: true }} />
                      </label>
                      <SelectorCurrency disabled={disableFields} name="currency" id="currency"
                        value={currency}
                        updateParent={handleCurrency} />
                      {!validCurrency &&
                        <TextDangerStyled>
                          {<Translate id="wizard.step-2.errors.currency"
                            options={{ renderInnerHtml: true }} />}
                        </TextDangerStyled>
                      }
                    </div>
                  </div>

                  <div className="wrapper-wizard-form--col">
                    <div className="form-wizard-fields">
                      <label>
                        *<Translate id="wizard.step-2.price-per-unit" options={{ renderInnerHtml: true }} />{" "}
                        <span
                          className="tool"
                          data-tip={translate("wizard.step-2.price-per-unit-tip")}
                          tabindex="1"
                        >
                          <i className="fas fa-question-circle" />
                        </span>
                      </label>
                      <input type="text" className="form-wizard-fields__input" value={amount} disabled={disableFields} onChange={handleAmount} maxLength={8}/>
                      {!validAmount &&
                        <TextDangerStyled>
                          {<Translate id="wizard.step-2.errors.price-per-unit"
                            options={{ renderInnerHtml: true }} />}
                        </TextDangerStyled>
                      }
                    </div>
                  </div>
                </div>

                <div className="wrapper-wizard-form">
                  <div className="wrapper-wizard-form--col">
                    <div className="form-wizard-fields wizard-fields__select">
                      <label>
                        *<Translate id="wizard.step-2.billing-interval" options={{ renderInnerHtml: true }} />{" "}
                        <span
                          className="tool"
                          data-tip={translate("wizard.step-2.interval-tip")}
                          tabindex="2"
                        >
                          <i className="fas fa-question-circle" />
                        </span>
                      </label>
                      <SelectorBillingInterval value={interval} disabled={disableFields}
                        name="interval" id="interval"
                        updateParent={handleInterval} />
                      {!validIntervalBilling &&
                        <TextDangerStyled>
                          {<Translate id="wizard.step-2.errors.billing-interval"
                            options={{ renderInnerHtml: true }} />}
                        </TextDangerStyled>
                      }
                    </div>
                  </div>

                  <div className="wrapper-wizard-form--col">
                    <div className="form-wizard-fields">
                      <label>
                        *<Translate id="wizard.step-2.billing-interval-counter" options={{ renderInnerHtml: true }} />{" "}
                        <span
                          className="tool"
                          data-tip={translate("wizard.step-2.interval-example-tip")}
                          tabindex="1"
                        >
                          <i className="fas fa-question-circle" />
                        </span>
                      </label>
                      <input type="text" className="form-wizard-fields__input"
                        disabled={disableFields}
                        value={intervalCount}
                        onChange={handleIntervalCount}
                        maxLength={8}
                      />
                      {!validIntervalCount &&
                        <TextDangerStyled>
                          {<Translate id="wizard.step-2.errors.billing-interval-counter"
                            options={{ renderInnerHtml: true }} />}
                        </TextDangerStyled>
                      }
                    </div>
                  </div>
                </div>

                <div className="wrapper-wizard-form">
                  <div className="wrapper-wizard-form--col">
                    <div className="form-wizard-fields">
                      <label>
                        *<Translate id="wizard.step-2.trial-period" options={{ renderInnerHtml: true }} />{" "}
                        <span
                          className="tool"
                          data-tip={translate("wizard.step-2.quantity-of-days-trial-tip")}
                          tabindex="3"
                        >
                          <i className="fas fa-question-circle" />
                        </span>
                      </label>
                      <input type="text" className="form-wizard-fields__input"
                        disabled={disableFields}
                        value={trialDays}
                        onChange={handleTrialDays} 
                        maxLength={4}
                      />
                      {!validTrialDays &&
                        <TextDangerStyled>
                          {<Translate id="wizard.step-2.errors.trial-period"
                            options={{ renderInnerHtml: true }} />}
                        </TextDangerStyled>
                      }
                    </div>
                  </div>
                  <div className="wrapper-wizard-form--col">
                    <div className="form-wizard-fields">
                      <label>
                        *<Translate id="wizard.step-2.profile-quantity" options={{ renderInnerHtml: true }} />{" "}
                        <span
                          className="tool"
                          data-tip={translate("wizard.step-2.quantity-of-profiles-tip")}
                          tabindex="4">
                          <i className="fas fa-question-circle" />
                        </span>
                      </label>
                      <input type="text" className="form-wizard-fields__input"
                        disabled={disableFields}
                        value={profilesQuantity}
                        onChange={handleStudentQuantity} 
                        maxLength={4}
                      />
                      {!validProfilesQuantity &&
                        <TextDangerStyled>
                          {<Translate id="wizard.step-2.errors.profiles-quantity"
                            options={{ renderInnerHtml: true }} />}
                        </TextDangerStyled>
                      }
                    </div>
                  </div>
                </div>
                <div className="mt-2 clear-p">
                  <p>
                    <Translate id="wizard.step-2.taxes" options={{ renderInnerHtml: true }} />
                  </p>
                  <p>
                    <small>
                      <Translate id="wizard.step-2.taxes-description" options={{ renderInnerHtml: true }} />
                    </small>
                  </p>
                  <div className="wrapper-wizard-form style-checkboxes">
                    <CheckboxTaxes disabled={disableFields} selected={planTaxesIds} updateParent={handleCheckedTaxes} />
                  </div>
                </div>
              </div>
            </div> : null}
        </div>
      </div>

      {!isSchool ?
      <div className="content-wizard__footer">
        {disableFields ? null : (
            <button className="content-wizard--primary" onClick={submitPlan} disabled={!validSubmitButton}>
          {" "}
          <Translate id="wizard.step-2.submit" options={{ renderInnerHtml: true }} />{" "}
        </button>)}
      </div> : null}
      {messagesModal.visible && <ModalGeneric data={messagesModal}
        updateParent={toggleAlertModal} />}
      {modalPlanVisible && <ModalPlanSelection visible={modalPlanVisible}
        updateParent={handleSelectedPlan} isCommercial={!isSchool} />}
      {loading ? <LoadingFullWindow /> : null}
    </>
  );
};

export default withLocalize(Step2);
