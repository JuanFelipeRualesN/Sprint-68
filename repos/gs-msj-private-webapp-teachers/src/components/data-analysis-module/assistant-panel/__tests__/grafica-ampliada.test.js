import React from "react";
import ReactDOM from "react-dom";
import { act } from "react-dom/test-utils";
import Grafica from "../grafica";
import { anchoDeEje } from "../grafica-ampliada";
import { datosVistaPrevia, totalesDe, construirGrafica } from "../grafica-base";
import { textosPara } from "../textos";
import { ASSISTANT_URL } from "../config";

// La sesión (header Token) sale de getAuthHeaders(); se simula para no cargar axios/SweetAlert/imágenes.
jest.mock("../../../../api/interceptors", () => ({ getAuthHeaders: () => ({ Token: "tok-sesion", "Accept-Language": "es" }) }));

const t = textosPara("es");
const categoria = (i) => `Escuela de prueba número ${String(i + 1).padStart(2, "0")}`;
const filas = (n, ceros = 0) => Array.from({ length: n }, (_, i) => ({ categoria: categoria(i), conexiones: i < n - ceros ? (n - i) * 10 : 0 }));
const specBarras = (extra = {}) => ({
  consulta: "conexiones_por_escuela", tipo: "bar", layout: "vertical", titulo: "Conexiones por escuela",
  eje_x: { clave: "categoria", titulo: "Escuela" }, eje_y: { titulo: "Conexiones", unidad: "conexiones" },
  series: [{ clave: "conexiones", nombre: "Conexiones" }], data: filas(10), filas: 10, total_categorias: 47, categorias_mostradas: 10, categorias_en_cero: 0,
  orden: "desc", incluir_ceros: false, todas: false, hay_mas_filas: false, id: "abc", url: "/asistente/graficas/abc", ...extra,
});
const specLineas = () => ({
  consulta: "tendencia_conexiones", tipo: "line", titulo: "Conexiones por mes", eje_x: { clave: "categoria", titulo: "Período" }, eje_y: { titulo: "Conexiones", unidad: "conexiones" },
  series: [{ clave: "conexiones", nombre: "Conexiones" }], data: [{ categoria: "2026-01", conexiones: 1 }, { categoria: "2026-02", conexiones: 2 }],
  filas: 2, total_categorias: 2, categorias_mostradas: 2, hay_mas_filas: false, id: "lin", url: "/asistente/graficas/lin",
});

let contenedor;
let fetchMock;
beforeEach(() => {
  global.ResizeObserver = class { observe() {} unobserve() {} disconnect() {} };   // jsdom no lo trae (lo usa ResponsiveContainer de Recharts)
  contenedor = document.createElement("div");
  document.body.appendChild(contenedor);
  fetchMock = jest.fn(async () => ({ ok: true, status: 200, headers: { get: () => null }, json: async () => specBarras({ data: filas(47), categorias_mostradas: 47, todas: true }) }));
  global.fetch = fetchMock;
});
afterEach(() => {
  act(() => { ReactDOM.unmountComponentAtNode(contenedor); });
  contenedor.remove();
  delete global.fetch;
});

const montar = (spec, descarga) => act(() => { ReactDOM.render(<Grafica spec={spec} t={t} idioma="es" descarga={descarga} />, contenedor); });
const clic = (el) => act(() => { el.dispatchEvent(new MouseEvent("click", { bubbles: true, cancelable: true })); });
const tecla = (el, key, extra = {}) => act(() => { el.dispatchEvent(new KeyboardEvent("keydown", { key, bubbles: true, cancelable: true, ...extra })); });
const dialogo = () => document.body.querySelector('[role="dialog"]');
const botonCon = (texto, raiz = document.body) => Array.from(raiz.querySelectorAll("button")).find((b) => b.textContent.trim() === texto);
const esperar = () => act(async () => { await Promise.resolve(); await Promise.resolve(); });

describe("vista previa compacta", () => {
  test("47 categorías: muestra 10, «10 de 47» y «Ver todas»; la altura es fija (200 px) y hay botón Ampliar", () => {
    montar(specBarras());
    expect(contenedor.querySelector(".asst-grafica-nota").textContent).toContain("10 de 47");
    expect(botonCon("Ver todas", contenedor)).toBeTruthy();
    expect(contenedor.querySelector(".asst-grafica-lienzo").style.height).toBe("200px");
    const ampliar = contenedor.querySelector(".asst-grafica-ampliar");
    expect(ampliar.getAttribute("aria-label")).toContain(t.ampliar);
    expect(contenedor.querySelector('[role="img"]').getAttribute("aria-label")).toContain("10");
  });
  test("sin categorías de más no hay nota ni «Ver todas»; las en 0 ocultas se avisan", () => {
    montar(specBarras({ total_categorias: 10, categorias_en_cero: 0 }));
    expect(contenedor.querySelector(".asst-grafica-nota")).toBeNull();
    montar(specBarras({ total_categorias: 15, categorias_en_cero: 32 }));
    expect(contenedor.querySelector(".asst-grafica-nota").textContent).toContain("10 de 15 · 32 sin datos ocultas");
  });
  test("un servicio antiguo (sin total_categorias y con 30 filas) se recorta a 10 en la vista previa", () => {
    const antiguo = specBarras({ data: filas(30), total_categorias: undefined, categorias_mostradas: undefined });
    expect(datosVistaPrevia(antiguo)).toHaveLength(10);
    expect(totalesDe(antiguo, datosVistaPrevia(antiguo))).toMatchObject({ mostradas: 10, total: 10 });
    expect(construirGrafica(antiguo, datosVistaPrevia(antiguo), { t, idioma: "es", compacto: true }).alto).toBe(200);
  });
  test("las líneas no se recortan ni piden «ver todas»", () => {
    montar(specLineas());
    expect(contenedor.querySelector(".asst-grafica-nota")).toBeNull();
    expect(contenedor.querySelector(".asst-grafica-lienzo").style.height).toBe("200px");
  });
});

describe("ventana ampliada", () => {
  test("abre con Ampliar: role=dialog, aria-modal, título, foco en ✕; se cierra con ✕, Esc y clic fuera y devuelve el foco", () => {
    montar(specBarras());
    const ampliar = contenedor.querySelector(".asst-grafica-ampliar");
    ampliar.focus();
    clic(ampliar);
    const d = dialogo();
    expect(d).toBeTruthy();
    expect(d.getAttribute("aria-modal")).toBe("true");
    expect(document.getElementById(d.getAttribute("aria-labelledby")).textContent).toBe("Conexiones por escuela");
    expect(document.activeElement).toBe(d.querySelector(".asst-modal-cerrar"));
    expect(document.body.style.overflow).toBe("hidden");
    clic(d.querySelector(".asst-modal-cerrar"));
    expect(dialogo()).toBeNull();
    expect(document.activeElement).toBe(ampliar);
    expect(document.body.style.overflow).toBe("");

    clic(ampliar);
    tecla(document.body, "Escape");
    expect(dialogo()).toBeNull();

    clic(ampliar);
    act(() => { document.querySelector(".asst-modal-fondo").dispatchEvent(new MouseEvent("mousedown", { bubbles: true })); });
    expect(dialogo()).toBeNull();
    clic(ampliar);
    act(() => { dialogo().dispatchEvent(new MouseEvent("mousedown", { bubbles: true })); });
    expect(dialogo()).toBeTruthy();   // un clic DENTRO de la ventana no la cierra
  });

  test("foco atrapado: Tab desde el último elemento vuelve al primero y Shift+Tab desde el primero va al último", () => {
    montar(specBarras(), { id: "abc", url: "/asistente/descargas/abc", nombre: "x.xlsx" });
    clic(contenedor.querySelector(".asst-grafica-ampliar"));
    const d = dialogo();
    const lista = Array.from(d.querySelectorAll("button:not(:disabled), input:not(:disabled)"));
    const primero = lista[0];
    const ultimo = lista[lista.length - 1];
    ultimo.focus();
    tecla(ultimo, "Tab");
    expect(document.activeElement).toBe(primero);
    tecla(primero, "Tab", { shiftKey: true });
    expect(document.activeElement).toBe(ultimo);
  });

  test("«Ver todas» abre directamente en «Todas»: pide el servicio con Token y muestra 47 de 47 con barras de altura fija (sin aplastarse)", async () => {
    montar(specBarras());
    clic(botonCon("Ver todas", contenedor));
    await esperar();
    expect(fetchMock).toHaveBeenCalledTimes(1);
    const [url, opciones] = fetchMock.mock.calls[0];
    expect(url).toBe(`${ASSISTANT_URL}/asistente/graficas/abc?todas=true&orden=desc&incluir_ceros=false`);
    expect(opciones.headers.Token).toBe("tok-sesion");
    const d = dialogo();
    expect(d.querySelector(".asst-modal-cuenta").textContent).toContain("47 de 47");
    expect(botonCon("Todas", d).getAttribute("aria-pressed")).toBe("true");
    // 47 categorías × 28 px + márgenes: la ventana hace scroll, las barras no se achican
    expect(parseInt(d.querySelector(".asst-modal-lienzo").style.height, 10)).toBeGreaterThanOrEqual(47 * 28);
  });

  test("Top 10 ↔ Todas, orden ↓/↑ y «Ocultar 0» piden la vista al servicio; volver a la vista inicial no vuelve a pedir", async () => {
    montar(specBarras());
    clic(contenedor.querySelector(".asst-grafica-ampliar"));
    await esperar();
    expect(fetchMock).not.toHaveBeenCalled();                         // la vista inicial es la del chat
    const d = dialogo();
    expect(botonCon("Top 10", d).getAttribute("aria-pressed")).toBe("true");
    clic(botonCon("Todas", d)); await esperar();
    expect(fetchMock.mock.calls[0][0]).toContain("todas=true&orden=desc&incluir_ceros=false");
    clic(d.querySelector(".asst-modal-orden")); await esperar();
    expect(fetchMock.mock.calls[1][0]).toContain("todas=true&orden=asc&incluir_ceros=false");
    expect(d.querySelector(".asst-modal-orden").textContent).toContain(t.ordenAsc);
    clic(d.querySelector(".asst-modal-ceros input")); await esperar();
    expect(fetchMock.mock.calls[2][0]).toContain("todas=true&orden=asc&incluir_ceros=true");
    clic(d.querySelector(".asst-modal-ceros input")); clic(d.querySelector(".asst-modal-orden")); await esperar();
    clic(botonCon("Top 10", d)); await esperar();
    const antes = fetchMock.mock.calls.length;
    expect(botonCon("Top 10", d).getAttribute("aria-pressed")).toBe("true");
    expect(d.querySelector(".asst-modal-cuenta").textContent).toContain("10 de 47");   // volvió a la vista del chat sin pedirla otra vez
    expect(fetchMock.mock.calls.length).toBe(antes);
  });

  test("un error del servicio (sesión vencida) se muestra con el texto de sesión y la ventana sigue usable", async () => {
    fetchMock.mockImplementation(async () => ({ ok: false, status: 401, headers: { get: () => null }, json: async () => ({ error: "no_autorizado", mensaje: "x" }) }));
    montar(specBarras());
    clic(contenedor.querySelector(".asst-grafica-ampliar"));
    clic(botonCon("Todas", dialogo())); await esperar();
    expect(dialogo().querySelector('[role="alert"]').textContent).toBe(t.errSesion);
  });

  test("las líneas también se amplían, sin «Top 10 / Todas» ni orden, con Descargar imagen", () => {
    montar(specLineas(), { id: "lin", url: "/asistente/descargas/lin", nombre: "x.xlsx" });
    clic(contenedor.querySelector(".asst-grafica-ampliar"));
    const d = dialogo();
    expect(botonCon("Top 10", d)).toBeUndefined();
    expect(botonCon("Todas", d)).toBeUndefined();
    expect(d.querySelector(".asst-modal-orden")).toBeNull();
    expect(botonCon(t.descargarImagen, d)).toBeTruthy();
    expect(botonCon(t.descargarExcel, d)).toBeTruthy();
  });

  test("sin descarga asociada no hay botón de Excel; el eje de nombres se ajusta al nombre más largo con tope", () => {
    montar(specBarras());
    clic(contenedor.querySelector(".asst-grafica-ampliar"));
    expect(botonCon(t.descargarExcel, dialogo())).toBeUndefined();
    expect(anchoDeEje([{ categoria: "Ana" }], "categoria")).toBe(96);
    expect(anchoDeEje([{ categoria: "x".repeat(200) }], "categoria")).toBe(300);
    const medio = anchoDeEje([{ categoria: "x".repeat(30) }], "categoria");
    expect(medio).toBeGreaterThan(96);
    expect(medio).toBeLessThan(300);
  });
});

describe("textos", () => {
  test("los textos de la ventana existen en ES y EN", () => {
    for (const idioma of ["es", "en"]) {
      const x = textosPara(idioma);
      ["ampliar", "cerrarVentana", "top10", "todasLasCategorias", "ordenDesc", "ordenAsc", "ocultarCeros", "descargarImagen", "verTodas"].forEach((k) => expect(typeof x[k]).toBe("string"));
      expect(x.nDeM(10, 47)).toMatch(/10.*47/);
    }
    expect(textosPara("en").nDeM(10, 47)).toBe("10 of 47");
    expect(textosPara("es").ocultarCeros).toBe("Ocultar 0");
  });
});
