const defaultState = null;

function reducer(state = defaultState, action) {
  switch (action.type) {
    case "GRADE_SELECTED":
      return action.payload;
    default:
      return state;
  }
}

export default reducer;
