import React, { useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import useGetPlanningDetails from "./shared/hooks/useGetPlanningDetails";
import moment from "moment";
import PlanningsTitle from "./plannings-title";
import Heading from "./shared/components/heading";
import Border from "./shared/components/border";
import Text from "./shared/components/text";
import FormLabelValue from "./shared/components/form/form-label-value";
import { translate } from "./shared/utils";
import PlanningsEdit from "./plannings-edit";
import { Translate } from "react-localize-redux";
import { faClock, faExclamationCircle } from "@fortawesome/free-solid-svg-icons";
import LanguageManager from "../../../custom-classes/language-manager";
import {Alert as AlertBootstrap} from "reactstrap";

moment.locale("es");

export default function PlanningDetailHeading({ planningId, disabled }) {
  const { planning, getPlanning } = useGetPlanningDetails();

  const formatOpenCloseDates = (openDate, closeDate, separator = "-") => {
    if (!openDate || !closeDate) return "";

    return `${moment.utc(openDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")} ${separator} ${moment.utc(closeDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")}`;
  };

  const formatAcademicYear = (openDate, closeDate) => {
    if (!openDate || !closeDate) return "";

    return `${moment.utc(openDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")} - ${moment.utc(closeDate).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")}`;
  };

  const getPlanningTypeLabel = (number, timeframe) => {
    return translate(`planning.timeframe-${timeframe || "0"}`).replace(
      "{number}",
      `${number || "0"}`
    );
  };

  const formatStudentGroups = (planningGroups) => {
    const uniqueGroups = Object.values(
      (planningGroups ?? []).reduce((acc, item) => {
        acc[item.GroupId] = item;
        return acc;
      }, {})
    )
    if (uniqueGroups && uniqueGroups.length > 0) {
      return uniqueGroups?.map((x) => x.GroupName).join(", ");
    } else {
      return "";
    }
  };

  useEffect(() => {
    getPlanning(planningId);
  }, []);

  return (
    <>
      <div className="wrapper-twoCol-highlight mb-0 mt-3 d-flex">
        <div className="col w-50 pl-0">
          <Heading as="h1" className="d-flex align-items-center ">
            {planning?.name && (
              <PlanningsTitle
                planningId={planningId}
                name={planning?.name}
                onChange={(title) => {
                  planning.name = title;
                }}
              />
            )}
          </Heading>
              <AlertBootstrap
                  color="danger"
                  isOpen={disabled}
                  className="alert-info"
              >
                <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />
                {' '}
                <span> <Translate id="planning.no-active-periods-limited-access" options={{ renderInnerHtml: true }} /></span>
              </AlertBootstrap>
        </div>
        <div className="d-flex align-items-center">
          <PlanningsEdit
              disabled={disabled}
            planning={planning}
            onCompleted={() => window.location.reload()}
          />
        </div>
      </div>

      {/* <Alert
        className="m-0"
        message={<Translate id="planning.planning-closing-notes" />}
      /> */}

      <div className="wrapper-average-progress">
        <div className="wrapper-average-progress__note">
          <small> <FontAwesomeIcon icon={faExclamationCircle} className="dt-icons-size" />{" "}
            <Translate id="planning.planning-closing-notes" options={{ renderInnerHtml: true }} />
          </small>
        </div>
      </div>

      <Border side="Bottom">
        <div className="form-label-value-group">
          <FormLabelValue
            label={getPlanningTypeLabel(
              planning.weekNumber,
              planning.timeframe
            )}
            value={formatOpenCloseDates(
              planning.openDate,
              planning.closeDate
            )}
          />
          <FormLabelValue
            label={translate("group-management.group-students")}
            value={formatStudentGroups(planning.planningGroups)}
          />
          <FormLabelValue
            label={translate("planning.subject-grade")}
            value={planning.subjectAndGrade}
          />
        </div>
        <div className="form-label-value-group">
          <FormLabelValue
            label={translate("planning.academic-period")}
            value={`${planning.periodNumber || ""}: ${formatOpenCloseDates(
              planning.periodFrom,
              planning.periodTo
            )}`}
          />
          <FormLabelValue
            label={translate("planning.academic-year")}
            value={formatAcademicYear(
              planning.academic?.fromDate,
              planning.academic?.toDate
            )}
          />
        </div>
      </Border>

      <Border side="Bottom">
        <Text type="regular">
          <span className="space-margin-left-sm">
            <FontAwesomeIcon className="color-esmerald" icon={faClock} />
            <span className="space-margin-left-sm">
              <Translate id="planning.academic-period-close" />:
            </span>{" "}
            <strong>
              {moment.utc(planning.closure).locale(LanguageManager.getLanguage()).format("MMMM DD, YYYY")}
            </strong>
          </span>
        </Text>
      </Border>
    </>
  );
}
