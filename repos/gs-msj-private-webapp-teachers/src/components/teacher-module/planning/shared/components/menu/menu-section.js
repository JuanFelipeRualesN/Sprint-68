import React from "react";
import { Translate } from "react-localize-redux";

export default function MenuSection({ title, children }) {
  return (
    <>
      <div className="sidebarMenu--itemTitle">
        <Translate id={title} />
      </div>
      {children}
    </>
  );
}
