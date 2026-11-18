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
    // A2 — alcance. La instancia raíz (Alcance.Todo) comparte conexión y metadatos con las vistas acotadas que crea Para().
    readonly ConsultasMySql _raiz;
    readonly Alcance _alc = Alcance.Todo;
    string? _descAlcance;

    public ConsultasMySql(string cadena, int offsetUtcMinutos, ExclusionCuentas exclusion)
    {
        _cadena = cadena;
        _offsetMin = offsetUtcMinutos;
        _excl = exclusion;
        _raiz = this;
    }

    ConsultasMySql(ConsultasMySql raiz, Alcance alcance)
    {
        _cadena = raiz._cadena; _offsetMin = raiz._offsetMin; _excl = raiz._excl;
        _raiz = raiz; _alc = alcance;
    }

    public IConsultasUso Para(Alcance alcance) => alcance.EsTodo ? _raiz : new ConsultasMySql(_raiz, alcance);

    /// <summary>
    /// Filtro de alcance sobre la escuela (alias <paramref name="sc"/>), SIEMPRE en SQL. Con alcance, un usuario sin escuela queda fuera
    /// (la comparación con NULL no se cumple). ORE por <c>school.ore_id</c> = OreId de UsersAPI (SUPUESTO sin verificar: ver DISENO_AUTH.md pregunta 8).
    /// </summary>
    string FA(string sc)
    {
        if (_alc.EsTodo) return "";
        var sb = new StringBuilder();
        if (_alc.OreId is not null) sb.Append($" AND {sc}.ore_id = @eya_ore");
        if (_alc.EscuelaId is not null) sb.Append($" AND {sc}.school_id = @eya_esc");
        return sb.ToString();
    }

    void ParamsAlcance(MySqlParameterCollection p)
    {
        if (_alc.OreId is { } o) p.AddWithValue("@eya_ore", o);
        if (_alc.EscuelaId is { } e) p.AddWithValue("@eya_esc", e);
    }

    public string DescripcionAlcance
    {
        get
        {
            if (_alc.EsTodo) return "todas las escuelas";
            if (_descAlcance is not null) return _descAlcance;
            string? Uno(string sql, Action<MySqlParameterCollection> par) => Leer(sql, par, r => r.IsDBNull(0) ? null : r.GetString(0)).FirstOrDefault();
            var d = _alc.EscuelaId is { } e
                ? "escuela " + (Uno("SELECT school_name FROM school WHERE school_id = @e", p => p.AddWithValue("@e", e)) ?? $"#{e}")
                : "ORE " + (Uno("SELECT MIN(ore) FROM school WHERE ore_id = @o", p => p.AddWithValue("@o", _alc.OreId!.Value)) ?? $"#{_alc.OreId}");
            return _descAlcance = d;
        }
    }

    public int? EscuelaDeUsuario(string userId) =>
        int.TryParse(userId, NumberStyles.None, CultureInfo.InvariantCulture, out var id)
            ? Leer("SELECT school_id FROM user WHERE user_id = @u AND school_id IS NOT NULL LIMIT 1", p => p.AddWithValue("@u", id), r => (int?)Entero(r, 0)).FirstOrDefault()
            : null;

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

    string FE(string sc)      // por alcance (A2) y por ORE / nombre de la escuela excluidos (los usuarios sin escuela quedan fuera)
    {
        if (!_excl.Activa) return FA(sc);
        var sb = new StringBuilder(FA(sc));
        // Misma semántica que las vistas de Power BI: NOT (ore LIKE '%X%'); si la escuela o su ORE son NULL, la fila queda fuera.
        for (var i = 0; i < _excl.Ores.Length; i++) sb.Append($" AND {sc}.ore NOT LIKE @xo{i}");
        for (var i = 0; i < _excl.Escuelas.Length; i++) sb.Append($" AND COALESCE({sc}.school_name, '') <> @xe{i}");
        return sb.ToString();
    }

    string FU(string us, string sc) => FD(us) + FE(sc);

    void ParamsExcl(MySqlParameterCollection p)
    {
        ParamsAlcance(p);
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

    // «Datos hasta» es de toda la base (no revela nada del resto): lo calcula y cachea la instancia raíz.
    public DateOnly? DatosHastaPruebas { get { _raiz.Refrescar(); return _raiz._hastaPruebas; } }
    public DateOnly? DatosHastaConexiones { get { _raiz.Refrescar(); return _raiz._hastaConexiones; } }

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

    // ── Conexiones acumuladas ───────────────────────────────────────────────────────────
    // Mismo universo que Tendencia (usuarios con escuela, rol ≠ client, sin cuentas de prueba). «Usuario nuevo» = día de su
    // PRIMERA conexión dentro del rango, así la suma de nuevos = usuarios distintos del rango (la tarjeta «Conectado único»).
    public List<(DateOnly Dia, int Conexiones, int UsuariosNuevos)> ConexionesYNuevosPorDia(DateOnly desde, DateOnly hasta, string? escuela)
    {
        var sql = $@"SELECT x.dia, SUM(x.n) AS conexiones, SUM(x.es_primera) AS nuevos
            FROM (SELECT d.dia, d.user_id, d.n, d.dia = MIN(d.dia) OVER (PARTITION BY d.user_id) AS es_primera
                  FROM (SELECT DATE(DATE_ADD(dt.connected_date, INTERVAL {_offsetMin} MINUTE)) AS dia, dt.user_id, COUNT(*) AS n
                        FROM connected_date dt
                        JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {RolCliente}
                        LEFT JOIN school sh ON sh.school_id = us.school_id
                        WHERE dt.connected_date >= @a AND dt.connected_date < @b AND us.school_id IS NOT NULL
                          AND (@e IS NULL OR sh.school_name = @e) {FU("us", "sh")}
                        GROUP BY dia, dt.user_id) d) x
            GROUP BY x.dia ORDER BY x.dia";
        return Leer(sql, p =>
        {
            p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta));
            p.AddWithValue("@e", (object?)escuela ?? DBNull.Value); ParamsExcl(p);
        }, r => (DateOnly.FromDateTime(r.GetDateTime(0)), Entero(r, 1), Entero(r, 2)));
    }

    // Réplica de vw_total_users_with_connection_details (wiki): suscripción con status 'active' (SIN mirar cancellation_date),
    // rol ≠ client. Solo se aplica la exclusión por ORE y por dominio: la tarjeta de Power BI no tiene escuela, así que el filtro
    // de página «no es Facilitadores MSJ» no la afecta (cruce 18-nov: 7.277 = 7.277).
    public ConteoActivos UsuariosActivos()
    {
        var fo = new StringBuilder(FA("sh"));      // con alcance, solo usuarios de las escuelas del alcance (los sin escuela quedan fuera)
        if (_excl.Activa) for (var i = 0; i < _excl.Ores.Length; i++) fo.Append($" AND sh.ore NOT LIKE @xo{i}");
        var filtro = $"sph.status = 'active' AND us.role_id <> {RolCliente} {fo} {FD("us")}";
        var sql = $@"SELECT
            (SELECT COUNT(DISTINCT us.user_id) FROM user us LEFT JOIN school sh ON sh.school_id = us.school_id
                    JOIN subscription_profile_history sph ON sph.user_id = us.user_id WHERE {filtro}) AS activos,
            (SELECT COUNT(DISTINCT us.user_id) FROM user us LEFT JOIN school sh ON sh.school_id = us.school_id
                    JOIN subscription_profile_history sph ON sph.user_id = us.user_id
                    WHERE {filtro} AND EXISTS (SELECT 1 FROM connected_date dt WHERE dt.user_id = us.user_id)) AS conectados";
        return Leer(sql, ParamsExcl, r => new ConteoActivos(Entero(r, 0), Entero(r, 1))).First();   // ParamsExcl incluye @eya_*
    }

    public string? NotaActivos =>
        "usuarios_activos = usuarios con una suscripción en estado 'active' (no se revisa la fecha de cancelación), de cualquier rol salvo client; es la tarjeta «Activos» de Power BI y NO cambia con el filtro de escuela. " +
        "porcentaje_conexion_usuarios = usuarios_conectados del período / usuarios_activos, igual que la tarjeta «% Conexión usuarios» de Power BI: los conectados se cuentan tengan o no suscripción activa, así que no es una proporción exacta (en teoría puede pasar de 100 %). usuarios_conectados_alguna_vez = activos con al menos una conexión en todo el histórico.";

    // ── Uso de docentes (vw_teacher_activities) ─────────────────────────────────────────
    // La vista (definición real 18-nov, distinta de la wiki) trae UNA FILA POR DOCENTE Y PERÍODO ACADÉMICO de su escuela, con los
    // indicadores calculados dentro de ese período. Power BI elige un período y cuenta maestros DISTINTOS (validado 18-nov:
    // Escuela del Deporte, período 2026-09-12 → 18 · 8 asignaciones · 7 grupos · 0 planes · 0 planes con datos).
    // Lee bases de producción de otras APIs a través de la vista: los datos son del momento de la consulta (Power BI = foto diaria).
    string FiltroDocentes()
    {
        var sb = new StringBuilder();
        // A2: la vista solo trae NOMBRES (escuela y ORE), no ids. Se exige que exista una escuela del alcance con el MISMO nombre de escuela
        // y el MISMO nombre de ORE (restrictivo: no se filtra por solo uno). SUPUESTO por verificar: nombre de ORE ↔ ore_id es 1:1.
        if (!_alc.EsTodo) sb.Append($" AND EXISTS (SELECT 1 FROM school sa WHERE sa.school_name = t.school AND sa.ore = t.ore {FA("sa")})");
        if (!_excl.Activa) return sb.ToString();
        for (var i = 0; i < _excl.Ores.Length; i++) sb.Append($" AND t.ore NOT LIKE @xo{i}");
        for (var i = 0; i < _excl.Escuelas.Length; i++) sb.Append($" AND COALESCE(t.school, '') <> @xe{i}");
        return sb.ToString();
    }

    public (List<FilaUsoDocentes> Filas, FilaUsoDocentes Total) UsoDocentes(DateOnly? fecha, DateOnly? periodoDesde, string? escuela)
    {
        const string Indicadores = @"COUNT(DISTINCT t.sie),
            COUNT(DISTINCT CASE WHEN t.has_connections_current_period = 'SI' THEN t.sie END),
            COUNT(DISTINCT CASE WHEN t.has_created_instructional_plans_current_period = 'SI' THEN t.sie END),
            COUNT(DISTINCT CASE WHEN t.plans_have_data_current_period = 'SI' THEN t.sie END),
            COUNT(DISTINCT CASE WHEN t.has_created_groups_current_year = 'SI' THEN t.sie END),
            COUNT(DISTINCT CASE WHEN t.has_created_assignments_overall = 'SI' THEN t.sie END)";
        var donde = $@"FROM vw_teacher_activities t
            WHERE (CASE WHEN @pd IS NULL THEN @f BETWEEN DATE(t.from_period) AND DATE(t.to_period) ELSE DATE(t.from_period) = @pd END)
              AND (@e IS NULL OR t.school = @e) {FiltroDocentes()}";
        void Par(MySqlParameterCollection p)
        {
            p.AddWithValue("@f", fecha is { } f ? f.ToDateTime(TimeOnly.MinValue) : DBNull.Value);
            p.AddWithValue("@pd", periodoDesde is { } d ? d.ToDateTime(TimeOnly.MinValue) : DBNull.Value);
            p.AddWithValue("@e", (object?)escuela ?? DBNull.Value); ParamsExcl(p);
        }
        static string Dia(MySqlDataReader r, int i) => r.IsDBNull(i) ? "" : DateOnly.FromDateTime(r.GetDateTime(i)).ToString("yyyy-MM-dd");
        var filas = Leer($@"SELECT t.school, DATE(t.from_period), DATE(t.to_period), {Indicadores} {donde}
            GROUP BY t.school, DATE(t.from_period), DATE(t.to_period)
            ORDER BY COUNT(DISTINCT t.sie) DESC, t.school, DATE(t.from_period)", Par,
            r => new FilaUsoDocentes(r.IsDBNull(0) ? "(sin escuela)" : r.GetString(0), Dia(r, 1), Dia(r, 2),
                Entero(r, 3), Entero(r, 4), Entero(r, 5), Entero(r, 6), Entero(r, 7), Entero(r, 8)));
        var total = Leer($"SELECT {Indicadores} {donde}", Par,
            r => new FilaUsoDocentes("TOTAL", "", "", Entero(r, 0), Entero(r, 1), Entero(r, 2), Entero(r, 3), Entero(r, 4), Entero(r, 5))).First();
        return (filas, total);
    }

    public string? NotaDocentes =>
        "Fuente: vista vw_teacher_activities (la misma de la página de uso de maestros de Power BI). Cuenta maestros DISTINTOS con suscripción activa. " +
        "Cada escuela tiene su propio calendario: se usa, por escuela, el período académico que contiene la fecha pedida (o el que empieza en periodo_desde). " +
        "con_conexiones = se conectó dentro de ese período; con_planes / planes_con_datos = creó planes instruccionales en el período / con quizzes o lecciones; " +
        "con_grupos = creó grupos en el año académico; con_asignaciones = ha creado alguna asignación (histórico). Datos al momento de la consulta (Power BI muestra la foto de su refresco diario). " + NotaExclusion;

    // ── Patrones de conexión (día de la semana / rol) ───────────────────────────────────
    static readonly string[] DiasSemana = ["lunes", "martes", "miércoles", "jueves", "viernes", "sábado", "domingo"];

    public List<FilaPatron> PatronConexiones(DateOnly desde, DateOnly hasta, string agrupar, string? escuela)
    {
        var clave = agrupar == "rol" ? "COALESCE(us.role, '(sin rol)')" : $"WEEKDAY(DATE_ADD(dt.connected_date, INTERVAL {_offsetMin} MINUTE))";
        var sql = $@"SELECT {clave} AS k, COUNT(*) AS conexiones, COUNT(DISTINCT dt.user_id) AS usuarios
            FROM connected_date dt
            JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {RolCliente}
            LEFT JOIN school sh ON sh.school_id = us.school_id
            WHERE dt.connected_date >= @a AND dt.connected_date < @b AND us.school_id IS NOT NULL
              AND (@e IS NULL OR sh.school_name = @e) {FU("us", "sh")}
            GROUP BY k";
        var filas = Leer(sql, p =>
        {
            p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta));
            p.AddWithValue("@e", (object?)escuela ?? DBNull.Value); ParamsExcl(p);
        }, r => (K: Convert.ToString(r.GetValue(0), CultureInfo.InvariantCulture)!, N: Entero(r, 1), U: Entero(r, 2)));
        if (agrupar == "rol")
            return filas.OrderByDescending(f => f.N).ThenBy(f => f.K, StringComparer.Ordinal).Select(f => new FilaPatron(f.K, f.N, f.U)).ToList();
        return DiasSemana.Select((nombre, i) =>
        {
            var f = filas.FirstOrDefault(x => x.K == i.ToString(CultureInfo.InvariantCulture));
            return new FilaPatron(nombre, f.N, f.U);
        }).ToList();
    }

    // ── Participación por escuela ───────────────────────────────────────────────────────
    // activos = suscripción 'active' (misma definición que la tarjeta «Activos»); conectados = usuarios distintos con alguna conexión
    // en el período (con o sin suscripción activa, como la tarjeta de Power BI). Escuelas = las que tienen activos o conectados.
    public List<FilaParticipacion> ParticipacionEscuelas(DateOnly desde, DateOnly hasta)
    {
        var sql = $@"SELECT sh.school_name,
            (SELECT COUNT(DISTINCT us.user_id) FROM user us JOIN subscription_profile_history sph ON sph.user_id = us.user_id
              WHERE us.school_id = sh.school_id AND sph.status = 'active' AND us.role_id <> {RolCliente} {FD("us")}) AS activos,
            (SELECT COUNT(DISTINCT dt.user_id) FROM connected_date dt JOIN user us ON us.user_id = dt.user_id AND us.role_id <> {RolCliente}
              WHERE us.school_id = sh.school_id AND dt.connected_date >= @a AND dt.connected_date < @b {FD("us")}) AS conectados
            FROM school sh WHERE 1 = 1 {FE("sh")}
            HAVING activos > 0 OR conectados > 0
            ORDER BY sh.school_name";
        return Leer(sql, p => { p.AddWithValue("@a", Inicio(desde)); p.AddWithValue("@b", FinExclusivo(hasta)); ParamsExcl(p); },
            r => { int a = Entero(r, 1), c = Entero(r, 2); return new FilaParticipacion(r.GetString(0), a, c, a == 0 ? null : Math.Round(100.0 * c / a, 1)); });
    }

    public string? NotaParticipacion =>
        "participacion = usuarios conectados en el período / usuarios con suscripción activa de la escuela (gráfico «% de usuarios conectados por escuela» de Power BI). " +
        "usuarios_activos = suscripción 'active' sin revisar la fecha de cancelación; usuarios_conectados cuenta a quien se conectó en el período tenga o no suscripción activa, " +
        "así que el porcentaje puede pasar de 100 %. Solo usuarios con escuela, sin el rol client. Solo escuelas con usuarios activos o con conexiones en el período. " + NotaExclusion;
}
