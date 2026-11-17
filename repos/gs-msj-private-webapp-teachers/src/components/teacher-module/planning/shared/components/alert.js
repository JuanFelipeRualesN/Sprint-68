import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faExclamationTriangle, faInfoCircle } from '@fortawesome/free-solid-svg-icons';

export default function Alert({
  message,
  className = "",
  variant = "info",
  size,
}) {
  return (
    <div
      className={`RegionBase-notificaction${
        size === "sm" ? "-simple" : ""
      } notificaction__${variant} ${className}`}
    >
      <FontAwesomeIcon
        icon={variant === "info" ? faInfoCircle : faExclamationTriangle}
      />
      {message}
    </div>
  );
}
