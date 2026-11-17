import React, { useState } from "react";
import ProjectsDetail from "./projects-detail";
import { formatWithDayName, getMinAndMaxDates } from "../../shared/utils";
import DeleteModal from "../../shared/components/delete-modal";
import EditModal from "../../shared/components/edit-modal";
import useDeleteProject from "../hooks/use-delete-project";
import useUpdateProject from "../hooks/use-update-project";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";

export default function ProjectsTableRow({ project, headers, onChange }) {
  const { minDate, maxDate } = getMinAndMaxDates(project.implementationDates);
  const { deleteProject, isLoading: isDeleting } = useDeleteProject();
  const { updateProject, isLoading: isUpdating } = useUpdateProject();
  const { planningId } = SessionManager.getSelectedPlanning();
  const [data, setData] = useState(project);

  const handleChange = (changes) => {
    setData((prev) => ({ ...prev, ...changes }));
  };

  return (
    <tr>
      <td data-th={headers[0]}>
        {project.title}
        {project.subtitle && (
          <div>
            <strong>
              <Translate id="general.other" />:{" "}
            </strong>
            {project.subtitle}
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
            updateProject({
              planningId,
              ...data,
            })
          }
          onCompleted={onChange}
          isLoading={isUpdating}
        >
          <ProjectsDetail project={project} onChange={handleChange} />
        </EditModal>
      </td>
      <td className="table-ppal--td-actions">
        <DeleteModal
          title={"project.modal-delete-title"}
          description={"project.modal-delete-description"}
          confirmButtonText={"general.modal-delete-confirmation"}
          cancelButtonText={"general.modal-delete-cancel"}
          onConfirmation={() => deleteProject(data.id)}
          onCompleted={onChange}
          isLoading={isDeleting}
        />
      </td>
    </tr>
  );
}
