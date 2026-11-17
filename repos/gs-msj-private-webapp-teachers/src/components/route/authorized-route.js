import React from "react";
import { Route, Redirect } from "react-router-dom";

const AuthorizedRoute = ({ authorized, component: Component, ...rest }) => {
  return (
    <Route
      {...rest}
      render={(props) =>
        authorized ? (
          <Component {...props} />
        ) : (
          <Redirect
            to={{
              pathname: "/",
              state: { from: props.location },
            }}
          />
        )
      }
    />
  );
};

export default AuthorizedRoute;
