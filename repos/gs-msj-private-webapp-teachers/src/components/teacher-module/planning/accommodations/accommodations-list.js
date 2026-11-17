import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useAccommodations from "./hooks/use-get-accommodations";
import AccommodationsTable from "./components/accommodations-table";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import SessionManager from "../../../../custom-classes/session-manager";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function AccommodationsList() {
  const navigate = useHistory();
  const { accommodations, getAccommodations, isLoading } = useAccommodations();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

    useEffect(() => {
        // Reset breadcrumb paths
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("strategy.title"),
            url: "?page=accommodations",
        });

    }, [LanguageManager.getLanguage()]);

    useEffect(() => {
        getAccommodations(planningId);
    }, [planningId]);

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
          onClick={() => navigate.push("?page=accommodations-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <AccommodationsTable
        accommodations={accommodations}
        isLoading={isLoading}
        onChange={() => getAccommodations(planningId)}
      />
    </>
  );
}
