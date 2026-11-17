import React from "react";
import Table from "../../shared/components/table";
import IntegrationsTableRow from "./integrations-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function IntegrationTable({
  integrations = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("integration.header-integration"),
    translate("integration.header-start-date"),
    translate("integration.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={integrations}
      headers={headers}
      noContentMessage={<Translate id="integration.empty-message" />}
      renderRow={(integration, index) => {
        return (
          <IntegrationsTableRow
            key={`tr-${index}`}
            integration={integration}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
