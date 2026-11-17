import React, { useEffect } from "react";
import {withLocalize} from "react-localize-redux";
import {withRouter} from "react-router-dom";
import {renderToStaticMarkup} from "react-dom/server";
import SignIn from "../components/signin";
import useVersionedTranslations from "../../../../../hooks/useVersionedTranslations";
import SessionManager from "../../../../../custom-classes/session-manager";
import { SendAccessLog } from "../../../../../utils/access_logger";

const TeamsSignin = () => {
  const sessionDetails = SessionManager.getSessionDetails();
  
  // Initialize translations based on the current version
  useVersionedTranslations();
  useEffect(() => {
    try{            
        SendAccessLog({
          Username: sessionDetails.Username,
          InvitationCode: sessionDetails.InvitationCode,
          Role: sessionDetails.Roles[0],
          AreaId: "10"
        })
    }
    catch (error){
        // empty catch block to avoid breaking the application
    }
  }, []);

  return (
    <>
      <div className="container-padding animated delay-05s fadeIn">
        <div className="wrapper-principal-container">
          <SignIn />
        </div>
      </div>
    </>
  );
};

export default withRouter(withLocalize(TeamsSignin));
