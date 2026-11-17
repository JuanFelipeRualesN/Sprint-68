import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import DifferentiationStrategiesDetail from "./components/differentiation-strategies-detail";
import useCreateDifferentiationStrategy from "./hooks/use-create-differentiation-strategies";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function DifferentiationStrategiesCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createDifferentiationStrategy, isLoading } = useCreateDifferentiationStrategy();
  const [differentiationStrategy, setDifferentiationStrategy] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createDifferentiationStrategy({ ...differentiationStrategy, planningId });
  };

  const isValid = () => {
    return (
      differentiationStrategy?.implementationDates?.length > 0 &&
      (differentiationStrategy?.defaults?.length > 0 || differentiationStrategy?.customs?.length > 0)
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("strategy.title"),
            url: "?page=differentiation-strategies",
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
          onClick={() => navigate.push("?page=differentiation-strategies")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=differentiation-strategies");
          }}
          isLoading={isLoading}
        />
      </Border>

      <DifferentiationStrategiesDetail differentiationStrategy={differentiationStrategy} onChange={setDifferentiationStrategy} />
    </>
  );
}
