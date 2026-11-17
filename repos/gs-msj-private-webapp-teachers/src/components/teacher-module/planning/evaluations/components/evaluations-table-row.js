import React, { useState } from "react";
import EvaluationsDetail from "./evaluations-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteEvaluation from "../hooks/use-delete-evaluation";
import useUpdateEvaluation from "../hooks/use-update-evaluation";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";

export default function EvaluationsTableRow({ evaluation, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(
    evaluation.implementationDates
  );
  const { deleteEvaluation, isLoading: isDeleting } = useDeleteEvaluation();
  const { updateEvaluation, isLoading: isUpdating } = useUpdateEvaluation();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(evaluation);
  const [disableSave, setDisableSave] = useState(false);

  const handleChange = (changes) => {
      // Validate if implementationDates are null. If null or empty, set disableSave to true
      // Also, these should be at least one category.
        const hasImplementationDates = changes?.implementationDates?.length > 0;
       // Evaluate if has at least one diagnostic, formative or summative
        const hasAtLeastOneCategory =
            changes?.diagnostic?.defaults?.length > 0 ||
            changes?.diagnostic?.customs?.length > 0 ||
            changes?.formative?.defaults?.length > 0 ||
            changes?.formative?.customs?.length > 0 ||
            changes?.summative?.defaults?.length > 0 ||
            changes?.summative?.customs?.length > 0;


        setDisableSave(!hasImplementationDates || !hasAtLeastOneCategory);

    setData((prev) => ({ ...prev, ...changes }));
  };



  return (
    <tr className="align-baseline">
      <td data-th={headers[0]} className="w-50">
        {evaluation.categories?.map((category, index) => {
          return (
            <div key={`category-${index}`} className="mb-2">
              <span>
                <strong>{category.name}:</strong>{" "}
                {category.title}
                {category.subtitle && (
                  <div>
                    <strong>
                      <Translate id="general.other" />:{" "}
                    </strong>
                    {category.subtitle}
                  </div>
                )}
              </span>
            </div>
          );
        })}
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
            updateEvaluation({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
          disableSaveButton={disableSave}
        >
          <EvaluationsDetail
            evaluation={evaluation}
            onChange={(e) => handleChange(e)}
          />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"evaluation.modal-delete-title"}
          description={"evaluation.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteEvaluation(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
