import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, Col } from 'reactstrap';
import { Translate } from "react-localize-redux";
import {getPlansAdvanced} from "../../../api/subscriptions";
import FormGroupPlan from '../forms/form-group-plan';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
/**
 * Modal for update a plan
 */
const ModalPlans = ({ isEdit, planId, visible, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);
    const [loading, setLoading] = useState(false);
    const [currencyIsoCode, setCurrencyIsoCode] = useState('');
    const [interval, setInterval] = useState('');
    const [intervalCount, setIntervalCount] = useState('');
    const [productId, setProductId] = useState('');
    const [planName, setPlanName] = useState('');
    const [price, setPrice] = useState('');
    const [validPrice, setValidPrice] = useState(true);
    const [tryPeriod, setTryPeriod] = useState('');
    const [isForSchoolChecked, setIsForSchoolChecked] = useState(false);
    const [isPrivate, setIsPrivate] = useState(false);
    const [studentQuantity, setStudentQuantity] = useState('');
    const [validStudentQuantity, setValidStudentQuantity] = useState(true);
    const [isAvailableChecked, setIsAvailableChecked] = useState(false);
    const [taxesSelected, setTaxesSelected] = useState([]);
    const [planDescriptionDetails, setPlanDescriptionDetails] = useState([]);
    const [validDescriptions, setValidDescriptions] = useState(true);
    const [planAvailableIsos, setPlanAvailableIsos] = useState([]);
    const [checkedValues, setCheckedValues] = useState([]);
    const [checkedRegionsValues, setCheckedRegionsValues] = useState([]);
    const [disabledGenerics, setDisabledGenerics] = useState(false);
    const [disabledPlanName, setDisabledPlanName] = useState(false);
    const [disabledPlanForSchools, setDisabledPlanForSchools] = useState(false);
    const [disabledPrivatePlan, setDisabledPrivatePlan] = useState(false);
    const [disabledProfilesQuantity, setDisabledProfilesQuantity] = useState(false);
    const [disabledPlanAvailability, setDisabledPlanAvailability] = useState(false);
    const [disabledPlanDescriptions, setDisabledPlanDescriptions] = useState(false);
    const [disabledRegionCountryIsos, setDisabledRegionCountryIsos] = useState(false);

    useEffect(() => {
        if(planId !== null) {
            doSearch();
        }
    }, [planId]);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent(newVisible);
    };

    // Call the results to display
    const doSearch = () => {
        setLoading(true);

        let apiCall = getPlansAdvanced(planId, null, null, 1, 1, true, null, true);
        apiCall.then((response) => {
            let Plan = response.data.Plans[0];

            // Prepare the descriptions
            let planDescriptionArray = [];
            for(let i=0; i < Plan.PlanDescriptionByLang.length;i++) {
                planDescriptionArray.push({
                    langCode: Plan.PlanDescriptionByLang[i].LangCode,
                    title: Plan.PlanDescriptionByLang[i].Name,
                    description: Plan.PlanDescriptionByLang[i].Description
                });
            }

            // Prepare taxes
            let planTaxes = [];
            for(let i=0; i < Plan.PlanTaxes.length;i++) {
                planTaxes.push(Plan.PlanTaxes[i].TaxId);
            }

            // Prepare the countries
            let countriesSelected = [];
            let regionsSelected = [];

            for (let i=0; i < Plan.PlanAvailableIsos.length; i++) {
                if(Plan.PlanAvailableIsos[i].StateIso !== null){
                    regionsSelected.push(Plan.PlanAvailableIsos[i].StateIso + "|" + Plan.PlanAvailableIsos[i].CountryIso);
                    continue; // Continue. Do not add the country alone.
                }

                if(Plan.PlanAvailableIsos[i].CountryIso !== null) {
                    countriesSelected.push(Plan.PlanAvailableIsos[i].CountryIso);
                }
            }

            setCurrencyIsoCode(Plan.Currency);
            setInterval(Plan.Interval);
            setIntervalCount(Plan.IntervalCount);
            setProductId(Plan.ProductId);
            setPlanName(Plan.Name);
            setPrice(Plan.Amount);
            setValidPrice(true);
            setTryPeriod(Plan.TrialDays);
            setIsForSchoolChecked(Plan.IsForSchool === "1");
            setIsPrivate(Plan.IsPrivate === "1");
            setStudentQuantity(Plan.ProfilesQuantity);
            setValidStudentQuantity(true);
            setIsAvailableChecked(Plan.Available === "1");
            setTaxesSelected(planTaxes); // Prepare taxes object
            setPlanDescriptionDetails(planDescriptionArray);
            setValidDescriptions(true);
            setPlanAvailableIsos([]); // Prepare object
            setCheckedValues(countriesSelected); // Set checked countries
            setCheckedRegionsValues(regionsSelected); // Set checked regions
            setDisabledGenerics(true);
            setDisabledPlanName(false);
            setDisabledPlanForSchools(false);
            setDisabledPrivatePlan(false);
            setDisabledProfilesQuantity(false);
            setDisabledPlanAvailability(false);
            setDisabledPlanDescriptions(false);
            setDisabledRegionCountryIsos(false);
            setLoading(false);
        });
    };

    const handlePlanCreated = (data) => {};

    const state = {
        isEdit,
        planId,
        loading,
        currencyIsoCode,
        interval,
        intervalCount,
        productId,
        planName,
        price,
        validPrice,
        tryPeriod,
        isForSchoolChecked,
        isPrivate,
        studentQuantity,
        validStudentQuantity,
        isAvailableChecked,
        taxesSelected,
        planDescriptionDetails,
        validDescriptions,
        planAvailableIsos,
        checkedValues,
        checkedRegionsValues,
        disabledGenerics,
        disabledPlanName,
        disabledPlanForSchools,
        disabledPrivatePlan,
        disabledProfilesQuantity,
        disabledPlanAvailability,
        disabledPlanDescriptions,
        disabledRegionCountryIsos,
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col style={{paddingLeft: 40, paddingRight: 40}}>
                            <FormGroupPlan data={state} updateParent={handlePlanCreated}/>
                        </Col>
                    </Row>

                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="general.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
            {loading ?
            <LoadingFullWindow /> : null
            }
        </div>
    );
};

export default ModalPlans;