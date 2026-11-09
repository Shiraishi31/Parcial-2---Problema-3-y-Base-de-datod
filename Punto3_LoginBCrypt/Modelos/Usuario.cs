namespace Punto3_LoginBCrypt.Modelos;

/// <summary>
/// Representa una fila de la tabla dbo.Usuarios.
/// Ojo: NO existe una propiedad "Password". Solo se guarda el hash.
/// </summary>
public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
    public string PasswordHash { get; set; } = "";
}
