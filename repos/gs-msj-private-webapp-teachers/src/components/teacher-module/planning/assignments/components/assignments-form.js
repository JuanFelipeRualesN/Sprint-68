import React, { useEffect, useState } from "react";
import AssignmentSection from "./assignments-form-section";
import Heading from "../../shared/components/heading";
import Button from "../../shared/components/button";
import useGetAssignments from "../hooks/useGetAssignments";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";

export default function AssignmentsForm({
  type = "add",
  planningId,
  assignment,
  assignmentList = [],
  isLoading,
  planningAssignments = [],
  setPlanningAssignments = () => {},
  onEditChange = (planningAssingment) => {},
}) {

  const planning = SessionManager.getSelectedPlanning();
  const [loading, setLoading] = useState(isLoading);
  const [editAssignment, setEditAssignment] = useState([]);
  

  useEffect(() => {
    if (type === "edit") setEditAssignment(assignment);
  }, []);

  const addAssignment = () => {
    setPlanningAssignments((prevAssignments) => {
      const updatePrevAssignments = Array.isArray(prevAssignments) ? prevAssignments : [];
      const ids = updatePrevAssignments.map(a => a.id || 0); // get all ids or default to 0
      const nextId = ids.length ? Math.max(...ids) + 1 : 1; //  by incrementing the current maximum, or start at 1 if none exist
      return [
        ...updatePrevAssignments,
        {
          id: nextId,
          PlanningId: planningId,
          TriesQuantity: "1",
          ForGrading: true,
          LangCode: "es",
        }
      ];
    });
  };

  const handleAssignmentChange = (id = 1, updatedFields) => {

    if (!updatedFields) return;
    if (type === "edit") {
      setEditAssignment({
        ...editAssignment,
        ...updatedFields,
      });
      return;
    }
    setPlanningAssignments(prevAssignments => {
      const arr = Array.isArray(prevAssignments) ? prevAssignments : [];
      return arr.map(assign =>
        assign.id === id ? { ...assign, ...updatedFields } : assign
      );
    });
  };

  const removeAssignment = (id) => {
    setPlanningAssignments(prevAssignments => {
      const arr = Array.isArray(prevAssignments) ? prevAssignments : [];
      return arr.filter(assign => assign.id !== id);
    });
  };

  useEffect(() => {
    onEditChange([editAssignment]);
  }, [editAssignment]);

  useEffect(() => {
    setLoading(isLoading);
  }, [isLoading]);

  
  const safePlanningAssignments = Array.isArray(planningAssignments) ? planningAssignments : [planningAssignments];

  // Check if the last assignment is valid
  const lastAssignment = safePlanningAssignments[safePlanningAssignments.length - 1];
  const isAssignmentValid = (assignment) => assignment.DueDate && assignment.QuizId;
  const canAddAssignment = !lastAssignment || isAssignmentValid(lastAssignment);

  const selectedQuizIds = safePlanningAssignments
  .map(a => a.QuizId)
  .filter(Boolean);

  const getAssignmentListFor = (currentQuizId) => {
    return assignmentList
      .filter(opt => !selectedQuizIds.includes(opt.Id) || opt.Id === currentQuizId)
      .map(opt => ({
        label: opt.AssignmentName,
        value: opt.Id,
      }));
  };

  return (
    <>
      <div className="contentForms mt-2">
        <Heading as="h3">
          <Translate id="assignment.activity-date-title" />
        </Heading>
      </div>
      {type === "add" ? (
        <>
          {safePlanningAssignments.map((assign) => (
            <AssignmentSection
              key={assign.id}
              assignment={assign}
              onDelete={() => removeAssignment(assign.id)}
              onChange={updatedFields => handleAssignmentChange(assign.id, updatedFields)}
              planningId={planningId}
              type={type}
              assignmentList={getAssignmentListFor(assign.QuizId)}
              openPlanningDate={planning?.openDate}
              closePlanningDate={planning?.closeDate}
              isLoading={loading}
            />
          ))}
          <Button
            variant="regular"
            onClick={addAssignment}
            disabled={isLoading || !canAddAssignment}
            className="mt-2"
          >
            <Translate id="planning.add-another-assignment-to-the-plan" />
          </Button>
        </>
      ) : (
        <AssignmentSection
          type={type}
          planningId={planningId}
          assignment={editAssignment}
          assignmentList={assignmentList}
          openPlanningDate={planning?.openDate}
          closePlanningDate={planning?.closeDate}
          isLoading={loading}
          onChange={updatedFields => handleAssignmentChange(assignment.QuizId, updatedFields)}
        />
      )}
    </>
  );
}
