import React from "react";
import Table from "../../shared/components/table";
import CommonExperienceTableRow from "./common-experiences-table-row";
import { Translate } from "react-localize-redux";
import { translate } from "../../shared/utils";

export default function CommonExperienceTable({
  commonExperiences = [],
  isLoading,
  onChange,
}) {
  const headers = [
    translate("common-experience.header-common-experience"),
    translate("common-experience.header-start-date"),
    translate("common-experience.header-end-date"),
    "",
  ];

  return (
    <Table
      isLoading={isLoading}
      content={commonExperiences}
      headers={headers}
      noContentMessage={<Translate id="common-experience.empty-message" />}
      renderRow={(commonExperience, index) => {
        return (
          <CommonExperienceTableRow
            key={`tr-${index}`}
            commonExperience={commonExperience}
            headers={headers}
            onChange={onChange}
          />
        );
      }}
    />
  );
}
