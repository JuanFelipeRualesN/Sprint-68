import "react-app-polyfill/ie9";
import "react-app-polyfill/ie11";
import React, { useReducer, useEffect } from "react";
import ReactDOM from "react-dom";
import "./index.css";
import "bootstrap/dist/css/bootstrap.css";
import { Route, Router, Switch, Redirect } from "react-router-dom";
import LayoutRoute from "./components/route/layout-route";
import LayoutPrivateRoute from "./components/route/layout-private-route";
import { LocalizeProvider } from "react-localize-redux";
import "./i18n";
import Login from "./components/guest-module/view/login";
import PasswordReset from "./components/guest-module/view/password-reset";
import DownloadPage from "./components/guest-module/view/download-page";
import SubscriptionDownload from "./components/shared-module-components/form/form-subscription-download";
import PlanSelect from "./components/guest-module/view/plan-select";
import AccountAccessRecovery from "./components/guest-module/view/account-access-recovery";
import UserHelpWindow from "./components/guest-module/view/user-manual";
import TagManager from "react-gtm-module";
import Settings from "../src/custom-classes/settings";
import Payment from "./components/guest-module/v2-first-time-account-flow/payment";
import RegisterV2 from "./components/guest-module/v2-first-time-account-flow/register-v2";
import RegisterFree from "./components/guest-module/view/register-free";
import TeachersLanding from "./components/teacher-module/view/teachers";
import Exam from "./components/teacher-module/view/exam";
import ExamUpload from "./components/teacher-module/view/exam-upload";
import Terms from "./components/guest-module/view/terms";
import Privacy from "./components/guest-module/view/privacy";
import BillingTerms from "./components/guest-module/view/billing-terms";
import PromoSubscription from "./components/guest-module/v2-first-time-account-flow/promo-subscription";
import Footer from "./components/shared-module-components/footer/footer-simple/footer-simple";
import Maintenance from "./components/shared-module-components/maintenance/maintenance";
import { createBrowserHistory } from "history";
import { PublicClientApplication, EventType } from "@azure/msal-browser";
import { MsalProvider } from "@azure/msal-react";
import { msalConfig } from "./authConfig";
import { getSettingsPowerDe } from "./api/gsweb";
import { AppContext, initialState } from "./App.Context";
import appReducer from "./app.reducer";
import { appReducerCases } from "./types";
import 'quill-table-better/dist/quill-table-better.css'
import LearningGapView from "./components/teacher-module/learning-gap/learning-gap-view";
import store from "./redux-students-area/store";
import { Provider } from 'react-redux';
import { Loader } from "./components/student-module/view/lesson-student/common/styled-components/loader";
import { Suspense } from "react";
import { getLearningGapTeacherHasNotifications } from "./api/teachers";
import sessionManager from "./custom-classes/session-manager";
import {loadStripe} from "@stripe/stripe-js";
import LanguageManager from "./custom-classes/language-manager";
import {Elements} from "@stripe/react-stripe-js";
import ChatbotButton from "./components/teacher-module/chatbot/chatbot-btn/chatbot-button";

const tagManagerArgs = {
  gtmId: Settings.GOOGLE_TAG_MANAGER_ID,
};
TagManager.initialize(tagManagerArgs);

const msalInstance = new PublicClientApplication(msalConfig);

const accounts = msalInstance.getAllAccounts();
if (accounts && accounts.length > 0) {
  msalInstance.setActiveAccount(accounts[0]);
}

export const history = createBrowserHistory();

const App = () => {
  const [state, dispatch] = useReducer(appReducer, initialState);

  useEffect(() => {
    window.addEventListener("message", (e) => {
      if (e.origin !== Settings.URL_SITE_STUDENTS) {
        return;
      }
      const user = e.data;
      localStorage.setItem("auth", JSON.stringify(user));
    });
    getParametersPDE();
    refreshNotifications();
  }, []);

  // Check if teacher has active notifications
  const refreshNotifications = async () => {
    try {
      if (sessionManager.getSession() !== null && sessionManager.getUserRole().toLowerCase() === Settings.ROLE_TEACHER_STR.toLowerCase()) {
        const response = await getLearningGapTeacherHasNotifications();
        dispatch({
          type: appReducerCases.setHasNotifications,
          payload: response.data
        });
      }
    } catch (error) {
      console.error('Error fetching notifications:', error);
    }
  };

  const getParametersPDE = async () => {
    const response = await getSettingsPowerDe();
    const data = response.data;
    const { Valid, Messages, Id, ...settings } = data;
    dispatch({
      type: appReducerCases.setParameters,
      payload: { Id, ...settings },
    });
  };

  const stripePromise = React.useMemo(() => loadStripe(Settings.stripeTokenClient, {
    locale: LanguageManager.getLanguage(),
  }), []);

  return (
      <Elements stripe={stripePromise}>
      <LocalizeProvider>
        <Router history={history}>
          <AppContext.Provider value={{ state, dispatch, refreshNotifications }}>
            {!Settings.IS_APP_MAINTENANCE ? <>
              <Switch>
                {/* <Route push exact path="/">
                <Redirect to="/login" />
              </Route> */}
                <Route push path="/download" component={DownloadPage} />
                <Route push exact path="/" component={Login} />
                <Route push path="/password-reset" component={PasswordReset} />
                <Route
                    push
                    path="/subscription-download"
                    component={SubscriptionDownload}
                />
                <Route
                    push
                    path="/account-access-recovery"
                    component={AccountAccessRecovery}
                />

                <Route push path="/payment" component={Payment} />
                <Route
                    push
                    path="/promo-subscription"
                    component={PromoSubscription}
                />
                <Route push path="/register-v2" component={RegisterV2} />
                <Route push path="/register-free" component={RegisterFree} />
                <Route push path="/teachers" component={TeachersLanding} />
                <Route push path="/exam" component={Exam} />
                <Route push path="/exam-upload" component={ExamUpload} />
                <Route push path="/terms">
                  <Terms />
                </Route>
                <Route push path="/privacy">
                  <Privacy />
                </Route>
                <Route push path="/billing-terms">
                  <BillingTerms />
                </Route>
                <LayoutPrivateRoute path="/" component={LayoutRoute} />
              </Switch>
              {sessionManager.getSession() !== null &&  <ChatbotButton />}
              <Footer />
            </> : <Maintenance />}
          </AppContext.Provider>
        </Router>
      </LocalizeProvider>
      </Elements>
  );
}

// Load CSS file based on the project version and attach it to the document
const loadCSS = (theme) => {
  return new Promise((resolve, reject) => {
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = `/themes/theme-${theme}.css`;
    link.onload = () => resolve();
    link.onerror = () => reject(`Unable to load the theme ${theme}`);
    document.head.appendChild(link);
  });
};

// Set the document title and favicon based on the version
document.title = Settings.PROJECT_NAME;

const favicon = document.querySelector("link[rel='icon']");
if (favicon) {
  favicon.href = `/${Settings.PROJECT_VERSION}-favicon.ico`;
}

loadCSS(Settings.PROJECT_VERSION).then(() => {
  ReactDOM.render(
    <Provider store={store}>
      <Suspense fallback={<Loader />}>
        <MsalProvider instance={msalInstance}>
          <App />
        </MsalProvider>
      </Suspense>
    </Provider>,
    document.getElementById("root")
  );
});
