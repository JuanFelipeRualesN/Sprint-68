import React from "react";
import Table from "../../shared/components/table";
import { translate } from "../../shared/utils";
import ForumsTableRow from "./forums-table-row";
import { Translate } from "react-localize-redux";

export default function ForumTable({ forums = [], isLoading, onChange, disabled }) {
  const headers = [
    translate("forum.header-forum-title"),
    translate("forum.header-start-date"),
    translate("forum.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={forums}
      headers={headers}
      noContentMessage={<Translate id="forum.empty-message" />}
      renderRow={(forum, index) => {
        return (
          <ForumsTableRow
              disabled={disabled}
            key={`tr-${index}`}
            forum={forum}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
