const defaultState = null;

function reducer(state = defaultState, action) {
  switch (action.type) {
    case "SUBSCRIPTION_TYPE":
      return action.payload;
    default:
      return state;
  }
}

export default reducer;
