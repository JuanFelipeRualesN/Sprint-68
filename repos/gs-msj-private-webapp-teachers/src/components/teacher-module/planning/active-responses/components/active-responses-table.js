import React from "react";
import Table from "../../shared/components/table";
import ActiveResponsesTableRow from "./active-responses-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function ActiveResponsesTable({
  activeResponses = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("active-response.header-active-response"),
    translate("active-response.header-includes-active"),
    translate("active-response.header-includes-check"),
    translate("active-response.header-start-date"),
    translate("active-response.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={activeResponses}
      headers={headers}
      noContentMessage={<Translate id="active-response.empty-message" />}
      renderRow={(activeResponse, index) => {
        return (
          <ActiveResponsesTableRow
            key={`tr-${index}`}
            activeResponse={activeResponse}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
