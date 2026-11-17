import React from "react";

export default function Text({ type, children, className = "" }) {
  return <div className={`text-${type} ${className}`}>{children}</div>;
}
