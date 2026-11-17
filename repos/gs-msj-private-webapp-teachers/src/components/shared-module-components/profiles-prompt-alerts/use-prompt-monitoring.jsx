/**
 * Prompt-monitoring state for the profile screen.
 *
 * Concentrates every piece of state the prompt-alerts blocks need for the
 * selected user — the banner flagged count, the paginated tutor prompt history,
 * the date-range filter and the academic-period presets — so the screen stays
 * presentational and just wires the returned values into <PromptSafetyAlert> and
 * <TutorPromptHistory>.
 */

import { useEffect, useRef, useState } from "react";
import { getAcademicYears } from "../../../api/schools";
import { fetchTutorPromptHistory, fetchUserFlaggedCount } from "./prompt-monitoring-data";

// How many prompts a history page shows.
const HISTORY_PAGE_SIZE = 10;


/**
 * Converts an API date ("MM/DD/YYYY HH:mm:ss") to the "YYYY-MM-DD" string an
 * <input type="date"> expects.
 * @param {string} apiDate
 * @returns {string} "" when the value is missing/malformed.
 */
const apiDateToInputValue = (apiDate) => {
  if (typeof apiDate !== "string") {
    return "";
  }
  const [datePart] = apiDate.split(" ");
  const [month, day, year] = datePart.split("/");
  if (!month || !day || !year) {
    return "";
  }
  return `${year}-${month.padStart(2, "0")}-${day.padStart(2, "0")}`;
};

/**
 * @param {Object} params
 * @param {boolean} params.enabled Whether the prompt-alerts feature is active.
 * @param {string|number} [params.userId] Selected user; changing it resets the
 *        paging and date filter so a new selection starts clean.
 */
export const usePromptMonitoring = ({ enabled, userId }) => {
  const [flaggedCount, setFlaggedCount] = useState(0);
  const [prompts, setPrompts] = useState([]);
  const [total, setTotal] = useState(0);
  const [pageSize, setPageSize] = useState(HISTORY_PAGE_SIZE);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  // Date range filter (YYYY-MM-DD, "" = unbounded).
  const [from, setFrom] = useState("");
  const [to, setTo] = useState("");
  // Academic periods offered as quick date-range presets.
  const [periods, setPeriods] = useState([]);

  const totalPages = Math.max(1, Math.ceil(total / (pageSize || 1)));

  // Reset paging and the date filter when the selection changes, during render
  // so the fetch effects below run once with the fresh values (no double fetch).
  const lastUserId = useRef(userId);
  if (lastUserId.current !== userId) {
    lastUserId.current = userId;
    setPage(0);
    setFrom("");
    setTo("");
  }

  // Changing the date range restarts the history at the first page.
  const setDateRange = ({ from: nextFrom, to: nextTo }) => {
    setPage(0);
    setFrom(nextFrom);
    setTo(nextTo);
  };

  // Banner flagged total: accurate count across every page, so it only depends
  // on the selection, not on paging or the date filter.
  useEffect(() => {
    if (!enabled || !userId) {
      setFlaggedCount(0);
      return;
    }

    let cancelled = false;

    fetchUserFlaggedCount(userId)
      .then((flagged) => { if (!cancelled) setFlaggedCount(flagged); })
      .catch((error) => {
        if (cancelled) return;
        console.error("Error fetching flagged count:", error);
        setFlaggedCount(0);
      });

    return () => { cancelled = true; };
  }, [enabled, userId]);

  // One page of the tutor prompt history, refetched on page/date-range changes.
  useEffect(() => {
    if (!enabled || !userId) {
      setPrompts([]);
      setTotal(0);
      return;
    }

    let cancelled = false;
    setLoading(true);

    // Expand the picked days to inclusive boundaries so the range covers the
    // whole "to" day (the picker gives date-only values).
    const fromIso = from ? `${from}T00:00:00` : undefined;
    const toIso = to ? `${to}T23:59:59` : undefined;

    fetchTutorPromptHistory(userId, page + 1, HISTORY_PAGE_SIZE, fromIso, toIso)
      .then((history) => {
        if (cancelled) return;
        setPrompts(history.prompts);
        setTotal(history.total);
        setPageSize(history.pageSize);
      })
      .catch((error) => {
        if (cancelled) return;
        console.error("Error fetching tutor prompt history:", error);
        setPrompts([]);
        setTotal(0);
      })
      .finally(() => { if (!cancelled) setLoading(false); });

    return () => { cancelled = true; };
  }, [enabled, userId, page, from, to]);

  // Academic years offered as quick date-range presets. The history spans every
  // period, so we bring them ALL (getAcademicYears) and each option filters the
  // history by that term's start/end dates. A failure just leaves the dropdown
  // empty (hidden).
  useEffect(() => {
    if (!enabled) {
      setPeriods([]);
      return;
    }

    let cancelled = false;

    (async () => {
      try {
        const response = await getAcademicYears();
        const items = response?.data?.Items ?? [];

        if (cancelled) return;

        setPeriods(
          items
            .map((item) => {
              const from = apiDateToInputValue(item.FullTermStartDate);
              const to = apiDateToInputValue(item.FullTermEndDate);
              return { id: String(item.Id), from, to, label: `${from} – ${to}` };
            })
            .filter((period) => period.from && period.to)
            // Most recent term first.
            .sort((a, b) => b.from.localeCompare(a.from))
        );
      } catch (error) {
        console.error("Error fetching academic years:", error);
      }
    })();

    return () => { cancelled = true; };
  }, [enabled]);

  return {
    flaggedCount,
    prompts,
    page,
    totalPages,
    loading,
    from,
    to,
    periods,
    setPage,
    setDateRange,
  };
};
