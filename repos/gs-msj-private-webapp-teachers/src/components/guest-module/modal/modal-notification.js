/**
 * Created by portiz on 5/17/20.
 */
import React, { useState } from 'react';
import {Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Component to view the message details.
 */
const ModalNotification = () => {
    const [visible, setVisible] = useState(
        typeof localStorage.getItem("notification") === 'undefined' || 
        localStorage.getItem("notification") === null || 
        localStorage.getItem("notification") !== '1'
    );

    const toggle = () => {
        setVisible(false);
        localStorage.setItem("notification", "1");
    };

    return (
        <div>
            <Modal isOpen={visible} toggle={toggle}>
                <ModalHeader toggle={toggle}><h2>  <Translate id="notification.attention"/></h2></ModalHeader>
                <ModalBody>
                    <Translate id="notification.notification"/>
                </ModalBody>
                <ModalFooter/>
            </Modal>
        </div>
    );
};

export default  ModalNotification;