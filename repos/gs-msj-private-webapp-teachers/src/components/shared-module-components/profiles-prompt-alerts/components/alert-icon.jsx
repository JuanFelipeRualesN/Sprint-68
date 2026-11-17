import React from "react";

/**
 * Inline warning triangle used by the prompt alert pieces. It is drawn with
 * plain SVG (no icon library) so the module stays portable, and it inherits the
 * color of its container through `currentColor`.
 *
 * @param {number} [size] Width and height in pixels.
 */
const AlertIcon = ({ size = 18 }) => (
  <svg
    viewBox="0 0 24 24"
    width={size}
    height={size}
    fill="none"
    stroke="currentColor"
    strokeWidth="2.2"
    strokeLinecap="round"
    strokeLinejoin="round"
    aria-hidden="true"
    focusable="false"
  >
    <path d="M12 3 2.8 19h18.4L12 3Z" />
    <path d="M12 9v4" />
    <path d="M12 16.5h.01" />
  </svg>
);

export default AlertIcon;
