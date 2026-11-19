import React from "react";
import {
  BarChart, Bar, LineChart, Line, PieChart, Pie, Cell, Label,
  XAxis, YAxis, CartesianGrid, Tooltip, Legend, LabelList,
} from "recharts";

/**
 * Gráfica del asistente (Fase 4 · REQ-3741). Dibuja con Recharts la especificación que arma el SERVICIO
 * (`graficas[i]`, ver leerAdjuntos en assistant-api.js): el modelo no escribe cifras. Sin SVG propio.
 *   tipo "line" | "bar" | "pie"; layout (solo bar): "horizontal" = columnas, "vertical" = barras horizontales;
 *   agrupar "grouped" = barras con varias series lado a lado; variante "dona" (pie) con `total_valor` en el centro;
 *   extra {clave, nombre, unidad} = medida en otra escala (p. ej. % de participación) que va en el tooltip y en la etiqueta del valor, no como serie;
 *   serie_temporal = el eje X es el tiempo (solo ellas pueden verse como línea).
 * Aquí está lo compartido por la vista previa del chat (grafica.js) y la ventana ampliada (grafica-ampliada.js).
 */
// Colores de nivel medio: se leen bien sobre fondo claro y oscuro. Los textos del SVG usan `currentColor` (heredan el color del contenedor, que sigue el tema).
export const COLORES = ["#3B82C4", "#E8912D", "#2FA37A", "#9A63C9", "#D8505F", "#7C8794"];
// Estados de suscripción: color con significado (verde activa, ámbar inactiva, rojo cancelada, gris sin suscripción)
const COLOR_ESTADO_SERIE = { activas: "#2FA37A", inactivas: "#E8912D", canceladas: "#D8505F", sin_suscripcion: "#7C8794" };
const COLOR_ESTADO_NOMBRE = { Activas: "#2FA37A", Inactivas: "#E8912D", Canceladas: "#D8505F", "Sin suscripción": "#7C8794" };
export const ALTO_VISTA_PREVIA = 200;
export const MAX_VISTA_PREVIA = 10;
export const MAX_DONA = 6; // una dona solo con 6 categorías o menos: con más vuelve a barras

export const TIPOS = { BARRAS: "barras", LINEA: "linea", DONA: "dona" };

export const recortar = (v, max = 18) => {
  const s = String(v == null ? "" : v);
  return s.length > max ? `${s.slice(0, max - 1)}…` : s;
};

export const esBarras = (spec) => spec.tipo === "bar";

export const colorDeSerie = (spec, serie, i) => (spec.consulta === "usuarios_por_suscripcion" && COLOR_ESTADO_SERIE[serie.clave]) || COLORES[i % COLORES.length];
export const colorDeCategoria = (spec, nombre, i) => (spec.consulta === "usuarios_por_suscripcion" && COLOR_ESTADO_NOMBRE[nombre]) || COLORES[i % COLORES.length];

/** Categorías de la vista previa: el servicio ya manda las 10 primeras; si viniera más (servicio antiguo) se recortan aquí. Las donas y las líneas van completas. */
export const datosVistaPrevia = (spec) => (esBarras(spec) ? spec.data.slice(0, MAX_VISTA_PREVIA) : spec.data);
/** «10 de 47»: categorías mostradas y las que hay (con las opciones de la gráfica). Sin los campos nuevos del servicio, lo que trae data. */
export const totalesDe = (spec, data) => ({
  mostradas: data.length,
  total: Math.max(typeof spec.total_categorias === "number" ? spec.total_categorias : data.length, data.length),
  ceros: typeof spec.categorias_en_cero === "number" && spec.incluir_ceros === false ? spec.categorias_en_cero : 0,
});

/** Tipo con el que se dibuja por defecto: el de la especificación; una dona con más de 6 categorías vuelve a barras. */
export const tipoPorDefecto = (spec, data) => {
  if (spec.tipo === "line") return TIPOS.LINEA;
  if (spec.tipo === "pie") return data.length <= MAX_DONA ? TIPOS.DONA : TIPOS.BARRAS;
  return TIPOS.BARRAS;
};
/**
 * Qué tipos se pueden elegir con estos datos: barras siempre; línea solo para series de tiempo; dona solo con 6 categorías o menos y UNA sola medida.
 */
export const tiposPermitidos = (spec, data) => ({
  [TIPOS.BARRAS]: true,
  [TIPOS.LINEA]: spec.tipo === "line" || spec.serie_temporal === true,
  [TIPOS.DONA]: spec.series.length === 1 && data.length >= 2 && data.length <= MAX_DONA,
});

/** Etiqueta de categoría en UNA línea (Recharts parte los textos largos en varias): se recorta con «…» y el nombre completo va en <title> y en el tooltip. */
export const tickNombre = (max, fuente) => function TickNombre({ x, y, payload }) {
  const completo = String(payload && payload.value != null ? payload.value : "");
  return (
    <text x={x - 6} y={y} dy={fuente * 0.35} textAnchor="end" fontSize={fuente} style={{ fill: "currentColor", opacity: 0.78 }}>
      <title>{completo}</title>
      {recortar(completo, max)}
    </text>
  );
};

const numero = (v, idioma) => (typeof v === "number" ? v.toLocaleString(idioma === "en" ? "en-US" : "es-CO") : v);

/** Tooltip con el nombre COMPLETO de la categoría, cada serie y la medida «extra» (p. ej. el % de participación). */
export const tooltipGrafica = (spec, formato, idioma) => function TooltipGrafica({ active, payload, label }) {
  if (!active || !payload || payload.length === 0) return null;
  const fila = payload[0].payload || {};
  const titulo = label != null && label !== "" ? label : payload[0].name;
  const extra = spec.extra;
  return (
    <div className="asst-tooltip" role="tooltip">
      <div className="asst-tooltip-titulo">{String(titulo)}</div>
      {payload.map((p, i) => (
        <div className="asst-tooltip-fila" key={`${p.dataKey || p.name}-${i}`}>
          <span className="asst-tooltip-punto" style={{ background: p.color || (p.payload && p.payload.fill) || "#7C8794" }} />
          {p.name}: <b>{formato(p.value)}</b>
        </div>
      ))}
      {extra && fila[extra.clave] !== undefined && fila[extra.clave] !== null && (
        <div className="asst-tooltip-extra">{extra.nombre}: <b>{numero(fila[extra.clave], idioma)}{extra.unidad === "%" ? " %" : ` ${extra.unidad || ""}`}</b></div>
      )}
    </div>
  );
};

/**
 * Arma el gráfico de Recharts. `compacto` = vista previa del chat; si no, la ventana ampliada (`anchoEje` = ancho del eje de nombres, `altoBarra` = alto por categoría).
 * `tipoVista` (barras | linea | dona) fuerza el tipo (selector de la ventana); sin él se usa el de la especificación.
 * Devuelve { cuerpo, alto, vertical, tipoVista } para envolverlo en un ResponsiveContainer del alto indicado (no hay ancho fijo: se ajusta al contenedor).
 */
export function construirGrafica(spec, data, { t, idioma, compacto, anchoEje = 112, altoBarra = 28, altoLinea = 320, tipoVista } = {}) {
  const vista = tipoVista || tipoPorDefecto(spec, data);
  const claveX = (spec.eje_x && spec.eje_x.clave) || "categoria";
  const unidad = (spec.eje_y && spec.eje_y.unidad) || "";
  const esPct = unidad === "%";
  const formato = (v) => {
    if (v === null || v === undefined) return "—";
    const n = numero(v, idioma);
    return esPct ? `${n}%` : n;
  };
  const fuente = compacto ? (spec.series.length > 1 ? 9 : 10) : 12;
  const eje = { fontSize: fuente, style: { fill: "currentColor", opacity: 0.78 } };
  const tooltip = <Tooltip content={tooltipGrafica(spec, formato, idioma)} cursor={{ fill: "rgba(124,135,148,0.18)" }} wrapperStyle={{ zIndex: 20, maxWidth: "90%" }} />;
  const leyenda = spec.series.length > 1 ? <Legend verticalAlign={compacto ? "bottom" : "top"} wrapperStyle={{ fontSize: fuente }} iconSize={compacto ? 8 : 12} /> : null;
  const colorSerie = (s, i) => colorDeSerie(spec, s, i);
  const extra = spec.extra;
  const textoExtra = (fila) => (extra && fila && fila[extra.clave] !== undefined && fila[extra.clave] !== null ? ` · ${numero(fila[extra.clave], idioma)}${extra.unidad === "%" ? " %" : ""}` : "");

  // ── dona ──
  if (vista === TIPOS.DONA) {
    const s = spec.series[0];
    const total = typeof spec.total_valor === "number" && spec.tipo === "pie" ? spec.total_valor : data.reduce((a, d) => a + (Number(d[s.clave]) || 0), 0);
    const porcion = (d) => (extra && d[extra.clave] !== undefined && d[extra.clave] !== null ? numero(d[extra.clave], idioma) : total > 0 ? numero(Math.round(((Number(d[s.clave]) || 0) / total) * 1000) / 10, idioma) : "0");
    const textoPorcion = (d) => `${formato(d[s.clave])} · ${porcion(d)} %`;
    const centro = ({ viewBox }) => {
      const cx = viewBox && viewBox.cx;
      const cy = viewBox && viewBox.cy;
      if (cx === undefined || cy === undefined) return null;
      return (
        <g className="asst-dona-centro">
          <text x={cx} y={cy - 2} textAnchor="middle" fontSize={compacto ? 16 : 24} fontWeight={700} style={{ fill: "currentColor" }}>{numero(total, idioma)}</text>
          <text x={cx} y={cy + (compacto ? 12 : 18)} textAnchor="middle" fontSize={compacto ? 10 : 12} style={{ fill: "currentColor", opacity: 0.7 }}>{t ? t.total : "Total"}</text>
        </g>
      );
    };
    const etiquetaFuera = ({ cx, cy, midAngle, outerRadius, index }) => {
      const rad = Math.PI / 180;
      const r = outerRadius + 16;
      const x = cx + r * Math.cos(-midAngle * rad);
      const y = cy + r * Math.sin(-midAngle * rad);
      return <text x={x} y={y} textAnchor={x > cx ? "start" : "end"} dominantBaseline="central" fontSize={12} style={{ fill: "currentColor" }}>{textoPorcion(data[index])}</text>;
    };
    // Vista previa: la leyenda (con valor y % de cada porción) es una lista HTML al lado de la dona; Recharts no reserva bien el ancho de una leyenda lateral y recortaba el anillo.
    const leyendaLateral = compacto && (
      <ul className="asst-dona-leyenda" aria-label={spec.titulo}>
        {data.map((d, i) => (
          <li key={d[claveX]}><span className="asst-dona-punto" style={{ background: colorDeCategoria(spec, d[claveX], i) }} />{d[claveX]}: <b>{textoPorcion(d)}</b></li>
        ))}
      </ul>
    );
    return {
      alto: compacto ? ALTO_VISTA_PREVIA : 400, vertical: false, tipoVista: vista, leyenda: leyendaLateral || undefined,
      cuerpo: (
        <PieChart>
          <Pie data={data} dataKey={s.clave} nameKey={claveX} cx="50%" cy={compacto ? "50%" : "46%"} innerRadius={compacto ? 46 : 78} outerRadius={compacto ? 72 : 124}
            paddingAngle={2} stroke="none" isAnimationActive={false} label={compacto ? false : etiquetaFuera} labelLine={!compacto}>
            {data.map((d, i) => <Cell key={i} fill={colorDeCategoria(spec, d[claveX], i)} />)}
            <Label position="center" content={centro} />
          </Pie>
          <Tooltip content={tooltipGrafica(spec, formato, idioma)} wrapperStyle={{ zIndex: 20, maxWidth: "90%" }} />
          {!compacto && (
            <Legend layout="horizontal" align="center" verticalAlign="bottom" wrapperStyle={{ fontSize: fuente }} iconSize={12}
              formatter={(valor) => { const d = data.find((x) => x[claveX] === valor); return <span style={{ color: "inherit" }}>{valor}{d ? `: ${textoPorcion(d)}` : ""}</span>; }} />
          )}
        </PieChart>
      ),
    };
  }

  // ── línea ──
  if (vista === TIPOS.LINEA) {
    return {
      alto: compacto ? ALTO_VISTA_PREVIA : altoLinea, vertical: false, tipoVista: vista,
      cuerpo: (
        <LineChart data={data} margin={{ top: 10, right: 18, left: 0, bottom: 4 }}>
          <CartesianGrid strokeDasharray="3 3" stroke="rgba(124,135,148,0.4)" />
          <XAxis dataKey={claveX} tick={eje} tickFormatter={(v) => recortar(v, compacto ? 8 : 14)} interval={compacto && data.length > 6 ? "preserveStartEnd" : 0} />
          <YAxis tick={eje} tickFormatter={esPct ? (v) => `${v}%` : undefined} width={compacto ? 40 : 56} />
          {tooltip}{leyenda}
          {spec.series.map((s, i) => <Line key={s.clave} type="monotone" dataKey={s.clave} name={s.nombre || s.clave} stroke={colorSerie(s, i)} strokeWidth={2} dot={{ r: compacto ? 2 : 3 }} connectNulls={false} isAnimationActive={false} />)}
        </LineChart>
      ),
    };
  }

  // ── barras (simples o agrupadas) ──
  // Layout: el de la especificación; una línea o una dona vistas como barras: columnas para el tiempo, barras horizontales para nombres.
  const vertical = (spec.layout || (spec.tipo === "line" ? "horizontal" : "vertical")) === "vertical"; // barras horizontales: categorías en el eje Y
  const agrupada = spec.series.length > 1;
  const dominio = [0, "auto"]; // el valor va fuera del área de trazado (margen), así los ejes quedan en números redondos
  // Etiqueta del valor al final de cada barra: en las agrupadas, en TODAS en la ampliada y solo en la última (con el % extra) en la vista previa
  const etiqueta = (i, ultima) => {
    if (!(!agrupada || !compacto || ultima)) return null;
    const contenido = ({ x, y, width, height, value, index }) => {
      if (value === null || value === undefined) return null;
      const texto = `${formato(value)}${ultima ? textoExtra(data[index]) : ""}`;
      return vertical
        ? <text x={x + width + 4} y={y + height / 2} dominantBaseline="central" fontSize={fuente} style={{ fill: "currentColor" }}>{texto}</text>
        : <text x={x + width / 2} y={y - 5} textAnchor="middle" fontSize={fuente} style={{ fill: "currentColor" }}>{texto}</text>;
    };
    return <LabelList dataKey={spec.series[i].clave} content={contenido} />;
  };
  const barras = spec.series.map((s, i) => (
    <Bar key={s.clave} dataKey={s.clave} name={s.nombre || s.clave} fill={colorSerie(s, i)} isAnimationActive={false}
      maxBarSize={compacto ? (agrupada ? 9 : 16) : (agrupada ? 18 : 26)}>
      {etiqueta(i, i === spec.series.length - 1)}
    </Bar>
  ));
  const margenDerecho = (compacto ? 34 : 56) + (extra && vertical ? (compacto ? 44 : 64) : 0);
  if (vertical) {
    const alto = compacto ? ALTO_VISTA_PREVIA : Math.max(220, data.length * altoBarra * spec.series.length + 64 + (agrupada ? data.length * 6 : 0));
    return {
      alto, vertical, tipoVista: vista,
      cuerpo: (
        <BarChart data={data} layout="vertical" margin={{ top: 4, right: margenDerecho, left: 0, bottom: compacto ? 0 : 4 }} barCategoryGap={compacto ? "14%" : "22%"} barGap={agrupada ? 1 : 4}>
          {!compacto && <CartesianGrid strokeDasharray="3 3" horizontal={false} stroke="rgba(124,135,148,0.4)" />}
          <XAxis type="number" hide={compacto} domain={dominio} tick={eje} tickFormatter={esPct ? (v) => `${v}%` : undefined} />
          <YAxis type="category" dataKey={claveX} tick={tickNombre(compacto ? 14 : Math.max(18, Math.floor(anchoEje / 6.3)), fuente)} width={compacto ? 100 : anchoEje} interval={0} />
          {tooltip}{leyenda}
          {barras}
        </BarChart>
      ),
    };
  }
  const muchas = data.length > (compacto ? 4 : 8);
  return {
    alto: compacto ? ALTO_VISTA_PREVIA : 380, vertical, tipoVista: vista,
    cuerpo: (
      <BarChart data={data} layout="horizontal" margin={{ top: 22, right: 12, left: 0, bottom: 4 }} barGap={agrupada ? 1 : 4}>
        <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="rgba(124,135,148,0.4)" />
        <XAxis dataKey={claveX} tick={eje} tickFormatter={(v) => recortar(v, compacto ? 9 : 16)} interval={0} angle={muchas ? -35 : 0} textAnchor={muchas ? "end" : "middle"} height={muchas ? (compacto ? 46 : 70) : 28} />
        <YAxis tick={eje} domain={dominio} tickFormatter={esPct ? (v) => `${v}%` : undefined} width={compacto ? 40 : 52} />
        {tooltip}{leyenda}
        {barras}
      </BarChart>
    ),
  };
}
