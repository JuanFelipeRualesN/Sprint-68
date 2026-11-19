import React, { useState, useEffect, useRef, useCallback } from "react";
import { Sparkles, X, Send, RotateCcw, ChevronDown, ChevronUp, Info, TriangleAlert, Download } from "lucide-react";
import { preguntar, leerAdjuntos, descargarExcel } from "./assistant-api";
import Grafica from "./grafica";
import ExportarDirecto from "./exportar-directo";
import { textosPara } from "./textos";
import { mensajeDeError } from "./errores";

export { mensajeDeError };
import "./style.css";

/**
 * Panel del asistente de datos junto a los tableros de Power BI (REQ-3741).
 *
 * Props:
 *  - report:          instancia del informe embebido de powerbi-client (o null). Solo se lee la página activa
 *                     para orientar al asistente; NUNCA se usa como dato ni como período.
 *  - contextoReporte: nombre del informe que se está viendo (texto).
 *  - contextoPagina:  nombre de la página que se está viendo (texto, se usa si Power BI no entrega el suyo).
 *  - idioma:          "es" | "en" (idioma activo de la plataforma).
 *  - onToggle(abierto): avisa al contenedor para hacer espacio al panel.
 *
 * Las referencias (período, fecha filtrada, escuela, datos hasta, filas) las pone el SERVICIO a partir de lo que
 * devolvió la API, no el modelo; aquí solo se muestran.
 */

// ---------- utilidades de presentación ----------
function Negrita({ texto }) {
  return texto.split(/(\*\*[^*]+\*\*)/g).map((p, i) =>
    p.startsWith("**") && p.endsWith("**") && p.length > 4 ? <strong key={i}>{p.slice(2, -2)}</strong> : <React.Fragment key={i}>{p}</React.Fragment>
  );
}

function TextoRespuesta({ texto }) {
  const bloques = [];
  let lista = null;
  (texto || "").split("\n").forEach((linea, i) => {
    const m = linea.match(/^\s*(\d+[.)]|[-•*])\s+(.*)$/);
    if (m) {
      if (!lista) {
        lista = { tipo: "lista", ordenada: /\d/.test(m[1]), items: [], key: i };
        bloques.push(lista);
      }
      lista.items.push(m[2]);
    } else {
      lista = null;
      if (linea.trim()) bloques.push({ tipo: "p", texto: linea, key: i });
    }
  });
  return bloques.map((b) => {
    if (b.tipo === "p") return <p key={b.key}><Negrita texto={b.texto} /></p>;
    const Tag = b.ordenada ? "ol" : "ul";
    return <Tag key={b.key}>{b.items.map((it, j) => <li key={j}><Negrita texto={it} /></li>)}</Tag>;
  });
}

function fecha(iso, idioma) {
  const m = /^(\d{4})-(\d{2})-(\d{2})/.exec(iso || "");
  if (!m) return iso || "";
  return new Date(+m[1], +m[2] - 1, +m[3]).toLocaleDateString(idioma === "en" ? "en-US" : "es-CO", { day: "2-digit", month: "short", year: "numeric" });
}

function textoPeriodo(p, idioma) {
  if (!p) return "";
  if (typeof p === "string") return p;
  if (p.desde) return `${fecha(p.desde, idioma)} → ${fecha(p.hasta, idioma)}`;
  if (p.a && p.b) return `A: ${fecha(p.a.desde, idioma)} → ${fecha(p.a.hasta, idioma)} · B: ${fecha(p.b.desde, idioma)} → ${fecha(p.b.hasta, idioma)}`;
  return "";
}

function chipsDe(ref, t, idioma) {
  const f = ref.filtros || {};
  const chips = [];
  const per = textoPeriodo(ref.periodo, idioma);
  if (per) chips.push([t.periodo, per]);
  if (ref.columna_fecha && ref.columna_fecha !== "no aplica") chips.push([t.fecha, ref.columna_fecha]);
  if (f.escuela) chips.push([t.escuela, f.escuela === "todas" ? t.todas : f.escuela]);
  if (f.cuentas_de_prueba) chips.push([t.cuentas, f.cuentas_de_prueba === "excluidas" ? t.excluidas : t.incluidas]);
  if (ref.datos_hasta) chips.push([t.datosHasta, fecha(ref.datos_hasta, idioma)]);
  if (typeof ref.total_filas === "number") chips.push([t.filas, String(ref.total_filas)]);
  return chips;
}

function Referencias({ refs, consultas, t, idioma }) {
  const [abierto, setAbierto] = useState(false);
  if (!refs || refs.length === 0) return null;
  return (
    <div className="asst-refs">
      <button type="button" className="asst-refs-toggle" onClick={() => setAbierto(!abierto)} aria-expanded={abierto}>
        <Info size={13} /> {t.referencias} ({refs.length}) {abierto ? <ChevronUp size={13} /> : <ChevronDown size={13} />}
      </button>
      {abierto && refs.map((r, i) => (
        <div className="asst-ref" key={i}>
          <div className="asst-ref-nombre">{r.consulta}</div>
          <div className="asst-chips">
            {chipsDe(r, t, idioma).map(([k, v], j) => (
              <span className="asst-chip" key={j} title={`${k}: ${v}`}><b>{k}</b> {v.length > 46 ? `${v.slice(0, 44)}…` : v}</span>
            ))}
          </div>
          {consultas && consultas[i] && (
            <details className="asst-consulta">
              <summary>{t.verConsulta}</summary>
              <code>{consultas[i].herramienta}({JSON.stringify(consultas[i].argumentos)})</code>
            </details>
          )}
        </div>
      ))}
    </div>
  );
}

function BotonExcel({ descarga, t }) {
  const [estado, setEstado] = useState("");
  const [error, setError] = useState("");
  const ctlRef = useRef(null);
  useEffect(() => () => { if (ctlRef.current) ctlRef.current.abort(); }, []);
  const bajar = async () => {
    if (estado === "cargando") return;
    setEstado("cargando"); setError("");
    const ctl = new AbortController();
    ctlRef.current = ctl;
    try {
      await descargarExcel(descarga, ctl.signal);
      setEstado("");
    } catch (e) {
      if (e && e.name === "AbortError") return;
      setError(mensajeDeError(e, t)); setEstado("");
    }
  };
  return (
    <div className={`asst-descarga-fila${descarga.destacar ? " asst-descarga-fila--destacada" : ""}`}>
      <button type="button" className="asst-descarga" onClick={bajar} disabled={estado === "cargando"} title={descarga.nombre || ""}>
        <Download size={14} /> {estado === "cargando" ? t.descargando : t.descargarExcel}
      </button>
      <span className="asst-descarga-nota">{t.excelFilas}{typeof descarga.filas_estimadas === "number" ? ` (~${descarga.filas_estimadas})` : ""}</span>
      {error && <div className="asst-advertencia" role="alert">{error}</div>}
    </div>
  );
}

/** La descarga (Excel) de la misma consulta que la gráfica: comparten `id`. Sin `id` (servicio antiguo) la ventana ampliada no ofrece Excel. */
function descargaDeGrafica(g, descargas) {
  return g && g.id ? (descargas || []).find((d) => d.id === g.id) || { id: g.id, url: `/asistente/descargas/${g.id}`, nombre: `${g.consulta || "datos"}.xlsx` } : undefined;
}

function Mensaje({ m, t, idioma }) {
  if (m.rol === "usuario") return <div className="asst-msg asst-msg-usuario"><p>{m.texto}</p></div>;
  return (
    <div className={`asst-msg asst-msg-asistente${m.error || m.bloqueado ? " asst-msg-error" : ""}${(m.graficas || []).length > 0 ? " asst-msg--ancho" : ""}`}>
      <TextoRespuesta texto={m.texto} />
      {(m.avisos || []).map((a, i) => <div className="asst-aviso-dato" key={i}><TriangleAlert size={13} /> <span>{a}</span></div>)}
      {(m.advertencias || []).map((a, i) => <div className="asst-advertencia" key={i}><b>{t.verificacion}:</b> {a}</div>)}
      {(m.graficas || []).map((g, k) => <Grafica key={`g${k}`} spec={g} t={t} idioma={idioma} descarga={descargaDeGrafica(g, m.descargas)} />)}
      {(m.descargas || []).map((d, k) => <BotonExcel key={d.id || `d${k}`} descarga={d} t={t} />)}
      <Referencias refs={m.referencias} consultas={m.consultas} t={t} idioma={idioma} />
    </div>
  );
}

// ---------- componente ----------
const AssistantPanel = ({ report, contextoReporte, contextoPagina, idioma, onToggle }) => {
  const t = textosPara(idioma);
  const lang = String(idioma || "es").toLowerCase().startsWith("en") ? "en" : "es";
  const [abierto, setAbierto] = useState(false);
  const [mensajes, setMensajes] = useState([]);
  const [entrada, setEntrada] = useState("");
  const [cargando, setCargando] = useState(false);
  const [conversacionId, setConversacionId] = useState(null);
  const [paginaPbi, setPaginaPbi] = useState("");
  const finRef = useRef(null);
  const abortRef = useRef(null);
  const contadorRef = useRef(0);

  // El botón flotante del Chatbot de la plataforma ocupa la esquina inferior derecha: si existe, nuestro botón se apila encima.
  const [sobreChatbot, setSobreChatbot] = useState(false);
  useEffect(() => {
    const revisar = () => setSobreChatbot(!!document.querySelector(".chatbot-button-container"));
    revisar();
    const id = setInterval(revisar, 1500);
    return () => clearInterval(id);
  }, []);

  // Avisa al contenedor (para reservar el espacio del panel) y limpia peticiones en curso al desmontar.
  useEffect(() => { if (onToggle) onToggle(abierto); }, [abierto]);
  useEffect(() => () => { if (abortRef.current) abortRef.current.abort(); }, []);

  // Página activa del informe de Power BI (solo orienta al asistente).
  useEffect(() => {
    if (!report) { setPaginaPbi(""); return undefined; }
    let vivo = true;
    const leer = async () => {
      try { const p = await report.getActivePage(); if (vivo && p) setPaginaPbi(p.displayName || ""); } catch (_) { /* aún no cargó */ }
    };
    const alCambiar = (e) => { const n = e && e.detail && e.detail.newPage; if (vivo && n) setPaginaPbi(n.displayName || ""); };
    try { report.on("loaded", leer); report.on("pageChanged", alCambiar); } catch (_) { /* informe sin eventos */ }
    leer();
    return () => {
      vivo = false;
      try { report.off("loaded", leer); report.off("pageChanged", alCambiar); } catch (_) { /* ya destruido */ }
    };
  }, [report]);

  useEffect(() => { if (finRef.current && finRef.current.scrollIntoView) finRef.current.scrollIntoView({ block: "end" }); }, [mensajes, cargando]);

  const pagina = paginaPbi || contextoPagina || "";
  const nuevoId = () => { contadorRef.current += 1; return contadorRef.current; };

  const enviar = useCallback(async (texto) => {
    const pregunta = (texto || "").trim();
    if (!pregunta || cargando) return;
    setMensajes((m) => [...m, { id: nuevoId(), rol: "usuario", texto: pregunta }]);
    setEntrada("");
    setCargando(true);
    const ctl = new AbortController();
    abortRef.current = ctl;
    try {
      const d = await preguntar({
        pregunta,
        conversacionId,
        contexto: contextoReporte || pagina ? { reporte: contextoReporte || "", pagina } : undefined,
        idioma: lang,
        signal: ctl.signal,
      });
      if (d.conversacion_id) setConversacionId(d.conversacion_id);
      const { descargas, graficas } = leerAdjuntos(d);
      setMensajes((m) => [...m, {
        id: nuevoId(), rol: "asistente", descargas, graficas,
        texto: d.error === "contenido_bloqueado" ? t.bloqueado : d.respuesta,
        referencias: d.referencias || [], avisos: d.avisos || [], advertencias: d.advertencias || [], consultas: d.consultas || [],
        bloqueado: d.error === "contenido_bloqueado",
      }]);
    } catch (e) {
      if (e && e.name === "AbortError") return;
      setMensajes((m) => [...m, { id: nuevoId(), rol: "asistente", texto: mensajeDeError(e, t), error: true }]);
    } finally {
      setCargando(false);
    }
  }, [cargando, conversacionId, contextoReporte, pagina, t, lang]);

  const reiniciar = () => {
    if (abortRef.current) abortRef.current.abort();
    setMensajes([]); setConversacionId(null); setEntrada(""); setCargando(false);
  };

  const alTeclear = (e) => {
    if (e.key === "Enter" && !e.shiftKey) { e.preventDefault(); enviar(entrada); }
  };

  if (!abierto) {
    return (
      <button type="button" className={`asst-fab${sobreChatbot ? " asst-fab--sobre-chatbot" : ""}`} onClick={() => setAbierto(true)} aria-label={t.boton}>
        <Sparkles size={18} /> <span>{t.boton}</span>
      </button>
    );
  }

  return (
    <aside className="asst-panel" aria-label={t.titulo}>
      <header className="asst-header">
        <div className="asst-titulo"><Sparkles size={16} /> <strong>{t.titulo}</strong> <span className="asst-beta">{t.beta}</span></div>
        <div className="asst-acciones">
          <button type="button" onClick={reiniciar} title={t.nueva} aria-label={t.nueva}><RotateCcw size={16} /></button>
          <button type="button" onClick={() => setAbierto(false)} title={t.cerrar} aria-label={t.cerrar}><X size={18} /></button>
        </div>
      </header>

      {(contextoReporte || pagina) && (
        <div className="asst-contexto" title={`${contextoReporte || ""} ${pagina}`}>
          {t.viendo}: <b>{[contextoReporte, pagina].filter(Boolean).join(" · ")}</b>
        </div>
      )}

      <div className="asst-mensajes" aria-live="polite">
        {mensajes.length === 0 && (
          <div className="asst-bienvenida">
            <p>{t.bienvenida}</p>
            <div className="asst-sug-titulo">{t.sugerenciasTitulo}</div>
            {t.sugerencias.map((s) => (
              <button type="button" key={s} className="asst-sug" onClick={() => enviar(s)} disabled={cargando}>{s}</button>
            ))}
            <ExportarDirecto t={t} mensajeDeError={mensajeDeError} />
          </div>
        )}
        {mensajes.map((m) => <Mensaje key={m.id} m={m} t={t} idioma={lang} />)}
        {cargando && <div className="asst-escribiendo" role="status" aria-label={t.pensando}><span /><span /><span /><em>{t.pensando}</em></div>}
        <div ref={finRef} />
      </div>

      <form className="asst-entrada" onSubmit={(e) => { e.preventDefault(); enviar(entrada); }}>
        <textarea
          rows={2} value={entrada} maxLength={1000} placeholder={t.placeholder} disabled={cargando}
          onChange={(e) => setEntrada(e.target.value)} onKeyDown={alTeclear} aria-label={t.placeholder}
        />
        <button type="submit" disabled={cargando || !entrada.trim()} aria-label={t.enviar} title={t.enviar}><Send size={18} /></button>
      </form>
      <footer className="asst-pie">{t.aviso}</footer>
    </aside>
  );
};

export default AssistantPanel;
