import React from 'react';
import {Col, Row, Input} from 'reactstrap';
import {Translate} from "react-localize-redux";
import 'bootstrap/js/dist/modal';
import 'bootstrap/js/dist/dropdown';
import 'bootstrap/js/dist/tooltip';
import 'bootstrap/dist/css/bootstrap.css';
import {Textarea} from "@mantine/core";

const PlanDescriptionEditorSN = ({ description, disabled, languageCode, languageName, title, updateParent }) => {
    // Handles the description changes
    const handleChange = (e) => {
        const state = {
            description: e.currentTarget.value,
            disabled,
            languageCode,
            languageName,
            title
        };
        updateParent(state);
    };

    // Handles title change
    const handleTitleChange = (e) => {
        const state = {
            description,
            disabled,
            languageCode,
            languageName,
            title: e.currentTarget.value
        };
        updateParent(state);
    };

    return (
        <Row>
            <Col sm={{size: 12}}>
                <div>
                    <b><Translate id="plan-description-editor.description-for-language"/>{languageName}</b>
                    <p><Translate id="plan-description-editor.title"/></p>
                    <Input disabled={disabled} type="text" onChange={handleTitleChange}
                            value={title}   className="mantine-like__control"
                           classNamePrefix="mantine-like"/>
                    <p><Translate id="plan-description-editor.instruction"/></p>
                    <Textarea
                        className="mantine-like__control"
                        classNamePrefix="mantine-like"
                        type="text"
                        disabled={disabled}
                        value={description}
                        onChange={handleChange}
                        rows="5"
                    />
                </div>
            </Col>
        </Row>
    );
};

export default PlanDescriptionEditorSN;