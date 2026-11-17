/**
 * Created by portiz on 7/09/20.
 */
import React, { useState, useContext } from "react";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
/** End of Multiple Language imports **/
import { AppContext } from "../../../App.Context";
import { getAsset } from "../../../utils/getAssets";

const IconStudent = getAsset("Modal-icon-Students");
const IconGroups = getAsset("Modal-icon-Groups");
const IconInvitations = getAsset("Modal-icon-Invitations");
const IconMessages = getAsset("Modal-icon-Messages");
const IconPlans = getAsset("Modal-icon-Plans");
const IconProgress = getAsset("Modal-icon-Progress");

/**
 * Class that handles the modal how to use it
 */
const ModalShowMeHowToUseItAlt = ({ visible, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);
    const context = useContext(AppContext);

    // Hide or shows the window
    const toggle = () => {
        localStorage.setItem("modal-tour-seen", "1");
        updateParent();          // parent sets modalShowHowToUseGSWeb = false
    };

    return (
        <div>
            <Modal isOpen={visible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                    <Translate id="dashboard-steps.steps-alt-title" />
                </ModalHeader>
                <ModalBody className="pt-0">
                    <p><Translate id="dashboard-steps.steps-alt" /></p>
                    <div className="wrapper-modal-steps-col">
                    {context.state.variables.InvitationTeacherToStudentManagementTeacher && <div className="steps-three-col">
                        <div className="mod-steps">
                            <div className="mod-steps-title">
                                <div className="mod-steps-title__fixed-size mod-steps-title__icon">
                                    {" "}
                                    <img src={IconStudent} alt="Student icon" />{" "}
                                </div>
                                <div className="mod-steps-title__auto-size mod-steps-title__title">
                                    {/* <p className="__steps">
                                        <Translate id="dashboard-steps.steps1-title" />
                                    </p> */}
                                    <p className="__title">
                                        <Translate id="dashboard-steps.steps1-subtitle" />
                                    </p>
                                </div>
                            </div>
                            <p>
                                <Translate id="dashboard-steps.steps1-content" />
                            </p>
                        </div>
                    </div>}

                    <div className="steps-three-col">
                        <div className="mod-steps">
                            <div className="mod-steps-title">
                                <div className="mod-steps-title__fixed-size mod-steps-title__icon">
                                    {" "}
                                    <img src={IconGroups} alt="Groups icon" />{" "}
                                </div>
                                <div className="mod-steps-title__auto-size mod-steps-title__title">
                                    {/* <p className="__steps">
                                        <Translate id="dashboard-steps.steps2-title" />
                                    </p> */}
                                    <p className="__title">
                                        <Translate id="dashboard-steps.steps2-subtitle" />
                                    </p>
                                </div>
                            </div>
                            <p>
                                <Translate id="dashboard-steps.steps2-content" />
                            </p>
                        </div>
                    </div>

                    <div className="steps-three-col">
                        <div className="mod-steps">
                            <div className="mod-steps-title">
                                <div className="mod-steps-title__fixed-size mod-steps-title__icon">
                                    {" "}
                                    <img src={IconPlans} alt="Plans icon" />{" "}
                                </div>
                                <div className="mod-steps-title__auto-size mod-steps-title__title">
                                    {/* <p className="__steps">
                                        <Translate id="dashboard-steps.steps3-title" />
                                    </p> */}
                                    <p className="__title">
                                        <Translate id="dashboard-steps.steps3-subtitle" />
                                    </p>
                                </div>
                            </div>
                            <p>
                                <Translate
                                    id="dashboard-steps.steps3-content"
                                    options={{ renderInnerHtml: true }}
                                />
                            </p>
                        </div>
                    </div>

                    <div className="steps-three-col">
                        <div className="mod-steps">
                            <div className="mod-steps-title">
                                <div className="mod-steps-title__fixed-size mod-steps-title__icon">
                                    {" "}
                                    <img src={IconProgress} alt="Progress icon" />{" "}
                                </div>
                                <div className="mod-steps-title__auto-size mod-steps-title__title">
                                    {/* <p className="__steps">
                                        <Translate id="dashboard-steps.steps4-title" />
                                    </p> */}
                                    <p className="__title">
                                        <Translate id="dashboard-steps.steps4-subtitle" />
                                    </p>
                                </div>
                            </div>
                            <p>
                                <Translate id="dashboard-steps.steps4-content" />
                            </p>
                        </div>
                    </div>

                    {context.state.variables.MessageManagementTeacher && <div className="steps-three-col">
                        <div className="mod-steps">
                            <div className="mod-steps-title">
                                <div className="mod-steps-title__fixed-size mod-steps-title__icon">
                                    {" "}
                                    <img src={IconMessages} alt="Messages icon" />{" "}
                                </div>
                                <div className="mod-steps-title__auto-size mod-steps-title__title">
                                    {/* <p className="__steps">
                                        <Translate id="dashboard-steps.steps5-title" />
                                    </p> */}
                                    <p className="__title">
                                        <Translate id="dashboard-steps.steps5-subtitle" />
                                    </p>
                                </div>
                            </div>
                            <p>
                                <Translate id="dashboard-steps.steps5-content" />
                            </p>
                        </div>
                    </div>}

                    {context.state.variables.InvitationTeacherToParentManagementTeacher && <div className="steps-three-col">
                        <div className="mod-steps">
                            <div className="mod-steps-title">
                                <div className="mod-steps-title__fixed-size mod-steps-title__icon">
                                    {" "}
                                    <img src={IconInvitations} alt="Invitations icon" />{" "}
                                </div>
                                <div className="mod-steps-title__auto-size mod-steps-title__title">
                                    {/* <p className="__steps">
                                        <Translate id="dashboard-steps.steps6-title" />
                                    </p> */}
                                    <p className="__title">
                                        <Translate id="dashboard-steps.steps6-subtitle" />
                                    </p>
                                </div>
                            </div>
                            <p>
                                <Translate id="dashboard-steps.steps6-content" />
                            </p>
                        </div>
                    </div>}
                    </div>            
                </ModalBody>
                <ModalFooter className="pt-0">
                    <Button onClick={toggle} color="info">
                        <Translate id="dashboard-steps.close" />
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalShowMeHowToUseItAlt;
