import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faExclamationTriangle} from '@fortawesome/free-solid-svg-icons';

/**
 * Generic modal. Set whatever you want to put in this message box.
 */
const ModalGeneric = ({ data, updateParent }) => {
    const [bodyText, setBodyText] = useState(data?.bodyText);
    const [modal, setModal] = useState(data?.visible);

    // Hide or shows the window
    const toggle = () => {
        setModal(!modal);

        const state = {
            bodyText: bodyText,
            modal: !modal
        };
        
        updateParent(state);
    };

    return (
        <div>
            <Modal style={{zIndex: 99999}} isOpen={modal} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="general.attention"/>
                </ModalHeader>
                <ModalBody>
                   <span dangerouslySetInnerHTML={{__html: bodyText}}/>
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="general.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalGeneric;