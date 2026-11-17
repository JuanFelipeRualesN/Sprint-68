import React, { createContext } from "react";

export const PreviewTableContext = createContext({
  headers: [],
  viewAsPDF: false,
});

const PreviewTableProvider = ({ headers, children, viewAsPDF = false }) => {
  return (
    <PreviewTableContext.Provider value={{ headers, viewAsPDF }}>
      {children}
    </PreviewTableContext.Provider>
  );
};

export default PreviewTableProvider;
