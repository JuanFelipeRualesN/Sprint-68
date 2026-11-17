import React from "react";

export default function Space({ side = "bottom", variant = "lg" }) {
  return <div className={`space-margin-${side}-${variant}`}></div>;
}
