import React from "react";
import Table from "../../shared/components/table";
import ProjectsTableRow from "./projects-table-row";
import { translate } from "../../shared/utils";
import { Translate } from "react-localize-redux";

export default function ProjectsTable({ projects = [], isLoading, onChange }) {
  const headers = [
    translate("project.header-project"),
    translate("project.header-start-date"),
    translate("project.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={projects}
      headers={headers}
      noContentMessage={<Translate id="project.empty-message" />}
      renderRow={(project, index) => {
        return (
          <ProjectsTableRow
            key={`tr-${index}`}
            project={project}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
