import React from "react";

export default function Heading({ as, className = "", children }) {
  const headingClass = `heading-${as} ${className}`;

  switch (as) {
    case "h1":
      return <h1 className={headingClass}>{children}</h1>;
    case "h2":
      return <h2 className={headingClass}>{children}</h2>;
    case "h3":
      return <h3 className={headingClass}>{children}</h3>;
    case "h4":
      return <h4 className={headingClass}>{children}</h4>;
    case "h5":
      return <h5 className={headingClass}>{children}</h5>;
    case "h6":
      return <h6 className={headingClass}>{children}</h6>;
  }
}
