import { useEffect, useState } from 'react';
import {Input, Row, Col, Alert, Button, Label, FormGroup} from 'reactstrap';
import {createCoupon} from "../../../api/subscriptions";
import {Translate} from "react-localize-redux";
import SelectorCurrency from '../selectors/selector-currency';
import DatePicker from "../../shared-module-components/date-picker/date-picker-recent";
import ModalGeneric from "../../shared-module-components/modal/modal-generic";
import moment from 'moment';
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
/**
 * Creates or edit a coupon
 */
const FormCouponManager = ({ coupon }) => {
    const [amountOff, setAmountOff] = useState(coupon?.AmountOff ?? '');
    const [available, setAvailable] = useState(coupon?.Available ?? '0');
    const [code, setCode] = useState(coupon?.Code ?? '');
    const [currency, setCurrency] = useState(coupon?.Currency ?? null);
    const [duration, setDuration] = useState(coupon?.Duration ?? null);
    const [durationInMonths, setDurationInMonths] = useState(coupon?.DurationInMonths ?? '');
    const [interval, setInterval] = useState(coupon?.IntervalIntended ?? null);
    const [loading, setLoading] = useState(false);
    const [maxRedemptions, setMaxRedemptions] = useState(coupon?.MaxRedemptions ?? '');
    const [name, setName] = useState(coupon?.Name ?? '');
    const [percentOff, setPercentOff] = useState(coupon?.PercentOff ?? '');
    const [percentOrAmount, setPercentOrAmount] = useState(coupon?.AmountOff !== null && coupon?.AmountOff !== undefined ? 'amount' : 'percent');
    const [propsModal, setPropsModal] = useState(null);
    const [redeemBy, setRedeemBy] = useState(coupon?.RedeemBy ? moment(coupon.RedeemBy) : null);
    const [showModal, setShowModal] = useState(false);
    const [valid, setValid] = useState(true);

    useEffect(() => {
        if (coupon) {
        setAmountOff(coupon?.AmountOff ?? '');
        setAvailable(coupon?.Available ?? '0');
        setCode(coupon?.Code ?? '');
        setCurrency(coupon?.Currency ?? null);
        setDuration(coupon?.Duration ?? null);
        setDurationInMonths(coupon?.DurationInMonths ?? '');
        setInterval(coupon?.IntervalIntended ?? null);
        setMaxRedemptions(coupon?.MaxRedemptions ?? '');
        setName(coupon?.Name ?? '');
        setPercentOff(coupon?.PercentOff ?? '');
        setPercentOrAmount(coupon?.AmountOff !== null && coupon?.AmountOff !== undefined ? 'amount' : 'percent');
        setRedeemBy(coupon?.RedeemBy ? moment(coupon?.RedeemBy) : null);
        setValid(true);
        }
    }, [coupon]);

    const handleAvailable = () => {
        let value = available === '0' || available === null ? '1' : '0';
        setAvailable(value);
    };

    const handleNameChange = (e) => {
        setName(e.currentTarget.value);
    };

    const handleCodeChange = (e) => {
        setCode(e.currentTarget.value);
    };

    const handleDuration = (e) => {
        const value = e.currentTarget.value;
        setDuration(value);
        if (value !== 'repeating') {
            setDurationInMonths('');
        }
    };

    const handleDurationInMonths = (e) => {
        const re = /^[0-9\b]+$/;
        if (e.currentTarget.value === '' || re.test(e.currentTarget.value)) {
            setDurationInMonths(e.currentTarget.value);
        }
    };

    const handleMaxRedemptions = (e) => {
        const re = /^[0-9\b]+$/;
        if (e.currentTarget.value === '' || re.test(e.currentTarget.value)) {
            setMaxRedemptions(e.currentTarget.value);
        }
    };

    const handleSetRedeemBy = (e) => {
        setRedeemBy(e.date);
    };

    // Handles the coupon submition
    const handleCouponSubmit = () => {
        setLoading(true);
        setValid(true);
        // Validate
        if (name === null || name.trim().length === 0 ||code === null || code.trim().length === 0 ||  duration === null ||
            (duration === 'repeating' && (durationInMonths === null || durationInMonths === '')) ||
            maxRedemptions === null || maxRedemptions.trim().length === 0 ||
            redeemBy === null ||
            (percentOrAmount === 'amount' && (
                currency === null || currency === '' ||
                amountOff === null || amountOff === '' || parseInt(amountOff) === 0
            )) ||
            (percentOrAmount === 'percent' && (
                percentOff === null || percentOff === '' || parseInt(percentOff) === 0
            ))) {
            setValid(false);
            setLoading(false);
        } else {
            const apiCall = createCoupon(
                name, code, currency, percentOff, amountOff, duration,
                durationInMonths, maxRedemptions, redeemBy, available, interval
            );
            apiCall.then((response) => {
                setLoading(false);
                let messageStr = '';
                for (let i = 0; i < response.data.Messages.length; i++) {
                messageStr += response.data.Messages[i] + '<br/>';
                }
                const modalProps = {
                    bodyText: messageStr,
                    visible: true
                };
                setPropsModal(modalProps);
                setShowModal(true);
            });
        }
    };

    const handleCurrencyChange = (e) => {
        setCurrency(e);
    };

    const handleAmountOrPercent = (e) => {
        setPercentOrAmount(e.currentTarget.value);
    };

    const handleInterval = (e) => {
        setInterval(e.currentTarget.value);
    };

    const handlePercentOff = (e) => {
        const re = /^[0-9\b]+$/;
        if (e.currentTarget.value === '' || re.test(e.currentTarget.value)) {
        setPercentOff(e.currentTarget.value);
        setAmountOff('');
        }
    };

    const handleAmountOff = (e) => {
        const re = /^[0-9]*$/;

        if (e.currentTarget.value === '' || re.test(e.currentTarget.value)) {
        setAmountOff(e.currentTarget.value);
        setPercentOff('');
        }
    };

    // Toogles alert modal
    const toggleAlertModal = () => {
        setShowModal(false);
    };


    return (
            <div>
                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Label for="name" className="label-forms-blue form-label">
                            * <Translate id="coupons-manager.name" options={{renderInnerHtml: true}}/>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="coupons-manager.name-instructions" options={{renderInnerHtml: true}}/>
                        </small>
                        <Input
                            className="mantine-like__control"
                            value={name}
                               type="text"
                               name="name"
                               id="name"
                               maxLength={20}
                               onChange={handleNameChange}/>
                        <br/>
                        {
                           (name === null ||  name.trim().length === 0) ?
                                <Alert color="danger">
                                    <Translate id="coupons-manager.name-required" options={{renderInnerHtml: true}}/>
                                </Alert> : null
                        }
                    </Col>
                </Row>
                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Label for="code" className="label-forms-blue form-label">
                            * <Translate id="coupons-manager.code" options={{renderInnerHtml: true}}/>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="coupons-manager.code-instructions" options={{renderInnerHtml: true}}/>
                        </small>
                        <Input
                            className="mantine-like__control"
                            value={code}
                               type="text"
                               name="code"
                               id="code"
                               maxLength={10}
                               onChange={handleCodeChange}/>
                        <br/>
                        {
                            (code === null ||  code.trim().length === 0) ?
                                <Alert color="danger">
                                    <Translate id="coupons-manager.code-required" options={{renderInnerHtml: true}}/>
                                </Alert> : null
                        }
                    </Col>
                </Row>
                {/* interval */}
                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Label for="duration" className="label-forms-blue form-label">
                            * <Translate id="coupons-manager.interval" options={{renderInnerHtml: true}}/>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="coupons-manager.interval-choose" options={{renderInnerHtml: true}}/>
                        </small>
                        <Input className="mantine-like__control" value={interval} type="select" name="select" id="interval" onChange={handleInterval}>
                            <option>----</option>
                            <option value="month">month</option>
                            <option value="year">year</option>
                        </Input>
                        <br/>
                        {
                            (interval === null ||  interval.trim().length === 0) ?
                                <Alert color="danger">
                                    <Translate id="coupons-manager.interval-required" options={{renderInnerHtml: true}}/>
                                </Alert> : null
                        }
                    </Col>
                </Row>

                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Label for="duration" className="label-forms-blue form-label">
                            * <Translate id="coupons-manager.duration" options={{renderInnerHtml: true}}/>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="coupons-manager.duration-instructions" options={{renderInnerHtml: true}}/>
                        </small>
                        <Input className="mantine-like__control" value={duration} type="select" name="select" id="duration" onChange={handleDuration}>
                            <option>----</option>
                            <option value="once">once</option>
                            <option value="forever">forever</option>
                            <option value="repeating">repeating</option>
                        </Input>
                        <br/>
                        {
                            (duration === null ||  duration.trim().length === 0) ?
                                <Alert color="danger">
                                    <Translate id="coupons-manager.duration-required" options={{renderInnerHtml: true}}/>
                                </Alert> : null
                        }
                    </Col>
                </Row>
                {duration === 'repeating' ?
                    <Row>
                        <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <Label for="duration-months" className="label-forms-blue form-label">
                                * <Translate id="coupons-manager.duration-in-months" options={{renderInnerHtml: true}}/>
                            </Label>
                            <br/>
                            <small>
                                <Translate id="coupons-manager.duration-in-months-instructions"
                                           options={{renderInnerHtml: true}}/>
                            </small>
                            <Input className="mantine-like__control" value={durationInMonths}
                                   type="number"
                                   name="duration-months"
                                   id="duration-months"
                                   maxLength={2}
                                   onChange={handleDurationInMonths}/>
                            <br/>
                            {
                                (durationInMonths === null ||  durationInMonths.trim().length === 0) ?
                                    <Alert color="danger">
                                        <Translate id="coupons-manager.duration-in-months-required"
                                                   options={{renderInnerHtml: true}}/>
                                    </Alert> : null
                            }
                        </Col>
                    </Row> : null
                }

                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Label for="max-redemptions" className="label-forms-blue form-label">
                            * <Translate id="coupons-manager.max-redemptions" options={{renderInnerHtml: true}}/>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="coupons-manager.max-redemptions-instructions"
                                       options={{renderInnerHtml: true}}/>
                        </small>
                        <Input className="mantine-like__control" value={maxRedemptions}
                               type="number"
                               name="max-redemptions"
                               id="max-redemptions"
                               maxLength={5}
                               onChange={handleMaxRedemptions}/>
                        <br/>
                        {
                            (maxRedemptions === null ||  maxRedemptions.trim().length === 0) ?
                                <Alert color="danger">
                                    <Translate id="coupons-manager.max-redemptions-required"
                                               options={{renderInnerHtml: true}}/>
                                </Alert> : null
                        }
                    </Col>
                </Row>
                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <Label for="redeem-by" className="label-forms-blue form-label">
                            * <Translate id="coupons-manager.redeem-by" options={{renderInnerHtml: true}}/>
                        </Label>
                        <br/>
                        <small>
                            <Translate id="coupons-manager.redeem-by-instructions"
                                       options={{renderInnerHtml: true}}/>
                        </small>
                        <DatePicker className="mantine-like__control" date={redeemBy}
                                    updateParent={handleSetRedeemBy}/>
                        <br/>
                        {
                            (redeemBy === null) ?
                                <Alert color="danger">
                                    <Translate id="coupons-manager.redeem-by-required"
                                               options={{renderInnerHtml: true}}/>
                                </Alert> : null
                        }
                    </Col>
                </Row>

                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        <FormGroup check>
                            <Label check className="label-forms-blue form-label">
                                <Input type="checkbox" checked={available === "1"}
                                       onChange={handleAvailable}/>{' '}
                                * <Translate id="coupons-manager.available" options={{renderInnerHtml: true}}/>
                                {' '}
                                <small>
                                    <Translate id="coupons-manager.available-instructions"
                                               options={{renderInnerHtml: true}}/>
                                </small>
                            </Label>
                        </FormGroup>
                    </Col>
                </Row>

                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                    <FormGroup tag="fieldset">
                            <Translate id="coupons-manager.amount-or-percent"
                                       options={{renderInnerHtml: true}}/>
                        <FormGroup check>
                            <Label check>
                                <Input className="mantine-like__control" type="radio" name="percentage"
                                       value="percent"
                                       checked={percentOrAmount === "percent"}
                                       onChange={handleAmountOrPercent}
                                />{' '}
                                <Translate id="coupons-manager.percent-off"
                                           options={{renderInnerHtml: true}}/>{' '}

                            </Label>
                        </FormGroup>
                        <FormGroup check>
                            <Label check>
                                <Input className="mantine-like__control" type="radio" name="amount"
                                       value="amount"
                                       checked={percentOrAmount === "amount"}
                                       onChange={handleAmountOrPercent}
                                />{' '}
                                <Translate id="coupons-manager.amount-off"
                                           options={{renderInnerHtml: true}}/>{' '}
                            </Label>
                        </FormGroup>
                    </FormGroup>
                    </Col>
                </Row>

                {
                    percentOrAmount === "percent" ?
                    <Row>
                        <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                            <Label for="discount" className="label-forms-blue form-label">
                                * <Translate id="coupons-manager.discount" options={{renderInnerHtml: true}}/>
                            </Label>
                            <br/>
                                <small><Translate id="coupons-manager.percent-off-instructions"
                                                  options={{renderInnerHtml: true}}/>{' '}
                                </small>
                            <Input className="mantine-like__control" value={percentOff}
                                   type="number"
                                   name="percentOff"
                                   id="percentOff"
                                   maxLength={3}
                                   onChange={handlePercentOff}/>
                            <br/>
                            {
                                (percentOff === null ||  percentOff.trim().length === 0) ?
                                    <Alert color="danger">
                                        <Translate id="coupons-manager.discount-required"
                                                   options={{renderInnerHtml: true}}/>
                                    </Alert> : null
                            }
                        </Col>
                    </Row> :
                        <Row>
                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                <Label for="currency" className="label-forms-blue form-label">
                                    * <Translate id="coupons-manager.currency" options={{renderInnerHtml: true}}/>
                                </Label>
                                <br/>
                                <small><Translate id="coupons-manager.currency-instructions"
                                                  options={{renderInnerHtml: true}}/>{' '}
                                </small>
                                <SelectorCurrency disabled={false} value={currency}
                                                  updateParent={handleCurrencyChange}/>
                            </Col>
                            <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                                <Label for="discount" className="label-forms-blue form-label">
                                    * <Translate id="coupons-manager.discount" options={{renderInnerHtml: true}}/>
                                </Label>
                                <br/>
                                <small><Translate id="coupons-manager.amount-off-instructions"
                                                  options={{renderInnerHtml: true}}/>{' '}
                                </small>
                                <Input value={amountOff}
                                       type="number"
                                       name="amountOff"
                                       id="amountOff"
                                       maxLength={5}
                                       onChange={handleAmountOff}/>
                                <br/>
                                {
                                    (amountOff === null ||  amountOff.trim().length === 0) ?
                                        <Alert color="danger">
                                            <Translate id="coupons-manager.discount-required"
                                                       options={{renderInnerHtml: true}}/>
                                        </Alert> : null
                                }
                            </Col>
                        </Row>
                }
                <Row>
                    <Col xs={{size: 12}} sm={{size: 12}} md={{size: 12}} lg={{size: 12}}>
                        {
                            !valid?
                                <Alert color="danger">
                                    <Translate id="coupons-manager.submit-error"
                                               options={{renderInnerHtml: true}}/>
                                </Alert> : null
                        }
                        <Button color="success" onClick={handleCouponSubmit}>
                            <Translate id="coupons-manager.submit"
                                       options={{renderInnerHtml: true}}/>
                        </Button>
                    </Col>
                </Row>

                {loading ? <LoadingFullWindow/> : null}
                {showModal ?
                    <ModalGeneric data={propsModal} updateParent={toggleAlertModal}/> : null}

            </div>
        )
};


export default FormCouponManager;