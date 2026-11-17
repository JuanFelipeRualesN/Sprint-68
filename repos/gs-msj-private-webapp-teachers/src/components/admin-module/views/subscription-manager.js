import React from 'react';
import { withLocalize } from "react-localize-redux";
import Wizard from "./subscriptions-wizard/wizard";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
 
const SubscriptionManager = () => {
    const queryParams = new URLSearchParams(location.search);
    const subscriptionType = queryParams.get('subscriptionType') || null;
    // Initialize translations based on the current version
    useVersionedTranslations();
  return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <Wizard subscriptionType={subscriptionType}/>
                </div>
            </div>
        </div>
    );
};
 
 export default withLocalize(SubscriptionManager);