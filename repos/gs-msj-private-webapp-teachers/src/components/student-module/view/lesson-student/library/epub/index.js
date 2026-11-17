import React,{useRef} from "react";
import "./style.css";
import { ReactReader } from "react-reader";

const Epub = ({ book }) => {
  return (
    <>
    <div className="student-book-title">
     <h2>
       {book.Title}
     </h2>
   </div>
      <div
        className="book"
        style={{ position: "relative", width: "100%", height: "90vh" }}
      >
        {" "}
        <ReactReader
          url={book.Url}
          showToc={false}
          location={"epubcfi(/6/2[cover]!/6)"}
        />
      </div>
    </>
  );
};

export default Epub;
