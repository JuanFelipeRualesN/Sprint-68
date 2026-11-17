import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faExclamationTriangle } from "@fortawesome/free-solid-svg-icons";

export default function PreviewRowTitle({ rowSpan, title, disclaimer }) {
  return (
    <th rowSpan={rowSpan}>
      {disclaimer ? (
        <>
          <p>{title}</p>
          <div className="RegionBase-notificaction-simple">
            <FontAwesomeIcon icon={faExclamationTriangle} />
            {disclaimer}
          </div>
        </>
      ) : (
        title
      )}
    </th>
  );
}
