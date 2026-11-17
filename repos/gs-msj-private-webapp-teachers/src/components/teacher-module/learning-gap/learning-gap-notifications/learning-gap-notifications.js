import React, { useContext, useState, useEffect, useRef } from 'react'
import { Row, Col, Button } from 'reactstrap'
import { Translate, withLocalize } from 'react-localize-redux'
import ImgAvanza from "../../../../assets/images/common-icons/Avanza-icon-Logo.png";
import DataTable from '../../../shared-module-components/datatables/datatable'
import { faPen, faCalendarDay} from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import PaginationCustom from '../../../shared-module-components/datatables/pagination';
import { getLearningGapReadStudentLessonQuizResultList, getLearningGapDiagnosticScore, getLearningGapTeacherNotifications, 
        getLearningGapStudentHasNotifications } from '../../../../api/teachers';
import ChangeLimitDateModal from '../../modal/modal-change-limit-date-learning-gap-notifications';
import AssignScoreModal from '../../modal/modal-assign-score-learning-gap-notifications';
import { AppContext } from '../../../../App.Context';
import { PulseLoader } from 'react-spinners';

// Component that displays the learning gap notifications for a student
const LearningGapNotifications = ({translate, studentId, studentName, refreshIfStudentHasNotifications}) => {
  const [notifications, setNotifications] = useState([])
  const [processedData, setProcessedData] = useState([])
  const [changeDateItemModalShow, setChangeDateItemModalShow] = useState(false)
  const [selectedItem, setSelectedItem] = useState(null)
  const [changeItemScoreModalShow, setChangeItemScoreModalShow] = useState(false)
  const [quizResults, setQuizResults] = useState(null)
  const [diagnosticTestsData, setDiagnosticTestsData] = useState(null)
  const [limiter] = useState(10)
  const [totalPages, setTotalPages] = useState(0)
  const [currentPage, setCurrentPage] = useState(1);
  const [studentHasNotifications, setStudentHasNotifications] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const isRequestLoading = useRef(false);
  const { refreshNotifications } = useContext(AppContext);

  const [headerOptions, setHeaderOptions] = useState([
    { key: "0", name: translate("learning-gap.notification")},
    { key: "1", name: translate("learning-gap.type")},
    { key: "2", name:  translate("learning-gap.action")}
  ]);

  useEffect(() => {
    setHeaderOptions([
      { key: "0", name: translate("learning-gap.notification")},
      { key: "1", name: translate("learning-gap.type")},
      { key: "2", name:  translate("learning-gap.action")}
    ]);
  }, [translate]);

  // Get notifications and diagnostic tests data when the component mounts
  useEffect(() => {
    fetchNotifications(); 
    fetchDiagnosticTestsData();
  }, [studentId, currentPage, limiter]);

  // Process the notifications data to be displayed in the table
  useEffect(() => {
    if (notifications.length > 0) {
      const newArr = notifications.map(item => ({
        NotificationTitle: (
          <div className="notification-title" style={{
            maxWidth: "550px",
            minHeight: "40px",
            textOverflow: "ellipsis"
          }}>
            {item.NotificationText}
          </div>
        ),
        NotificationType: item?.NotificationType === "lesson" ? (
          <div style={{fontWeight: 600}}>
            <Translate id="learning-gap.exam" options={{renderInnerHtml: true}}/>
          </div>
        ) : (
          <div style={{fontWeight: 600}}>
            <Translate id="learning-gap.diagnostic-test" options={{renderInnerHtml: true}}/>
          </div>
        ),
        Action: (
          <div style={{display: "flex", gap: "5px", justifyContent: window.innerWidth > 750 ? "center" : "flex-start"}}>
            <button
              type="button"
              className="btn-default button-secondary-custom"
              style={{minWidth: '10rem'}} 
              onClick={()=> {handleItemScoreModalOpen(item)}}
            >
              <FontAwesomeIcon icon={faPen} className="mr-2" />
              <span>
                <Translate id="learning-gap.assign-score" options={{renderInnerHtml: true}}/>
              </span>
            </button>
            <button
              type="button"
              className="btn-default button-secondary-custom"
              style={{minWidth: '10rem'}} 
              onClick={()=> {handleDateLimitModalOpen(item)}}
            >
              <FontAwesomeIcon icon={faCalendarDay} className="mr-2" />
              <span>
                <Translate id="learning-gap.change-limit-date" options={{renderInnerHtml: true}}/>
              </span>
            </button>
          </div>
        )
      }));
      setProcessedData(newArr);
    }
  }, [notifications]);

  useEffect(() => {
    refreshIfStudentHasNotifications(studentHasNotifications);
  }, [studentHasNotifications]);

  // Returns true or false if the student has notifications
  const checkIfStudentHasNotifications = async () => {
    try {
        const response = await getLearningGapStudentHasNotifications();
        setStudentHasNotifications(response.data);
    } catch (error) {
        console.error("Error fetching notifications:", error);
    }
  };

  // Get unread notifications
  const fetchNotifications = async () => {
    try {
      setIsLoading(true);
      let fromPage = currentPage - 1;
      if (fromPage < 0) fromPage = 0;
      const response = await getLearningGapTeacherNotifications(studentId, fromPage, limiter);
      setNotifications(response.data.Items);
      setTotalPages(response.data.TotalPages)
    } catch (error) {
      console.error("Error fetching notifications:", error);
    } finally {
      await refreshNotifications();
      await checkIfStudentHasNotifications();
      setIsLoading(false);
    }
  };

  // Get diagnostic tests data
  const fetchDiagnosticTestsData = async () => {
    try {
      const response = await getLearningGapDiagnosticScore(studentId);
      setDiagnosticTestsData(response.data);
    } catch (error) {
      console.error("Error fetching diagnostic tests data:", error);
    }
  }

  // Open the modal to change the date limit for a notification
  const handleDateLimitModalOpen = (item) => {
    setSelectedItem(item);
    setChangeDateItemModalShow(true);
  }

  // Close the modal to change the date limit for a notification
  const handleDateLimitModalClose = () => {
    setChangeDateItemModalShow(false);
  }

  // Open the modal to assign a score for a notification
  // Fetch the quiz results if the notification type is a lesson
  const handleItemScoreModalOpen = async (item) => {
    if (isRequestLoading.current) {
      return;
    }
    setSelectedItem(item);
    try {
      if (item?.NotificationType === "lesson") {
        isRequestLoading.current = true;
        const response = await getLearningGapReadStudentLessonQuizResultList(item?.AssignedLessonId);
        setQuizResults(response.data);
      } 
      setChangeItemScoreModalShow(true);
    } catch (error) {
      console.error("Error fetching results", error);
    } finally {
      isRequestLoading.current = false;
    }
  }
  
  // Close the modal to assign a score for a notification
  const handleItemScoreModalClose = () => {
    setChangeItemScoreModalShow(false);
    setQuizResults(null);
  }

  // Handle page click for notifications pagination
  const handlePageChange = (page) => {
    if (page !== currentPage){
      setCurrentPage(page);
    }
  }

  return (
    !isLoading ? (
      <div className='learning-gap-fade-in'>
        <Row className="align-vertical-center">
          <Col xs={8} className="align-vertical-center">
            <span className="learning-gap-subtitle">
              <Translate id="learning-gap.notifications" options={{renderInnerHtml: true}}/>
            </span>
          </Col>
        </Row>
        {notifications.length === 0 && !isLoading ? (
          <div className="learning-gap-welcome-area">
            <Row>
              <Col xs={12} style={{textAlign: "center"}}>
                  <img src={ImgAvanza} alt="Avanza" className="img-fluid" style={{maxWidth: 50, width: "100%", opacity: "40%"}} />
              </Col>
              <Col xs={12} style={{fontSize: 12, textAlign: "center"}}>
                  <Translate id="learning-gap.no-pending-notifications" options={{renderInnerHtml: true}}/>
              </Col>
            </Row>
          </div>) 
          : 
          (
          <>
            <DataTable headerOptions={headerOptions} data={processedData} />
          </>
        )}
        {changeDateItemModalShow &&
          <ChangeLimitDateModal 
            isOpen={changeDateItemModalShow}
            handleClose={handleDateLimitModalClose}
            selectedItem={selectedItem}
            studentName={studentName}
            diagnosticResults={diagnosticTestsData}
            fetchNotifications={fetchNotifications}
          />
        }
        {changeItemScoreModalShow && 
          <AssignScoreModal
            isOpen={changeItemScoreModalShow}
            handleClose={handleItemScoreModalClose}
            studentId={studentId}
            studentName={studentName}
            selectedItem={selectedItem}
            quizResults={quizResults}
            diagnosticResults={diagnosticTestsData}
            fetchNotifications={fetchNotifications}
          />
        }
        {notifications.length > 0 && (
          <PaginationCustom
            totalPages={totalPages}
            currentPage={currentPage}
            updateParent={handlePageChange}
          />
        )}     
      </div>
      )
    : 
    (
      <div style={{display: "flex", justifyContent: "center", alignItems: "center", marginTop: "20px"}}>
        <PulseLoader size={10} color="#395a8f" />
      </div>
    )
    
  )
}

export default withLocalize(LearningGapNotifications);