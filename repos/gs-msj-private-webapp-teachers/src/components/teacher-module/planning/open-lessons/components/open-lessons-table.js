import React from "react";
import Table from "../../shared/components/table";
import OpenLessonsTableRow from "./open-lessons-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function OpenLessonTable({
  lessons = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("lesson.title"),
    translate("lesson.header-start-date"),
    translate("lesson.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={lessons}
      headers={headers}
      noContentMessage={<Translate id="lesson.empty-message" />}
      renderRow={(lesson, index) => {
        return (
          <OpenLessonsTableRow
            key={`tr-${index}`}
            lesson={lesson}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
