import React from "react";
import {
    ResponsiveContainer,
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    Legend
} from "recharts";
import { withLocalize, Translate } from "react-localize-redux";
import { PulseLoader } from "react-spinners";
import { MONTH_KEYS, DAY_KEYS, buildSkeleton, tickTranslate, tooltipLabel } from "../chart-skeleton";
import LanguageSwitch, { getLanguageSection } from "../language-switch";
import ReadingComprehensionTrendChart from "./reading-comprehension-trend-chart";

const getMetrics = (record = {}) => ({
    precision: record.AvgAccuracyScore ?? 0,
    fluency: record.AvgFluencyScore ?? 0,
    completeness: record.AvgCompletenessScore ?? 0,
    pronunciation: record.AvgPronunciationScore ?? 0,
});

// Picks the records of the selected language from the performance metrics response
export const getPerformanceRecords = (performanceMetrics, language) => {
    return getLanguageSection(performanceMetrics, language) ?? [];
};

// show one record per day Mon–Sun (use week granularity by default)
const toMondayIndex = (day) => {
    return day === 0 ? 6 : day - 1;
};

// Builds skeleton for the reading performance metrics chart
export const buildPerformanceMetricsSkeleton = (granularity, records, selectedPeriod) => {
    if (granularity === "year") {
        // show one record per month
        const byMonth = {};

        records.forEach((record) => {
            byMonth[record.Month] = record;
        });

        return MONTH_KEYS.map((label, index) => {
            const record = byMonth[index + 1];

            return {
                month: label,
                ...getMetrics(record)
            };
        });
    }

    if (granularity === "month") {
        const monthRef = selectedPeriod?.dateFrom ? new Date(`${selectedPeriod.dateFrom}T00:00:00`) : new Date();

        const monthKey = MONTH_KEYS[monthRef.getMonth()];

        // get number of days in the month
        const daysInMonth = new Date(monthRef.getFullYear(), monthRef.getMonth() + 1, 0).getDate();

        // show one record per calendar day
        return Array.from({ length: daysInMonth }, (_, index) => {
            const day = index + 1;

            const record = records.find((record) => {
                return (
                    record.Date &&
                    new Date(`${record.Date}T00:00:00`).getDate() === day
                );
            });

            return {
                month: String(day),
                monthKey,
                ...getMetrics(record)
            };
        });
    }

    // show one record per day Mon–Sun (use week granularity by default)
    const byWeekday = {};

    records.forEach((record) => {
        if (!record.Date) return;

        const weekday = new Date(`${record.Date}T00:00:00`).getDay();
        const index = toMondayIndex(weekday);

        byWeekday[index] = record;
    });

    return DAY_KEYS.map((label, index) => {
        const record = byWeekday[index];

        return {
            month: label,
            ...getMetrics(record)
        };
    });
};

// Configura las series de niveles de comprensión (4 niveles)
const comprehensionSeries = [
    {
        key: "level_literal",
        labelKey: "dashboards.progress-and-mastery.reading-levels.literal", // e.g., "Literal"
        stroke: "#2563eb",
        fill: "#2563eb"
    },
    {
        key: "level_inferential",
        labelKey: "dashboards.progress-and-mastery.reading-levels.inferential", // e.g., "Inferencial"
        stroke: "#16a34a",
        fill: "#16a34a"
    },
    {
        key: "level_critical",
        labelKey: "dashboards.progress-and-mastery.reading-levels.critical", // e.g., "Crítico"
        stroke: "#f59e0b",
        fill: "#f59e0b"
    },
    {
        key: "level_appreciative",
        labelKey: "dashboards.progress-and-mastery.reading-levels.appreciative", // e.g., "Apreciativo/Creativo"
        stroke: "#ef4444",
        fill: "#ef4444"
    }
];

// Configura las series de métricas cuantitativas (opcional)
const metricsSeries = [
    {
        key: "precision",
        labelKey: "dashboards.progress-and-mastery.reading-metrics.precision", // "Precisión"
        stroke: "#0ea5e9",
        fill: "#0ea5e9"
    },
    {
        key: "fluency",
        labelKey: "dashboards.progress-and-mastery.reading-metrics.fluency", // "Fluidez"
        stroke: "#22c55e",
        fill: "#22c55e"
    },
    {
        key: "completeness",
        labelKey: "dashboards.progress-and-mastery.reading-metrics.completeness", // "Completitud"
        stroke: "#a855f7",
        fill: "#a855f7"
    },
    {
        key: "pronunciation",
        labelKey: "dashboards.progress-and-mastery.reading-metrics.pronunciation", // "Pronunciación"
        stroke: "#fb7185",
        fill: "#fb7185"
    }
];


const ChartFrame = ({
                        titleId,
                        data,
                        series,
                        translate,
                        yAxisLabelId = "dashboards.progress-and-mastery.level"
                    }) => (
    <>
        <div className="charts-title" style={{ marginBottom: 8 }}>
            <Translate id={titleId} />
        </div>
        <div style={{ width: "100%", height: 330 }}>
            <ResponsiveContainer width="100%" height="100%">
                <AreaChart data={data} margin={{ top: 12, right: 16, left: 16, bottom: 12 }}>
                    <CartesianGrid strokeDasharray="3 3" stroke="rgba(0,0,0,0.08)" />
                    <XAxis
                        dataKey="month"
                        axisLine={false}
                        tickLine={false}
                        tickFormatter={tickTranslate(translate)}
                    />
                    <YAxis
                        domain={[0, 100]}
                        axisLine={false}
                        tickLine={false}
                        label={{
                            value: translate(yAxisLabelId),
                            angle: -90,
                            position: "insideLeft",
                            offset: 10,
                            style: { textAnchor: "middle", fontSize: "15px" }
                        }}
                    />
                    <Tooltip
                        labelFormatter={tooltipLabel(translate)}
                        formatter={(value, name, props) => {
                            // Mapea el dataKey a su label traducida
                            const s = series.find((ss) => ss.key === props.dataKey);
                            const label = s ? translate(s.labelKey) : name;
                            return [value, label];
                        }}
                        cursor={{ stroke: "rgba(0,0,0,0.15)" }}
                        contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
                    />
                    <Legend
                        formatter={(value) => {
                            const s = series.find((ss) => ss.key === value);
                            return s ? translate(s.labelKey) : value;
                        }}
                    />
                    {series.map((s) => (
                        <Area
                            key={s.key}
                            type="monotone"
                            dataKey={s.key}
                            stroke={s.stroke}
                            fill={s.fill}
                            fillOpacity={0.25}
                            name={s.key}
                            activeDot={{ r: 4 }}
                        />
                    ))}
                </AreaChart>
            </ResponsiveContainer>
        </div>
    </>
);

const ReadingLevelChart = ({ translate, data, performanceMetrics = {}, granularity = "week", selectedPeriod,
                               storyLanguage = "en", onStoryLanguageChange = () => {}, loading = false, readingLessonsSeries = [] }) => {
    const languageRecords = getPerformanceRecords(performanceMetrics, storyLanguage);
    const hasMetricsData = languageRecords.length > 0;

    const metricsData = buildPerformanceMetricsSkeleton(granularity, languageRecords, selectedPeriod);

    return (
        <>
            <div style={{ marginBottom: 14 }}>
                <LanguageSwitch language={storyLanguage} onChange={onStoryLanguageChange} />
            </div>

            {/* Gráfica 1: Niveles de comprensión */}
            {/* Hidden for now since it's using mocked data */}
            {/* <ChartFrame
                titleId="dashboards.progress-and-mastery.achieved-reading-levels" // Nuevo: plural
                data={data}
                series={comprehensionSeries}
                translate={translate}
                yAxisLabelId="dashboards.progress-and-mastery.level" // 0-100
            /> */}

            {/* Gráfica 2 (opcional): Métricas cuantitativas (precisión, fluidez, completitud, pronunciación) */}
            <div style={{ marginTop: 24 }}>
                {loading ? (
                    <div style={{ display: "flex", alignItems: "center", justifyContent: "center", height: 330 }}>
                        <PulseLoader size={8} color="#266fc2ff" />
                    </div>
                ) : hasMetricsData ? (
                    <ChartFrame
                        titleId="dashboards.progress-and-mastery.reading-performance-metrics"
                        data={metricsData}
                        series={metricsSeries}
                        translate={translate}
                        yAxisLabelId="dashboards.progress-and-mastery.score"
                    />
                ) : (
                    <>
                        <div className="charts-title" style={{ marginBottom: 8 }}>
                            <Translate id="dashboards.progress-and-mastery.reading-performance-metrics" />
                        </div>
                        <div
                            className="d-flex align-items-center justify-content-center"
                            style={{ width: "100%", height: 330, color: '#94a3b8', fontSize: 14 }}
                        >
                            <Translate id="dashboards.usage-and-engagement.no-results" />
                        </div>
                    </>
                )}
            </div>

            <ReadingComprehensionTrendChart
                series={readingLessonsSeries}
                granularity={granularity}
                selectedPeriod={selectedPeriod}
                loading={loading}
            />
        </>
    );
};

export default withLocalize(ReadingLevelChart);
