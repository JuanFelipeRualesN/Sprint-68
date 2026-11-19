import React from "react";
import {
  BarChart, Bar, LineChart, Line, PieChart, Pie, Cell,
  XAxis, YAxis, CartesianGrid, Tooltip, Legend, LabelList,
} from "recharts";

/**
 * Gráfica del asistente (Fase 4 · REQ-3741). Dibuja con Recharts la especificación que arma el SERVICIO
 * (`graficas[i]`, ver leerAdjuntos en assistant-api.js): el modelo no escribe cifras. Sin SVG propio.
 *   tipo "line" | "bar" | "pie"; layout (solo bar): "horizontal" = columnas, "vertical" = barras horizontales.
 * Aquí está lo compartido por la vista previa del chat (grafica.js) y la ventana ampliada (grafica-ampliada.js).
 */
export const COLORES = ["#1A7BAA", "#E08A1E", "#2E9E6B", "#8E5BC0", "#D1495B"];
export const ALTO_VISTA_PREVIA = 200;
export const MAX_VISTA_PREVIA = 10;

export const recortar = (v, max = 18) => {
  const s = String(v == null ? "" : v);
  return s.length > max ? `${s.slice(0, max - 1)}…` : s;
};

/** Etiqueta de categoría en UNA línea (Recharts parte los textos largos en varias): se recorta con «…» y el nombre completo va en <title> y en el tooltip. */
export const tickNombre = (max, fuente) => function TickNombre({ x, y, payload }) {
  const completo = String(payload && payload.value != null ? payload.value : "");
  return (
    <text x={x - 6} y={y} dy={fuente * 0.35} textAnchor="end" fontSize={fuente} fill="#475569">
      <title>{completo}</title>
      {recortar(completo, max)}
    </text>
  );
};

export const esBarras = (spec) => spec.tipo === "bar";

/** Categorías de la vista previa: el servicio ya manda las 10 primeras; si viniera más (servicio antiguo) se recortan aquí. */
export const datosVistaPrevia = (spec) => (esBarras(spec) ? spec.data.slice(0, MAX_VISTA_PREVIA) : spec.data);
/** «10 de 47»: categorías mostradas y las que hay (con las opciones de la gráfica). Sin los campos nuevos del servicio, lo que trae data. */
export const totalesDe = (spec, data) => ({
  mostradas: data.length,
  total: Math.max(typeof spec.total_categorias === "number" ? spec.total_categorias : data.length, data.length),
  ceros: typeof spec.categorias_en_cero === "number" && spec.incluir_ceros === false ? spec.categorias_en_cero : 0,
});

/**
 * Arma el gráfico de Recharts. `compacto` = vista previa del chat; si no, la ventana ampliada (`anchoEje` = ancho del eje de nombres, `altoBarra` = alto por categoría).
 * Devuelve { cuerpo, alto, vertical } para envolverlo en un ResponsiveContainer del alto indicado (no hay ancho fijo: se ajusta al contenedor).
 */
export function construirGrafica(spec, data, { t, idioma, compacto, anchoEje = 112, altoBarra = 28, altoLinea = 320 }) {
  const claveX = (spec.eje_x && spec.eje_x.clave) || "categoria";
  const unidad = (spec.eje_y && spec.eje_y.unidad) || "";
  const esPct = unidad === "%";
  const formato = (v) => {
    if (v === null || v === undefined) return "—";
    const n = typeof v === "number" ? v.toLocaleString(idioma === "en" ? "en-US" : "es-CO") : v;
    return esPct ? `${n}%` : n;
  };
  const fuente = compacto ? 10 : 12;
  const eje = { fontSize: fuente };
  // tooltip con el nombre COMPLETO de la categoría (los ejes la recortan)
  const tooltip = <Tooltip formatter={(v, nombre) => [formato(v), nombre]} labelFormatter={(l) => String(l)} cursor={{ fill: "rgba(15,23,42,0.06)" }} wrapperStyle={{ zIndex: 20, maxWidth: "90%", whiteSpace: "normal" }} />;
  const leyenda = spec.series.length > 1 || spec.tipo === "pie" ? <Legend wrapperStyle={{ fontSize: fuente }} /> : null;
  const unaSerie = spec.series.length === 1;
  const etiquetaValor = (posicion) => unaSerie && (
    <LabelList dataKey={spec.series[0].clave} position={posicion} formatter={formato} style={{ fontSize: fuente, fill: "#334155" }} />
  );
  const maxEtiqueta = compacto ? 14 : Math.max(18, Math.floor(anchoEje / 6.3));

  if (spec.tipo === "pie") {
    const s = spec.series[0];
    return {
      alto: compacto ? ALTO_VISTA_PREVIA : 360,
      cuerpo: (
        <PieChart>
          <Pie data={data} dataKey={s.clave} nameKey={claveX} outerRadius={compacto ? 62 : 120} label={false}>
            {data.map((_, i) => <Cell key={i} fill={COLORES[i % COLORES.length]} />)}
          </Pie>
          <Tooltip formatter={(v) => [formato(v), s.nombre]} />
          {leyenda}
        </PieChart>
      ),
    };
  }
  if (spec.tipo === "line") {
    return {
      alto: compacto ? ALTO_VISTA_PREVIA : altoLinea,
      cuerpo: (
        <LineChart data={data} margin={{ top: 10, right: 18, left: 0, bottom: 4 }}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey={claveX} tick={eje} tickFormatter={(v) => recortar(v, compacto ? 8 : 14)} interval={compacto && data.length > 6 ? "preserveStartEnd" : 0} />
          <YAxis tick={eje} tickFormatter={esPct ? (v) => `${v}%` : undefined} width={compacto ? 40 : 56} />
          {tooltip}{leyenda}
          {spec.series.map((s, i) => <Line key={s.clave} type="monotone" dataKey={s.clave} name={s.nombre || s.clave} stroke={COLORES[i % COLORES.length]} strokeWidth={2} dot={{ r: compacto ? 2 : 3 }} connectNulls={false} />)}
        </LineChart>
      ),
    };
  }
  // barras
  const vertical = spec.layout === "vertical"; // barras horizontales: categorías en el eje Y
  const dominio = [0, "auto"]; // el valor al final de la barra va fuera del área de trazado (margen derecho), así los ejes quedan en números redondos
  if (vertical) {
    const alto = compacto ? ALTO_VISTA_PREVIA : Math.max(220, data.length * altoBarra * (compacto ? 1 : spec.series.length) + 64);
    return {
      alto, vertical,
      cuerpo: (
        <BarChart data={data} layout="vertical" margin={{ top: 4, right: compacto ? 34 : 56, left: 0, bottom: compacto ? 0 : 4 }} barCategoryGap={compacto ? "18%" : "22%"}>
          {!compacto && <CartesianGrid strokeDasharray="3 3" horizontal={false} />}
          <XAxis type="number" hide={compacto} domain={dominio} tick={eje} tickFormatter={esPct ? (v) => `${v}%` : undefined} />
          <YAxis type="category" dataKey={claveX} tick={tickNombre(maxEtiqueta, fuente)} width={compacto ? 100 : anchoEje} interval={0} />
          {tooltip}{leyenda}
          {spec.series.map((s, i) => (
            <Bar key={s.clave} dataKey={s.clave} name={s.nombre || s.clave} fill={COLORES[i % COLORES.length]} maxBarSize={compacto ? 16 : 26} isAnimationActive={false}>
              {i === 0 && etiquetaValor("right")}
            </Bar>
          ))}
        </BarChart>
      ),
    };
  }
  const muchas = data.length > (compacto ? 4 : 8);
  return {
    alto: compacto ? ALTO_VISTA_PREVIA : 380, vertical,
    cuerpo: (
      <BarChart data={data} layout="horizontal" margin={{ top: 18, right: 12, left: 0, bottom: 4 }}>
        <CartesianGrid strokeDasharray="3 3" vertical={false} />
        <XAxis dataKey={claveX} tick={eje} tickFormatter={(v) => recortar(v, compacto ? 9 : 16)} interval={0} angle={muchas ? -35 : 0} textAnchor={muchas ? "end" : "middle"} height={muchas ? (compacto ? 46 : 70) : 28} />
        <YAxis tick={eje} domain={dominio} tickFormatter={esPct ? (v) => `${v}%` : undefined} width={compacto ? 40 : 52} />
        {tooltip}{leyenda}
        {spec.series.map((s, i) => (
          <Bar key={s.clave} dataKey={s.clave} name={s.nombre || s.clave} fill={COLORES[i % COLORES.length]} maxBarSize={compacto ? 22 : 36} isAnimationActive={false}>
            {i === 0 && etiquetaValor("top")}
          </Bar>
        ))}
      </BarChart>
    ),
  };
}

