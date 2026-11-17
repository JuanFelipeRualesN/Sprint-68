namespace EyaInsightsApi;

// Filas que devuelve cada consulta. Los nombres salen en snake_case (pruebas_asignadas, ...).
public record FilaArea(string Nombre, int PruebasAsignadas, int PruebasCompletadas, double? PorcentajeCompletado, double? PorcentajeDelTotalCompletadas = null);
public record FilaEscuela(string Nombre, int Conexiones, int UsuariosConectados, double? PorcentajeDelTotal = null, double? ConexionesPorUsuario = null);
public record FilaMaestro(string Nombre, string Escuela, int Conexiones, double? PorcentajeDelTotal = null);
public record FilaComparacion(string Nombre, int PruebasCompletadasA, int PruebasCompletadasB, int Diferencia, double? VariacionPct);
public record FilaTendencia(string Periodo, int Conexiones, int? DiferenciaConPeriodoAnterior = null, double? VariacionPct = null);
public record FilaComparacionEscuela(string Nombre, int ConexionesA, int ConexionesB, int Diferencia, double? VariacionPct);
public record FilaCatalogoEscuela(string Nombre);
/// <summary>Promedio de % Full Quiz. Escuela/Materia son null cuando no se agrupa por ese eje.</summary>
public record FilaPromedioQuiz(string? Escuela, string? Materia, int PruebasAsignadas, int PruebasCompletadas,
    double? PromedioSoloRealizadas, double PromedioTodasLasAsignadas);

/// <summary>
/// Respuesta común a las 5 consultas. Lleva las REFERENCIAS que el agente debe citar:
/// qué consulta se ejecutó, sobre qué columna de fecha se filtró, el período y los filtros aplicados,
/// hasta qué día llegan los datos cargados, cuántas filas salieron y cuándo se generó.
/// </summary>
public record Respuesta(string Consulta, string Fuente, string ColumnaFecha, object Periodo, object Filtros,
    DateOnly? DatosHasta, object Resultados, string? Nota = null, object? Resumen = null)
{
    public int TotalFilas => Resultados is System.Collections.ICollection c ? c.Count : 0;
    public DateTime GeneradoEn { get; } = DateTime.UtcNow;
}

/// <summary>Un parámetro falta o no es válido: se responde 400 con el mensaje.</summary>
public class ErrorConsulta(string mensaje) : Exception(mensaje);

/// <summary>
/// Las 5 consultas de solo lectura que usa el agente. Hoy hay una implementación con datos
/// inventados (ConsultasEjemplo); la de MySQL (gs_data_analysis) irá detrás de esta misma interfaz.
/// </summary>
public interface IConsultasUso
{
    string Fuente { get; }
    string? NotaArea { get; }
    /// <summary>Columna sobre la que se filtra el período en las consultas de pruebas (se cita en la respuesta).</summary>
    string ColumnaFechaPruebas { get; }
    /// <summary>Columna sobre la que se filtra el período en las consultas de conexiones (se cita en la respuesta).</summary>
    string ColumnaFechaConexiones { get; }
    /// <summary>Último día con datos de pruebas; si el período pedido llega más allá, la API lo avisa.</summary>
    DateOnly? DatosHastaPruebas { get; }
    /// <summary>Último día con datos de conexiones (puede ser distinto al de pruebas).</summary>
    DateOnly? DatosHastaConexiones { get; }
    /// <summary>Cómo leer las cifras de conexiones (definición de «conexión», alcance). Se devuelve como nota.</summary>
    string? NotaConexiones { get; }
    /// <summary>true si se excluyen las cuentas internas y de prueba; se declara en cada respuesta.</summary>
    bool ExcluyeCuentasDePrueba { get; }
    bool EscuelaExiste(string escuela);
    /// <summary>Escuelas cuyo nombre contiene el texto (sin distinguir mayúsculas ni tildes). Sirve para que el agente use el nombre exacto.</summary>
    List<FilaCatalogoEscuela> BuscarEscuelas(string texto, int limite);
    List<FilaArea> UsoPorArea(DateOnly desde, DateOnly hasta, bool desc, int limite);
    List<FilaEscuela> ConexionesPorEscuela(DateOnly desde, DateOnly hasta, bool desc, int limite);
    List<FilaMaestro> ConexionesPorMaestro(DateOnly desde, DateOnly hasta, bool desc, int limite, string? escuela);
    List<FilaComparacion> CompararAreas(DateOnly desdeA, DateOnly hastaA, DateOnly desdeB, DateOnly hastaB);
    List<FilaTendencia> Tendencia(DateOnly desde, DateOnly hasta, string granularidad, string? escuela);
    /// <summary>Cómo leer el promedio de % Full Quiz (definición, escala, cohorte). Se devuelve como nota.</summary>
    string? NotaPromedioQuiz { get; }
    /// <summary>agrupar: «escuela», «materia» o «escuela_materia».</summary>
    List<FilaPromedioQuiz> PromedioFullQuiz(DateOnly desde, DateOnly hasta, string agrupar, bool desc, int limite, string? escuela);
}
