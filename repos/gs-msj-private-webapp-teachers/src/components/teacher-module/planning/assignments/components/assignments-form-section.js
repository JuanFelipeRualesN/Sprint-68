import React, { useEffect, useState } from "react";
import moment from "moment";
import AssignmentPreview from "./assignments-preview";
import useGetAssignmentDetails from "../hooks/useGetAssignmentDetails";
import FormDatePicker from "../../shared/components/form/form-date-picker";
import FormSelect from "../../shared/components/form/form-select";
import Button from "../../shared/components/button";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt, faEye } from "@fortawesome/free-solid-svg-icons";
import ModalExamPreview from "../../../modal/modal-exam-preview";

export default function AssignmentSection({
  planningId,
  type,
  assignment,
  assignmentList = [],
  openPlanningDate,
  closePlanningDate,
  isLoading,
  onChange,
  onDelete,
}) {
  const [currentOpenPlanningDate, setCurrentOpenPlanningDate] = useState();
  const [currentClosePlanningDate, setCurrentClosePlanningDate] = useState();
  const [openPreview, setOpenPreview] = useState(false);
  const [loading, setLoading] = useState(isLoading);
  const {
    assigmentDetail,
    isLoading: isLoadingAssignmentDetail,
    getAssignmentDetail,
  } = useGetAssignmentDetails();

  const handleFieldChange = (name, value) => {
    if (assignment?.[name] === value) return;

    // if (name === "QuizId" && value !== undefined) {
    //   getAssignmentDetail(value);
    // }
    onChange({ [name]: value });
  };

  const handleOnDelete = () => {
    if (onDelete) {
      onDelete();
    }
  };

  const toggleModalPreview = () => {
    setOpenPreview(!openPreview);
  }

  useEffect(() => {
    setCurrentOpenPlanningDate(openPlanningDate);
    setCurrentClosePlanningDate(closePlanningDate);
  }, [openPlanningDate, closePlanningDate]);

  useEffect(() => {
    setLoading(isLoading);
  }, [isLoading]);

  // useEffect(() => {
  //   if (assignment?.QuizId) {
  //     getAssignmentDetail(assignment.QuizId);
  //   }
  // }, [assignment?.QuizId]);

  return (
    <div className="wrapper-form-group">
      <div className="wrapperFlex contentSize-md contentForms-inline">
        <div className="col pl-0">
          <FormDatePicker
            name="startDate"
            label={<Translate id="assignment.header-due-date" />}
            value={
              assignment?.DueDate
                ? moment(assignment?.DueDate).toDate()
                : undefined
            }
            onChange={(values) =>
              handleFieldChange("DueDate", `${moment(values).format("YYYY-MM-DD")} 00:00:00`)
            }
            minDate={
              currentOpenPlanningDate
                ? moment(currentOpenPlanningDate).toDate()
                : undefined
            }
            maxDate={
              currentClosePlanningDate
                ? moment(currentClosePlanningDate).toDate()
                : undefined
            }
            disabled={loading}
          />
        </div>

        <div className="col">
          <FormSelect
            name="assignment"
            label={<Translate id="assignment.assignment" />}
            placeholder={translate("general.select")}
            options={assignmentList}
            onChange={(value) => handleFieldChange("QuizId", value ?? "")}
            value={assignment?.QuizId}
            disabled={type === "edit" || loading}
          />
        </div>
        {type === "add" ? (
          <div className="col">
            <div className="form-group-fields">
              {/** This modal preview is not compatible with tables */ }
              {/* <AssignmentPreview
                loading={isLoading || isLoadingAssignmentDetail}
                disabled={assignment?.QuizId === undefined}
                quiz={assigmentDetail}
              /> */}

              <Button
                variant="simple"
                icon={<FontAwesomeIcon icon={faEye} />}
                onClick={toggleModalPreview}
                disabled={assignment?.QuizId === undefined}
              />
              <Button
                icon={<FontAwesomeIcon icon={faTrashAlt} />}
                onClick={handleOnDelete}
              />
            </div>
          </div>
        ) : null}
      </div>
      <div className="col pl-0">
        <FormCheckbox
          name="ForGrading"
          label={<Translate id="assignment.for-grading" />}
          checked={
            assignment?.ForGrading === undefined
              ? true
              : assignment?.ForGrading
          }
          onChange={(value) => handleFieldChange("ForGrading", value)}
          disabled={loading}
        />
      </div>
      {/** Modal functionality is the same as the assignment modal */ }
      {openPreview && <ModalExamPreview
        quizId={assignment?.QuizId}
        visible={openPreview}
        preview={true}
        updateParent={toggleModalPreview}
      />}
    </div>
  );
}
