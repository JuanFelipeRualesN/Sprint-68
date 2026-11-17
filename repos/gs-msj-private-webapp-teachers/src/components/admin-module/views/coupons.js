import React, { useState, useEffect } from 'react';
import {
    Card,
    Col,
    Row,
    CardBody,
    Form
} from 'reactstrap';
import {withLocalize} from "react-localize-redux";
import {Translate} from "react-localize-redux";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import CouponDataTable from '../datatables/coupon-data-table';
import ModalCouponManager from '../modals/modal-coupon-manager';
import {getCoupons} from "../../../api/subscriptions";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

// This is the item list
const CouponSearch = () => {
    const [coupon, setCoupon] = useState(null);
    const [didMount, setDidMount] = useState(false);
    const [loading, setLoading] = useState(false);
    const [propsModal, setPropsModal] = useState(null);
    const [redoSearch, setRedoSearch] = useState(false);
    const [showCouponsModal, setShowCouponsModal] = useState(false);
    const [showModal, setShowModal] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Evaluate the resize, to show the proper navbar menu style
    useEffect(() => {
        setDidMount(true);
    }, []);

    // Toggles alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
    };

    // Toggle the update plan modal
    const toggleCouponsModal = () => {
        setShowCouponsModal(prev => !prev); // Correct way to toggle boolean state
        setRedoSearch(true);
    };

    const handleCouponSelection = (e) => {
        // Search the data
        let apiCall = getCoupons(
            null,
            null,
            null,
            null,
            e,
            1,
            1);
        apiCall.then((response) => {
            setCoupon(response.data.Coupons[0]);
            setShowCouponsModal(true);
        });
    };

    const handleNewCoupon = () => {
        setShowCouponsModal(true);
        setCoupon(null);
    };

    // Renders the main page
    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">

                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal">
                                <Translate id="coupons.coupons"/>
                            </div>
                        </div>
                        <div className="col txt-right">
                            <button className="btn-default button-primary-custom" onClick={handleNewCoupon}>
                                <Translate id="coupons-manager.add-new-coupon"/>
                            </button>
                        </div>
                    </div>

                    <Row className="clear-style-card td-responsive">
                        <Col xs={12}>
                            <Card>
                                {/* <CardHeader  className="text-right">
                                    <button className="btn-default button-primary-custom" onClick={handleNewCoupon}>
                                        <Translate id="coupons-manager.add-new-coupon"/>
                                    </button>
                                </CardHeader> */}
                                <CardBody>
                                    <Form>
                                        <CouponDataTable redoSearch={redoSearch}
                                                       updateParent={handleCouponSelection}/>
                                    </Form>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                    <hr/>
                </div>
            </div>

            {loading ? <LoadingFullWindow/> : null}
            {showModal ?
                <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}
            {showCouponsModal ?
                <ModalCouponManager coupon={coupon}
                            visible={showCouponsModal}
                                     updateParent={toggleCouponsModal}/> : null
            }
        </div>
    );
};

export default withLocalize(CouponSearch);