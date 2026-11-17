import React from "react";

export default function FormLabel({ label, forHtml = "input", className = "" }) {
  return (
    <label htmlFor={forHtml} className={`control-label-w-placeholder ${className}`}>
      {label}
    </label>
  );
}
