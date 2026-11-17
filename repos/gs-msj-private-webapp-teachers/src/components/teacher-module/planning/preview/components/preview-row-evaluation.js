import React, { useContext, useEffect, useState } from "react";
import PreviewRowTitle from "./preview-row-title";
import { PreviewTableContext } from "../contexts/preview-table-provider";
import { QuizRowCell } from "./preview-row-quiz";
import { AdditionalContentRowCell } from "./preview-row-additional-content";
import { Translate } from "react-localize-redux";
import { format, translate } from '../../shared/utils';

export default function PreviewRowEvaluation({
  quizzes,
  forums,
  assignments,
  evaluations,
}) {
  const { headers, viewAsPDF } = useContext(PreviewTableContext);
  const [transformedEvaluations, setTransformedEvaluations] = useState();
  
  const showRow = quizzes?.length > 0 || forums?.length > 0 || assignments?.length > 0 || evaluations?.length > 0;

  const onForumEdit = (id) => {
    console.log(id);
  };

  const onAssignmentEdit = (id) => {
    console.log(id);
  };

  const onEvaluationEdit = (id) => {
    console.log(id);
  };

  useEffect(() => {
    const content = evaluations?.flatMap((evaluation) => {
      return evaluation.implementationDates.map((date) => ({
        id: evaluation.Id,
        type: "default",
        title: "",
        description: evaluation.categories?.map((category, index) => (
          <div key={`category-${index}`} className="mb-2">
            <div>
              <strong>{category.name}:</strong>{" "}
              {category.title}
              {category.subtitle && (
                <div>
                  <strong>
                    <Translate id="general.other" />:{" "}
                  </strong>
                  {category.subtitle}
                </div>
              )}
            </div>
          </div>
        )),
        date: format(date),
      }));
    });

    setTransformedEvaluations(content);
  }, [evaluations]);

  return viewAsPDF && showRow ? (
    <>
      <tr>
        <PreviewRowTitle
          rowSpan={1}
          title={<Translate id="preview.table-evaluations" />}
        />
        {headers.map((header, index) => (
          <td key={`evaluations-cell-${index}`}>
            <QuizRowCell day={header} quizzes={quizzes} viewAsPDF={viewAsPDF} />
            <AdditionalContentRowCell
              day={header}
              content={forums}
              viewAsPDF={viewAsPDF}
              isHtml={true}
              onEdit={onForumEdit}
            />
            <AdditionalContentRowCell
              day={header}
              content={assignments}
              viewAsPDF={viewAsPDF}
              onEdit={onAssignmentEdit}
            />
            <AdditionalContentRowCell
              day={header}
              content={transformedEvaluations}
              viewAsPDF={viewAsPDF}
              onEdit={onEvaluationEdit}
            />
          </td>
        ))}
      </tr>
    </>
  ) : null;
}
