import React, { useState } from 'react';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';

/**
 * The modal video of youtube genial skills
 */
const ModalGeneric = ({ visible, updateParent }) => {
    const [modal, setModal] = useState(visible);

    const toggle = () => {
        setModal(!modal);
        
        const state = {
            modal: !modal
        };
        
        updateParent(state);
    };

    return (
        <Modal isOpen={modal} toggle={toggle} className="modal-theater">
            <ModalHeader toggle={toggle}/>
            <ModalBody className="centered-text">
                <iframe 
                    title="video-genial" 
                    style={{width:'100%', height: 'auto'}} 
                    src="https://www.youtube.com/embed/xvRQE1HT3Ug" 
                    frameBorder="0" 
                    allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" 
                    allowFullScreen
                />
            </ModalBody>
        </Modal>
    );
};

export default ModalGeneric;