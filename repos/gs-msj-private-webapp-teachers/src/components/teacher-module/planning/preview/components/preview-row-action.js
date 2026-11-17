import React, { useContext } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PreviewTableContext } from "../contexts/preview-table-provider";
import { faPlusCircle } from "@fortawesome/free-solid-svg-icons";

export default function PreviewRowAction({ title, children, onClick }) {
  const { headers } = useContext(PreviewTableContext);

  return (
    <tr className="RowActions">
      {children}
      {headers.map((header, index) => (
        <td key={`${header.date}-row-action-${index}`}>
          <button
            className="btn-base btn-onlyPDF"
            onClick={() => onClick(header.date)}
          >
            {title}
            <FontAwesomeIcon icon={faPlusCircle} />
          </button>
        </td>
      ))}
    </tr>
  );
}
