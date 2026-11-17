import React, { useContext } from "react";
import { useLocation, useHistory } from "react-router-dom";
import { BreadcrumbContext } from "../../providers/breadcrumb";
import { MENU_ITEMS } from "../../constants/menu";
import { Translate } from "react-localize-redux";

export default function MenuItems({ section }) {
  const { resetPaths } = useContext(BreadcrumbContext);
  const navigate = useHistory();
  const query = new URLSearchParams(useLocation().search);
  const page = query.get("page");

  const items = MENU_ITEMS.filter((m) => m.section === section);

  const goTo = (path) => {
    resetPaths();
    navigate.push(path);
  };

  const activeClassName = (path) => (page.startsWith(path) ? "active" : "");

  return (
    <>
      {items.map((item, index) => {
        return (
          <div
            className={`sidebarMenu--item ${activeClassName(item.activeClass)}`}
            key={`menu-item-${index}`}
          >
            <a onClick={() => goTo(item.url)}>
              <Translate id={item.title} />
            </a>
          </div>
        );
      })}
    </>
  );
}
