import React, { useState } from "react";

export default function Accordion({
  id = "accordion-1",
  title,
  children,
  open = false,
}) {
  const [isOpen, setIsOpen] = useState(open);

  const handleOnChange = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="wrapperAcordeon">
      <input
        className="acordeonToggle-box"
        id={id}
        type="checkbox"
        checked={isOpen}
        onChange={handleOnChange}
      />
      <label htmlFor={id} className="d-flex align-items-center">
        <div className="acordeonToggle-box--title">{title}</div>
      </label>
      <div className="acordeonContent">{children}</div>
    </div>
  );
}
