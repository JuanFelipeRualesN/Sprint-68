import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import ActiveResponsesDetail from "./components/active-responses-detail";
import useCreateActiveResponse from "./hooks/use-create-active-responses";
import { Translate } from "react-localize-redux";
import { translate } from "../shared/utils";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function ActiveResponsesCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createActiveResponse, isLoading } = useCreateActiveResponse();
  const [activeResponse, setActiveResponse] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createActiveResponse({ ...activeResponse, planningId });
  };

    const isValid = () => {
        const hasImplementationDates = activeResponse?.implementationDates?.length > 0;

        const hasType = activeResponse?.activeResponseTypeIds?.length > 0;

        return hasImplementationDates && hasType;
    };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("planning.menu-active-responses"),
            url: "?page=active-responses",
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
          <Translate id="active-response.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=active-responses")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=active-responses");
          }}
          isLoading={isLoading}
        />
      </Border>

      <ActiveResponsesDetail activeResponse={activeResponse} onChange={setActiveResponse} />
    </>
  );
}
