import React, { useState, useEffect } from "react";
import Settings from "../../../../custom-classes/settings";
import {getPlanTotalAdmin, insertSubscriptionbyEmployeeV2} from "../../../../api/subscriptions";
import DatePicker from "../../../shared-module-components/date-picker/date-picker-recent";
import ModalGeneric from "../../../shared-module-components/modal/modal-generic";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import moment from "moment";
import { Translate } from "react-localize-redux";
import {Col, Input, Label, Modal, ModalFooter, ModalHeader, Row, Table} from "reactstrap";
import {CardElement, useElements, useStripe} from "@stripe/react-stripe-js";
import NumericInput from "react-numeric-input";
import LanguageManager from "../../../../custom-classes/language-manager";
import CalculationBox from "../../../guest-module/calculation-box/calculation-box-v2";
import CouponDataTable from "../../datatables/coupon-data-table";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSearch, faTrash} from "@fortawesome/free-solid-svg-icons";

function FormatDate(date) {
  return new Date(date).toISOString().split('T')[0]
}

const Step5 = ({ stepCompleted,
  entityAcademicDetailId,
  planId,
  userId,
  entityId,
  typeEntity,
  typePlan,
  setSubscriptionIdToWizard, isSchool }) => {

  const studentQuantityTranslation = LanguageManager.getTranslation("subscription-steps.students-quantity", Settings.LANGUAGES);
  const [studentQuantityTranslationState] = useState(studentQuantityTranslation);
  const [billingWithCode, setBillingWithCode] = useState(null);
  const [loading, setLoading] = useState(false);
  const [disabledFields, setDisabledFields] = useState(false);
  const [viewSubscriptionButton, setViewSubscriptionButton] = useState(false);
  const [expirationDate, setExpirationDate] = useState(new Date());
  const [messagesModal, setMessagesModal] = useState({
    visible: false,
    bodyText: ""
  });
  const [cardComplete, setCardComplete] = useState(false);
  const [stripeErrorMessage, setStripeErrorMessage] = useState(null);
  const stripe = useStripe();
  const elements = useElements();
  const [validCalculation, setValidCalculation] = useState(false);
  const [planCostDetails, setPlanCostDetails] = useState(null);
  const [studentsQuantity, setStudentsQuantity] = useState(1);
  const [couponPlatformId, setCouponPlatformId] = useState( null);
  const [showModalCoupon, setShowModalCoupon] = useState(false);
  const [isValidStudentQuantity, setIsValidStudentQuantity] = useState(true);

  const MIN_STUDENTS = 1;
  const MAX_STUDENTS = 5;

  useEffect(() => {
    if(!isSchool) {
      calculateCosts(planId, studentsQuantity, couponPlatformId);
    }
  }, []);

  const handleQuantityProfilesChange = (value) => {
    setStudentsQuantity(value);
    const isValid = value >= MIN_STUDENTS && value <= MAX_STUDENTS && !isNaN(value) && value !== null;
    setIsValidStudentQuantity(isValid);

    if (isValid) {
      calculateCosts(planId, value, couponPlatformId);
    }
  };

  const retrieveSubTokenStripe = (event) => {
    setCardComplete(event.complete);
    if (event.error) {
      setStripeErrorMessage(event.error.message);
    } else {
      setStripeErrorMessage(null);
    }
  };

  const handleCouponSelection = (e) => {
    setCouponPlatformId(e);
    calculateCosts(planId, studentsQuantity, e);
    setShowModalCoupon(false);
  }

  const toggleAlertModal = () => {
    setMessagesModal({
      visible: false
    })
  }

  const handleCheckedPlanAvailable = (e) => {
    const value = e.target.value;
    setBillingWithCode(value);
    setViewSubscriptionButton(true);
  }

  const handleSelectedExpirationDate = (value) => {
    if(moment(value.date) > moment())
    {
      setExpirationDate(value.date);
      if(isSchool){
        setViewSubscriptionButton(true);
      }
    }
  };

  const getSubscriptionModelForSchoolOrProgram = () => {
    let model = {
      SchoolId: typeEntity === Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID ? entityId : null,
      ProgramId: typeEntity === Settings.SCHOOL_MODULE_PROGRAM_TYPE_ID ? entityId : null,
      PlanId: planId,
      UserId: userId,
      Type: typeEntity === Settings.SCHOOL_MODULE_SCHOOL_TYPE_ID ? "S" : "T",
      Quantity: 1,
      PlatformCode: billingWithCode,
      CurrentPeriodEnd: FormatDate(expirationDate),
      SubTokenStripe: null,
      entityAcademicDetailId: entityAcademicDetailId
    }
    return model;
  }
  const getSubscriptionModelForClient = (paymentMethodId = null) => {
    let model = {
      PlanId: planId,
      UserId: userId,
      Quantity: studentsQuantity,
      SubTokenStripe: paymentMethodId,
      Type: typeof entityId === 'undefined' || entityId === null || entityId.trim() === "" ?  "H" : "S",
      PlatformCode: billingWithCode,
      CurrentPeriodEnd: FormatDate(expirationDate),
      CouponPlatformId: couponPlatformId
    }
    return model;
  }

  const createSubscription = async () => {
    try
    {
      let subscriptionModel = {}
      if(typeof entityId === 'undefined' || entityId === null || entityId.trim() === ""){
        subscriptionModel =  getSubscriptionModelForClient(null);
      }
      else
      {
        subscriptionModel =  getSubscriptionModelForSchoolOrProgram();
      }
      if(billingWithCode === Settings.BILL_SUB_STRIPE_CODE)
      {

        if (!stripe || !elements) {
          // Stripe.js has not yet loaded.
          const errorMessage = LanguageManager.getTranslation("commercial.stripe-not-loaded");
          setStripeErrorMessage(errorMessage);
          return;
        }

        const cardElement = elements.getElement(CardElement);
        if (!cardComplete || !cardElement) {
          const errorMessage = LanguageManager.getTranslation("commercial.complete-card-details");
          setStripeErrorMessage(errorMessage);
          setLoading(false);
          return;
        }

          const {paymentMethod, error: stripeError} = await stripe.createPaymentMethod({
            type: 'card',
            card: cardElement //,
            // billing_details: { // Optionally add billing details if collected
            //  name: `${firstName} ${lastName}`,
            // email: email
            // }
            //     address: {
            //         country: country,
            //         state: state,
            //         // Add other address fields if available in your form
            //     },
            // },
          });

          if (stripeError) {
            console.error('[Stripe createPaymentMethod error]', stripeError);
            setStripeErrorMessage(stripeError.message);
            setLoading(false);
            return;
          }
          subscriptionModel = getSubscriptionModelForClient(paymentMethod.id);
        }

      setLoading(true);

      const response = await insertSubscriptionbyEmployeeV2(subscriptionModel);
      let messages = response.data.Messages[0];
      const subscriptionId = response.data.Subscriptions[0].SubscriptionId;
      setViewSubscriptionButton(false);
      stepCompleted(true);
      setSubscriptionIdToWizard(subscriptionId);
      setDisabledFields(true);

      setMessagesModal({
        visible: true,
        bodyText: messages
      });

      setLoading(false);
    } catch (error) {
      let messages = error.response.data.Messages[0];

      setMessagesModal({
        visible: true,
        bodyText: messages
      });

      setLoading(false);
    }
  }

  const renderTotals = (costData) => {
    return(
        <CalculationBox costData={{costData}}/>
    );
  };

  const calculateCosts = async (selectedPlanId, selectedStudentQuantity, couponEntered) => {
    setLoading(true);

    if (selectedStudentQuantity > 0 && selectedPlanId !== null) {
      try {
        const response = await getPlanTotalAdmin(
            userId,
            selectedPlanId,
            selectedStudentQuantity,
            couponEntered
        );

        if (response.data !== null) {
          setValidCalculation(true);
        } else {
          setValidCalculation(false);
        }

        setPlanCostDetails(renderTotals(response.data));
      } catch (error) {
        console.error("Error calculating plan costs:", error);
        setValidCalculation(false);
        setPlanCostDetails(null);
        // Optionally show a user-friendly error message or log to external system
      } finally {
        setLoading(false);
      }
    } else {
      setPlanCostDetails(null);
      setValidCalculation(false);
      setLoading(false);
    }
  };

  const canCreateSubscription = () => {
    if (isSchool) {
      return viewSubscriptionButton;
    }

    if(billingWithCode === Settings.BILL_SUB_PRIVATE_CODE) {
      return viewSubscriptionButton && isValidStudentQuantity && expirationDate != null;
    }

    if(billingWithCode === Settings.BILL_SUB_STRIPE_CODE){
      // Commercial validations
      return viewSubscriptionButton && isValidStudentQuantity &&  cardComplete && billingWithCode === Settings.BILL_SUB_STRIPE_CODE;

    }

   };

  return (
    <>
      <div className="content-wizard">
        {Settings.IS_APP_MANAGED_BY_REQUIRED && !isSchool?
        <h1 className="content-wizard--title">
          <Translate id="wizard.step-5.title" options={{ renderInnerHtml: true }} />
        </h1> : null
        }
        {Settings.IS_APP_MANAGED_BY_REQUIRED && !isSchool ?
        <h4 className="mt-2 mb-3"><Translate id="wizard.step-5.stripe-billing" options={{ renderInnerHtml: true }} /></h4> : null}

        {Settings.IS_APP_MANAGED_BY_REQUIRED && !isSchool ?
        <div className="wrapper-wizard-form">
          <div className="form-wizard-fields form-wizard-fields__box">
            <input type="radio"
              name="plan_available"
              value={Settings.BILL_SUB_STRIPE_CODE}
              checked={billingWithCode == Settings.BILL_SUB_STRIPE_CODE}
              disabled={disabledFields}
              onChange={handleCheckedPlanAvailable}
            />
            <label for="plan-disponible">
              {" "}
              <Translate id="wizard.step-5.stripe-billing-description" options={{ renderInnerHtml: true }} />
            </label>
          </div>

          <div className="form-wizard-fields form-wizard-fields__box">
            <input type="radio"
              name="plan_available"
              value={Settings.BILL_SUB_PRIVATE_CODE}
              disabled={disabledFields}
              checked={!Settings.IS_APP_MANAGED_BY_REQUIRED ? true : billingWithCode == Settings.BILL_SUB_PRIVATE_CODE}
              onChange={handleCheckedPlanAvailable} />
            <label for="plan-disponible">
              {" "}
              <Translate id="wizard.step-5.no-stripe-billing-description" options={{ renderInnerHtml: true }} />
              {" "}
            </label>
          </div>
        </div> : null
        }
        {billingWithCode === Settings.BILL_SUB_PRIVATE_CODE || isSchool ?
        <div className="wrapper-wizard-form">
          <div className="form-wizard-fields form-wizard-fields__box mt-2">

                <div>
                  <label for="expiration-date"><Translate id="wizard.step-5.expiration-date-subscription" options={{ renderInnerHtml: true }} /> {" "}<small><Translate id="wizard.step-5.expiration-date-subscription-exp" options={{ renderInnerHtml: true }} /> </small> </label>
                </div>
                <DatePicker name="expiration-date" updateParent={handleSelectedExpirationDate} disabled={disabledFields} minDate={moment().add(1,'d')} />
          </div>
        </div> : null }
      </div>

      {billingWithCode !== null && billingWithCode === Settings.BILL_SUB_STRIPE_CODE &&
          <>
            <hr/>
            <Row>
              <Table striped variant="dark" responsive style={{marginTop: '1rem'}}>
                <tbody>
                <tr>
                  <td style={{width: '30%'}}>
                    <Label for="quantity" className="label-forms-blue">
                      <Translate id="guest.students-quantity" options={{renderInnerHtml: true}}/>
                    </Label>
                  </td>
                  <td style={{textAlign: "left"}}>
                    <NumericInput
                        style={{maxWidth: 200}}
                        value={studentsQuantity}
                        onChange={handleQuantityProfilesChange}
                        placeholder={studentQuantityTranslationState}
                        className="mantine-like__control"
                        strict={false}
                        mobile={'auto'}
                    />
                    {!isValidStudentQuantity && (
                      <div style={{color: 'red', fontSize: '12px', marginTop: '5px'}}>
                        <Translate id="wizard.step-5.quantity-range-error" />
                      </div>
                    )}
                  </td>
                </tr>

                <tr>
                  <td>
                    <Label for="coupon" className="label-forms-blue">
                      <Translate id="coupons.coupon" options={{renderInnerHtml: true}}/>
                    </Label>
                  </td>
                  <td>
                    <div style={{display: 'flex', alignItems: 'center', gap: '0.5rem'}}>
                      <Input
                          style={{maxWidth: 200}}
                          type="text"
                          value={couponPlatformId || ''}
                          readOnly={true}
                          disabled={true}
                          className="mantine-like__control"
                      />
                      <button
                          className="btn-info"
                          onClick={() => setShowModalCoupon(true)}
                          style={{height: '100%'}}
                      >
                        <FontAwesomeIcon icon={faSearch}/>
                      </button>
                      <button
                          className="btn-info btn-danger"
                          onClick={() => {
                            setCouponPlatformId(null);
                            calculateCosts(planId, studentsQuantity, null)}}
                          style={{height: '100%'}}
                      >
                        <FontAwesomeIcon icon={faTrash}/>
                      </button>
                    </div>
                  </td>
                </tr>

                <tr>
                  <td>
                    <span className="label-forms-blue">Total</span>
                  </td>
                  <td style={{textAlign: "left"}}>{planCostDetails}</td>
                </tr>
                </tbody>
              </Table>
            </Row>
            <Row>
              <Col xs={6}>
                <CardElement
                    id="card-element"
                    onChange={retrieveSubTokenStripe}
                    options={{
                      style: {
                        base: {
                          fontSize: '16px',
                          color: '#424770',
                          '::placeholder': {
                            color: '#aab7c4',
                          },
                        },
                        invalid: {
                          color: '#9e2146',
                        },
                      },
                    }}
                />
                {stripeErrorMessage && <div style={{color: 'red', marginTop: '10px'}}>{stripeErrorMessage}</div>}

              </Col>
            </Row>
          </>
      }
      <div className="content-wizard__footer">
        {viewSubscriptionButton &&
          <button 
            className="content-wizard--primary" 
            onClick={createSubscription}
            disabled={!canCreateSubscription()}>
            {" "}
            <Translate id="wizard.step-5.create-subscription" options={{ renderInnerHtml: true }} />
            {" "}
          </button>
        }
      </div>
      {messagesModal.visible && <ModalGeneric data={messagesModal}
        updateParent={toggleAlertModal} />}
      {loading ? <LoadingFullWindow /> : null}
      {showModalCoupon ?
      <Modal size="lg" isOpen={showModalCoupon} toggle={()=>{
        setShowModalCoupon(!showModalCoupon)
      }}>
        <ModalHeader toggle={() => {
            setShowModalCoupon(!showModalCoupon);
        }}/>
        <CouponDataTable redoSearch={true}
                         updateParent={handleCouponSelection}/>
        <ModalFooter/>
      </Modal> : null
      }
    </>
  );
};

export default Step5;
