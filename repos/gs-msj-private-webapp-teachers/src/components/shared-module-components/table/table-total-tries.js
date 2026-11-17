import React, { useState, useEffect } from 'react';
import { Button, Table, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { getUserQuizDetails } from "../../../api/gsweb";
import ResultsToHTMLConverter from "../../../custom-classes/results-html-converter";
import LoaderFullWindow from "../loader-windows/loader-full-window";
import { faPrint, faEye } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Settings from "../../../custom-classes/settings";
import { formatUtcToLocal } from "../../../utils/utils";
import LanguageManager from "../../../custom-classes/language-manager";

const TableTotalTries = ({ data, profileId, showTries }) => {
    const [loading, setLoading] = useState(false);
    const [examInfo, setExamInfo] = useState(null);
    const [languages] = useState(Settings.LANGUAGES);
    const [modal, setModal] = useState(false);
    const [pdfLocation, setPdfLocation] = useState(null);
    const [rows, setRows] = useState([]);
    const [triesData, setTriesData] = useState(data);
    const [language] = useState(LanguageManager.getLanguage());

    useEffect(() => {
        setTriesData(data);
    }, [data]);

    useEffect(() => {
        mountData();
        // re-render rows when showTries changes too
    }, [triesData, showTries]);

    const toggle = () => setModal(!modal);

    const callResults = (tryRowId) => {
        setLoading(true);
        setExamInfo('');

        getUserQuizDetails(tryRowId, profileId).then((response) => {
            const resultsHtml = ResultsToHTMLConverter.getHtmlResults(
                response.data.UserQuizFullDetails,
                response.data.StudentName,
                languages,
                true,
                showTries
            );
            setExamInfo(resultsHtml);
            setPdfLocation(response.data.PDFLocation);
            setLoading(false);
            setModal(true);
        });
    };

    const mountData = () => {
        const renderedRows = triesData.map((item) => (
            <tr key={item.Id}>
                {showTries && <td>{item.TryNo}</td>}
                <td>{item.TotalPercent}</td>
                <td>{formatUtcToLocal(item.CreatedAt, language)}</td>
                <td>
                    <Button className="btn-default button-secondary-custom" onClick={() => callResults(item.Id)}>
                        <FontAwesomeIcon icon={faEye} />{' '}
                        <Translate id="report-student-average.see-details" options={{ renderInnerHtml: true }} />
                    </Button>
                </td>
            </tr>
        ));
        setRows(renderedRows);
    };

    return (
        <div>
            <div className="progress-table-inside">
                <Table responsive>
                    <thead>
                    <tr>
                        {showTries && (
                            <th>
                                <Translate id="report-student-average.try" options={{ renderInnerHtml: false }} />
                            </th>
                        )}
                        <th>
                            <Translate id="report-student-average.total" options={{ renderInnerHtml: false }} />
                        </th>
                        <th>
                            <Translate id="report-student-average.taken-date" options={{ renderInnerHtml: false }} />
                        </th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>{rows.length > 0 ? rows : null}</tbody>
                </Table>
            </div>

            {loading && <LoaderFullWindow />}

            {examInfo !== null && (
                <Modal isOpen={modal} toggle={toggle} className="modal-results modal-progress-results">
                    <ModalHeader className="card-white" toggle={toggle}>
                        <Translate id="report-student-average.results" options={{ renderInnerHtml: false }} />{' '}
                        {pdfLocation && (
                            <a href={pdfLocation} target="_blank" rel="noreferrer" download={pdfLocation} className="btn btn-primary">
                                <FontAwesomeIcon icon={faPrint} />{' '}
                                <Translate id="report-student-average.print" options={{ renderInnerHtml: false }} />
                            </a>
                        )}
                    </ModalHeader>
                    <ModalBody className="card-white">
                        {examInfo && <div dangerouslySetInnerHTML={{ __html: examInfo }} />}
                    </ModalBody>
                    <ModalFooter className="card-white"></ModalFooter>
                </Modal>
            )}
        </div>
    );
};

export default TableTotalTries;
