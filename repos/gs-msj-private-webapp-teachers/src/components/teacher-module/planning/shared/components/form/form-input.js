import React, { useState } from "react";

export default function FormInput({
  name,
  placeholder,
  value,
  disabled,
  onChange,
  onEnter = () => {},
    maxLength,
}) {
  return (
    <>
      <input
        type="text"
        name={name}
        placeholder={placeholder}
        onChange={onChange}
        maxLength={maxLength}
        style={{borderRadius: '.2rem' }}
        onKeyDown={(event) => {
          if (event.key === "Enter") onEnter(event.target.value);
        }}
        value={value}
        disabled={disabled}
      />
      <i className="bar"></i>
    </>
  );
}
