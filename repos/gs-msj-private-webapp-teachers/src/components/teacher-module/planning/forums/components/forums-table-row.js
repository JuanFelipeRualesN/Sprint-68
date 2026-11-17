import React from "react";
import ForumsDetail from "./forums-detail";
import EditModal from "../../shared/components/edit-modal";
import DeleteModal from "../../shared/components/delete-modal";
import useDeleteForum from "../hooks/useDeleteForum";
import useUpdateForum from "../hooks/useUpdateForum";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import { useHistory } from "react-router-dom";
import Button from "../../shared/components/button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Translate } from "react-localize-redux";
import { faEye } from "@fortawesome/free-solid-svg-icons";
import SessionManager from "../../../../../custom-classes/session-manager";

export default function ForumsTableRow({ forum, headers, onChange, disabled }) {
  const { minDate, maxDate } = getMinAndMaxDates(forum.implementationDates);
  const { deleteForum, isLoading: isDeleting } = useDeleteForum();
  const { updateForum, isLoading: isUpdating } = useUpdateForum();
  const navigate = useHistory();
  const { planningId } = SessionManager.getSelectedPlanning();

  return (
    <tr>
      <td data-th={headers[0]}>{forum.title}</td>
      <td data-th={headers[1]} className="text-capitalize">
        {formatWithDayName(minDate)}
      </td>
      <td data-th={headers[2]} className="text-capitalize">
        {formatWithDayName(maxDate)}
      </td>
      <td className="table-ppal--td-actions">
        <Button
          className="mr-2"
          onClick={() => navigate.push(`?page=forums-details&forumId=${forum.id}&teacherPlanId=${planningId}`)}
          variant="secondary"
          icon={<FontAwesomeIcon icon={faEye} />}
        >
          <Translate id="general.view" />
        </Button>
        <EditModal
            disabled={disabled}
          editButtonText={"general.edit"}
          title={"general.viewOrEdit"}
          confirmButtonText={"confirm-dialog.save"}
          cancelButtonText={"confirm-dialog.button-cancel"}
          onConfirmation={() => updateForum(forum)}
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <ForumsDetail
            forum={forum}
            onChange={(changes) => (forum = changes)}
          />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
            disabled={disabled}
          title="forum.modal-delete-title"
          description="forum.modal-delete-description"
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteForum(forum.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
