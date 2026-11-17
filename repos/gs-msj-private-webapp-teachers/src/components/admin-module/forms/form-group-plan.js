import React, { useState, useEffect } from 'react';
import {Input, Col, Label, Alert, Row, Button} from 'reactstrap';
import {Translate} from "react-localize-redux";
import {getPlansAdvanced, insertPlan, updatePlan} from "../../../api/subscriptions";
import {getLanguages} from "../../../api/gsweb";
import SelectorCurrency from '../selectors/selector-currency';
import SelectorBillingInterval from '../selectors/selector-billing-interval';
import SelectorPlanProduct from '../selectors/selector-plan-product';
import CheckboxTaxes from '../selectors/multi-selector-taxes';
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import PlanDescriptionEditorSN from '../html-editor/plan-description-editor-sn';
import ModalPlanSelection from '../modals/modal-plan-selection';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import Settings from "../../../custom-classes/settings";

/**
 * Class to handle the created plans
 */
const FormGroupCommercialPlans = (props) => {
    const [checkedValues, setCheckedValues] = useState(props.data.checkedValues ?? []);
    const [currencyIsoCode, setCurrencyIsoCode] = useState(props.data.currencyIsoCode ?? "USD");
    const [disabledGenerics, setDisabledGenerics] = useState(props.data.disabledGenerics);
    const [disabledPlanAvailability, setDisabledPlanAvailability] = useState(props.data.disabledPlanAvailability);
    const [disabledPlanDescriptions, setDisabledPlanDescriptions] = useState(props.data.disabledPlanDescriptions);
    const [disabledPlanName, setDisabledPlanName] = useState(props.data.disabledPlanName);
    const [disabledPrivatePlan, setDisabledPrivatePlan] = useState(props.data.disabledPrivatePlan);
    const [disabledProfilesQuantity, setDisabledProfilesQuantity] = useState(props.data.disabledProfilesQuantity);
    const [interval, setInterval] = useState(props.data.interval ?? "month");
    const [intervalCount, setIntervalCount] = useState(props.data.intervalCount ?? 1);
    const [isAvailableChecked, setIsAvailableChecked] = useState(props.data.isAvailableChecked !== undefined ? props.data.isAvailableChecked : true);
    const [isEdit, setIsEdit] = useState(props.data.isEdit ?? false);
    const [isPrivate, setIsPrivate] = useState(props.data.isPrivate ?? false);
    const [isReadOnly, setIsReadOnly] = useState(props.data.isReadOnly ?? false);
    const [languages, setLanguages] = useState([]);
    const [loading, setLoading] = useState(false);
    const [modalPlanVisible, setModalPlanVisible] = useState(false);
    const [planDescriptionCurrentDescription, setPlanDescriptionCurrentDescription] = useState(null);
    const [planDescriptionCurrentLangCode, setPlanDescriptionCurrentLangCode] = useState(null);
    const [planDescriptionCurrentLangName, setPlanDescriptionCurrentLangName] = useState(null);
    const [planDescriptionCurrentTitle, setPlanDescriptionCurrentTitle] = useState(null);
    const [planDescriptionDetails, setPlanDescriptionDetails] = useState(props.data.planDescriptionDetails ?? []);
    const [planDescriptionShowModal, setPlanDescriptionShowModal] = useState(false);
    const [planId, setPlanId] = useState(props.data.planId ?? null);
    const [planName, setPlanName] = useState(props.data.planName ?? '');
    const [planSubmitEnabled, setPlanSubmitEnabled] = useState(props.data.planSubmitEnabled ?? false);
    const [price, setPrice] = useState(props.data.price ?? null);
    const [productId, setProductId] = useState(props.data.productId ?? null);
    const [selectAll, setSelectAll] = useState(props.data.selectAll ?? false);
    const [showAddExistingPlanButton, setShowAddExistingPlanButton] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [studentQuantity, setStudentQuantity] = useState(props.data.studentQuantity ?? 1);
    const [taxesSelected, setTaxesSelected] = useState(props.data.taxesSelected ?? []);
    const [tryPeriod, setTryPeriod] = useState(props.data.tryPeriod ?? null);
    const [validDescriptions, setValidDescriptions] = useState(props.data.validDescriptions ?? false);
    const [validPrice, setValidPrice] = useState(props.data.validPrice ?? false);
    const [validStudentQuantity, setValidStudentQuantity] = useState(props.data.validStudentQuantity ?? true);
    const [propsModal, setPropsModal] = useState(null);

    useEffect(() => {
        if (typeof props.data !== 'undefined' && props.data.isEdit) {
            setCheckedValues(props.data.checkedValues || []);
            setCurrencyIsoCode(props.data.currencyIsoCode || null);
            setDisabledGenerics(props.data.disabledGenerics);
            setDisabledPlanAvailability(props.data.disabledPlanAvailability);
            setDisabledPlanDescriptions(props.data.disabledPlanDescriptions);
            setDisabledPlanName(props.data.disabledPlanName);
            setDisabledPrivatePlan(props.data.disabledPrivatePlan);
            setDisabledProfilesQuantity(props.data.disabledProfilesQuantity);
            setInterval(props.data.interval || null);
            setIntervalCount(props.data.intervalCount || 1);
            setIsAvailableChecked(
            props.data.isAvailableChecked !== undefined ? props.data.isAvailableChecked : true
            );
            setIsEdit(props.data.isEdit || false);
            setIsPrivate(props.data.isPrivate || false);
            setIsReadOnly(props.data.isReadOnly || false);
            setLoading(false);
            setPlanDescriptionDetails(props.data.planDescriptionDetails || []);
            setPlanId(props.data.planId || null);
            setPlanName(props.data.planName || '');
            setPlanSubmitEnabled(props.data.planSubmitEnabled || false);
            setPrice(props.data.price || null);
            setProductId(props.data.productId || null);
            setSelectAll(props.data.selectAll || false);
            setShowAddExistingPlanButton(false);
            setShowModal(false);
            setStudentQuantity(props.data.studentQuantity || 1);
            setTaxesSelected(props.data.taxesSelected || []);
            setTryPeriod(props.data.tryPeriod || null);
            setValidDescriptions(props.data.validDescriptions || false);
            setValidPrice(props.data.validPrice || false);
            setValidStudentQuantity(props.data.validStudentQuantity || true);
        }
    }, [props.data]);

    useEffect(() => {
        searchLanguages();
    }, []);

    const searchLanguages = () => {
        // This is promise
        let apiCall = getLanguages();

        // After the api call, fill the data
        apiCall.then((response) => {

            let options = [];
            let dataReturned = response.data;

            // Now fill the selections
            for (let i = 0; i < dataReturned.length; i++) {
                options.push({
                    name: dataReturned[i].Desc,
                    key: dataReturned[i].Code,
                    label: dataReturned[i].Code
                });
            }
            setLanguages(options);
            return options;
        });
    };

    // Render to add the descriptions
    const renderDescriptions = () => {
        return (
            <Row>
                {
                    languages.map(item => (
                        <Col sm={{ size: 3 }} key={item.key}>
                            <button block className="btn-default button-secondary-custom" value={item.key}
                                    disabled={disabledPlanDescriptions}
                                    onClick={openPlanDescriptionModal}>
                                {item.name}
                            </button>
                        </Col>
                    ))
                }
            </Row>
        );
    };

    // To open the lang description modal
    const openPlanDescriptionModal = (e) => {
        const langCode = e.currentTarget.value;

        setPlanDescriptionCurrentLangCode(langCode);
        setPlanDescriptionCurrentTitle(searchTitleTextByLangCode(langCode, planDescriptionDetails));
        setPlanDescriptionCurrentDescription(searchDescriptionTextByLangCode(langCode, planDescriptionDetails));

        setPlanDescriptionCurrentLangName(languages.find((item) => item.key === langCode)?.name);
        setPlanDescriptionShowModal(true);
    };

    // Search the description by the lang code
    const searchDescriptionTextByLangCode = (langCode, objectArray) => {
        for (let i = 0; i < objectArray.length; i++) {
            if (objectArray[i].langCode === langCode)
                return objectArray[i].description;
        }
        return '';
    };

    // Search the title text by the lang code
    const searchTitleTextByLangCode = (langCode, objectArray) => {
        for (let i = 0; i < objectArray.length; i++) {
            if (objectArray[i].langCode === langCode)
                return objectArray[i].title;
        }
        return '';
    };

    // Updates the title text
    const updateDescriptionText = (langCode, titleText, description, callback) => {
        let found = false;
        for (let i = 0; i < planDescriptionDetails.length; i++) {
            if (planDescriptionDetails[i].langCode === langCode) {
                planDescriptionDetails[i].title = titleText;
                planDescriptionDetails[i].description = description;
                setPlanDescriptionCurrentTitle(planDescriptionDetails[i].title);
                setPlanDescriptionCurrentDescription(planDescriptionDetails[i].description);
                found = true;
            }
        }
        if (!found) {
            // Add the new description
            let objectDescription = {
                langCode: langCode,
                title: titleText,
                description: description
            };
            setPlanDescriptionCurrentTitle(titleText);
            setPlanDescriptionCurrentDescription(description);
            planDescriptionDetails.push(objectDescription);
        }
        callback();
    };


    //Submits the plan to the server
    const submitPlan = async () => {
        if (planSubmitEnabled) {
            setLoading(true);

            try {
                let apiCall;

                if (!isEdit)
                    apiCall = insertPlan(prepareJsonSubmition(), true);
                else
                    apiCall = updatePlan(prepareJsonSubmition(), true);

                const response = await apiCall;

                let messageToShow = '';
                for (let i = 0; i < response.data.Messages.length; i++) {
                    messageToShow += response.data.Messages[i] + '<br/>';
                }

                const propsModalObj = {
                    bodyText: messageToShow,
                    visible: true
                };


                if (response.status === 200 || response.status === 201) {
                    if (isEdit) {
                        setPropsModal(propsModalObj);
                        setShowModal(true);
                    } else {
                        setPlanId(response.data.Plans[0].Id);
                        setPropsModal(propsModalObj);
                        setShowModal(true);
                        setDisabledGenerics(true);
                        setDisabledPlanName(true);
                        setDisabledPrivatePlan(true);
                        setDisabledProfilesQuantity(true);
                        setDisabledPlanAvailability(true);
                        setDisabledPlanDescriptions(true);
                        setPlanSubmitEnabled(false);

                        // Update parent and send the plan Id
                        props.updateParent({
                            checkedValues,
                            currencyIsoCode,
                            disabledGenerics,
                            disabledPlanAvailability,
                            disabledPlanDescriptions,
                            disabledPlanName,
                            disabledPrivatePlan,
                            disabledProfilesQuantity,
                            interval,
                            intervalCount,
                            isAvailableChecked,
                            isEdit,
                            isPrivate,
                            isReadOnly,
                            languages,
                            loading,
                            modalPlanVisible,
                            planDescriptionCurrentDescription,
                            planDescriptionCurrentLangCode,
                            planDescriptionCurrentLangName,
                            planDescriptionCurrentTitle,
                            planDescriptionDetails,
                            planDescriptionShowModal,
                            planId,
                            planName,
                            planSubmitEnabled,
                            price,
                            productId,
                            propsModal,
                            selectAll,
                            showAddExistingPlanButton,
                            showModal,
                            studentQuantity,
                            taxesSelected,
                            tryPeriod,
                            validDescriptions,
                            validPrice,
                            validStudentQuantity
                        });
                    }
                } else {
                    setPropsModal(propsModalObj);
                    setShowModal(true);
                }
            } catch (error) {
                setPropsModal({
                    bodyText: "An unexpected error occurred. Please try again later.",
                    visible: true
                });
                setShowModal(true);
                console.error(error);
            } finally {
                setLoading(false);
            }
        }
    };

    const prepareJsonSubmition = () => {
        let PlanCreationModel = {
                ProductId: productId,
                Name: planName,
                Interval: interval,
                IntervalCount: intervalCount,
                Currency: currencyIsoCode,
                Amount:  price,
                IsPrivate: (isPrivate ? "1" : "0"),
                ProfilesQuantity: studentQuantity,
                Available: isAvailableChecked ? "1" : "0",
                PlanTaxesIds: taxesSelected,
                TrialDays: tryPeriod,
                PlanDescriptionsByLang: [],
                IsForSchool: "0",
                Id: planId
        };

        for (let i = 0; i < planDescriptionDetails.length; i++) {
                PlanCreationModel.PlanDescriptionsByLang.push({
                    LangCode: planDescriptionDetails[i].langCode,
                    Name: planDescriptionDetails[i].title,
                    Description: planDescriptionDetails[i].description
                });
            }

        return PlanCreationModel;
    };

    const validateDescriptions = (languages, planDescriptions) => {
        let valid = true;
        setValidDescriptions(true);

        if (planDescriptions.length < languages.length) {
            setValidDescriptions(false);
            valid = false;
        }

        for (let i = 0; i < planDescriptions.length; i++) {
            if (
                typeof planDescriptions[i].description === 'undefined' ||
                planDescriptions[i].description === null ||
                planDescriptions[i].description.trim().length === 0 ||
                planDescriptions[i].description === "<p>&nbsp;</p>" ||
                typeof planDescriptions[i].title === 'undefined' ||
                planDescriptions[i].title === null ||
                planDescriptions[i].title.trim().length === 0 ||
                planDescriptions[i].title === "<p>&nbsp;</p>"
            ) {
                setValidDescriptions(false);
                valid = false;
                break;
            }
        }

        return valid;
    };

    // Evaluates if the plan is ready for submission
    const checkIfSubmitPlanButtonReady = () => {

            if (
                validDescriptions &&
                currencyIsoCode !== null &&
                currencyIsoCode !== '' &&
                currencyIsoCode !== '0' &&
                productId !== null &&
                planName !== null &&
                planName.trim() !== '' &&
                validPrice &&
                tryPeriod !== null &&
                validStudentQuantity &&
                validDescriptions &&
                studentQuantity !== '0' &&
                intervalCount !== '0'
            ) {
                setPlanSubmitEnabled(true);
            } else {
                setPlanSubmitEnabled(false);
            }
    };

    useEffect(() => {
        checkIfSubmitPlanButtonReady();
    }, [currencyIsoCode, productId, planName, price, tryPeriod, validPrice, validStudentQuantity, validDescriptions,
        studentQuantity, intervalCount]);

    // Handles the change for the html editors
    const handleChangeDescriptionEditor = (data) => {
        updateDescriptionText(data.languageCode, data.title, data.description, () => {
            validateDescriptions(languages, planDescriptionDetails);
        });
        checkIfSubmitPlanButtonReady();
    };

    // Handles the checked taxes
    const handleCheckedTaxes = (event) => {
        setTaxesSelected(event);
        checkIfSubmitPlanButtonReady();
    };

    const handleOnClickSearchPlan = () => {
        setModalPlanVisible(true);
    };

    // Handles the plan availability checkbox when is checked
    const handlePlanAvailability = () => {
        setIsAvailableChecked(prevState => {
            const newValue = !prevState;
            checkIfSubmitPlanButtonReady();
            return newValue;
        });
    };

    // Handles the student quantity input is changed
    const handleStudentQuantity = (event) => {
        const value = event.currentTarget.value;

        if (value === null || value === '') {
            setValidStudentQuantity(false);
            setStudentQuantity('');
            checkIfSubmitPlanButtonReady();
        } else {
            if (isNormalInteger(value)) {
                setValidStudentQuantity(true);
                setStudentQuantity(value);
                checkIfSubmitPlanButtonReady();
            }
        }
    };

    // Handles when the private checkbox is checked
    const handleIsPrivate = (e) => {
        setIsPrivate(e.currentTarget.checked);
        checkIfSubmitPlanButtonReady();
    };

    // Handles when the currency is changed
    const handleCurrencyChange = (value) => {
        setCurrencyIsoCode(value);
        checkIfSubmitPlanButtonReady();
    };

    // Handles when interval count component is changed.
    const handleIntervalCount = (value) => {
        setIntervalCount(value.currentTarget.value);
        checkIfSubmitPlanButtonReady();
    };

    // Handles when interval is changed
    const handleInterval = (value) => {
        setInterval(value);
        checkIfSubmitPlanButtonReady();
    };

    // Handles when the product id is changed
    const handleProduct = (value) => {
        setProductId(value);
        checkIfSubmitPlanButtonReady();
    };

    // Handles when the plan name is changed
    const handleNameChange = (event) => {
        setPlanName(event.currentTarget.value);
        checkIfSubmitPlanButtonReady();
    };

    // Handles when the price input is changed
    const handlePriceChange = (event) => {
        const value = event.currentTarget.value;

        if (isNormalInteger(value)) {
            setPrice(value);
            setValidPrice(true);
        } else {
            setPrice(null);
            setValidPrice(false);
        }

        checkIfSubmitPlanButtonReady();
    };

    // Handle the try period
    const handleTry = (event) => {
        const value = event.currentTarget.value;

        if (isNormalInteger(value)) {
            setTryPeriod(value);
        } else {
            setTryPeriod(null);
        }

        checkIfSubmitPlanButtonReady();
    };

    // Evaluates if its a positive integer.
    const isNormalInteger = (str) => {
        let n = Math.floor(Number(str));
        return n !== Infinity && String(n) === str && n >= 0;
    };


    // Renders the generic modal
    const renderGenericModal = () => {
        return (
            <ModalGeneric data={propsModal} updateParent={toggleAlertModal} />
        );
    };

    const toggleAlertModal = () => {
        setShowModal(false)
    }

    const handleSelectedPlan = (e) => {
        setModalPlanVisible(false);
        const selectedPlan = e !== null ? e : null;
        setPlanId(selectedPlan);

        if (selectedPlan !== null) {
            setLoading(true);

            getPlansAdvanced(selectedPlan.Id, null, null, 1, 1, true).then((response) => {
                let Plan = response.data.Plans[0];

                // Prepare the descriptions
                let planDescriptionArray = [];

                for (let i = 0; i < Plan.PlanDescriptionByLang.length; i++) {
                    planDescriptionArray.push({
                        langCode: Plan.PlanDescriptionByLang[i].LangCode,
                        title: Plan.PlanDescriptionByLang[i].Name,
                        description: Plan.PlanDescriptionByLang[i].Description
                    })
                }

                // Prepare taxes
                let planTaxes = [];
                for (let i = 0; i < Plan.PlanTaxes.length; i++) {
                    planTaxes.push(Plan.PlanTaxes[i].TaxId);
                }

                setCurrencyIsoCode(Plan.Currency);
                setInterval(Plan.Interval);
                setProductId(Plan.ProductId);
                setPlanName(Plan.Name);
                setPrice(Plan.Amount);
                setValidPrice(true);
                setTryPeriod(Plan.TrialDays);
                setIsPrivate(Plan.IsPrivate === "1");
                setStudentQuantity(Plan.ProfilesQuantity);
                setValidStudentQuantity(true);
                setIsAvailableChecked(Plan.Available === "1");
                setTaxesSelected(planTaxes); // Prepare taxes object
                setPlanDescriptionDetails(planDescriptionArray);
                setValidDescriptions(true);
                setPlanId(Plan.Id);
                setDisabledGenerics(true);
                setDisabledPlanName(true);
                setDisabledPrivatePlan(true);
                setDisabledProfilesQuantity(true);
                setDisabledPlanAvailability(true);
                setDisabledPlanDescriptions(true);
                setLoading(false);

                props.updateParent({
                    currencyIsoCode: Plan.Currency,
                    disabledGenerics: true,
                    disabledPlanAvailability: true,
                    disabledPlanDescriptions: true,
                    disabledPlanName: true,
                    disabledPrivatePlan: true,
                    disabledProfilesQuantity: true,
                    disabledRegionCountryIsos: true,
                    interval: Plan.Interval,
                    isAvailableChecked: Plan.Available === "1",
                    isEdit,
                    isPrivate: Plan.IsPrivate === "1",
                    isReadOnly,
                    loading: false,
                    planDescriptionDetails: planDescriptionArray,
                    planId: Plan.Id,
                    planName: Plan.Name,
                    planSubmitEnabled,
                    price: Plan.Amount,
                    productId: Plan.ProductId,
                    selectAll,
                    showAddExistingPlanButton,
                    showModal,
                    studentQuantity: Plan.ProfilesQuantity,
                    taxesSelected: planTaxes,
                    tryPeriod: Plan.TrialDays,
                    validDescriptions: true,
                    validPrice: true,
                    validStudentQuantity: true
                });
            });
        }
    };
    const getCurrentState = () => ({
        checkedValues, currencyIsoCode, disabledGenerics, disabledPlanAvailability, disabledPlanDescriptions,
        disabledPlanName, disabledPrivatePlan, disabledProfilesQuantity, interval, intervalCount, isAvailableChecked, isEdit,
        isPrivate, isReadOnly, languages, loading, modalPlanVisible, planDescriptionCurrentDescription, planDescriptionCurrentLangCode,
        planDescriptionCurrentLangName, planDescriptionCurrentTitle, planDescriptionDetails, planDescriptionShowModal, planId, planName, planSubmitEnabled, price, productId,
        selectAll, showAddExistingPlanButton, showModal, studentQuantity, taxesSelected, tryPeriod, validDescriptions, validPrice, validStudentQuantity, propsModal
    })

    return (
            <div>
                {
                    showAddExistingPlanButton ?
                <Row>

                    <h4>
                        <small>
                            <Translate id="plan-creator.general-instructions"/>
                        </small>
                        {' '}

                                <button className="btn-default button-primary-custom" onClick={handleOnClickSearchPlan}>
                                    <Translate id="plan-creator.select-existing"/>
                                </button>                        
                    </h4>
                </Row> : null }

                <Row>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="interval">
                            <b><Translate id="commercial.select-the-product"/></b>
                        </Label>
                        <br/>
                        <SelectorPlanProduct disabled={disabledGenerics} value={productId}
                                             isCommercial={true}
                                             name="product" id="product" updateParent={handleProduct}/>
                        <br/>
                        {productId === null ?
                            <Alert color="danger">
                                <Translate id="product.product-select-error"/>
                            </Alert> : null
                        }
                    </Col>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="plan-name">
                            <b><Translate id="plan-creator.plan-name"/></b>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="plan-creator.plan-name-instructions"/>
                        </small>
                        <Input disabled={disabledPlanName} value={planName} type="text"
                               classNamePrefix="mantine-like"
                               className="mantine-like__control"
                               name="plan-name" id="plan-name" onChange={handleNameChange}/>
                        <br/>
                        {planName === '' ?
                            <Alert color="danger">
                                <Translate id="plan-creator.plan-name-error"/>
                            </Alert> : null
                        }
                    </Col>
                </Row>
                <hr/>
                {Settings.IS_APP_MANAGED_BY_REQUIRED ?
                    <>
                <Row>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="currency">
                            <b><Translate id="currency.currency"/></b>
                        </Label>
                        <SelectorCurrency disabled={disabledGenerics} name="currency" id="currency"
                                          value={currencyIsoCode}
                                          updateParent={handleCurrencyChange}/>
                        <br/>
                        {currencyIsoCode === null ?
                            <Alert color="danger">
                                <Translate id="currency.currency-error"/>
                            </Alert> : null
                        }
                    </Col>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="plan-name">
                            <b><Translate id="plan-creator.price-per-unit"/></b>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="plan-creator.price-instructions"/>
                        </small>
                        <Input value={price} disabled={disabledGenerics} type="number" min="0"
                               classNamePrefix="mantine-like"
                               className="mantine-like__control"
                               step="1" id="price-per-unit"
                               name="price-per-name" onChange={handlePriceChange}/>
                        <br/>
                        {
                            price === null || !validPrice ?
                                <Alert color="danger">
                                    <Translate id="plan-creator.price-per-unit-error"/>
                                </Alert> : null
                        }
                    </Col>
                </Row>
                <hr/>
                <Row>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="interval">
                            <b><Translate id="billing-interval.billing-interval"/></b>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="billing-interval.billing-interval-instructions"/>
                        </small>
                        <SelectorBillingInterval value={interval} disabled={disabledGenerics}
                                                 name="interval" id="interval"
                                                 updateParent={handleInterval}/>
                        <br/>
                        {interval === null ?
                            <Alert color="danger">
                                <Translate id="billing-interval.billing-interval-error"/>
                            </Alert> : null
                        }
                    </Col>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="interval-count">
                            <b><Translate id="billing-interval.billing-interval-count"/></b>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="billing-interval.billing-interval-count-instructions"/>
                        </small>
                        <Input value={intervalCount} type="number" disabled={disabledGenerics} min="0"
                               className="mantine-like__control"
                                                 name="interval-count" id="interval-count"
                                                 onChange={handleIntervalCount}/>
                        <br/>
                    </Col>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="trial">
                            <b><Translate id="plan-creator.trial-period"/></b>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="plan-creator.trial-period-instructions"/>
                        </small>
                        <Input value={tryPeriod} disabled={disabledGenerics} type="number" min="0"
                               step="1" id="trial-period"
                               className="mantine-like__control"
                               name="trial-period" onChange={handleTry}/>
                        <br/>
                        {tryPeriod === null ?
                            <Alert color="danger">
                                <Translate id="plan-creator.trial-period-error"/>
                            </Alert> : null
                        }
                    </Col>
                </Row>
                <hr/>
                <Row>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="is-for-school">
                            <b><Translate id="plan-creator.is-for-private"/></b>
                        </Label>
                        <br/>
                        <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <small>
                                <Input disabled={disabledPrivatePlan} type="checkbox"
                                       name="is-for-private-checkbox"
                                       id="is-for-private" checked={isPrivate}
                                       onChange={handleIsPrivate}/>
                                <Translate id="plan-creator.is-for-private-instructions"/>
                            </small>
                        </Col>
                    </Col>
                </Row>
                <hr/> </> : null}
                <Row>
                    <Col sm={{size: 12}} md={{size: 6}}>
                        <Label for="profiles-quantity">
                            <b><Translate id="plan-creator.profiles-quantity"/></b>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="plan-creator.profiles-quantity-instructions"/>
                        </small>
                        <Input disabled={disabledProfilesQuantity} type="number"
                               className="mantine-like__control"
                               value={studentQuantity} min="1" step="1" id="profile-quantity"
                               name="profile-quantity" onChange={handleStudentQuantity}/>
                        <br/>
                        {  !validStudentQuantity ?
                            <Alert color="danger">
                                <Translate id="plan-creator.profiles-quantity-error"/>
                            </Alert> : null
                        }
                    </Col>
                    <Col sm={{size: 12}} md={{size: 6}} style={{paddingLeft: 20}}>
                        <Label for="is-available">
                            <b><Translate id="plan-creator.is-available"/></b>
                        </Label>
                        <br/>
                        <small>
                                <Input disabled={disabledPlanAvailability} type="checkbox"

                                       name="is-available"
                                       id="is-available" checked={isAvailableChecked}
                                       onChange={handlePlanAvailability}/>
                                <Translate id="plan-creator.is-available-instructions"/>
                            </small>
                    </Col>
                </Row>
                <Row>
                    <Col sm={{size: 12}}>
                        <Label for="taxes">
                            <b><Translate id="taxes.title"/></b>
                        </Label>
                        <br/>
                        <small><Translate id="taxes.instructions"/></small>
                        <CheckboxTaxes disabled={disabledGenerics} selected={getCurrentState().taxesSelected}
                                       updateParent={handleCheckedTaxes}/>
                    </Col>
                </Row>

                <Row>
                    <b><Translate id="plan-creator.description-by-language"/></b>
                </Row>
                <Row>
                    <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        { languages !== null ?
                            renderDescriptions() : null
                        }
                    </Col>
                    <Col sm={{size: 12}}>
                        {
                            planDescriptionShowModal ?
                                <PlanDescriptionEditorSN disabled={disabledPlanDescriptions}
                                                         languageName={planDescriptionCurrentLangName}
                                                         title={planDescriptionCurrentTitle}
                                                         description={planDescriptionCurrentDescription}
                                                         languageCode={planDescriptionCurrentLangCode}
                                                         updateParent={handleChangeDescriptionEditor}
                                /> : null
                        }
                        <br/>
                        {   !validDescriptions ?
                            <Alert color="danger">
                                <Translate id="plan-creator.plan-details-error"/>
                            </Alert> : null
                        }
                    </Col>
                </Row>
                <hr/>
                {
                    !isReadOnly ?
                        <Row>
                            <Col sm={{size: 8}}><b><Translate id="plan-creator.validate-submission"/></b></Col>
                            <Col sm={{size: 4}}>
                                <Button onClick={submitPlan} color="success" block disabled={!planSubmitEnabled}>
                                    <Translate id="plan-creator.submit"/>
                                </Button>
                            </Col>
                        </Row> : null
                }
                {loading ? <LoadingFullWindow/> : null}
                {showModal ? renderGenericModal() : null }
                {modalPlanVisible ? <ModalPlanSelection visible={modalPlanVisible}
                                                                   updateParent={handleSelectedPlan}/> : null}
            </div>
        )
}

export default FormGroupCommercialPlans;