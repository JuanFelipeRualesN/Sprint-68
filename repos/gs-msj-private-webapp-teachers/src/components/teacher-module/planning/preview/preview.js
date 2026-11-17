import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PreviewTable from "./components/preview-table";
import { translate } from "../shared/utils";
import Heading from "../shared/components/heading";
import Space from "../shared/components/space";
import Button from "../shared/components/button";
import Text from "../shared/components/text";
import FormSelect from "../shared/components/form/form-select";
import Loading from "../shared/components/loading"
// import FormMultiSelectCheckmarks from "../shared/components/form-multi-select-checkmarks";
import useGetPlanningDetails from "../shared/hooks/useGetPlanningDetails";
import useGetPreviewPdf from "./hooks/useGetPreviewPdf";
import SessionManager from "../../../../custom-classes/session-manager";
import Settings from "../../../../custom-classes/settings";
import { faSearch, faTable, faFilePdf, faExclamationTriangle, faBroom } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import LanguageManager from "../../../../custom-classes/language-manager";


export default function Preview() {
  const { planningId } = SessionManager.getSelectedPlanning();
  const { planning, getPlanning } = useGetPlanningDetails();
  const { pdfDocument, isLoading, getPreviewPdf } = useGetPreviewPdf();
  const [currentLesson, setCurrentLesson] = useState("");
  const [viewAsPDF, setViewAsPDF] = useState(true);
  const [selectedSections, setSelectedSections] = useState([""]);
  const [selectedLesson, setSelectedLesson] = useState("");

  const sections = [
    { label: translate("preview.table-all"), value: "" },
    { label: translate("preview.table-lessons"), value: "lessons" },
    { label: translate("preview.table-standards"), value: "standards" },
    { label: translate("preview.table-expectations"), value: "expectations" },
    { label: translate("preview.table-strategies"), value: "strategies" },
    { label: translate("preview.table-goals"), value: "goals" },
    { label: translate("preview.table-units"), value: "units" },
    // The MED Quindío lesson template has no cross-curricular themes
    ...(Settings.IS_MED_QUINDIO ? [] : [{ label: translate("preview.table-topics"), value: "topics" }]),
    { label: translate("preview.table-quizzes"), value: "quizzes" },
    { label: translate("preview.table-forums"), value: "forums" },
    { label: translate("preview.table-assignments"), value: "assignments" },
    { label: translate("preview.table-evaluations"), value: "evaluations" },
    { label: translate("preview.table-integrations"), value: "integrations" },
    { label: translate("preview.table-projects"), value: "projects" },
    { label: translate("preview.table-accommodations"), value: "accommodations" },
    { label: translate("preview.table-materials"), value: "materials" },
    { label: translate("preview.table-reflections"), value: "reflections" },
    { label: translate("preview.table-instructional-strategies"), value: "instructional-strategies" },
  ];

  useEffect(() => {
    getPlanning(planningId);
    getPreviewPdf(planningId);
  }, []);

  useEffect(() => {
    getPreviewPdf(planningId);
  }, [LanguageManager.getLanguage()]);

  const downloadPDF = async () => {

    if (!pdfDocument || !pdfDocument.PreviewBase64) {
      alert("Error generating PDF.");
      return;
    }

    // Decode Base64 to binary
    const binaryPDF = atob(pdfDocument.PreviewBase64);
    
    // Create Uint8Array from binary PDF data
    const array = new Uint8Array(binaryPDF.length);
    for (let i = 0; i < binaryPDF.length; i++) {
      array[i] = binaryPDF.charCodeAt(i);
    }
  
    // Create Blob from binary PDF data
    const blob = new Blob([array], { type: 'application/pdf' });

    // Create download link
    const url = URL.createObjectURL(blob);

    // Create anchor element
    const a = document.createElement('a');
    a.href = url;
    a.download = 'Planning Preview.pdf';

    // Append anchor to body, click it, and remove it
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

    // Revoke URL object explicitly after download
    URL.revokeObjectURL(url);
  };

  return (
    <>
      <Space variant="regular" />
      <Heading as="h2" className="mb-0">
        <Translate id="preview.title" />
      </Heading>
      <Text type="regular">
        <Translate id="preview.message" />
      </Text>
      <Space variant="regular" />
      {viewAsPDF && (
        <div className="RegionBase-notificaction-simple">
          <FontAwesomeIcon icon={faExclamationTriangle} />
          <span>
            <Translate id="preview.pdf-warning" />
          </span>
      </div>
      )}
      <>
        <div className="wrapper-twoCol-spaceBetween row-reverse">
          {/*<Button
              variant="secondary"
              icon={<FontAwesomeIcon icon={viewAsPDF ? faTable : faFilePdf} />}
              onClick={() => setViewAsPDF(!viewAsPDF)}
            >
              {viewAsPDF ? (
                <Translate id="preview.view-planning" />
              ) : (
                <Translate id="preview.view-pdf" />
              )} 
            </Button> */}
            {viewAsPDF &&
              <Button
                variant="none"
                color="none"
                className="button-secondary-custom"
                icon={!isLoading && <FontAwesomeIcon icon={faFilePdf} />}
                onClick={downloadPDF}
                disabled={isLoading}
              >
                {isLoading ? (
                  <Loading type="button" show={isLoading} text={<Translate id="preview.generating-pdf" />} />
                ) : (
                  <span>
                    <Translate id="preview.print-pdf" />
                  </span>
                )}
              </Button>
            }
          <div className="d-flex w-50">
            <FormSelect
              className="m-0 w-50"
              name="lessons"
              label=""
              placeholder={translate("preview.select-lesson")}
              options={planning?.lessons?.map((lesson) => ({
                label: lesson.title,
                value: lesson.lessonId,
              }))}
              value={selectedLesson}
              onChange={(value) => {
                setSelectedLesson(value);
              }}
            />
            {/* <FormMultiSelectCheckmarks
              className="mt-0 w-50 ms-4"
              name="sections"
              label=""
              placeholder={translate("preview.table-all")}
              options={sections}
              values={selectedSections}
              onChange={(selectedValues) => {
                setSelectedSections(selectedValues);
              }}
            /> */}

            <Button
              variant="simple"
              className="ml-4"
              icon={<FontAwesomeIcon icon={faSearch} />}
              onClick={() => {
                setCurrentLesson(selectedLesson);
              }}
            />
            <Button
              variant="simple"
              className="ml-4"
              icon={<FontAwesomeIcon icon={faBroom} />}
              onClick={() => {
                setCurrentLesson("");
                setSelectedLesson("");
              }}
            />
          </div>
        </div>
        <PreviewTable
          planning={planning}
          filters={{
            lesson: currentLesson,
            sections: selectedSections,
          }}
          viewAsPDF={viewAsPDF}
        />
      </>
    </>
  );
}
