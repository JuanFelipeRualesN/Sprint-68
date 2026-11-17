import React, { useState, useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useGetQuizzes from "./hooks/useGetQuizzes";
import { translate } from "../shared/utils";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import Text from "../shared/components/text";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { Translate } from "react-localize-redux";
import QuizzesTable from "./components/quizzes-table";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import SessionManager from '../../../../custom-classes/session-manager';
import PaginationCustom from "../../../shared-module-components/datatables/pagination";

export default function QuizzesList() {
  const navigate = useHistory();
  const { quizzes, getQuizzes, isLoading } = useGetQuizzes();
  const { setPath } = useContext(BreadcrumbContext);
  const { planningId } = SessionManager.getSelectedPlanning();

    const lessonsLimiter = 10;
    const [currentPage, setCurrentPage] = useState(0);
    const [totalItems, setTotalItems] = useState(null);

  useEffect(() => {
    getQuizzes(planningId, lessonsLimiter, 0);
    setPath({
      root: translate("planning.menu-genial"),
      name: translate("quiz.header-quiz"),
      url: "?page=quizzes",
    });
  }, []);

  useEffect(() => {
    if (typeof quizzes.TotalQuizzes === "number") {
      setTotalItems(quizzes.TotalQuizzes);
    }
  }, [quizzes.TotalQuizzes]);

  const handlePageChange = (page) => {
    const newPageIndex = page - 1;
    setCurrentPage(newPageIndex);
    getQuizzes(planningId, lessonsLimiter, newPageIndex);
  }

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="quiz.header-quiz" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="quiz.title-message" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=quizzes-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <QuizzesTable
        quizzes={quizzes.quizzesLessons}
        isLoading={isLoading}
        onChange={() => getQuizzes(planningId, lessonsLimiter, currentPage)}
      />
      <PaginationCustom
        currentPage={currentPage + 1}
        totalPages={Math.ceil(totalItems / lessonsLimiter)}
        updateParent={handlePageChange}
      />
    </>
  );
}
