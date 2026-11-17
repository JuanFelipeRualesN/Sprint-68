import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import AccommodationsDetail from "./components/accommodations-detail";
import useCreateAccommodation from "./hooks/use-create-accommodation";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function AccommodationsCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createAccommodation, isLoading } = useCreateAccommodation();
  const [accommodation, setAccommodation] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createAccommodation({ ...accommodation, planningId });
  };

  const isValid = () => {
    return (
      accommodation?.implementationDates?.length > 0 &&
      (accommodation?.defaults?.length > 0 ||
        accommodation?.customs?.length > 0)
    );
  };

  useEffect(() => {
      // Reset breadcrumb paths
      resetPaths();
      setPath({
          root: translate("planning.menu-open"),
          name: translate("strategy.title"),
          url: "?page=accommodations",
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
          onClick={() => navigate.push("?page=accommodations")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=accommodations");
          }}
          isLoading={isLoading}
        />
      </Border>

      <AccommodationsDetail
        accommodation={accommodation}
        onChange={setAccommodation}
      />
    </>
  );
}
