import React from "react";

export default function Button({
  className = "",
  variant,
  children,
  icon,
  onClick,
  disabled = false,
                                 style,
}) {
  const buttonClass = icon && !variant ? "btn-onlyIcon" : `btn-${variant}`;

  return (
    <button
      type="button"
      disabled={disabled}
      onClick={onClick}
      style={style}
      className={`pl btn-base ${buttonClass} ${
        icon ? "btn-icon" : ""
      } ${className ?? ""}`}
    >
      {icon}
      {children}
    </button>
  );
}
