import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useGetIntegrations from "./hooks/use-get-integrations";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import IntegrationsTable from "./components/integrations-table";
import SessionManager from "../../../../custom-classes/session-manager";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function IntegrationsList() {
  const navigate = useHistory();
  const { integrations, getIntegrations, isLoading } = useGetIntegrations();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
      resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-integrations"),
      url: "?page=integrations",
    });
    getIntegrations(planningId);
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getIntegrations(planningId);
  }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-integrations" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="integration.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=integrations-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <IntegrationsTable
        integrations={integrations}
        isLoading={isLoading}
        onChange={() => getIntegrations(planningId)}
      />
    </>
  );
}
