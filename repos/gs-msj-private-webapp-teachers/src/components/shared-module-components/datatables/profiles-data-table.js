import React, { useState, useEffect } from 'react';
import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";
import { Input } from "reactstrap";
import LoaderFullWindow from "../loader-windows/loader-full-window";
import { Translate } from "react-localize-redux";
import { getSubscriptionProfiles, getProfiles } from "../../../api/users";
import PaginationCustom from "./pagination";
import DataTable from "./datatable";
import "react-dates/initialize";
import "react-dates/lib/css/_datepicker.css";
import {faSearch, faTrash, faUserTimes} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import "./style-datatables.css";
import {convertMaxLengthSting, formatUtcToLocal} from "../../../utils/utils";

const ProfilesDataTable = ({ addRemoveButton, isStaff, roleId, subscriptionId, redoSearch, updateParent, notSelectable, studentNotificationsInfo, isForLearningGapProfiles, removeUser }) => {
  const [currentPage, setCurrentPage] = useState(0);
  const [data, setData] = useState([]);
  const [firstName, setFirstName] = useState(null);
  const [lastName, setLastName] = useState(null);
  const [limiter] = useState(10);
  const [loading, setLoading] = useState(false);
  const [totalPages, setTotalPages] = useState(1);
  const [username, setUsername] = useState(null);
  const [triggerSearch, setTriggerSearch] = useState(false);
  const language = LanguageManager.getLanguage();

  const headerOptions = [
    { key: "0", name: LanguageManager.getTranslation("users-datatable.username", Settings.LANGUAGES) },
    { key: "1", name: LanguageManager.getTranslation("users-datatable.first-name", Settings.LANGUAGES) },
    { key: "2", name: LanguageManager.getTranslation("users-datatable.last-name", Settings.LANGUAGES) },
    { key: "3", name: LanguageManager.getTranslation("users-datatable.invitation-code", Settings.LANGUAGES) },
    { key: "4", name: LanguageManager.getTranslation("users-datatable.last-session", Settings.LANGUAGES) },
    { key: "5", name: LanguageManager.getTranslation("users-datatable.record-of-school", Settings.LANGUAGES) },
    { key: "6", name: "" },
  ];

  const COLUMN_USERNAME = LanguageManager.getTranslation("users-datatable.username", Settings.LANGUAGES)

  useEffect(() => {
    doSearch();
  }, [currentPage, triggerSearch]);

  useEffect(() => {
    doSearch();
  }, [redoSearch]);

  useEffect(() => {
    doSearch()
  }, [LanguageManager.getLanguage()]);

  const doSearch = () => {
    setLoading(true);
    const apiCall = isStaff
        ? getSubscriptionProfiles(subscriptionId, firstName, lastName, username, limiter, currentPage, roleId)
        : getProfiles(firstName, lastName, username, currentPage, limiter, roleId);

    apiCall.then((response) => {
      setLoading(false);
      setTotalPages(response.data.PageTotals);
      if (response.data.UserInfo) {
        convertDataToDataTable(response.data.UserInfo);
      } else {
        setData([]);
      }
    });
  };

  const convertDataToDataTable = (dataToConvert) => {
    const newArray = dataToConvert.map((user) => ({
      Username: user.Username,
      GivenName: user.GivenName,
      Surname: user.Surname,
      Token: user.Token,
      LastSession: formatUtcToLocal(user.LastSession, language),
      UserSchoolRecord: user.UserSchoolRecord,
          Action: (
              <>
                {!notSelectable && (
                    <button
                        value={user.UserId}
                        className="button-secondary-custom"
                        onClick={() => updateParent(user)}
                    >
                      <Translate id="users-datatable.select" />
                    </button>
                )}
                {addRemoveButton && (
                    <button
                        value={user.UserId}
                        className="btn-info btn-danger"
                        style={{ marginLeft: "5px" }}
                        onClick={() => removeUser(user)}
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </button>
                )}
              </>
          ),
      value: user.UserId,
    }));

    setData(newArray);
  };

  const filter = () => {
    setCurrentPage(0);
    setTriggerSearch(prev => !prev); // toggle to force effect run
  };

  return (
      <div>
        <div className="wrapper-col mb-4">
          <div className="four-col">
            <div className="form-group-fields">
              <div className="label"><Translate id="users-datatable.first-name" /></div>
              <Input type="text" value={firstName || ''} onChange={(e) => setFirstName(convertMaxLengthSting(e.target.value, 100))} />
            </div>
          </div>
          <div className="four-col">
            <div className="form-group-fields">
              <div className="label"><Translate id="users-datatable.last-name" /></div>
              <Input type="text" value={lastName || ''} onChange={(e) => setLastName(convertMaxLengthSting(e.target.value, 100))} />
            </div>
          </div>
          <div className="four-col">
            <div className="form-group-fields">
              <div className="label"><Translate id="users-datatable.username" /></div>
              <Input type="text" value={username || ''} onChange={(e) => setUsername(convertMaxLengthSting(e.target.value, 50))} />
            </div>
          </div>
          <div className="four-col mt-3">
            <button className="btn-w-icon" onClick={filter}>
              <FontAwesomeIcon icon={faSearch} /> <Translate id="users-datatable.filter" />
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
              <FontAwesomeIcon icon={faUserTimes} /> <Translate id="users-datatable.no-found" />
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

export default ProfilesDataTable;
