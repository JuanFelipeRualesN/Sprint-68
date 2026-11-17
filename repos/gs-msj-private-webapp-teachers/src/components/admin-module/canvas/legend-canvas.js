/**
 * Created by portiz on 2/4/20.
 */
import React, { useRef, useEffect } from 'react';

/**
 * Used to complement the charts with a legend
 **/
const CanvasLegend= ({ color }) => {
    const canvasRef = useRef(null);

    const drawCanvas = (canvasColor) => {
        const canvas = canvasRef.current;
        if (canvas) {
            const ctx = canvas.getContext("2d");
            ctx.fillStyle = canvasColor;
            ctx.fillRect(0, 0, 20, 20);
        }
    };

    useEffect(() => {
        drawCanvas(color);
    }, [color]);

    return(
        <div style={{float: "left"}}>
            <canvas ref={canvasRef} width={20} height={20} />
        </div>
    );
};

export default CanvasLegend