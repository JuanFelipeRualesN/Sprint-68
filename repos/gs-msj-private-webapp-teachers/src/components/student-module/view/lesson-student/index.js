import React, { useState, useEffect } from "react";
import "./style.css";
import Concept from "./concept";
import LessonMenu from "./lesson-menu";
import Vocabulary from "./vocabulary";
import Example from "./example";
import Challenge from "./challenge";
import Practice from "./practice";
import DownloadLesson from "./download-lesson";
import AdditionalMaterial from "./additional-material";
import { getLessonById } from "../../../../api/gsweb";
import {useLocation, useHistory, useParams, withRouter} from "react-router-dom";
import {Loader} from "./common/styled-components/loader";
import {renderToStaticMarkup} from "react-dom/server";
import {withLocalize } from "react-localize-redux";
import useVersionedTranslations from "../../../../hooks/useVersionedTranslations";

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const LessonDetail = () => {
  let query = useQuery();
  let { lessonId } = useParams();
  const history = useHistory();
  let location = useLocation();

  const [lesson, setLesson] = useState(null);
  const [subject, setSubject] = useState(null);
  const [loading, setLoading] = useState(false);
  const [tabActive, setTabActive] = useState(query.get("tab") || "concept");
  const [fromLearningGap, setFromLearningGap] = useState(query.get("fromLearningGap") || false);
  const [studentId, setStudentId] = useState(query.get("studentId") || null);
  const [studentName, setStudentName] = useState(query.get("studentName") || null);

  // Initialize translations based on the current version
  useVersionedTranslations();

  useEffect(() => {

    if (lessonId) {
      getDetailModelLesson(lessonId);
    }
  }, [lessonId]); // Include lessonId as a dependency

  const getDetailModelLesson = async (lessonId) => {
    setLoading(true);
    try {
      const res = await getLessonById(lessonId);
      const data = res.data;
      setSubject(res.data.SubjectCode);
      setLesson(data);
      setLoading(false);
    } catch {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (query.get("tab")) {
      const params = new URLSearchParams(location.search);

      // Preserve the important params
      const studentId = params.get("studentId");
      const studentName = params.get("studentName");
      const fromLearningGap = params.get("fromLearningGap");

      // Remove the "tab" param
      params.delete("tab");

      // Re-append preserved params if they exist
      const preservedParams = new URLSearchParams();
      if (studentId) preservedParams.set("studentId", studentId);
      if (studentName) preservedParams.set("studentName", studentName);
      if (fromLearningGap) preservedParams.set("fromLearningGap", fromLearningGap);

      history.push({
        pathname: location.pathname,
        search: preservedParams.toString(),
      });
    }
  }, [tabActive]);

  // Prevent rendering until subject is available
  if (!subject) {
    return <Loader />; // or return null;
  }

  return (
    <>
      <LessonMenu tabActive={tabActive} setTabActive={setTabActive}
                  fromLearningGap={fromLearningGap}
                  studentId={studentId}
                  studentName={studentName}
      />
      {tabActive === "concept" && (
        <Concept
          lesson={lesson}
          kids={false}
          subject={subject}
          lessonId={lessonId}
          setLoading={setLoading}
          loading={loading}
        />
      )}
      {tabActive === "vocabulary" && (
        <Vocabulary
          lesson={lesson}
          kids={true}
          subject={subject}
          lessonId={lessonId}
          setLoading={setLoading}
          loading={loading}
        />
      )}
      {tabActive === "example" && (
        <Example
          lesson={lesson}
          kids={true}
          subject={subject}
          lessonId={lessonId}
          setLoading={setLoading}
          loading={loading}
        />
      )}
      {tabActive === "practice" && (
        <Practice
          lesson={lesson}
          kids={true}
          subject={subject}
          lessonId={lessonId}
          setLoading={setLoading}
          loading={loading}
          fromLearningGap={fromLearningGap}
            studentId={studentId}
          studentName={studentName}
        />
      )}
      {tabActive === "challenge" && (
        <Challenge
          lesson={lesson}
          kids={true}
          subject={subject}
          lessonId={lessonId}
          setLoading={setLoading}
          loading={loading}
          fromLearningGap={fromLearningGap}
            studentId={studentId}
            studentName={studentName}
        />
      )}
      {tabActive === "additional" && (
        <AdditionalMaterial
          lesson={lesson}
          kids={true}
          subject={subject}
          lessonId={lessonId}
          setLoading={setLoading}
          loading={loading}
        />
      )}
      {tabActive === "download" && (
        <DownloadLesson
          lesson={lesson}
          kids={true}
          subject={subject}
          lessonId={lessonId}
          setLoading={setLoading}
          loading={loading}
        />
      )}
      {loading && <Loader />}
    </>
  );
};

export default withRouter(withLocalize(LessonDetail));
