using System.Globalization;
using System.Text;
using MySqlConnector;

namespace EyaInsightsApi;

/// <summary>
/// Cuentas internas y de prueba que distorsionan los rankings (descubierto el 12-nov-2026: casi todas están en el ORE
/// «DEMOSTRACION»; los correos @requiretechnology.com suman el 33 % de las conexiones de 2026). Se EXCLUYEN por defecto y la
/// respuesta lo declara siempre; Datos__ExcluirCuentasDePrueba=false las incluye (útil para comparar con Power BI).
/// </summary>
public record ExclusionCuentas(bool Activa, string[] Ores, string[] Dominios, string[] Escuelas);

/// <summary>
/// Las consultas del agente sobre gs_data_analysis (MySQL). SOLO LECTURA:
///  · la sesión se abre en modo READ ONLY y con time_zone UTC (las columnas son TIMESTAMP en UTC);
///  · todo va con parámetros; el agente nunca escribe SQL;
///  · jamás se devuelven emails, usernames ni datos de estudiantes (solo agregados y nombre de docente).
/// Definiciones (confirmadas con el esquema real, 12-nov-2026):
///  · conexión = una fila de connected_date = un usuario en un día (hay una fila por usuario-día);
///  · docente = user.role_id 5; estudiante = role_id 3; «área» = materia (subject), pendiente de definición en el ticket;
///  · pruebas: cohorte por fecha de asignación (quiz_assigned_to_plan_date); «completada» = tiene quiz_taken_date;
///  · «día» se lee en hora de Puerto Rico (UTC-4) por defecto (decisión de Juan, 12-nov-2026); Datos:OffsetUtcMinutos=0 lo cambia a UTC.
/// </summary>
public class ConsultasMySql : IConsultasUso
{
    const int RolCliente = 1, RolEstudiante = 3, RolDocente = 5;   // la vista vw_connections_report de Power BI excluye el rol client (1)

    readonly string _cadena;
    readonly int _offsetMin;
    readonly ExclusionCuentas _excl;
    readonly object _candado = new();
    DateTime _expira = DateTime.MinValue;
    DateOnly? _hastaPruebas, _hastaConexiones;

    public ConsultasMySql(string cadena, int offsetUtcMinutos, ExclusionCuentas exclusion)
    {
        _cadena = cadena;
        _offsetMin = offsetUtcMinutos;
        _excl = exclusion;
    }

    // ── Metadatos que se citan en cada respuesta ────────────────────────────────────────
    string Tz => _offsetMin switch
    {
        0 => "UTC",
        -240 => "hora de Puerto Rico (UTC-4)",
        _ => $"UTC{(_offsetMin > 0 ? "+" : "-")}{Math.Abs(_offsetMin) / 60.0:0.##}"
    };
    public string Fuente => "mysql:gs_data_analysis";
    public string ColumnaFechaPruebas =>
        $"result_generic.quiz_assigned_to_plan_date (fecha en que se asignó la prueba); día en {Tz}";
    public string ColumnaFechaConexiones => $"connected_date.connected_date; día en {Tz}";
    public string? NotaArea =>
        "Área = materia (definición pendiente). Solo pruebas de estudiantes y materias con pruebas asignadas alguna vez. " +
        "Cada prueba cuenta en el período por su fecha de asignación; pruebas_completadas = de las asignadas en el período, las que ya " +
        "fueron realizadas a la fecha de los datos (por eso nunca supera a pruebas_asignadas); porcentaje_completado = completadas / asignadas. " + NotaExclusion;
    public string? NotaConexiones =>
        "Conexión = un usuario con ingreso en un día (una fila por usuario y día); usuarios_conectados = usuarios distintos con alguna conexión en el período (la tarjeta «Usuarios conectados» de Power BI). Rol de cualquier tipo salvo client. Por escuela: solo escuelas con al menos " +
        "un estudiante o una conexión (escuelas reales de Estudia y Avanza; así el ranking por escuela suma lo mismo que el total por período). Por maestro: solo usuarios con rol teacher. Solo se cuentan usuarios con escuela asignada y se excluye el rol client, " +
        "igual que la vista vw_connections_report de Power BI, así que el total por escuela y el total por trimestre coinciden. " + NotaExclusion;

    public bool ExcluyeCuentasDePrueba => _excl.Activa;

    string NotaExclusion
    {
        get
        {
            if (!_excl.Activa) return "Incluye cuentas internas y de prueba (no se excluye nada).";
            var partes = new List<string>();
            if (_excl.Ores.Length > 0) partes.Add($"escuelas cuyo ORE contiene «{string.Join("», «", _excl.Ores)}» (igual que las vistas de Power BI)");
            if (_excl.Escuelas.Length > 0) partes.Add($"las escuelas «{string.Join("», «", _excl.Escuelas)}»");
            if (_excl.Dominios.Length > 0) partes.Add($"usuarios con correo @{string.Join(" o @", _excl.Dominios)}");
            return "Se EXCLUYEN cuentas internas y de prueba: " + string.Join(", ", partes) + ".";
        }
    }

    // ── Filtros de exclusión (SQL con parámetros fijos @xd#/@xo#/@xe#, que se registran con ParamsExcl) ─────────
    string FD(string us)      // por dominio de correo del usuario
    {
        if (!_excl.Activa) return "";
        var sb = new StringBuilder();
        for (var i = 0; i < _excl.Dominios.Length; i++) sb.Append($" AND COALESCE({us}.email, '') NOT LIKE @xd{i}");
        return sb.ToString();
    }

    string FE(string sc)      // por ORE y por nombre de la escuela (los usuarios sin escuela pasan)
    {
        if (!_excl.Activa) return "";
        var sb = new StringBuilder();
        // Misma semántica que las vistas de Power BI: NOT (ore LIKE '%X%'); si la escuela o su ORE son NULL, la fila queda fuera.
        for (var i = 0; i < _excl.Ores.Length; i++) sb.Append($" AND {sc}.ore NOT LIKE @xo{i}");
        for (var i = 0; i < _excl.Escuelas.Length; i++) sb.Append($" AND COALESCE({sc}.school_name, '') <> @xe{i}");
        return sb.ToString();
    }

    string FU(string us, string sc) => FD(us) + FE(sc);

    void ParamsExcl(MySqlParameterCollection p)
    {
        if (!_excl.Activa) return;
        for (var i = 0; i < _excl.Dominios.Length; i++) p.AddWithValue($"@xd{i}", "%@" + _excl.Dominios[i]);
        for (var i = 0; i < _excl.Ores.Length; i++) p.AddWithValue($"@xo{i}", "%" + _excl.Ores[i] + "%");
        for (var i = 0; i < _excl.Escuelas.Length; i++) p.AddWithValue($"@xe{i}", _excl.Escuelas[i]);
    }

    /// <summary>Usuario de MySQL con el que se conectó (solo para el mensaje de arranque; nunca la clave).</summary>
    public string UsuarioConectado
    {
        get
        {
            using var c = Abrir();
            using var cmd = new MySqlCommand("SELECT CURRENT_USER()", c);
            return Convert.ToString(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) ?? "?";
        }
    }

    public DateOnly? DatosHastaPruebas { get { Refrescar(); return _hastaPruebas; } }
    public DateOnly? DatosHastaConexiones { get { Refrescar(); return _hastaConexiones; } }

    // ── Infraestructura ─────────────────────────────────────────────────────────────────
    MySqlConnection Abrir()
    {
        var c = new MySqlConnection(_cadena);
        c.Open();
        foreach (var sql in new[] { "SET time_zone = '+00:00'", "SET SESSION TRANSACTION READ ONLY" })
        {
            using var cmd = new MySqlCommand(sql, c);
            cmd.ExecuteNonQuery();
        }
        // Defensa extra (la cuenta puede tener más permisos de los necesarios): no se trabaja si la sesión no es de solo lectura.
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
        using var cmd = new MySqlCommand(sql, c) { CommandTimeout = 30 };
        parametros(cmd.Parameters);
        using var r = cmd.ExecuteReader();
        var filas = new List<T>();
        while (r.Read()) filas.Add(mapa(r));
        return filas;
    }

    static int Entero(MySqlDataReader r, int i) => r.IsDBNull(i) ? 0 : Convert.ToInt32(r.GetValue(i), CultureInfo.InvariantCulture);

    // El día "d" local empieza en este instante UTC (d 00:00 local = d 00:00 UTC - offset).
    DateTime Inicio(DateOnly d) => d.ToDateTime(TimeOnly.MinValue).AddMinutes(-_offsetMin);
    DateTime FinExclusivo(DateOnly h) => h.AddDays(1).ToDateTime(TimeOnly.MinValue).AddMinutes(-_offsetMin);
    static string Dir(bool desc) => desc ? "DESC" : "ASC";

    void Refrescar()
    {
        lock (_candado)
        {
            if (DateTime.UtcNow < _expira) return;
            DateOnly? Dia(DateTime? v) => v is { } dt ? DateOnly.FromDateTime(dt.AddMinutes(_offsetMin)) : null;
            // «Datos hasta» se calcula sobre los datos que SÍ se consultan (sin las cuentas de prueba excluidas).
            _hastaConexiones = Dia(Leer($@"SELECT MAX(dt.connected_date)
                    FROM connected_date dt JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {RolCliente}
                    LEFT JOIN school sc ON sc.school_id = us.school_id
                    WHERE 1 = 1 {FU("us", "sc")}",
                ParamsExcl, r => r.IsDBNull(0) ? (DateTime?)null : r.GetDateTime(0)).FirstOrDefault());
            _hastaPruebas = Dia(Leer($@"SELECT GREATEST(MAX(r.quiz_taken_date), MAX(r.quiz_assigned_to_plan_date))
                    FROM result_generic r JOIN user us ON us.user_id = r.user_id AND us.role_id = @rol
                    LEFT JOIN school sc ON sc.school_id = us.school_id
                    WHERE 1 = 1 {FU("us", "sc")}",
                p => { ParamsExcl(p); p.AddWithValue("@rol", RolEstudiante); },
                r => r.IsDBNull(0) ? (DateTime?)null : r.GetDateTime(0)).FirstOrDefault());
            _expira = DateTime.UtcNow.AddMinutes(10);
        }
    }

    // ── Catálogo de escuelas ────────────────────────────────────────────────────────────
    public bool EscuelaExiste(string escuela) =>
        Leer($"SELECT 1 FROM school sc WHERE sc.school_name = @n {FE("sc")} LIMIT 1",
            p => { ParamsExcl(p); p.AddWithValue("@n", escuela); }, r => 1).Count > 0;

    public List<FilaCatalogoEscuela> BuscarEscuelas(string texto, int limite)
    {
        var patron = "%" + texto.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_") + "%";
        return Leer($"SELECT sc.school_name FROM school sc WHERE sc.school_name LIKE @p {FE("sc")} ORDER BY sc.school_name LIMIT @l",
            p => { ParamsExcl(p); p.AddWithValue("@p", patron); p.AddWithValue("@l", limite); },
            r => new FilaCatalogoEscuela(r.GetString(0)));
    }

    // ── Pruebas por área ────────────────────────────────────────────────────────────────
    // Universo: materias con al menos una prueba asignada alguna vez (el catálogo tiene ~70 materias casi todas vacías).
    // OJO: NO usar `r.user_id IN (SELECT ... LEFT JOIN ...)` dentro del ON de un LEFT JOIN: MySQL 8 devolvió resultados
    // equivocados (contaba pruebas de docentes como de estudiantes). Se usa una tabla derivada con JOIN explícitos.
    string DesdeAreas() => $@"
        FROM subject s
        LEFT JOIN (SELECT rr.subject_code, rr.quiz_assigned_to_plan_date, rr.quiz_taken_date
                   FROM result_generic rr
                   JOIN user us ON us.user_id = rr.user_id AND us.role_id = @rol
                   LEFT JOIN school sc ON sc.school_id = us.school_id
                   WHERE 1 = 1 {FU("us", "sc")}) r ON r.subject_code = s.subject_code
        WHERE EXISTS (SELECT 1 FROM result_generic x WHERE x.subject_code = s.subject_code)
        GROUP BY s.subject_code, s.subject_name";

    public List<FilaArea> UsoPorArea(DateOnly desde, DateOnly hasta, bool desc, int limite)
    {
        // Una sola base de fechas (cohorte por fecha de asignación): completadas <= asignadas y el % es coherente.
        var sql = $@"SELECT s.subject_name,
            COALESCE(SUM(r.quiz_assigned_to_plan_date >= @a AND r.quiz_assigned_to_plan_date < @b), 0) AS asignadas,
            COALESCE(SUM(r.quiz_assigned_to_plan_date >= @a AND r.quiz_assigned_to_plan_date < @b
                         AND r.quiz_taken_date IS NOT NULL), 0) AS completadas
            {DesdeAreas()}
            ORDER BY completadas {Dir(desc)}, s.subject_name {Dir(desc)}
            LIMIT @l";
        return Leer(sql, p =>
        {
            p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta));
            p.AddWithValue("@rol", RolEstudiante); p.AddWithValue("@l", limite); ParamsExcl(p);
        }, r =>
        {
            int asig = Entero(r, 1), comp = Entero(r, 2);
            return new FilaArea(r.GetString(0), asig, comp, asig == 0 ? null : Math.Round(100.0 * comp / asig, 1));
        });
    }

    public List<FilaComparacion> CompararAreas(DateOnly desdeA, DateOnly hastaA, DateOnly desdeB, DateOnly hastaB)
    {
        var sql = $@"SELECT s.subject_name,
            COALESCE(SUM(r.quiz_assigned_to_plan_date >= @a1 AND r.quiz_assigned_to_plan_date < @b1 AND r.quiz_taken_date IS NOT NULL), 0) AS va,
            COALESCE(SUM(r.quiz_assigned_to_plan_date >= @a2 AND r.quiz_assigned_to_plan_date < @b2 AND r.quiz_taken_date IS NOT NULL), 0) AS vb
            {DesdeAreas()}
            ORDER BY s.subject_name";
        return Leer(sql, p =>
        {
            p.AddWithValue("@a1", Inicio(desdeA)); p.AddWithValue("@b1", FinExclusivo(hastaA));
            p.AddWithValue("@a2", Inicio(desdeB)); p.AddWithValue("@b2", FinExclusivo(hastaB));
            p.AddWithValue("@rol", RolEstudiante); ParamsExcl(p);
        }, r =>
        {
            int va = Entero(r, 1), vb = Entero(r, 2), dif = vb - va;
            return new FilaComparacion(r.GetString(0), va, vb, dif, va == 0 ? null : Math.Round(100.0 * dif / va, 1));
        });
    }

    public string? NotaPromedioQuiz =>
        "% Full Quiz = result_generic.total_percent_full_quiz tal como está en la base (misma escala que Power BI). Solo pruebas de estudiantes con escuela. " +
        "Cada prueba cuenta en el período por su fecha de asignación. promedio_solo_realizadas = promedio de las pruebas ya realizadas; " +
        "promedio_todas_las_asignadas = promedio de todas las asignadas, donde las no realizadas cuentan como 0 (igual que la vista vw_academics_average). " +
        "Las materias sin pruebas en el período no aparecen. " + NotaExclusion;

    public List<FilaPromedioQuiz> PromedioFullQuiz(DateOnly desde, DateOnly hasta, string agrupar, bool desc, int limite, string? escuela)
    {
        bool porEscuela = agrupar is "escuela" or "escuela_materia", porMateria = agrupar is "materia" or "escuela_materia";
        var cols = new List<string>(); var grupo = new List<string>();
        if (porEscuela) { cols.Add("sc.school_name"); grupo.Add("sc.school_name"); }
        if (porMateria) { cols.Add("COALESCE(s.subject_name, rr.subject_code)"); grupo.Add("COALESCE(s.subject_name, rr.subject_code)"); }
        var orden = string.Join(", ", grupo);
        var sql = $@"SELECT {(porEscuela ? "sc.school_name" : "NULL")}, {(porMateria ? "COALESCE(s.subject_name, rr.subject_code)" : "NULL")},
            COUNT(*) AS asignadas,
            COALESCE(SUM(rr.quiz_taken_date IS NOT NULL), 0) AS realizadas,
            AVG(CASE WHEN rr.quiz_taken_date IS NOT NULL THEN rr.total_percent_full_quiz END) AS prom_realizadas,
            AVG(COALESCE(rr.total_percent_full_quiz, 0)) AS prom_todas
            FROM result_generic rr
            JOIN user us ON us.user_id = rr.user_id AND us.role_id = @rol
            JOIN school sc ON sc.school_id = us.school_id
            LEFT JOIN subject s ON s.subject_code = rr.subject_code
            WHERE rr.quiz_assigned_to_plan_date >= @a AND rr.quiz_assigned_to_plan_date < @b
              AND (@e IS NULL OR sc.school_name = @e) {FU("us", "sc")}
            GROUP BY {orden}
            ORDER BY prom_realizadas IS NULL, prom_realizadas {Dir(desc)}, {orden}
            LIMIT @l";
        return Leer(sql, p =>
        {
            p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta));
            p.AddWithValue("@rol", RolEstudiante); p.AddWithValue("@l", limite);
            p.AddWithValue("@e", (object?)escuela ?? DBNull.Value); ParamsExcl(p);
        }, r => new FilaPromedioQuiz(
            r.IsDBNull(0) ? null : r.GetString(0), r.IsDBNull(1) ? null : r.GetString(1), Entero(r, 2), Entero(r, 3),
            r.IsDBNull(4) ? null : Math.Round(Convert.ToDouble(r.GetValue(4), CultureInfo.InvariantCulture), 2),
            Math.Round(Convert.ToDouble(r.GetValue(5), CultureInfo.InvariantCulture), 2)));
    }

    // ── Conexiones ──────────────────────────────────────────────────────────────────────
    public List<FilaEscuela> ConexionesPorEscuela(DateOnly desde, DateOnly hasta, bool desc, int limite)
    {
        var sql = $@"SELECT sh.school_name, COALESCE(x.n, 0) AS conexiones, COALESCE(x.u, 0) AS usuarios
            FROM school sh
            LEFT JOIN (SELECT us.school_id, COUNT(*) AS n, COUNT(DISTINCT dt.user_id) AS u
                       FROM connected_date dt JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {RolCliente}
                       WHERE dt.connected_date >= @a AND dt.connected_date < @b {FD("us")}
                       GROUP BY us.school_id) x ON x.school_id = sh.school_id
            WHERE (EXISTS (SELECT 1 FROM user u2 WHERE u2.school_id = sh.school_id AND u2.role_id = {RolEstudiante})
                   OR EXISTS (SELECT 1 FROM connected_date d2 JOIN user u3 ON u3.user_id = d2.user_id AND u3.role_id <> {RolCliente}
                              WHERE u3.school_id = sh.school_id)) {FE("sh")}
            ORDER BY conexiones {Dir(desc)}, sh.school_name {Dir(desc)}
            LIMIT @l";
        return Leer(sql, p =>
        {
            p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta)); p.AddWithValue("@l", limite);
            ParamsExcl(p);
        }, r => new FilaEscuela(r.GetString(0), Entero(r, 1), Entero(r, 2)));
    }

    public List<FilaMaestro> ConexionesPorMaestro(DateOnly desde, DateOnly hasta, bool desc, int limite, string? escuela)
    {
        var sql = $@"SELECT TRIM(CONCAT_WS(' ', us.first_name, us.last_name)) AS maestro,
                            COALESCE(sh.school_name, '(sin escuela)') AS escuela,
                            COALESCE(x.n, 0) AS conexiones
            FROM user us
            LEFT JOIN school sh ON sh.school_id = us.school_id
            LEFT JOIN (SELECT dt.user_id, COUNT(*) AS n FROM connected_date dt
                       WHERE dt.connected_date >= @a AND dt.connected_date < @b
                       GROUP BY dt.user_id) x ON x.user_id = us.user_id
            WHERE us.role_id = @rol AND us.school_id IS NOT NULL AND (@e IS NULL OR sh.school_name = @e) {FU("us", "sh")}
            ORDER BY conexiones {Dir(desc)}, maestro {Dir(desc)}, us.user_id
            LIMIT @l";
        return Leer(sql, p =>
        {
            p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta));
            p.AddWithValue("@rol", RolDocente); p.AddWithValue("@l", limite);
            p.AddWithValue("@e", (object?)escuela ?? DBNull.Value); ParamsExcl(p);
        }, r => new FilaMaestro(r.GetString(0), r.GetString(1), Entero(r, 2)));
    }

    public List<FilaTendencia> Tendencia(DateOnly desde, DateOnly hasta, string granularidad, string? escuela)
    {
        var sql = $@"SELECT DATE(DATE_ADD(dt.connected_date, INTERVAL {_offsetMin} MINUTE)) AS dia, COUNT(*) AS n
            FROM connected_date dt
            JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {RolCliente}
            LEFT JOIN school sh ON sh.school_id = us.school_id
            WHERE dt.connected_date >= @a AND dt.connected_date < @b AND us.school_id IS NOT NULL AND (@e IS NULL OR sh.school_name = @e) {FU("us", "sh")}
            GROUP BY dia";
        var porDia = Leer(sql, p =>
        {
            p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta));
            p.AddWithValue("@e", (object?)escuela ?? DBNull.Value); ParamsExcl(p);
        }, r => (Dia: DateOnly.FromDateTime(r.GetDateTime(0)), N: Entero(r, 1)));

        var acc = new SortedDictionary<string, int>(StringComparer.Ordinal);
        for (var d = desde; d <= hasta; d = d.AddDays(1)) acc.TryAdd(ConsultasEjemplo.Clave(d, granularidad), 0);   // incluye períodos en cero
        foreach (var (dia, n) in porDia) acc[ConsultasEjemplo.Clave(dia, granularidad)] += n;
        return acc.Select(kv => new FilaTendencia(kv.Key, kv.Value)).ToList();
    }
}
