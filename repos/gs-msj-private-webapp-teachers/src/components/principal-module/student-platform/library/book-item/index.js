import React from "react";
import { useHistory } from "react-router-dom";
import { getLanguage } from "../../../../student-module/view/lesson-student/utils/utils";
import { getGradesOrdinals } from "../../../../student-module/view/lesson-student/utils/lesson";
import { getFileExtension } from "../../../../student-module/view/lesson-student/utils/file";
import { getAsset } from "../../../../../utils/getAssets";

const IconLibrary = getAsset("Library-icon-Cover");

const BookItem = ({ book }) => {
  const history = useHistory();
  const sp = book.Url.split("/");
  const lastSegment = sp[sp.length - 1];
  const file = lastSegment.includes("#:~:") ? "" : lastSegment;
  const ext = getFileExtension(file);

  let titleComponent;
  if (book.ForDownload === "0" && ext === "epub") {
    titleComponent = (
      <a onClick={() => history.push(`/students/library/${book.Id}`)}>
        {" "}
        {book.Title}{" "}
      </a>
    );
  } else if (book.ForDownload === "0" && ext === "pdf") {
    titleComponent = (
      <a onClick={() => history.push(`/students/library/${book.Id}`)}>
        {" "}
        {book.Title}{" "}
      </a>
    );
  } else {
    titleComponent = (
      <a href={book.Url} target="_blank">
        {" "}
        {book.Title}{" "}
      </a>
    );
  }

  return (
    <div className="four-col">
      <div className="general-module">
        <img
          className="image-library"
          src={
            book.UrlCover
              ? book.UrlCover
              : IconLibrary
          }
          alt=""
        />
        <p className="list-library__title">{titleComponent}</p>
        <p className="list-library__description">{book.Description}</p>
        <p className="list-library__aditional">
          {getGradesOrdinals(book.Level, getLanguage())} {" "}
        </p>
      </div>
    </div>
  );
};

export default BookItem;