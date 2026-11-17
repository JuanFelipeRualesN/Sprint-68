import { createStore, compose, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import reducers from './reducers';

//UNCOMMENT THIS LINE ONLY FOR DEVELOPMENT PURPOSES
// const  composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose
const composeEnhancers = compose;
export default createStore(reducers, composeEnhancers(applyMiddleware(thunk)))