import React, { useCallback, useEffect, useRef, useState } from "react";
import ReactDOM from "react-dom";
import { ResponsiveContainer } from "recharts";
import { X, Download, Image as Imagen, ArrowDown, ArrowUp } from "lucide-react";
import { COLORES, construirGrafica } from "./grafica-base";
import { pedirGrafica, descargarExcel } from "./assistant-api";
import { descargarImagen } from "./imagen";
import { mensajeDeError } from "./errores";

/**
 * Ventana ampliada de una gráfica (Fase 4 · REQ-3741): ~80 % del viewport, nombres completos, «Top 10» / «Todas» (con «Todas» la ventana hace
 * scroll vertical y cada barra mantiene su altura: no se aplastan), orden ↓/↑, «Ocultar 0», Descargar Excel y Descargar imagen (PNG).
 * Se cierra con Esc, ✕ o clic fuera. Accesible: role="dialog" + aria-modal, foco atrapado y devuelto a quien la abrió.
 * Las vistas «Todas», el orden y los ceros se piden al servicio (`spec.url`, GET /asistente/graficas/{id}): él ordena, filtra y aplica el alcance de la persona.
 */
const MAX_EJE = 300; // tope del ancho del eje de nombres (px): más allá se recorta con «…» y el tooltip da el nombre completo
const FOCUSABLES = 'button:not(:disabled), input:not(:disabled), select:not(:disabled), textarea:not(:disabled), a[href], [tabindex]:not([tabindex="-1"])';

export const anchoDeEje = (data, claveX) => {
  const mayor = data.reduce((m, d) => Math.max(m, String(d[claveX] == null ? "" : d[claveX]).length), 6);
  return Math.min(MAX_EJE, Math.max(96, Math.round(mayor * 6.6) + 12));
};

const GraficaAmpliada = ({ spec, descarga, t, idioma, inicialTodas, onCerrar }) => {
  const puedePedir = spec.tipo === "bar" && !!spec.url;
  const ordenInicial = spec.orden === "asc" ? "asc" : "desc";
  const cerosInicial = spec.incluir_ceros === true;
  const [actual, setActual] = useState(spec);
  const [todas, setTodas] = useState(!!inicialTodas && puedePedir);
  const [orden, setOrden] = useState(ordenInicial);
  const [ocultarCeros, setOcultarCeros] = useState(!cerosInicial);
  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState("");
  const [bajando, setBajando] = useState("");
  const dialogoRef = useRef(null);
  const cerrarRef = useRef(null);
  const lienzoRef = useRef(null);
  const previoRef = useRef(typeof document !== "undefined" ? document.activeElement : null); // quien abrió la ventana: recupera el foco al cerrar
  const idTitulo = useRef(`asst-modal-${Math.random().toString(36).slice(2, 9)}`).current;
  const alCerrar = useRef(onCerrar);
  alCerrar.current = onCerrar;

  // --- accesibilidad: Esc, foco inicial, foco devuelto, sin scroll del fondo ---
  useEffect(() => {
    const previo = previoRef.current;
    const overflow = document.body.style.overflow;
    document.body.style.overflow = "hidden";
    if (cerrarRef.current) cerrarRef.current.focus();
    const alTeclear = (e) => {
      if (e.key === "Escape" || e.key === "Esc") { e.stopPropagation(); e.preventDefault(); alCerrar.current(); }
    };
    document.addEventListener("keydown", alTeclear, true);
    return () => {
      document.removeEventListener("keydown", alTeclear, true);
      document.body.style.overflow = overflow;
      if (previo && typeof previo.focus === "function" && document.contains(previo)) previo.focus();
    };
  }, []);

  const atraparTab = (e) => {
    if (e.key !== "Tab" || !dialogoRef.current) return;
    const lista = Array.from(dialogoRef.current.querySelectorAll(FOCUSABLES));
    if (lista.length === 0) { e.preventDefault(); return; }
    const primero = lista[0];
    const ultimo = lista[lista.length - 1];
    if (!dialogoRef.current.contains(document.activeElement)) { e.preventDefault(); primero.focus(); }
    else if (e.shiftKey && document.activeElement === primero) { e.preventDefault(); ultimo.focus(); }
    else if (!e.shiftKey && document.activeElement === ultimo) { e.preventDefault(); primero.focus(); }
  };

  // --- datos: la vista inicial es la del chat; cualquier cambio se pide al servicio (que ordena, filtra y aplica el alcance) ---
  const primera = useRef(true);
  useEffect(() => {
    if (!puedePedir) return undefined;
    const esLaInicial = !todas && orden === ordenInicial && ocultarCeros === !cerosInicial;
    if (primera.current) {
      primera.current = false;
      if (esLaInicial) return undefined;
    } else if (esLaInicial) {
      setActual(spec); setError(""); setCargando(false);
      return undefined;
    }
    const ctl = new AbortController();
    setCargando(true); setError("");
    pedirGrafica(spec.url, { todas, orden, incluirCeros: !ocultarCeros }, ctl.signal)
      .then((nueva) => { setActual(nueva); setCargando(false); })
      .catch((e) => { if (e && e.name === "AbortError") return; setError(mensajeDeError(e, t)); setCargando(false); });
    return () => ctl.abort();
  }, [puedePedir, spec, t, todas, orden, ocultarCeros, ordenInicial, cerosInicial]);

  const claveX = (actual.eje_x && actual.eje_x.clave) || "categoria";
  const data = actual.data;
  const anchoEje = anchoDeEje(data, claveX);
  const { cuerpo, alto, vertical } = construirGrafica(actual, data, { t, idioma, compacto: false, anchoEje, altoBarra: 28 });
  const muchasColumnas = actual.tipo === "bar" && !vertical && data.length > 12;
  const total = Math.max(typeof actual.total_categorias === "number" ? actual.total_categorias : data.length, data.length);
  const ceros = typeof actual.categorias_en_cero === "number" && actual.incluir_ceros === false ? actual.categorias_en_cero : 0;

  const bajarExcel = async () => {
    if (!descarga || bajando) return;
    setBajando("excel"); setError("");
    try { await descargarExcel(descarga); } catch (e) { setError(mensajeDeError(e, t)); }
    setBajando("");
  };
  const bajarImagen = async () => {
    if (bajando) return;
    setBajando("png"); setError("");
    try {
      await descargarImagen(lienzoRef.current, { titulo: actual.titulo, series: actual.series.map((s) => s.nombre || s.clave), colores: COLORES, consulta: actual.consulta });
    } catch (e) { setError(t.errImagen); }
    setBajando("");
  };
  const alClicFondo = useCallback((e) => { if (e.target === e.currentTarget) onCerrar(); }, [onCerrar]);

  const modal = (
    <div className="asst-modal-fondo" onMouseDown={alClicFondo} data-testid="asst-modal-fondo">
      <div className="asst-modal" role="dialog" aria-modal="true" aria-labelledby={idTitulo} ref={dialogoRef} onKeyDown={atraparTab}>
        <header className="asst-modal-cab">
          <h2 id={idTitulo} className="asst-modal-titulo">{actual.titulo}</h2>
          <button type="button" ref={cerrarRef} className="asst-modal-cerrar" onClick={onCerrar} aria-label={t.cerrarVentana} title={t.cerrarVentana}><X size={18} /></button>
        </header>

        {actual.tipo === "bar" && (
          <div className="asst-modal-controles">
            {puedePedir && (
              <>
                <div className="asst-segmentos" role="group" aria-label={t.vista}>
                  <button type="button" aria-pressed={!todas} className={!todas ? "activo" : ""} onClick={() => setTodas(false)}>{t.top10}</button>
                  <button type="button" aria-pressed={todas} className={todas ? "activo" : ""} onClick={() => setTodas(true)}>{t.todasLasCategorias}</button>
                </div>
                <button type="button" className="asst-modal-orden" onClick={() => setOrden(orden === "desc" ? "asc" : "desc")}
                  aria-label={`${t.orden}: ${orden === "desc" ? t.ordenDesc : t.ordenAsc}`} title={t.orden}>
                  {orden === "desc" ? <ArrowDown size={14} /> : <ArrowUp size={14} />} {orden === "desc" ? t.ordenDesc : t.ordenAsc}
                </button>
                <label className="asst-modal-ceros">
                  <input type="checkbox" checked={ocultarCeros} onChange={(e) => setOcultarCeros(e.target.checked)} /> {t.ocultarCeros}
                </label>
              </>
            )}
            <span className="asst-modal-cuenta" aria-live="polite">{t.nDeM(data.length, total)}{ceros > 0 ? ` · ${t.cerosOcultos(ceros)}` : ""}</span>
          </div>
        )}

        <div className="asst-modal-cuerpo" aria-busy={cargando}>
          {error && <div className="asst-advertencia" role="alert">{error}</div>}
          {cargando && <div className="asst-modal-cargando" role="status">{t.cargandoGrafica}</div>}
          <div ref={lienzoRef} className="asst-modal-lienzo" role="img" aria-label={`${actual.titulo}. ${data.length} ${t.puntos}`}
            style={{ height: alto, minWidth: muchasColumnas ? data.length * 46 : undefined, opacity: cargando ? 0.45 : 1 }}>
            <ResponsiveContainer width="100%" height="100%">{cuerpo}</ResponsiveContainer>
          </div>
        </div>

        <footer className="asst-modal-pie">
          {descarga && (
            <button type="button" className="asst-descarga" onClick={bajarExcel} disabled={bajando === "excel"}>
              <Download size={14} /> {bajando === "excel" ? t.descargando : t.descargarExcel}
            </button>
          )}
          <button type="button" className="asst-descarga asst-descarga--sec" onClick={bajarImagen} disabled={bajando === "png"}>
            <Imagen size={14} /> {t.descargarImagen}
          </button>
        </footer>
      </div>
    </div>
  );
  return ReactDOM.createPortal(modal, document.body);
};

export default GraficaAmpliada;
