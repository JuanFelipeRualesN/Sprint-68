import Settings from "./custom-classes/settings";
import ReportConnections from "./components/data-analysis-module/report-connections";
import ReportAcademics from "./components/data-analysis-module/report-academics";
import ReportUser from "./components/data-analysis-module/report-user";
import ReportStudentsActivityTeacher from "./components/data-analysis-module/report-students-activity-teacher";

export const componentRegistry = {
    'REPORT_CONNECTIONS': ReportConnections,
    'REPORT_ACADEMICS': ReportAcademics,
    'REPORT_USER': ReportUser,
    'STUDENT_ACTIVITIES_TEACHER': ReportStudentsActivityTeacher
};

export const reportConfig = [
  {
    category: "report-users",
    workspaceId: Settings.PBI_WORKSPACE_ID,
    reportId: Settings.PBI_REPORT_USER_ID,
    allowedRoles: [Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR, Settings.ROLE_DIRECTOR_STR, Settings.ROLE_EMPLOYEE_STR],
    subItems: [
      { 
        label: "report-users", 
        pageName: Settings.PBI_USERS_PAGE_ID,
        allowedRoles: [Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR]
      },
      {
          label: "download-excel-users", 
          type: 'component', 
          componentKey: 'REPORT_USER'
      }
    ]
  },    
  {
    category: "report-connections",
    workspaceId: Settings.PBI_WORKSPACE_ID,
    reportId: Settings.PBI_REPORT_CONNECTIONS_ID,
    allowedRoles: [Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR, Settings.ROLE_DIRECTOR_STR],
    subItems: [
      { 
          label: "download-excel-connections", 
          type: 'component', 
          componentKey: 'REPORT_CONNECTIONS'
      }
    ]
  },
  {
    category: "report-connections",
    workspaceId: Settings.PBI_WORKSPACE_ID,
    reportId: Settings.PBI_REPORT_CONNECTIONS_ID,
    allowedRoles: [Settings.ROLE_TEACHER_STR],
      subItems: [
      { 
          label: "download-excel-activity", 
          type: 'component', 
          componentKey: 'STUDENT_ACTIVITIES_TEACHER'
      }
    ]
  },
  {
    category: "report-academic",
    workspaceId: Settings.PBI_WORKSPACE_ID,
    reportId: Settings.PBI_REPORT_ACADEMIC_ID,
      subItems: [
      { 
        label: "report-academic", 
        pageName: Settings.PBI_ACADEMICS_PAGE_ID,
        allowedRoles: [Settings.ROLE_ADMIN_STR, Settings.ROLE_ADMIN_ORE_STR]
      },
      { 
          label: "download-excel-academic", 
          type: 'component', 
          componentKey: 'REPORT_ACADEMICS'
      }
    ]
  },
  {
    category: "report-admin",
    workspaceId: Settings.PBI_WORKSPACE_ID,
    reportId: Settings.PBI_REPORT_ADMINISTRATIVE_ID,
    allowedRoles: [Settings.ROLE_ADMIN_STR], 
    subItems: [
        { 
          label: "report-connections", 
          pageName: Settings.PBI_USER_CONNECTIONS_PAGE_ID,
        },
        { 
          label: "cumulative-connections-report", 
          pageName: Settings.PBI_CUMULATIVE_CONNECTIONS_PAGE_ID,
        },
        { 
          label: "teacher-usage-report", 
          pageName: Settings.PBI_TEACHER_ACTIVITIES_PAGE_ID,
        }
    ]
  }
];