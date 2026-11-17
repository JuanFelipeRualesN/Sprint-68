/**
 * Created by portiz on 2/4/20.
 */
import React, { useRef, useEffect } from 'react';
const CanvasLegend = ({ color }) => {
    const canvasRef = useRef(null);

    useEffect(() => {
        const canvas = canvasRef.current;
        const ctx = canvas.getContext("2d");
        ctx.fillStyle = color;
        ctx.fillRect(0, 0, 20, 20);
    }, [color]);

    return(
        <div style={{float: "left"}}>
            <canvas ref={canvasRef} width={20} height={20} />
        </div>
    );
};

export default CanvasLegend