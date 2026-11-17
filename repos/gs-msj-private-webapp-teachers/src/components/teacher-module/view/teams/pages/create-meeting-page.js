import React, { useEffect } from "react";
import { renderToStaticMarkup } from "react-dom/server";
import { withLocalize } from "react-localize-redux";
import CreateMeeting from "../components/create-meeting";
import useVersionedTranslations from "../../../../../hooks/useVersionedTranslations";

const CreateMeetingPage = () => {

  // Initialize translations based on the current version
  useVersionedTranslations();

  return (
    <>
      <div className="container-padding animated delay-05s fadeIn">
        <div className="wrapper-principal-container">
          <CreateMeeting />
        </div>
      </div>
    </>
  );
};

export default withLocalize(CreateMeetingPage);
