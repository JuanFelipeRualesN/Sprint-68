# API de exportación (REQ-3741 · Fase 4)

`GET /exportar/{consulta}?formato=xlsx|grafica&<mismos parámetros que la consulta>`

- `{consulta}` = la ruta de consulta sin `/`: `uso-por-area`, `conexiones-por-escuela`, `conexiones-por-maestro`, `comparar-areas`, `comparar-escuelas`, `promedio-full-quiz`, `tendencia-conexiones`, `conexiones-acumuladas`, `uso-docentes`, `patrones-conexion`, `participacion-escuelas`. (`buscar-escuela` no se exporta.)
- Los parámetros y sus validaciones son **los mismos** que los de la ruta JSON (mismos mensajes 400/503).
- `formato=xlsx` (por defecto): libro `.xlsx` con la hoja `Datos` y la hoja `Referencias` (consulta, fuente, columna de fecha, período, filtros, cuentas de prueba, datos hasta, filas exportadas, nota completa, resumen). Sin `limite` trae **todas** las filas (hasta 5000; `limite` admite 1–5000). El nombre viene en `Content-Disposition`: `{consulta}_{desde}_{hasta}.xlsx`.
- `formato=grafica`: JSON listo para Recharts (`tipo` `line`|`bar`, `layout`, `data`, `series`, `eje_x`, `eje_y`, `titulo`, referencias). Usa el `limite` normal (≤ 50). Menos de 2 filas → 422 `sin_grafica`.
- Errores: 404 `consulta_desconocida`, 400 `parametros_invalidos` (formato, fechas, límite, escuela), 422 `sin_grafica`, 503 `base_no_disponible`.

## Protección y riesgos (leer antes de desplegar)
- **Misma protección que el resto de las rutas:** el middleware exige `x-api-key` **solo si** existe la variable `EYA_API_KEY`. Si no existe (p. ej. con `correr_api_mysql.sh`, que no la define) **cualquiera que llegue a la API puede descargar los datos**.
- No hay autenticación de usuario ni alcance por rol o por escuela: quien tenga la clave (o acceso a la red) descarga cualquier escuela. El Excel trae nombres de maestros (hasta 5000 filas). Solo para uso local hasta que esté detrás del backend de la plataforma.
- No está en `openapi.json` a propósito: el asistente convierte cada `path` de ese archivo en una herramienta del modelo.
- CORS (solo si el asistente está habilitado): política `panel` con `GET`/`POST` y `Content-Disposition` expuesto; no permite el header `x-api-key` desde el navegador, así que una descarga directa desde el front con clave debe pasar por el backend de la plataforma.
