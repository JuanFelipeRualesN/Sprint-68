/**
 * Created by portiz on 2/12/19.
 */
import React, { useState, useEffect } from 'react';
import { Table } from 'reactstrap';
import { faExclamationCircle } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import "./style.css";

/**
 * Datatable class. We had problem with the pagination of the other libraries as
 * reactbootstraptable, so we decided to create own own
 * Example of data to send
 *  let headerOptions = [{name:'Id'}, {name:'First Name'}, {name:'amount'}];
 let data = [
 {
     id: '2', name: 'My Plan', value: '2', amount: '599'
 },
 {
     id: '3', name: 'My Plan three', value: '3', amount: '599'
 }
 ];
 */
const DataTable = (props) => {
    const [textAlign] = useState(props.textAlign ? props.textAlign : null);
    const [data, setData] = useState(props.data ? props.data : []);
    const [headerOptions, setHeaderOptions] = useState(props.headerOptions ? props.headerOptions : []);
    const [totalToShowPerPage, setTotalToShowPerPage] = useState(props.totalToShowPerPage ? props.totalToShowPerPage : 0);
    const [rowHeight] = useState( props.rowHeight ?? null);

    useEffect(() => {
        setData(props.data ? props.data : []);
        setHeaderOptions(props.headerOptions ? props.headerOptions : []);
        setTotalToShowPerPage(props.totalToShowPerPage ? props.totalToShowPerPage : 0);
    }, [props.data, props.headerOptions, props.totalToShowPerPage]);

    const onRowClick = (e) => {
        for(let i=0; i < data.length; i++)
        {
            if(data[i].value === e.currentTarget.getAttribute('value'))
                if(typeof props.updateParent !== 'undefined' && props.updateParent !== null)
                    return props.updateParent(data[i]);
        }
    };

    // Renders the pagination, always refreshing according to results
    let uniqueKey = 0;
    // Create header
    let trs = [];
    data.forEach((items)=>{
        let valueItem = items.value !== null && typeof items.value !== 'undefined' ? items.value : null;
        let tds = [];

        let dataColumnIteration = 0;
        for(let item in items)
        {
            uniqueKey++;
            let keyName = parseInt(uniqueKey);
            // Push it if it is not a value. We do not need a value here
            if(item !== 'value') {
                let dataColumnName = typeof headerOptions !== 'undefined' && headerOptions.length > 0 && typeof headerOptions[dataColumnIteration] !== 'undefined' ? headerOptions[dataColumnIteration].name : '';

                if(typeof items[item] === 'object' && items[item] !== null){
                    if(textAlign !== null)
                    {
                        tds.push(<td style={{textAlign: textAlign}} data-column={dataColumnName} key={keyName} value={valueItem}><span className="table-data--date" onClick={onRowClick}>{items[item]}</span></td>);
                    }
                    else
                    {
                        tds.push(<td data-column={dataColumnName} key={keyName} value={valueItem}><span className="table-data--date" onClick={onRowClick}>{items[item]}</span></td>);
                    }
                }
                else {
                    if (textAlign !== null) {
                        tds.push(<td style={{textAlign: textAlign === "left" ? "left" : "center"}}
                                     data-column={dataColumnName} key={keyName} value={valueItem}><span
                            className="table-data--date" onClick={onRowClick}
                            dangerouslySetInnerHTML={{__html: items[item]}}/></td>);
                    } else {
                        tds.push(<td data-column={dataColumnName} key={keyName} value={valueItem}>
                            <span className="table-data--date" onClick={onRowClick}dangerouslySetInnerHTML={{__html: items[item]}}/>
                            {props.isForLearningGapProfiles && props.studentNotificationsInfo?.length > 0 && props.studentNotificationsInfo
                            .filter(item => dataColumnName === props.columnName && item.StudentId === valueItem && item.HasNotifications)
                            .map(item => (
                                <FontAwesomeIcon
                                key={item.StudentId}
                                icon={faExclamationCircle}
                                style={{
                                    color: '#d41f13',
                                    backgroundColor: 'white',
                                    borderRadius: '50%',
                                    fontSize: '16px',
                                    transform: 'translate(30%, 0%)',
                                }}
                                />
                            ))}
                            </td>
                        );
                    }
                }
            }
            dataColumnIteration++;
            if(dataColumnIteration > headerOptions.length)
                dataColumnIteration = 0;
        }
        if(rowHeight !== null){
            trs.push(<tr key={uniqueKey} style={{minHeight: rowHeight}} onClick={onRowClick}>{tds}</tr>);
        }
        else {
            trs.push(<tr  key={uniqueKey} onClick={onRowClick}>{tds}</tr>);
        }
    });

    return (
        // <div style={{minHeight: 400}} className="wrapper-table-data-task">
        <div className="wrapper-table-data-task">
            <Table striped responsive className="content-table-data">
                <thead>
                <tr>
                    {headerOptions.map((d, index) => (
                        <th key={`${d.key}-${index}`}>{d.name}</th>
                    ))}
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
