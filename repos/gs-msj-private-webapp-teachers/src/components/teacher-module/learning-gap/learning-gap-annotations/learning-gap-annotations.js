import React from 'react';
import 'react-bootstrap-table-next/dist/react-bootstrap-table2.min.css';
import {Card, CardBody} from 'reactstrap';
import { withLocalize } from 'react-localize-redux';
import LearningGapAnnotationArea from "./learning-gap-annotation-area/learning-gap-annotation-area";
import LearningGapScoresArea from "./learning-gap-scores-area/learning-gap-scores-area";

/**
 * To display the learning gap progress of the student.
 * @param translate The translate function.
 * @param studentId The student ID.
 * @param redoSearch The redo search flag.
 * @returns {Element}
 * @constructor
 */
const LearningGapAnnotations = ({ translate, studentId }) => {


    return (
        <div className="learning-gap-fade-in">
            <Card>
                <CardBody style={{padding: 10}}>
                    <LearningGapScoresArea studentId={studentId} />
                </CardBody>
            </Card>
            <Card style={{marginTop: 10}}>
                <CardBody style={{padding: 10}}>
                    <LearningGapAnnotationArea studentId={studentId} />
                </CardBody>
            </Card>
        </div>
    );
};

export default withLocalize(LearningGapAnnotations);
