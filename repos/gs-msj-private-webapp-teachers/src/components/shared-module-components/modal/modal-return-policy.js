/**
 * Created by portiz on 9/10/19.
 */
import React, { useState } from 'react';import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Class that handles the modal ters
 */
const ModalReturnPolicy = () => {
    const [modal, setModal] = useState(false);

    //Hide or shows the window
    const toggle = () => {
        setModal(!modal);
    };

    return (
        <div>
            <span color="link" onClick={toggle} style={{textDecoration: "underline"}}> <Translate>{({translate}) => <span>{translate("return-policy.title")}</span>}</Translate></span>
            <Modal isOpen={modal} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="return-policy.title"/>
                </ModalHeader>
                <ModalBody>
                    <Translate>{({translate}) => <span>{translate("return-policy.conditions", null, { renderInnerHtml: true })}</span>}</Translate>
                </ModalBody>
                <ModalFooter>
                    <Button onClick={toggle} color="info">
                        <Translate id="return-policy.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default  ModalReturnPolicy;