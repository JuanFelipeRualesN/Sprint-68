import React, { useState } from "react";
import AccommodationsDetail from "./accommodations-detail";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteAccommodation from "../hooks/use-delete-accommodation";
import useUpdateAccommodation from "../hooks/use-update-accommodation";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from 'react-localize-redux';

export default function AccommodationsTableRow({
  accommodation,
  headers,
  onChange,
}) {
  const { minDate, maxDate } = getMinAndMaxDates(
    accommodation.implementationDates
  );
  const {
    deleteAccommodation,
    isLoading: isDeleting,
  } = useDeleteAccommodation();
  const {
    updateAccommodation,
    isLoading: isUpdating,
  } = useUpdateAccommodation();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(accommodation);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]} className="w-50">
        {accommodation.title}
        {accommodation.subtitle && (
          <div>
            <strong>
              <Translate id="general.other" />:{" "}
            </strong>
            {accommodation.subtitle}
          </div>
        )}
      </td>
      <td data-th={headers[1]} className="text-capitalize">
        {formatWithDayName(minDate)}
      </td>
      <td data-th={headers[2]} className="text-capitalize">
        {formatWithDayName(maxDate)}
      </td>
      <td className="table-ppal--td-actions">
        <EditModal
          editButtonText={"general.viewOrEdit"}
          title={"general.viewOrEdit"}
          confirmButtonText={"confirm-dialog.save"}
          cancelButtonText={"confirm-dialog.button-cancel"}
          onConfirmation={() =>
            updateAccommodation({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <AccommodationsDetail
            accommodation={accommodation}
            onChange={handleChange}
          />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"accommodation.modal-delete-title"}
          description={"accommodation.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteAccommodation(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
