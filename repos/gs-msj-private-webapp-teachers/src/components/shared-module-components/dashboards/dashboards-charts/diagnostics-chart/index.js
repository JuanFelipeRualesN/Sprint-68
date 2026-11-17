import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faSpinner} from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import React from "react";

export const DiagnosticsChart = ({ data }) => {
    const chartStyle = {
        height: '250px',
        backgroundColor: '#ffffff',
        borderRadius: '12px',
        padding: '20px',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'space-around',
        boxShadow: '0 4px 10px rgba(0,0,0,.05)',
    };

    const barContainerStyle = {
        display: 'flex',
        justifyContent: 'space-around',
        alignItems: 'flex-end',
        height: '100%',
        paddingTop: '30px'
    };

    const maxScore = Math.max(...data.map(d => d.score), 100);

    return (
        <div style={chartStyle} className="">
            <span className="mb-4 text-center title-secondary font-weight-bold">
                <Translate id="dashboards.diagnostic-progress" />
            </span>
            <div style={barContainerStyle}>
                {data.map((item, index) => {
                    const barColor = item.status === 'completed'
                        ? (item.score >= 75 ? '#198754' : '#ffc107')
                        : '#adb5bd';

                    const barHeight = item.status === 'completed' ? (item.score / maxScore) * 100 : 30;

                    return (
                        <div key={index} style={{
                            textAlign: 'center',
                            width: '20%',
                            height: '100%',
                            position: 'relative',
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            justifyContent: 'flex-end'
                        }}>
                            {item.status === 'completed' && (
                                <span style={{ position: 'absolute', top: '-20px', width: '100%', fontWeight: 'bold', color: barColor }}>{item.score}%</span>
                            )}
                            <div style={{
                                backgroundColor: barColor,
                                width: '100%',
                                height: `${barHeight}%`,
                                minHeight: item.status === 'pending' ? '15px' : '0',
                                borderRadius: '4px',
                                transition: 'height 0.5s ease-in-out',
                                boxShadow: '0 2px 5px rgba(0,0,0,.1)'
                            }}>
                                {item.status === 'pending' && (
                                    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                                        <FontAwesomeIcon icon={faSpinner} spin size="1x" color="#ffffff" />
                                    </div>
                                )}
                            </div>
                            <div style={{ marginTop: '10px', fontSize: '0.9rem', fontWeight: '500', color: '#495057' }}>{item.month}</div>
                            {item.status === 'pending' && (
                                <small className="text-warning mt-1">
                                    <Translate id="dashboards.pending" />
                                </small>
                            )}
                        </div>
                    );
                })}
            </div>
        </div>
    );
};