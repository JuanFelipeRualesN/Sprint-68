import {CURRENT_SUBJECT} from '../../constants/subject';

const defaultState = {};

function reducer(state = defaultState, action) {
    switch(action.type) {
        case CURRENT_SUBJECT:
            return action.payload;
        default:
            return state;
    }
}

export default reducer;