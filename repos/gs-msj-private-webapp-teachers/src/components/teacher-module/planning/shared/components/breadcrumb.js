import React, { useContext } from "react";
import { useHistory } from "react-router-dom";
import { BreadcrumbContext } from "../providers/breadcrumb";

export default function Breadcrumb() {
  const navigate = useHistory();
  const breadcrumb = useContext(BreadcrumbContext);

  return breadcrumb.paths && breadcrumb.paths.length > 0 ? (
    <nav className="breadcrumb space-margin-top-md">
      {breadcrumb.paths?.map((item, index) => {
        const isLink = item.url && index < breadcrumb.paths.length - 1;

        return (
          <span key={`menu-item-${index}`} className="d-flex">
            {item.root && <li className="breadcrumb--title">{item.root}</li>}
            <li>
              {isLink ? (
                <a onClick={() => navigate.push(item.url)}>{item.name}</a>
              ) : (
                item.name
              )}
            </li>
          </span>
        );
      })}
    </nav>
  ) : null;
}
