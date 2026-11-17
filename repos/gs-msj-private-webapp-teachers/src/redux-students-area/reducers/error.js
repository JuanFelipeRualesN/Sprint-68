import {ERROR, CLEAR_ERROR} from '../../constants/general';

const defaultState = null;

function reducer(state = defaultState, action) {
    switch (action.type) {
        case ERROR:
            return action.payload;
        case CLEAR_ERROR:
            return null;
        default:
            return state;
    }
}

export default reducer;