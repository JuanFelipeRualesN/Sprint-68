import React from "react";
import Table from "../../shared/components/table";
import ObservationsTableRow from "./observations-table-row";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";

export default function ObservationsTable({
  observations = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("observation.header-observation"),
    translate("observation.header-start-date"),
    translate("observation.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={observations}
      headers={headers}
      noContentMessage={<Translate id="observation.empty-message" />}
      renderRow={(observation, index) => {
        return (
          <ObservationsTableRow
            key={`tr-${index}`}
            observation={observation}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
