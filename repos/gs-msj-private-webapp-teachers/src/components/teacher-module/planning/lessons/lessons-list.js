import React, { useState, useContext, useEffect } from "react";
import { useHistory } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Button from "../shared/components/button";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Space from "../shared/components/space";
import { translate } from "../shared/utils";
import useGetPlanningLessons from "./hooks/useGetPlanningLessons";
import SessionManager from "../../../../custom-classes/session-manager";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { Translate } from "react-localize-redux";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import LessonsTable from "./components/lessons-table";
import PaginationCustom from "../../../shared-module-components/datatables/pagination";
import LanguageManager from "../../../../custom-classes/language-manager";

export default function LessonsList({disabled}) {
  const navigate = useHistory();
  const {
    planningLesson: lessons,
    isLoading,
    getPlanningLessons: getLessons,
  } = useGetPlanningLessons();
  const { planningId } = SessionManager.getSelectedPlanning();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);

  const lessonsLimiter = 10;
  const [currentPage, setCurrentPage] = useState(0);
  const [totalItems, setTotalItems] = useState(null);
  

  useEffect(() => {
    getLessons(planningId, lessonsLimiter, 0);
  }, [planningId]);

  useEffect(() => {
    resetPaths();
    setPath({
        root: translate("planning.menu-genial"),
        name: translate("lesson.title"),
        url: "?page=lessons",
    });
    getLessons(planningId, lessonsLimiter, 0)
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    if (typeof lessons.TotalLessons === "number") {
      setTotalItems(lessons.TotalLessons);
    }
  }, [lessons.TotalLessons]);
  

  const handlePageChange = (page) => {
    const newPageIndex = page - 1;
    setCurrentPage(newPageIndex);
    getLessons(planningId, lessonsLimiter, newPageIndex);
  }

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="lesson.title" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="lesson.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
            disabled={disabled}
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          variant="secondary"
          onClick={() => navigate.push("?page=lessons-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />
      <LessonsTable
          disabled={disabled}
        lessons={lessons.planningLessons}
        isLoading={isLoading}
        onChange={() => getLessons(planningId, lessonsLimiter, currentPage)}
      />
      <PaginationCustom
        currentPage={currentPage + 1}
        totalPages={Math.ceil(totalItems / lessonsLimiter)}
        updateParent={handlePageChange}
      />
    </>
  );
}
