import React, { Component } from 'react';
import './App.css';
import { Link, Switch, Route } from 'react-router-dom';
import Register from './components/Register';
import Login from './components/Login';
import Home from './components/Home';
import logo from './logo.svg';
import withHttpHandler from './hoc/httpHandler';
import Axios from 'axios';
import CallbackPage from './components/CallbackPage';


class App extends Component {

  render(){

  return (
    <div className="App">
    <header className="App-header">
      <img src={logo} className="App-logo" alt="logo" />
      <h1 className="App-title">Welcome to React</h1>
    </header>
    <div className="menu">
        <ul>
          <li> <Link to="/">Home</Link> </li>
          <li> <Link to="/register">Register</Link> </li>
          <li> <Link to="/login">Login</Link> </li>
          
        </ul>
    </div>
    <div className="App-intro">
      <Switch>
        <Route exact path="/"  component={Home} />
        <Route path="/register" component={Register} />
        <Route path="/login" component={Login} />
        <Route path="/callback" component={CallbackPage} />
      </Switch>
    </div>
  </div>
  );
}

}

export default withHttpHandler(App, Axios);
  