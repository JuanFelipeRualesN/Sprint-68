import React from 'react';
import { withLocalize } from "react-localize-redux";
import Wizard from "./subscriptions-wizard/wizard";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const SchoolSubscription = () => {
    // Initialize translations based on the current version
    useVersionedTranslations();

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="col col-no-padding-left">
                    </div>
                    <Wizard />
                </div>
            </div>
        </div>
    );
};

export default withLocalize(SchoolSubscription);