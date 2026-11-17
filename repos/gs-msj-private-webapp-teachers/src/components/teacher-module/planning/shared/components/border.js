import React from "react";

export default function Border({ side = "Bottom", children, className = "" }) {
  return (
    <div className={`Region-w-border${side} ${className}`}>{children}</div>
  );
}
