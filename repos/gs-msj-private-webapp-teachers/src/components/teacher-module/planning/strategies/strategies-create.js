import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import StrategiesDetail from "./components/strategies-detail";
import useCreateStrategy from "./hooks/use-create-strategy";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function StrategiesCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createStrategy, isLoading } = useCreateStrategy();
  const [strategy, setStrategy] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createStrategy({ ...strategy, planningId });
  };

  const isValid = () => {
    return (
      strategy?.implementationDates?.length > 0 &&
      (strategy?.defaults?.length > 0 || strategy?.customs?.length > 0)
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("accommodation.title"),
            url: "?page=strategies",
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
          <Translate id="accommodation.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=strategies")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=strategies");
          }}
          isLoading={isLoading}
        />
      </Border>

      <StrategiesDetail strategy={strategy} onChange={setStrategy} />
    </>
  );
}
