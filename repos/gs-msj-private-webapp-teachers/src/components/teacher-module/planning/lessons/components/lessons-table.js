import { useEffect } from "react";
import Table from "../../shared/components/table";
import { translate } from "../../shared/utils";
import LessonsTableRow from "./lessons-table-row";
import { Translate } from "react-localize-redux";

export default function LessonsTable({ lessons = [], isLoading, onChange, disabled = false }) {
  const headers = [
    translate("lesson.title"),
    translate("lesson.header-start-date"),
    translate("lesson.header-end-date"),
      translate("assignment.header-for-grade"),
    translate("lesson.header-available-date"),
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
          <LessonsTableRow
              disabled={disabled}
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
