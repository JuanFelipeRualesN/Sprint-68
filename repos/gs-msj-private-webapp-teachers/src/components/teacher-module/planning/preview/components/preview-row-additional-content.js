import React, { useContext } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PreviewRowAction from "./preview-row-action";
import PreviewRowTitle from "./preview-row-title";
import { PreviewTableContext } from "../contexts/preview-table-provider";
import { faPenSquare } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";

export const AdditionalContentRowCell = ({
  day,
  content,
  viewAsPDF,
  isHtml,
  onEdit,
}) => {
  const contentForDay = content?.filter((l) => l.date === day.date);

  return !contentForDay || contentForDay.length < 1 ? null : (
    <div className="tablePDF__container">
      <ul className="tablePDF__list">
        {contentForDay.map((content, index) => {
          return (
            <li
              key={`${content.id}-content-item-${index}`}
              className="tablePDF__list--item"
            >
              {!viewAsPDF && (
                <button
                  className={`btn-base btn-onlyPDF ${index !== 0 &&
                    "borderTop"}`}
                  onClick={() => onEdit(content.id)}
                >
                  <FontAwesomeIcon icon={faPenSquare} />
                </button>
              )}
              <div
                className={`tablePDF__list--item_desc ${viewAsPDF &&
                  "py-2"} ${index !== 0 && viewAsPDF && "borderTop"}`}
              >
                  <p className={`list--item_desc--title list--${content.type === "default" ? "item_firstTitle" : "item_secondTitle"}`}>
                    {content.title}
                  </p>
                {isHtml ? (
                  <div
                    className="list--item_desc--desc_content"
                    dangerouslySetInnerHTML={{ __html: content.description }}
                  />
                ) : (
                  <div className="list--item_desc--desc_content">
                    {content.description}
                  </div>
                )}
              </div>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

export default function PreviewRowAdditionalContent({
  title,
  data,
  onAdd,
  onEdit,
  addButtonTitle = <Translate id="preview.table-add-additional-content" />,
  disclaimer,
}) {
  const { headers, viewAsPDF } = useContext(PreviewTableContext);
  const defaultAdditionalContents = data.filter((s) => s.type !== "additional");
  const additionalAdditionalContents = data.filter(
    (s) => s.type === "additional"
  );

  const DefaultAdditionalContentRowCell = ({ day }) => {
    return (
      <AdditionalContentRowCell
        day={day}
        content={defaultAdditionalContents}
        onEdit={onEdit}
        viewAsPDF={viewAsPDF}
      />
    );
  };

  const AdditionalAdditionalContentRowCell = ({ day }) => {
    return (
      <AdditionalContentRowCell
        day={day}
        content={additionalAdditionalContents}
        onEdit={onEdit}
        viewAsPDF={viewAsPDF}
      />
    );
  };

  const hasDefaultsToDisplay =
    defaultAdditionalContents && defaultAdditionalContents.length > 0;
  const hasAdditionalsToDisplay =
    additionalAdditionalContents && additionalAdditionalContents.length > 0;
  const rowSpan =
    Number(hasDefaultsToDisplay) +
    Number(hasAdditionalsToDisplay) +
    Number(!viewAsPDF);

  return (
    <>
      {hasDefaultsToDisplay ? (
        <tr>
          <PreviewRowTitle
            rowSpan={rowSpan}
            title={title}
            disclaimer={disclaimer}
          />
          {headers.map((header, index) => (
            <td key={`default-content-cell-${index}`}>
              <DefaultAdditionalContentRowCell day={header} />
            </td>
          ))}
        </tr>
      ) : null}

      {!viewAsPDF ? (
        <PreviewRowAction title={addButtonTitle} onClick={onAdd}>
          {!hasDefaultsToDisplay ? (
            <PreviewRowTitle
              rowSpan={rowSpan}
              title={title}
              disclaimer={disclaimer}
            />
          ) : null}
        </PreviewRowAction>
      ) : null}

      {hasAdditionalsToDisplay ? (
        <tr>
          {headers.map((header, index) => (
            <td key={`additional-content-cell-${index}`}>
              <AdditionalAdditionalContentRowCell day={header} />
            </td>
          ))}
        </tr>
      ) : null}
    </>
  );
}
