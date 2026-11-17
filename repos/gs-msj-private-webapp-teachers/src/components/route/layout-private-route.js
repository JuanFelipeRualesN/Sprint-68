import React from "react";
import { Redirect, Route } from "react-router-dom";
import SessionManager from "../../custom-classes/session-manager";

const LayoutPrivateRoute = ({ component: Component, ...rest }) => {
  let isLoggedIn = SessionManager.evaluateSession();

  const sessionDetails = SessionManager.getSessionDetails();
  if (sessionDetails === null) {
    isLoggedIn = false;
  }

  return (
    <Route
      {...rest}
      render={(props) =>
        isLoggedIn ? (
          <Component {...props} />
        ) : (
          <Redirect
            to={{ pathname: "/", state: { from: props.location } }}
          />
        )
      }
    />
  );
};

export default LayoutPrivateRoute;
