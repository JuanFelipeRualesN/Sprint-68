import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import { Translate } from "react-localize-redux";
import FormCouponManager from '../forms/form-coupon-manager';

/**
 * Modal to handle the coupons
 */
const ModalCouponManager = ({ visible, coupon, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        setIsVisible(visible);
    }, [visible]);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        updateParent(newVisible);
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} size="lg">
                <ModalHeader toggle={toggle}>
                    <Translate id="coupons-manager.coupon"/>
                </ModalHeader>
                <ModalBody>
                    <FormCouponManager
                        coupon={coupon}
                    />
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="coupons-manager.close"/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalCouponManager;