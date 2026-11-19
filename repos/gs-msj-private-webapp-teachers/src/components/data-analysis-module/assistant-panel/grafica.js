import React from "react";
import {
  ResponsiveContainer, BarChart, Bar, LineChart, Line, PieChart, Pie, Cell,
  XAxis, YAxis, CartesianGrid, Tooltip, Legend,
} from "recharts";
import { TriangleAlert } from "lucide-react";

/**
 * Gráfica del asistente (Fase 4 · REQ-3741). Dibuja con Recharts la especificación que arma el SERVICIO
 * (`graficas[i]`, ver leerAdjuntos en assistant-api.js): el modelo no escribe cifras. Sin SVG propio.
 *   tipo "line" | "bar" | "pie"; layout (solo bar): "horizontal" = columnas, "vertical" = barras horizontales.
 */
const COLORES = ["#1A7BAA", "#E08A1E", "#2E9E6B", "#8E5BC0", "#D1495B"];
const MAX_ETIQUETA = 18;

const recortar = (v) => {
  const s = String(v == null ? "" : v);
  return s.length > MAX_ETIQUETA ? `${s.slice(0, MAX_ETIQUETA - 1)}…` : s;
};

const Grafica = ({ spec, t, idioma }) => {
  const claveX = (spec.eje_x && spec.eje_x.clave) || "categoria";
  const unidad = (spec.eje_y && spec.eje_y.unidad) || "";
  const esPct = unidad === "%";
  const formato = (v) => {
    if (v === null || v === undefined) return "—";
    const n = typeof v === "number" ? v.toLocaleString(idioma === "en" ? "en-US" : "es-CO") : v;
    return esPct ? `${n}%` : n;
  };
  const tooltip = <Tooltip formatter={(v, nombre) => [formato(v), nombre]} />;
  const leyenda = spec.series.length > 1 || spec.tipo === "pie" ? <Legend wrapperStyle={{ fontSize: 11 }} /> : null;
  const eje = { fontSize: 11 };
  const titulo = spec.titulo || "";
  const resumen = `${titulo}. ${spec.data.length} ${t.puntos}`;

  let cuerpo;
  let alto = 240;
  if (spec.tipo === "pie") {
    const s = spec.series[0];
    cuerpo = (
      <PieChart>
        <Pie data={spec.data} dataKey={s.clave} nameKey={claveX} outerRadius={80} label={false}>
          {spec.data.map((_, i) => <Cell key={i} fill={COLORES[i % COLORES.length]} />)}
        </Pie>
        <Tooltip formatter={(v) => [formato(v), s.nombre]} />
        {leyenda}
      </PieChart>
    );
  } else if (spec.tipo === "line") {
    cuerpo = (
      <LineChart data={spec.data} margin={{ top: 8, right: 22, left: 0, bottom: 4 }}>
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey={claveX} tick={eje} tickFormatter={recortar} />
        <YAxis tick={eje} tickFormatter={esPct ? (v) => `${v}%` : undefined} width={48} />
        {tooltip}{leyenda}
        {spec.series.map((s, i) => <Line key={s.clave} type="monotone" dataKey={s.clave} name={s.nombre || s.clave} stroke={COLORES[i % COLORES.length]} strokeWidth={2} dot={{ r: 3 }} connectNulls={false} />)}
      </LineChart>
    );
  } else {
    const vertical = spec.layout === "vertical"; // barras horizontales: categorías en el eje Y
    if (vertical) alto = Math.max(220, spec.data.length * 26 * spec.series.length + 56);
    cuerpo = (
      <BarChart data={spec.data} layout={vertical ? "vertical" : "horizontal"} margin={{ top: 8, right: 16, left: 0, bottom: 4 }}>
        <CartesianGrid strokeDasharray="3 3" />
        {vertical ? (
          <>
            <XAxis type="number" tick={eje} tickFormatter={esPct ? (v) => `${v}%` : undefined} />
            <YAxis type="category" dataKey={claveX} tick={eje} tickFormatter={recortar} width={112} interval={0} />
          </>
        ) : (
          <>
            <XAxis dataKey={claveX} tick={eje} tickFormatter={recortar} interval={0} angle={spec.data.length > 5 ? -30 : 0} textAnchor={spec.data.length > 5 ? "end" : "middle"} height={spec.data.length > 5 ? 64 : 30} />
            <YAxis tick={eje} tickFormatter={esPct ? (v) => `${v}%` : undefined} width={48} />
          </>
        )}
        {tooltip}{leyenda}
        {spec.series.map((s, i) => <Bar key={s.clave} dataKey={s.clave} name={s.nombre || s.clave} fill={COLORES[i % COLORES.length]} maxBarSize={28} />)}
      </BarChart>
    );
  }

  return (
    <figure className={`asst-grafica${spec.destacar ? " asst-grafica--destacada" : ""}`}>
      <figcaption className="asst-grafica-titulo">{titulo}</figcaption>
      <div className="asst-grafica-lienzo" role="img" aria-label={resumen} style={{ height: alto }}>
        <ResponsiveContainer width="100%" height="100%">{cuerpo}</ResponsiveContainer>
      </div>
      {spec.hay_mas_filas && <div className="asst-aviso-dato"><TriangleAlert size={13} /> <span>{t.hayMasFilas}</span></div>}
    </figure>
  );
};

export default Grafica;
