import React from "react";
import Table from "../../shared/components/table";
import ReflectionsTableRow from "./reflections-table-row";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";

export default function ReflectionsTable({
  reflections = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("reflection.header-reflection"),
    translate("reflection.header-start-date"),
    translate("reflection.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={reflections}
      headers={headers}
      noContentMessage={<Translate id="reflection.empty-message" />}
      renderRow={(reflection, index) => {
        return (
          <ReflectionsTableRow
            key={`tr-${index}`}
            reflection={reflection}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
