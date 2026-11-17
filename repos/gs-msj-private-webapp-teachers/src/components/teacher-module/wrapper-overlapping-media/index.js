import React, { useRef } from "react";
import { usePreventMediaOverlapping } from "../../../hooks/usePreventMediaOverlapping";
const WrapperOverlappingMedia = ({ children, section }) => {
    const containerRef = useRef(null);
    usePreventMediaOverlapping(containerRef, section);
  
    return <div ref={containerRef}>{children}</div>;
  };
export default WrapperOverlappingMedia;