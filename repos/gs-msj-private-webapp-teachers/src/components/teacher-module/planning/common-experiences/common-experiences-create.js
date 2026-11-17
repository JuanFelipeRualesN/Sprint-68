import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import useCreateCommonExperience from "./hooks/use-create-common-experiences";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import CommonExperiencesDetail from "./components/common-experiences-detail";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function CommonExperiencesCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createCommonExperience, isLoading } = useCreateCommonExperience();
  const [commonExperience, setCommonExperience] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createCommonExperience({ ...commonExperience, planningId });
  };

  const isValid = () => {
    return (
      commonExperience?.implementationDates?.length > 0 &&
      ((commonExperience?.achieved && commonExperience?.detail) ||
        (commonExperience?.achieved === false && commonExperience?.reasons?.length > 0))
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("planning.menu-common-experiences"),
            url: "?page=common-experiences",
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
          <Translate id="common-experience.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=common-experiences")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=common-experiences");
          }}
          isLoading={isLoading}
        />
      </Border>

      <CommonExperiencesDetail commonExperience={commonExperience} onChange={setCommonExperience} />
    </>
  );
}
