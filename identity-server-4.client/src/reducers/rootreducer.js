import { combineReducers } from "redux";
import { reducer as oidcReducer } from 'redux-oidc';
import { routerReducer } from "react-router-redux";

const todos = (state = [], action) => {
  switch (action.type) {
    case 'ADD_TODO':
      return [
        ...state,
        {
          id: action.id,
          text: action.text,
          completed: false
        }
      ]
    case 'TOGGLE_TODO':
      return state.map(todo =>
        (todo.id === action.id)
          ? {...todo, completed: !todo.completed}
          : todo
      )
    default:
      return state
  }
}


const rootReducer = combineReducers({
  routing: routerReducer,
  todos,
  oidc: oidcReducer,
});

export default rootReducer;