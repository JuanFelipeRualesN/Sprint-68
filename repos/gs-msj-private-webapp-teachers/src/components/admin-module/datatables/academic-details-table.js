import React from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import { Translate } from "react-localize-redux";
import moment from 'moment';
import "../styles/style.css";

const AcademicDetailsTable = ({ details, updateParent, buttonTxt }) => {

    const handleSelectedItem = (item) => {
        updateParent(item);
    };

    return (
        <>
            <div className="wrapper-table-admin mt-3">
                <table className="content-table-admin">
                    <thead>
                        <tr>
                            <th width="60">ID</th>
                            <th><Translate id="school-academics-details.term" /></th>
                            <th><Translate id="school-academics-details.detail" /></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {details.map((detail) => (
                            <tr>
                                <td>{detail.Id}</td>
                                <td><span className="txt-LighterBlue">{moment(detail.FullTermStartDate, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY") + " - " + moment(detail.FullTermEndDate, "M/DD/YYYY hh:mm:ss A").format("MM/DD/YYYY")}</span></td>
                                <td>
                                    <p><strong><Translate id="wizard.step-4.periods-quantity" />:&ensp;</strong>{detail.PeriodQuantity}</p>
                                    <p><strong><Translate id="wizard.step-4.days-number-for-closing-term" />:&ensp;</strong>{detail.DaysToClosePeriod}</p></td>
                                <td>
                                    <button type='button' className="btn-default button-secondary-custom" onClick={() => handleSelectedItem(detail)}>
                                        <Translate id={`school-academics-details.${buttonTxt}`} />
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </>
    );

}

export default AcademicDetailsTable;