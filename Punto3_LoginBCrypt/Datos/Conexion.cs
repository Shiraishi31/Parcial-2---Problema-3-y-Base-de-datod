using MySql.Data.MySqlClient;

namespace Punto3_LoginBCrypt.Datos;

/// <summary>
/// Un solo lugar para la cadena de conexión a MySQL.
/// Usa el usuario "parcial", que se crea con BaseDeDatos/03_CrearUsuarioParcial.sql.
/// Así no hace falta poner (ni subir a GitHub) la contraseña de root.
/// </summary>
public static class Conexion
{
    private const string CadenaConexion =
        "Server=localhost;Port=3306;Database=LoginBCryptDB;User ID=parcial;Password=Parcial123*;";

    public static MySqlConnection Crear()
    {
        return new MySqlConnection(CadenaConexion);
    }
}