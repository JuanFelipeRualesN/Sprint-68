import React from "react";

export default function FormLabelValue({ label, value }) {
  return (
    <span>
      <strong>{label}</strong>: {value}
    </span>
  );
}
