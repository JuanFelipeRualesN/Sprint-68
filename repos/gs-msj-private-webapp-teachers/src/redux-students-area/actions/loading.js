import { LOADING } from '../../constants/general';

const loading = (loading) => {
    return {
        type: LOADING,
        payload: loading
    };
};

export default loading;