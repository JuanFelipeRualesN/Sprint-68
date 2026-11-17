import React, { useEffect, useState } from "react";
import useCreateQuiz from "../hooks/useCreateQuiz";
import moment from "moment";
import QuizzesPreview from "./quizzes-preview";
import Border from "../../shared/components/border";
import Heading from "../../shared/components/heading";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import FormDatePicker from "../../shared/components/form/form-date-picker";
import FormSelect from "../../shared/components/form/form-select";
import FormField from "../../shared/components/form/form-field";
import { addDaysToDate, compareLocalAndUTC, translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from '../../../../../custom-classes/session-manager';
import ModalViewQuiz from "../../../modal/modal-view-quiz";
import Button from "../../shared/components/button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEye } from '@fortawesome/free-solid-svg-icons';

export default function QuizzesDetail({
  quiz,
  onChange = () => {},
  isForEdit = false,
}) {
  const [isActive, setIsActive] = useState(true);
  const { getLists, times, timezones } = useCreateQuiz();
  const [quizState, setQuizState] = useState(quiz);
  const { closure } = SessionManager.getSelectedPlanning();

    const [showModalViewQuiz, setShowModalViewQuiz] = useState(false);

  const handleChange = (name, value) => {
    const newState = { ...quizState, [name]: value };
    setQuizState(newState);
    onChange(newState);
  };

  const getMinDate = () => {
    const daysToDate = compareLocalAndUTC();
    return addDaysToDate(quizState.lessonDueDate, daysToDate);
  };

  const defaultDate = quizState.date ? new Date(quizState.date) : getMinDate();
  const defaultTime =
    quizState.time ??
    (isForEdit ? moment(defaultDate).format("hh:mm A") : "10:00 AM");

  // const handleCheckChanges = (checked) => {
  //   setIsActive(checked);
  //   handleChange('isActive', checked);
  // };

  useEffect(() => {
    getLists();
    const newState = {
      ...quizState,
      time: quizState.time ?? defaultTime,
      date: quizState.date ?? defaultDate,
      duration: quizState.duration ?? 60,
      timezone: quizState.timezone ?? (timezones && timezones[0]?.value),
    };
    setQuizState(newState);
    onChange(newState);
  }, []);

  useEffect(() => {
    if (isForEdit || !timezones || timezones.length <= 0) return;

    handleChange("timezone", timezones[0].value);
  }, [timezones]);

  const minDate = new Date(getMinDate());
  const maxDate = moment(closure).toDate();
  const validValue = defaultDate ? new Date(defaultDate) : new Date();

  return (
    <>
      <Border side="Bottom">
        <>
          <Heading as="h4">{quizState.name}</Heading>
          {/* // In MSJ, it is required that all quizzes be added to the lesson, uncomment this line to enable the checkbox
          <FormCheckbox
            name="prueba1"
            checked={isActive}
            disabled={true} // remove this line to enable the checkbox
            onChange={handleCheckChanges}
            label={<Translate id="quiz.check-lesson" />}
          /> */}
          {isActive && (
            <>
              <div className="wrapperFlex FlexAlingEnd mb-2">
                <div className="col date pl-0">
                  <FormDatePicker
                    name="date"
                    label={<Translate id="quiz.header-date" />}
                    minDate={minDate <= maxDate ? minDate : maxDate}
                    maxDate={maxDate <= minDate ? minDate : maxDate}
                    value={validValue}
                    onChange={(value) => handleChange("date", value)}
                  />
                </div>
                <div className="col time">
                  <FormSelect
                    name="time"
                    label={<Translate id="quiz.time" />}
                    placeholder={translate("general.select")}
                    options={times}
                    value={defaultTime}
                    onChange={(value) => handleChange("time", value)}
                    searchable
                  />
                </div>
                <div className="col timezone">
                  <FormSelect
                    name="timezone"
                    label={<Translate id="quiz.timezone" />}
                    placeholder={translate("general.select")}
                    options={timezones}
                    value={quizState.timezone}
                    clearable={false}
                    onChange={(value) => handleChange("timezone", value)}
                  />
                </div>
                <div className="col duration">
                  <FormField
                    name="duration"
                    label={<Translate id="quiz.duration" />}
                    placeholder={translate("general.select")}
                    value={quiz.duration}
                    onChange={(event) =>
                      handleChange("duration", event.target.value)
                    }
                  />
                </div>
                <div className="col">
                  <Button
                    className="pl-2"
                    variant="simple"
                    icon={<FontAwesomeIcon icon={faEye} />}
                    onClick={() => setShowModalViewQuiz(true)}
                  />
                </div>
              </div>

              {/* <FormCheckbox
                className="count-for-grade"
                name={`${quizState.id}-countForGradeMessage`}
                label={<Translate id="quiz.count-for-grade-message" />}
                checked={quizState.countForGrade}
                onChange={(value) => handleChange("countForGrade", value)}
              /> */}
            </>
          )}
        </>
      </Border>
      {showModalViewQuiz ?
          <ModalViewQuiz visible={showModalViewQuiz} lessonId={quiz.lessonId} onClose={setShowModalViewQuiz} quizId={quiz.quizId}  /> : null
      }
    </>
  );
}
