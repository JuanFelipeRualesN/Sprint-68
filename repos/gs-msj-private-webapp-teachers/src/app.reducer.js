import { appReducerCases } from './types';

const appReducer = (state, action) => {
    switch (action.type) {
        case appReducerCases.setParameters:
            return {
                ...state,
                variables: action.payload
            };
        case appReducerCases.setHasNotifications:
            return {
                ...state,
                notifications: {
                    ...state.notifications,
                    hasUnreadNotifications: action.payload
                }
            };                      
        default:
            return state;
    }
};

export default appReducer;
