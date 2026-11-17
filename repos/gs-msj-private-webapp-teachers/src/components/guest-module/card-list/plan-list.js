/**
 * Created by portiz on 2/22/19.
 */
import React, { useState, useEffect } from 'react';
import {getCommercialPlans} from '../../../api/subscriptions';
import {Col, Row, TabContent, TabPane, Card, CardText, Button} from 'reactstrap';
import {Translate} from "react-localize-redux";
import ModalOffer from '../modal/modal-offer';

/**
 * Handle the checkboxes of the countries isos
 */
const PlanList = ({ term, updateParent }) => {
    const [activeTab, setActiveTab] = useState(term ?? "2");
    const [mounted, setMounted] = useState(false);
    const [planMonthlyList, setPlanMonthlyList] = useState([]);
    const [planYearList, setPlanYearList] = useState([]);
    const [showModalOffer, setShowModalOffer] = useState(false);
    const [planIdSelected, setPlanIdSelected] = useState(null);
    const [trialDays, setTrialDays] = useState(null);

    useEffect(() => {
        mountData();
    }, []);

    const toggle = (tab) => {
        if (activeTab !== tab) {
            setActiveTab(tab);
        }
    };

    // Mounts the data. Call the data from the server and sets the data to the list
    const mountData = () => {
        let apiCall = getCommercialPlans();

        // After the api call, fill the data
        apiCall.then((response) => {
            let yearlyPlans = [];
            let monthlyPlans = [];

            for (let i = 0; i < response.data.length; i++) {
                if (response.data[i].Interval === "year") {
                    yearlyPlans.push(response.data[i]);
                }

                if (response.data[i].Interval === "month") {
                    monthlyPlans.push(response.data[i]);
                }
            }

            setPlanYearList(yearlyPlans);
            setPlanMonthlyList(monthlyPlans);
            setMounted(true);
        });
    };

    const handleSelect = (e) => {
        updateParent(e.currentTarget.value, e.currentTarget.attributes.interval.value);
    };

    const handleSelectMonthly = (e) => {
        setShowModalOffer(true);
        setPlanIdSelected(e.currentTarget.value);
        setTrialDays(e.currentTarget.getAttribute('trialdays'));
    };

    const modelOffer = (e) => {
        if(e === 1) { // Offer was accepted
            toggle("1");
        } else {
            // Redirect user to selected plan
            updateParent(planIdSelected, "month");
        }
        setShowModalOffer(false);
    };

    return (
        !mounted ? null :
        <div>
                {/*<Nav tabs>
                    <NavItem>
                        <NavLink
                            className="btn btn-orange-subscription white-text margin-buttons-next"
                            onClick={() => {toggle('1')}}>
                            <Translate id="plan.yearly" options={{renderInnerHtml: true}}/>
                        </NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink
                            className="btn btn-blue-subscription white-text margin-buttons-next"
                            onClick={() => {toggle('2')}}>
                            <Translate id="plan.monthly" options={{renderInnerHtml: true}}/>
                        </NavLink>
                    </NavItem>

                </Nav> */}
            {showModalOffer ? <ModalOffer updateParent={modelOffer}/> : null }
            <TabContent activeTab={activeTab}>
                <TabPane tabId="1">
                    <Row>
                        {
                            planYearList.map((item) => {
                                return(
                                    <Col sm={{size: 12}} md={{size: 6}} lg={{size: 4}} key={item.Id}>
                                        <Card body className="ie11up ie910 animated delay-05s fadeIn">
                                            <CardText>
                                                <span dangerouslySetInnerHTML={{ __html: item.PlanDescriptionByLang[0].Description }}/>
                                            </CardText>
                                            <hr/>
                                            <button trialdays={item.TrialDays} value={item.Id} className="btn-default button-primary-custom"
                                                    onClick={handleSelect}
                                                    interval={item.Interval}>
                                                <Translate id="plan.buy-now" options={{renderInnerHtml: true}}/>
                                            </button>
                                        </Card>
                                    </Col>
                                )
                            })
                        }
                    </Row>
                </TabPane>
                <TabPane tabId="2">
                    <Row>
                        {
                            planMonthlyList.map((item) => {
                                return(
                                    <Col sm={{size: 12}} md={{size: 6}} lg={{size: 4}} key={item.Id}>
                                        <Card body className="card-subscriptions animated delay-05s fadeIn">

                                            <CardText>
                                                <span dangerouslySetInnerHTML={{ __html: item.PlanDescriptionByLang[0].Description }}/>
                                            </CardText>
                                            <hr/>

                                            <button trialdays={item.TrialDays} value={item.Id} className="btn-default button-primary-custom"
                                                    onClick={handleSelectMonthly} interval={item.Interval}>
                                                    <Translate id="plan.buy-now" options={{renderInnerHtml: true}}/>
                                            </button>
                                        </Card>
                                    </Col>
                                )
                            })
                        }
                    </Row>
                </TabPane>

            </TabContent>
        </div>
    );
};

export default PlanList;