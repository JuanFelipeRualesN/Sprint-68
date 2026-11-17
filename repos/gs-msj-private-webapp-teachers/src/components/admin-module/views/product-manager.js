import React from "react";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faArrowLeft} from "@fortawesome/free-solid-svg-icons";
import {Translate, withLocalize} from "react-localize-redux";
import {withRouter} from "react-router-dom";
import FormProductManager from "../forms/form-product-manager";

const ProductManager = ({ history }) => {

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Renders the main page
    return (
        <div>
            <div>
                <div className="container-padding animated delay-05s fadeIn">
                    <div className="container-padding animated delay-05s fadeIn">
                        <div className="wrapper-principal-container">
                            <div className="col col-no-padding-left">
                                <div className="btn-back" style={{cursor: 'pointer'}} onClick={() => history.goBack()}>
                                    <FontAwesomeIcon icon={faArrowLeft} />{' '}
                                    <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                                </div>
                            </div>
                    <FormProductManager isCommercial={false}/>
                </div>
            </div>

        </div>
            </div>
        </div>
    );
};

export default withRouter(withLocalize(ProductManager));