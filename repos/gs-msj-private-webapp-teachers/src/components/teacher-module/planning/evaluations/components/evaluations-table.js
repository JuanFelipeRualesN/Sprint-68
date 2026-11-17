import React from "react";
import Table from "../../shared/components/table";
import EvaluationsTableRow from "./evaluations-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function EvaluationsTable({
  evaluations = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("evaluation.header-evaluation"),
    translate("evaluation.header-start-date"),
    translate("evaluation.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={evaluations}
      headers={headers}
      noContentMessage={<Translate id="evaluation.empty-message" />}
      renderRow={(evaluation, index) => {
        return (
          <EvaluationsTableRow
            key={`tr-${index}`}
            evaluation={evaluation}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
