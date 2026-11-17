import React, { useState, useEffect } from 'react';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowAltCircleLeft, faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import { Translate } from "react-localize-redux";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import { renderToStaticMarkup } from "react-dom/server";
import { withRouter } from 'react-router-dom';
import { withLocalize } from "react-localize-redux";
import { getStudentPlanningsv2 } from "../../../../api/teachers";
import useVersionedTranslations from '../../../../hooks/useVersionedTranslations';
import styled from "styled-components";

const EmptyListStyled = styled.p`
  color: #ccc;
  display: block
  justify-content: center;
  text-align: center;
  margin: 3em auto;

  & i {
    display: block;
    margin: 2em 0 0.25em 0;
    font-size: 45px;
    opacity: .5;
  }
`;
const InstructionalPlans = ({ match, history }) => {
    const [loading, setLoading] = useState(false);
    const [plans, setPlans] = useState([]);
    const [studentId, setStudentId] = useState("");

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        const studentIdFromParams = match.params.studentId;
        setStudentId(studentIdFromParams);
        getStudentPlannings(studentIdFromParams);
    }, [match.params.studentId]);

    const getStudentPlannings = async (studentId) => {
        setLoading(true);
        try {
            const res = await getStudentPlanningsv2(studentId);
            const data = res.data;
            setLoading(false);
            setPlans(data);
        } catch (error) {
            setLoading(false);
        }
    };

    const goBack = () => {
        history.goBack();
    };

    return (
        <>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <a className="btn-back" onClick={goBack}>
                        <FontAwesomeIcon icon={faArrowLeft} />{" "}
                        <Translate
                            id="general.back-action"
                            options={{ renderInnerHtml: false }}
                        />
                    </a>
                    <div className="wrapper-col borderGray-bottom block-display-responsive">
                        <div className="col col-no-padding-left">
                            <div className="title-principal"><Translate id="pending-assignments.assignment" /></div>
                        </div>
                    </div>
                    <div className='mt-2'>
                        <div className="wrapper-school wrapper-flex-start">
                            {plans.length > 0 && plans.map((plan, i) => (
                                <div className="school-three-col school-three-col--clear-margin-bottom border-blueLight" key={i}>
                                    <div className="mod-school">
                                        <div className="mod-school__title txt-custom">
                                            {plan.SubjectName} {" "} {plan.LevelCode}
                                        </div>
                                        <div className="mod-school__description mt-4">
                                            <div><Translate id="pending-assignments.plan" />: <b>{plan.PlanName}</b></div>
                                            {plan.TeacherName && <div><Translate id="pending-assignments.teacher" />: <b>{plan.TeacherName}</b></div>}
                                            {plan.CreatedAt && <div className="txt-LighterBlue"><Translate id="pending-assignments.created-at" />: <b>{plan.CreatedAt}</b></div>}
                                            <Link to={`/pending-assignments-student-planning/${studentId}/${plan.PlanId}`} className="btn-simple--only-text">
                                                <Translate id="subscription-module.go" options={{ renderInnerHtml: true }} />
                                            </Link>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                    {!plans.length && (
                        <EmptyListStyled>
                            <i className="fas fa-book-open"></i>
                            <Translate id="pending-assignments.plans-empty" />
                        </EmptyListStyled>
                    )}
                </div>
            </div>
            {loading ? <LoadingFullWindow /> : null}
        </>
    );
};

export default withRouter(withLocalize(InstructionalPlans));