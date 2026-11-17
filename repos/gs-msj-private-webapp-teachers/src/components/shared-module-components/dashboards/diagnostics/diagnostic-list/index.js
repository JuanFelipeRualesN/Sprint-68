import React, { useState, useEffect } from 'react';
import { useDiagnostics } from '../helpers';
import {
    Button,
    ListGroup,
    ListGroupItem
} from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faBook,
    faPlus,
    faPen,
    faTrash,
    faRocket
} from '@fortawesome/free-solid-svg-icons';
import { Translate, withLocalize } from 'react-localize-redux';
import useVersionedTranslations from "../../../../../hooks/useVersionedTranslations";

// API to load custom reading diagnostics from backend
import { getCustomReadingDiagnosticsByTeacher } from '../../../../../api/reading-api';

function DiagnosticList({ navigateToEdit }) {
    const { diagnostics, deleteDiagnostic, publishDiagnostic } = useDiagnostics();
    const [customDiagnostics, setCustomDiagnostics] = useState([]);

    useVersionedTranslations();

    // Load custom reading diagnostics created by the logged-in teacher
    useEffect(() => {
        const loadCustomDiagnostics = async () => {
            try {
                const response = await getCustomReadingDiagnosticsByTeacher();
                const data = response.data || [];

                // Map backend DTO to the UI diagnostic shape expected by the list
                const mapped = data.map(d => ({
                    Id: d.Id,
                    Title: d.Title,
                    LevelCode: d.LevelCode,
                    SubjectCode: d.SubjectCode === "en" ? "English": "Español",
                    Published:  true, // temporary d.Published,
                    IsCustom: true
                }));

                setCustomDiagnostics(mapped);

            } catch (error) {
                console.error("Error loading custom diagnostics:", error);
            }
        };

        loadCustomDiagnostics();
    }, []);

    // Combine original diagnostics (from local system) and custom diagnostics (from backend)
    const allDiagnostics = [...diagnostics, ...customDiagnostics];

    return (
        <div>
            <h2 className="mb-3">
                <FontAwesomeIcon icon={faBook} className="me-2 text-primary" />{' '}
                <Translate id="dashboards.my_diagnostics">My Diagnostics</Translate>
            </h2>

            <Button color="primary" onClick={() => navigateToEdit('new')} className="mb-3">
                <FontAwesomeIcon icon={faPlus} className="me-2" />{' '}
                <Translate id="dashboards.add_new_diagnostic">Add New Diagnostic</Translate>
            </Button>

            <ListGroup>
                {allDiagnostics.map((diag) => (
                    <ListGroupItem
                        key={`${diag.isCustom ? 'custom-' : ''}${diag.Id}`}
                        className="d-flex justify-content-between align-items-center"
                    >
                        <div>
                            <strong>{diag.Title}</strong>{' '}
                            <Translate id="dashboards.grade">Grade</Translate> {diag.LevelCode} |
                            {' '}{diag.SubjectCode}

                            {diag.Published && (
                                <span className="badge bg-success ms-2">
                                    <Translate id="dashboards.published">Published</Translate>
                                </span>
                            )}
                        </div>

                        <div>
                            <Button
                                color="info"
                                size="sm"
                                onClick={() => navigateToEdit(diag.Id)}
                                disabled={diag.Published}
                                className="me-2"
                            >
                                <FontAwesomeIcon icon={faPen} className="me-2" />{' '}
                                <Translate id="dashboards.edit">Edit</Translate>
                            </Button>

                            <Button
                                color="danger"
                                size="sm"
                                onClick={() => deleteDiagnostic(diag.id)}
                                disabled={diag.Published}
                                className="me-2"
                            >
                                <FontAwesomeIcon icon={faTrash} className="me-2" />{' '}
                                <Translate id="dashboards.delete">Delete</Translate>
                            </Button>

                            {!diag.Published && (
                                <Button
                                    color="success"
                                    size="sm"
                                    onClick={() => publishDiagnostic(diag.Id)}
                                >
                                    <FontAwesomeIcon icon={faRocket} className="me-2" />{' '}
                                    <Translate id="dashboards.publish">Publish</Translate>
                                </Button>
                            )}
                        </div>
                    </ListGroupItem>
                ))}
            </ListGroup>
        </div>
    );
}

export default withLocalize(DiagnosticList);
