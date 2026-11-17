import { history } from '../index';

/**
 * Created by portiz on 9/20/18.
 */
class SessionManager {
    /**
     * Sets the session
     * @param sessionDetails the session details of the connected user.
     */
    static setSession(sessionDetails) {
        localStorage.setItem('auth', JSON.stringify({
            "InvitationCode": sessionDetails.InvitationCode,
            "Roles": sessionDetails.Roles,
            "Token": sessionDetails.Token,
            "Name": sessionDetails.Name,
            "Username": sessionDetails.Username,
            "ProjectEnvironment": sessionDetails.ProjectEnvironment
        }));
    }

    /**
     * Set for Microsoft Teams.
     */
    static setMicrosoftTeams() {
        localStorage.setItem('mst', "1");
    }

    /**
     * Get Microsoft Teams
     * @returns {boolean}
     */
    static getIfMicrosoftTeams() {
        return localStorage.getItem('mst') !== null;
    }

    /**
     * Sets the selected lesson to show.
     * @param lessonId Id of the lesson
     * @param planningId Planning Id
     * @param planningName Planning name
     */
    static setSelectedLessonToShow(lessonId, planningId, planningName) {
        sessionStorage.setItem('lsd', JSON.stringify({
            lessonId: lessonId,
            planningId: planningId,
            planningName: planningName,
        }));
    }

    /**
     * Gets the selected lesson to show.
     * @returns {null|any}
     */
    static getSelectedLessonToShow() {
        try {
            let lessonToShow = sessionStorage.getItem('lsd');
            if (lessonToShow == null)
                return null;
            else
                return JSON.parse(lessonToShow);
        }
        catch (e) {
            return null;
        }
    }

    /**
     * Set the selected course
     * @param course The course Id
     */
    static setSelectedCourse(course) {
        localStorage.setItem('crs', JSON.stringify(course));
    }

    /**
     * Get the selected course
     * @returns {string|null}
     */
    static getSelectedCourse() {
        let course = localStorage.getItem('crs');
        if (typeof course === 'undefined' || course === null)
            return null;
        else
            return JSON.parse(course);
    }

    /**
     * Returns the session
     * @returns {*}
     */
    static getSession() {
        try {
            let session = localStorage.getItem('auth');
            if (session != null) {
                return JSON.parse(session).Token;
            }
            else {
                return null;
            }
        }
        catch (e) {
            return null;
        }
    }

    /** Returns the user role.
     * @returns {null|*} User role.
     */
    static getUserRole(){
        try {
            let session = localStorage.getItem('auth');
            if (session != null) {
                return JSON.parse(session).Roles[0];
            }
            else {
                return null;
            }
        }
        catch (e) {
            return null;
        }
    }

    static isAuth = () => {
        return localStorage.getItem("auth") !== null;
    };

    static getSessionDetails() {
        try {
            let session = localStorage.getItem('auth');
            if (session != null) {
                let sessionParsed = JSON.parse(session);
                if(typeof sessionParsed.Roles === undefined || sessionParsed.Roles === null) {
                    return null;
                }

                return sessionParsed;
            }
            else
                return null;
        }
        catch (e) {
            return null;
        }
    }

    /**
     * Evaluates if session is active or not
     * @returns {boolean}
     */
    static evaluateSession() {
        return (SessionManager.getSession() != null && SessionManager.getSession().length > 0);
    }

    /**
     * Deletes a session
     */
    static deleteSession() {
        localStorage.removeItem('auth');
        localStorage.removeItem('crs');
        sessionStorage.removeItem("etype");
        this.deleteSelectedEntity();
        sessionStorage.removeItem("selected-planning");
        sessionStorage.removeItem("selected-group");
        sessionStorage.removeItem("selected-forum");
        window.location.href = "./";
    }

    static deleteSelectedEntity() {
        sessionStorage.removeItem("entity");
    }

    /**
     * Sets the last selected entity type id.
     * @param e
     */
    static setLastEntityTypeId(e) {
        sessionStorage.setItem("etype", e);
    }

    /**
     * Returns the last selected entity type id.
     * @returns {string}
     */
    static getLastEntityTypeId() {
        return sessionStorage.getItem("etype");
    }

    /**
     * Saves the selected entity id.
     * @param e
     */
    static setLastEntityId(e) {
        sessionStorage.setItem("entity", e);
    }

    /**
     * Sets a selected planning.
     * @param planningId Id of the plan
     * @param planningName Planning name.
     */
    static setSelectedPlanning(planningId, planningName, planningClosed, openDate, closeDate, closure, subjectName, subjectCode, levelCode) {
        sessionStorage.setItem("selected-planning", JSON.stringify({
            planningId,
            planningName,
            planningClosed,
            openDate,
            closeDate,
            closure,
            subjectName,
            subjectCode,
            levelCode
        }));
    }

    /**
     * Sets the selected group.
     * @param groupId Id of the group
     * @param groupName Name of the group
     */
    static setSelectedGroup(groupId, groupName) {

        sessionStorage.setItem("selected-group", JSON.stringify({
            groupId: groupId,
            groupName: groupName
        }));
    }

    /**
     * Returns the last selected group
     * @returns {null|any}
     */
    static getSelectedGroup() {
        let selectedGroup = sessionStorage.getItem("selected-group");

        if (selectedGroup == null)
            return null;
        else
            return JSON.parse(selectedGroup);
    }

    /**
     * returns the selected planning.
     */
    static getSelectedPlanning() {
        let selectedPlanning = sessionStorage.getItem("selected-planning");

        if (selectedPlanning == null)
            return null;
        else
            return JSON.parse(selectedPlanning);
    }

    /**
     * Sets a selected forum.
     * @param forumId Id of the forum
     */
    static setSelectedForum(forumId) {
        sessionStorage.setItem("selected-forum", forumId);
    }

    /**
     * returns the selected forum.
     */
    static getSelectedForum() {
        return sessionStorage.getItem("selected-forum");
    }

    /**
     * Sets a parent screen forum.
     * @param screem of the forum
     */
    static setParentScreenForum(screem) {
        sessionStorage.setItem("parent-forum-screen", screem);
    }

    /**
     * returns the selected parent screen forum.
     */
    static getParentScreenForum() {
        return sessionStorage.getItem("parent-forum-screen");
    }

    static getUserRole() {
        try {
            let session = localStorage.getItem('auth');
            if (session != null) {
                return JSON.parse(session).Roles[0];
            }
            else {
                return null;
            }
        }
        catch (e) {
            return null;
        }
    }

    /**
     * @returns breadcrumbs paths
     */
    static getBreadcrumb() {
        return sessionStorage.getItem("breadcrumb-paths");
    }

    /**
    * set breadcrumbs paths
    */
    static setBreadcrumb(paths) {
        sessionStorage.setItem("breadcrumb-paths", paths);
    }

    /**
     * reset breadcrumbs paths
     */
    static resetBreadcrumb() {
        sessionStorage.removeItem("breadcrumb-paths");
    }

        /**
     * Sets a Assignment Id and title in sessionStorage.
     * @param assignmentId
     * @param assignmentTitle
     */
    static setSelectedAssignment(assignmentId, assignmentTitle) {
        sessionStorage.setItem("selected-assignment", JSON.stringify({
            assignmentId: assignmentId,
            assignmentTitle: assignmentTitle
        }));
    }

    /**
     * Returns the selected assignment.
     * @returns {null|any}
     * */
    static getSelectedAssignment() {
        let selectedAssignment = sessionStorage.getItem("selected-assignment");

        if (selectedAssignment == null)
            return null;
        else
            return JSON.parse(selectedAssignment);
    }

    /**
     * Sets a Lesson Id and title in sessionStorage.
     * @param lessonId
     * @param lessonTitle
     */
    static setSelectedLesson(lessonId, lessonTitle) {
        sessionStorage.setItem("selected-lesson", JSON.stringify({
            lessonId: lessonId,
            lessonTitle: lessonTitle
        }));
    }

    /**
     * Returns the selected lesson.
     * @returns {null|any}
     */
    static getSelectedLesson() {
        let selectedLesson = sessionStorage.getItem("selected-lesson");

        if (selectedLesson == null)
            return null;
        else
            return JSON.parse(selectedLesson);
    }



    /**
     * Sets the last AreaId in sessionStorage.
     * @param areaId
     */
    static setLastAreaId(areaId) {
        if (typeof areaId === 'undefined' || areaId === null)
            sessionStorage.removeItem("access-logger");
        else
            sessionStorage.setItem("access-logger", JSON.stringify(areaId));
    }

    /**
     * Returns the Last AreaId from sessionStorage.
     * @returns {string|null}
     */
    static getLastAreaId() {
        let areaId = sessionStorage.getItem("access-logger");
        if (typeof areaId === 'undefined' || areaId === null)
            return {};
        else
            return JSON.parse(areaId);
    }
}

export default SessionManager;
