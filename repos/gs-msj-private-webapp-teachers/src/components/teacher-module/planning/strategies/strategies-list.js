import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useStrategies from "./hooks/use-get-strategies";
import StrategiesTable from "./components/strategies-table";
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

export default function StrategiesList() {
  const navigate = useHistory();
  const { strategies, getStrategies, isLoading } = useStrategies();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
      resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("accommodation.title"),
      url: "?page=strategies",
    });
    getStrategies(planningId);
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getStrategies(planningId);
  }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="accommodation.title" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="accommodation.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=strategies-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <StrategiesTable
        strategies={strategies}
        isLoading={isLoading}
        onChange={() => getStrategies(planningId)}
      />
    </>
  );
}
