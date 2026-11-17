import React, { useContext, useEffect } from "react";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useCommonExperiences from "./hooks/use-get-common-experiences";
import Space from "../shared/components/space";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import CommonExperiencesTable from "./components/common-experiences-table";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import { translate } from '../shared/utils';
import { Translate } from 'react-localize-redux';
import SessionManager from '../../../../custom-classes/session-manager';
import LanguageManager from "../../../../custom-classes/language-manager";

const CommonExperiencesList = () => {
  const navigate = useHistory();
  const { commonExperiences, getCommonExperiences, isLoading } = useCommonExperiences();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
    resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-common-experiences"),
      url: "?page=common-experiences",
    });
  }, [LanguageManager.getLanguage()]);

    useEffect(() => {
        getCommonExperiences(planningId);
    }, []);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-common-experiences" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="common-experience.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=common-experiences-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <CommonExperiencesTable
        commonExperiences={commonExperiences}
        isLoading={isLoading}
        onChange={() => getCommonExperiences(planningId)}
      />
    </>
  );
};

export default CommonExperiencesList;
