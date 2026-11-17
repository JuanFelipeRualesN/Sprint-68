import React, { useState } from "react";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import DateSelections from "../../shared/components/date-selections";
import FormField from "../../shared/components/form/form-field";
import SessionManager from "../../../../../custom-classes/session-manager";
import { Translate } from "react-localize-redux";
import moment from "moment";
import OpenLessonsItem from "./open-lessons-item";
import OpenLessonsTopicItem from "./open-lessons-topic-item";
import OpenLessonsItemWithSuggestions from "../../../chatbot/plannings-suggestions/open-lessons-item-suggestion";

export default function OpenLessonsDetail({ lesson, onChange = () => {}, suggestionsEnabled = false, planningContext = null }) {
  const planning = SessionManager.getSelectedPlanning();
  const [lessonState, setLessonState] = useState(lesson);

  const handleChange = (name, value) => {
    const newState = { ...lessonState, [name]: value };
    setLessonState(newState);
    onChange(newState);
  };

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={lessonState.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="lesson.content" />
      </Heading>

      <Space variant="regular" />

      <div className="wrapperFlex FlexAlingEnd w-50 mb-2">
        <div className="col title px-0">

          {suggestionsEnabled && planningContext ? (
            <OpenLessonsItemWithSuggestions
              name="title"
              fieldType="lesson_title"
              label={<Translate id="lesson.form-title" />}
              value={lessonState.title}
              handleChange={handleChange}
              planningContext={planningContext}
              isSingleField={true}
            />
          ) : (
            <FormField
              name="title"
              label={<Translate id="lesson.form-title" />}
              value={lessonState.title}
              onChange={(event) => handleChange("title", event.target.value)}
            />
          )}
        </div>
      </div>

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="standards"
          fieldType="standards"
          values={lessonState?.standards}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="standards"
          values={lessonState?.standards}
          handleChange={handleChange}
        />
      )}

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="expectations"
          fieldType="expectations"
          values={lessonState?.expectations}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="expectations"
          values={lessonState?.expectations}
          handleChange={handleChange}
        />
      )}

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="strategies"
          fieldType="strategies"
          values={lessonState?.strategies}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="strategies"
          values={lessonState?.strategies}
          handleChange={handleChange}
        />
      )}

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="objectives"
          fieldType="objectives"
          values={lessonState?.objectives}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="objectives"
          values={lessonState?.goals}
          handleChange={handleChange}
        />
      )}

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="units"
          fieldType="unit"
          values={lessonState?.units}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="units"
          values={lessonState?.units}
          handleChange={handleChange}
        />
      )}

      <OpenLessonsTopicItem name="topics" values={lessonState?.topics} handleChange={handleChange} />

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="initial"
          fieldType="initial"
          values={lessonState?.initial}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="initial"
          values={lessonState?.initial}
          handleChange={handleChange}
        />
      )}

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="development"
          fieldType="development"
          values={lessonState?.development}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="development"
          values={lessonState?.development}
          handleChange={handleChange}
        />
      )}

      {suggestionsEnabled && planningContext ? (
        <OpenLessonsItemWithSuggestions
          name="close"
          fieldType="close"
          values={lessonState?.close}
          handleChange={handleChange}
          planningContext={planningContext}
        />
      ) : (
        <OpenLessonsItem
          name="close"
          values={lessonState?.close}
          handleChange={handleChange}
        />
      )}
    </>
  );
}
