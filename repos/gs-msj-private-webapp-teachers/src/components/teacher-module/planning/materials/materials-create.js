import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import MaterialDetail from "./components/materials-detail";
import useCreateMaterial from "./hooks/use-create-material";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function MaterialsCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createMaterial, isLoading } = useCreateMaterial();
  const [material, setMaterial] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createMaterial({ ...material, planningId });
  };

  const isValid = () => {
    return (
      material?.implementationDates?.length > 0 &&
      (material?.defaults?.length > 0 || material?.customs?.length > 0)
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("planning.menu-materials"),
            url: "?page=materials",
        });
        setPath({ name: translate("general.create-new") });
    }, [LanguageManager.getLanguage()]);


  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="general.create-new" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="material.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=materials")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=materials");
          }}
          isLoading={isLoading}
        />
      </Border>

      <MaterialDetail material={material} onChange={setMaterial} />
    </>
  );
}
