/**
 * Created by portiz on 9/10/19.
 */
import React, { useState } from 'react';
import { Button, Modal, ModalBody, ModalFooter } from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/

/**
 * Class that handles the modal ters
 */
const ModalOffer = ({ updateParent }) => {
    const [modal, setModal] = useState(true);

    const toggle = () => {
        setModal(!modal);
    };

    const acceptOffer = () => {
        updateParent(1);
        setModal(!modal);
    };

    const denyOffer = () => {
        updateParent(0);
        setModal(!modal);
    };

    return (
        <div>
            <Modal isOpen={modal} toggle={toggle}>
                <ModalBody>
                    <h2 style={{padding: 30}}><Translate id="modal-offer.save" options={{renderInnerHtml: true}}/></h2>
                    <div className="text-center">
                        <Button onClick={acceptOffer} color="danger">
                            <Translate id="modal-offer.accept"/>
                        </Button>{' '}
                        <Button onClick={denyOffer}>
                            <Translate id="modal-offer.no-thanks"/>
                        </Button>
                    </div>
                </ModalBody>
                <ModalFooter className="text-center">
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default  ModalOffer;