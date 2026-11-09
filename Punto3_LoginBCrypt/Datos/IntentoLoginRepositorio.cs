using MySql.Data.MySqlClient;
using Punto3_LoginBCrypt.Modelos;

namespace Punto3_LoginBCrypt.Datos;

/// <summary>
/// Acceso a la tabla IntentosLogin: guarda cada intento (exitoso o fallido)
/// con el usuario y la fecha, para poder analizarlos después.
/// </summary>
public class IntentoLoginRepositorio
{
    /// <param name="usuarioId">Id del usuario, o null si el nombre de usuario no existe.</param>
    public void Registrar(int? usuarioId, string nombreUsuarioIngresado, bool exitoso, string detalle)
    {
        // FechaIntento no se envía: la pone MySQL con DEFAULT CURRENT_TIMESTAMP
        const string sql = @"INSERT INTO IntentosLogin (UsuarioId, NombreUsuarioIngresado, Exitoso, Detalle)
                             VALUES (@UsuarioId, @NombreUsuarioIngresado, @Exitoso, @Detalle);";

        using MySqlConnection cn = Conexion.Crear();
        using MySqlCommand cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@UsuarioId", (object?)usuarioId ?? DBNull.Value);   // null de C# -> NULL de SQL
        cmd.Parameters.AddWithValue("@NombreUsuarioIngresado", nombreUsuarioIngresado);
        cmd.Parameters.AddWithValue("@Exitoso", exitoso);
        cmd.Parameters.AddWithValue("@Detalle", detalle);

        cn.Open();
        cmd.ExecuteNonQuery();
    }

    /// <summary>Devuelve los últimos intentos, del más reciente al más antiguo.</summary>
    public List<IntentoLogin> ObtenerUltimos(int cantidad)
    {
        const string sql = @"SELECT NombreUsuarioIngresado, Exitoso, FechaIntento, Detalle
                             FROM IntentosLogin
                             ORDER BY FechaIntento DESC, Id DESC
                             LIMIT @Cantidad;";

        var lista = new List<IntentoLogin>();

        using MySqlConnection cn = Conexion.Crear();
        using MySqlCommand cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@Cantidad", cantidad);

        cn.Open();
        using MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new IntentoLogin
            {
                NombreUsuarioIngresado = reader.GetString(0),
                Exitoso = reader.GetBoolean(1),
                FechaIntento = reader.GetDateTime(2),
                Detalle = reader.IsDBNull(3) ? null : reader.GetString(3)
            });
        }

        return lista;
    }
}
