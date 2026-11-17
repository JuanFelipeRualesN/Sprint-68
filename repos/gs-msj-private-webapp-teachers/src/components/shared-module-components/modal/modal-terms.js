/**
 * Created by portiz on 8/26/18.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Class that handles the modal ters
 */
const ModalTerms = () => {
    const [modal, setModal] = useState(false);

    // Hide or shows the window
    const toggle = () => {
        setModal(!modal);
    };

    return (
        <div>
            <span color="link" onClick={toggle} style={{textDecoration: "underline"}}> <Translate>{({translate}) => <span>{translate("terms.title")}</span>}</Translate></span>
            <Modal isOpen={modal} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="terms.title"/>
                </ModalHeader>
                <ModalBody>
                    <Translate>{({translate}) => <span>{translate("terms.conditions", null, { renderInnerHtml: true })}</span>}</Translate>
                </ModalBody>
                <ModalFooter>
                    <Button onClick={toggle} color="info">
                        <Translate id="terms.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default  ModalTerms;