import React, {useEffect} from 'react';
import {Link} from "react-router-dom";
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import './style.css';
import { getAsset } from '../../../utils/getAssets';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const IconPlan = getAsset("Plans-icon-Plan");
const IconAssignments = getAsset("Plans-icon-Assignments");

/**
 * The component (window) to handle the planning selection
 */
const PlanningSelect = () => {

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        document.body.classList.remove("landing-white");
    }, []);

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <div className="wrapper-planning mt-5">
                        {/* My Content */}
                        {/*}
                        <div className="planning-two-col bg-SmokeGray">
                            <div className="mod-planning">
                                <div className="mod-planning-title">
                                    <div className="mod-planning-title__fixed-size">
                                        <img src={IconPlan} alt="" />
                                    </div>
                                    <div className="mod-planning-title__auto-size">
                                        <h1 className="mod-planning-title__title txt-custom">
                                            <Translate
                                                id="planning.my-content"
                                                options={{ renderInnerHtml: true }}
                                            />
                                        </h1>
                                    </div>
                                </div>
                                <p>
                                    <Translate
                                        id="planning.my-content-subtitle"
                                        options={{ renderInnerHtml: true }}
                                    />
                                </p>
                                <div className="center-button btn-bg-DarkBlue">
                                    <Link to="/plannings" className="centered-text btn button-generic">
                                        <Translate id="general.enter" options={{ renderInnerHtml: true }} />
                                    </Link>
                                </div>
                            </div>
                        </div> */}

                        {/* Assignments */}
                        <div className="planning-two-col bg-SmokeGray">
                            <div className="mod-planning">
                                <div className="mod-planning-title">
                                    <div className="mod-planning-title__fixed-size">
                                        <img src={IconAssignments} alt="" />
                                    </div>
                                    <div className="mod-planning-title__auto-size">
                                        <h1 className="mod-planning-title__title text-assignments-title">
                                            <Translate
                                                id="planning.assignments"
                                                options={{ renderInnerHtml: true }}
                                            />
                                        </h1>
                                    </div>
                                </div>
                                <p>
                                    <Translate
                                        id="planning.assignments-subtitle"
                                        options={{ renderInnerHtml: true }}
                                    />
                                </p>
                                <div className="center-button btn-bg-LightGreen">
                                    <Link to="/assignments" className="centered-text btn button-primary-custom">
                                        <Translate id="general.enter" options={{ renderInnerHtml: true }} />
                                    </Link>
                                </div>
                            </div>
                        </div>


                        {/* Planning v2 */}
                        <div className="planning-two-col bg-SmokeGray">
                            <div className="mod-planning">
                                <div className="mod-planning-title">
                                    <div className="mod-planning-title__fixed-size">
                                        <img src={IconPlan} alt="" />
                                    </div>
                                    <div className="mod-planning-title__auto-size">
                                        <h1 className="mod-planning-title__title text-plans-title">
                                            <Translate
                                                id="planning.planning-title"
                                                options={{ renderInnerHtml: true }}
                                            />{" "}
                                        </h1>
                                    </div>
                                </div>
                                <p>
                                    <Translate
                                        id="planning.my-content-subtitle"
                                        options={{ renderInnerHtml: true }}
                                    />
                                </p>
                                <div className="center-button btn-bg-DarkBlue">
                                    <Link to="/v2/plannings" className="centered-text btn button-primary-custom">
                                        <Translate id="general.enter" options={{ renderInnerHtml: true }} />
                                    </Link>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default withLocalize(PlanningSelect);