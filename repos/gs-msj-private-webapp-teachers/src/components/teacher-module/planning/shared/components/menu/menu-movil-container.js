import React from "react";

export default function MenuMovilContainer({ title, children }) {
  return (
    <div className="wrapper-movilMenu">
      <input
        className="movilMenuToggle-box"
        id="identifier-1"
        type="checkbox"
      />
      <label htmlFor="identifier-1">
        <div className="movilMenubox--title">{title}</div>
      </label>
      <div className="movilMenu_Content">{children}</div>
    </div>
  );
}
