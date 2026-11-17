import React, { useState, useEffect } from 'react';
import {Button, Col} from 'reactstrap';
import { Translate } from "react-localize-redux";
import {getProfilesActivity} from '../../../api/gsweb';
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome';
import {faSyncAlt, faTasks} from '@fortawesome/free-solid-svg-icons';
import {formatUtcToLocal} from "../../../utils/utils";
import LanguageManager from "../../../custom-classes/language-manager";
/**
 * Class that handles the modal
 */
const TableActivity = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [rows, setRows] = useState([]);

    // Prepare it for the first view with the data included
    useEffect(() => {
        callProfilesActivities();
    }, []);

    useEffect(() => {
        callProfilesActivities();
    }, [LanguageManager.getLanguage()]);

    // Calls the profile last activities (we cut it to 3)
    const callProfilesActivities = () => {
        setIsLoading(true);
        
        let apiCall = getProfilesActivity();
        apiCall.then((response) => {
            if(typeof response.data !== 'undefined' && response.data !== null) {
                let newRows = [];
                for (let i = 0; i < response.data.length; i++) {
                    if (i === 3)
                        break;
                    // If active, add the button;
                    let row =
                        <div key={i} className="border-bottom">
                            <b>{response.data[i].StudentName}</b>
                            <br/>
                            <small>{
                                formatUtcToLocal(
                                    response.data[i].CreatedDate,
                                    LanguageManager.getLanguage()
                                )
                            }</small>
                            <br/>
                            <Translate id="table-activity.made"/>
                            {
                                response.data[i].IsExam ?
                                    <Translate id="table-activity.exam"/> :
                                    <Translate id="table-activity.practice"/>
                            }
                            {' '}
                            <Translate id="table-activity.score"/> {' '}
                            {response.data[i].TotalPercent}{' '}
                            <Translate id="table-activity.course"/> {' '}
                            {response.data[i].Subject}{' '}
                            <Translate id="table-activity.grade"/> {' '}
                            {response.data[i].Level} {' '}
                        </div>;
                    newRows.push(row);
                }
                setRows(newRows);
                setIsLoading(false);
            }
        });
    };

    if(isLoading){
        return (<div className="spinner-container">
            <span className="spinner-button-alt"/>
        </div>);
    }

    if (rows.length === 0) {
        return (
            <div>
                <Translate id="table-activity.no-activity" options={{renderInnerHtml: false}}/>
                {' '}
                <div className="text-right">
                    <button className="btn-default button-primary-custom" onClick={callProfilesActivities}>
                        <FontAwesomeIcon icon={faSyncAlt}/>{' '}
                        <Translate id="table-activity.refresh" options={{renderInnerHtml: false}}/>
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div>
            <div className="wrapper-col wrapper-col__align-top block-display-responsive">
                <div className="col col-no-padding-left">
                    <div className="title-principal mb-4">
                        <Translate id="table-activity.last-activity" options={{renderInnerHtml: false}}/>
                    </div>
                </div>
                <div className="col txt-right">
                    <button className="btn-default button-primary-custom" onClick={callProfilesActivities}>
                        <FontAwesomeIcon icon={faSyncAlt}/>{' '}
                        <Translate id="table-activity.refresh" options={{renderInnerHtml: false}}/>
                    </button>
                </div>
            </div>
            <div className="dashboard-activity-table">
                {rows !== "0" ? rows : null}
            </div>

            {/* <div className="text-right">
                <button className="btn-default button-primary-custom" onClick={this.callProfilesActivities}>
                    <FontAwesomeIcon icon={faSyncAlt}/>{' '}
                    <Translate id="table-activity.refresh" options={{renderInnerHtml: false}}/>
                </button>
            </div> */}
            
            {isLoading ? <LoaderFullWindow/> : null}
        </div>
    );
};

export default TableActivity;