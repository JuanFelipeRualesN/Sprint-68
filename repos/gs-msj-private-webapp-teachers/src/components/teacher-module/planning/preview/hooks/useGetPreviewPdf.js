import { useState, useRef, useEffect} from "react";
import * as previewService from "../services/preview.service";

const useGetPreviewPdf = () => {

    const [pdfDocument, setPdfDocument] = useState({});
    const [isLoading, setIsLoading] = useState(false);
    const [succeeded, setSucceeded] = useState(false);
    const [message, setMessage] = useState("");
    const controllerRef = useRef(null);

    const getPreviewPdf = async (planningId) => {

        if (controllerRef.current) {
            controllerRef.current.abort();
        }
        controllerRef.current = new AbortController();

        setIsLoading(true);
        setSucceeded(false);

        try {
          const response = await previewService.getPreviewPdf(planningId, controllerRef.current.signal);
          setSucceeded(true);
          setPdfDocument(response);
        } catch (error) {
          if (!axios.isCancel(error)) {
          setSucceeded(false);
          setMessage("An error occurred while getting PDF preview");
          }
          return Promise.resolve({});
        }
    
        setIsLoading(false);
      };
      // Abort the request when the component unmounts
      useEffect(() => {
        return () => {
          if (controllerRef.current) {
              controllerRef.current.abort();
          }
        };
      }, []);
    
    return {
        isLoading,
        succeeded,
        message,
        pdfDocument,
        getPreviewPdf
    }
}

export default useGetPreviewPdf;