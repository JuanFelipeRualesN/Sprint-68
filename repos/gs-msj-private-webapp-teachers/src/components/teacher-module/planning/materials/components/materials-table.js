import React from "react";
import Table from "../../shared/components/table";
import MaterialsTableRow from "./materials-table-row";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";

export default function MaterialsTable({
  materials = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("material.header-material"),
    translate("material.header-start-date"),
    translate("material.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={materials}
      headers={headers}
      noContentMessage={<Translate id="material.empty-message" />}
      renderRow={(material, index) => {
        return (
          <MaterialsTableRow
            key={`tr-${index}`}
            material={material}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
