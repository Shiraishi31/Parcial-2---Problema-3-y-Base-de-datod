namespace Punto3_LoginBCrypt.Modelos;

/// <summary>
/// Representa una fila de la tabla dbo.IntentosLogin (historial de accesos).
/// </summary>
public class IntentoLogin
{
    public string NombreUsuarioIngresado { get; set; } = "";
    public bool Exitoso { get; set; }
    public DateTime FechaIntento { get; set; }
    public string? Detalle { get; set; }
}
