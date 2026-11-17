import React, { useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faArrowLeft,
    faEdit,
    faPlus,
    faExclamationCircle, faInfoCircle,
} from "@fortawesome/free-solid-svg-icons";
import "../view/style.css";
import Button from "./shared/components/button";
import Space from "./shared/components/space";
import PlanningsListFilters from "./plannings-list-filters";
import moment from "moment";
import useGetPlannings from "./shared/hooks/useGetPlannings";
import useDeletePlanning from "./shared/hooks/useDeletePlanning";
import Table from "./shared/components/table";
import { useHistory, Link } from "react-router-dom";
import { translate } from "./shared/utils";
import SessionManager from "../../../custom-classes/session-manager";
import {addTranslation, Translate, withLocalize} from "react-localize-redux";
import { renderToStaticMarkup } from "react-dom/server";
import LanguageManager from "../../../custom-classes/language-manager";
import DeleteModal from './shared/components/delete-modal';
import Alert from "react-bootstrap/Alert";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from "../../../utils/access_logger";

/**
 * The component (window) to handle the teacher plannings
 */
const PlanningsList = () => {
  const userDetails = SessionManager.getSessionDetails();
  const { plannings, isLoading, getPlannings } = useGetPlannings();
  const { deletePlanning, isLoading: isDeleting } = useDeletePlanning();
  const navigate = useHistory();

  // Initialize translations based on the current version
  useVersionedTranslations();

  const headers = [
    translate("planning.topic"),
    translate("planning.closure"),
    translate("planning.weekly"),
    translate("planning.subject"),
    translate("group-management.group-students"),
    translate("planning.view"),
  ];

  const formatOpenCloseDates = (openDate, closeDate, separator = "-") => {
    if (!openDate || !closeDate) return "";
    return `${moment.utc(openDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")} ${separator} ${moment.utc(closeDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")}`;
  };

  const selectPlanning = (planning) => {
    SessionManager.setSelectedPlanning(
      planning.id,
      planning.name,
      false,
      planning.openDate,
      planning.closeDate,
      planning.closure,
      planning.subjectAndGrade,
      planning.subjectCode,
      planning.levelCode
    );
    SessionManager.resetBreadcrumb();
  };

  useEffect(() => {
    getPlannings();
    try{        
      SendAccessLog({
          UserName: userDetails.Username,
          InvitationCode: userDetails.InvitationCode,
          Role: userDetails.Roles[0],
          AreaId: "3",
      })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
  }, []);

  const Row = ({ planning }) => {
    return (
      <tr>
        <td data-th={headers[0]}>{planning.name}
            {!planning.openDate || !planning.closeDate ? (
                <Alert className="alert-small-padding alert-custom">
                    <small>
                        <FontAwesomeIcon icon={faInfoCircle} />{' '}
                        <Translate id="planning.instructional-plans-not-compatible-with-planning" />
                    </small>
                </Alert>) : null
            }
        </td>
        <td className="strongTD" data-th={headers[1]}>
          {moment.utc(planning.closure).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")}
        </td>
        <td data-th={headers[2]}>
          {formatOpenCloseDates(planning.openDate, planning.closeDate)}
        </td>
        <td data-th={headers[3]}>{planning.subjectAndGrade}</td>
        <td data-th={headers[4]}>{planning.studentsGroups.join(' | ')}</td>
        <td className="table-ppal--td-actions" data-th={headers[5]}>
          <Button
            icon={<FontAwesomeIcon icon={faEdit} />}
            className="button-secondary-custom"
            color="none"
            variant="none"
            onClick={() => {
              selectPlanning(planning);
              navigate.push("/v2/plannings-detail?page=lessons");
            }}
            disabled={!planning.openDate || !planning.closeDate}
          >
            <Translate id="general.viewOrEdit" />
          </Button>
        </td>
        <td className="table-ppal--td-actions">
          <DeleteModal
            title={"planning.archive-question"}
            description={"planning.archive-note"}
            confirmButtonText={"general.modal-delete-confirmation"}
            cancelButtonText={"general.modal-delete-cancel"}
            onConfirmation={() => deletePlanning(planning.id)}
            onCompleted={() => getPlannings()}
            isLoading={isDeleting}
            disabled={!planning.openDate || !planning.closeDate}
          />
        </td>
      </tr>
    );
  };

  return (
    <>

      <div className="container-padding animated delay-05s fadeIn">
        <div className="wrapper-principal-container pr-3">
          {/* <div className="wrapper-col wrapper-col--flex-end mb-3 block-display-responsive">
            <Border side="Bottom" className="pt-0">
              <Button
                variant="simple"
                icon={<FontAwesomeIcon icon={faArrowLeft} />}
              >
                <Translate id="general.back-action" />
              </Button>
            </Border>
            <div className="col col-no-padding-left mt-3">
              <Heading as="h1">
                <Translate id="planning.planning-title" />
              </Heading>
            </div>
            <div className="col txt-right pr-0">
              <Button
                onClick={() => navigate.push("/v2/plannings-create")}
                variant="primary"
                icon={<FontAwesomeIcon icon={faPlusCircle} />}
              >
                <Translate id="planning.new" />
              </Button>
            </div>
          </div>

          <Alert message={<Translate id="planning.planning-closing-notes" />} />

          <Border side="Bottom">
            <Text type="regular">
              <Translate id="planning.message" />
            </Text>
          </Border> */}

          <div className="wrapper-col wrapper-col--flex-end borderGray-bottom mb-3 block-display-responsive">
            <div className="col col-no-padding-left">
                <Link to="/planning-select" className="btn-back">
                    <FontAwesomeIcon icon={faArrowLeft} />{' '}
                    <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                </Link>
                <div className="title-principal mt-3">
                    <Translate id="planning.planning-title" options={{ renderInnerHtml: true }} />
                </div>
            </div>
            <div className="col txt-right">
                <button onClick={() => navigate.push("/v2/plannings-create")} className="btn-default button-primary-custom">
                    <FontAwesomeIcon icon={faPlus} />{' '}
                    <Translate id="planning.new" options={{ renderInnerHtml: true }} />
                </button>
            </div>
          </div>

          <div className="section-description mb-3">
            <div className="wrapper-average-progress">
                <div className="wrapper-average-progress__note">
                    <small> <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />{" "}
                        <Translate id="planning.planning-closing-notes" options={{ renderInnerHtml: true }} />
                    </small>
                </div>
            </div>
            <small><Translate id="planning.planning-required-subtitle" options={{ renderInnerHtml: true }} /></small>
          </div>

          <PlanningsListFilters
            onChange={(filters) => {
              getPlannings(filters);
            }}
          />

          <Space variant="lg" />

          <Table
            isLoading={isLoading}
            content={plannings}
            headers={headers}
            noContentMessage={<Translate id="planning.plans-empty" />}
            renderRow={(planning, index) => {
              return <Row key={`tr-${index}`} planning={planning} />;
            }}
          />
        </div>
      </div>
    </>
  );
};

export default withLocalize(PlanningsList);
