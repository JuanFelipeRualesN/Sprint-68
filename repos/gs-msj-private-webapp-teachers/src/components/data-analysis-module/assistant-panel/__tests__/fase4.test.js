import React from "react";
import { renderToStaticMarkup } from "react-dom/server";
import { leerAdjuntos, descargarArchivo, descargarExportacion, preguntar } from "../assistant-api";
import { mensajeDeError } from "../index";
import { textosPara } from "../textos";
import { ASSISTANT_URL } from "../config";
import Grafica from "../grafica";

// A5: la sesión de la plataforma (header Token) sale de getAuthHeaders(); aquí se simula para no cargar axios/SweetAlert/imágenes.
jest.mock("../../../../api/interceptors", () => ({ getAuthHeaders: () => ({ Token: "tok-sesion", "Accept-Language": "es" }) }));

const spec = (extra = {}) => ({
  consulta: "tendencia_conexiones", tipo: "line", titulo: "Conexiones por mes",
  eje_x: { clave: "categoria", titulo: "Período" }, eje_y: { titulo: "Conexiones", unidad: "conexiones" },
  series: [{ clave: "conexiones", nombre: "Conexiones" }],
  data: [{ categoria: "2026-01", conexiones: 12 }, { categoria: "2026-02", conexiones: 30 }],
  hay_mas_filas: false, ...extra,
});

describe("leerAdjuntos", () => {
  test("sin campos devuelve listas vacías", () => {
    expect(leerAdjuntos({ respuesta: "x" })).toEqual({ descargas: [], graficas: [] });
    expect(leerAdjuntos(null)).toEqual({ descargas: [], graficas: [] });
  });
  test("descarta elementos incompletos", () => {
    const d = { descargas: [{ id: "a", url: "/asistente/descargas/a" }, { id: "b" }, null], graficas: [spec(), { data: [] }, { data: [{}], series: [] }] };
    const r = leerAdjuntos(d);
    expect(r.descargas).toHaveLength(1);
    expect(r.graficas).toHaveLength(1);
  });
});

describe("descarga", () => {
  const original = { fetch: global.fetch, crear: URL.createObjectURL, revocar: URL.revokeObjectURL };
  let nombres;
  beforeEach(() => {
    nombres = [];
    URL.createObjectURL = jest.fn(() => "blob:x");
    URL.revokeObjectURL = jest.fn();
    jest.spyOn(HTMLAnchorElement.prototype, "click").mockImplementation(function () { nombres.push(this.download); });
  });
  afterEach(() => { global.fetch = original.fetch; URL.createObjectURL = original.crear; URL.revokeObjectURL = original.revocar; jest.restoreAllMocks(); });

  const respuesta = (estado, cuerpo, cabeceras = {}) => ({
    ok: estado >= 200 && estado < 300, status: estado,
    headers: { get: (k) => cabeceras[k.toLowerCase()] || null },
    blob: async () => new Blob([cuerpo]), json: async () => JSON.parse(cuerpo),
  });

  test("usa el nombre de content-disposition", async () => {
    global.fetch = jest.fn(async () => respuesta(200, "x", { "content-disposition": 'attachment; filename="tendencia_2026.xlsx"' }));
    expect(await descargarArchivo("/asistente/descargas/abc", "respaldo.xlsx")).toBe("tendencia_2026.xlsx");
    expect(global.fetch.mock.calls[0][0]).toMatch(/\/asistente\/descargas\/abc$/);
    expect(nombres).toEqual(["tendencia_2026.xlsx"]);
  });
  test("nombre UTF-8 y respaldo", async () => {
    global.fetch = jest.fn(async () => respuesta(200, "x", { "content-disposition": "attachment; filename*=UTF-8''a%C3%B1o.xlsx" }));
    expect(await descargarArchivo("/x", "r.xlsx")).toBe("año.xlsx");
    global.fetch = jest.fn(async () => respuesta(200, "x"));
    expect(await descargarArchivo("/x", "r.xlsx")).toBe("r.xlsx");
  });
  test("404 = descarga_expirada", async () => {
    global.fetch = jest.fn(async () => respuesta(404, '{"error":"descarga_expirada"}'));
    await expect(descargarArchivo("/x")).rejects.toMatchObject({ codigo: "descarga_expirada", estado: 404 });
  });
  test("400 conserva el error del servicio; red caída = sin_conexion", async () => {
    global.fetch = jest.fn(async () => respuesta(400, '{"error":"parametros_invalidos","mensaje":"fecha inválida"}'));
    await expect(descargarArchivo("/x")).rejects.toMatchObject({ codigo: "parametros_invalidos", message: "fecha inválida" });
    global.fetch = jest.fn(async () => { throw new TypeError("fail"); });
    await expect(descargarArchivo("/x")).rejects.toMatchObject({ codigo: "sin_conexion" });
  });
  test("exportación directa arma /exportar/{consulta}?formato=xlsx&desde&hasta", async () => {
    global.fetch = jest.fn(async () => respuesta(200, "x"));
    await descargarExportacion("conexiones-por-maestro", { desde: "2026-01-01", hasta: "2026-11-10", escuela: "" });
    const url = global.fetch.mock.calls[0][0];
    expect(url).toMatch(/\/exportar\/conexiones-por-maestro\?formato=xlsx&desde=2026-01-01&hasta=2026-11-10$/);
  });
});

describe("textos", () => {
  test("es y en tienen las mismas claves", () => {
    const es = textosPara("es"), en = textosPara("en");
    expect(Object.keys(en).sort()).toEqual(Object.keys(es).sort());
    expect(Object.keys(en.consultas).sort()).toEqual(Object.keys(es.consultas).sort());
  });
});

describe("Grafica", () => {
  const t = textosPara("es");
  test.each([
    ["line", {}], ["bar", { layout: "horizontal" }], ["bar", { layout: "vertical", eje_y: { unidad: "%" } }], ["pie", {}],
  ])("renderiza %s sin fallar y con título accesible", (tipo, extra) => {
    const html = renderToStaticMarkup(<Grafica spec={spec({ tipo, ...extra })} t={t} idioma="es" />);
    expect(html).toContain("Conexiones por mes");
    expect(html).toContain('role="img"');
  });
  test("avisa si hay más filas", () => {
    expect(renderToStaticMarkup(<Grafica spec={spec({ hay_mas_filas: true })} t={t} idioma="es" />)).toContain(t.hayMasFilas);
    expect(renderToStaticMarkup(<Grafica spec={spec()} t={t} idioma="es" />)).not.toContain(t.hayMasFilas);
  });
});

describe("sesión (Fase 4 · A5)", () => {
  const original = global.fetch;
  afterEach(() => { global.fetch = original; jest.restoreAllMocks(); });
  const resp = (estado, cuerpo) => ({
    ok: estado >= 200 && estado < 300, status: estado, headers: { get: () => null },
    blob: async () => new Blob(["x"]), json: async () => JSON.parse(cuerpo),
  });
  const cabeceras = () => global.fetch.mock.calls[0][1].headers;

  test("preguntar manda Token y Accept-Language, sin x-api-key", async () => {
    global.fetch = jest.fn(async () => resp(200, '{"respuesta":"ok"}'));
    await preguntar({ pregunta: "hola" });
    expect(cabeceras()).toMatchObject({ "Content-Type": "application/json", Token: "tok-sesion", "Accept-Language": "es" });
    expect(Object.keys(cabeceras()).map((k) => k.toLowerCase())).not.toContain("x-api-key");
  });
  test("descargarArchivo y la exportación directa mandan Token", async () => {
    jest.spyOn(HTMLAnchorElement.prototype, "click").mockImplementation(() => {});
    URL.createObjectURL = jest.fn(() => "blob:x"); URL.revokeObjectURL = jest.fn();
    global.fetch = jest.fn(async () => resp(200, "x"));
    await descargarArchivo("/asistente/descargas/abc", "a.xlsx");
    expect(cabeceras().Token).toBe("tok-sesion");
    global.fetch = jest.fn(async () => resp(200, "x"));
    await descargarExportacion("uso-por-area", { desde: "2026-01-01", hasta: "2026-11-10" });
    expect(cabeceras().Token).toBe("tok-sesion");
  });
  test.each([
    [401, "no_autorizado", "errSesion"], [403, "sin_permiso", "errPermiso"], [503, "autenticacion_no_disponible", "errAutenticacion"],
  ])("HTTP %i (%s) se traduce al texto de la sesión en preguntar y en descargas", async (estado, codigo, texto) => {
    const t = textosPara("es");
    const cuerpo = JSON.stringify({ error: codigo, mensaje: "m" });
    global.fetch = jest.fn(async () => resp(estado, cuerpo));
    const e1 = await preguntar({ pregunta: "hola" }).catch((e) => e);
    expect(e1.codigo).toBe(codigo);
    expect(mensajeDeError(e1, t)).toBe(t[texto]);
    global.fetch = jest.fn(async () => resp(estado, cuerpo));
    const e2 = await descargarArchivo("/x").catch((e) => e);
    expect(e2.codigo).toBe(codigo);
    expect(mensajeDeError(e2, t)).toBe(t[texto]);
  });
  test("no manda el Token a una URL absoluta de otro origen; sí a una absoluta del propio servicio", async () => {
    global.fetch = jest.fn(async () => resp(200, "x"));
    const e = await descargarArchivo("https://otro-host.invalid/x").catch((err) => err);
    expect(e.codigo).toBe("url_no_permitida");
    expect(global.fetch).not.toHaveBeenCalled();
    jest.spyOn(HTMLAnchorElement.prototype, "click").mockImplementation(() => {});
    URL.createObjectURL = jest.fn(() => "blob:x"); URL.revokeObjectURL = jest.fn();
    await descargarArchivo(`${ASSISTANT_URL}/asistente/descargas/abc`, "a.xlsx");
    expect(cabeceras().Token).toBe("tok-sesion");
  });
  test("textos de sesión: 401 y 403 (sin código de la API) y en inglés", () => {
    const es = textosPara("es"), en = textosPara("en");
    expect(mensajeDeError({ codigo: "http_401" }, es)).toBe("Tu sesión expiró, vuelve a iniciar sesión.");
    expect(mensajeDeError({ codigo: "http_403" }, es)).toBe("No tienes permiso para estos datos.");
    expect(es.errAutenticacion).toBe("No se pudo validar tu sesión, intenta de nuevo.");
    expect(mensajeDeError({ codigo: "no_autorizado" }, en)).toBe(en.errSesion);
  });
});
