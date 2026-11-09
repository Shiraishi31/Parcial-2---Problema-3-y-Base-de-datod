using MySql.Data.MySqlClient;
using Punto3_LoginBCrypt.Modelos;

namespace Punto3_LoginBCrypt.Datos;

/// <summary>
/// Acceso a la tabla Usuarios.
/// Todas las consultas usan PARÁMETROS (@NombreUsuario...) para evitar inyección SQL.
/// </summary>
public class UsuarioRepositorio
{
    public bool Existe(string nombreUsuario)
    {
        const string sql = "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario;";

        using MySqlConnection cn = Conexion.Crear();
        using MySqlCommand cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

        cn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    /// <summary>Guarda el usuario con su HASH (nunca la contraseña) y devuelve el Id generado.</summary>
    public int Insertar(string nombreUsuario, string nombreCompleto, string passwordHash)
    {
        const string sql = @"INSERT INTO Usuarios (NombreUsuario, NombreCompleto, PasswordHash)
                             VALUES (@NombreUsuario, @NombreCompleto, @PasswordHash);";

        using MySqlConnection cn = Conexion.Crear();
        using MySqlCommand cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
        cmd.Parameters.AddWithValue("@NombreCompleto", nombreCompleto);
        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

        cn.Open();
        cmd.ExecuteNonQuery();
        return (int)cmd.LastInsertedId;   // el Id AUTO_INCREMENT que MySQL acaba de generar
    }

    /// <summary>Busca un usuario por su nombre de usuario. Devuelve null si no existe.</summary>
    public Usuario? BuscarPorNombreUsuario(string nombreUsuario)
    {
        const string sql = @"SELECT Id, NombreUsuario, NombreCompleto, PasswordHash
                             FROM Usuarios
                             WHERE NombreUsuario = @NombreUsuario;";

        using MySqlConnection cn = Conexion.Crear();
        using MySqlCommand cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

        cn.Open();
        using MySqlDataReader reader = cmd.ExecuteReader();

        if (!reader.Read())
            return null;

        return new Usuario
        {
            Id = reader.GetInt32(0),
            NombreUsuario = reader.GetString(1),
            NombreCompleto = reader.GetString(2),
            PasswordHash = reader.GetString(3)
        };
    }
}
