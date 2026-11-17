import React, { useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useGetOpenLessons from "./hooks/use-get-open-lessons";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import LessonsTable from "./components/open-lessons-table";
import SessionManager from "../../../../custom-classes/session-manager";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function OpenLessonsList() {
  const navigate = useHistory();
  const { lessons, getOpenLessons, isLoading } = useGetOpenLessons();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  useEffect(() => {
    getOpenLessons(planningId);

  }, []);

  useEffect(() => {
    resetPaths();
    setPath({
        root: translate("planning.menu-open"),
        name: translate("planning.menu-lessons"),
        url: "?page=open-lessons",
    });
    getOpenLessons(planningId);
  }, [LanguageManager.getLanguage()]);

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="planning.menu-lessons" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="lesson.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=open-lessons-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <LessonsTable
        lessons={lessons}
        isLoading={isLoading}
        onChange={() => getOpenLessons(planningId)}
      />
    </>
  );
}
