import React, { useRef, useState } from "react";
import { ResponsiveContainer } from "recharts";
import { TriangleAlert, Maximize2 } from "lucide-react";
import GraficaAmpliada from "./grafica-ampliada";
import { construirGrafica, datosVistaPrevia, totalesDe, esBarras, ALTO_VISTA_PREVIA } from "./grafica-base";

/**
 * Vista previa de una gráfica en el chat (Fase 4 · REQ-3741): compacta, de altura fija (200 px), top 10 y el valor al final de cada barra.
 * «Ampliar» (⤢) y «Ver todas» abren la ventana de grafica-ampliada.js. La especificación (`graficas[i]`) la arma el SERVICIO; el modelo no escribe cifras.
 */
const Grafica = ({ spec, t, idioma, descarga }) => {
  const [ampliada, setAmpliada] = useState(null); // null | "top" | "todas": con qué vista se abrió la ventana
  const botonRef = useRef(null);
  const titulo = spec.titulo || "";
  const data = datosVistaPrevia(spec);
  const { mostradas, total, ceros } = totalesDe(spec, data);
  const { cuerpo, alto } = construirGrafica(spec, data, { t, idioma, compacto: true });
  const resumen = `${titulo}. ${mostradas} ${t.puntos}`;
  const hayMas = esBarras(spec) && total > mostradas;

  return (
    <figure className={`asst-grafica${spec.destacar ? " asst-grafica--destacada" : ""}`}>
      <div className="asst-grafica-cab">
        <figcaption className="asst-grafica-titulo" title={titulo}>{titulo}</figcaption>
        <button type="button" ref={botonRef} className="asst-grafica-ampliar" onClick={() => setAmpliada("top")} aria-label={`${t.ampliar}: ${titulo}`} title={t.ampliar}>
          <Maximize2 size={14} />
        </button>
      </div>
      <div className="asst-grafica-lienzo" role="img" aria-label={resumen} style={{ height: Math.min(alto, ALTO_VISTA_PREVIA) }}>
        <ResponsiveContainer width="100%" height="100%">{cuerpo}</ResponsiveContainer>
      </div>
      {(hayMas || ceros > 0) && (
        <div className="asst-grafica-nota">
          {t.nDeM(mostradas, total)}
          {ceros > 0 && ` · ${t.cerosOcultos(ceros)}`}
          {hayMas && <> · <button type="button" className="asst-enlace" onClick={() => setAmpliada("todas")}>{t.verTodas}</button></>}
        </div>
      )}
      {spec.hay_mas_filas && <div className="asst-aviso-dato"><TriangleAlert size={13} /> <span>{t.hayMasFilas}</span></div>}
      {ampliada && (
        <GraficaAmpliada spec={spec} descarga={descarga} t={t} idioma={idioma} inicialTodas={ampliada === "todas"} onCerrar={() => setAmpliada(null)} />
      )}
    </figure>
  );
};

export default Grafica;
