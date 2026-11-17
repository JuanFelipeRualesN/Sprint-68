import { SET_PARAMETERS } from '../../constants/general';

const defaultState = {};

function reducer(state = defaultState, action) {
    switch(action.type) {
        case SET_PARAMETERS:
            return action.payload;
        default:
            return state;
    }
}

export default reducer;