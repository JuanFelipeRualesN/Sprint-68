import React from "react";
import Table from "../../shared/components/table";
import DifferentiationStrategiesTableRow from "./differentiation-strategies-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function DifferentiationStrategiesTable({
  differentiationStrategies = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("strategy.header-category"),
    translate("strategy.header-strategy"),
    translate("strategy.header-start-date"),
    translate("strategy.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={differentiationStrategies}
      headers={headers}
      noContentMessage={<Translate id="strategy.empty-message" />}
      renderRow={(differentiationStrategy, index) => {
        return (
          <DifferentiationStrategiesTableRow
            key={`tr-${index}`}
            differentiationStrategy={differentiationStrategy}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
