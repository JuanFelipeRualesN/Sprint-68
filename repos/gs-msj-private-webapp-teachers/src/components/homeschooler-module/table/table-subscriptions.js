import React, { useState, useEffect, useCallback } from 'react';
import { Button, Table, Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import {getUserSubscriptions} from '../../../api/subscriptions';
import { Link, useHistory  } from 'react-router-dom'; // Import useNavigate for programmatic navigation
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faBan, faUsers} from "@fortawesome/free-solid-svg-icons";

/**
 * Table that includes the main account subscriptions.
 */
const TableSubscriptions = (props) => {
    // State Hooks
    const [modal, setModal] = useState(false);
    const [rows, setRows] = useState([]);
    const [subscriptionId, setSubscriptionId] = useState(null); // Used for redirection

    const history = useHistory();

    // Event Handlers - wrapped in useCallback for performance
    const toggle = useCallback(() => {
        setModal(prevModal => !prevModal);
    }, []);

    const redirectToProfileManagement = useCallback((e) => {
        // In React Router v6, if you want to redirect, you use `Maps`
        // The `value` on the button typically holds the subscriptionId
        const clickedSubscriptionId = e.currentTarget.value;
        setSubscriptionId(clickedSubscriptionId); // Set state, then useEffect handles navigation
    }, []);

    // Effect Hook for data fetching (componentWillMount equivalent)
    useEffect(() => {
        const fetchSubscriptions = async () => {
            let fetchedSubscriptions = []; // Array to build table rows

            try {
                const response = await getUserSubscriptions();

                if (response.status === 200 || response.status === 201) {
                    const subscriptionsDB = response.data;

                    for (let i = 0; i < subscriptionsDB.length; i++) {
                        const subscription = subscriptionsDB[i];
                        const intervalTranslateId = `subscriptions.${subscription.Plan.Interval}`;
                        const statePlanTranslatedId = `subscriptions.${subscription.Status}`;

                        let rowContent;
                        // If the status is not 'active' or 'trialing', hide the cancel button
                        if (subscription.Status !== 'active' && subscription.Status !== 'trialing') {
                            rowContent = (
                                <>
                                    <td>{'ORD' + subscription.SubscriptionId}</td>
                                    <td>{subscription.Plan.PlanDescriptionByLang[0]?.Name}</td> {/* Added optional chaining */}
                                    <td><Translate id={intervalTranslateId}/></td>
                                    <td>{subscription.ProfilesTotalQuantity}</td>
                                    <td><Translate id={statePlanTranslatedId}/></td>
                                    <td></td> {/* Empty for cancel button */}
                                    <td></td> {/* Empty for manage profiles button */}
                                </>
                            );
                        } else {
                            rowContent = (
                                <>
                                    <td>{'ORD' + subscription.SubscriptionId}</td>
                                    <td>{subscription.Plan.PlanDescriptionByLang[0]?.Name}</td> {/* Added optional chaining */}
                                    <td><Translate id={intervalTranslateId}/></td>
                                    <td>{subscription.ProfilesTotalQuantity}</td>
                                    <td><Translate id={statePlanTranslatedId}/></td>
                                    <td>
                                        <Button className="btn-danger" onClick={toggle}>
                                            <FontAwesomeIcon icon={faBan}/>{' '}
                                            <Translate id="subscriptions.cancel" options={{renderInnerHtml: false}}/>
                                        </Button>
                                    </td>
                                    <td>
                                        <Link className="btn-default button-primary-custom btn-bg-blueDark" to={{
                                            pathname: "/add-child-profile",
                                            state: { subscriptionId: subscription.SubscriptionId } // Pass state via `state` property
                                        }}>
                                            <FontAwesomeIcon icon={faUsers}/>{' '}
                                            <Translate id="subscriptions.manage-profiles"/>
                                        </Link>
                                    </td>
                                </>
                            );
                        }
                        fetchedSubscriptions.push(<tr key={subscription.SubscriptionId}>{rowContent}</tr>);
                    }
                }
            } catch (error) {
                console.error("Error fetching subscriptions:", error);
                // Handle error state if needed
            } finally {
                setRows(fetchedSubscriptions);
            }
        };

        fetchSubscriptions();
    }, [toggle]); // Dependency array: `toggle` is used inside, so it should be a dependency.
    // useCallback ensures `toggle` itself is stable.

    // Effect Hook for redirection based on `subscriptionId`
    useEffect(() => {
        if (subscriptionId !== null && subscriptionId !== '') {
            // IMPORTANT CHANGE: Use history.push() for programmatic navigation in React Router v5
            // In v5, state is passed as the second argument directly to push/replace
            history.push("/add-child-profile", { subscriptionId: subscriptionId });
            // Optionally, clear subscriptionId state after navigation if you don't want to navigate again
            // setSubscriptionId(null);
        }
    }, [subscriptionId, history]); // Dependencies: subscriptionId state and history object

    return (
        <div>
            <Table className="card-white" responsive>
                <thead>
                <tr>
                    <th><Translate id="subscriptions.ref" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="subscriptions.plan" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="subscriptions.interval" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="subscriptions.profiles-included" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="subscriptions.status" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="subscriptions.cancel" options={{renderInnerHtml: false}}/></th>
                    <th><Translate id="subscriptions.manage-profiles" options={{renderInnerHtml: false}}/></th>
                </tr>
                </thead>
                <tbody>
                { rows.length > 0 ? rows : null }
                {/* You might want a loading/empty state instead of just null */}
                {/* { rows.length === 0 ? <tr><td colSpan="7">No subscriptions found.</td></tr> : rows } */}
                </tbody>
            </Table>

            <Modal isOpen={modal} toggle={toggle} className={props.className}>
                <ModalHeader className="card-white" toggle={toggle}>
                    <Translate id="subscriptions.cancel-title" options={{renderInnerHtml: false}}/>
                </ModalHeader>
                <ModalBody className="card-white">
                    <Translate id="subscriptions.cancel-details" options={{renderInnerHtml: true}}/>
                </ModalBody>
                <ModalFooter className="card-white">
                    <Button onClick={toggle} color="info">
                        <Translate id="subscriptions.close" options={{renderInnerHtml: false}}/>
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default TableSubscriptions;