import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import useCreateReflection from "./hooks/use-create-reflection";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import ReflectionsDetail from "./components/reflections-detail";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function ReflectionsCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createReflection, isLoading } = useCreateReflection();
  const [reflection, setReflection] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createReflection({ ...reflection, planningId });
  };

  const isValid = () => {
    return (
      reflection?.implementationDates?.length > 0 &&
      ((reflection?.achieved && reflection?.detail) ||
        (reflection?.achieved === false && reflection?.reasons?.length > 0))
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("planning.menu-reflections"),
            url: "?page=reflections",
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
          <Translate id="reflection.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=reflections")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=reflections");
          }}
          isLoading={isLoading}
        />
      </Border>

      <ReflectionsDetail reflection={reflection} onChange={setReflection} />
    </>
  );
}
