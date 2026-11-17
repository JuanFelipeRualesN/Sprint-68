import Settings from '../custom-classes/settings';
import SessionManager from '../custom-classes/session-manager';

export const accessAreas = {
  "student":{
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "1",
  },
  "group":{
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "2",
  },
  "plan": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "3",
  },
  "assignment": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "4",
  },
  "progress": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "5",
  },
  "ungraded": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "6",
  },
  "invitation": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "7",
  },
  "message": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "8",
  },
  "chat": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "9",
  },
  "teams": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "10",
  },
  "avanza": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "11",
  },
  "catalog": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "12",
  },
  "analysis": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "13",
  },
  "lesson": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "14",
  },
  "library": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "15",
  },
  "forum": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "16",
  },
  "kinder": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "17",
  },
  "game-app": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "18",
  },
  "teacher": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "19",
  },
  "calendar": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "20",
  },
  "academic-content": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "21",
  },
  "arcade":{
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "22",
  },
  "challanges": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "23",
  },
  "enrollments": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "24",
  },
  "payment": {
    Username: null,
    InvitationCode: null,
    Role: null,
    AreaId: "25",
  }
}
const THROTTLE_INTERVAL_MS = 60 * 1000; // 1 minute

/**
 * Get { payload: Object, timestamp: 0 } from sessionStorage.
 */
function GetAreaId() {
    const json = SessionManager.getLastAreaId();
    return json != null || json !== undefined ? json : {};
}

/**
 * Saves the map { payload: Object, timestamp: 0 } to sessionStorage.
 */
function SaveAreaId(areaIdMap) {
    SessionManager.setLastAreaId(areaIdMap);
}

/**
 * Logs a user's access event to the backend, throttling repeated events for the same area within a defined interval.
 *
 * This function enriches the payload with the current project environment, checks if a similar event was recently sent,
 * and only sends the log if it is new or the throttle interval has passed. The event is sent in the background and errors
 * are silently handled to avoid disrupting the user experience.
 *
 * @param {Object} payload - The access log event payload.
 * @param {string} payload.UserName - User id.
 * @param {string} payload.InvitationCode - User invitation code.
 * @param {string} payload.Role - Role of the user (Student, Teacher, Director).
 * @param {string} payload.ProjectEnvironment - Environment of the project (private-beta, private, msj-beta, msj).
 * @param {string} payload.AreaId - Id of the access area.
 * @param {string} [payload.LessonId] - Id of the lesson.
 * @param {string} [payload.LessonTitle] - Title of the lesson.
 * @param {string} [payload.Subject] - Subject code or name.
 * @param {string} [payload.Grade] - Grade or level.
 * @param {string} [payload.Details] - Additional details or context.
 * @param {string} [payload.PlanId] - Id of the plan.
 * @param {string} [payload.PlanName] - Name of the plan.
 * @param {string} [payload.AssignmentId] - Id of the assignment.
 * @param {string} [payload.AssignmentTitle] - Title of the assignment.
 * @param {string} [payload.ForumId] - Id of the forum.
 * @param {string} [payload.ForumTitle] - Title of the forum.
 * @param {string} [payload.SubscriptionId] - Id of the subscription.
 * @param {string} [payload.PlanSubscriptionId] - Id of the plan subscription.
 * @param {string} [payload.PlanSubscriptionName] - Name of the plan subscription.
 * @param {string} [payload.QuantityEnrollment] - Quantity of enrollments for the plan.
 * @param {object} [payload.ProfileInformation] - Information about student profile like UserId, Role, InvitationCode, UserName, GiveName, SurName.
 * @param {object} [payload.ClientInformation] - Information when the new users try to register and payment like GiveName, SurName, Email, Country, State (if applicable).
 * @param {string} [payload.CreatedAt] - Date and time when the record was created (ISO string).
 * @returns {Promise<void>} Resolves when the log is sent or throttled.
 */
export function SendAccessLog(payload) {
  const now = Date.now();
  const projectEnv = Settings.PROJECT_ENVIRONMENT;
  payload.ProjectEnvironment = projectEnv;

  const fetchAccessLog = async () => {
    try {
      let lastSentMap = GetAreaId();
      const last = lastSentMap || { payload: null, timestamp: 0 };

      const currentPayload = JSON.stringify(payload);

      if (last.payload === currentPayload && (now - last.timestamp) < THROTTLE_INTERVAL_MS) {
        return;
      }

      lastSentMap = {
        payload: currentPayload,
        timestamp: now
      };
      SaveAreaId(lastSentMap);

      const tokenSession = SessionManager.getSession();

      await fetch(Settings.getAccessInfoAPI(), {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Token': tokenSession
        },
        body: JSON.stringify(payload),
        keepalive: true
      }).catch(err => {
        console.error('Background logging failed:', err);
      });
    }
    catch (error) {
      // empty catch block to avoid breaking the application
    }
  }
  fetchAccessLog().then();
}

/**
 * Sends a plan access log event to the backend.
 * Use this function only if the user does not have a logged-in session.
 * The payload will be enriched with the current project environment before sending.
 * Errors are handled silently to avoid disrupting the user experience.
 */
export function SendPlanAccessLog(payload){

  const projectEnv = Settings.PROJECT_ENVIRONMENT;
  payload.ProjectEnvironment = projectEnv;

  const fetchAccessLog = async () => {
    try {

      const tokenSession = SessionManager.getSession();

      await fetch(Settings.getPlanAccessAPI(), {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Token': tokenSession
        },
        body: JSON.stringify(payload),
        keepalive: true
      }).catch(err => {
        console.error('Background logging failed:', err);
      });
    }
    catch (error) {
      // empty catch block to avoid breaking the application
    }
  }
  fetchAccessLog().then();
}
