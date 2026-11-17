import React, { useContext, useEffect } from "react";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useObservations from "./hooks/use-get-observations";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import ObservationsTable from "./components/observations-table";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import { translate } from '../shared/utils';
import { Translate } from 'react-localize-redux';
import SessionManager from '../../../../custom-classes/session-manager';
import LanguageManager from "../../../../custom-classes/language-manager";

const ObservationsList = () => {
  const navigate = useHistory();
  const { observations, getObservations, isLoading } = useObservations();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
    resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-observations"),
      url: "?page=observations",
    });
  }, [LanguageManager.getLanguage()]);

    useEffect(() => {
        getObservations(planningId);
    }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-observations" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="observation.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=observations-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <ObservationsTable
        observations={observations}
        isLoading={isLoading}
        onChange={() => getObservations(planningId)}
      />
    </>
  );
};

export default ObservationsList;
