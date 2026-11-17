import React, { useState } from "react";
import {Input, Row, Col, Alert, Button} from 'reactstrap';
import {getCouponValidate} from '../../../api/subscriptions';
import {Translate} from "react-localize-redux";
import LanguageManager from '../../../custom-classes/language-manager';
import Settings from "../../../custom-classes/settings";

/**
 * Validates the coupon
 */
const FormGroupCoupon = ({interval, couponPlatformId, updateParent}) => {
    const [coupon, setCoupon] = useState(couponPlatformId ? couponPlatformId : "");
    const [couponError, setCouponError] = useState("");
    const [couponTranslation] = useState(LanguageManager.getTranslation("coupons.coupon-optional", Settings.LANGUAGES));
    const [disabled, setDisabled] = useState( !!couponPlatformId);
    const [isReadOnly] = useState(!!couponPlatformId);
    const [valid, setValid] = useState(!!couponPlatformId);

    // Handles the coupon submition
    const handleCouponSubmit = () => {
        setCouponError("");
        if (coupon.trim().length > 0) {
        let apiCall = getCouponValidate(coupon, interval);
        apiCall.then((response) => {
            if (!response.data.CouponValid) {
                let errorMsg = "";
                if (typeof response.data.CouponReason !== 'undefined' && response.data.CouponReason !== null) {
                    errorMsg = response.data.CouponReason;
                }
                setCouponError(errorMsg);
                setValid(false);
                updateParent(null);
            } else {
                setValid(true);
                setDisabled(true);
                updateParent(response.data.CouponPlatformId);
            }
        });
        }
    };

    const handleCouponRemove = () => {
        setCoupon("");
        setValid(false);
        setDisabled(false);
        updateParent(null);
    };

    // Action when the email is changed
    const handleChange = (event) => {
        setCoupon(event.currentTarget.value);
    };

    // Render the form group
    return (
        <div>
            {interval === null ? null :
            <Row>
                <Col sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    <Row>
                        <Col xs={{size: 5}}  sm={{size: 5}} md={{size: 5}} lg={{size: 5}}>
                            <Input disabled={isReadOnly || disabled} value={coupon} type="coupon" name="coupon"
                                    id="coupon"
                                   className="mantine-like__control"
                                    placeholder={couponTranslation}
                                    onChange={handleChange}

                            />
                        </Col>
                        <Col xs={{size: 7}}  sm={{size: 7}} md={{size: 7}} lg={{size: 7}}>
                            {
                                !isReadOnly && !disabled ?
                                    <Button outline color="primary" className="btn-font-size-13" onClick={handleCouponSubmit}>
                                        <Translate id="coupons.submit"/>
                                    </Button> : null
                            }
                            {
                                valid && !isReadOnly ?
                                    <Button outline color="danger" className="btn-font-size-13" onClick={handleCouponRemove}>
                                        <Translate id="coupons.remove"/>
                                    </Button> :null
                            }
                        </Col>
                    </Row>
                    {
                        couponError.length !== 0 ?
                            <div>
                                <Alert color="danger" isOpen={!valid}>
                                    {couponError}
                                </Alert>
                            </div> : null
                    }
                    {
                        valid ?
                            <div>
                                <Alert color="info" isOpen={valid}>
                                    <Translate id="coupons.coupon-alert"/>
                                </Alert>
                            </div> : null
                    }
                </Col>
            </Row> }
        </div>
    )
};

export default FormGroupCoupon;