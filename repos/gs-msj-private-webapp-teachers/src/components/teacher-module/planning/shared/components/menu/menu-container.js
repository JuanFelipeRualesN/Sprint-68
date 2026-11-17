import React from "react";

export default function MenuContainer({ title, children }) {
  return (
    <nav className="sidebarMenu">
      <div className="sidebarMenu--itemPpal">
        {title} <i className="fas fa-chevron-down"></i>
      </div>
      {children}
    </nav>
  );
}
