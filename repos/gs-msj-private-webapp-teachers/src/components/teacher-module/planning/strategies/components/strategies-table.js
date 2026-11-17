import React from "react";
import Table from "../../shared/components/table";
import StrategiesTableRow from "./strategies-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function StrategiesTable({
  strategies = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("accommodation.header-category"),
    translate("accommodation.header-accommodation"),
    translate("accommodation.header-start-date"),
    translate("accommodation.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={strategies}
      headers={headers}
      noContentMessage={<Translate id="accommodation.empty-message" />}
      renderRow={(strategy, index) => {
        return (
          <StrategiesTableRow
            key={`tr-${index}`}
            strategy={strategy}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
