import React, {useEffect, useState} from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Button, Row, Col, Input, Modal, ModalHeader, ModalBody} from 'reactstrap';
import LoaderFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import { Translate } from "react-localize-redux";
import {createAnnotation, getTeacherStudentAnnotations} from "../../../api/teachers";
import DataTable from '../../shared-module-components/datatables/datatable';
import {
    faPlus,
} from '@fortawesome/free-solid-svg-icons';
import 'react-dates/initialize';
import 'react-dates/lib/css/_datepicker.css';
import PaginationCustom from "../../shared-module-components/datatables/pagination";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import LanguageManager from "../../../custom-classes/language-manager";
import Settings from "../../../custom-classes/settings";
import {formatUtcToLocal, getTimeZone} from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";
import {BeatLoader} from "react-spinners";

/**
 * Data table to handle the annotations
 */
const AnnotationsDataTable = ({ planningId, studentId, redoSearch }) => {
    const [currentPage, setCurrentPage] = useState(1);
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showModalAddAnnotation, setShowModalAddAnnotation] = useState(false);
    const [totalPages, setTotalPages] = useState(1);
    const [language] = useState(LanguageManager.getLanguage());
    const [annotation, setAnnotation] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const headerOptions = [
        { key: "0", name: LanguageManager.getTranslation("progress.date-created", Settings.LANGUAGES) },
        { key: "1", name: LanguageManager.getTranslation("progress.annotation", Settings.LANGUAGES) },
    ];
    const limiter = 10;

    useEffect(() => {
        doSearch();
    }, []);

    useEffect(() => {
        doSearch();
    }, [redoSearch]);

    const handlePageClick = (e) => {
        setCurrentPage(e);
        setTimeout(() => doSearch(), 0); // Ensure state has updated
    };

    const toggleModalAnnotation = () => {
        setShowModalAddAnnotation(false);
        doSearch();
    };

    const handleShowModalAddAnnotation = () => {
        setShowModalAddAnnotation(true);
    };

    const convertDataToDataTable = (dataToConvert) => {
        if (!dataToConvert || dataToConvert.length === 0) {
            setData([]);
            return;
        }

        const newArray = dataToConvert.map((item) => ({
            DateCreated: formatUtcToLocal(item.CreatedAt, language),
            Annotation: item.Annotation,
            value: item.Id,
        }));

        setData(newArray);
    };

    const doSearch = () => {
        setLoading(true);
        getTeacherStudentAnnotations(planningId, studentId, currentPage, limiter)
            .then((response) => {
                setTotalPages(response.data.TotalPages);
                convertDataToDataTable(response.data.TeacherStudentAnnotationModelList);
                setLoading(false);
            });
    };

    const handleSaveAnnotation = () => {
        setLoading(true);
        setShowErrorMessage(false);
        createAnnotation(planningId, studentId, annotation).then((response) => {
            setLoading(false);
            if (response.data.Messages && response.data.Messages.length > 0) {
                const message = response.data.Messages.join('<br/>');
                setErrorMessage(message);
                setShowErrorMessage(true);
            } else {
                setAnnotation('');
                toggleModalAnnotation();
            }
        });
    };

    return (
        <div>
            <Row>
                <Col className="text-right">
                    <button type="button" className="btn-default button-primary-custom" onClick={handleShowModalAddAnnotation}>
                        <FontAwesomeIcon icon={faPlus} />{' '}
                        <Translate id="progress.add-annotation" options={{ renderInnerHtml: true }} />
                    </button>
                </Col>
            </Row>

            {data == null || data.length === 0 ? (
                <Translate id="progress.no-annotation" options={{ renderInnerHtml: true }} />
            ) : (
                <>
                    <DataTable headerOptions={headerOptions} data={data} />
                    <PaginationCustom
                        totalPages={totalPages}
                        currentPage={currentPage}
                        updateParent={handlePageClick}
                        dapper={false}
                    />
                </>
            )}

            {loading && <LoaderFullWindow />}

            {showModalAddAnnotation && (
                <Modal isOpen={showModalAddAnnotation} toggle={toggleModalAnnotation}>
                    <ModalHeader toggle={toggleModalAnnotation}>
                        <Translate id="progress.annotations" />
                    </ModalHeader>
                    <ModalBody>
                        {showErrorMessage ? (
                            <div className="content" dangerouslySetInnerHTML={{ __html: errorMessage }} />
                        ) : (
                            <div>
                                <Translate id="progress.annotation" />
                                <hr />
                                <Input
                                    type="textarea"
                                    value={annotation}
                                    onChange={(e) => setAnnotation(e.currentTarget.value)}
                                    rows="10"
                                    name="bodyText"
                                    id="bodyText"
                                />
                                <hr />
                                <p className="text-center">
                                    <Button
                                        disabled={
                                            annotation.length === 0 ||
                                            !studentId ||
                                            !planningId ||
                                            loading
                                        }
                                        color="success"
                                        onClick={handleSaveAnnotation}
                                    >
                                        {loading ? (
                                            <BeatLoader size={10} color={"#fff"} />
                                        ) : null}{' '}
                                        <Translate id="progress.save" />
                                    </Button>{' '}
                                    <button type="button" className="btn-default button-secondary-custom" onClick={toggleModalAnnotation} disabled={loading}>
                                        <Translate id="progress.cancel" />
                                    </button>
                                </p>
                            </div>
                        )}
                    </ModalBody>
                </Modal>
            )}
        </div>
    );
};

export default AnnotationsDataTable;