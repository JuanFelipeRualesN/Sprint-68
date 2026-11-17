import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import { BeatLoader } from 'react-spinners';
import { getSubscriptionCourses } from '../../../../../api/subscriptions';

/**
 * Handles the selection of subjects using React Hooks
 */
const LearningGapSelectorSubject = ({ updateParent, subjectCode = null, levelCode = null }) => {
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [selectedValue, setSelectedValue] = useState(null);
    const [subjectTranslation, setSubjectTranslation] = useState("");

    useEffect(() => {
        const fetchSubscriptionCourses = async () => {
            setLoading(true);
            try {
                const response = await getSubscriptionCourses();
                const validLevels = ['c1', 'c2', 'c3', 'c4', 'n1', 'n2', 'n3', 'n4'];
                const arrOptions = response.data
                    .filter(item => validLevels.includes(item.Level.toLowerCase()))
                    .map((item, index) => ({
                        subject: item.Subject,
                        label: `${item.SubjectName} ${item.Level}`,
                        level: item.Level,
                        value: index,
                        subjectCode: item.Subject,
                        levelCode: item.Level
                    }));
                setOptions(arrOptions);
            } catch (error) {
                console.error('Error fetching subscription courses:', error);
            } finally {
                setLoading(false);
            }
        };
        fetchSubscriptionCourses();
    }, []);

    const handleChange = (selectedOption) => {
        setSelectedValue(selectedOption);
        updateParent(selectedOption);
    };

    if (loading && options.length === 0) {
        return <BeatLoader size={10} color={'#6bc4ff'} />;
    }

    const selectedOption = options.find(
        option => option.subject === subjectCode && option.level === levelCode
    );

    return (
        <Select
            classNamePrefix="mantine-like"
            className="mantine-like__control"
            value={selectedOption}
            placeholder={subjectTranslation}
            onChange={handleChange}
            options={options}
        />
    );
};

export default LearningGapSelectorSubject;