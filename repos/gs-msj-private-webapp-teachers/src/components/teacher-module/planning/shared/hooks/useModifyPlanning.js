import { useState } from 'react';
import axios from "../../../../../api/interceptors";
import Settings from '../../../../../custom-classes/settings';

const useModifyPlanning = () => {
    const [succeeded, setSucceeded] = useState(false);
    const [message, setMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const modifyPlanning = async (planning) => {
        try {
            setIsLoading(true);
            setSucceeded(false);
            const endpoint = Settings.getModifyPlanAPI();
            const response = await axios.post(endpoint, planning);
            const result = response.status === 200;
            setIsLoading(false);
            // Update state after successful response
            setMessage(response.data.Messages[0]);
            setSucceeded(result);

            return { succeeded: result, planId: response.data.Id };
        } catch (error) {
            setSucceeded(false);
            setIsLoading(false);
            return false;
        }
    };

    return {
        modifyPlanning,
        succeeded,
        message,
        isLoading
    };
};

export default useModifyPlanning;
