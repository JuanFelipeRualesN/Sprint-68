import React, { useEffect, useState } from "react";

export default function Loading({ show, type = "button", text }) {
  const [hidden, setHidden] = useState(!show);

  useEffect(() => {
    setHidden(!show);
  }, [show]);

  return (
    <>
      {type !== "button" ? (
        <div className={`loading-${type}`}>
          <div className="spinner" hidden={hidden}></div>
        </div>
      ) : (
        <div className={`loading-${type}`}>
          <div className={`spinner-${type}`} hidden={hidden}></div>
          {text}
        </div>
      )}
    </>
  );
}
