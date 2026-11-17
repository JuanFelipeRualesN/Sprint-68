import React, { useContext, useEffect } from "react";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useReflections from "./hooks/use-get-reflections";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import ReflectionsTable from "./components/reflections-table";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import { translate } from '../shared/utils';
import { Translate } from 'react-localize-redux';
import SessionManager from '../../../../custom-classes/session-manager';
import LanguageManager from "../../../../custom-classes/language-manager";

const ReflectionsList = () => {
  const navigate = useHistory();
  const { reflections, getReflections, isLoading } = useReflections();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
    resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-reflections"),
      url: "?page=reflections",
    });
    getReflections(planningId);
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getReflections(planningId);
  }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-reflections" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="reflection.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=reflections-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <ReflectionsTable
        reflections={reflections}
        isLoading={isLoading}
        onChange={() => getReflections(planningId)}
      />
    </>
  );
};

export default ReflectionsList;
