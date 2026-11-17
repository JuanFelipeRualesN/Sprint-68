import React, { useState } from "react";
import ActiveResponsesDetail from "./active-responses-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteActiveResponse from "../hooks/use-delete-active-responses";
import useUpdateActiveResponse from "../hooks/use-update-active-responses";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from 'react-localize-redux';

export default function ActiveResponsesTableRow({ activeResponse, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(activeResponse.implementationDates);
  const { deleteActiveResponse, isLoading: isDeleting } = useDeleteActiveResponse();
  const { updateActiveResponse, isLoading: isUpdating } = useUpdateActiveResponse();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(activeResponse);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]} className="w-25">{activeResponse.title}</td>
      <td data-th={headers[1]} className="text-capitalize w-15">
        {activeResponse.includesStudentActiveResponse ? (<Translate id="general.yes" />) : (<Translate id="general.no" />)}
      </td>
      <td data-th={headers[2]} className="text-capitalize w-15">
        {activeResponse.includesLearningCheckActivity ? (<Translate id="general.yes" />) : (<Translate id="general.no" />)}
      </td>
      <td data-th={headers[3]} className="text-capitalize">
        {formatWithDayName(minDate)}
      </td>
      <td data-th={headers[4]} className="text-capitalize">
        {formatWithDayName(maxDate)}
      </td>
      <td className="table-ppal--td-actions">
        <EditModal
          editButtonText={"general.viewOrEdit"}
          title={"general.viewOrEdit"}
          confirmButtonText={"confirm-dialog.save"}
          cancelButtonText={"confirm-dialog.button-cancel"}
          onConfirmation={() =>
            updateActiveResponse({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <ActiveResponsesDetail activeResponse={activeResponse} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"active-response.modal-delete-title"}
          description={"active-response.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteActiveResponse(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
