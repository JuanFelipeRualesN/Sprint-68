/**
 * Created by portiz on 9/6/19.
 */
import React, { useState, useEffect } from 'react';
import {getCommercialPlans} from '../../../api/subscriptions';
import { Col, Row, FormGroup, Label, Input} from 'reactstrap';

/**
 * Handle the checkboxes of the countries isos
 */
const PlanRadioList = ({ planId, termSelected, updateParent }) => {
    const [mounted, setMounted] = useState(false);
    const [planIdState] = useState(planId ?? null);
    const [planList, setPlanList] = useState([]);
    const [termSelectedState] = useState(termSelected ?? null);

    useEffect(() => {
        mountData();
    }, []);

    // Mounts the data. Call the data from the server and sets the data to the list
    const mountData = () => {
        let apiCall = getCommercialPlans();
        // After the api call, fill the data
        apiCall.then((response) => {
            let filteredPlans = [];
            for (let i = 0; i < response.data.length; i++) {
                if (response.data[i].Interval === termSelectedState) {
                    filteredPlans.push(response.data[i]);
                }
            }

            setPlanList(filteredPlans);
            setMounted(true);
        });
    };

    // Action when the checkbox changes
    const handleSelect = (e) => {
        updateParent(e.currentTarget.value);
    };

    if (!mounted) {
        return null;
    }

    return (
        <Row className="align-vertical-center">
            {
                planList.map((item) => {
                    return (
                        <Col xs={{size:6}} sm={{size:4}} md={{size:4}} lg={{size:4}} key={item.Id}>
                            <FormGroup check className="margin-left">
                                <Label check>
                                    {item.Id === planIdState ?
                                        <Input type="radio" name="radio1" value={item.Id} defaultChecked
                                               onChange={handleSelect}/>
                                        : <Input type="radio" name="radio1" value={item.Id} onChange={handleSelect}/>
                                    }
                                    {' '}{item.PlanDescriptionByLang[0].Name}
                                </Label>
                            </FormGroup>
                        </Col>
                    )
                })
            }
        </Row>
    );
};

export default PlanRadioList;