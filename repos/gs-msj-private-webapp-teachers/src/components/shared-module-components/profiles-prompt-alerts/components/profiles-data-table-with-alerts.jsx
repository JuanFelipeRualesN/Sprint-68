import React, { useState, useEffect, useRef } from "react";
import { Button, Input } from "reactstrap";
import { useTranslation } from "react-i18next";
import { faSearch, faTrash, faUserTimes } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";
import customParseFormat from "dayjs/plugin/customParseFormat";

// Shared pieces reused from the host project.
import LoaderFullWindow from "../../loader-windows/loader-full-window";
import PaginationCustom from "../../datatables/pagination";
import DataTable from "../../datatables/datatable";
import "../../datatables/style-datatables.css";
import {
  getSubscriptionProfiles,
  getProfiles,
} from "../../../../api/users";
import { getTimeZone, convertMaxLengthSting } from "../../../../utils/utils";

// New, self-contained functionality for this feature.
import PromptAlertIndicator from "./prompt-alert-indicator";
import AlertIcon from "./alert-icon";
import { fetchUsersPromptSummary, fetchUnreadAlertsTotal } from "../prompt-monitoring-data";
import "./prompt-unread-banner.css";

dayjs.extend(utc);
dayjs.extend(timezone);
dayjs.extend(customParseFormat);

/**
 * Drop-in variant of ProfilesDataTable that adds a "prompt alerts" column.
 *
 * It keeps the exact same props/behavior as the original ProfilesDataTable so
 * it can replace it wherever the alerts column is wanted, plus one optional
 * prop to keep the feature portable:
 *
 * @param {string} [alertsColumnLabel] Header label for the alerts column.
 *        Defaults to the "prompt-alerts.column-title" translation.
 *
 * The alerts value comes from the prompt-monitoring summary endpoint, queried in
 * batch for the users of the current page.
 */
const ProfilesDataTableWithAlerts = ({
  addRemoveButton,
  isStaff,
  roleId,
  subscriptionId,
  redoSearch,
  updateParent,
  selectable = true,
  studentNotificationsInfo,
  isForLearningGapProfiles,
  removeUser,
  alertsColumnLabel,
}) => {
  const [currentPage, setCurrentPage] = useState(0);
  const [data, setData] = useState([]);
  const [firstName, setFirstName] = useState(null);
  const [lastName, setLastName] = useState(null);
  const [limiter] = useState(10);
  const [loading, setLoading] = useState(false);
  const [totalPages, setTotalPages] = useState(1);
  const [username, setUsername] = useState(null);
  const [triggerSearch, setTriggerSearch] = useState(false);
  // Viewer-wide count of unread alerts across every page (not just this one).
  const [unreadAlertsTotal, setUnreadAlertsTotal] = useState(0);

  // Identifies the latest search so a slow alerts response from a previous
  // search can't overwrite the rows of a newer one.
  const searchRequestId = useRef(0);

  const { t, i18n } = useTranslation(["users"]);

  const alertsHeaderLabel = alertsColumnLabel || t("prompt-alerts.column-title");

  // Order MUST match the key order produced in convertDataToDataTable, because
  // DataTable maps row-object keys to headers positionally.
  const headerOptions = [
    { key: "0", name: t("users-datatable.username") },
    { key: "1", name: t("users-datatable.first-name") },
    { key: "2", name: t("users-datatable.last-name") },
    { key: "3", name: t("users-datatable.invitation-code") },
    { key: "4", name: t("users-datatable.last-session") },
    { key: "5", name: t("users-datatable.record-of-school") },
    { key: "6", name: alertsHeaderLabel },
    { key: "7", name: "" },
  ];

  const COLUMN_USERNAME = t("users-datatable.username");

  useEffect(() => {
    doSearch();
  }, [currentPage, triggerSearch, redoSearch, i18n.language]);

  // Viewer-wide unread-alerts badge, independent of the table's pagination. It
  // refreshes on mount and when the parent asks for a new search (e.g. after
  // returning from a student's history, which marks their alerts as read). On
  // failure we KEEP the last known value instead of painting a false zero.
  useEffect(() => {
    let cancelled = false;

    (async () => {
      try {
        const total = await fetchUnreadAlertsTotal();
        if (!cancelled) setUnreadAlertsTotal(total);
      } catch (error) {
        console.error("Error fetching unread alerts total:", error);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [redoSearch]);

  const doSearch = async () => {
    const requestId = ++searchRequestId.current;
    setLoading(true);
    try {
      const response = isStaff
        ? await getSubscriptionProfiles(subscriptionId, firstName, lastName, username, limiter, currentPage, roleId)
        : await getProfiles(firstName, lastName, username, currentPage, limiter, roleId);

      const userInfo = response.data.UserInfo || [];
      setTotalPages(response.data.PageTotals);

      // Render the profiles right away with no counts. The alerts column is
      // auxiliary and must never delay or block the table.
      convertDataToDataTable(userInfo, {});

      // Fill the alerts column asynchronously (fire-and-forget): a slow or
      // failing summary call can't hold up the listing.
      loadPromptAlertsForPage(userInfo, requestId);
    } catch (error) {
      console.error("Error fetching profiles:", error);
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches the prompt-alerts summary for the given page and re-renders the
   * rows with the counts once it arrives. Off the critical path, so a failure
   * just leaves the column empty. Ignores its result if a newer search has
   * started in the meantime (stale response).
   */
  const loadPromptAlertsForPage = async (userInfo, requestId) => {
    if (userInfo.length === 0) return;
    try {
      const summary = await fetchUsersPromptSummary(userInfo.map((user) => user.UserId));
      if (requestId !== searchRequestId.current) return;
      convertDataToDataTable(userInfo, summary.byUser);
    } catch (error) {
      console.error("Error fetching prompt alerts summary:", error);
    }
  };

  /**
   * Formats the UTC last session returned by the API into the user's time zone.
   * @param {string} lastSession Date as "MM/DD/YYYY HH:mm:ss" in UTC.
   * @returns {string}
   */
  const formatLastSession = (lastSession) =>
    lastSession
      ? dayjs
          .utc(lastSession, "MM/DD/YYYY HH:mm:ss")
          .tz(getTimeZone())
          .format("MM/DD/YYYY hh:mm a")
      : "";

  const convertDataToDataTable = (dataToConvert, alertsByUser = {}) => {
    const newArray = dataToConvert.map((user) => {
      const alerts = alertsByUser[user.UserId] || {};
      const unreadCount = alerts.unreadCount || 0;

      const row = {
        Username: user.Username,
        GivenName: user.GivenName,
        Surname: user.Surname,
        Token: user.Token,
        LastSession: formatLastSession(user.LastSession),
        UserSchoolRecord: user.UserSchoolRecord,
        // React element => rendered as-is by DataTable; empty when no alerts.
        PromptAlerts: (
          <PromptAlertIndicator
            unreadCount={unreadCount}
            label={t(unreadCount === 1 ? "prompt-alerts.new-alert-singular" : "prompt-alerts.new-alert-plural")}
            title={alertsHeaderLabel}
          />
        ),
        Action: null,
        value: user.UserId,
      };

      row.Action = (
        <>
          {selectable && (
            <Button color="info" onClick={() => updateParent(user)}>
              {" "}
              {t("users-datatable.select")}{" "}
            </Button>
          )}
          {addRemoveButton && (
            <Button
              color="danger"
              style={{ marginLeft: "5px" }}
              onClick={() => removeUser(user)}
            >
              <FontAwesomeIcon icon={faTrash} />
            </Button>
          )}
        </>
      );

      return row;
    });

    setData(newArray);
  };

  const filter = () => {
    setCurrentPage(0);
    setTriggerSearch((prev) => !prev); // toggle to force effect run
  };

  return (
    <div>
      {unreadAlertsTotal > 0 && (
        <div className="prompt-unread-banner" role="status">
          <AlertIcon />
          <span>
            {t(
              unreadAlertsTotal === 1
                ? "prompt-alerts.unread-alerts-banner-singular"
                : "prompt-alerts.unread-alerts-banner-plural",
              { count: unreadAlertsTotal }
            )}
          </span>
        </div>
      )}
      <div className="wrapper-col mb-4">
        <div className="four-col">
          <div className="form-group-fields">
            <div className="label">{t("users-datatable.first-name")}</div>
            <Input type="text" value={firstName || ""} onChange={(e) => setFirstName(convertMaxLengthSting(e.target.value, 100))} />
          </div>
        </div>
        <div className="four-col">
          <div className="form-group-fields">
            <div className="label">{t("users-datatable.last-name")}</div>
            <Input type="text" value={lastName || ""} onChange={(e) => setLastName(convertMaxLengthSting(e.target.value, 100))} />
          </div>
        </div>
        <div className="four-col">
          <div className="form-group-fields">
            <div className="label">{t("users-datatable.username")}</div>
            <Input type="text" value={username || ""} onChange={(e) => setUsername(convertMaxLengthSting(e.target.value, 50))} />
          </div>
        </div>
        <div className="four-col mt-3">
          <button className="btn-w-icon" onClick={filter}>
            <FontAwesomeIcon icon={faSearch} /> {t("users-datatable.filter")}
          </button>
        </div>
      </div>
      {data.length > 0 ? (
        <DataTable
          headerOptions={headerOptions}
          data={data}
          updateParent={updateParent}
          isForLearningGapProfiles={isForLearningGapProfiles}
          columnName={COLUMN_USERNAME}
          studentNotificationsInfo={studentNotificationsInfo}
        />
      ) : (
        <div className="module-notfound">
          <FontAwesomeIcon icon={faUserTimes} /> {t("users-datatable.no-found")}
        </div>
      )}
      <PaginationCustom
        totalPages={totalPages}
        currentPage={currentPage}
        updateParent={(page) => setCurrentPage(page)}
        dapper={true}
      />
      {loading && <LoaderFullWindow />}
    </div>
  );
};

export default ProfilesDataTableWithAlerts;
