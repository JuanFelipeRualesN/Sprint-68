import React, {useState} from 'react';
// import DiagnosticDetails from "./diagnostic-details";
// import DiagnosticList from "./diagnostic-list";
import {withLocalize} from "react-localize-redux";
import {Row, Col} from "reactstrap";
import DiagnosticList from "./diagnostic-list";
import DiagnosticDetails from "./diagnostic-details";

// Component that display the lesson catalog
const DiagnosticView = () => {
// Simple state to manage the current view (simulating navigation)
    const [view, setView] = useState('list'); // 'list' or 'details'
    const [selectedDiagnosticId, setSelectedDiagnosticId] = useState(null);

    const navigateToEdit = (id) => {
        setSelectedDiagnosticId(id);
        setView('details');
    };

    const navigateToList = () => {
        setSelectedDiagnosticId(null);
        setView('list');
    };

    return (
        <>
            <Row className="animated animate__animated animate__fadeIn">
                <Col>
                    {view === 'list' && (
                       <DiagnosticList navigateToEdit={navigateToEdit} />
                    )}
                    {view === 'details' && selectedDiagnosticId && (
                        <DiagnosticDetails diagnosticId={selectedDiagnosticId} navigateToList={navigateToList} />
                    )}
                </Col>
            </Row>
        </>
    );
};

export default withLocalize(DiagnosticView);