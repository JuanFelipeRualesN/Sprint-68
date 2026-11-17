import React, { useEffect, useState } from "react";
import PreviewTableHeader from "./preview-table-header";
import PreviewRowQuiz from "./preview-row-quiz";
import AdditionalContentRows from "./preview-row-additional-content";
import PreviewTableProvider from "../contexts/preview-table-provider";
import useGetLessonDetail from "../hooks/useGetLessonDetail";
import useGetForumsForPreview from "../hooks/useGetForumsForPreview";
import useGetQuizzesForPreview from "../hooks/useGetQuizzesForPreview";
import useGetIntegrationsForPreview from "../hooks/useGetIntegrationsForPreview";
import useGetProjectsForPreview from "../hooks/useGetProjectsForPreview";
import useGetAccommodationsForPreview from "../hooks/useGetAccommodationsForPreview";
import useGetMaterialsForPreview from "../hooks/useGetMaterialsForPreview";
import useGetReflectionsForPreview from "../hooks/useGetReflectionsForPreview";
import useGetDifferentiationStrategiesForPreview from "../hooks/useGetDifferentiationStrategiesForPreview";
import useGetObservationsForPreview from "../hooks/useGetObservationsForPreview";
import useGetCommonExperiencesForPreview from "../hooks/useGetCommonExperiencesForPreview";
import useGetActiveResponsesForPreview from "../hooks/useGetActiveResponsesForPreview";
import PreviewRowEvaluation from "./preview-row-evaluation";
import LessonRows from "./preview-row-lesson";
import { getDatesBetween, translate, format } from "../../shared/utils";
import { Translate } from "react-localize-redux";
import useGetStrategiesForPreview from '../hooks/useGetStrategiessForPreview';
import useGetEvaluations from '../../evaluations/hooks/use-get-evaluations';
import Loading from "../../shared/components/loading";
import { getOpenLessons } from "../../open-lessons/services/open-lessons.service";
import LanguageManager from "../../../../../custom-classes/language-manager";
import Settings from "../../../../../custom-classes/settings";

/// The MED Quindío lesson template has no cross-curricular themes, so that section is
/// left out of the preview for that version only.
const HIDDEN_SECTIONS = Settings.IS_MED_QUINDIO ? ["topics"] : [];

export default function PreviewTable({ planning, filters, viewAsPDF = false }) {
  const { getManyLessonDetail, lessonsDetail } = useGetLessonDetail();
  const [headers, setHeaders] = useState([]);
  const [lessons, setLessons] = useState([]);
  const [customLessons, setCustomLessons] = useState([]);
  const [standards, setStandards] = useState([]);
  const [expectations, setExpectations] = useState([]);
  const [goals, setGoals] = useState([]);
  const [topics, setTopics] = useState([]);
  const [strategies, setStrategies] = useState([]);
  const [assignments, setAssignments] = useState([]);
  const [units, setUnits] = useState([]);
  const [introductoryActivity, setIntroductoryActivity] = useState([]);
  const [developmentActivity, setDevelopmentActivity] = useState([]);
  const [closingActivity, setClosingActivity] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const { forums, getForums } = useGetForumsForPreview();
  const { quizzes, getQuizzes } = useGetQuizzesForPreview();
  const { integrations, getIntegrations } = useGetIntegrationsForPreview();
  const { projects, getProjects } = useGetProjectsForPreview();
  const { accommodations, getAccommodations } = useGetAccommodationsForPreview();
  const { materials, getMaterials } = useGetMaterialsForPreview();
  const { reflections, getReflections } = useGetReflectionsForPreview();
  const { differentiationStrategies, getDifferentiationStrategies } = useGetDifferentiationStrategiesForPreview();
  const { observations, getObservations } = useGetObservationsForPreview();
  const { commonExperiences, getCommonExperience } = useGetCommonExperiencesForPreview();
  const { activeResponses, getActiveResponses } = useGetActiveResponsesForPreview();
  const { strategies: instructionalStrategy, getStrategies: getInstructionalStrategy } = useGetStrategiesForPreview();
  const { evaluations, getEvaluations } = useGetEvaluations();

  const mapLessons = () => {
    const lessons = planning?.lessons?.filter(
      (l) => l.lessonId === filters.lesson || !filters.lesson
    );
    if (!lessons || lessons.length <= 0) return [];

    const content = lessons?.flatMap((lesson) => {
      return lesson.dates.map((date) => ({
        id: lesson.id,
        lessonId: lesson.lessonId,
        type: "genial",
        title: translate("preview.genial-content"),
        description: lesson.title,
        date: format(date),
      }));
    });

    setLessons(content);
  };

  const mapAssignments = () => {
    const assignments = planning?.assignments;
    if (!assignments || assignments.length <= 0) return [];

    const content = assignments?.map((assignment) => ({
      id: assignment.id,
      type: "default",
      title: translate("preview.table-assignments"),
      description: assignment.title,
      date: format(assignment.dueDate),
    }));

    setAssignments(content);
  };

  const mapDates = (setData = (value) => {}, data) => {
    if (!data || data.length <= 0) return [];

    const content = data?.flatMap((item) => {
      const dates = lessons
        .filter((l) => l.lessonId === item.lessonId)
        .map((lesson) => lesson.date);
      return dates.map((date) => ({ ...item, date }));
    });

    setData(content);
  };

  const shouldShowRow = (name) => {
    if (HIDDEN_SECTIONS.includes(name)) return false;
    return filters.sections[0] === "" || filters.sections.includes(name);
  };

// Hide custom lessons when the filter is applied
const processData = (data) => {
  const filteredData = filters.lesson 
    ? data.filter(item => item.type !== "custom") 
    : data;
  
  // Remove duplicates
  const uniqueMap = new Map();
  filteredData.forEach(item => {
    const uniqueKey = `${item.id}-${item.date}`;
    
    if (!uniqueMap.has(uniqueKey)) {
      uniqueMap.set(uniqueKey, item);
    }
  });
  
  return Array.from(uniqueMap.values());
};

useEffect(() => {
  const loadData = async () => {
    try {
      setIsLoading(true);
      
      if (!planning || !planning.id) {
        return;
      }

      const apiPromises = [
        getForums(planning.id),
        getQuizzes(planning.id),
        getIntegrations(planning.id),
        getProjects(planning.id),
        getAccommodations(planning.id),
        getMaterials(planning.id),
        getReflections(planning.id),
        getInstructionalStrategy(planning.id),
        getEvaluations(planning.id),
        getOpenLessons(planning.id).then(customLessons => {
          setCustomLessons(customLessons);
          return customLessons;
        }),
        getDifferentiationStrategies(planning.id),
        getObservations(planning.id),
        getCommonExperience(planning.id),
        getActiveResponses(planning.id)
      ];

      await Promise.all(apiPromises);
      
      mapLessons();
      mapAssignments();

      const dates = getDatesBetween(
        new Date(planning.openDate),
        new Date(planning.closeDate)
      );
      const headers = dates.map((date, index) => ({
        day: `${translate("calendar.day")} ${index + 1}`,
        date: format(date),
      }));

      setHeaders(headers);
      
    } catch (error) {
      console.error("Error loading planning data:", error);
    } finally {
      setIsLoading(false);
    }
  };

    loadData();
}, [filters.lesson, planning, LanguageManager.getLanguage()]);

useEffect(() => {
  const fetchLessonDetails = async () => {
    try {
      setIsLoading(true);
      
      if (!lessons || lessons.length <= 0) return;
      
      const lessonsId = lessons.map((lesson) => lesson.lessonId);
      const ids = Array.from(new Set(lessonsId));

      await getManyLessonDetail(ids);
      
    } catch (error) {
      console.error("Error fetching lessons:", error);
    }
  };
  
  fetchLessonDetails();
}, [lessons]);

  useEffect(() => {
  if (!customLessons || customLessons.length <= 0) return;
  
  // Map custom lessons standards
  const customStandards = customLessons.flatMap(lesson => {
    const lessonStandards = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 1) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonStandards.map(standard => ({
        id: standard.Id,
        type: "custom",
        title: lesson.Title,
        description: standard.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });
  
  // Map custom lessons expectations
  const customExpectations = customLessons.flatMap(lesson => {
    const lessonExpectations = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 2) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonExpectations.map(expectation => ({
        id: expectation.Id,
        type: "custom",
        title: lesson.Title,
        description: expectation.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });
  
  // Map custom lessons strategies
  const customStrategies = customLessons.flatMap(lesson => {
    const lessonStrategies = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 3) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonStrategies.map(strategy => ({
        id: strategy.Id,
        type: "custom",
        title: lesson.Title,
        description: strategy.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });
  
  // Map custom lessons goals
  const customGoals = customLessons.flatMap(lesson => {
    const lessonGoals = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 4) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonGoals.map(goal => ({
        id: goal.Id,
        type: "custom",
        title: lesson.Title,
        description: goal.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });
  
  // Map custom lessons units
  const customUnits = customLessons.flatMap(lesson => {
    const lessonUnits = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 5) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonUnits.map(unit => ({
        id: unit.Id,
        type: "custom",
        title: lesson.Title,
        description: unit.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });

  // Map custom lessons topics
  const customTopics = customLessons.flatMap(lesson => {
    const lessonTopics = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 0) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonTopics.map(topic => ({
        id: topic.Id,
        type: "custom",
        title: lesson.Title,
        description: topic.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });

  // Map custom lessons introductory activity
  const customIntroActivity = customLessons.flatMap(lesson => {
    const lessonIntroActivity = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 6) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonIntroActivity.map(introActivity => ({
        id: introActivity.Id,
        type: "custom",
        title: lesson.Title,
        description: introActivity.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });

  // Map custom lessons development activity
  const customDevelopmentActivity = customLessons.flatMap(lesson => {
    const lessonDevelopmentActivity = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 7) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonDevelopmentActivity.map(developActivity => ({
        id: developActivity.Id,
        type: "custom",
        title: lesson.Title,
        description: developActivity.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });

  // Map custom lessons closing activity
  const customClosingActivity = customLessons.flatMap(lesson => {
    const lessonClosingActivity = lesson.TeacherPlanCustomLessonEducationalCriterias
      ?.filter(criteria => criteria.EducationalCriteria?.Type === 8) || [];
    
    return lesson.TeacherPlanCustomLessonDates.flatMap(dateObj => {
      return lessonClosingActivity.map(closingActivity => ({
        id: closingActivity.Id,
        type: "custom",
        title: lesson.Title,
        description: closingActivity.EducationalCriteria.Description,
        date: format(dateObj.ImplementationDate),
        lessonId: lesson.TeacherPlanCustomLessonId
      }));
    });
  });
  
  // Combine custom lessons data with genial lessons data
  setStandards(prevStandards => [...prevStandards, ...customStandards]);
  setExpectations(prevExpectations => [...prevExpectations, ...customExpectations]);
  setStrategies(prevStrategies => [...prevStrategies, ...customStrategies]);
  setGoals(prevGoals => [...prevGoals, ...customGoals]);
  setTopics(prevTopics => [...customTopics]);
  setUnits(prevUnits => [...prevUnits, ...customUnits]);
  setIntroductoryActivity(prevIntroActivity => [...prevIntroActivity, ...customIntroActivity]);
  setDevelopmentActivity(prevDevActivity => [...prevDevActivity, ...customDevelopmentActivity]);
  setClosingActivity(prevClosingActivity => [...prevClosingActivity, ...customClosingActivity]);
}, [customLessons]);

useEffect(() => {
  if (!lessonsDetail || lessonsDetail.length === 0) return;

  // Map the data for both genial and custom lessons
  const mapData = (setState = (value) => {}, key) => {
    const data = lessonsDetail.flatMap((detail) => detail[key]);
    mapDates((mappedData) => {
      setState(prevData => {
        const customPrevData = prevData.filter(item => item.type === "custom");
        return [...customPrevData, ...mappedData];
      });
    }, data);
  };

  mapData(setStandards, "standards");
  mapData(setExpectations, "expectations");
  mapData(setUnits, "units");
  mapData(setGoals, "goals");
  mapData(setTopics, "topics");
  mapData(setStrategies, "strategies");
}, [lessonsDetail]);

  const tableHasContent = () => {
    return (
      lessons.length > 0 ||
      customLessons.length > 0 ||
      standards.length > 0 ||
      expectations.length > 0 ||
      strategies.length > 0 ||
      goals.length > 0 ||
      units.length > 0 ||
      topics.length > 0 ||
      introductoryActivity.length > 0 ||
      developmentActivity.length > 0 ||
      closingActivity.length > 0 ||
      quizzes.length > 0 ||
      forums.length > 0 ||
      integrations.length > 0 ||
      projects.length > 0 ||
      evaluations.length > 0 ||
      accommodations.length > 0 ||
      materials.length > 0 ||
      assignments.length > 0 ||
      instructionalStrategy.length > 0 ||
      reflections.length > 0 ||
      differentiationStrategies.length > 0 ||
      observations.length > 0 ||
      commonExperiences.length > 0 ||
      activeResponses.length > 0
    );
  } 

  const renderEmptyRows = () => (
    Array.from({ length: 8 }).map((_, index) => (
      <tr class="RowActions" style={{ height: "60px" }}>
        <th/>
        {headers.map(() => (
          <td/>
        ))}     
      </tr>
    ))
  )

  return (
    <>
      {isLoading && <Loading type="table" show={isLoading} />}
      {!isLoading && <div className={`wrapper-tablePDF ${viewAsPDF && "table-pdf-preview"}`}>
      <div className="tablePDF__scroll">
        <table className="tablePDF__content">
          <PreviewTableProvider headers={headers} viewAsPDF={viewAsPDF}>
            <PreviewTableHeader />
            <tbody>
              {tableHasContent() ? (
                <>
                  {shouldShowRow("lessons") && (lessons.length > 0 || customLessons.length > 0)
                    && <LessonRows lessons={lessons} customLessons={filters.lesson ? [] : customLessons}/>
                  }
                  {shouldShowRow("units") && !viewAsPDF && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-units" />}
                      data={processData(units)}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("topics") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-topics" />}
                      data={processData(topics)}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("standards") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-standards" />}
                      data={processData(standards)}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("expectations") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-expectations" />}
                      data={processData(expectations)}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("goals") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-goals" />}
                      data={processData(goals)}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("strategies") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-strategies" />}
                      data={processData(strategies)}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  <AdditionalContentRows
                    title={<Translate id="preview.table-introductory-activity" />}
                    data={processData(introductoryActivity)}
                    onAdd={(date) => console.log(date)}
                    onEdit={(id) => console.log(id)}
                  />
                  <AdditionalContentRows
                    title={<Translate id="preview.table-development-activity" />}
                    data={processData(developmentActivity)}
                    onAdd={(date) => console.log(date)}
                    onEdit={(id) => console.log(id)}
                  />
                  <AdditionalContentRows
                    title={<Translate id="preview.table-closing-activity" />}
                    data={processData(closingActivity)}
                    onAdd={(date) => console.log(date)}
                    onEdit={(id) => console.log(id)}
                  />
                  {shouldShowRow("quizzes") && <PreviewRowQuiz quizzes={quizzes} />}
                  {shouldShowRow("forums") && !viewAsPDF && (
                    <AdditionalContentRows
                    title={<Translate id="preview.table-forums" />}
                    addButtonTitle={<Translate id="preview.table-add" />}
                    disclaimer={<Translate id="preview.table-disclaimer" />}
                    data={processData(forums)}
                    onAdd={(date) => console.log(date)}
                    onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("assignments") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-assignments" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={assignments}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("integrations") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-integrations" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={integrations}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("projects") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-projects" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={projects}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("evaluations") && (
                    <PreviewRowEvaluation
                      quizzes={quizzes}
                      forums={forums}
                      evaluations={evaluations}
                    />
                  )}
                  {shouldShowRow("accommodations") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-accommodations" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={accommodations}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("instructional-strategies") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-accommodations" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={instructionalStrategy}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("differentiation-strategies") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-differentiation-strategies" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={differentiationStrategies}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("active-responses") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-active-responses" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={activeResponses}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("common-experiences") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-common-experiences" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={commonExperiences}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("materials") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-materials" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={materials}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("observations") && (
                    <AdditionalContentRows
                    title={<Translate id="preview.table-observations" />}
                    addButtonTitle={<Translate id="preview.table-add" />}
                    data={observations}
                    onAdd={(date) => console.log(date)}
                    onEdit={(id) => console.log(id)}
                    />
                  )}
                  {shouldShowRow("reflections") && (
                    <AdditionalContentRows
                      title={<Translate id="preview.table-reflections" />}
                      addButtonTitle={<Translate id="preview.table-add" />}
                      data={reflections}
                      onAdd={(date) => console.log(date)}
                      onEdit={(id) => console.log(id)}
                    />
                  )}
                </>
              ) 
              : 
                renderEmptyRows()
              }
            </tbody>
          </PreviewTableProvider>
        </table>
      </div>
    </div>}
    </>
  );
}
