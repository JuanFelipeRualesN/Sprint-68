const defaultState = null;

function reducer(state = defaultState, action) {
  switch (action.type) {
    case "PREV_PATH":
      return action.payload;
    default:
      return state;
  }
}

export default reducer;
