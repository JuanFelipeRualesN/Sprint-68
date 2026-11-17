import React from "react";
import { Route, Redirect } from "react-router-dom";
import { useSelector } from "react-redux";

const KinderRoute = ({ authorized, component: Component, ...rest }) => {
  const { grade } = useSelector((state) => ({
    grade: state.grade,
  }));
  return (
    <Route
      {...rest}
      render={(props) =>
        grade === "k" && authorized ? (
          <Component {...props} />
        ) : (
          <Redirect
            to={{
              pathname: "/students",
              state: { from: props.location },
            }}
          />
        )
      }
    />
  );
};

export default KinderRoute;