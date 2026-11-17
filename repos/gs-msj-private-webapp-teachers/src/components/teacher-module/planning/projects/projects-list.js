import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory, useParams } from "react-router-dom";
import ProjectsTable from "./components/projects-table";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import { translate } from "../shared/utils";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import useGetProjects from "./hooks/use-get-projects";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function ProjectsList() {
  const navigate = useHistory();
  const { projects, getProjects, isLoading } = useGetProjects();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
    resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-projects"),
      url: "?page=projects",
    });
    getProjects(planningId);
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getProjects(planningId);
  }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-projects" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="project.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=projects-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <ProjectsTable
        projects={projects}
        isLoading={isLoading}
        onChange={() => getProjects(planningId)}
      />
    </>
  );
}
