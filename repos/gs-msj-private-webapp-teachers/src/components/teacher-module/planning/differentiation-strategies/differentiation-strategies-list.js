import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useDifferentiationStrategies from "./hooks/use-get-differentiation-strategies";
import DifferentiationStrategiesTable from "./components/differentiation-strategies-table";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function DifferentiationStrategiesList() {
  const navigate = useHistory();
  const { differentiationStrategies, getDifferentiationStrategies, isLoading } = useDifferentiationStrategies();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
      resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("strategy.title"),
      url: "?page=differentiation-strategies",
    });
    getDifferentiationStrategies(planningId);
  }, [LanguageManager.getLanguage()]);

    useEffect(() => {
        getDifferentiationStrategies(planningId);
    }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="strategy.title" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="strategy.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=differentiation-strategies-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <DifferentiationStrategiesTable
        differentiationStrategies={differentiationStrategies}
        isLoading={isLoading}
        onChange={() => getDifferentiationStrategies(planningId)}
      />
    </>
  );
}
