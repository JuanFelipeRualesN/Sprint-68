import React, { useState, useEffect, useContext } from "react";
import SelectorTerms from '../../admin-module/selectors/selector-terms';
import DatePicker from "../../shared-module-components/date-picker/date-picker-recent";
import DatePickerTerm from "../../shared-module-components/date-picker/date-picker-term";
import SelectorTermNumber from '../../admin-module/selectors/selector-term-number';
import { Translate } from "react-localize-redux";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { isRoleAllowed } from "../../../utils/utils";
import "react-datepicker/dist/react-datepicker.css";
import styled from "styled-components";
import { getDetailByAcademicDetails, createAcademicsDetails, modifyAcademicsDetails } from "../../../api/schools";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import moment from 'moment';
import { AppContext } from "../../../App.Context";

const TextDangerStyled = styled.p`
  color: #fa2626;
  font-size: 11px;
  margin: auto;
  padding: 0 0 0.25em 0;
  text-align: left;
  font-weight: bold;
`;

function FormatDate(date) {
  return new Date(date).toISOString().split('T')[0]
}

const AddDetails = ({ entityId, entityAcademicDetail, toggle }) => {
  const { state } = useContext(AppContext);
  const [term, setTerm] = useState("");
  const [termsQuantity, setTermsQuantity] = useState(0);
  const [beginAcademicYear, setBeginAcademicYear] = useState(moment().add(1, 'days').format("YYYY-MM-DD"));
  const [endAcademicYear, setEndAcademicYear] = useState(moment().add(2, 'days').format("YYYY-MM-DD"));
  const [daysNumberForClosingTerm, setDaysNumberForClosingTerm] = useState(0);
  const [dynamicTerms, setDynamicTerms] = useState([]);
  const [validTerm, setValidTerm] = useState(true);
  const [validTermsQuantity, setValidTermsQuantity] = useState(true);
  const [validAcademicRange, setValidAcademicRange] = useState(true);
  const [disabledFields, setDisabledFields] = useState(false);
  const [validDynamicMainRange, setValidDynamicMainRange] = useState(true);
  const [validDynamicSimpleRange, setValidDynamicSimpleRange] = useState(true);
  const [details, setDetails] = useState({});
  const [loading, setLoading] = useState(false);
  const sessionDetails = SessionManager.getSessionDetails();
  const [messagesModal, setMessagesModal] = useState({
    visible: false,
    bodyText: ""
  });

  const toggleAlertModal = () => {
    setMessagesModal({
      visible: false
    })
  }

  useEffect(() => {
    if (entityAcademicDetail && entityAcademicDetail.Id && entityId) {
      getAcademicsDetails(entityId, entityAcademicDetail.Id);
    }
  }, []);

  const getAcademicsDetails = async (entityId, entityAcademicDetailId) => {
    setLoading(true);
    try {
      const res = await getDetailByAcademicDetails(entityId, entityAcademicDetailId);
      const details = res.data;
      setDetails(details);
      setTerm(details.EntityAcademicDetailModel.TermId);
      setTermsQuantity(details.EntityAcademicDetailModel.PeriodQuantity);
      setDynamicTerms(details.AcademicPeriodModelList.map(item => {
        return {
          PeriodNumber: item.PeriodNumber,
          PeriodFrom: moment(item.PeriodFrom, "MM/DD/YYYY HH:mm:ss").format("YYYY-MM-DD"),
          PeriodTo: moment(item.PeriodTo, "MM/DD/YYYY HH:mm:ss").format("YYYY-MM-DD"),
          Id: item.Id,
        };
      }));
      setDaysNumberForClosingTerm(details.EntityAcademicDetailModel.DaysToClosePeriod);
      const starDate = moment(details.EntityAcademicDetailModel.FullTermStartDate, "MM/DD/YYYY HH:mm:ss");
      const endDate = moment(details.EntityAcademicDetailModel.FullTermEndDate, "MM/DD/YYYY HH:mm:ss");
      setBeginAcademicYear(starDate.format("YYYY-MM-DD"));
      setEndAcademicYear(endDate.format("YYYY-MM-DD"));
      if (((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && !state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && !state.variables.SchoolManagementAdminOre))) {
        setDisabledFields(true);
      }
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  const handleTermsChange = (value) => {
    setTerm(value);
  }
  const handleTermsQuantityChange = (value) => {

    if (entityAcademicDetail && entityAcademicDetail.Id) {
      const academicPeriods = details.AcademicPeriodModelList.map(item => {
        return {
          PeriodNumber: item.PeriodNumber,
          PeriodFrom: moment(item.PeriodFrom, "MM/DD/YYYY HH:mm:ss").format("YYYY-MM-DD"),
          PeriodTo: moment(item.PeriodTo, "MM/DD/YYYY HH:mm:ss").format("YYYY-MM-DD"),
          Id: item.Id,
        };
      });
      if (value >= academicPeriods.length) {
        const length = value - academicPeriods.length;
        let arrDynamics = [];

        for (var i = 0; i < length; i++) {
          arrDynamics.push({
            PeriodNumber: dynamicTerms.length + 1,
            PeriodFrom: moment().format("YYYY-MM-DD"),
            PeriodTo: moment().format("YYYY-MM-DD")
          });
        }
        const terms = academicPeriods.concat(arrDynamics);
        setDynamicTerms(terms);
        setTermsQuantity(value);
      }
    } else {
      let arrDynamics = [];
      for (var i = 0; i < value; i++) {
        arrDynamics.push({
          PeriodNumber: i + 1,
          PeriodFrom: moment().add(1, 'days').format("YYYY-MM-DD"),
          PeriodTo: moment().add(2, 'days').format("YYYY-MM-DD")
        });
      }
      setDynamicTerms(arrDynamics);
      setTermsQuantity(value);
    }
  }
  const handleDaysToCloseTerm = (value) => {
    setDaysNumberForClosingTerm(value);
  }
  const handleSelectedBeginAcademicYear = (value) => {
    setBeginAcademicYear(FormatDate(value.date));
  }
  const handleSelectedEndAcademicYear = (value) => {
    setEndAcademicYear(FormatDate(value.date));
  }

  const handleDynamicStartTerm = (value, id) => {
    let dynamicCopy = [...dynamicTerms];
    let dynamicCopyItem = dynamicCopy.find(e => e.PeriodNumber === id);
    const date = FormatDate(value.date);
    dynamicCopyItem.PeriodFrom = date;
    validateOverlappingDynamicTerms(dynamicCopy);
  }

  const handleDynamicEndTerm = (value, id) => {
    let dynamicCopy = [...dynamicTerms];
    let dynamicCopyItem = dynamicCopy.find(e => e.PeriodNumber === id);
    const date = FormatDate(value.date);
    dynamicCopyItem.PeriodTo = date;
    validateOverlappingDynamicTerms(dynamicCopy);
  }

  const validateOverlapping = (dynamicTerms) => {
    let overlapping = false;

    for (let item of dynamicTerms) {
      if (item.overlapping) {
        overlapping = true;
      }
    }
    return overlapping;
  };

  const validateOverlappingDynamicTerms = (dynamicTerms) => {
    let newArrayDinamics = [];

    for (let item of dynamicTerms) {
      let overlapping = false;
      const currentPeriodFrom = moment(item.PeriodFrom, "YYYY-MM-DD");
      const currentPeriodTo = moment(item.PeriodTo, "YYYY-MM-DD");
      for (let item2 of dynamicTerms) {
        const startDate = moment(item2.PeriodFrom, "YYYY-MM-DD");
        const endDate = moment(item2.PeriodTo, "YYYY-MM-DD");
        if (currentPeriodFrom.isBetween(startDate, endDate) || currentPeriodTo.isBetween(startDate, endDate)) {
          overlapping = true;
          break;
        }
      }
      newArrayDinamics.push({ ...item, overlapping });
    }
    setDynamicTerms(newArrayDinamics);
    validateOverlapping(newArrayDinamics);
  };

  const submitAcademicPeriods = () => {
    let validTerm = !(term == 0),
      validTermsQuantity = !(termsQuantity == 0),
      validAcademicRange = !(beginAcademicYear >= endAcademicYear),
      validDynamicTerm = true;

    setValidTerm(validTerm);
    setValidTermsQuantity(validTermsQuantity);
    setValidAcademicRange(validAcademicRange);

    let overlapping = validateOverlapping(dynamicTerms);

    var i = 0,
      invalidMainRange = false,
      invalidSimpleRange = false;
    do {
      var dynamicTerm = dynamicTerms[i];
      //range validation
      if (dynamicTerm.PeriodFrom >= dynamicTerm.PeriodTo) {
        invalidSimpleRange = true;
      }
      //validate period range part 1
      if (dynamicTerm.PeriodFrom < beginAcademicYear || dynamicTerm.PeriodFrom > endAcademicYear) {
        invalidMainRange = true
      }
      //validate period range part 2
      if (dynamicTerm.PeriodTo > endAcademicYear || dynamicTerm.PeriodTo < beginAcademicYear) {
        invalidMainRange = true;
      }
      i++;
    } while (i < dynamicTerms.length && (!invalidSimpleRange || !invalidMainRange));

    setValidDynamicSimpleRange(!invalidSimpleRange);
    setValidDynamicMainRange(!invalidMainRange);

    if (validAcademicRange
      && validTerm
      && validTermsQuantity
      && validDynamicTerm
      && !invalidSimpleRange
      && !invalidMainRange
      && !overlapping) {

      const detailsAcademics = {
        TermId: term,
        PeriodQuantity: termsQuantity,
        DaysToClosePeriod: daysNumberForClosingTerm,
        FullTermStartDate: FormatDate(beginAcademicYear),
        FullTermEndDate: FormatDate(endAcademicYear)
      };

      if (entityAcademicDetail && entityAcademicDetail.Id) {
        const req = {
          EntityAcademicDetail: { Id: entityAcademicDetail.Id, ...detailsAcademics },
          AcademicPeriods: dynamicTerms
        };
        updateEntityAcademicDetail(req, entityAcademicDetail.Id);
      } else {
        const req = {
          EntityAcademicDetail: { ...detailsAcademics },
          AcademicPeriods: dynamicTerms
        };
        createEntityAcademicDetail(req, entityId);        
      }
    }
  }

  const updateEntityAcademicDetail = async (req, entityAcademicDetailId) => {
    setLoading(true);
    try {
      await modifyAcademicsDetails(req, entityAcademicDetailId);      
      setLoading(false);
      toggle();
    } catch (error) {      
      setLoading(false);
    }
  };

  const createEntityAcademicDetail = async (req, entityId) => {
    setLoading(true);
    try {
      const response = await createAcademicsDetails(req, entityId);      
      let msg = "";
      if (response.data.Messages && response.data.Messages.length) {
        response.data.Messages.forEach((message) => {
          msg += message + "<br/>";
        });
      }
      setMessagesModal({
        visible: true,
        bodyText: msg
      });
      setLoading(false);
      toggle();
    } catch (error) {            
      setLoading(false);
    }
  };

  return (
    <>
      <div className="content-wizard">
        <h1 className="content-wizard--title">
          <Translate id="wizard.step-4.academic-details-title" options={{ renderInnerHtml: true }} />
        </h1>
        <div className="wrapper-wizard-form mt-3">
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*<Translate id="wizard.step-4.terms-input"
                options={{ renderInnerHtml: true }} />
              </label>
              <SelectorTerms
                value={term}
                updateParent={handleTermsChange}
                disabled={disabledFields}
              />
              {!validTerm &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-4.errors.select-terms-required"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*<Translate id="wizard.step-4.periods-quantity"
                options={{ renderInnerHtml: true }} />
              </label>
              <SelectorTermNumber
                value={termsQuantity}
                optionsNumber={Settings.DEFAULT_PERIODS_QUANTITY}
                updateParent={handleTermsQuantityChange}
                disabled={disabledFields} />

              {!validTermsQuantity &&
                <TextDangerStyled>
                  {<Translate id="wizard.step-4.errors.period-quantity"
                    options={{ renderInnerHtml: true }} />}
                </TextDangerStyled>
              }
            </div>
          </div>
        </div>
        <div className="wrapper-wizard-form mt-1">
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*<Translate id="wizard.step-4.begin-academic-year"
                options={{ renderInnerHtml: true }} />
              </label>
              <DatePicker
                date={beginAcademicYear ? moment(beginAcademicYear, "YYYY-MM-DD") : null}
                updateParent={handleSelectedBeginAcademicYear}
                disabled={disabledFields} />
            </div>

            {!validAcademicRange &&
              <TextDangerStyled>
                {<Translate id="wizard.step-4.errors.academic-range"
                  options={{ renderInnerHtml: true }} />}
              </TextDangerStyled>}
          </div>
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>*<Translate id="wizard.step-4.end-academic-year"
                options={{ renderInnerHtml: true }} />
              </label>
              <DatePicker
                date={endAcademicYear ? moment(endAcademicYear, "YYYY-MM-DD") : null}
                updateParent={handleSelectedEndAcademicYear}
                disabled={disabledFields} />
            </div>
          </div>
          <div className="wrapper-wizard-form--col">
            <div className="form-wizard-fields">
              <label>
                <Translate id="wizard.step-4.days-number-for-closing-term"
                  options={{ renderInnerHtml: true }} />
                {/* <span
                  className="tool"
                  data-tip="breve explicación de este item."
                  tabindex="1"
                >
                  <i className="fas fa-question-circle" />
                </span> */}
              </label>
              <SelectorTermNumber
                value={daysNumberForClosingTerm}
                optionsNumber={Settings.DEFAULT_TO_CLOSE_PERIODS_QUANTITY}
                updateParent={handleDaysToCloseTerm}
                disabled={disabledFields} />
            </div>
          </div>
          <hr />
        </div>
        <p>
          <Translate id="wizard.step-4.quantity-selected-periods"
            options={{ renderInnerHtml: true }} />
          <br />
          {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && state.variables.SchoolManagementAdminOre)) && <small>
            <Translate id="wizard.step-4.quantity-selected-periods-label"
              options={{ renderInnerHtml: true }} />
          </small>}
        </p>
        <div className="wrapper-wizard-form period-form">
          {!validDynamicSimpleRange &&
            <TextDangerStyled>
              {<Translate id="wizard.step-4.errors.dynamic-period-simple-range"
                options={{ renderInnerHtml: true }} />}
            </TextDangerStyled>}
          {!validDynamicMainRange &&
            <TextDangerStyled>
              {<Translate id="wizard.step-4.errors.dynamic-period-main-range"
                options={{ renderInnerHtml: true }} />}
            </TextDangerStyled>}
          {
            dynamicTerms.map((value, index) => (
              <div className="wrapper-wizard-form--col" key={index + value.PeriodNumber}>
                <label><Translate id="wizard.step-4.period"
                  options={{ renderInnerHtml: true }} /> {index + 1}</label>
                <div className="form-wizard-fields wizard-fields_datapicker-period">
                  <span className='datapicker_input-step'><DatePickerTerm disabled={disabledFields} date={value.PeriodFrom ? moment(value.PeriodFrom, "YYYY-MM-DD") : null} id={value.PeriodNumber} onChange={handleDynamicStartTerm} minDate={moment().add(1, 'days').format("YYYY-MM-DD")} /></span>
                  <span className='datapicker_input-step'><DatePickerTerm disabled={disabledFields} date={value.PeriodTo ? moment(value.PeriodTo, "YYYY-MM-DD") : null} id={value.PeriodNumber} onChange={handleDynamicEndTerm} minDate={moment().add(1, 'days').format("YYYY-MM-DD")} /></span>
                </div>
                {value.overlapping && <TextDangerStyled>
                  <Translate id="wizard.step-4.overlapping"
                    options={{ renderInnerHtml: true }} />
                </TextDangerStyled>}
              </div>
            ))
          }
        </div>
      </div>
      {((isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && state.variables.SchoolManagementAdmin) || (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && state.variables.SchoolManagementAdminOre)) && <div className="content-wizard__footer">
        {entityAcademicDetail && entityAcademicDetail.Id ? <button className="content-wizard--primary" type="submit" onClick={submitAcademicPeriods}>
          {" "}
          <Translate id="wizard.step-4.editDetailAcademics"
            options={{ renderInnerHtml: true }} />{" "}
        </button> :
          <button className="content-wizard--primary" type="submit" onClick={submitAcademicPeriods} disabled={disabledFields}>
            {" "}
            <Translate id="wizard.step-4.confirmDetailAcademics"
              options={{ renderInnerHtml: true }} />{" "}
          </button>}
      </div>}
      {messagesModal.visible && <ModalGeneric data={messagesModal}
        updateParent={toggleAlertModal} />}
      {loading ? <LoadingFullWindow /> : null}
    </>
  );
};

export default AddDetails;
