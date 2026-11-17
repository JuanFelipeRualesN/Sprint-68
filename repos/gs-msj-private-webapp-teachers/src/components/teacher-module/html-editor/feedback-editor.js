import React, { useState, useEffect } from 'react';
import {Col, Row} from 'reactstrap';
import 'bootstrap/js/dist/modal';
import 'bootstrap/js/dist/dropdown';
import 'bootstrap/js/dist/tooltip';
import 'bootstrap/dist/css/bootstrap.css';

const FeedbackEditor = ({description, disabled, updateParent}) => {
    const [localDescription, setLocalDescription] = useState(description);
    const [localDisabled, setLocalDisabled] = useState(disabled);

    useEffect(() => {
        setLocalDescription(description);
        setLocalDisabled(disabled);
    }, [description, disabled]);

    const handleChange = (e) => {
        const value = e.target.value;
        setLocalDescription(value);
        updateParent(value);
    };

    return (
        <Row>
            <Col sm={{ size: 12 }}>
                <div>
                    <input
                        type="text"
                        disabled={localDisabled}
                        value={localDescription}
                        onChange={handleChange}
                    />
                </div>
            </Col>
        </Row>
    );
}

export default FeedbackEditor;