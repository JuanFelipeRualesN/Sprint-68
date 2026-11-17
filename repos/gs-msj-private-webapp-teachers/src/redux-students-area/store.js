import { createStore, combineReducers, compose, applyMiddleware } from "redux";
import { DEFAULT_STATE } from "../constants/general";
import user from "./reducers/user";
import parameters from "./reducers/params";
import error from "./reducers/error";
import loading from "./reducers/loading";
import subject from "./reducers/subject";
import lessons from "./reducers/lessons";
import subjects from "./reducers/subjects";
import grade from "./reducers/grade";
import prevPath from "./reducers/path";
import subscriptionType from "./reducers/subscription-type";
import { reducer as toastrReducer } from "react-redux-toastr";
import thunk from "redux-thunk";

const appReducer = combineReducers({
  parameters,
  loading,
  user,
  error,
  subject,
  lessons,
  subjects,
  grade,
  subscriptionType,
  prevPath,
  toastr: toastrReducer,
});

const rootReducer = (state, action) => {
  if (action.type === DEFAULT_STATE) {
    state = undefined;
  }

  return appReducer(state, action);
};

const globalState = localStorage.getItem("GLOBAL_STATE");
const initialState = globalState ? JSON.parse(globalState) : undefined;

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const store = createStore(
  rootReducer,
  initialState,
  composeEnhancers(applyMiddleware(thunk))
);

export const saveState = () => {
  const state = store.getState();
  const stateClone = {
    ...state,
    error: null,
    toastr: undefined,
    loading: false,
  };
  localStorage.setItem("GLOBAL_STATE", JSON.stringify(stateClone));
};

export default store;
