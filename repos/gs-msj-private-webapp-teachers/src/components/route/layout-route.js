import React, { useEffect, useState, useRef, useContext } from "react";
import { Switch, Redirect, Route, matchPath, useLocation } from "react-router-dom";
import TeacherRoute from "./teacher-route";
import KinderRoute from "./kinder-route";
import PrivateRoute from "./private-route";
import StudentTeacherSharedRoute from "./student-teacher-shared-route";
import EmployeeRoute from "./employee-route";
import Assignments from "../../components/teacher-module/view/assigments";
import Syllabus from "../../components/homeschooler-module/view/syllabus";
import Menu from "../shared-module-components/menu";
import { MobileBottomNav } from "../shared-module-components/navbar/mobile-bottom-nav/mobile-bottom-nav";
import NavbarClients from "../shared-module-components/navbar/navbar-clients";
import "../../styles/med.css";
import "../../styles/app-shell.css";
import Dashboard from "../../components/shared-module-components/view/dashboard/dashboard";
import AiToolsPage from "../../components/teacher-module/ai-module/ai-tools-page";
import MyDocumentsPage from "../../components/teacher-module/ai-module/my-documents-page";
import ReportStudentAverage from "../../components/shared-module-components/view/report-student-average";
import ReportStudentLessonAverage from "../../components/shared-module-components/view/report-student-lesson-average";
import ReportResults from "../../components/shared-module-components/view/report-results";
import ProfileUpdateTeacher from "../../components/shared-module-components/view/profile-update-teacher";
import ProfileUpdateStudent from "../../components/shared-module-components/view/profile-update-student";
import SubscriptionAddNew from "../../components/homeschooler-module/view/subscription-add-new";
import SubscriptionSetProfileFirstTime from "../../components/homeschooler-module/view/subscription-set-profile-first-time";
import SubscriptionSetProfileExistingAccount from "../homeschooler-module/view/homeschool-subs-set-profile-view";
import DownloadFirstTime from "../../components/guest-module/view/download-first-time";
import PlanningSelect from "../../components/teacher-module/view/planning-select";
import Plannings from "../../components/teacher-module/view/plannings";
import PlanningDetail from "../../components/teacher-module/view/planning-detail";
import TeacherGroups from "../../components/teacher-module/view/teacher-groups";
import TeacherGroupsDetail from "../../components/teacher-module/view/teacher-groups-detail";
import Messages from "../../components/message-module/view/messages";
import Record from "../../components/teacher-module/view/record";
import DashboardsView from "../shared-module-components/dashboards/dashboards-view";
import Invitations from "../../components/shared-module-components/view/invitations";
import Chat from "../shared-module-components/view/chat";
import AssignmentManager from "../../components/teacher-module/view/assignment-manager";
import LessonPresentationTeacher from "../../components/teacher-module/view/lesson-presentation";
import ForumDetail from "../../components/teacher-module/view/forum-detail";
import ForumCreate from "../teacher-module/forum/forum-create";
import TeacherCalendar from "../../components/shared-module-components/view/teacher-calendar";
import QuizGenial from "../../components/teacher-module/view/quiz-genial";
import SubscriptionManager from "../../components/admin-module/views/subscription-manager";
import PlanManager from "../../components/admin-module/views/plans-manager";
import PlansSearch from "../../components/admin-module/views/plans-search";
import SubscriptionsSearch from "../../components/admin-module/views/subscriptions-search";
import AcademicDetails from "../../components/admin-module/views/academic-details";
import CouponSearch from "../../components/admin-module/views/coupons";
import UsersSearch from "../../components/admin-module/views/users-search";
import SchoolEntitySearch from "../../components/admin-module/views/school-entity-search";
import SchoolEntityManage from "../../components/admin-module/views/school-entity-manage";
import SchoolSubscription from "../../components/admin-module/views/school-subscription";
import SchoolConfirmUser from "../../components/admin-module/views/school-confirm-user";
import CommercialModule from "../../components/admin-module/views/commercial-module";
import UserManager from "../../components/admin-module/views/user-manager";
import ItemsProductView from "../../components/admin-module/views/items-product-manager";
import ProductManager from "../../components/admin-module/views/product-manager";
import PermissionManager from "../../components/admin-module/views/permission-manager";
import RoleManager from "../../components/admin-module/views/role-manager";
import Access from "../../components/admin-module/views/access";
import EventsSearch from "../../components/admin-module/views/events-search";
import io from "socket.io-client";
import SessionManager from "../../custom-classes/session-manager";
import Settings from "../../custom-classes/settings";
import AcademicContentSelect from "../teacher-module/view/academic-content-select";
import PendingAssignmentStudent from "../teacher-module/view/pending-assignments/pending-assignment-student";
import PendingAssignments from "../teacher-module/view/pending-assignments/pending-assignments";
import InstructionalPlans from "../teacher-module/view/pending-assignments/instructional-plans";
import StudentProgress from "../teacher-module/view/progress/student-progress";
import ProgressReport from "../teacher-module/view/progress/progress-report";
import PendingTeacherAssignments from "../teacher-module/view/pending-teacher-assignments/pending-teacher-assignments";
import LiveChat from 'react-livechat';
import TeamsSignin from "../teacher-module/view/teams/pages/teams-signin";
import CalendarPage from "../teacher-module/view/teams/pages/calendar-page";
import CreateMeetingPage from "../teacher-module/view/teams/pages/create-meeting-page";
import ConfigurationPage from "../admin-module/views/configuration/configuration";
import AuthorizedRoute from "./authorized-route";
import { AppContext } from "../../App.Context";
import { isRoleAllowed } from "../../utils/utils";
import UserHelpWindow from "../guest-module/view/user-manual";
import LessonDetail from "../student-module/view/lesson-student";
import QuizStudent from "../student-module/view/lesson-student/quiz";
import PlanningsList from '../teacher-module/planning/plannings-list';
import PlanningsCreate from '../teacher-module/planning/plannings-create';
import PlanningsDetail from '../teacher-module/planning/plannings-detail';
import LessonCatalogView from "../teacher-module/lesson-catalog/lesson-catalog-view";
import Lesson from "../principal-module/student-platform/lesson";
import Library from "../principal-module/student-platform/library";
import BookDetail from "../principal-module/student-platform/library/book-detail";
import LearningGapView from "../teacher-module/learning-gap/learning-gap-view";
import KinderInteractive from "../principal-module/student-platform/kinder-interactive";
import RecordHomeschool from "../homeschooler-module/view/record-homeschool";
import RecordHomeschoolLesson from "../homeschooler-module/view/record-homeschool-lesson";
import CurriculumModule from "../admin-module/views/curriculum-module";
import DashboardDataAnalysis from "../data-analysis-module/dashboard/index";
import ReportUser from "../data-analysis-module/report-user";
import ReportConnections from "../data-analysis-module/report-connections";
import ReportAcademics from "../data-analysis-module/report-academics";
import ReportStudentsActivityTeacher from "../data-analysis-module/report-students-activity-teacher";
import CommercialProductManager from "../admin-module/views/commercial-product-manager";
import DiagnosticView from "../shared-module-components/dashboards/diagnostics";
import Chatbot from "../teacher-module/chatbot/chatbot";
import EnglishLessonCatalogView from "../teacher-module/lesson-catalog/english-lesson-catalog-view";


const ROUTES_WITHOUT_MENU = [
  { path: "/students/lesson/:lessonId", exact: true },
  { path: "/students/lesson/quiz/:quizId", exact: true },
];

const shouldHideMenu = (pathname) =>
  ROUTES_WITHOUT_MENU.some((route) => matchPath(pathname, route));

let socket;

const LayoutRoute = () => {
  const { state } = useContext(AppContext);
  const { pathname } = useLocation();
  const hideMenu = shouldHideMenu(pathname);
  const [sidebarCollapsed, setSidebarCollapsed] = useState(() => {
    try {
      return localStorage.getItem("sidebar-collapsed") === "true";
    } catch (e) {
      return false;
    }
  });
  const [drawerOpen, setDrawerOpen] = useState(false);

  const toggleSidebar = () => setSidebarCollapsed((v) => !v);
  const openDrawer = () => setDrawerOpen(true);
  const closeDrawer = () => setDrawerOpen(false);

  useEffect(() => {
    if (!drawerOpen) return undefined;
    const onKey = (e) => { if (e.key === "Escape") setDrawerOpen(false); };
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [drawerOpen]);

  useEffect(() => {
    if (hideMenu) setDrawerOpen(false);
  }, [hideMenu]);

  const [users, setUsers] = useState([]);
  const [usernames, setUsernames] = useState([]);
  const [chats, setChats] = useState([]);
  const [chatSelected, setChatSelected] = useState(null);
  const usersRef = useRef(users);
  const chatsRef = useRef(chats);
  const chatSelectedRef = useRef(chatSelected);
  const sessionDetails = SessionManager.getSessionDetails();
  const username = sessionDetails.Username;
  const token = sessionDetails.Token;

  useEffect(() => {
    usersRef.current = users;
    chatsRef.current = chats;
    chatSelectedRef.current = chatSelected;
  });

  useEffect(() => {
    const nicks = users.map((user) => user.username);
    setUsernames(nicks);
  }, [users]);

  useEffect(() => {
    if ((isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) && state.variables.ChatManagementTeacher) || (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR], sessionDetails.Roles) && state.variables.ChatManagementParent) || (isRoleAllowed([Settings.ROLE_CLIENT_STR], sessionDetails.Roles))) {
      initSocket();
      return () => {
        closeSocket();
      };
    }
  }, []);


  const initSocket = () => {
    socket = io(Settings.SOCKET_URL, {
      query: { token },
    });

    socket.on("connect", () => {
      socket.emit("CONNECT_USER", { username }, (error) => {
        if (error) {
          console.log(error);
        }
      });
    });

    socket.on("USERS_ONLINE", ({ users }) => {
      const chats = chatsRef.current;
      const usersNickname = users.map((user) => user.username);
      const chatsOnline = chats.map((chat) => {
        chat.online = usersNickname.includes(chat.name);
        return chat;
      });
      setChats(chatsOnline);
      setUsers(users);
    });

    socket.on("LOAD_CHATS", ({ chats }) => {
      const data = chats.map((chat) => {
        chat.name = createNickReceived(chat.users, username);
        return chat;
      });
      setChats(data);
    });

    socket.on("SEND_CHAT", ({ chat, exists }) => {
      const users = usersRef.current;
      const usersNickname = users.map((user) => user.username);
      chat.name = createNickReceived(chat.users, username);
      chat.online = usersNickname.includes(chat.name);

      if (!exists) {
        setChats((state) => [...state, chat]);
        setChatSelected(chat);
      } else {
        const chats = chatsRef.current;
        const chatExists = chats.find((item) => item._id === chat._id);
        if (!chatExists) {
          setChats((state) => [...state, chat]);
        }
        setChatSelected(chat);
      }
    });

    socket.on("MESSAGE_RECIEVED", ({ chatUpdated }) => {
      const users = usersRef.current;
      const usersNickname = users.map((user) => user.username);
      chatUpdated.name = createNickReceived(chatUpdated.users, username);
      chatUpdated.online = usersNickname.includes(chatUpdated.name);
      const chats = chatsRef.current;

      const chatExists = chats.find((item) => item._id === chatUpdated._id);
      if (!chatExists) {
        chatsRef.current = [...chats, chatUpdated];
      }

      let newChats = chatsRef.current.map((item) => {
        if (item._id === chatUpdated._id) {
          return chatUpdated;
        }
        return item;
      });
      setChats(newChats);

      const chatSelected = chatSelectedRef.current;

      setChatSelected(
        chatSelected && chatSelected._id === chatUpdated._id
          ? chatUpdated
          : chatSelected
      );
    });
  };

  const handleSendMessage = (message) => {
    socket.emit("SEND_MESSAGE", {
      sender: username,
      chatId: chatSelected._id,
      message,
    });
  };

  const closeSocket = () => {
    socket.disconnect();
  };

  const handleCreateChat = (reciever) => {
    socket.emit("START_CHAT", {
      reciever,
      sender: username,
    });
  };

  const createNickReceived = (users, excludedUser) => {
    return users.filter((u) => u !== excludedUser).join("");
  };
 
  return (
    <div className="med-app-root">
      {!hideMenu && (
        <Menu
          collapsed={sidebarCollapsed}
          onToggleCollapse={toggleSidebar}
          drawerOpen={drawerOpen}
          onCloseDrawer={closeDrawer}
        />
      )}

      <div className="med-app-main">
        <NavbarClients onHamburger={hideMenu ? undefined : openDrawer} />

        <div className="med-app-content">
      <Switch>
        <TeacherRoute path="/assignments" component={Assignments} />
        <TeacherRoute
          path="/assignment-manager"
          component={AssignmentManager}
        />
        <TeacherRoute path="/plannings" component={Plannings} />
        <TeacherRoute path="/v2/plannings" component={PlanningsList} />
        <TeacherRoute path="/v2/plannings-create" component={PlanningsCreate} />
        <TeacherRoute path="/v2/plannings-detail" component={PlanningsDetail} />
        <TeacherRoute path="/planning-detail" component={PlanningDetail} />
        <TeacherRoute path="/teacher-groups" component={TeacherGroups} />
        <PrivateRoute path="/learning-gap" component={LearningGapView} />
        <PrivateRoute path="/lesson-catalog" component={LessonCatalogView} />
        <PrivateRoute path="/english-content" component={EnglishLessonCatalogView} />
        <TeacherRoute path="/metrics/diagnostics" component={DiagnosticView} />
        <TeacherRoute path="/chatbot" component={Chatbot} />
        <TeacherRoute path="/ai-tools" component={AiToolsPage} />
        <TeacherRoute path="/my-documents" component={MyDocumentsPage} />
        <TeacherRoute
          path="/teacher-groups-detail"
          component={TeacherGroupsDetail}
        />
        <TeacherRoute
          path="/lesson-presentation"
          component={LessonPresentationTeacher}
        />
        <TeacherRoute path="/forum-details" component={ForumDetail} />
        <TeacherRoute path="/forum-create" component={ForumCreate} />

        <PrivateRoute
          path="/subscription-add-new"
          component={SubscriptionAddNew}
        />
        <AuthorizedRoute authorized={isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles)} exact path="/students/library" component={Library} />  
        <AuthorizedRoute authorized={isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles)} exact path="/students/library/:bookId" component={BookDetail} />  
        <AuthorizedRoute authorized={isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles)} path="/students/lesson" exact component={Lesson} />
        <KinderRoute authorized={isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles)} path="/students/kinder-interactive" exact component={KinderInteractive} />
        <PrivateRoute path="/help" component={UserHelpWindow} />
        <PrivateRoute path="/syllabus" component={Syllabus} />
        <PrivateRoute path="/planning-select" component={PlanningSelect} />
        <PrivateRoute push exact path="/dashboard" component={Dashboard} />
        <PrivateRoute path="/report-average" component={ReportStudentAverage} />
        <PrivateRoute
          path="/report-lesson-average"
          component={ReportStudentLessonAverage}
        />
        <PrivateRoute path="/report-results" component={ReportResults} />
        <AuthorizedRoute authorized={
          (isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles)
              && state.variables.ConfigurationManagerAdmin) ||
            (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles)
                && state.variables.ConfigurationManagerAdminOre)
            || (isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles)
                && state.variables.ConfigurationManagerTeacher) ||
            (isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles)
                && state.variables.ConfigurationManagerPrincipal) ||
            (isRoleAllowed([Settings.ROLE_CLIENT_STR], sessionDetails.Roles)
                && state.variables.ConfigurationManagerPrincipal) ||
            (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR], sessionDetails.Roles)
                && state.variables.ConfigurationManagerParent)}
          path="/configuration"
          component={ConfigurationPage}
        />
        <PrivateRoute path="/profile-update" component={ProfileUpdateStudent} />
        <PrivateRoute path="/profile-teacher" component={ProfileUpdateTeacher} />
        <PrivateRoute
          path="/set-up-child-profile"
          component={SubscriptionSetProfileFirstTime}
        />
        <PrivateRoute
          path="/add-child-profile"
          component={SubscriptionSetProfileExistingAccount}
        />
        <PrivateRoute
          path="/download-first-time"
          component={DownloadFirstTime}
        />

        <PrivateRoute path="/metrics" component={DashboardsView} />
        <PrivateRoute path="/record" component={Record} />
        <PrivateRoute path="/record-homeschool" component={RecordHomeschool} />
        <PrivateRoute path="/record-homeschool-lesson" component={RecordHomeschoolLesson} />
        <PrivateRoute path="/progress-average" component={StudentProgress} />
        <PrivateRoute path="/progress-average-student/:studentId" component={ProgressReport} />
        <PrivateRoute path="/academic-content-select" component={AcademicContentSelect} />
        <PrivateRoute path="/students/lesson/quiz/:quizId" component={QuizStudent} />
        <PrivateRoute path="/students/lesson/:lessonId" component={LessonDetail} />
        <AuthorizedRoute authorized={(isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) && state.variables.InvitationManagementTeacher) || (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR], sessionDetails.Roles) && state.variables.InvitationManagementParent)} path="/invitations" component={Invitations} />
        <Route
          path="/chat"
          render={(props) => (
            <Chat
              {...props}
              usernames={usernames}
              chats={chats}
              chatSelected={chatSelected}
              setChatSelected={setChatSelected}
              handleSendMessage={handleSendMessage}
              handleCreateChat={handleCreateChat}
            />
          )}
        />
        <AuthorizedRoute authorized={
            (isRoleAllowed([Settings.ROLE_ADMIN_STR], sessionDetails.Roles) && state.variables.MessageManagementAdmin) ||
            (isRoleAllowed([Settings.ROLE_ADMIN_ORE_STR], sessionDetails.Roles) && state.variables.MessageManagementAdminOre) ||
            (isRoleAllowed([Settings.ROLE_DIRECTOR_STR], sessionDetails.Roles) && state.variables.MessageManagementPrincipal) ||
            (isRoleAllowed([Settings.ROLE_PARENT_NO_CLIENT_STR], sessionDetails.Roles) && state.variables.MessageManagementParent) ||
            (isRoleAllowed([Settings.ROLE_TEACHER_STR], sessionDetails.Roles) && state.variables.MessageManagementTeacher) ||
            (isRoleAllowed([Settings.ROLE_CLIENT_STR], sessionDetails.Roles) && state.variables.MessageManagementParent)
        } path="/messages" component={Messages} />
        <PrivateRoute
          path="/pending-assignments"
          component={PendingAssignments}
        />
        <PrivateRoute
          path="/pending-assignments-student/:studentId"
          component={InstructionalPlans}
        />
        <PrivateRoute
          path="/pending-assignments-student-planning/:studentId/:planningId"
          component={PendingAssignmentStudent}
        />
        <PrivateRoute path="/calendar" component={TeacherCalendar} />

        {/*teams routers*/}        
        <AuthorizedRoute authorized={isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_CLIENT_STR], sessionDetails.Roles) && state.variables.TeamMeetsManagementTeacher} path="/teams-signin" component={TeamsSignin} />
        <AuthorizedRoute authorized={isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_CLIENT_STR], sessionDetails.Roles) && state.variables.TeamMeetsManagementTeacher} path="/teams-calendar" component={CalendarPage} />
        <AuthorizedRoute authorized={isRoleAllowed([Settings.ROLE_TEACHER_STR, Settings.ROLE_CLIENT_STR], sessionDetails.Roles) && state.variables.TeamMeetsManagementTeacher} path="/create-meeting" component={CreateMeetingPage} />

        <StudentTeacherSharedRoute path="/quiz" component={QuizGenial} />
        <EmployeeRoute
          push
          path="/subscription-manager"
          component={SubscriptionManager}
        />
        <EmployeeRoute push path="/commercial-plans-manager" component={PlanManager} />
        <EmployeeRoute push path="/commercial-plans-search" component={PlansSearch} />
        <EmployeeRoute
          push
          path="/commercial-subscriptions-search"
          component={SubscriptionsSearch}
        />
        <EmployeeRoute push path="/coupons" component={CouponSearch} />
        <EmployeeRoute
          push
          path="/commercial-module"
          component={CommercialModule}
        />
        <EmployeeRoute
          push
          path="/curriculum-module"
          component={CurriculumModule}
          />
        <EmployeeRoute push path="/users-search" component={UsersSearch} />
        <EmployeeRoute
          push
          path="/school-entity-search"
          component={SchoolEntitySearch}
        />
        <EmployeeRoute
          push
          path="/school-entity-manage"
          component={SchoolEntityManage}
        />
        <EmployeeRoute
          push
          path="/school-subscription"
          component={SchoolSubscription}
        />
        <EmployeeRoute
          push
          path="/confirm-user"
          component={SchoolConfirmUser}
        />
        <EmployeeRoute push path="/user-manager" component={UserManager} />
        <EmployeeRoute
          push
          path="/curriculum-items-product"
          component={ItemsProductView}
        />
        <EmployeeRoute push path="/curriculum-products" component={ProductManager} />
        <EmployeeRoute push path="/commercial-products" component={CommercialProductManager} />
        <EmployeeRoute
          push
          path="/permission-manager"
          component={PermissionManager}
        />
        <EmployeeRoute push path="/roles-manager" component={RoleManager} />
        <EmployeeRoute push path="/access" component={Access} />
        <AuthorizedRoute authorized={isRoleAllowed([Settings.ROLE_EMPLOYEE_STR, Settings.ROLE_ADMIN_STR], sessionDetails.Roles)} path="/events-search" component={EventsSearch} />
        <EmployeeRoute
          push
          path="/academic-details"
          component={AcademicDetails}
        />
        <PrivateRoute
          path="/pending-teacher-assignments"
          component={PendingTeacherAssignments}
        />
        <PrivateRoute
          path="/analysis/dashboard"
          component={DashboardDataAnalysis}
        />
        <PrivateRoute
          path="/analysis/users"
          component={ReportUser}
        />
        <PrivateRoute
          path="/analysis/connections"
          component={ReportConnections}
        />
        <PrivateRoute
          path="/analysis/academics"
          component={ReportAcademics}
        />
        <PrivateRoute
          path="/analysis/student-activities-teacher"
          component={ReportStudentsActivityTeacher}
        />
      </Switch>
        </div>
      </div>
      {!hideMenu && drawerOpen && (
        <div className="med-sidebar-backdrop" onClick={closeDrawer} />
      )}
      <MobileBottomNav />
      <LiveChat license={Settings.CHAT_LIVE_CHAT_LICENSE} />
    </div>
  );
};

export default LayoutRoute;
