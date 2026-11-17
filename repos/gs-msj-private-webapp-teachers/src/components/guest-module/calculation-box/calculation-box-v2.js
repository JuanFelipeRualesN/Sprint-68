/**
 * Created by portiz on 3/2/20.
 */
import React, { useState, useEffect } from 'react';
import {Col, Row} from 'reactstrap';
import {Translate} from "react-localize-redux";

/**
 * Handle the checkboxes of the countries isos
 */
const CalculationBox = ({ costData }) => {
    const [costDataState, setCostDataState] = useState(costData.costData);

    useEffect(() => {
        setCostDataState(costData.costData);
    }, [costData]);

    if (costDataState == null) {
        return null;
    }

    let taxesStr = '';
    if(costDataState.Taxes !== null) {
        for (let i = 0; i < costDataState.Taxes.length; i++) {
            taxesStr += costDataState.Taxes[i].TaxName;
        }
    }
    let intervalTranslate = "plan-calculator-form." + costDataState.Interval;

    return(
        <div style={{backgroundColor: '#f0f0f0', paddingTop: 10, paddingBottom: 10, paddingLeft: 5, paddingRight: 5, borderRadius: 5}}>
            <Row>
                <Col sm={{ size: 5 }} md={{ size: 5 }} lg={{ size: 5}} xs={{ size: 5 }}>
                    <Translate id="plan-calculator-form.subtotal"/>
                </Col>
                <Col sm={{ size: 4 }} md={{ size: 4 }} lg={{ size: 4 }} xs={{ size: 4 }}>
                    { costDataState.SubTotalWithDiscount !== null ?
                        <span style={{textDecoration: "line-through"}}>
                            {costDataState.SubTotal}
                        </span> :
                        <span>
                            {costDataState.SubTotal}
                        </span>
                    }
                </Col>
            </Row>
            {
                costDataState.SubTotalWithDiscount !== null ?
                    <Row className="green-text">
                        <Col sm={{ size: 5 }} md={{ size: 5 }} lg={{ size: 5}} xs={{ size: 5 }}>
                            <Translate id="plan-calculator-form.subtotal-with-discount" />
                        </Col>
                        <Col sm={{ size: 4 }} md={{ size: 4 }} lg={{ size: 4 }} xs={{ size: 4 }}>
                            {costDataState.SubTotalWithDiscount}
                        </Col>
                    </Row> : null
            }

            {
                taxesStr !== '' &&
                (typeof costDataState.TaxTotalWithDiscount === 'undefined'
                    || costDataState.TaxTotalWithDiscount === null) ?
                    <Row>
                        <Col sm={{ size: 5 }} md={{ size: 5 }} lg={{ size: 5}} xs={{ size: 5 }}>
                            <Translate id="plan-calculator-form.taxtotal"/> {' '}
                            <small>{taxesStr}</small>
                        </Col>
                        <Col sm={{ size: 4 }} md={{ size: 4 }} lg={{ size: 4 }} xs={{ size: 4 }}>
                            {costDataState.TaxTotal}
                        </Col>

                    </Row> : null
            }
            {
                typeof costDataState.TaxTotalWithDiscount !== 'undefined'
                && costDataState.TaxTotalWithDiscount !== null
                && costDataState.TaxTotalWithDiscount.length > 0
                && costDataState.TaxTotalWithDiscount !== "0"
                    ?
                    <Row>
                        <Col sm={{ size: 5 }} md={{ size: 5 }} lg={{ size: 5}} xs={{ size: 5 }}>
                            <Translate id="plan-calculator-form.taxtotal-with-discount"/> {' '}
                            <small>{taxesStr}</small>
                        </Col>
                        <Col sm={{ size: 4 }} md={{ size: 4 }} lg={{ size: 4 }} xs={{ size: 4 }}>
                            {costDataState.TaxTotalWithDiscount}
                        </Col>
                    </Row>
                    : null
            }
            <Row>
                <Col sm={{ size: 5 }} md={{ size: 5 }} lg={{ size: 5}} xs={{ size: 5 }}>
                    {costDataState.TotalWithDiscounts === null ?
                        <b><Translate id="plan-calculator-form.total"/></b> :
                        <span style={{textDecoration: "line-through"}}>
                                <b><Translate id="plan-calculator-form.total"/></b>
                            </span>
                    }

                </Col>
                <Col sm={{ size: 4 }} md={{ size: 4 }} lg={{ size: 4 }} xs={{ size: 4 }}>
                    {costDataState.TotalWithDiscounts === null ?
                        <b> {costDataState.TotalCost}</b> :
                        <span style={{textDecoration: "line-through"}}>
                                <b> {costDataState.TotalCost}</b>
                            </span>
                    }
                </Col>
            </Row>
            {costDataState.TotalWithDiscounts !== null ?
                <Row>
                    <Col sm={{ size: 5 }} md={{ size: 5 }} lg={{ size: 5}} xs={{ size: 5 }}>
                        <b className="green-text">
                            <Translate id="plan-calculator-form.total-with-discount"/>
                            {' ('}
                            <Translate id={intervalTranslate}/> {') '}</b>
                        <br/>
                    </Col>
                    <Col sm={{ size: 4 }} md={{ size: 4 }} lg={{ size: 4 }} xs={{ size: 4 }}>
                        <b className="green-text">
                            {costDataState.TotalWithDiscounts}
                        </b>
                    </Col>
                </Row> : null}
            <Row>
                {
                    costDataState.Interval === 'month' ?
                        <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }} xs={{ size: 12 }}>
                            <span> <Translate id="plan-calculator-form.automatic-bill"/> {costDataState.IntervalCount} <Translate id="plan-calculator-form.months"/> </span>
                        </Col> :
                        <Col sm={{ size: 12 }} md={{ size: 12 }} lg={{ size: 12 }} xs={{ size: 12 }}>
                            <span> <Translate id="plan-calculator-form.automatic-bill"/> {costDataState.IntervalCount}  <Translate id="plan-calculator-form.years"/> </span>
                        </Col>
                }
            </Row>
        </div>
    );
};

export default CalculationBox;