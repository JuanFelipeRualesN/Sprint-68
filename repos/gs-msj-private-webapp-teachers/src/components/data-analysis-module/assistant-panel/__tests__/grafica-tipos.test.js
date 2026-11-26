import React from "react";
import ReactDOM from "react-dom";
import { renderToStaticMarkup } from "react-dom/server";
import { act } from "react-dom/test-utils";
import { Bar, BarChart, PieChart, Label, Pie } from "recharts";
import Grafica from "../grafica";
import { construirGrafica, tiposPermitidos, tipoPorDefecto, TIPOS, MAX_DONA, colorDeCategoria } from "../grafica-base";
import { textosPara } from "../textos";

jest.mock("../../../../api/interceptors", () => ({ getAuthHeaders: () => ({ Token: "tok-sesion", "Accept-Language": "es" }) }));

const t = textosPara("es");

// ── especificaciones como las arma el servicio ──
const participacion = (n = 10) => ({
  consulta: "participacion_escuelas", tipo: "bar", layout: "vertical", agrupar: "grouped", titulo: "Usuarios activos y conectados por escuela",
  eje_x: { clave: "categoria", titulo: "Escuela" }, eje_y: { titulo: "Usuarios", unidad: "usuarios" },
  series: [{ clave: "usuarios_activos", nombre: "Usuarios activos" }, { clave: "usuarios_conectados", nombre: "Usuarios conectados" }],
  extra: { clave: "porcentaje_participacion", nombre: "% de participación", unidad: "%" },
  data: Array.from({ length: n }, (_, i) => ({ categoria: `Escuela ${i + 1}`, usuarios_activos: 20 + i, usuarios_conectados: 10 + i, porcentaje_participacion: 14.3 + i })),
  filas: n, total_categorias: 47, categorias_mostradas: n, categorias_en_cero: 0, orden: "desc", incluir_ceros: false, todas: false, hay_mas_filas: false, id: "p1", url: "/asistente/graficas/p1",
});
const dona = (extra = {}) => ({
  consulta: "usuarios_por_suscripcion", tipo: "pie", variante: "dona", titulo: "Usuarios por estado de suscripción",
  eje_x: { clave: "categoria", titulo: "Estado" }, eje_y: { titulo: "Usuarios", unidad: "usuarios" }, series: [{ clave: "usuarios", nombre: "Usuarios" }],
  extra: { clave: "porcentaje_del_total", nombre: "% del total", unidad: "%" }, total_valor: 137,
  data: [{ categoria: "Activas", usuarios: 110, porcentaje_del_total: 80.3 }, { categoria: "Inactivas", usuarios: 8, porcentaje_del_total: 5.8 },
    { categoria: "Canceladas", usuarios: 10, porcentaje_del_total: 7.3 }, { categoria: "Sin suscripción", usuarios: 9, porcentaje_del_total: 6.6 }],
  filas: 4, total_categorias: 4, categorias_mostradas: 4, categorias_en_cero: 0, incluir_ceros: false, hay_mas_filas: false, id: "d1", url: "/asistente/graficas/d1", ...extra,
});
const barras = (n, extra = {}) => ({
  consulta: "conexiones_por_escuela", tipo: "bar", layout: "vertical", titulo: "Conexiones por escuela", eje_x: { clave: "categoria", titulo: "Escuela" }, eje_y: { titulo: "Conexiones", unidad: "conexiones" },
  series: [{ clave: "conexiones", nombre: "Conexiones" }], data: Array.from({ length: n }, (_, i) => ({ categoria: `Escuela ${i + 1}`, conexiones: (n - i) * 10 })),
  filas: n, total_categorias: n, categorias_mostradas: n, categorias_en_cero: 0, orden: "desc", incluir_ceros: false, todas: false, hay_mas_filas: false, id: "b1", url: "/asistente/graficas/b1", ...extra,
});
const linea = (n = 6, series = 1) => ({
  consulta: "tendencia_conexiones", tipo: "line", serie_temporal: true, titulo: "Conexiones por mes", eje_x: { clave: "categoria", titulo: "Período" }, eje_y: { titulo: "Conexiones", unidad: "conexiones" },
  series: [{ clave: "conexiones", nombre: "Conexiones" }, { clave: "otra", nombre: "Otra" }].slice(0, series),
  data: Array.from({ length: n }, (_, i) => ({ categoria: `2026-${String(i + 1).padStart(2, "0")}`, conexiones: 10 + i, otra: 5 + i })),
  filas: n, total_categorias: n, categorias_mostradas: n, hay_mas_filas: false, id: "l1", url: "/asistente/graficas/l1",
});

// Recorre el árbol de elementos de React y devuelve los que cumplan el filtro (para inspeccionar lo que Recharts va a dibujar sin medir el DOM).
function elementos(nodo, filtro, acc = []) {
  if (Array.isArray(nodo)) { nodo.forEach((n) => elementos(n, filtro, acc)); return acc; }
  if (!nodo || typeof nodo !== "object" || !nodo.props) return acc;
  if (filtro(nodo)) acc.push(nodo);
  elementos(nodo.props.children, filtro, acc);
  return acc;
}
const tipoDe = (spec, extra = {}) => construirGrafica(spec, spec.data, { t, idioma: "es", compacto: true, ...extra });

describe("barras agrupadas", () => {
  test("2 series: una <Bar> por serie dentro de un BarChart, lado a lado, con leyenda y el % fuera de las series", () => {
    const { cuerpo } = tipoDe(participacion());
    expect(cuerpo.type).toBe(BarChart);
    const bars = elementos(cuerpo, (e) => e.type === Bar);
    expect(bars.map((b) => b.props.dataKey)).toEqual(["usuarios_activos", "usuarios_conectados"]);
    expect(bars[0].props.fill).not.toBe(bars[1].props.fill);
    expect(bars.some((b) => b.props.dataKey === "porcentaje_participacion")).toBe(false);       // el % está en otra escala: no es una tercera barra
    expect(elementos(cuerpo, (e) => e.type && e.type.displayName === "Legend")).toHaveLength(1);
    expect(cuerpo.props.layout).toBe("vertical");
    expect(construirGrafica(participacion(), participacion().data, { t, idioma: "es", compacto: true }).alto).toBe(200);
  });
  test("la etiqueta del valor de la última barra lleva el % de participación y el tooltip lo trae aparte", () => {
    const spec = participacion(3);
    const { cuerpo } = tipoDe(spec);
    const bars = elementos(cuerpo, (e) => e.type === Bar);
    const etiqueta = elementos(bars[1].props.children, (e) => e.props && typeof e.props.content === "function")[0];
    const nodo = etiqueta.props.content({ x: 10, y: 10, width: 40, height: 8, value: 11, index: 1 });
    const texto = renderToStaticMarkup(<svg>{nodo}</svg>);
    expect(texto).toContain("11 · 15,3 %");                  // valor · % de participación (como lo lee es-CO: coma decimal)
    expect(elementos(bars[0].props.children, (e) => e.props && typeof e.props.content === "function")).toHaveLength(0);   // en la vista previa solo la última serie lleva etiqueta
    const tooltip = elementos(cuerpo, (e) => e.props && typeof e.props.content === "function" && !e.props.dataKey && e.type.displayName === "Tooltip")[0];
    const html = renderToStaticMarkup(tooltip.props.content({ active: true, label: "Escuela 2", payload: [
      { name: "Usuarios activos", value: 21, color: "#3B82C4", dataKey: "usuarios_activos", payload: spec.data[1] },
      { name: "Usuarios conectados", value: 11, color: "#E8912D", dataKey: "usuarios_conectados", payload: spec.data[1] }] }));
    expect(html).toContain("Escuela 2");
    expect(html).toContain("% de participación: <b>15,3 %</b>");
  });
  test("la vista previa del chat mantiene el top 10 y la leyenda; «10 de 47» con las agrupadas", () => {
    const cont = document.createElement("div");
    document.body.appendChild(cont);
    global.ResizeObserver = class { observe() {} unobserve() {} disconnect() {} };
    act(() => { ReactDOM.render(<Grafica spec={participacion(14)} t={t} idioma="es" />, cont); });
    expect(cont.querySelector(".asst-grafica-nota").textContent).toContain("10 de 47");
    expect(cont.querySelector(".asst-grafica-lienzo").getAttribute("data-tipo")).toBe("barras");
    act(() => { ReactDOM.unmountComponentAtNode(cont); });
    cont.remove();
  });
});

describe("dona", () => {
  test("por defecto con ≤ 6 categorías y UNA medida: PieChart con anillo, un Cell por porción y colores por estado", () => {
    const spec = dona();
    expect(tipoPorDefecto(spec, spec.data)).toBe(TIPOS.DONA);
    const { cuerpo, tipoVista } = tipoDe(spec);
    expect(tipoVista).toBe("dona");
    expect(cuerpo.type).toBe(PieChart);
    const pie = elementos(cuerpo, (e) => e.type === Pie)[0];
    expect(pie.props.innerRadius).toBeGreaterThan(0);
    const celdas = elementos(pie, (e) => e.props && e.props.fill && e.type.displayName === "Cell").map((c) => c.props.fill);
    expect(celdas).toEqual(["#2FA37A", "#E8912D", "#D8505F", "#7C8794"]);                       // activa, inactiva, cancelada, sin suscripción
    expect(colorDeCategoria(spec, "Canceladas", 2)).toBe("#D8505F");
  });
  test("el total va en el centro (el del servicio) y cada porción muestra valor y % (leyenda en la vista previa)", () => {
    const spec = dona();
    const { cuerpo } = tipoDe(spec);
    const centro = elementos(cuerpo, (e) => e.type === Label)[0];
    const html = renderToStaticMarkup(<svg>{centro.props.content({ viewBox: { cx: 100, cy: 80 } })}</svg>);
    expect(html).toContain(">137<");
    expect(html).toContain(">Total<");
    const { leyenda } = tipoDe(spec);                                                             // vista previa: leyenda lateral con valor y % de cada porción
    const item = renderToStaticMarkup(leyenda);
    expect(item).toContain("Activas: <b>110 · 80,3 %</b>");
    expect(item).toContain("Sin suscripción: <b>9 · 6,6 %</b>");
    const ampliada = construirGrafica(spec, spec.data, { t, idioma: "es", compacto: false });      // ventana: etiquetas fuera de cada porción y leyenda de Recharts
    expect(ampliada.leyenda).toBeUndefined();
    // con tamaño explícito Recharts dibuja el SVG completo (sin medir el DOM): anillo + total en el centro
    const svg = renderToStaticMarkup(React.cloneElement(cuerpo, { width: 320, height: 200 }));
    expect(svg).toContain("recharts-pie-sector");
    expect(svg).toContain("137");
  });
  test("con más de 6 categorías vuelve a barras (por defecto) y la dona queda deshabilitada", () => {
    const muchas = dona({ data: Array.from({ length: MAX_DONA + 1 }, (_, i) => ({ categoria: `E${i}`, usuarios: 10 + i })), total_valor: 99 });
    expect(tipoPorDefecto(muchas, muchas.data)).toBe(TIPOS.BARRAS);
    expect(tiposPermitidos(muchas, muchas.data)[TIPOS.DONA]).toBe(false);
    const { cuerpo, tipoVista } = tipoDe(muchas);
    expect(tipoVista).toBe("barras");
    expect(cuerpo.type).toBe(BarChart);
    const justo6 = dona({ data: muchas.data.slice(0, MAX_DONA) });
    expect(tipoPorDefecto(justo6, justo6.data)).toBe(TIPOS.DONA);
    expect(tiposPermitidos(justo6, justo6.data)[TIPOS.DONA]).toBe(true);
  });
  test("una dona vista como barras usa barras horizontales (nombres en el eje) y las líneas no se ofrecen", () => {
    const spec = dona();
    const { cuerpo } = tipoDe(spec, { tipoVista: "barras" });
    expect(cuerpo.type).toBe(BarChart);
    expect(cuerpo.props.layout).toBe("vertical");
    expect(tiposPermitidos(spec, spec.data)[TIPOS.LINEA]).toBe(false);
  });
});

describe("selector de tipo: habilitado según los datos", () => {
  const permitidos = (spec) => tiposPermitidos(spec, spec.data);
  test("barras de 10 categorías, una medida: solo Barras", () => {
    expect(permitidos(barras(10))).toEqual({ barras: true, linea: false, dona: false });
  });
  test("barras de 5 categorías, una medida: Barras y Dona (Línea no: no es serie de tiempo)", () => {
    expect(permitidos(barras(5))).toEqual({ barras: true, linea: false, dona: true });
  });
  test("agrupadas (2 series): solo Barras, aunque sean pocas categorías", () => {
    expect(permitidos(participacion(4))).toEqual({ barras: true, linea: false, dona: false });
  });
  test("serie de tiempo de 6 meses con una medida: las tres; de 12 meses: Barras y Línea; con 2 medidas: Barras y Línea", () => {
    expect(permitidos(linea(6))).toEqual({ barras: true, linea: true, dona: true });
    expect(permitidos(linea(12))).toEqual({ barras: true, linea: true, dona: false });
    expect(permitidos(linea(6, 2))).toEqual({ barras: true, linea: true, dona: false });
  });

  // ── la ventana ampliada: botones habilitados/deshabilitados y cambio de tipo ──
  let cont;
  beforeEach(() => {
    global.ResizeObserver = class { observe() {} unobserve() {} disconnect() {} };
    cont = document.createElement("div");
    document.body.appendChild(cont);
    global.fetch = jest.fn();
  });
  afterEach(() => { act(() => { ReactDOM.unmountComponentAtNode(cont); }); cont.remove(); delete global.fetch; });
  const abrir = (spec) => {
    act(() => { ReactDOM.render(<Grafica spec={spec} t={t} idioma="es" />, cont); });
    act(() => { cont.querySelector(".asst-grafica-ampliar").dispatchEvent(new MouseEvent("click", { bubbles: true })); });
    return document.body.querySelector('[role="dialog"]');
  };
  const boton = (d, tipo) => d.querySelector(`.asst-segmentos [data-tipo="${tipo}"]`);
  const clic = (el) => act(() => { el.dispatchEvent(new MouseEvent("click", { bubbles: true })); });
  const lienzo = (d) => d.querySelector(".asst-modal-lienzo").getAttribute("data-tipo");

  test("barras de 10 categorías: Línea y Dona deshabilitadas con su motivo; Barras activa", () => {
    const d = abrir(barras(10));
    expect(boton(d, "barras").disabled).toBe(false);
    expect(boton(d, "linea").disabled).toBe(true);
    expect(boton(d, "dona").disabled).toBe(true);
    expect(boton(d, "linea").title).toBe(t.soloSerieTemporal);
    expect(boton(d, "dona").title).toBe(t.soloDona);
    expect(boton(d, "barras").getAttribute("aria-pressed")).toBe("true");
  });
  test("serie de tiempo de 6 meses: abre en Línea; pasar a Dona y a Barras cambia el gráfico sin pedir nada al servicio", () => {
    const d = abrir(linea(6));
    expect(lienzo(d)).toBe("linea");
    expect(boton(d, "linea").getAttribute("aria-pressed")).toBe("true");
    expect(boton(d, "dona").disabled).toBe(false);
    clic(boton(d, "dona"));
    expect(lienzo(d)).toBe("dona");
    expect(boton(d, "dona").getAttribute("aria-pressed")).toBe("true");
    clic(boton(d, "barras"));
    expect(lienzo(d)).toBe("barras");
    clic(boton(d, "linea"));
    expect(lienzo(d)).toBe("linea");
    expect(global.fetch).not.toHaveBeenCalled();
  });
  test("barras de 5 categorías: la Dona se habilita y se puede elegir; la Línea no", () => {
    const d = abrir(barras(5));
    expect(boton(d, "dona").disabled).toBe(false);
    expect(boton(d, "linea").disabled).toBe(true);
    clic(boton(d, "dona"));
    expect(lienzo(d)).toBe("dona");
  });
  test("una dona (≤ 6) abre en Dona; se puede pasar a Barras; la Línea queda deshabilitada", () => {
    const d = abrir(dona());
    expect(lienzo(d)).toBe("dona");
    expect(boton(d, "linea").disabled).toBe(true);
    clic(boton(d, "barras"));
    expect(lienzo(d)).toBe("barras");
  });
  test("agrupadas de 2 series: Dona deshabilitada con su motivo", () => {
    const d = abrir(participacion(4));
    expect(boton(d, "dona").disabled).toBe(true);
    expect(boton(d, "linea").disabled).toBe(true);
  });
  test("si lo elegido deja de ser posible con los datos nuevos (la dona con más de 6 categorías), vuelve al tipo de la especificación", async () => {
    const chico = barras(5);
    const grande = barras(8, { todas: true });
    global.fetch = jest.fn(async () => ({ ok: true, status: 200, headers: { get: () => null }, json: async () => grande }));
    const d = abrir(chico);
    clic(boton(d, "dona"));
    expect(lienzo(d)).toBe("dona");
    const todas = Array.from(d.querySelectorAll("button")).find((b) => b.textContent.trim() === "Todas");
    // «Todas» solo se ofrece con barras: se vuelve a Barras, se piden y el selector refleja 8 categorías (dona deshabilitada)
    clic(boton(d, "barras"));
    clic(Array.from(d.querySelectorAll("button")).find((b) => b.textContent.trim() === "Todas") || todas);
    await act(async () => { await Promise.resolve(); await Promise.resolve(); });
    expect(boton(d, "dona").disabled).toBe(true);
    expect(lienzo(d)).toBe("barras");
  });
});

describe("textos del selector", () => {
  test("ES y EN", () => {
    expect([t.tipoBarras, t.tipoLinea, t.tipoDona, t.tipoGrafica, t.total]).toEqual(["Barras", "Línea", "Dona", "Tipo de gráfica", "Total"]);
    const en = textosPara("en");
    expect([en.tipoBarras, en.tipoLinea, en.tipoDona, en.tipoGrafica, en.total]).toEqual(["Bars", "Line", "Donut", "Chart type", "Total"]);
    expect(en.soloDona).toMatch(/6/);
  });
});
