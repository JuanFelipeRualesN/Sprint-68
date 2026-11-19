namespace EyaInsightsApi;

/// <summary>
/// Datos INVENTADOS con semilla fija (no son cifras reales). Sirven para probar que el agente
/// elige bien la consulta. Equivale a api_ejemplo.py, pero en .NET.
/// «Área» todavía no está definida en el ticket: aquí se toma área = materia (supuesto).
/// </summary>
public class ConsultasEjemplo : IConsultasUso
{
    static readonly string[] Areas = ["Matemáticas", "Lectura", "Ciencias", "Estudios Sociales", "Inglés"];
    static readonly string[] TodasEscuelas = "ABCDEFGHIJKL".Select(c => $"Escuela Ejemplo {c}").ToArray();   // K y L sin actividad
    static readonly (string Maestro, string Escuela)[] TodosMaestros =
        Enumerable.Range(0, 36).Select(i => ($"Maestro Ejemplo {i + 1:00}", TodasEscuelas[i % 12])).ToArray();

    // A2 — ids sintéticos para que el alcance también funcione sin MySQL: escuela i (A=0…) → school_id i+1; ORE 1 = A–F, ORE 2 = G–L.
    static int IdEscuela(string e) => Array.IndexOf(TodasEscuelas, e) + 1;
    static int OreDe(string e) => IdEscuela(e) <= 6 ? 1 : 2;

    // Vistas acotadas (Para): comparten los datos generados pero solo ven las escuelas del alcance.
    string[] Escuelas = TodasEscuelas;
    string[] EscuelasActivas = TodasEscuelas.Take(10).ToArray();
    (string Maestro, string Escuela)[] Maestros = TodosMaestros;
    Alcance _alc = Alcance.Todo;
    public string DescripcionAlcance => _alc.EsTodo ? "todas las escuelas"
        : _alc.EscuelaId is { } e ? $"escuela {TodasEscuelas.ElementAtOrDefault(e - 1) ?? "#" + e}" : $"ORE ejemplo {_alc.OreId}";
    public int? EscuelaDeUsuario(string userId) => null;       // los datos de ejemplo no tienen usuarios con escuela

    public IConsultasUso Para(Alcance alcance)
    {
        if (alcance.EsTodo) return this;
        bool Ve(string e) => (alcance.OreId is null || OreDe(e) == alcance.OreId) && (alcance.EscuelaId is null || IdEscuela(e) == alcance.EscuelaId);
        var v = (ConsultasEjemplo)MemberwiseClone();
        v._alc = alcance;
        v.Escuelas = Escuelas.Where(Ve).ToArray();
        v.EscuelasActivas = EscuelasActivas.Where(Ve).ToArray();
        v.Maestros = Maestros.Where(m => Ve(m.Escuela)).ToArray();
        var visibles = v.Escuelas.ToHashSet();
        v._conexiones = _conexiones.Where(c => visibles.Contains(c.Escuela)).ToList();
        v._pruebas = _pruebas.Where(x => visibles.Contains(x.Escuela)).ToList();
        return v;
    }

    record Conexion(DateOnly Dia, string Escuela, string Maestro, int Cantidad);
    record Prueba(DateOnly Dia, string Area, string Escuela, int Asignadas, int Completadas);

    List<Conexion> _conexiones = [];
    List<Prueba> _pruebas = [];

    public string Fuente => "datos_de_ejemplo";
    public string ColumnaFechaPruebas => "día de la prueba (dato de ejemplo)";
    public string ColumnaFechaConexiones => "día de la conexión (dato de ejemplo)";
    public DateOnly? DatosHastaPruebas => new DateOnly(2026, 11, 10);      // el generador crea datos hasta esta fecha
    public DateOnly? DatosHastaConexiones => new DateOnly(2026, 11, 10);
    public string? NotaConexiones => null;
    public bool ExcluyeCuentasDePrueba => false;
    public string? NotaArea => "Uso = pruebas completadas; «área» se tomó como materia (definición pendiente).";

    public ConsultasEjemplo()
    {
        var rnd = new Random(2026);
        for (var dia = new DateOnly(2026, 1, 12); dia <= new DateOnly(2026, 11, 10); dia = dia.AddDays(1))
        {
            if (dia.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
            foreach (var (maestro, escuela) in Maestros)
                if (EscuelasActivas.Contains(escuela) && rnd.NextDouble() < 0.35)
                    _conexiones.Add(new(dia, escuela, maestro, rnd.Next(1, 4)));
            foreach (var escuela in EscuelasActivas)
                for (var i = 0; i < Areas.Length; i++)
                {
                    var asignadas = rnd.Next(0, 13);
                    var peso = 1.0 + 0.4 * i;
                    var completadas = Math.Min(asignadas, (int)(asignadas * rnd.NextDouble() * peso * 0.6));
                    _pruebas.Add(new(dia, Areas[i], escuela, asignadas, completadas));
                }
        }
    }

    public bool EscuelaExiste(string escuela) => Escuelas.Contains(escuela);

    static string Plano(string s) =>
        new string(s.Normalize(System.Text.NormalizationForm.FormD)
            .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
            .ToArray()).ToLowerInvariant();

    public List<FilaCatalogoEscuela> BuscarEscuelas(string texto, int limite) =>
        Escuelas.Where(e => Plano(e).Contains(Plano(texto))).Take(limite).Select(e => new FilaCatalogoEscuela(e)).ToList();

    static IEnumerable<T> Ordenar<T>(IEnumerable<T> filas, Func<T, int> valor, Func<T, string> nombre, bool desc) =>
        desc ? filas.OrderByDescending(valor).ThenByDescending(nombre, StringComparer.Ordinal)
             : filas.OrderBy(valor).ThenBy(nombre, StringComparer.Ordinal);

    public List<FilaArea> UsoPorArea(DateOnly desde, DateOnly hasta, bool desc, int limite)
    {
        var filas = Areas.Select(a =>
        {
            var p = _pruebas.Where(x => x.Area == a && x.Dia >= desde && x.Dia <= hasta).ToList();
            int asig = p.Sum(x => x.Asignadas), comp = p.Sum(x => x.Completadas);
            return new FilaArea(a, asig, comp, asig == 0 ? null : Math.Round(100.0 * comp / asig, 1));   // el % lo calcula la API, no el modelo
        });
        return Ordenar(filas, f => f.PruebasCompletadas, f => f.Nombre, desc).Take(limite).ToList();
    }

    public List<FilaEscuela> ConexionesPorEscuela(DateOnly desde, DateOnly hasta, bool desc, int limite)
    {
        var filas = Escuelas.Select(e =>   // incluye escuelas con cero conexiones
            new FilaEscuela(e, _conexiones.Where(c => c.Escuela == e && c.Dia >= desde && c.Dia <= hasta).Sum(c => c.Cantidad),
                            _conexiones.Where(c => c.Escuela == e && c.Dia >= desde && c.Dia <= hasta).Select(c => c.Maestro).Distinct().Count()));
        return Ordenar(filas, f => f.Conexiones, f => f.Nombre, desc).Take(limite).ToList();
    }

    public List<FilaMaestro> ConexionesPorMaestro(DateOnly desde, DateOnly hasta, bool desc, int limite, string? escuela)
    {
        var filas = Maestros.Where(m => escuela is null || m.Escuela == escuela).Select(m =>
            new FilaMaestro(m.Maestro, m.Escuela, _conexiones.Where(c => c.Maestro == m.Maestro && c.Dia >= desde && c.Dia <= hasta).Sum(c => c.Cantidad)));
        return Ordenar(filas, f => f.Conexiones, f => f.Nombre, desc).Take(limite).ToList();
    }

    public List<FilaComparacion> CompararAreas(DateOnly desdeA, DateOnly hastaA, DateOnly desdeB, DateOnly hastaB)
    {
        int Total(string area, DateOnly d, DateOnly h) =>
            _pruebas.Where(x => x.Area == area && x.Dia >= d && x.Dia <= h).Sum(x => x.Completadas);
        return Areas.Select(a =>
        {
            int va = Total(a, desdeA, hastaA), vb = Total(a, desdeB, hastaB), dif = vb - va;
            return new FilaComparacion(a, va, vb, dif, va == 0 ? null : Math.Round(100.0 * dif / va, 1));
        }).ToList();
    }

    public string? NotaPromedioQuiz => "Los datos de ejemplo no incluyen puntajes.";
    public List<FilaPromedioQuiz> PromedioFullQuiz(DateOnly desde, DateOnly hasta, string agrupar, bool desc, int limite, string? escuela) =>
        throw new ErrorConsulta("El promedio de % Full Quiz no está disponible con datos de ejemplo.");

    internal static string Clave(DateOnly dia, string gran) => gran switch
    {
        "mes" => $"{dia.Year}-{dia.Month:00}",
        "trimestre" => $"{dia.Year}-T{(dia.Month - 1) / 3 + 1}",
        _ => dia.Year.ToString()
    };

    public List<FilaTendencia> Tendencia(DateOnly desde, DateOnly hasta, string granularidad, string? escuela)
    {
        var acc = new SortedDictionary<string, int>(StringComparer.Ordinal);
        for (var d = desde; d <= hasta; d = d.AddDays(1)) acc.TryAdd(Clave(d, granularidad), 0);   // incluye períodos en cero
        foreach (var c in _conexiones.Where(c => c.Dia >= desde && c.Dia <= hasta && (escuela is null || c.Escuela == escuela)))
            acc[Clave(c.Dia, granularidad)] += c.Cantidad;
        return acc.Select(kv => new FilaTendencia(kv.Key, kv.Value)).ToList();
    }

    // ── Conexiones acumuladas y uso de docentes (datos inventados) ─────────────────────
    public List<(DateOnly Dia, int Conexiones, int UsuariosNuevos)> ConexionesYNuevosPorDia(DateOnly desde, DateOnly hasta, string? escuela)
    {
        var sel = _conexiones.Where(c => c.Dia >= desde && c.Dia <= hasta && (escuela is null || c.Escuela == escuela)).ToList();
        var primera = sel.GroupBy(c => c.Maestro).ToDictionary(g => g.Key, g => g.Min(c => c.Dia));
        return sel.GroupBy(c => c.Dia).OrderBy(g => g.Key)
            .Select(g => (g.Key, g.Sum(c => c.Cantidad), g.Select(c => c.Maestro).Distinct().Count(m => primera[m] == g.Key))).ToList();
    }

    public ConteoActivos UsuariosActivos() => new(Maestros.Length * 4, _conexiones.Select(c => c.Maestro).Distinct().Count());
    public string? NotaActivos => "Datos de ejemplo: usuarios_activos y usuarios_conectados_alguna_vez son inventados.";

    public (List<FilaUsoDocentes> Filas, FilaUsoDocentes Total) UsoDocentes(DateOnly? fecha, DateOnly? periodoDesde, string? escuela)
    {
        // Dos calendarios inventados: escuelas A–F (ago–oct / oct–ene) y G–J (sep–oct / oct–ene).
        (DateOnly D, DateOnly H)[] Cal(int i) => i < 6
            ? [(new(2026, 8, 28), new(2026, 10, 23)), (new(2026, 10, 24), new(2027, 1, 8))]
            : [(new(2026, 9, 12), new(2026, 10, 15)), (new(2026, 10, 16), new(2027, 1, 21))];
        var filas = new List<FilaUsoDocentes>();
        for (var i = 0; i < EscuelasActivas.Length; i++)
        {
            var esc = EscuelasActivas[i];
            if (escuela is not null && esc != escuela) continue;
            foreach (var (d, h) in Cal(i))
            {
                if (periodoDesde is { } pd ? d != pd : !(fecha is { } f && f >= d && f <= h)) continue;
                int m = Maestros.Count(x => x.Escuela == esc), k = (i + d.Month) % 3;
                filas.Add(new(esc, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"), m, Math.Min(m, k + 1), k % 2, 0, Math.Min(m, k), Math.Min(m, 2)));
            }
        }
        filas = filas.OrderByDescending(f => f.Maestros).ThenBy(f => f.Escuela, StringComparer.Ordinal).ToList();
        var total = new FilaUsoDocentes("TOTAL", "", "", filas.Sum(f => f.Maestros), filas.Sum(f => f.ConConexiones), filas.Sum(f => f.ConPlanes),
            filas.Sum(f => f.PlanesConDatos), filas.Sum(f => f.ConGrupos), filas.Sum(f => f.ConAsignaciones));
        return (filas, total);
    }

    public string? NotaDocentes => "Datos de ejemplo: maestros y actividades inventados.";

    public List<FilaPatron> PatronConexiones(DateOnly desde, DateOnly hasta, string agrupar, string? escuela)
    {
        var sel = _conexiones.Where(c => c.Dia >= desde && c.Dia <= hasta && (escuela is null || c.Escuela == escuela)).ToList();
        if (agrupar == "rol") return sel.Count == 0 ? [] : [new("teacher", sel.Sum(c => c.Cantidad), sel.Select(c => c.Maestro).Distinct().Count())];
        string[] dias = ["lunes", "martes", "miércoles", "jueves", "viernes", "sábado", "domingo"];
        return dias.Select((n, i) =>
        {
            var g = sel.Where(c => ((int)c.Dia.DayOfWeek + 6) % 7 == i).ToList();
            return new FilaPatron(n, g.Sum(c => c.Cantidad), g.Select(c => c.Maestro).Distinct().Count());
        }).ToList();
    }

    public string? NotaParticipacion => "Datos de ejemplo: usuarios activos inventados (4 por maestro).";
    public List<FilaParticipacion> ParticipacionEscuelas(DateOnly desde, DateOnly hasta) =>
        EscuelasActivas.Select(e =>
        {
            int activos = Maestros.Count(m => m.Escuela == e) * 4;
            int con = _conexiones.Where(c => c.Escuela == e && c.Dia >= desde && c.Dia <= hasta).Select(c => c.Maestro).Distinct().Count();
            return new FilaParticipacion(e, activos, con, activos == 0 ? null : Math.Round(100.0 * con / activos, 1));
        }).ToList();
}
