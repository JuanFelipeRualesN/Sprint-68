import axios from "../../../../../api/interceptors";
import { useState } from 'react';
import Settings from '../../../../../custom-classes/settings';

const useClosePlanning = () => {
    const [succeeded, setSucceeded] = useState(false);

    const closePlanning = async (id) => {
        try {
            const endpoint = Settings.getModifyPlanAPI();
            const response = await axios.post(endpoint, {
                Id: id,
                Opened: '0'
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
        closePlanning,
        succeeded
    };
};

export default useClosePlanning;
