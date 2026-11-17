import React, {useContext, useEffect, useState} from "react";
import { Col, Button, Form, FormGroup } from "reactstrap";
import { useMsal, useIsAuthenticated } from "@azure/msal-react";
import { Redirect, useHistory } from "react-router-dom";
import { getLoginSso } from "../../../api/users";
import LoaderFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import SessionManager from "../../../custom-classes/session-manager";
import AzureIcon from "../../../assets/images/common-icons/Login-icon-Microsoft.svg";
import { Translate } from "react-localize-redux";
import { isRoleAllowed } from "../../../utils/utils";
import Settings from "../../../custom-classes/settings";
import {AppContext} from "../../../App.Context";

const Sso = () => {
  const { instance } = useMsal();
  const isAuthenticated = useIsAuthenticated();
  const history = useHistory();
  const [loading, setLoading] = useState(false);
  const { refreshNotifications } = useContext(AppContext);

  useEffect(() => {
    // if (isAuthenticated) {
    //   history.push("/dashboard");
    // }
  }, []);

  // useEffect to handle the redirect response
  useEffect(() => {
    instance
        .handleRedirectPromise()
        .then(async (response) => {
          if (response) {
            // This is where you'll land after the redirect and authentication.
            instance.setActiveAccount(response.account);
            const { idToken } = response;
            await loginSso(idToken);
            await refreshNotifications();
            return <Redirect to="/dashboard" />;
          }
        })
        .catch((error) => {
          console.error('Handle redirect promise error:', error);
        });
  }, [instance]);

  const handleLogin = () => {
    instance
        .loginRedirect({
          scopes: ["User.Read"], // Define the scopes required for your app
          prompt: "select_account", // Forces the user to select an account each time
        })
        .then(async (response) => {
          instance.setActiveAccount(response.account);
          const { idToken } = response;
          await loginSso(idToken);
        })
        .catch((error) => {

        });
  };

  const loginSso = async (idToken) => {
    setLoading(true);
    try {
      const res = await getLoginSso(idToken);
      const session = res.data;
      setLoading(false);
      if (isRoleAllowed([Settings.ROLE_STUDENT_STR], session.Roles)) {
        postCrossRedirectStudentLogged(session);
      } else {
        SessionManager.setSession(session);
        history.push("/dashboard");
      }
    } catch (error) {
      setLoading(false);
    }
  };

  const postCrossRedirectStudentLogged = (session) => {
    let iframe = document.createElement('iframe');
    iframe.src = Settings.URL_SITE_STUDENTS;
    iframe.loading = 'lazy';
    iframe.style = 'visibility: hidden;';
    document.body.appendChild(iframe);
    iframe.onload = () => {
      let win = iframe.contentWindow;
      win.postMessage({ type: 'login', body: session }, Settings.URL_SITE_STUDENTS);
      const url = `${Settings.URL_SITE_STUDENTS}/students/home`;
      window.location.href = url;
    }
  };

  return (
    <>
      <FormGroup row>
        <Col sm={{ size: 10, offset: 1 }}>
          <button type="button" class="btn-Microsoft" onClick={handleLogin}>
            <img src={AzureIcon} /> <Translate id="login.loginSso" />
          </button>
        </Col>
      </FormGroup>
      {loading ? <LoaderFullWindow /> : null}
    </>
  );
};

export default Sso;
