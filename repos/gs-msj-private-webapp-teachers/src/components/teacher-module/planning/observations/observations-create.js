import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import useCreateObservations from "./hooks/use-create-observations";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import ObservationsDetail from "./components/observations-detail";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function ObservationsCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createObservation, isLoading } = useCreateObservations();
  const [observation, setObservation] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createObservation({ ...observation, planningId });
  };

  const isValid = () => {
    return (
      observation?.implementationDates?.length > 0 &&
      ((observation?.achieved && observation?.detail) ||
        (observation?.achieved === false && observation?.reasons?.length > 0))
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("planning.menu-observations"),
            url: "?page=observations",
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
          <Translate id="observation.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=observations")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=observations");
          }}
          isLoading={isLoading}
        />
      </Border>

      <ObservationsDetail observation={observation} onChange={setObservation} />
    </>
  );
}
