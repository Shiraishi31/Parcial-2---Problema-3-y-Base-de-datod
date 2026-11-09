using System.Text;
using MySql.Data.MySqlClient;
using Punto3_LoginBCrypt.Datos;
using Punto3_LoginBCrypt.Modelos;
using Punto3_LoginBCrypt.Seguridad;

Console.OutputEncoding = Encoding.UTF8;   // para que se vean bien las tildes y la ñ

var usuarios = new UsuarioRepositorio();
var intentos = new IntentoLoginRepositorio();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("========= LOGIN CON BCRYPT =========");
    Console.WriteLine("1. Registrar usuario (genera el hash)");
    Console.WriteLine("2. Iniciar sesión");
    Console.WriteLine("3. Ver últimos intentos de acceso");
    Console.WriteLine("4. Salir");
    Console.Write("Elige una opción: ");

    string? opcion = Console.ReadLine();
    if (opcion is null) break;   // fin de la entrada

    try
    {
        switch (opcion.Trim())
        {
            case "1": RegistrarUsuario(); break;
            case "2": IniciarSesion(); break;
            case "3": VerIntentos(); break;
            case "4": return;
            default: Console.WriteLine("Opción no válida."); break;
        }
    }
    catch (MySqlException ex)
    {
        Console.WriteLine($"Error de base de datos: {ex.Message}");
        Console.WriteLine("Revisa que MySQL esté encendido y la contraseña en Datos/Conexion.cs.");
    }
}

// ---------------------------------------------------------------------------
// 1) REGISTRO: aquí se GENERA el hash con BCrypt y se guarda en la base de datos
// ---------------------------------------------------------------------------
void RegistrarUsuario()
{
    Console.WriteLine();
    Console.WriteLine("--- Registrar usuario ---");

    string nombreUsuario = LeerTexto("Nombre de usuario: ");
    if (usuarios.Existe(nombreUsuario))
    {
        Console.WriteLine("Ese nombre de usuario ya existe.");
        return;
    }

    string nombreCompleto = LeerTexto("Nombre completo: ");
    string password = LeerPassword("Contraseña: ");
    string confirmacion = LeerPassword("Confirma la contraseña: ");

    if (password.Length < 6)
    {
        Console.WriteLine("La contraseña debe tener al menos 6 caracteres.");
        return;
    }
    if (password != confirmacion)
    {
        Console.WriteLine("Las contraseñas no coinciden.");
        return;
    }

    string hash = ServicioPassword.GenerarHash(password);          // BCrypt.HashPassword
    int id = usuarios.Insertar(nombreUsuario, nombreCompleto, hash); // se guarda el HASH, no la contraseña

    Console.WriteLine($"Usuario registrado con Id {id}.");
    Console.WriteLine($"Hash guardado en la BD: {hash}");
}

// ---------------------------------------------------------------------------
// 2) LOGIN: aquí se VERIFICA la contraseña con BCrypt.Verify y se registra el intento
// ---------------------------------------------------------------------------
void IniciarSesion()
{
    Console.WriteLine();
    Console.WriteLine("--- Iniciar sesión ---");

    string nombreUsuario = LeerTexto("Usuario: ");
    string password = LeerPassword("Contraseña: ");

    Usuario? usuario = usuarios.BuscarPorNombreUsuario(nombreUsuario);

    if (usuario is null)
    {
        intentos.Registrar(null, nombreUsuario, false, "Usuario no existe");
        Console.WriteLine("Usuario o contraseña incorrectos.");
        return;
    }

    bool esCorrecta = ServicioPassword.Verificar(password, usuario.PasswordHash);   // BCrypt.Verify

    if (esCorrecta)
    {
        intentos.Registrar(usuario.Id, nombreUsuario, true, "Acceso concedido");
        Console.WriteLine();
        Console.WriteLine($"¡Acceso concedido! Bienvenido(a), {usuario.NombreCompleto}.");
    }
    else
    {
        intentos.Registrar(usuario.Id, nombreUsuario, false, "Contraseña incorrecta");
        Console.WriteLine("Usuario o contraseña incorrectos.");
    }
}

// ---------------------------------------------------------------------------
// 3) HISTORIAL: muestra los intentos guardados (para el análisis posterior)
// ---------------------------------------------------------------------------
void VerIntentos()
{
    List<IntentoLogin> lista = intentos.ObtenerUltimos(20);

    Console.WriteLine();
    Console.WriteLine($"{"Fecha",-21}{"Usuario",-16}{"Resultado",-11}Detalle");
    Console.WriteLine(new string('-', 70));

    foreach (IntentoLogin intento in lista)
    {
        string resultado = intento.Exitoso ? "EXITOSO" : "FALLIDO";
        Console.WriteLine($"{intento.FechaIntento,-21:yyyy-MM-dd HH:mm:ss}{intento.NombreUsuarioIngresado,-16}{resultado,-11}{intento.Detalle}");
    }

    if (lista.Count == 0)
        Console.WriteLine("Todavía no hay intentos registrados.");
}

// ---------------------------------------------------------------------------
// Funciones de apoyo para leer datos del teclado
// ---------------------------------------------------------------------------
static string LeerTexto(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string? linea = Console.ReadLine();
        if (linea is null) Environment.Exit(0);   // se cerró la entrada (Ctrl+Z): salimos en vez de repetir sin fin

        string texto = linea.Trim();
        if (texto.Length > 0) return texto;
        Console.WriteLine("Este campo es obligatorio.");
    }
}

// Lee la contraseña mostrando * en lugar de los caracteres
static string LeerPassword(string mensaje)
{
    Console.Write(mensaje);

    if (Console.IsInputRedirected)   // cuando la entrada viene de un archivo no se puede ocultar
        return Console.ReadLine() ?? "";

    var password = new StringBuilder();
    while (true)
    {
        ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

        if (tecla.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            break;
        }
        if (tecla.Key == ConsoleKey.Backspace)
        {
            if (password.Length > 0)
            {
                password.Length--;
                Console.Write("\b \b");   // borra el último * de la pantalla
            }
        }
        else if (!char.IsControl(tecla.KeyChar))
        {
            password.Append(tecla.KeyChar);
            Console.Write('*');
        }
    }
    return password.ToString();
}
