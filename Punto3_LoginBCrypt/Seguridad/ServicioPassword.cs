namespace Punto3_LoginBCrypt.Seguridad;

/// <summary>
/// Todo lo relacionado con BCrypt está aquí.
/// Se escribe BCrypt.Net.BCrypt (namespace.namespace.clase) a propósito:
/// si solo pones "using BCrypt.Net;" y luego "BCrypt.HashPassword(...)",
/// C# confunde el namespace BCrypt con la clase BCrypt y no compila.
/// </summary>
public static class ServicioPassword
{
    // Factor de trabajo (work factor / cost): 2^11 = 2048 rondas.
    // Más alto = más difícil de atacar por fuerza bruta, pero cada login tarda más.
    private const int FactorTrabajo = 11;

    /// <summary>Genera el hash BCrypt de una contraseña. La sal (salt) se crea sola y va dentro del hash.</summary>
    public static string GenerarHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, FactorTrabajo);
    }

    /// <summary>Compara la contraseña escrita con el hash guardado en la base de datos.</summary>
    public static bool Verificar(string password, string hashGuardado)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashGuardado);
    }
}
