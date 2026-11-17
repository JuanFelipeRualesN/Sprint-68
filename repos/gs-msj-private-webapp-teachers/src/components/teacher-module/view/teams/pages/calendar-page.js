import React, { useEffect } from 'react';
import {renderToStaticMarkup} from "react-dom/server";
import {withLocalize} from "react-localize-redux";
import Calendar from "../components/calendar";
import useVersionedTranslations from '../../../../../hooks/useVersionedTranslations';

const CalendarPage = () => {
  
  // Initialize translations based on the current version
  useVersionedTranslations();

  return (
    <>
      <div className="container-padding animated delay-05s fadeIn">
        <div className="wrapper-principal-container">
          <Calendar />
        </div>
      </div>
    </>
  );
};

export default withLocalize(CalendarPage);
