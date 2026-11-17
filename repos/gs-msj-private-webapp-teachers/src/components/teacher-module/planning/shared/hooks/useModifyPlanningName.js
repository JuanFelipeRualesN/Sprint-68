import { useState } from 'react';
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useModifyPlanningName = () => {
    const [succeeded, setSucceeded] = useState(false);

    const updatePlanningName = async (id, name) => {
        try {
            const endpoint = Settings.getTeacherPlanningDetailsAPI(id);
            const response = await axios.patch(`${endpoint}/name`, {
                Name: name
            });
            const result = response.status === 200;

            setSucceeded(result);
            return Promise.resolve(result);
        } catch (error) {
            setSucceeded(false);
            return Promise.resolve(false);
        }
    };

    return {
        updatePlanningName,
        succeeded
    };
};

export default useModifyPlanningName;
