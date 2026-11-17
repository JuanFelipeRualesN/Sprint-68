import React from "react";
import { ResponsiveContainer,LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from "recharts";
import { withLocalize, Translate } from "react-localize-redux";
import { PulseLoader } from "react-spinners";
import { buildSkeleton, tickTranslate, tooltipLabel } from "../chart-skeleton";

const SERIES_KEY = "percent";

export const buildComprehensionTrendSkeleton = (granularity, series, selectedPeriod) => {
    return buildSkeleton(granularity, series ?? [], selectedPeriod, "AvgPercent", SERIES_KEY);
};

// Chart to show the trend of reading comprehension results over time, based on the average percent correct for each lesson.
const ReadingComprehensionTrendChart = ({ translate, series = [], granularity = "week", selectedPeriod, loading = false }) => {
    const hasData = (series?.length ?? 0) > 0;

    const data = buildComprehensionTrendSkeleton(granularity, series, selectedPeriod);

    return (
        <>
            <div className="charts-title" style={{ marginBottom: 8 }}>
                <Translate id="dashboards.progress-and-mastery.reading-comprehension-trend" />
            </div>

            {loading ? (
                <div style={{ display: "flex", alignItems: "center", justifyContent: "center", height: 330 }}>
                    <PulseLoader size={8} color="#266fc2ff" />
                </div>
            ) : hasData ? (
                <div style={{ width: "100%", height: 330 }}>
                    <ResponsiveContainer width="100%" height="100%">
                        <LineChart data={data} margin={{ top: 12, right: 16, left: 16, bottom: 12 }}>
                            <CartesianGrid strokeDasharray="3 3" stroke="rgba(0,0,0,0.08)" />
                            <XAxis
                                dataKey="label"
                                axisLine={false}
                                tickLine={false}
                                tickFormatter={tickTranslate(translate)}
                            />
                            <YAxis
                                domain={[0, 100]}
                                axisLine={false}
                                tickLine={false}
                                label={{
                                    value: translate("dashboards.progress-and-mastery.score"),
                                    angle: -90,
                                    position: "insideLeft",
                                    offset: 10,
                                    style: { textAnchor: "middle", fontSize: "15px" }
                                }}
                            />
                            <Tooltip
                                labelFormatter={tooltipLabel(translate)}
                                formatter={(value) => [`${value}%`, translate("dashboards.progress-and-mastery.reading-comprehension-result")]}
                                cursor={{ stroke: "rgba(0,0,0,0.15)" }}
                                contentStyle={{ borderRadius: 12, border: "1px solid #e5e7eb" }}
                            />
                            <Legend
                                formatter={() => translate("dashboards.progress-and-mastery.reading-comprehension-result")}
                            />
                            <Line
                                type="monotone"
                                dataKey={SERIES_KEY}
                                stroke="#2563eb"
                                strokeWidth={2}
                                name={SERIES_KEY}
                                dot={{ r: 3 }}
                                activeDot={{ r: 5 }}
                                connectNulls
                            />
                        </LineChart>
                    </ResponsiveContainer>
                </div>
            ) : (
                <div
                    className="d-flex align-items-center justify-content-center"
                    style={{ width: "100%", height: 330, color: "#94a3b8", fontSize: 14 }}
                >
                    <Translate id="dashboards.usage-and-engagement.no-results" />
                </div>
            )}
        </>
    );
};

export default withLocalize(ReadingComprehensionTrendChart);
