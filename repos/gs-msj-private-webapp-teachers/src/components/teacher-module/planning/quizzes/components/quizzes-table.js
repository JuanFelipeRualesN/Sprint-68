import React from "react";
import Table from "../../shared/components/table";
import QuizzesTableRow from "./quizzes-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function QuizzesTable({
  quizzes = [],
  isLoading = false,
  onChange,
}) {
  const headers = [
    translate("quiz.header-lesson"),
    translate("quiz.header-quiz"),
    translate("quiz.header-date"),
    translate("quiz.header-count-for-grade"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={quizzes}
      headers={headers}
      noContentMessage={<Translate id="quiz.empty-message" />}
      renderRow={(quiz, index) => {
        return (
          <QuizzesTableRow
            key={`tr-${index}`}
            quiz={quiz}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
