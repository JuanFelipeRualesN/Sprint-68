import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useGetActiveResponses from "./hooks/use-get-active-responses";
import ActiveResponsesTable from "./components/active-responses-table";
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

export default function ActiveResponsesList() {
  const navigate = useHistory();
  const { activeResponses, getActiveResponses, isLoading } = useGetActiveResponses();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
      resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-active-responses"),
      url: "?page=active-responses",
    });
    getActiveResponses(planningId);
  }, [LanguageManager.getLanguage()]);

    useEffect(() => {
        getActiveResponses(planningId);
    }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-active-responses" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="active-response.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=active-responses-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <ActiveResponsesTable
        activeResponses={activeResponses}
        isLoading={isLoading}
        onChange={() => getActiveResponses(planningId)}
      />
    </>
  );
}
