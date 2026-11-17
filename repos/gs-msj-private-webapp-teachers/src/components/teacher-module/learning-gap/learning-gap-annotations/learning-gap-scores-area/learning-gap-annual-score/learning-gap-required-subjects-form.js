import React, {useEffect, useState} from "react";
import {Row, Col, Input} from "reactstrap";
import {withLocalize} from "react-localize-redux";
import Settings from "../../../../../../custom-classes/settings";

const LearningGapRequiredSubjectsForm = ({ translate, requiredSubjects, updateParent }) => {

    // Initialize the state for the model
    const [subjectScores, setSubjectScores] = useState(
        requiredSubjects.map((subject) => ({ SubjectCode: subject.SubjectCode, Score: "" }))
    );

    /**
     * Handle input changes for scores.
     */
    const handleScoreChange = (subjectCode, value) => {
        // Allow only numeric values, including decimals
        const numericRegex = Settings.getRegexScore();

        if (numericRegex.test(value)) {
            // Parse the value as a number to check if it's less than or equal to 100
            const numericValue = parseFloat(value);
            if (numericValue <= 100 || value === "") {
                // Update the state locally first
                setSubjectScores((prev) =>
                    prev.map((subject) =>
                        subject.SubjectCode === subjectCode ? { ...subject, Score: value } : subject
                    )
                );

                // After successful update, trigger parent update with the new scores
                updateParent(subjectScores); // Get the updated scores from state
            } else {
                console.log("Value exceeds 100");
            }
        } else {
            console.log("Invalid value format");
        }
    };

    // Use useEffect to schedule the parent update after the state has settled
    useEffect(() => {
        updateParent(subjectScores);
    }, [subjectScores]);

    return (
        <div>
            {requiredSubjects.map((subject, index) => (
                <Row key={subject.SubjectCode} className="mb-3">
                    <Col xs={4}>
                        <span>{subject.SubjectName}</span>
                    </Col>
                    <Col xs={8}>
                        <Input
                            type="text"
                            value={
                                subjectScores.find((s) => s.SubjectCode === subject.SubjectCode)
                                    ?.Score || ""
                            }
                            onChange={(e) =>
                                handleScoreChange(subject.SubjectCode, e.target.value)
                            }
                            placeholder={translate("learning-gap.enter-score-max-100")}
                        />
                    </Col>
                </Row>
            ))}
        </div>
    );
};

export default withLocalize(LearningGapRequiredSubjectsForm);
