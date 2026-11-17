import React, { useContext } from "react";
import { useLocation, useHistory } from "react-router-dom";
import { BreadcrumbContext } from "../../providers/breadcrumb";
import { Translate } from "react-localize-redux";

export default function PreviewItem() {
  const { resetPaths } = useContext(BreadcrumbContext);
  const navigate = useHistory();
  const query = new URLSearchParams(useLocation().search);
  const page = query.get("page");

  const goTo = (path) => {
    resetPaths();
    navigate.push(path);
  };

  const activeClassName = (path) => (page === path ? "active" : "");

  return (
    <div
      className={`sidebarMenu--item ${activeClassName("preview")}`}
      key="preview-item-1"
    >
      <a onClick={() => goTo("?page=preview")}>
        <Translate id="planning.menu-preview" />
      </a>
    </div>
  );
}
