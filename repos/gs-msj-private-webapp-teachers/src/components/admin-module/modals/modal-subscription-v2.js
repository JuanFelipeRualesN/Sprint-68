 import React from 'react';
 import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
 import { Translate } from "react-localize-redux";
 import {getPlansAdvanced, getSubscriptionSchoolEntitiesRelatedAdvanced} from "../../../api/subscriptions";
 import {getUsersAdvanced} from "../../../api/users";
 import Settings from '../../../custom-classes/settings';
 import Wizard from "../../admin-module/views/subscriptions-wizard/wizard"
 
const ModalSubscriptionV2 = ({userId,planId,subscriptionId}) => {
    const render = () => {
        
    }

    return (<Wizard userId={userId}
                    planId={planId}
                    subscriptionId={subscriptionId} />)
}