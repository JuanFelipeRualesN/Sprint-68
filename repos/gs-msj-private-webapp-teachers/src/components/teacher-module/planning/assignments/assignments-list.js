import React, { useState, useContext, useEffect } from "react";
import { useHistory } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AssignmentsForm from "./components/assignments-form";
import moment from "moment";
import Button from "../shared/components/button";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Table from "../shared/components/table";
import DeleteModal from "../shared/components/delete-modal";
import EditModal from "../shared/components/edit-modal";
import FormCheckbox from "../shared/components/form/form-checkbox";
import Border from "../shared/components/border";
import Space from "../shared/components/space";
import PaginationCustom from "../../../shared-module-components/datatables/pagination";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { translate, formatWithDayName } from "../shared/utils";
import { Translate } from "react-localize-redux";
import useGetPlanningAssignments from "./hooks/useGetPlanningAssignments";
import useDeletePlanningAssignment from "./hooks/useDeletePlanningAssignment";
import useModifyPlanningAssignment from "./hooks/useModifyPlanningAssignment";
import useGetAssignments from "./hooks/useGetAssignments";
import SessionManager from "../../../../custom-classes/session-manager";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function AssignmentsList() {
  const navigate = useHistory();
  const {
    planningAssignments,
    isLoading,
    getPlanningAssignments,
  } = useGetPlanningAssignments();
  const {
    succeeded,
    isLoading: isLoadingDelete,
    deletePlanningAssignment,
  } = useDeletePlanningAssignment();
  const {
    succeeded: isModified,
    isLoading: isLoadingModify,
    modifyPlanningAssignment,
  } = useModifyPlanningAssignment();
  const headers = [
    translate("assignment.header-assignments"),
    translate("assignment.header-due-date"),
    translate("assignment.header-for-grade"),
    "",
  ];
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { planningId } = SessionManager.getSelectedPlanning();
  const { assignments: assignmentList, getAssignments } = useGetAssignments();
  let editAssignment;

  const assignmentsLimiter = 10;
  const [currentPage, setCurrentPage] = useState(0);
  const [totalItems, setTotalItems] = useState(null);

  useEffect(() => {
    getPlanningAssignments(planningId, assignmentsLimiter, 0);
    getAssignments();
  }, []);

  useEffect(() => {
    resetPaths();
    setPath({
      root: translate("planning.menu-genial"),
      name: translate("assignment.title"),
      url: "?page=assignments",
    });
  }, [LanguageManager.getLanguage()]);

  const handleOnModelChangeEdit = (planningAssignments) => {
    editAssignment = planningAssignments[0];
  };

  useEffect(() => {
    if (typeof planningAssignments.TotalAssignments === "number") {
      setTotalItems(planningAssignments.TotalAssignments);
    }
  }, [planningAssignments.TotalAssignments]);

  const handleDeleteAssignment = async (assignmentId) => {
    return await deletePlanningAssignment(parseInt(planningId), assignmentId);
  };

  const handleEditAssignment = async () => {
    return await modifyPlanningAssignment(editAssignment);
  };

  const handlePageChange = (page) => {
    const newPageIndex = page - 1;
    setCurrentPage(newPageIndex);
    getPlanningAssignments(planningId, assignmentsLimiter, newPageIndex);
  }

  useEffect(() => {
    if (succeeded || isModified) {
      getPlanningAssignments(planningId);
    }
  }, [succeeded, isModified]);

  const Row = ({ assignment }) => {
    assignment.DueDate = moment(assignment.DueDate).format("YYYY-MM-DD");
    return (
      <tr>
        <td data-th={headers[0]}>{assignment.AssignmentName}</td>
        <td data-th={headers[1]}>{formatWithDayName(assignment.DueDate)}</td>
        <td data-th={headers[2]}>
          <FormCheckbox
            name=""
            checked={assignment.ForGrading}
            disabled={true}
          />
        </td>
        <td className="table-ppal--td-actions">
          <EditModal
            editButtonText={"general.viewOrEdit"}
            title={"general.viewOrEdit"}
            confirmButtonText={"confirm-dialog.save"}
            cancelButtonText={"confirm-dialog.button-cancel"}
            maxWidth="md"
            onConfirmation={() => handleEditAssignment()}
            className="scroll-hidden"
          >
            <AssignmentsForm
              type="edit"
              planningId={planningId}
              assignment={assignment}
              assignmentList={assignmentList.map(opt => ({
                label: opt.AssignmentName,
                value: opt.Id,
              }))}
              onEditChange={handleOnModelChangeEdit}
            />
          </EditModal>
        </td>
        <td className="table-ppal--td-actions">
          <DeleteModal
            title="assignment.modal-delete-title"
            description="assignment.modal-delete-description"
            confirmButtonText={"general.modal-delete-confirmation"}
            cancelButtonText={"general.modal-delete-cancel"}
            onConfirmation={() => handleDeleteAssignment(assignment.Id)}
            isLoading={isLoadingDelete}
          />
        </td>
      </tr>
    );
  };

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="assignment.title" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="assignment.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          variant="secondary"
          onClick={() => navigate.push("?page=assignments-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <Table
        isLoading={isLoading || isLoadingModify}
        content={planningAssignments.Assignments}
        headers={headers}
        noContentMessage={<Translate id="assignment.empty-message" />}
        renderRow={(assignment, index) => {
          return <Row key={`tr-${index}`} assignment={assignment} />;
        }}
      />
      <PaginationCustom
        currentPage={currentPage + 1}
        totalPages={Math.ceil(totalItems / assignmentsLimiter)}
        updateParent={handlePageChange}
      />
    </>
  );
}
