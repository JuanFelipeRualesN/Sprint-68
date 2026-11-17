import React from 'react'
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { registerLocale } from "react-datepicker";
import enUS from "date-fns/locale/en-US";
import es from "date-fns/locale/es";
import LanguageManager from '../custom-classes/language-manager';

registerLocale("en", enUS);
registerLocale("es", es);

// Centralized component to manage the locale of the date picker
const LocalizedDatePicker = (({...rest}) => {
  const appLocale = LanguageManager.getLanguage(); 
  const locale = ["en", "es"].includes(appLocale) ? appLocale : "en";

  return (
    <DatePicker
      locale={locale}
      {...rest}
    />
  );
});

export default LocalizedDatePicker;
