import { LOGIN_SUCCESS } from '../../constants/general';

const defaultState = {};

function reducer(state = defaultState, action) {
    switch(action.type) {
        case LOGIN_SUCCESS:
            return action.payload;
        default:
            return state;
    }
}

export default reducer;