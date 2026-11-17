import React, { useState, useEffect } from "react";
import Epub from "../epub";
import Pdf from "../pdf";
import { useParams, useHistory } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { getMaterialAdditionalById } from "../../../../api/lesson";
import { Loader } from "../../../common/styled-components/loader";
import { getFileExtension } from "../../../../utils/file";


const BookDetail = () => {
  const { bookId } = useParams();
  const history = useHistory();
  const [material, setMaterial] = useState({});
  const [loading, setLoading] = useState(false);
  const [ext, setExt] = useState("");

  useEffect(() => {
    getBookById();
  }, []);

  const getBookById = async () => {
    setLoading(true);
    try {
      const res = await getMaterialAdditionalById(bookId);
      const data = res.data;
      const sp = data.Url.split("/");
      const file = sp[sp.length - 1];
      const ext = getFileExtension(file);
      setExt(ext);
      setMaterial(data);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <div>
            <button className="student-btn-back" onClick={() => history.push("/students/library")}>
              <i className="fas fa-arrow-left"></i>
              <Translate id="student.go-to-library"
                         options={{renderInnerHtml: true}}/>
            </button>
          </div>
          {material.ForDownload === "0" && ext === "epub" && (
            <Epub book={material} />
          )}
          {material.ForDownload === "0" && ext === "pdf" && (
            <Pdf book={material} />
          )}
          {loading && <Loader />}
        </div>
      </div>
    </>
  );
};

export default BookDetail;
