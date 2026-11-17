/**
 * Created by portiz on 05/20/2020.
 */
import React, { useState, useEffect } from 'react';
import {Card, CardBody, CardTitle, Col, Container, Row} from 'reactstrap';
import {Link, Redirect} from "react-router-dom";
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
/** End of Multiple Language imports **/
import SessionManager from "../../../custom-classes/session-manager";
import Settings from "../../../custom-classes/settings";
import {getIfStudentsHasPlannings,getTeacherPlannings} from "../../../api/teachers";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import './style.css';
import iconHomeschoolerAverage from '../../../assets/images/common-icons/Homeschooler-icon-Average.svg';
import iconHomeschoolerResults from '../../../assets/images/common-icons/Homeschooler-icon-Results.svg';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * The component (window) to handle the progress select
 */
const ProgressSelect = () => {
    const [loading, setLoading] = useState(false);
    const [roles, setRoles] = useState(SessionManager.getSessionDetails() === null ? null : SessionManager.getSessionDetails().Roles);
    const [sessionValid, setSessionValid] = useState(true);
    const [hasPlannings, setHasPlannings] = useState(false);
    const [totalPages, setTotalPages] = useState(0);
    const [data, setData] = useState([]);

    // Initialize translations based on the current version
    useVersionedTranslations();

    /** Evaluate the resize, to show the proper navbar menu style*/
    useEffect(() => {
        document.body.classList.remove('landing-white');

        if(roles.indexOf(Settings.ROLE_TEACHER_STR) > -1) {
            checkIfTeacherPlannings();
        }
        else
        {
            // Check if has plannings
            // This is too heavy for admin, thats why we remove that option to enter directly to the record.
            if(roles.indexOf(Settings.ROLE_EMPLOYEE_STR) === -1 || roles.indexOf(Settings.ROLE_ADMIN_STR) === -1) {
                checkIfStudentsHasPlannings();
            }
            else
            {
                setHasPlannings(true); // Default for Admin.
            }
        }
    }, []);

    /**
     * Check if teacher has any academic plan created.
     */
    const checkIfTeacherPlannings = () => {
        setLoading(true);
        let apiCall = getTeacherPlannings();
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(1);
            setHasPlannings(response.data.length > 0);
            setData([]);
        });
    };

    /**
     * Check if student has any plan.
     */
    const checkIfStudentsHasPlannings = () => {
        setLoading(true);
        let apiCall = getIfStudentsHasPlannings();
        apiCall.then((response) => {
            setLoading(false);
            setTotalPages(1);
            setHasPlannings(response.data);
            setData([]);
        });
    };


    // Renders the main dashboard

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    {/* Homeschooler progress */}

                    {loading ? <LoaderFullWindow/> :
                    <div className="wrapper-planning mt-5">
                        {

                        }
                        {hasPlannings || !Settings.IS_APP_MANAGED_BY_REQUIRED?
                                <Redirect to="/record"/>
                                 : null
                            }
                            {!hasPlannings ? 
                        <div className="planning-two-col border-blueLight">
                            <div className="mod-planning">
                                    <div className="mod-planning-title">
                                        <div className="mod-planning-title__fixed-size">
                                            <img src={iconHomeschoolerAverage} alt="dashboard-average" />
                                        </div>
                                        <div className="mod-planning-title__auto-size">
                                            <h1 className="mod-planning-title__title planning-select-title">
                                                <Translate id="dashboard.profile-analysis"
                                                        options={{renderInnerHtml: true}}/>
                                            </h1>
                                        </div>
                                    </div>

                                    <p>
                                        <Translate id="dashboard.profile-analysis-subtitle"
                                            options={{renderInnerHtml: true}}/>
                                    </p>
                                    <div className="center-button btn-bg-DarkBlue">
                                        <Link to={{pathname: "/report-average", query: "/progress-select"}} className="centered-text btn btn-default button-primary-custom">
                                            <Translate id="dashboard.enter"
                                                options={{renderInnerHtml: true}}/>
                                        </Link>
                                    </div>
                            </div>
                        </div> : null
                            }
                            {!hasPlannings ?

                        <div className="planning-two-col border-greenLight">
                            <div className="mod-planning">
                                <div className="mod-planning-title">
                                    <div className="mod-planning-title__fixed-size">
                                        <img src={iconHomeschoolerResults} alt="dashboard-report-results" />
                                    </div>
                                    <div className="mod-planning-title__auto-size">
                                        <h1 className="mod-planning-title__title txt-greenDark">
                                            <Translate id="dashboard.results"
                                                options={{renderInnerHtml: true}}/>
                                        </h1>
                                    </div>
                                </div>
                                <p>
                                    <Translate id="dashboard.results-subtitle"
                                        options={{renderInnerHtml: true}}/>
                                </p>

                                <div className="center-button btn-bg-LightGreen">
                                    <Link to={{pathname: "/report-results", query: "/progress-select"}} className="centered-text btn btn-default button-primary-custom">
                                        <Translate id="dashboard.enter"
                                            options={{renderInnerHtml: true}}/>
                                    </Link>
                                </div>
                            </div>

                        </div>: null
                            }

                    </div> }

                </div>
            </div>

        </div>
    );
};

export default withLocalize(ProgressSelect);