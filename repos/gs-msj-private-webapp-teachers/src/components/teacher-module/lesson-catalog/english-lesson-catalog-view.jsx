import React, { useState, useEffect, useRef } from "react";
import NavbarClients from "../../shared-module-components/navbar/navbar-clients";
import Menu from "../../shared-module-components/menu";
import LessonView from "./lesson-view/lesson-view";
import { Translate, withLocalize, LocalizeProvider } from "react-localize-redux";
import { getAthenasSubjects, getAthenasGrades, getAthenasStandards, getAthenasExpectations, getAthenasAdvancedLessons, getTeacherPlannings } from "../../../api/teachers";
import { getProfiles } from "../../../api/users";
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import {EnglishLessonFiltersBar} from "./english-lesson-filters-bar";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { KeyboardArrowDown, RemoveRedEyeSharp, MenuBook } from "@material-ui/icons";
import LearningGapLessonView from "../learning-gap/learning-gap-lessons/learning-gap-lesson-view/learning-gap-lesson-view";
import CreateInstructionalPlanModal from "../modal/modal-create-instructional-plan";
import ModalTeacherPlanningNameCreator from "../modal/modal-teacher-planning-name-creator";
import ModalPlanningAttachItem from "../modal/modal-planning-attach-item";
import ModalSuccess from "../modal/modal-success";
import LearningGapAddLessonForm from "../learning-gap/learning-gap-lessons/learning-gap-add-lesson-form/learning-gap-add-lesson-form";
import { Modal, ModalBody, ModalHeader } from 'reactstrap';
import { makeStyles } from "@material-ui/core/styles";
import LessonSkeleton from "./lesson-skeleton/lesson-skeleton";
import Settings from "../../../custom-classes/settings";
import SessionManager from "../../../custom-classes/session-manager";
import { getSubscriptionCourses } from "../../../api/subscriptions";
import { Provider } from "react-redux";
import store from "../../../redux/store";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from "../../../utils/access_logger";

// Subject codes for english content carry the '-en' suffix (mat-en, cie-en, ...)
const ENGLISH_SUFFIX = '-en';
const isEnglishSubject = (code = '') => code.toLowerCase().endsWith(ENGLISH_SUFFIX);

const INK = '#16233A';
const SLATE = '#456083';
const MARIGOLD = '#F2A03D';
const CANVAS = '#F5F7FA';
const LINE = '#E3E8EF';

const useStyles = makeStyles({
  page: {
    backgroundColor: CANVAS,
    minHeight: '100vh',
    paddingBottom: '48px',
  },
  hero: {
    position: 'relative',
    overflow: 'hidden',
    backgroundColor: INK,
    color: '#FFFFFF',
    padding: '38px 24px 34px',
  },
  // Signature: oversized EN monogram bled off the right edge of the hero
  monogram: {
    position: 'absolute',
    right: '-14px',
    top: '50%',
    transform: 'translateY(-50%)',
    fontSize: '190px',
    fontWeight: 800,
    lineHeight: 0.8,
    letterSpacing: '-0.05em',
    color: 'rgba(255,255,255,0.055)',
    userSelect: 'none',
    pointerEvents: 'none',
  },
  eyebrow: {
    display: 'inline-flex',
    alignItems: 'center',
    gap: '8px',
    fontSize: '11px',
    fontWeight: 700,
    letterSpacing: '0.16em',
    textTransform: 'uppercase',
    color: MARIGOLD,
    marginBottom: '12px',
  },
  eyebrowDot: {
    width: '6px',
    height: '6px',
    borderRadius: '50%',
    backgroundColor: MARIGOLD,
  },
  heroTitle: {
    position: 'relative',
    fontSize: '38px',
    lineHeight: 1.1,
    fontWeight: 800,
    letterSpacing: '-0.02em',
    margin: 0,
  },
  heroSub: {
    position: 'relative',
    marginTop: '10px',
    maxWidth: '540px',
    fontSize: '15px',
    lineHeight: 1.5,
    color: 'rgba(255,255,255,0.72)',
  },
  heroLink: {
    position: 'relative',
    display: 'inline-flex',
    alignItems: 'center',
    gap: '6px',
    marginTop: '20px',
    fontSize: '13px',
    fontWeight: 700,
    color: '#FFFFFF',
    textDecoration: 'none',
    borderBottom: `2px solid ${MARIGOLD}`,
    paddingBottom: '2px',
    '&:hover': { color: MARIGOLD, textDecoration: 'none' },
  },
  resultsBar: {
    display: 'flex',
    flexWrap: 'wrap',
    alignItems: 'baseline',
    justifyContent: 'space-between',
    gap: '12px',
    padding: '0 24px',
    marginBottom: '18px',
  },
  count: {
    fontSize: '15px',
    fontWeight: 700,
    color: INK,
  },
  countMuted: {
    color: '#8A97A8',
    fontWeight: 600,
  },
  sortGroup: {
    display: 'flex',
    alignItems: 'center',
    gap: '4px',
    fontSize: '13px',
    color: SLATE,
  },
  sortBtn: {
    display: 'inline-flex',
    alignItems: 'center',
    cursor: 'pointer',
    padding: '4px 10px',
    borderRadius: '999px',
    fontWeight: 600,
    userSelect: 'none',
    transition: 'background-color .15s ease',
    '&:hover': { backgroundColor: '#EAEEF4' },
  },
  sortActive: {
    backgroundColor: INK,
    color: '#FFFFFF',
    '&:hover': { backgroundColor: INK },
  },
  grid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fill, minmax(270px, 1fr))',
    gap: '20px',
    padding: '0 24px',
  },
  empty: {
    gridColumn: '1 / -1',
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    gap: '10px',
    padding: '70px 20px',
    backgroundColor: '#FFFFFF',
    border: `1px dashed ${LINE}`,
    borderRadius: '16px',
    color: '#8A97A8',
    fontWeight: 600,
  },
  pagination: {
    marginTop: '32px',
    padding: '0 24px',
  },
});

// English-only version of the lesson catalog
const EnglishLessonCatalogView = () => {
  const classes = useStyles();
  const sessionDetails = SessionManager.getSessionDetails();
  const [subjects, setSubjects] = useState([]);
  const [grades, setGrades] = useState([]);
  const [standards, setStandards] = useState([]);
  const [expectations, setExpectations] = useState([]);
  const [lessons, setLessons] = useState([]);
  const [loading, setLoading] = useState(false);
  const [lessonsLimiter] = useState(12);
  const [studentsLimiter] = useState(10);
  const [fromPage] = useState(0);
  const [currentPage, setCurrentPage] = useState(0);
  const [selectedLesson, setSelectedLesson] = useState(null);
  const [plannings, setPlannings] = useState([]);
  const [students, setStudents] = useState([]);
  const [totalItems, setTotalItems] = useState(null);
  const [firstLoad, setFirstLoad] = useState(true);
  const [isCalledFromFilters, setIsCalledFromFilters] = useState(false);
  const [availableCourses, setAvailableCourses] = useState([]);
  const [userRole] = useState(SessionManager.getUserRole() || Settings.ROLE_TEACHER_STR);

  // Every english subject code available to this user. Used as the implicit
  // subject filter whenever the teacher has not picked specific subjects.
  const englishCodesRef = useRef([]);

  const [filters, setFilters] = useState({
    subjects: [],
    grades: [],
    standards: [],
    expectations: [],
    creationDate: null,
    titleSort: null,
  });
  const [searchFilters, setSearchFilters] = useState({
    subjects: [],
    grades: [],
    standards: [],
    expectations: [],
    creationDate: null,
    titleSort: null,
  });
  const [inputValues, setInputValues] = useState({ content: '', code: '' });
  const [searchInputValues, setSearchInputValues] = useState({ content: '', code: '' });
  const [modals, setModals] = useState({
    lessonDetails: false,
    noPlans: false,
    createPlan: false,
    assignPlan: false,
    assignAvanza: false,
    success: false,
  });

  useVersionedTranslations();

  // Never send an empty subject list: that would return spanish content too
  const resolveSubjectCodes = (selected, fallbackCodes) =>
      selected && selected.length > 0 ? selected : (fallbackCodes ?? englishCodesRef.current);

  useEffect(() => {
    try {
      SendAccessLog({
        Username: sessionDetails.Username,
        InvitationCode: sessionDetails.InvitationCode,
        Role: sessionDetails.Roles[0],
        AreaId: "12"
      });
    } catch (error) {
      // empty catch block to avoid breaking the application
    }
  }, []);

  useEffect(() => {
    const fetchAllData = async () => {
      setLoading(true);
      try {
        setSearchInputValues({ ...inputValues });
        setSearchFilters({ ...filters });

        // Subjects must resolve before the first lesson call so the request is
        // scoped to english content from the very first render
        const englishCodes = await callSubjects();

        await Promise.all([
          callGrades(),
          callStandards(),
          callExpectations(),
          callLessons(false, englishCodes),
        ]);

        const availableCoursesResponse = await getSubscriptionCourses();
        setAvailableCourses(availableCoursesResponse.data);

        setFirstLoad(false);
      } finally {
        setLoading(false);
      }
    };
    fetchAllData();
  }, []);

  useEffect(() => {
    if (!firstLoad && !isCalledFromFilters) {
      callLessons();
    }
  }, [currentPage, filters.creationDate, filters.titleSort]);

  const isLessonAvailable = (lesson) => {
    if (!availableCourses || availableCourses.length === 0) return false;
    const lessonSubject = lesson.LessonModel.SubjectCode;
    const lessonLevel = lesson.LessonModel.LevelCode;
    return availableCourses.some(course =>
        course.Subject === lessonSubject && course.Level === lessonLevel
    );
  };

  const clearAndFetchLessons = () => {
    setFilters({
      subjects: [],
      grades: [],
      standards: [],
      expectations: [],
      creationDate: null,
      titleSort: null,
    });
    setInputValues({ content: '', code: '' });
    setCurrentPage(0);
    callLessonsWithClearedFilters();
  };

  // Clearing returns to "all english content", not to all content
  const callLessonsWithClearedFilters = async () => {
    setLoading(true);
    try {
      const payload = {
        "SubjectCodes": englishCodesRef.current,
        "LevelCodes": [],
        "StandardCodes": [],
        "ExpectationCodes": [],
        "PageNumber": 0,
        "ContentFilter": '',
        "Limit": lessonsLimiter,
        "OrderByCreationDate": null,
        "TeacherInputText": '',
        "OrderByTitle": null,
      };

      const response = await getAthenasAdvancedLessons(payload);

      setSearchFilters({
        subjects: [],
        grades: [],
        standards: [],
        expectations: [],
        creationDate: null,
        titleSort: null,
      });
      setSearchInputValues({ content: '', code: '' });

      setLessons(response.data.LessonsDetails.Lessons);
      setTotalItems(response.data.LessonsDetails.TotalResults);
    } catch (error) {
      console.error("Error fetching lessons:", error);
    } finally {
      setLoading(false);
    }
  };

  const callLessons = async (fromFilters = false, englishCodesOverride = null) => {
    if (fromFilters) {
      setIsCalledFromFilters(true);
    }
    setLoading(true);
    try {
      const filtersToUse = fromFilters ? filters : searchFilters;
      const subjectCodes = resolveSubjectCodes(filtersToUse.subjects, englishCodesOverride);
      const gradeCodes = filtersToUse.grades;
      const standardCodes = filtersToUse.standards;
      const expectationCodes = filtersToUse.expectations;

      const contentFilter = fromFilters ? inputValues.content : searchInputValues.content;
      const codeFilter = fromFilters ? inputValues.code : searchInputValues.code;
      const pageToUse = fromFilters ? 0 : currentPage;

      if (fromFilters) {
        setCurrentPage(0);
      }

      if (contentFilter && contentFilter.trim() !== '') {
        setFilters(prev => ({ ...prev, titleSort: null }));
      }

      const payload = {
        "SubjectCodes": subjectCodes,
        "LevelCodes": gradeCodes,
        "StandardCodes": standardCodes,
        "ExpectationCodes": expectationCodes,
        "PageNumber": pageToUse,
        "ContentFilter": contentFilter.trim() || '',
        "Limit": lessonsLimiter,
        "OrderByCreationDate": filters.creationDate,
        "TeacherInputText": codeFilter.trim() || '',
        "OrderByTitle": (contentFilter && contentFilter.trim() !== '') ? null : filters.titleSort,
      };

      const response = await getAthenasAdvancedLessons(payload);

      if (fromFilters) {
        setSearchFilters({ ...filters });
        setSearchInputValues({
          content: inputValues.content,
          code: inputValues.code
        });
      }
      setLessons(response.data.LessonsDetails.Lessons);
      setTotalItems(response.data.LessonsDetails.TotalResults);
      return response.data;
    } catch (error) {
      console.error("Error fetching lessons:", error);
    } finally {
      setLoading(false);
      setIsCalledFromFilters(false);
    }
  };

  const filterLessonsByCheckbox = async (newFilters) => {
    setLoading(true);
    setCurrentPage(0);
    setIsCalledFromFilters(true);
    try {
      const payload = {
        "SubjectCodes": resolveSubjectCodes(newFilters.subjects),
        "LevelCodes": newFilters.grades,
        "StandardCodes": newFilters.standards,
        "ExpectationCodes": newFilters.expectations,
        "PageNumber": 0,
        "ContentFilter": searchInputValues.content.trim() || '',
        "TeacherInputText": searchInputValues.code.trim() || '',
        "Limit": lessonsLimiter,
        "OrderByCreationDate": newFilters.creationDate,
        "OrderByTitle": (searchInputValues.content && searchInputValues.content.trim() !== '') ? null : newFilters.titleSort,
      };

      const response = await getAthenasAdvancedLessons(payload);

      setSearchFilters(newFilters);
      setLessons(response.data.LessonsDetails.Lessons);
      setTotalItems(response.data.LessonsDetails.TotalResults);
      setInputValues({
        content: searchInputValues.content,
        code: searchInputValues.code
      });
    } catch (error) {
      console.error("Error fetching lessons:", error);
    } finally {
      setLoading(false);
      setIsCalledFromFilters(false);
    }
  };

  // Fetch the subjects, keeping only the english ones
  const callSubjects = async () => {
    try {
      const response = await getAthenasSubjects();
      const formattedSubjects = response.data
          .filter(subject => subject.Name !== "")
          .map(subject => ({
            label: subject.Name,
            code: subject.Code,
          }));

      const subjectsToFilterOut = Settings.getSubjectsToFilterOut();
      const englishSubjects = formattedSubjects.filter(subject =>
          !subjectsToFilterOut.includes(subject.code) && isEnglishSubject(subject.code));

      const englishCodes = englishSubjects.map(subject => subject.code);
      englishCodesRef.current = englishCodes;

      setSubjects(englishSubjects);
      setFilters(prev => ({ ...prev, subjects: [] }));

      return englishCodes;
    } catch (error) {
      console.error("Error fetching subjects:", error);
      return [];
    }
  };

  const callGrades = async () => {
    try {
      const response = await getAthenasGrades();
      const formattedGrades = response.data?.filter(grade => grade.Name !== "")
          .map(grade => ({
            label: grade.Name,
            code: grade.Code,
          }));

      const levelsToFilterOut = Settings.getLevelsToFilterOut();
      const filteredGrades = formattedGrades.filter(grade =>
          !levelsToFilterOut.includes(grade.code));

      setGrades(filteredGrades);
      setFilters(prev => ({ ...prev, grades: [] }));
    } catch (error) {
      console.error(error);
    }
  };

  const callStandards = async () => {
    try {
      const response = await getAthenasStandards();
      const formattedStandards = response.data?.Items.filter(standard => standard.Description !== "")
          .map(standard => ({
            label: standard.Description,
            code: standard.Code,
            codeId: standard.Id,
          }));
      setStandards(formattedStandards);
      setFilters(prev => ({ ...prev, standards: [] }));
    } catch (error) {
      console.error("Error fetching standards:", error);
    }
  };

  const callExpectations = async () => {
    try {
      const response = await getAthenasExpectations();
      const formattedExpectations = response.data?.Items
          .map(expectation => ({
            label: expectation.Description,
            code: expectation.Code,
            codeId: expectation.Id,
          }));
      setExpectations(formattedExpectations);
      setFilters(prev => ({ ...prev, expectations: [] }));
    } catch (error) {
      console.error("Error fetching expectations:", error);
    }
  };

  const getPlannings = async (lesson) => {
    setSelectedLesson(lesson);
    setLoading(true);
    try {
      const response = await getTeacherPlannings();
      setPlannings(response.data);
      response.data.length === 0 ? openModal('noPlans') : openModal('assignPlan');
    } catch (error) {
      console.error("Error fetching plannings:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleAssignAvanzaLesson = async (lesson) => {
    setSelectedLesson(lesson);
    setLoading(true);
    openModal('assignAvanza');
    try {
      const response = await getProfiles(null, null, null, fromPage, studentsLimiter);
      setStudents(response.data.UserInfo);
    } catch (error) {
      console.error("Error fetching students:", error);
    } finally {
      setLoading(false);
    }
  };

  const toggleCreationDate = () => {
    setFilters(prev => ({
      ...prev,
      creationDate:
          prev.creationDate === null ? 'ASC' : prev.creationDate === 'ASC' ? 'DESC' : null,
    }));
  };

  const toggleTitleSort = () => {
    setFilters(prev => ({
      ...prev,
      titleSort:
          prev.titleSort === null ? 'ASC' : prev.titleSort === 'ASC' ? 'DESC' : null
    }));
    setInputValues(prev => ({ ...prev, content: '' }));
  };

  const handlePageChange = (page) => setCurrentPage(page - 1);

  const openModal = (modalName) => setModals(prev => ({ ...prev, [modalName]: true }));
  const closeModal = (modalName) => setModals(prev => ({ ...prev, [modalName]: false }));

  const handleOpenLessonDetailsModal = (lesson) => {
    setSelectedLesson(lesson);
    try {
      SendAccessLog({
        UserName: sessionDetails.Username,
        InvitationCode: sessionDetails.InvitationCode,
        Role: sessionDetails.Roles[0],
        AreaId: "14",
        Details: "Previewed lesson details in english catalog",
        LessonId: lesson.LessonModel.Id ?? null,
        LessonTitle: lesson.LessonModel.LessonTitle ?? null,
        Subject: lesson.LessonModel.SubjectCode ?? null,
        Grade: lesson.LessonModel.LevelCode ?? null,
      });
    } catch (error) {
      // Silently catch errors to avoid disrupting the user experience
    }
    openModal('lessonDetails');
  };

  const handleOpenCreatePlanModal = () => {
    closeModal('noPlans');
    openModal('createPlan');
  };

  const handleCloseCreatePlanModal = () => {
    closeModal('createPlan');
    closeModal('noPlans');
  };

  const handleCloseAssignLessonModal = () => closeModal('assignPlan');
  const handleSuccessModal = () => openModal('success');

  const sortArrow = (direction) =>
      direction === 'DESC'
          ? <KeyboardArrowDown fontSize="small" />
          : direction === 'ASC'
              ? <KeyboardArrowDown fontSize="small" style={{ transform: 'rotate(180deg)' }} />
              : null;

  return (
      <>
        {loading && <LoaderFullWindow />}
        <div>
          <NavbarClients />
          <Menu />

          <div className={`${classes.page} learning-gap-fade-in`}>
            <header className={classes.hero}>
              <span className={classes.monogram}>EN</span>
              <div className={classes.eyebrow}>
                <span className={classes.eyebrowDot} />
                <Translate id="lesson-catalog.english-content-area" options={{ renderInnerHtml: false }} />
              </div>
              <h1 className={classes.heroTitle}>
                <Translate id="lesson-catalog.english-catalog-title" options={{ renderInnerHtml: false }} />
              </h1>
              <p className={classes.heroSub}>
                <Translate id="lesson-catalog.english-catalog-subtitle" options={{ renderInnerHtml: false }} />
              </p>
              {Settings.IS_MED_QUINDIO ? null :
                <a
                    href={Settings.LESSON_CATALOG_PDF_URL}
                    target="_blank"
                    rel="noopener noreferrer"
                    className={classes.heroLink}
                >
                  <RemoveRedEyeSharp style={{ fontSize: '16px' }} />
                  <Translate id="lesson-catalog.show-catalog" options={{ renderInnerHtml: false }} />
                </a>
              }
            </header>

            <EnglishLessonFiltersBar
                subjects={subjects}
                grades={grades}
                standards={standards}
                expectations={expectations}
                filters={filters}
                setFilters={setFilters}
                inputValues={inputValues}
                setInputValues={setInputValues}
                filterLessons={() => callLessons(true)}
                filterByCheckbox={filterLessonsByCheckbox}
                clearAndFetch={clearAndFetchLessons}
                setStandards={setStandards}
                setExpectations={setExpectations}
            />

            <div className={classes.resultsBar}>
            <span className={classes.count}>
              {totalItems ?? 0}{' '}
              <span className={classes.countMuted}>
                <Translate id="lesson-catalog.lesson-results" options={{ renderInnerHtml: false }} />
              </span>
            </span>

              <div className={classes.sortGroup}>
              <span style={{ marginRight: '4px', fontWeight: 600 }}>
                <Translate id="lesson-catalog.lesson-sort" options={{ renderInnerHtml: false }} />
              </span>
                {filters.titleSort === null && (
                    <span
                        onClick={toggleCreationDate}
                        className={`${classes.sortBtn} ${filters.creationDate !== null ? classes.sortActive : ''}`}
                    >
                  <Translate id="lesson-catalog.lesson-creation-date" options={{ renderInnerHtml: false }} />
                      {sortArrow(filters.creationDate)}
                </span>
                )}
                {filters.creationDate === null && searchInputValues.content === '' && (
                    <span
                        onClick={toggleTitleSort}
                        className={`${classes.sortBtn} ${filters.titleSort !== null ? classes.sortActive : ''}`}
                    >
                  <Translate id="lesson-catalog.title" options={{ renderInnerHtml: false }} />
                      {sortArrow(filters.titleSort)}
                </span>
                )}
              </div>
            </div>

            <div className={classes.grid}>
              {loading && lessons.length === 0 ? <LessonSkeleton count={lessonsLimiter} /> : null}
              {lessons.length === 0 && !loading ? (
                  <div className={classes.empty}>
                    <MenuBook style={{ width: '64px', height: '64px', color: '#CBD5E1' }} />
                    <Translate id="lesson-catalog.no-lessons-found" options={{ renderInnerHtml: false }} />
                  </div>
              ) : (
                  lessons.map((lesson) => (
                      <LessonView
                          key={lesson.Id}
                          availableAssignAvanza={userRole === Settings.ROLE_TEACHER_STR}
                          availableAssignPlanning={userRole === Settings.ROLE_TEACHER_STR}
                          lesson={lesson}
                          lessonImage={lesson.LessonModel.LessonImg}
                          lessonTitle={lesson.LessonModel.LessonTitle}
                          lessonCode={lesson.ConstructedCodeHandleModelList[0]?.ConstructedCodeFull}
                          handleOpenLessonDetailsModal={handleOpenLessonDetailsModal}
                          handleAssignAvanzaLesson={() => handleAssignAvanzaLesson(lesson)}
                          setLoading={setLoading}
                          getPlannings={() => getPlannings(lesson)}
                          isAvailable={isLessonAvailable(lesson)}
                      />
                  ))
              )}
            </div>

            <div className={classes.pagination}>
              <PaginationCustom
                  currentPage={currentPage + 1}
                  totalPages={Math.ceil(totalItems / lessonsLimiter)}
                  updateParent={handlePageChange}
              />
            </div>
          </div>

          {/* Modal to show the lesson details */}
          <Modal
              className="modal-full-width"
              isOpen={modals.lessonDetails}
              toggle={() => closeModal('lessonDetails')}
          >
            <ModalHeader toggle={() => closeModal('lessonDetails')} />
            <ModalBody>
              <Provider store={store}>
                <LocalizeProvider store={store}>
                  <LearningGapLessonView lessonId={selectedLesson?.LessonModel?.Id} />
                </LocalizeProvider>
              </Provider>
            </ModalBody>
          </Modal>

          {/* Modal to show when the user has no instructional plans */}
          {modals.noPlans && (
              <CreateInstructionalPlanModal
                  handleOpenCreatePlanModal={handleOpenCreatePlanModal}
                  handleCloseCreatePlanModal={handleCloseCreatePlanModal}
                  openNoPlanModal={modals.noPlans}
                  closeModal={closeModal}
              />
          )}

          {/* Modal to create a plan */}
          {modals.createPlan && (
              <ModalTeacherPlanningNameCreator
                  visible={modals.createPlan}
                  updateParent={handleCloseCreatePlanModal}
                  isCatalog={true}
              />
          )}

          {/* Modal to assign a lesson to a plan */}
          {modals.assignPlan && (
              <ModalPlanningAttachItem
                  visible={modals.assignPlan}
                  isCatalog={true}
                  updateParent={handleCloseAssignLessonModal}
                  lessonId={selectedLesson?.LessonModel?.Id}
                  plannings={plannings}
                  handleSuccessModal={handleSuccessModal}
              />
          )}

          {/* Modal to assign a lesson to Avanza */}
          {modals.assignAvanza && (
              <Modal isOpen={modals.assignAvanza} centered className="modal-results">
                <ModalHeader toggle={() => closeModal('assignAvanza')}>
              <span style={{ color: '#2a426d', fontWeight: 'bold' }}>
                <Translate id="learning-gap.assign-lesson" options={{ renderInnerHtml: true }} />
              </span>
                  <br />
                  <span style={{ fontSize: 13 }}>
                <i><Translate id="learning-gap.assign-all-fields" options={{ renderInnerHtml: true }} /></i>
              </span>
                </ModalHeader>
                <ModalBody>
                  <LearningGapAddLessonForm
                      isCatalog={true}
                      students={students}
                      selectedLessonProp={selectedLesson}
                      updateParent={() => {
                        closeModal('assignAvanza');
                        handleSuccessModal();
                      }}
                  />
                </ModalBody>
              </Modal>
          )}

          {/* Modal to show success when assigning a lesson */}
          {modals.success && (
              <ModalSuccess
                  isOpen={modals.success}
                  content={<Translate id="lesson-catalog.success-lesson" options={{ renderInnerHtml: false }} />}
                  closeModal={closeModal}
              />
          )}
        </div>
      </>
  );
};

export default withLocalize(EnglishLessonCatalogView);
