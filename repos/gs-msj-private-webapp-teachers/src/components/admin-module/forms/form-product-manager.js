import React, {useState} from "react";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import {getReadSpecificProductAPI, postAddProductsAPI, postUpdateProductsAPI} from "../../../api/subscriptions";
import swal from "sweetalert";
import {Translate} from "react-localize-redux";
import {Button, Col, Input, Label, Row} from "reactstrap";
import SelectorProduct from "../selectors/selector-product";
import MultiSelectorItemProduct from "../selectors/multi-selector-item-product";
import Checkbox from "@material-ui/core/Checkbox";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";

const ProductManagerForm = ({ isFromCommercial }) => {
    const [id, setId] = useState(null);
    const [availableTranslation, setAvailableTranslation] = useState(LanguageManager.getTranslation("product.available", Settings.LANGUAGES));
    const [available, setAvailable] = useState(true);
    const [loading, setLoading] = useState(false);
    const [isEdit, setIsEdit] = useState(false);
    const [addNew, setAddNew] = useState(false);
    const [resetBox, setResetBox] = useState(false);
    const [redoSearch, setRedoSearch] = useState(false);
    const [platformId, setPlatformId] = useState('');
    const [isCommercial, setIsCommercial] = useState(isFromCommercial);
    const [selectedItemProducts, setSelectedItemProducts] = useState([]);
    const [name, setName] = useState('');

    // Initialize translations based on the current version
    useVersionedTranslations();

    const handleProductSelection = (e) => {
        setId(e.value);
        setIsEdit(true);
        setResetBox(false);
        readData(e.value);
    };

    const handleItemProductSelection = (e) => {
        setSelectedItemProducts(e);
    };

    const readData = (productId = id) => {
        if (productId !== null) {
            setLoading(true);

            let apiCall = getReadSpecificProductAPI(productId);
            apiCall.then((response) => {
                setLoading(false);

                if (response.data !== null) {
                    setPlatformId(response.data.PlatformId);
                    setName(response.data.Name);
                    setIsCommercial(response.data.IsCommercial === "1");
                    setAvailable(response.data.Available === "1");

                    let selectedItemProducts = [];
                    for (let i = 0; i < response.data.ItemsIncluded.length; i++) {
                        selectedItemProducts.push({
                            value: response.data.ItemsIncluded[i].Id,
                            label: response.data.ItemsIncluded[i].Name,
                        });
                    }
                    setSelectedItemProducts(selectedItemProducts);
                }
            });
        }
    };

    const saveChanges = async () => { // Made the function async
        setLoading(true); // Start loading immediately
        const cleanedName = trimAndNormalizeText(name);

        try {
            if (id === null) {
                // Submits the registration
                let response = await postAddProductsAPI(cleanedName, platformId, available, selectedItemProducts, isCommercial); // Use await

                if (response.status === 200 || response.status === 201) {
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");

                    setRedoSearch(prev => !prev);
                    setId(null);
                    setPlatformId('');
                    setIsCommercial(false);
                    setSelectedItemProducts([]);
                    setName('');
                    setResetBox(true);
                } else {
                    if (response.data.Messages?.length) {
                        let message = response.data.Messages.join(" ");
                        swal("", message, "error"); // Changed to "error" for better UX
                    } else {
                        swal("", LanguageManager.getTranslation("general.error-occurred", Settings.LANGUAGES), "error"); // Fallback error
                    }
                }
            } else {
                // Update the product
                let response = await postUpdateProductsAPI(id, cleanedName, platformId, available, selectedItemProducts, isCommercial); // Use await
                // is response code 200?
                if (response.status === 200 || response.status === 201) {
                    swal("", LanguageManager.getTranslation("general.successful-transaction", Settings.LANGUAGES), "success");
                    setRedoSearch(prev => !prev);
                    setId(null);
                    setPlatformId('');
                    setName('');
                    setIsCommercial(false);
                    setResetBox(true);
                    setSelectedItemProducts([]);
                }
                else{
                    let message = response.data.Messages.join(" ");
                    swal("", message, "error"); // Changed to "error" for better UX
                }
            }
        } catch (error) {
            // Catch any errors that occur during the API call or subsequent processing
            console.error("Error in saveChanges:", error);
            swal("", LanguageManager.getTranslation("general.error-occurred", Settings.LANGUAGES) + ": " + error.message, "error");
        } finally {
            setLoading(false); // Always stop loading, regardless of success or failure
        }
    };

    const handleNewProduct = () => {
        setAddNew(true);
        setId(null);
        setPlatformId('');
        setName('');
        setResetBox(true);
        setIsCommercial(isCommercial);
        setSelectedItemProducts([]);
    };

    const handleNameChange = (e) => {
        setName(e.currentTarget.value);
    };

    const handlePlatformChange = (e) => {
        setPlatformId(e.currentTarget.value);
    };

    const handleCheckboxChange = (e) => {
        setAvailable(e.currentTarget.checked);
    };

    const handleCheckboxIsCommercialChange = (e) => {
        setIsCommercial(e.currentTarget.checked);
    }

    const trimAndNormalizeText = (text) => {
        return text.trim().replace(/\s{2,}/g, ' '); 
    };

    const isValidName = name && typeof name === 'string' && name.trim().length > 0 && name.length <= 100;

    // Renders the main page
    return (
        <div>
            <div className="wrapper-col borderGray-bottom block-display-responsive">
                            <div className="col col-no-padding-left">
                                <div className="title-principal"><Translate id="product.product" /></div>
                            </div>
                            <div className="col txt-right">
                                <button className="btn-default button-primary-custom" onClick={handleNewProduct}>
                                    <Translate id="user-management.add-new" />
                                </button>
                            </div>
                        </div>
            {!addNew ? <>
                                <Row className="clear-style-card td-responsive mt-4">
                                    <Col xs={12} md={8}>
                                        <Label for="subjects-grades">
                                            <Translate id="product.product-select" />
                                        </Label>
                                        <SelectorProduct resetBox={resetBox} redoSearch={redoSearch} disabled={addNew} updateParent={handleProductSelection} isCommercial={isCommercial} />
                                        <Label for="subjects-grades">
                                        </Label>
                                    </Col>
                                </Row>
                                <Row> <hr /></Row></>: null}
            <Row className="clear-style-card td-responsive mt-4">
                            <Col xs={12} md={8}>
                                <Label for="subjects-grades">
                                    * <Translate id="product.subjects-grades" />
                                </Label>
                                <MultiSelectorItemProduct value={selectedItemProducts} resetBox={resetBox} redoSearch={redoSearch} disabled={addNew} updateParent={handleItemProductSelection} />
                            </Col>
                        </Row>
            <Row className="clear-style-card td-responsive mt-4">
                            <Col xs={12} md={4}>
                                <Label for="name">
                                    * <Translate id="product.name" />
                                </Label>
                                <Input value={name} type="text" name="name" disabled={!isEdit && !addNew ? "disabled" : ""}
                                       id="name" onChange={handleNameChange} />
                            </Col>
                        </Row>
            <Row className="clear-style-card td-responsive mt-4">
                            <Col xs={12} md={4}>
                                <Label for="name">
                                    * <Translate id="product.available" />
                                </Label>
                                <Checkbox name={availableTranslation} disabled={!isEdit && !addNew ? "disabled" : ""}
                                          checked={available}
                                          onChange={handleCheckboxChange}
                                />
                            </Col>
                        </Row>
            <Row className="clear-style-card td-responsive mt-4">
                            <Col xs={12} md={4}>
                                <Label for="is-commercial">
                                    * <Translate id="product.is-commercial" />
                                </Label>
                                <Checkbox name={availableTranslation} disabled={!isEdit && !addNew ? "disabled" : ""}
                                          checked={isCommercial}
                                          onChange={handleCheckboxIsCommercialChange}
                                />
                            </Col>
                        </Row>
            <Row className="clear-style-card td-responsive mt-4">
                            <Col xs={12} md={12}>
                                <Label for="name">
                                    <Translate id="product.platform_id" />
                                    <br />
                                    <small><Translate id="product.platform-id-exp" /></small>
                                </Label>
                            </Col>

                            <Col xs={12} md={4}>
                                <Input value={platformId} type="text" name="platform-id" disabled={!isEdit && !addNew ? "disabled" : ""}
                                       id="platform-id" onChange={handlePlatformChange} />
                            </Col>
                        </Row>
            <Row className="clear-style-card td-responsive mt-4">
                            <Button 
                                color="info" 
                                disabled={name === "" || !isValidName || selectedItemProducts.length === 0 ? "disabled" : ""} 
                                onClick={saveChanges}
                            >
                                <Translate id="product.submit" />
                            </Button>
                        </Row>
            {loading && <LoadingFullWindow />}
        </div>
    );
};

export default ProductManagerForm;