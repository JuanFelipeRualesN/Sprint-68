/**
 * «Descargar imagen (PNG)» de una gráfica de Recharts (Fase 4 · REQ-3741): serializa el <svg> que dibuja Recharts, le añade el título y la
 * leyenda de las series y lo rasteriza en un canvas (x2) para bajar un PNG con fondo blanco. Sin dependencias nuevas.
 */
const FUENTE = "Poppins, Arial, Helvetica, sans-serif";

export const svgDe = (contenedor) => (contenedor ? contenedor.querySelector("svg.recharts-surface") : null);

function nombreSeguro(texto) {
  return String(texto || "grafica").normalize("NFD").replace(/[̀-ͯ]/g, "").replace(/[^a-zA-Z0-9_-]+/g, "_").replace(/^_+|_+$/g, "").toLowerCase().slice(0, 60) || "grafica";
}

/** Dibuja `svg` en un canvas con título y leyenda arriba. Devuelve el canvas (o lanza si el navegador no puede rasterizar el SVG). */
export function svgACanvas(svg, { titulo = "", series = [], colores = [], escala = 2 }) {
  const caja = svg.getBoundingClientRect();
  const ancho = Math.max(1, Math.round(parseFloat(svg.getAttribute("width")) || caja.width));
  const alto = Math.max(1, Math.round(parseFloat(svg.getAttribute("height")) || caja.height));
  const clon = svg.cloneNode(true);
  clon.setAttribute("xmlns", "http://www.w3.org/2000/svg");
  clon.setAttribute("width", String(ancho));
  clon.setAttribute("height", String(alto));
  clon.setAttribute("font-family", FUENTE);       // el SVG suelto ya no hereda la fuente del panel
  const texto = new XMLSerializer().serializeToString(clon);
  const cabecera = 52 + (series.length > 1 ? 22 : 0);
  return new Promise((resolver, rechazar) => {
    const img = new Image();
    img.onload = () => {
      const canvas = document.createElement("canvas");
      canvas.width = (ancho + 32) * escala;
      canvas.height = (alto + cabecera + 16) * escala;
      const c = canvas.getContext("2d");
      c.scale(escala, escala);
      c.fillStyle = "#ffffff";
      c.fillRect(0, 0, ancho + 32, alto + cabecera + 16);
      c.fillStyle = "#0f172a";
      c.font = `600 16px ${FUENTE}`;
      c.fillText(titulo, 16, 30, ancho);
      if (series.length > 1) {
        let x = 16;
        c.font = `12px ${FUENTE}`;
        series.forEach((s, i) => {
          c.fillStyle = colores[i % colores.length] || "#64748b";
          c.fillRect(x, 44, 12, 12);
          c.fillStyle = "#334155";
          c.fillText(s, x + 18, 54);
          x += 18 + c.measureText(s).width + 18;
        });
      }
      c.drawImage(img, 16, cabecera);
      resolver(canvas);
    };
    img.onerror = () => rechazar(new Error("no se pudo rasterizar la gráfica"));
    img.src = `data:image/svg+xml;charset=utf-8,${encodeURIComponent(texto)}`;
  });
}

/** Baja el PNG. `contenedor` = el elemento que contiene el <svg> de Recharts. */
export async function descargarImagen(contenedor, { titulo, series, colores, consulta }) {
  const svg = svgDe(contenedor);
  if (!svg) throw new Error("no hay gráfica que descargar");
  const canvas = await svgACanvas(svg, { titulo, series, colores });
  const blob = await new Promise((resolver, rechazar) => canvas.toBlob((b) => (b ? resolver(b) : rechazar(new Error("sin imagen"))), "image/png"));
  const nombre = `${nombreSeguro(consulta || titulo)}.png`;
  const href = URL.createObjectURL(blob);
  const enlace = document.createElement("a");
  enlace.href = href;
  enlace.download = nombre;
  document.body.appendChild(enlace);
  enlace.click();
  document.body.removeChild(enlace);
  setTimeout(() => URL.revokeObjectURL(href), 1000);
  return nombre;
}
