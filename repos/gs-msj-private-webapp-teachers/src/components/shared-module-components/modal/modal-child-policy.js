/**
 * Created by portiz on 8/26/18.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import { faChild } from '@fortawesome/free-solid-svg-icons';
/** End of Multiple Language imports **/

/**
 * To handle the child policy
 */
const ModalChildPolicy = () => {
    const [modal, setModal] = useState(false);

    const toggle = () => {
        setModal(!modal);
    };

    return (
        <div>
            <span className="red-text" onClick={toggle} style={{textDecoration: "underline"}}>
                <FontAwesomeIcon icon={faChild}/>{' '}<Translate id="children-policy.title"/>
            </span>
            <Modal isOpen={modal} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="children-policy.title"/>
                </ModalHeader>
                <ModalBody>
                    <span>
                        <Translate>{({translate}) => <span>{translate("children-policy.conditions", null, { renderInnerHtml: true })}</span>}</Translate>
                    </span>
                </ModalBody>
                <ModalFooter>
                    <Button onClick={toggle} color="info">
                        <Translate>{({translate}) => <span>{translate("children-policy.close")}</span>}</Translate>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default  ModalChildPolicy;