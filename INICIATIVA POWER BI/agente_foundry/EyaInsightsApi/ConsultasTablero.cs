using System.Globalization;
using MySqlConnector;

namespace EyaInsightsApi;

// Filas de las consultas «del tablero» (P1–P10 de MSJ_Utilization_modelo_y_10_preguntas.md). Salen en snake_case.
public record FilaUsuariosRol(string Rol, int Usuarios, int Activos, int Cancelados, int SoloCancelados, int SinSuscripcion, int ActivosYCancelados);
public record FilaConexionEscuelaPct(string Escuela, int UsuariosConectados, int UsuariosActivos, double? PorcentajeConectados);
public record FilaNoConectadosRol(string Rol, int Activos, int Conectados, int NoConectados);
public record FilaEscuelaVisitada(string Escuela, string? SchoolIdentifier, string? SistemaEducativo, bool Visitada,
    DateOnly? FechaVisitaInicial, DateOnly? FechaUltimaVisita, string? Alerta);
public record FilaQuizzesPeriodo(string AnioAcademico, string Periodo, int Asignados, int Tomados, int NoTomados, double? PorcentajeTomados);
public record FilaUbicacion(string Nombre, int UsuariosConectados, int Conexiones);
public record FilaSuscripcionesAnio(string AnioAcademico, int Suscripciones, int Usuarios, int Filas, int SuscripcionesActivas, int SuscripcionesCanceladas);
public record FilaConexionesMes(string Mes, int Conexiones, int UsuariosConectados);

/// <summary>La fuente de datos existe pero todavía no está disponible (p. ej. el Excel de visitas no se ha cargado). Se responde 503.</summary>
public class FuenteNoDisponible(string mensaje) : Exception(mensaje);

/// <summary>
/// Consultas sobre el esquema `agente` (sql/09_crear_vistas_agente.sql): vistas que leen las mismas vw_* que el modelo
/// Reports_Semantic_Model y aplican las reglas de su Power Query. Validadas contra Power BI el 17-nov-2026 (CRUCE_POWER_BI.md).
/// SOLO LECTURA (sesión READ ONLY) y solo agregados: nunca devuelve emails ni nombres de personas.
/// «definicion»: «tablero» reproduce la medida de Power BI tal cual; «corregida» aplica lo que el nombre de la medida promete
/// (p. ej. «conectados» = solo usuarios con suscripción activa).
/// </summary>
public class ConsultasTablero(string cadena)
{
    public string Fuente => "mysql:agente (vistas validadas contra Power BI)";

    // ── Infraestructura ─────────────────────────────────────────────────────────────────
    MySqlConnection Abrir()
    {
        var c = new MySqlConnection(cadena);
        c.Open();
        using (var cmd = new MySqlCommand("SET SESSION TRANSACTION READ ONLY", c)) cmd.ExecuteNonQuery();
        using (var cmd = new MySqlCommand("SELECT @@session.transaction_read_only", c))
            if (Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) != 1)
            {
                c.Dispose();
                throw new InvalidOperationException("La sesión de MySQL no quedó en solo lectura; se cancela por seguridad.");
            }
        return c;
    }

    List<T> Leer<T>(string sql, Action<MySqlParameterCollection> parametros, Func<MySqlDataReader, T> mapa)
    {
        using var c = Abrir();
        using var cmd = new MySqlCommand(sql, c) { CommandTimeout = 60 };
        parametros(cmd.Parameters);
        using var r = cmd.ExecuteReader();
        var filas = new List<T>();
        while (r.Read()) filas.Add(mapa(r));
        return filas;
    }

    static int E(MySqlDataReader r, int i) => r.IsDBNull(i) ? 0 : Convert.ToInt32(r.GetValue(i), CultureInfo.InvariantCulture);
    static string? S(MySqlDataReader r, int i) => r.IsDBNull(i) ? null : Convert.ToString(r.GetValue(i), CultureInfo.InvariantCulture);
    static DateOnly? D(MySqlDataReader r, int i) => r.IsDBNull(i) ? null : DateOnly.FromDateTime(Convert.ToDateTime(r.GetValue(i), CultureInfo.InvariantCulture));
    public static double? Pct(int parte, int total, int decimales = 2) => total == 0 ? null : Math.Round(100.0 * parte / total, decimales);

    // Filtro opcional de fechas sobre agente.conexiones (alias c). Sin fechas = todo el histórico.
    const string FiltroFechas = "(@a IS NULL OR c.connected_date >= @a) AND (@b IS NULL OR c.connected_date <= @b)";
    const string SinFacilitadores = "COALESCE(c.es_facilitadores, 0) = 0";
    static void Fechas(MySqlParameterCollection p, DateOnly? desde, DateOnly? hasta)
    {
        p.AddWithValue("@a", desde is { } d ? d.ToDateTime(TimeOnly.MinValue) : DBNull.Value);
        p.AddWithValue("@b", hasta is { } h ? h.ToDateTime(TimeOnly.MinValue) : DBNull.Value);
    }

    // ── Metadatos ───────────────────────────────────────────────────────────────────────
    public DateOnly? DatosHastaConexiones() =>
        Leer("SELECT MAX(connected_date) FROM agente.conexiones", _ => { }, r => D(r, 0)).FirstOrDefault();

    public bool EscuelaExiste(string escuela) =>
        Leer("SELECT 1 FROM agente.usuarios WHERE school_name = @n LIMIT 1", p => p.AddWithValue("@n", escuela), _ => 1).Count > 0;

    // ── P1 / P6 · usuarios y estado de suscripción ──────────────────────────────────────
    /// <summary>Por rol y total. Un usuario puede tener a la vez suscripciones activas y canceladas: por eso los estados se solapan.</summary>
    public (List<FilaUsuariosRol> PorRol, FilaUsuariosRol Total) ResumenUsuarios(string? escuela)
    {
        const string sql = @"SELECT COALESCE(role, '(sin rol)') AS rol, COUNT(*), SUM(a), SUM(c), SUM(c AND NOT a), SUM(s), SUM(a AND c)
            FROM (SELECT user_id, MAX(role) AS role, MAX(status = 'active') AS a, MAX(status = 'canceled') AS c,
                         MAX(status = 'sin subscripción') AS s
                  FROM agente.usuarios_suscripciones
                  WHERE (@e IS NULL OR school_name = @e)
                  GROUP BY user_id) t
            GROUP BY rol WITH ROLLUP";
        var filas = Leer(sql, p => p.AddWithValue("@e", (object?)escuela ?? DBNull.Value),
            r => new FilaUsuariosRol(S(r, 0) ?? "TOTAL", E(r, 1), E(r, 2), E(r, 3), E(r, 4), E(r, 5), E(r, 6)));
        var total = filas.LastOrDefault(f => f.Rol == "TOTAL") ?? new FilaUsuariosRol("TOTAL", 0, 0, 0, 0, 0, 0);
        return (filas.Where(f => f.Rol != "TOTAL").OrderByDescending(f => f.Usuarios).ToList(), total);
    }

    // ── P2 · % de usuarios conectados por escuela ───────────────────────────────────────
    /// <summary>
    /// tablero: conectados = usuarios distintos con conexión en la escuela, de cualquier estado (medida Conteo_Conectados_Si);
    /// corregida: solo los que hoy tienen suscripción activa. Denominador en ambos: usuarios con suscripción activa en la escuela.
    /// </summary>
    public List<FilaConexionEscuelaPct> ConexionPorEscuelaPct(DateOnly? desde, DateOnly? hasta, bool corregida)
    {
        var act = corregida ? "AND c.email IN (SELECT email FROM agente.usuarios_suscripciones WHERE status = 'active' AND email IS NOT NULL)" : "";
        var sql = $@"SELECT a.school_name, COALESCE(x.n, 0), a.activos
            FROM (SELECT school_name, COUNT(DISTINCT user_id) AS activos FROM agente.usuarios_suscripciones
                  WHERE status = 'active' AND school_name IS NOT NULL GROUP BY school_name) a
            LEFT JOIN (SELECT c.school_name, COUNT(DISTINCT c.email) AS n FROM agente.conexiones c
                       WHERE {FiltroFechas} {act} GROUP BY c.school_name) x ON x.school_name = a.school_name";
        return Leer(sql, p => Fechas(p, desde, hasta), r =>
        {
            int n = E(r, 1), activos = E(r, 2);
            return new FilaConexionEscuelaPct(S(r, 0)!, n, activos, Pct(n, activos));
        });
    }

    // ── P3 · usuarios activos no conectados, por rol ───────────────────────────────────
    /// <summary>
    /// tablero: medida «Usuarios NO Conectados (Activos por Rol)» = MAX(0, activos − conectados), donde «conectados» NO exige
    /// suscripción activa (cuenta cualquier email del rol que se haya conectado); corregida: conectados = solo activos.
    /// El total no es la suma de los roles: se calcula con usuarios distintos, como la fila de total de Power BI.
    /// </summary>
    public (List<FilaNoConectadosRol> PorRol, FilaNoConectadosRol Total) NoConectadosPorRol(DateOnly? desde, DateOnly? hasta, bool corregida)
    {
        var act = corregida ? "AND status = 'active'" : "";
        var porRol = $@"SELECT COALESCE(r.role, '(sin rol)'), r.activos, COALESCE(t.conectados, 0)
            FROM (SELECT role, COUNT(DISTINCT CASE WHEN status = 'active' THEN email END) AS activos
                  FROM agente.usuarios_suscripciones WHERE email IS NOT NULL GROUP BY role) r
            LEFT JOIN (SELECT u.role, COUNT(DISTINCT c.email) AS conectados
                       FROM agente.conexiones c
                       JOIN (SELECT DISTINCT role, email FROM agente.usuarios_suscripciones WHERE email IS NOT NULL {act}) u ON u.email = c.email
                       WHERE {FiltroFechas} GROUP BY u.role) t ON t.role <=> r.role";
        var total = $@"SELECT (SELECT COUNT(DISTINCT email) FROM agente.usuarios_suscripciones WHERE status = 'active' AND email IS NOT NULL),
                              (SELECT COUNT(DISTINCT c.email) FROM agente.conexiones c
                               WHERE {FiltroFechas}
                                 AND c.email IN (SELECT email FROM agente.usuarios_suscripciones WHERE email IS NOT NULL {act}))";
        FilaNoConectadosRol Fila(string rol, int activos, int conectados) => new(rol, activos, conectados, Math.Max(0, activos - conectados));
        var filas = Leer(porRol, p => Fechas(p, desde, hasta), r => Fila(S(r, 0)!, E(r, 1), E(r, 2)))
            .Where(f => f.Activos > 0 || f.Conectados > 0).OrderByDescending(f => f.Activos).ToList();
        var tot = Leer(total, p => Fechas(p, desde, hasta), r => Fila("TOTAL", E(r, 0), E(r, 1))).Single();
        return (filas, tot);
    }

    // ── P5 · escuelas visitadas (Excel cargado en agente.escuelas_visitadas, sql/10) ─────
    public (List<FilaEscuelaVisitada> Filas, DateTime? CargadoEn) EscuelasVisitadas()
    {
        const string sql = @"SELECT school_name, school_identifier, sistema_educativo, visitada, fecha_visita_inicial, fecha_ultima_visita, cargado_en
            FROM agente.escuelas_visitadas ORDER BY school_name";
        try
        {
            DateTime? cargado = null;
            var filas = Leer(sql, _ => { }, r =>
            {
                if (!r.IsDBNull(6)) { var c = Convert.ToDateTime(r.GetValue(6), CultureInfo.InvariantCulture); if (cargado is null || c > cargado) cargado = c; }
                bool visitada = E(r, 3) == 1;
                DateOnly? ini = D(r, 4), ult = D(r, 5);
                string? alerta = visitada && ult is null ? "Marcada como visitada pero sin fecha de última visita."
                    : ini is { } i && ult is { } u && u < i ? "La última visita es anterior a la visita inicial (dato a revisar en el Excel)."
                    : !visitada && (ini is not null || ult is not null) ? "Marcada como no visitada pero tiene fechas de visita."
                    : null;
                return new FilaEscuelaVisitada(S(r, 0)!, S(r, 1), S(r, 2), visitada, ini, ult, alerta);
            });
            return (filas, cargado);
        }
        catch (MySqlException e) when (e.ErrorCode is MySqlErrorCode.NoSuchTable or MySqlErrorCode.TableAccessDenied)
        {
            throw new FuenteNoDisponible("El Excel de escuelas visitadas todavía no está cargado en la base (tabla agente.escuelas_visitadas).");
        }
    }

    // ── P8 · quizzes tomados y no tomados por año académico y período ────────────────────
    public List<FilaQuizzesPeriodo> QuizzesPorPeriodo(string? anio, string? periodo, bool excluirPosprueba)
    {
        var sql = $@"SELECT COALESCE(anio_academico, '(sin año)'), COALESCE(periodo, '(sin período)'),
                            SUM(completado = 'Completed'), SUM(completado = 'Not Completed')
            FROM agente.pruebas
            WHERE (@an IS NULL OR anio_academico = @an) AND (@p IS NULL OR periodo = @p) {(excluirPosprueba ? "AND NOT es_posprueba" : "")}
            GROUP BY 1, 2 ORDER BY 1, 2";
        return Leer(sql, p =>
        {
            p.AddWithValue("@an", (object?)anio ?? DBNull.Value); p.AddWithValue("@p", (object?)periodo ?? DBNull.Value);
        }, r =>
        {
            int t = E(r, 2), nt = E(r, 3);
            return new FilaQuizzesPeriodo(S(r, 0)!, S(r, 1)!, t + nt, t, nt, Pct(t, t + nt));
        });
    }

    // ── P9 · conectados por ciudad u ORE ────────────────────────────────────────────────
    public List<FilaUbicacion> ConexionesPorUbicacion(DateOnly? desde, DateOnly? hasta, string agrupar, bool incluirFacilitadores)
    {
        var col = agrupar == "ore" ? "c.ore" : "c.city";   // lista cerrada: nunca se interpola texto del usuario
        var sql = $@"SELECT COALESCE({col}, '(sin dato)'), COUNT(DISTINCT c.email), COUNT(*)
            FROM agente.conexiones c
            WHERE {FiltroFechas} {(incluirFacilitadores ? "" : "AND " + SinFacilitadores)}
            GROUP BY 1 ORDER BY 2 DESC, 1";
        return Leer(sql, p => Fechas(p, desde, hasta), r => new FilaUbicacion(S(r, 0)!, E(r, 1), E(r, 2)));
    }

    /// <summary>Usuarios distintos y conexiones de todo el rango (los usuarios no se pueden sumar entre grupos o meses).</summary>
    public (int Usuarios, int Conexiones) TotalConexiones(DateOnly? desde, DateOnly? hasta, bool incluirFacilitadores) =>
        Leer($@"SELECT COUNT(DISTINCT c.email), COUNT(*) FROM agente.conexiones c
                WHERE {FiltroFechas} {(incluirFacilitadores ? "" : "AND " + SinFacilitadores)}",
            p => Fechas(p, desde, hasta), r => (E(r, 0), E(r, 1))).Single();

    // ── P4 · conexiones por mes (incluye los meses en cero) ─────────────────────────────
    public List<FilaConexionesMes> ConexionesPorMes(DateOnly desde, DateOnly hasta, bool incluirFacilitadores)
    {
        var sql = $@"SELECT k.anio_mes, COALESCE(x.n, 0), COALESCE(x.u, 0)
            FROM (SELECT DISTINCT anio_mes FROM agente.calendario WHERE fecha BETWEEN @a AND @b) k
            LEFT JOIN (SELECT DATE_FORMAT(c.connected_date, '%Y-%m') AS m, COUNT(*) AS n, COUNT(DISTINCT c.email) AS u
                       FROM agente.conexiones c
                       WHERE {FiltroFechas} {(incluirFacilitadores ? "" : "AND " + SinFacilitadores)}
                       GROUP BY m) x ON x.m = k.anio_mes
            ORDER BY k.anio_mes";
        return Leer(sql, p => Fechas(p, desde, hasta), r => new FilaConexionesMes(S(r, 0)!, E(r, 1), E(r, 2)));
    }

    // ── P10 · suscripciones por año académico ───────────────────────────────────────────
    /// <summary>criterio «fin» = «Academic Year» del tablero (año de cancellation_date, que es la fecha de fin programada);
    /// «activacion» = año académico (ago–jul) de activated_date.</summary>
    public List<FilaSuscripcionesAnio> SuscripcionesPorAnio(string criterio, string? anio)
    {
        var col = criterio == "activacion" ? "anio_academico_activacion" : "anio_academico_fin";   // lista cerrada
        var sql = $@"SELECT COALESCE({col}, '(sin fecha)'), COUNT(DISTINCT subscription_id), COUNT(DISTINCT user_id), COUNT(*),
                            COUNT(DISTINCT CASE WHEN status = 'active' THEN subscription_id END),
                            COUNT(DISTINCT CASE WHEN status = 'canceled' THEN subscription_id END)
            FROM agente.suscripciones WHERE (@an IS NULL OR {col} = @an)
            GROUP BY 1 ORDER BY 1";
        return Leer(sql, p => p.AddWithValue("@an", (object?)anio ?? DBNull.Value),
            r => new FilaSuscripcionesAnio(S(r, 0)!, E(r, 1), E(r, 2), E(r, 3), E(r, 4), E(r, 5)));
    }
}
