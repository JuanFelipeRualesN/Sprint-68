import React, { useState, useRef, useEffect } from "react";
import { Download, ChevronDown, ChevronUp } from "lucide-react";
import { descargarExportacion } from "./assistant-api";

/**
 * Exportación directa a Excel, sin pasar por el chat (Fase 4 · REQ-3741): GET /exportar/{consulta}.
 * Solo las consultas que se piden con `desde` y `hasta` (las comparativas y uso-docentes usan otros parámetros:
 * se piden por el chat, o se agregan aquí con sus campos). Parámetros opcionales (escuela, orden…) se añaden igual.
 */
const CONSULTAS = ["conexiones-por-escuela", "conexiones-por-maestro", "participacion-escuelas", "tendencia-conexiones", "conexiones-acumuladas", "patrones-conexion", "uso-por-area", "promedio-full-quiz"];

const ExportarDirecto = ({ t, mensajeDeError }) => {
  const [abierto, setAbierto] = useState(false);
  const [consulta, setConsulta] = useState(CONSULTAS[0]);
  const [desde, setDesde] = useState("");
  const [hasta, setHasta] = useState("");
  const [estado, setEstado] = useState("");
  const [error, setError] = useState("");
  const ctlRef = useRef(null);
  useEffect(() => () => { if (ctlRef.current) ctlRef.current.abort(); }, []);

  const descargar = async (e) => {
    e.preventDefault();
    if (!desde || !hasta || estado === "cargando") return;
    setEstado("cargando"); setError("");
    const ctl = new AbortController();
    ctlRef.current = ctl;
    try {
      await descargarExportacion(consulta, { desde, hasta }, ctl.signal);
      setEstado("");
    } catch (err) {
      if (err && err.name === "AbortError") return;
      setError(mensajeDeError(err, t)); setEstado("");
    }
  };

  return (
    <div className="asst-exportar">
      <button type="button" className="asst-refs-toggle" onClick={() => setAbierto(!abierto)} aria-expanded={abierto}>
        <Download size={13} /> {t.exportarTitulo} {abierto ? <ChevronUp size={13} /> : <ChevronDown size={13} />}
      </button>
      {abierto && (
        <form className="asst-exportar-form" onSubmit={descargar}>
          <label>{t.consulta}
            <select value={consulta} onChange={(e) => setConsulta(e.target.value)}>
              {CONSULTAS.map((c) => <option key={c} value={c}>{t.consultas[c] || c}</option>)}
            </select>
          </label>
          <div className="asst-exportar-fechas">
            <label>{t.desde}<input type="date" value={desde} max={hasta || undefined} onChange={(e) => setDesde(e.target.value)} required /></label>
            <label>{t.hasta}<input type="date" value={hasta} min={desde || undefined} onChange={(e) => setHasta(e.target.value)} required /></label>
          </div>
          <button type="submit" className="asst-descarga" disabled={!desde || !hasta || estado === "cargando"}>
            <Download size={14} /> {estado === "cargando" ? t.descargando : t.descargarExcel}
          </button>
          {error && <div className="asst-advertencia" role="alert">{error}</div>}
        </form>
      )}
    </div>
  );
};

export default ExportarDirecto;
