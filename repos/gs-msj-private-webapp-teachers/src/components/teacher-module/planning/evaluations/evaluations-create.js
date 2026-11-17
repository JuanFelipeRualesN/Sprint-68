import React, { useContext, useEffect, useState } from "react";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import EvaluationsDetail from "./components/evaluations-detail";
import useCreateEvaluation from "./hooks/use-create-evaluation";
import { Translate } from "react-localize-redux";
import { translate } from "../shared/utils";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function EvaluationsCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createEvaluation, isLoading } = useCreateEvaluation();
  const [evaluation, setEvaluation] = useState({});
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createEvaluation({ ...evaluation, planningId });
  };

    const isValid = () => {
        const hasImplementationDates = evaluation?.implementationDates?.length > 0;

        const hasDiagnostic =
            evaluation?.diagnostic?.defaults?.length > 0 ||
            evaluation?.diagnostic?.customs?.length > 0;

        const hasFormative =
            evaluation?.formative?.defaults?.length > 0 ||
            evaluation?.formative?.customs?.length > 0;

        const hasSummative =
            evaluation?.summative?.defaults?.length > 0 ||
            evaluation?.summative?.customs?.length > 0;

        return hasImplementationDates && (hasDiagnostic || hasFormative || hasSummative);
    };

    useEffect(() => {
        resetPaths();
        setPath({
            root: translate("planning.menu-open"),
            name: translate("planning.menu-evaluations"),
            url: "?page=evaluations",
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
          <Translate id="evaluation.create-message" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=evaluations")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=evaluations");
          }}
          isLoading={isLoading}
        />
      </Border>

      <EvaluationsDetail evaluation={evaluation} onChange={setEvaluation} />
    </>
  );
}
