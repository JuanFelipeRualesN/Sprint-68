import React, { useContext } from "react";
import { PreviewTableContext } from "../contexts/preview-table-provider";

export default function PreviewTableHeader() {
  const { headers } = useContext(PreviewTableContext);

  return (
    <thead>
      <tr>
        <th style={{minWidth: '170px'}} scope="col"/>
        {headers.map((header, index) => {
          return (
            <th scope="col" key={`preview-header-${index}`}>
              <span style={{display: 'flex', justifyContent: 'end', fontSize: '10px'}}>MM/DD/YYYY</span>
              <div className="tablePDF__infoDate">
                <div className="tablePDF__infoDate--day">{header.day}</div>
                <div className="tablePDF__infoDate--date">{header.date}</div>
              </div>
            </th>
          );
        })}
      </tr>
    </thead>
  );
}
