/**
 * Created by portiz on 09/30/20.
 */
import React, { useState, useEffect } from 'react';
import { Table } from 'reactstrap';
import "./style.css";
/** Datatable that includes row click value **/
const DataTable = ({ data, headerOptions, totalToShowPerPage, updateParent }) => {
    const [dataState, setDataState] = useState(data ? data : []);
    const [headerOptionsState, setHeaderOptionsState] = useState(headerOptions ? headerOptions : []);
    const [totalToShowPerPageState, setTotalToShowPerPageState] = useState(totalToShowPerPage ? totalToShowPerPage : 0);

    useEffect(() => {
        setDataState(data ? data : []);
        setHeaderOptionsState(headerOptions ? headerOptions : []);
        setTotalToShowPerPageState(totalToShowPerPage ? totalToShowPerPage : 0);
    }, [data, headerOptions, totalToShowPerPage]);

    const onRowClick = (e) => {
        return updateParent(e);
    };

    let uniqueKey = 0;
    // Create header
    let trs = [];
    dataState.forEach((items) => {
        let valueItem = typeof items.value !== 'undefined' && items.value !== null ? items.value : null;
        let tds = [];

        let dataColumnIteration = 0;

        for(let item in items)
        {
            uniqueKey++;
            let keyName = (parseInt(valueItem) + parseInt(uniqueKey));
            // Push it if it is not a value. We do not need a value here
            if(item !== 'value') {
                let dataColumnName = typeof headerOptionsState !== 'undefined' && headerOptionsState.length > 0 && typeof headerOptionsState[dataColumnIteration] !== 'undefined' ? headerOptionsState[dataColumnIteration].name : '';

                tds.push(<td data-column={dataColumnName} key={keyName}
                             value={valueItem}>
                    <span className="table-data--date" onClick={onRowClick}>{items[item]}</span></td>);
            }

            dataColumnIteration++;
            if(dataColumnIteration > headerOptionsState.length)
                dataColumnIteration = 0;
        }
        trs.push(<tr onClick={e => onRowClick(valueItem)} >{tds}</tr>);
    });

    // Render the pagination, always refreshing according to results
    return (
        // <div style={{minHeight: 400}} className="wrapper-table-data-task">
        <div className="wrapper-table-data-task">
            <Table striped responsive className="content-table-data">
                <thead>
                <tr>
                    {headerOptionsState.map((d) => {
                        return (<th key={d.key}>{d.name}</th>)
                    })}
                </tr>
                </thead>
                <tbody>
                {trs}
                </tbody>
            </Table>
        </div>
    );
};

export default DataTable;