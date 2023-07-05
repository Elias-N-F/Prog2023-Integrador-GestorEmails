// See https://aka.ms/new-console-template for more information
using ConsoleAppEmail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

bool exit= false;
string? email = string.Empty;
string? pass= string.Empty;
Console.Clear();
Llamadas.Config();

// Console.WriteLine(EncryptPassword("contra","jose@correo.com")); //Fm7BZJZ464J5CVm0AuVZRuZY6d7XkGQ1v0bpovOus+w=
while (true) {
    Console.WriteLine("Email:");
    email = Console.ReadLine();
    Console.WriteLine("Contraseña:");
    pass = Console.ReadLine();

    if (Llamadas.Login(email,pass))
    {
        Console.WriteLine("Inicio de sesion correcto");
        break;
    }

}

while (exit == false)
{
    Console.WriteLine("Ingrese un comando:");
    switch (Console.ReadLine())
    {
        case "usuarios":
            Llamadas.GetPersonas();
            break;
        case "correos":
            Llamadas.GetCorreos();
            break;
        case "usuario":
            Console.WriteLine("Ingrese el id del usuario");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Llamadas.GetPersona(id, true);
            }
            catch (Exception)
            {
                Console.WriteLine("Error, intente nuevamente");
            }
            break;
        case "correo":
            Console.WriteLine("Ingrese el id del correo");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Llamadas.GetCorreo(id);
            }
            catch (Exception)
            {
                Console.WriteLine("Error, intente nuevamente");
            }
            break;
        case "crearusuario":

            Console.WriteLine("Ingrese el email del usuario");
            string e = Console.ReadLine();
            if (!Regex.IsMatch(email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
            {
                Console.WriteLine("El email ingresado es incorrecto, intente nuevamente");
                break;
            }
            Console.WriteLine("Ingrese el nombre del usuario:");
            string nombre = Console.ReadLine();
            Console.WriteLine("Ingrese la contraseña del usuario:");
            string p = Console.ReadLine();

            try
            {
                Llamadas.CreatePersona(nombre,e,p);
            }
            catch (Exception)
            {
                Console.WriteLine("Error, intente nuevamente");
            }
            break;
        case "borrarusuario":
            Console.WriteLine("Ingrese el id del usuario");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Llamadas.DeletePersona(id);
            }
            catch (Exception)
            {
                Console.WriteLine("Error, intente nuevamente");
            }
            break;
        case "ayuda":
            Console.WriteLine("Comandos:");
            Console.WriteLine("usuarios: trae una lista de todos los usuarios");
            Console.WriteLine("correos: trae una lista de todos los correos");
            Console.WriteLine("usuario: trae un usuario segun id");
            Console.WriteLine("correo: trae un correo segun id");
            Console.WriteLine("crearusuario: crea un nuevo usuario");
            Console.WriteLine("borrarusuario: borra un usuario segun id");
            Console.WriteLine("salir: cierra el cliente");
            break;
        case "salir":
            exit = true;
            break;
        default:
            Console.WriteLine("Comandos: usuarios, correos, usuario, correo,");
            Console.WriteLine("crearusuario, borrarusuario, ayuda, salir");
            break;
    }

}

static string EncryptPassword(string password, string username)
{
    using (var sha256 = SHA256.Create())
    {
        var saltedPassword = string.Format("{0}{1}", username, password);
        byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
        return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
    }
}