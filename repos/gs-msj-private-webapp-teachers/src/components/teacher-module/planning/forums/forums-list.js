import React, { useState, useContext, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useHistory } from "react-router-dom";
import useGetForums from "./hooks/useGetForums";
import Heading from "../shared/components/heading";
import Text from "../shared/components/text";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import Space from "../shared/components/space";
import ForumsTable from "./components/forums-table";
import PaginationCustom from "../../../shared-module-components/datatables/pagination";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import SessionManager from "../../../../custom-classes/session-manager";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import { translate } from '../shared/utils';
import { Translate } from 'react-localize-redux';
import LanguageManager from "../../../../custom-classes/language-manager";

export default function ForumsList({disabled = false}) {
  const navigate = useHistory();
  const { forums, isLoading, getForums } = useGetForums();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { planningId } = SessionManager.getSelectedPlanning();

  const forumsLimiter = 10;
  const [currentPage, setCurrentPage] = useState(0);
  const [totalItems, setTotalItems] = useState(null);

  useEffect(() => {
      resetPaths();
    setPath({
      root: translate("planning.menu-genial"),
      name: translate("planning.menu-forums"),
      url: "?page=forums",
    });
  }, [LanguageManager.getLanguage()]);

    useEffect(() => {
        getForums(planningId, forumsLimiter, 0);
    }, []);

  useEffect(() => {
    if (typeof forums.TotalItems === "number") {
      setTotalItems(forums.TotalItems);
    }
  }, [forums.TotalItems]);

  const handlePageChange = (page) => {
    const newPageIndex = page - 1;
    setCurrentPage(newPageIndex);
    getForums(planningId, forumsLimiter, newPageIndex);
  }

  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="forum.forum" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="forum.title-message" />
        </Text>
      </Border>

      <Border side="Bottom">
        <Button
          variant="secondary"
          disabled={disabled}
          icon={<FontAwesomeIcon icon={faPlusCircle} />}
          onClick={() => navigate.push("?page=forums-create")}
        >
          <Translate id="general.create-new" />
        </Button>
      </Border>

      <Space variant="regular" />

      <ForumsTable
        forums={forums.Items}
        isLoading={isLoading}
        disabled={disabled}
        onChange={() => getForums(planningId, forumsLimiter, currentPage)}
      />
      <PaginationCustom
        currentPage={currentPage + 1}
        totalPages={Math.ceil(totalItems / forumsLimiter)}
        updateParent={handlePageChange}
      />
    </>
  );
}
