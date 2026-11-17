import { buildSkeleton } from "../chart-skeleton";

export const buildContentSkeleton = (granularity, pagesByPeriod, selectedPeriod) =>
    buildSkeleton(granularity, pagesByPeriod, selectedPeriod, "TotalPages", "pages");

export const buildAvgTimeSkeleton = (granularity, avgTimeByPeriod, selectedPeriod) =>
    buildSkeleton(granularity, avgTimeByPeriod, selectedPeriod, "AvgTimeSeconds", "avgTime");
