import React, { useContext, useEffect, useState } from "react";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import IntegrationsDetail from "./components/integrations-detail";
import useCreateIntegration from "./hooks/use-create-integration";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function IntegrationsCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createIntegration, isLoading } = useCreateIntegration();
  const [integration, setIntegration] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createIntegration({ ...integration, planningId });
  };

  const isValid = () => {
    return (
      integration?.implementationDates?.length > 0 &&
      integration?.subjects?.length > 0
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("planning.menu-integrations"),
            url: "?page=integrations",
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
          <Translate id="integration.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=integrations")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=integrations");
          }}
          isLoading={isLoading}
        />
      </Border>

      <IntegrationsDetail integration={integration} onChange={setIntegration} />
    </>
  );
}
