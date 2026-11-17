import React, { useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import AssignmentsForm from "./components/assignments-form";
import useCreatePlanningAssignment from "./hooks/useCreatePlanningAssignment";
import useGetAssignments from "./hooks/useGetAssignments";
import Button from "../shared/components/button";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import SaveButton from "../shared/components/save-button";
import Border from "../shared/components/border";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import SessionManager from "../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";
import { translate } from "../shared/utils";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function AssignmentsCreate({disabled}) {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const {
    succeeded,
    isLoading,
    createPlanningAssignment,
  } = useCreatePlanningAssignment();
  const { planningId: id } = SessionManager.getSelectedPlanning();
  const { assignments, getAssignments } = useGetAssignments();
  const [completed, setCompleted] = useState([]);
  const [planningAssignments, setPlanningAssignments] = useState([
    {
      id: 1,
      PlanningId: id,
      TriesQuantity: "1",
      ForGrading: true,
      LangCode: "es",
    }  
  ]);

  const handleCreate = async () => {
    const responses = await Promise.all(
      planningAssignments.map((assignment) =>
        createPlanningAssignment(assignment)
      )
    );

    return responses.every((value) => value);
  };

  const isAssignmentValid = (assignment) => {
    return Boolean(assignment.DueDate && assignment.QuizId);
  };

  const isValid = planningAssignments.length > 0 && planningAssignments.every(isAssignmentValid);

  useEffect(() => {
    getAssignments();
  }, []);

  useEffect(() => {
    // Reset breadcrumb paths
    resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("assignment.title"),
      url: "?page=assignments",
    });
    setPath({ name: translate("general.create-new") });
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    setCompleted([...completed, succeeded]);
  }, [succeeded]);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="general.create-new" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="assignment.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=assignments")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>

        <SaveButton
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=assignments");
          }}
          disabled={!isValid || isLoading}
          isLoading={isLoading}
        />
      </Border>
      <AssignmentsForm
        isLoading={isLoading}
        planningId={id}
        planningAssignments={planningAssignments}
        setPlanningAssignments={setPlanningAssignments}
        assignmentList={assignments}
      />
    </>
  );
}
