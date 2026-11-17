import React, {useState, useEffect} from "react";
import LessonView from "./lesson-view/lesson-view";
import {addTranslation, Translate, withLocalize, LocalizeProvider} from "react-localize-redux";
import { getAthenasSubjects, getAthenasGrades, getAthenasStandards, getAthenasExpectations, getAthenasAdvancedLessons, getTeacherPlannings } from "../../../api/teachers";
import { getProfiles } from "../../../api/users";
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import LessonFilters from "./lesson-filters/lesson-filters";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import { KeyboardArrowDown, RemoveRedEyeSharp } from "@material-ui/icons";
import LearningGapLessonView from "../learning-gap/learning-gap-lessons/learning-gap-lesson-view/learning-gap-lesson-view";
import CreateInstructionalPlanModal from "../modal/modal-create-instructional-plan";
import ModalTeacherPlanningNameCreator from "../modal/modal-teacher-planning-name-creator";
import ModalPlanningAttachItem from "../modal/modal-planning-attach-item";
import ModalSuccess from "../modal/modal-success";
import LearningGapAddLessonForm from "../learning-gap/learning-gap-lessons/learning-gap-add-lesson-form/learning-gap-add-lesson-form";
import {Modal, ModalBody, ModalHeader} from 'reactstrap';
import { MenuBook } from "@material-ui/icons";
import LessonSkeleton from "./lesson-skeleton/lesson-skeleton";
import Settings from "../../../custom-classes/settings";
import {renderToStaticMarkup} from "react-dom/server";
import SessionManager from "../../../custom-classes/session-manager";
import videoSpanish from "../../../assets/videos/avanza-es.mp4";
import { getSubscriptionCourses } from "../../../api/subscriptions";
import { Provider } from "react-redux";
import store from "../../../redux/store";
import useVersionedTranslations from "../../../hooks/useVersionedTranslations";
import { SendAccessLog } from "../../../utils/access_logger";

// Component that display the lesson catalog
const LessonCatalogView = () => {
  const sessionDetails = SessionManager.getSessionDetails();
  const [subjects, setSubjects] = useState([]);
  const [grades, setGrades] = useState([]);
  const [standards, setStandards] = useState([]);
  const [expectations, setExpectations] = useState([]);
  const [lessons, setLessons] = useState([]);
  const [loading, setLoading] = useState(false);
  const [lessonsLimiter] = useState(8);
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
  const [userRole, setUserRole] = useState(SessionManager.getUserRole() || Settings.ROLE_TEACHER_STR); // Default to 'teacher' if not set
  const [filters, setFilters] = useState({
    subjects: [],
    grades: [],
    standards: [],
    expectations: [],
    creationDate: null,
    titleSort: null,
  });
  // Create a copy of the filters to use in the API call, and not affect the fetching if the user change the filters and goes to another page
  const [searchFilters, setSearchFilters] = useState({
    subjects: [],
    grades: [],
    standards: [],
    expectations: [],
    creationDate: null,
    titleSort: null,
  });
  const [inputValues, setInputValues] = useState({
    content: '',
    code: ''
  });
  // Create a copy of the input values to use in the API call, and not affect the fetching if the user change the filters and goes to another page
  const [searchInputValues, setSearchInputValues] = useState({
    content: '',
    code: ''
  });
  const [modals, setModals] = useState({
    lessonDetails: false,
    noPlans: false,
    createPlan: false,
    assignPlan: false,
    assignAvanza: false,
    success: false,
  });

  // Initialize translations based on the current version
  useVersionedTranslations();
  
  useEffect(() => {
    try{            
      SendAccessLog({
        Username: sessionDetails.Username,
        InvitationCode: sessionDetails.InvitationCode,
        Role: sessionDetails.Roles[0],
        AreaId: "12"
      })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
  }, []);

  // Fetch the lessons and filters data ( subjects, grades, standards, expectations )
  useEffect(() => {
    // Fetch all the data when the component mounts
    const fetchAllData = async () => {
      setLoading(true);
      try {
        setSearchInputValues({...inputValues});
        setSearchFilters({...filters});
        await Promise.all([
          callSubjects(),
          callGrades(),
          callStandards(),
          callExpectations(),
          callLessons(),
        ]);

        // Set available courses for the teacher
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
    // After first load, just fetch the lessons when the filters change
    // Avoid fetching the lessons twice when it's called from the filters component
    if (!firstLoad && !isCalledFromFilters) {
      callLessons();
    }
  }, [currentPage, filters.creationDate, filters.titleSort]);

  // Check courses availability for the lesson
  const isLessonAvailable = (lesson) => {
    if (!availableCourses || availableCourses.length === 0) return false;
    
    const lessonSubject = lesson.LessonModel.SubjectCode;
    const lessonLevel = lesson.LessonModel.LevelCode;
    
    return availableCourses.some(course => {
      if (course.Subject === lessonSubject && course.Level === lessonLevel) {
        return true;
      }

      return false;
    });
  };

  // Clear the filters and input values, reset the current page to 0 and fetch the lessons
  const clearAndFetchLessons = () => {
    setFilters({
      subjects: [],
      grades: [],
      standards: [],
      expectations: [],
      creationDate: null,
      titleSort: null,
    });
    setInputValues({
      content: '',
      code: ''
    });
    setCurrentPage(0);
    callLessonsWithClearedFilters();
  };

  // Clear the search filters and fetch the lessons
  const callLessonsWithClearedFilters = async () => {
    setLoading(true);
    try {
      const payload = {
        "SubjectCodes": [],
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
      setSearchInputValues({
        content: '',
        code: ''
      });
      
      setLessons(response.data.LessonsDetails.Lessons);
      setTotalItems(response.data.LessonsDetails.TotalResults);
    } catch (error) {
      console.error("Error fetching lessons:", error);
    } finally {
      setLoading(false);
    }
  };

  // Fetch the lessons, if the method is called from the filters component, set the current page to 0 with every call
  const callLessons = async (fromFilters = false) => {
    if (fromFilters) {
      setIsCalledFromFilters(true);
    }
    setLoading(true);
    try {
      // Use selected filters if the method is called from the filters component, if it's called from pagination, use the search filters
      const filtersToUse = fromFilters ? filters : searchFilters;
      const subjectCodes = filtersToUse.subjects;
      const gradeCodes = filtersToUse.grades;
      const standardCodes = filtersToUse.standards;
      const expectationCodes = filtersToUse.expectations;
  
      const contentFilter = fromFilters ? inputValues.content : searchInputValues.content;
      const codeFilter = fromFilters ? inputValues.code : searchInputValues.code;

      // If the lessons are being called from the filters component, set the current page to 0 in every API call
      const pageToUse = fromFilters ? 0 : currentPage;

      if (fromFilters) {
        setCurrentPage(0);
      }

      // Allow one filter to be applied at a time, if the content filter is not empty, reset the title sort filter
      if (contentFilter && contentFilter.trim() !== '') {
        setFilters(prev => ({
          ...prev,
          titleSort: null
        }));
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

      // Save the search filters and input values after the API call if the method is called from the filters component
      if (fromFilters) {
        setSearchFilters({...filters});
        setSearchInputValues({
          content: inputValues.content,
          code: inputValues.code
        });
      }
      setLessons(response.data.LessonsDetails.Lessons);
      setTotalItems(response.data.LessonsDetails.TotalResults);
      return response.data

    } catch (error) {
      console.error("Error fetching lessons:", error);
    } finally {
      setLoading(false);
      setIsCalledFromFilters(false);
    }
  }

  const filterLessonsByCheckbox = async (newFilters) => {
  setLoading(true);
  setCurrentPage(0);
  setIsCalledFromFilters(true);
  try {
    const payload = {
      "SubjectCodes": newFilters.subjects,
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

  // Fetch the subjects and set the filters
  const callSubjects = async () => {
    try {
      const response = await getAthenasSubjects();
      const formattedSubjects = response.data
        .filter(subject => subject.Name !== "")
        .map(subject => ({
          label: subject.Name,
          code: subject.Code,
        }));

        // Get the subjects to filter out from Settings
        const subjectsToFilterOut = Settings.getSubjectsToFilterOut();
        // Filter out the subjects that are not in the subjectsToFilterOut array
        const filteredSubjects = formattedSubjects.filter(subject =>
            !subjectsToFilterOut.includes(subject.code));

      setSubjects(filteredSubjects);
      
      setFilters(prev => ({
        ...prev,
        subjects: []
      }));

    } catch (error) {
      console.error("Error fetching subjects:", error);
    }
  };

  // Fetch the grades and set the filters
  const callGrades = async () => {
    try {
      const response = await getAthenasGrades();
      const formamattedGrades = response.data?.filter(subject => subject.Name !== "")
      .map(subject => ({
        label: subject.Name,
        code: subject.Code,
      }));

      const levelsToFilterOut = Settings.getLevelsToFilterOut();
        // Filter out the grades that are not in the levelsToFilterOut array
        const filteredGrades = formamattedGrades.filter(grade =>
            !levelsToFilterOut.includes(grade.code));
  
      setGrades(filteredGrades);
      
      setFilters(prev => ({
        ...prev,
        grades: []
      }));
      
    } catch (error) {
      console.error(error)
    }
  }
  
  // Fetch the standards and set the filters
  const callStandards = async () => {
    try {
      const response = await getAthenasStandards()
      const formattedStandards = response.data?.Items.filter(standard => standard.Description !== "")
        .map(standard => ({
          label: standard.Description,
          code: standard.Code,
          codeId: standard.Id,
        }));
      setStandards(formattedStandards);

      setFilters(prev => ({
        ...prev,
        standards: []
      }));

    } catch (error) {
      console.error("Error fetching standards:", error);
    }
  };

  // Fetch the expectations and set the filters
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
      
      setFilters(prev => ({
        ...prev,
        expectations: []
      }));

    } catch (error) {
      console.error("Error fetching expectations:", error);
    }
  }

  // Fetch the plannings when user tries to assign a lesson to a plan
  const getPlannings = async (lesson) => {
    setSelectedLesson(lesson);
    setLoading(true);

    try {
      const response = await getTeacherPlannings();
      setPlannings(response.data);
      response.data.length == 0 ? openModal('noPlans') : openModal('assignPlan');
    } catch (error) {
      console.error("Error fetching plannings:", error);
    }
    finally {
      setLoading(false);
    }
  }

  // Fetch the students when user tries to assign a lesson to Avanza
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
  }

  // Toggle the creation date filter
  const toggleCreationDate = () => {
    setFilters(prev => ({
      ...prev,
      creationDate:
        prev.creationDate === null
          ? 'ASC'
          : prev.creationDate === 'ASC'
          ? 'DESC'
          : null,
    }));
  }

  // Toggle the title sort filter
  const toggleTitleSort = () => {
    setFilters(prev => ({
      ...prev,
      titleSort:
        prev.titleSort === null
          ? 'ASC'
          : prev.titleSort === 'ASC'
          ? 'DESC'
          : null
    }));
    setInputValues(prev => ({
      ...prev,
      content: '',
    }));
  }

  // Handle the page change in the pagination
  const handlePageChange = (page) => {
    setCurrentPage(page - 1);
  }

  // Open the selected modal
  const openModal = (modalName) => {
    setModals(prev => ({ ...prev, [modalName]: true }));
  };
  
  // Close the selected modal
  const closeModal = (modalName) => {
    setModals(prev => ({ ...prev, [modalName]: false }));
  };

  const handleOpenLessonDetailsModal = (lesson) => {
    setSelectedLesson(lesson);
    try{
      SendAccessLog({
        UserName: sessionDetails.Username,
        InvitationCode: sessionDetails.InvitationCode,
        Role: sessionDetails.Roles[0],
        AreaId: "14",
        Details: "Previewed lesson details in catalog",
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
    closeModal('noPlans')
    openModal('createPlan');
  }

  const handleCloseCreatePlanModal = () => {
    closeModal('createPlan');
    closeModal('noPlans');
  }

  const handleCloseAssignLessonModal = () => {
    closeModal('assignPlan');
  }

  const handleSuccessModal = () => {
    openModal('success');
  }

  return (
    <>
      {loading && <LoaderFullWindow />}
      <div>
      <div className="wrapper-principal-container learning-gap-fade-in">
        <div className="wrapper-col mb-3">
          <div className="col col-no-padding-left">
            <div className="title-principal mt-3">
              <span>
                <Translate id="lesson-catalog.lesson-catalog" options={{ renderInnerHtml: false }} />
              </span>
            </div>
          </div>
        </div>
        {Settings.IS_MED_QUINDIO ? null :
          <div className="section-description" style={{color: '#A9A9A9', fontWeight: 600, textAlign: 'right', fontSize: '13px'}}>
            <a
              href={Settings.LESSON_CATALOG_PDF_URL}
              target="_blank"
              style={{textDecoration: 'none', color: '#A9A9A9'}}
            >
              <RemoveRedEyeSharp fontSize="small" style={{marginRight: '5px', width: '15px'}}/>
              <Translate id="lesson-catalog.show-catalog" options={{ renderInnerHtml: false }} />
            </a>
          </div>
        }
        <div className="wrapper-rows">
          <LessonFilters
            filterLessons={() => callLessons(true)}
            setInputValues={setInputValues}
            filterByCheckbox={filterLessonsByCheckbox}
            clearAndFetch={clearAndFetchLessons}
            inputValues={inputValues}
            subjects={subjects}
            grades={grades}
            standards={standards}
            expectations={expectations}
            filters={filters}
            setFilters={setFilters}
            setStandards={setStandards}
            setExpectations={setExpectations}
          />
          <div className="lesson-catalog-container">
            <div className="lesson-results-title">
              <span>
                <Translate id="lesson-catalog.lesson-results" options={{ renderInnerHtml: false }}/>
              </span>
            </div>
            <div style={{display: 'flex', flexDirection: 'row', 'marginBottom': '5px' }}>
              <div style={{paddingRight: '10px', fontSize: '13px', fontWeight: 600}}>
                <span>
                  <Translate id="lesson-catalog.lesson-sort" options={{ renderInnerHtml: false }} />
                </span>
              </div>
              {filters.titleSort === null && <div 
                onClick={toggleCreationDate} 
                className="lesson-sorting-title"
                style={{
                  color: filters.creationDate === null ? '#A9A9A9' : 'black'
                }}>
                <span>
                  <Translate id="lesson-catalog.lesson-creation-date" options={{ renderInnerHtml: false }} />
                </span>
                {filters.creationDate === 'DESC' ? 
                  <KeyboardArrowDown fontSize="small"/> : 
                  filters.creationDate === 'ASC' ?
                  <KeyboardArrowDown fontSize="small" style={{transform: 'rotate(180deg)' }} /> : null
                }
              </div>
              }
              {filters.creationDate === null && searchInputValues.content === '' ? <div 
                onClick={toggleTitleSort} 
                className="lesson-sorting-title" 
                style={{
                  color: filters.titleSort === null ? '#A9A9A9' : 'black'
                }}>
                <span>
                  <Translate id="lesson-catalog.title" options={{ renderInnerHtml: false }} />
                </span>
                {filters.titleSort === 'DESC' ? 
                  <KeyboardArrowDown fontSize="small"/> : 
                  filters.titleSort === 'ASC' ?
                  <KeyboardArrowDown fontSize="small" style={{transform: 'rotate(180deg)' }} /> : null
                }
              </div>
              : null
              }
            </div>
            <div className="lessons-container">
              {loading && lessons.length === 0 ? <LessonSkeleton count={lessonsLimiter} /> : null}
              {lessons.length === 0 && !loading ? 
                <div className="no-results-container"> 
                  <MenuBook style={{width: '100px', height: '100px', color: '#d3d3d3'}} />
                  <Translate id="lesson-catalog.no-lessons-found" options={{ renderInnerHtml: false }} />
                </div>
                :
                lessons.map((lesson) => (
                <LessonView key={lesson.Id}
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
              ))}           
            </div>  
              <PaginationCustom
                currentPage={currentPage + 1}
                totalPages={Math.ceil(totalItems / lessonsLimiter)}
                updateParent={handlePageChange}
              />
          </div>
        </div>
      </div>

      {/* Modal to show the lesson details */}
      <Modal 
        className="modal-full-width" 
        isOpen={modals.lessonDetails} 
        toggle={() => closeModal('lessonDetails')}
      >
        <ModalHeader toggle={() => closeModal('lessonDetails')}/>
        <ModalBody>
          <Provider store={store}>
            <LocalizeProvider store={store}>
              <LearningGapLessonView 
                lessonId={selectedLesson?.LessonModel?.Id}
              />
            </LocalizeProvider> 
          </Provider>
        </ModalBody>
      </Modal>

      {/* Modal to show when the user has no instructional plans*/}
      {modals.noPlans && <CreateInstructionalPlanModal
        handleOpenCreatePlanModal={handleOpenCreatePlanModal}
        handleCloseCreatePlanModal={handleCloseCreatePlanModal}
        openNoPlanModal={modals.noPlans}
        closeModal={closeModal}
        />
      }

      {/* Modal to create a plan*/}
      {modals.createPlan && <ModalTeacherPlanningNameCreator visible={modals.createPlan} updateParent={handleCloseCreatePlanModal} isCatalog={true}/>}
      
      {/* Modal to assign a lesson to a plan */}
      {modals.assignPlan && 
        <ModalPlanningAttachItem
          visible={modals.assignPlan} 
          isCatalog={true} 
          updateParent={handleCloseAssignLessonModal}
          lessonId={selectedLesson?.LessonModel?.Id}
          plannings={plannings}
          handleSuccessModal={handleSuccessModal}
        />
      }

      {/* Modal to assign a lesson to Avanza */}
      {modals.assignAvanza &&
        <Modal isOpen={modals.assignAvanza} centered className="modal-results">
          <ModalHeader toggle={()=>{closeModal('assignAvanza')}}>
            <span style={{color: '#2a426d', fontWeight: 'bold'}}>
              <Translate id="learning-gap.assign-lesson" options={{renderInnerHtml: true}}/>
            </span>
              <br/>
              <span style={{fontSize: 13}}><i>
                  <Translate id="learning-gap.assign-all-fields" options={{renderInnerHtml: true}}/></i>
              </span>
          </ModalHeader>
          <ModalBody>
            <LearningGapAddLessonForm 
              isCatalog={true} 
              students={students}
              selectedLessonProp={selectedLesson}
              updateParent={() => {
                closeModal('assignAvanza')
                handleSuccessModal();
              }
            }
            />
          </ModalBody>
        </Modal>
      }

      {/* Modal to show success when assigning a lesson */}
      {modals.success && 
        <ModalSuccess 
          isOpen={modals.success}
          content={<Translate id="lesson-catalog.success-lesson" options={{ renderInnerHtml: false }} />}
          closeModal={closeModal}
        />
      }    
    </div>      
    </>
  )
};

export default withLocalize(LessonCatalogView);