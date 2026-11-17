import React from "react";
import Table from "../../shared/components/table";
import AccommodationsTableRow from "./accommodations-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function AccommodationsTable({
  accommodations = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("strategy.header-strategy"),
    translate("strategy.header-start-date"),
    translate("strategy.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={accommodations}
      headers={headers}
      noContentMessage={<Translate id="strategy.empty-message" />}
      renderRow={(accommodation, index) => {
        return (
          <AccommodationsTableRow
            key={`tr-${index}`}
            accommodation={accommodation}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
