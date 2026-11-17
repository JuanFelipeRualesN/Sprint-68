import React, { useState, createContext, useEffect } from "react";
import SessionManager from "../../../../../custom-classes/session-manager";
import LanguageManager from "../../../../../custom-classes/language-manager";

export const BreadcrumbContext = createContext({
  paths: [],
  setPath: () => {},
  resetPaths: () => {},
});

const BreadcrumbProvider = ({ children }) => {
  const [paths, setPaths] = useState([]);
  const [currentLanguage, setCurrentLanguage] = useState(LanguageManager.getLanguage());

  // Load from sessionStorage when mount
  useEffect(() => {
    const stored = SessionManager.getBreadcrumb();
    if (stored) {
      try {
        setPaths(JSON.parse(stored));
      } catch (e) { }
    }
  }, []);

  // Save in sessionStorage each paths updates
  useEffect(() => {
    SessionManager.setBreadcrumb(JSON.stringify(paths));
  }, [paths]);

  const setPath = (item) => {
    setPaths((previous) => {
      const index = previous.findIndex((p) => p.name === item.name);
      const updated = index >= 0 ? previous.slice(0, index + 1) : [...previous, item];
      return updated;
    });
  };




  const resetPaths = () => {
    setPaths([]);
    SessionManager.resetBreadcrumb();
  };

  return (
    <BreadcrumbContext.Provider value={{ paths, setPath, resetPaths }}>
      {children}
    </BreadcrumbContext.Provider>
  );
};

export default BreadcrumbProvider;
