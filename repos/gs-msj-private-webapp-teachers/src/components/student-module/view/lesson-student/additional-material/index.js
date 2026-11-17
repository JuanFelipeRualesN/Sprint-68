import React, { useState, useEffect } from "react";
import "./style.css";
import { getMaterialAdditionalByLesson } from "../../../../../api/athenas";
import styled from "styled-components";
import { useHistory } from "react-router-dom";
import LessonTitle from "../lesson-title";
import {Translate} from "react-localize-redux";
import { getAsset } from "../../../../../utils/getAssets";

const EmptyListStyled = styled.p`
  color: #ccc;
  display: block;
  justify-content: center;
  text-align: center;
  margin: 3em auto;

  & i {
    display: block;
    margin: 1em auto 0.25em auto;
    font-size: 45px;
    width: 175px;
    opacity: .5;
  }
`;

const Material = ({ item }) => {
  const history = useHistory();
  const sp = item.AdditionalMaterialModel.Url.split("/");
  const file = sp[sp.length - 1];
  const ext = getFileExtension(file);

  function assignImage(ext) {
    switch (ext) {
      case "epub":
        return "Lessons-icon-AdditionalDigital";
      case "doc":
        return "Lessons-icon-Word";
      case "docx":
        return "Lessons-icon-Word";
      case "mp3":
        return "Lessons-icon-Youtube";
      case "mp4":
        return "Lessons-icon-Youtube";
      case "pdf":
        return "Lessons-icon-PDF";
      default:
        return "Lessons-icon-Web";
    }
  }

  const iconName = assignImage(ext);
  const imageLoad = getAsset(iconName);

  function getFileExtension(filename) {
    return filename.lastIndexOf(".") > -1 ? filename.substring(filename.lastIndexOf(".")) : "";
  }

  return (
    <div className="student-aditional-list-w-icon__content">
      <div className="student-aditional-list__right">
        <img src={imageLoad} />
      </div>
      <div className="student-aditional-list__left">
        <p className="student-aditional-list__txt-ppal">
          {ext === "epub" ? (
            <a
              onClick={() =>
                history.push(
                  `/students/library/${item.AdditionalMaterialModel.Id}`
                )
              }
            >
              {" "}
              {item.AdditionalMaterialModel.Title}{" "}
            </a>
          ) : (
            <a href={item.AdditionalMaterialModel.Url} target="_blank">
              {" "}
              {item.AdditionalMaterialModel.Title}{" "}
            </a>
          )}
        </p>
        <p>{item.AdditionalMaterialModel.Description}</p>
      </div>
    </div>
  );
};

const AdditionalMaterial = ({
  lesson,
  kids,
  subject,
  lessonId,
  setLoading,
  loading,
}) => {

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [count, setCount] = useState(0);
  const [materials, setMaterials] = useState([]);

  useEffect(() => {
    getMaterialAdditional();
  }, []);

  const getMaterialAdditional = async () => {
    setLoading(true);
    try {
      const res = await getMaterialAdditionalByLesson(lessonId, page, pageSize);
      const data = res.data;
      setMaterials(data.AdditionalMaterials);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  let className = "student-wrapper-iframe";
  if (kids) {
    className = "student-wrapper-iframe-kids";
  }
  return (
    <>
      <div className="student-wrapper-student-content">
        <div className="student-center-container">
          <div className={className}>
            {lesson && <LessonTitle subject={subject} lesson={lesson} />}
            <div className="student-wrapper-aditional-student-list-w-icon">
              {materials.length !== 0 &&
                materials.map((item, i) => <Material key={i} item={item} />)}
            </div>

            {!loading && materials.length === 0 && (
              <EmptyListStyled>
                <i className="fas fa-book-reader"></i>
                <Translate id="student.empty-additional-material"
                           options={{renderInnerHtml: true}}/>
              </EmptyListStyled>
            )}
          </div>
        </div>
      </div>
    </>
  );
};

export default AdditionalMaterial;
