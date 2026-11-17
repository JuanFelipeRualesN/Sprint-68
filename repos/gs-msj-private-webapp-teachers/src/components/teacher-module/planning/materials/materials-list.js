import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useGetPlanningMaterials from "./hooks/use-get-materials";
import MaterialsTable from "./components/materials-table";
import { translate } from "../shared/utils";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function PlanningMaterials() {
  const navigate = useHistory();
  const { materials, getMaterials, isLoading } = useGetPlanningMaterials();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
      resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-materials"),
      url: "?page=materials",
    });
    getMaterials(planningId);
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getMaterials(planningId);
  }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-materials" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="material.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=materials-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <MaterialsTable
        materials={materials}
        isLoading={isLoading}
        onChange={() => getMaterials(planningId)}
      />
    </>
  );
}
