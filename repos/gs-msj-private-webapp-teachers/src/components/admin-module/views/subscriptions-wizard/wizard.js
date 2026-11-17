import React, { useState, useContext } from "react";
import "./styles/tooltip.css";
import "./styles/tab-wizard.css";
import "./styles/wizard-content-steps.css";
import "./styles/wizard-progress-steps.css";
import Step1 from "./step1";
import Step2 from "./step2";
import Step3 from "./step3";
import Step4 from "./step4";
import Step5 from "./step5";
import Step6 from "./step6";
import Stepper from "@material-ui/core/Stepper";
import Step from "@material-ui/core/Step";
import StepLabel from "@material-ui/core/StepLabel";
import Button from "@material-ui/core/Button";
import Typography from "@material-ui/core/Typography";
import { WizardContext } from "../../modals/modal-subscription";
import { AppContext } from "../../../../App.Context";
import {useHistory, useLocation} from "react-router-dom";
import { Translate } from "react-localize-redux";
import ModalCancellationSubscription from "../../modals/modal-cancel-subscription";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faArrowLeft,
  faTimes,
} from '@fortawesome/free-solid-svg-icons';
import Settings from "../../../../custom-classes/settings";
import { isRoleAllowed } from "../../../../utils/utils";
import SessionManager from "../../../../custom-classes/session-manager";

const Wizard = ({subscriptionType}) => {

  const { data, toggle } = useContext(WizardContext);
  const { state } = useContext(AppContext);
  const steps = ["", "", "", "", "", ""];
  let history = useHistory();
  const location = useLocation();

  const [activeStep, setActiveStep] = useState(0);
  const [subscriptionId, setSubscriptionId] = useState(null);
  const [nextStep, setNextStep] = useState(false);
  const [userId, setUserId] = useState(false);
  const [planId, setPlanId] = useState(0);
  const [entityId, setEntity] = useState(() => {
    const params = new URLSearchParams(location.search);
    const urlEntityId = params.get('entityId');
    // If entityId is found in the URL, use it; otherwise, fall back to SessionManager
    return urlEntityId !== null ? urlEntityId : null;
  });
  const [type, setType] = useState(subscriptionType === "H" ? "0" : "1");
  const [typeEntityToWizard, setTypeEntityToWizard] = useState(1);
  const [entityAcademicDetailId, setEntityAcademicDetailId] = useState("");
  const [showCancellationModal, setShowCancellationModal] = useState(false);
  const sessionDetails = SessionManager.getSessionDetails();

  const handleNext = () => {


    if (activeStep == 1 && (type === "0")) {
      if (data && data.subscriptionId) {
        setActiveStep((prevActiveStep) => prevActiveStep + 4);
      } else {
        setActiveStep((prevActiveStep) => prevActiveStep + 3);
      }
    }
    else if (activeStep == 1 && data && data.subscriptionId) {
      setActiveStep((prevActiveStep) => prevActiveStep + 4);
    }
    else if(activeStep == 1 && data && subscriptionType === "H"){
      if (data && data.subscriptionId) {
        setActiveStep((prevActiveStep) => prevActiveStep + 4);
      }
      setActiveStep((prevActiveStep) => prevActiveStep + 2);
    }
    else if(activeStep === 2 && subscriptionType === "H"){
      if (data && data.subscriptionId) {
        setActiveStep((prevActiveStep) => prevActiveStep + 4);
      }
      setActiveStep((prevActiveStep) => prevActiveStep + 2);
    }

    else if (activeStep == 3 && (type === "1") && data && data.subscriptionId) {
      setActiveStep((prevActiveStep) => prevActiveStep + 2);
    } else {
      setActiveStep((prevActiveStep) => prevActiveStep + 1);
    }
    setNextStep(false);
  };
  const handleBack = () => {
    if (activeStep == 4 && (type === "0")) {
      setActiveStep((prevActiveStep) => prevActiveStep - 3);
    } else if (activeStep == 5 && (type === "0")) {
      setActiveStep((prevActiveStep) => prevActiveStep - 4);
    } else if (activeStep == 5 && (type === "1") && data && data.subscriptionId) {
      setActiveStep((prevActiveStep) => prevActiveStep - 2);
    } else {
      setActiveStep((prevActiveStep) => prevActiveStep - 1);
    }
  };
  const setEntityId = (entityId) => {
    setEntity(entityId);
  }
  const handleReset = () => {
    setActiveStep(0);
  };
  const getStepContent = (stepIndex) => {
    switch (stepIndex) {
      case 0:
        return <Step1 stepCompleted={setNextStep}
          setUserId={setUserId} isSchool={subscriptionType !== "H"} />;
      case 1:
        return <Step2 stepCompleted={setNextStep} isSchool={subscriptionType !== "H"}
          setPlanIdToWizard={setPlanId}
          setType={setType}
        />;
      case 2:
        return <Step3 stepCompleted={setNextStep}
          setTypeEntityToWizard={setTypeEntityToWizard}
                      isSchool={subscriptionType !== "H"}
          setEntityIdToWizard={setEntityId} />;
      case 3:
        return <Step4 stepCompleted={setNextStep}
          entityId={entityId}
                      isSchool={subscriptionType !== "H"}
          setEntityAcademicDetailId={setEntityAcademicDetailId}
        />;
      case 4:
        return <Step5 stepCompleted={setNextStep}
                      isSchool={subscriptionType !== "H"}
          entityAcademicDetailId={entityAcademicDetailId}
          planId={planId}
          userId={userId}
          entityId={entityId}
          typeEntity={typeEntityToWizard}
          typePlan={type}
          setSubscriptionIdToWizard={setSubscriptionId}
        />;
      case 5:
        return <Step6 stepCompleted={setNextStep}
                      isSchool={subscriptionType !== "H"}
          typePlan={type}
          schoolId={entityId}
          subscriptionId={subscriptionId} />;
      default:
        return null;
    }
  }

  const handleNextOrFinish = () => {
    if (activeStep === steps.length - 1) {
      if (data && data.subscriptionId) {
        toggle();
      } else {
          if (subscriptionType === "H") {
          history.push("/commercial-subscriptions-search?subscriptionType=H");
        } else {
          history.push("/school-entity-manage?entityId=" + entityId);
        }
      }
    } else {
      handleNext();
    }
  };

  const toggleCancellationModal = (value) => {
    setShowCancellationModal(value);
  };

  const goBackArrowButtonClick = () => {
    if (subscriptionType === "H") {
      history.push("/commercial-subscriptions-search?subscriptionType=H");
    } else {
      history.push("/school-entity-manage?entityId=" + entityId);
    }
  }

  return (
    <>
      <div className="wrapper-content-wizard">
        <div className="btn-back" style={{cursor: 'pointer'}} onClick={() => goBackArrowButtonClick()}>
          <FontAwesomeIcon icon={faArrowLeft}/>{' '}
          <Translate id="general.back-action" options={{renderInnerHtml: true}}/>
        </div>
        <div>
          <Stepper activeStep={activeStep} alternativeLabel className="steps">
            {steps.map((label) => (
                <Step key={label} className="steps_stept">
                  <StepLabel>{label}</StepLabel>
                </Step>
            ))}
          </Stepper>
          {data && data.subscriptionId && ((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && state.variables.EnrollmentManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && state.variables.EnrollmentManagementAdminOre)) ? (
              <div className="wrapper-button-sec-wizard btn-align-right">
                <Button className="btn_cancelation" variant="contained"
                        onClick={() => toggleCancellationModal(!showCancellationModal)}>
                  <Translate id="subscription-cancellation.title"/> <FontAwesomeIcon icon={faTimes}/>{' '}
                </Button>
              </div>) : null}
          <div>
            {activeStep === steps.length ? (
                <div>
                  <Typography>All steps completed</Typography>
                  <Button onClick={handleReset}>Reset</Button>
                </div>
            ) : (
                <div>
                  <Typography>{getStepContent(activeStep)}</Typography>
                  <div className="wrapper-button-sec-wizard">
                    {(activeStep === 0) || (activeStep === 5 && !data) || (activeStep === 4 && subscriptionId) ? null : (
                        <Button className="back" onClick={handleBack}>
                          <Translate id="wizard.container.back" options={{renderInnerHtml: true}}/>
                        </Button>)}
                    <Button className="next" variant="contained" color="primary" onClick={handleNextOrFinish}
                            disabled={!nextStep}>
                      {activeStep === steps.length - 1 ?
                          <Translate id="wizard.container.finish" options={{renderInnerHtml: true}}/> :
                          <Translate id="wizard.container.next" options={{renderInnerHtml: true}}/>}
                    </Button>
                  </div>
                </div>
            )}
          </div>
        </div>
      </div>
      {showCancellationModal
          ?
          <ModalCancellationSubscription
              disabled={false}
              cancelAtPeriodEnd={false}
              subscriptionId={data.subscriptionId}
              isEmployee={true}
              visible={showCancellationModal}
              updateParent={toggleCancellationModal}
        /> : null
      }
    </>
  );
};

export default Wizard;
