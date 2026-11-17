import React, { useEffect } from "react";
import PlanningsMenu from "./plannings-menu";
import PlanningsDetailHeading from "./plannings-detail-heading";
import BreadcrumbProvider from "./shared/providers/breadcrumb";
import Breadcrumb from "./shared/components/breadcrumb";
import SessionManager from "../../../custom-classes/session-manager";
import { useHistory, useLocation, Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import Preview from "./preview/preview";
import ForumsList from "./forums/forums-list";
import ForumsCreate from "./forums/forums-create";
import ForumDetail from "../forum/forum-detail";
import LessonsList from "./lessons/lessons-list";
import LessonsCreate from "./lessons/lessons-create";
import QuizzesList from "./quizzes/quizzes-list";
import QuizzesCreate from "./quizzes/quizzes-create";
import AssignmentsList from "./assignments/assignments-list";
import AssignmentsCreate from "./assignments/assignments-create";
import OpenLessonsList from "./open-lessons/open-lessons-list";
import OpenLessonsCreate from "./open-lessons/open-lessons-create";
import IntegrationsList from "./integrations/integrations-list";
import IntegrationsCreate from "./integrations/integrations-create";
import ProjectsList from "./projects/projects-list";
import ProjectsCreate from "./projects/projects-create";
import EvaluationsList from "./evaluations/evaluations-list";
import EvaluationsCreate from "./evaluations/evaluations-create";
import AccommodationsList from "./accommodations/accommodations-list";
import AccommodationsCreate from "./accommodations/accommodations-create";
import MaterialsList from "./materials/materials-list";
import MaterialsCreate from "./materials/materials-create";
import StrategiesList from "./strategies/strategies-list";
import StrategiesCreate from "./strategies/strategies-create";
import ReflectionsList from "./reflections/reflections-list";
import ReflectionsCreate from "./reflections/reflections-create";
import DifferentiationStrategiesCreate from "./differentiation-strategies/differentiation-strategies-create";
import DifferentiationStrategiesList from "./differentiation-strategies/differentiation-strategies-list";
import ObservationsList from "./observations/observations-list";
import ObservationsCreate from "./observations/observations-create";
import CommonExperiencesList from "./common-experiences/common-experiences-list";
import CommonExperiencesCreate from "./common-experiences/common-experiences-create";
import ActiveResponsesList from "./active-responses/active-responses-list";
import ActiveResponsesCreate from "./active-responses/active-responses-create";
import { Translate, withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import Settings from "../../../custom-classes/settings";
import axios from "../../../api/interceptors";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from "../../../utils/access_logger";

const PlanningsDetail = ({ initialize, addTranslation }) => {
  const userDetails = SessionManager.getSessionDetails();

  const query = new URLSearchParams(useLocation().search);
  const page = query.get("page");
  const planning = SessionManager.getSelectedPlanning();
  const navigate = useHistory();
  const [disabled, setDisabled] = React.useState(false);

  const goBack = () => {
    navigate.push("/v2/plannings");
  };

  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {
    if (!planning) navigate.push("/dashboard");
  }, [planning]);

  useEffect(() => {
    if (!page) navigate.push("?page=lessons");
    try {
      SendAccessLog(
      {
        UserName: userDetails.Username,
        InvitationCode: userDetails.InvitationCode,
        Role: userDetails.Roles[0],
        AreaId: "3",
        PlanId: planning?.planningId ?? null,
        PlanName: planning?.planningName ?? null,
        Subject: planning?.subjectCode ?? null,
      })
    }
    catch (error){
      // empty catch block to avoid breaking the application
    }
  }, []);

  useEffect(() => {
    const fetchActiveSubscriptions = async () => {
      try {
        const endpointPlanning = Settings.getTeacherPlanningAPI();
        const responsePlanning = await axios.get(endpointPlanning);
        const dataPlanning = responsePlanning.status === 200 ? responsePlanning.data : [];
        const now = new Date();

        const datesPlanning = dataPlanning.filter(item =>
          {
            const to = new Date(item.CloseDate);
            return to >= now && item.PlanId === planning?.planningId;
          }) || [];

        const endpointSubsciptions = Settings.getSubscritionsAssociatedTeacherAPI();
        const responseSubsciptions = await axios.get(endpointSubsciptions);
        const dataSubsciptions = responseSubsciptions.status === 200 ? responseSubsciptions.data : [];

        const periods = dataSubsciptions.flatMap(item =>
            item.EntityAcademicDetailFullResponseModel?.AcademicPeriodModelList?.filter(p => {
              const to = new Date(p.PeriodTo);
              return to >= now;
            }) || []
        );


        if (dataPlanning.length === 0 || datesPlanning.length === 0 || periods.length === 0) {
          setDisabled(true);
        }
      } catch (error) {
        console.error("Failed to fetch active subscription:", error);
      }
    };

    fetchActiveSubscriptions();
  }, []);

  if (!planning) return null;

  return (
    <>

      <div className="container-padding animated delay-05s fadeIn">
        <div className="wrapper-principal-container pr-3">
          <div className="wrapper-col wrapper-col--flex-end mb-3 block-display-responsive">
            {/* <Border side="Bottom" className="pt-0">
              <Button
                variant="simple"
                icon={<FontAwesomeIcon icon={faArrowLeft} />}
                onClick={() => goBack()}
              >
                <Translate id="general.back-action" />
              </Button>
            </Border> */}

            <div className="col col-no-padding-left">
              <Link to="/v2/plannings" className="btn-back">
                <FontAwesomeIcon icon={faArrowLeft} />{' '}
                <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
              </Link>
            </div>

            <PlanningsDetailHeading planningId={planning.planningId} disabled={disabled} />
            <BreadcrumbProvider>
              {page && (
                <PlanningsMenu>
                  <div className="containerPlanning__content">
                    <Breadcrumb />
                    {page === "preview" && <Preview disabled={disabled} />}
                    {page === "lessons" && <LessonsList disabled={disabled} />}
                    {page === "lessons-create" && <LessonsCreate />}
                    {/* 
                    As agreed in the meeting on 08/05/2025, this menu area will be removed and unified into the planning lessons view.
                    Uncomment only if the separate lessons and quizzes views are required.
                    */}
                    {page === "quizzes" && <QuizzesList />}
                    {page === "quizzes-create" && <QuizzesCreate />}
                    {page === "forums" && <ForumsList disabled={disabled} />}
                    {page === "forums-create" && <ForumsCreate />}
                    {page === "forums-details" && <ForumDetail isEditable={false} />}
                    {page === "assignments" && <AssignmentsList disabled={disabled} />}
                    {page === "assignments-create" && <AssignmentsCreate disabled={disabled} />}
                    {page === "open-lessons" && <OpenLessonsList />}
                    {page === "open-lessons-create" && <OpenLessonsCreate />}                    
                    {page === "integrations" && <IntegrationsList />}
                    {page === "integrations-create" && <IntegrationsCreate />}
                    {page === "projects" && <ProjectsList />}
                    {page === "projects-create" && <ProjectsCreate />}
                    {page === "evaluations" && <EvaluationsList />}
                    {page === "evaluations-create" && <EvaluationsCreate />}
                    {page === "accommodations" && <AccommodationsList />}
                    {page === "accommodations-create" && (
                      <AccommodationsCreate />
                    )}
                    {page === "materials" && <MaterialsList />}
                    {page === "materials-create" && <MaterialsCreate />}
                    {page === "strategies" && <StrategiesList />}
                    {page === "strategies-create" && <StrategiesCreate />}
                    {page === "reflections" && <ReflectionsList />}
                    {page === "reflections-create" && <ReflectionsCreate />}
                    {page === "differentiation-strategies" && <DifferentiationStrategiesList />}
                    {page === "differentiation-strategies-create" && <DifferentiationStrategiesCreate />}
                    {page === "observations" && <ObservationsList />}
                    {page === "observations-create" && <ObservationsCreate />}
                    {page === "common-experiences" && <CommonExperiencesList />}
                    {page === "common-experiences-create" && <CommonExperiencesCreate />}
                    {page === "active-responses" && <ActiveResponsesList />}
                    {page === "active-responses-create" && <ActiveResponsesCreate />}
                  </div>
                </PlanningsMenu>
              )}
            </BreadcrumbProvider>
          </div>
        </div>
      </div>
    </>
  );
};

export default withLocalize(PlanningsDetail);
