import { createStore, applyMiddleware, compose } from "redux";
import { createBrowserHistory } from 'history';
import {
  syncHistoryWithStore,
  routerReducer,
  routerMiddleware
} from "react-router-redux";
import { createUserManager, loadUser } from "redux-oidc";
import rootReducer from "../reducers/rootreducer";
import userManager from "./userManager";

// create the middleware with the userManager
// const oidcMiddleware = createOidcMiddleware(userManager);
const history = createBrowserHistory(); 


const loggerMiddleware = store => next => action => {
  console.log("Action type:", action.type);
  console.log("Action payload:", action.payload);
  console.log("State before:", store.getState());
  next(action);
  console.log("State after:", store.getState());
};

const initialState = {};

const createStoreWithMiddleware = compose(
  applyMiddleware(loggerMiddleware, routerMiddleware(history))
)(createStore);

const store = createStoreWithMiddleware(rootReducer, initialState);
loadUser(store, userManager);

export default store;