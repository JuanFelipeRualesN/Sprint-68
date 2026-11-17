import { Document, Page, pdfjs } from 'react-pdf';
import React,{useState } from "react";
import { Loader } from "../../common/styled-components/loader";
import { useHistory, useLocation } from "react-router-dom";
import { useEffect } from 'react';
import "../pdf/style.css"

pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;

function useQuery() {
  return new URLSearchParams(useLocation().search);
}
function isNumeric(str) {
  if (typeof str != "string") return false // we only process strings!  
  return !isNaN(str) && // use type coercion to parse the _entirety_ of the string (`parseFloat` alone does not do this)...
         !isNaN(parseFloat(str)) // ...and ensure strings of whitespace fail
}

const Pdf = ({book}) => {

//pagination by url
let query = useQuery();
let pageFromQuery = query.get("page");
let history = useHistory();

const [numPages,setNumPages] = useState(0),
      [loading,setLoading] = useState(true),
      [pageNumber,setPageNumber] = useState(1);

useEffect(() => {
  if(isNumeric(pageFromQuery)) {
    setPageNumber(parseInt(pageFromQuery));
  }
})
const setPageNumberToQuery = (page) => {
  history.push(`/students/library/${book.Id}?page=${page}`);
}

const nextPage = () => {
 if (pageNumber < numPages) {
     setPageNumberToQuery(pageNumber + 1);
     setPageNumber(pageNumber + 1);
     
   }
};
const prevPage = () => {
 if (pageNumber > 1) {
    setPageNumberToQuery(pageNumber - 1);
    setPageNumber(pageNumber - 1);
   }
 };

const onDocumentLoadSuccess = (pages) => {
    setLoading(false);
    setNumPages(pages._pdfInfo.numPages);
}
    return (
      <>
   <div className="student-book-title">
     <h2>
       {book.Title}
     </h2>
   </div>
   <div className="student-pdfContainer">
        <Document
           file={book.Url}
           onLoadSuccess={onDocumentLoadSuccess}
           onContextMenu={(e) => e.preventDefault()}
           className="pdf-container">
        <Page pageNumber={pageNumber} />
        </Document>
    </div>
      {!loading && <div className="student-pageControls">
          <button disabled={pageNumber === 1} onClick={prevPage} className="btn student-btn-primary">
            <i className="fas fa-arrow-left"></i>
            </button>
                  {pageNumber}/{numPages}
          <button disabled={pageNumber === numPages} onClick={nextPage} className="btn student-btn-primary">
            <i className="fas fa-arrow-right"></i>
          </button>
      </div>}
      {loading && <Loader />}
    </>
    )
}

export default Pdf;