import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useEvaluations from "./hooks/use-get-evaluations";
import EvaluationTable from "./components/evaluations-table";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import SessionManager from '../../../../custom-classes/session-manager';
import LanguageManager from "../../../../custom-classes/language-manager";

export default function EvaluationsList() {
  const navigate = useHistory();
  const { evaluations, getEvaluations, isLoading } = useEvaluations();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
    getEvaluations(planningId);
  }, []);

  useEffect(() => {
    resetPaths();
    setPath({
        root: translate("planning.menu-open"),
        name: translate("planning.menu-evaluations"),
        url: "?page=evaluations",
    });
    getEvaluations(planningId);
  }, [LanguageManager.getLanguage()]);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-evaluations" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="evaluation.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=evaluations-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <EvaluationTable
        evaluations={evaluations}
        isLoading={isLoading}
        onChange={() => getEvaluations(planningId)}
      />
    </>
  );
}
