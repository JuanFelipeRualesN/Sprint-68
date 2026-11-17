import React, { useContext, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import ForumsDetail from "./components/forums-detail";
import useCreateForum from "./hooks/useCreateForum";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function CreateForum() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createForum, isLoading } = useCreateForum();
  const [forum, setForum] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createForum({ ...forum, planningId: planningId });
  };

  const isValid = () => {
    return (
      forum?.title &&
      forum?.description &&
      forum?.implementationDates &&
      forum?.implementationDates?.length > 0
    );
  };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-genial"),
            name: translate("planning.menu-forums"),
            url: "?page=forums",
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
          <Translate id="forum.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=forums")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=forums");
          }}
          isLoading={isLoading}
        />
      </Border>

      <ForumsDetail forum={forum} onChange={setForum} />
    </>
  );
}
