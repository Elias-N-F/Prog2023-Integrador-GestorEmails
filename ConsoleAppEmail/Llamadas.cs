using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleAppEmail.Modelos;

namespace ConsoleAppEmail
{
    public static class Llamadas
    {
        static private string baseurl = "http://localhost:5005/";
        private static CookieContainer cookieContainer = new CookieContainer();
        private static HttpClientHandler clienthandler =
                    new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        UseCookies = true,
                        CookieContainer = cookieContainer
                    };
        static private HttpClient client;
        static public void Config() {
            clienthandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clienthandler);
        }
        static public bool Login(string email, string password)
        {

            Persona persona = new Persona();
            persona.Password = password;
            persona.Email = email;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseurl+"api/login");
            request.Content = JsonContent.Create(persona);

            try
            {
                var response = client.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                if (response.StatusCode == HttpStatusCode.NotFound) 
                {
                    Console.WriteLine("Usuario o contraseña incorrectos");
                    return false; 
                }  

                return false;
            }
            catch (Exception)
            {
                Console.WriteLine("Error en el inicio de sesion");
                return false;
            }


        }
        static public void GetPersonas() {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseurl + "api/persona");
            try
            {
                var response = client.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    var personas = response.Content.ReadFromJsonAsync<List<Persona>>().Result;
                    if (personas !=null)
                    {
                        foreach (Persona p in personas)
                        {
                            Console.WriteLine(p.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron usuarios");
                    }
                    
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Error al traer personas");
            }
            Console.WriteLine("Listo!");
        }
        static public void GetCorreos() {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseurl + "api/correo");
            try
            {
                var response = client.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    var correos = response.Content.ReadFromJsonAsync<List<Correo>>().Result;
                    if (correos != null)
                    {
                        foreach (Correo c in correos)
                        {
                            Persona p = GetPersona(c.PersonaId, false);
                            c.Remitente = p;
                            Console.WriteLine(c.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron correos");
                    }

                }

            }
            catch (Exception)
            {
                Console.WriteLine("Error al traer correos");

            }
            Console.WriteLine("Listo!");
        }
        static public Persona GetPersona(int id, bool printpersona) {
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseurl + "api/persona/"+id);
            try
            {
                var response = client.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    var persona = response.Content.ReadFromJsonAsync<Persona>().Result;
                    if (persona != null)
                    {
                        if(printpersona){
                            Console.WriteLine(persona.ToString());
                        }
                        return persona;
                    }
                    else
                    {
                        Console.WriteLine("No se encontró el usuario");
                    }

                }

            }
            catch (Exception)
            {
                Console.WriteLine("Error al traer usuario");
            }
            Console.WriteLine("Listo!");
            return null;            
        }
        static public void GetCorreo(int id) {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseurl + "api/correo/" + id);
            try
            {
                var response = client.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    var correo = response.Content.ReadFromJsonAsync<Correo>().Result;
                    if (correo != null)
                    {
                        Console.WriteLine(correo.ToString());
                        Console.WriteLine("Contenido: ");
                        Console.WriteLine(correo.Contenido);
                    }
                    else
                    {
                        Console.WriteLine("No se encontró el correo");
                    }

                }

            }
            catch (Exception)
            {
                Console.WriteLine("Error al traer correo");
            }
            Console.WriteLine("Listo!");
        }
        static public void DeletePersona(int id) {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, baseurl + "api/persona/" + id);
            try
            {
                var response = client.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Eliminacion correcta");
                    return;
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No se encontró el usuario");
                }
                else {
                    Console.WriteLine("Error al borrar usuario");
                }


            }
            catch(Exception) {
                Console.WriteLine("Error al borrar usuario");
                return;
            }

        }
        static public void CreatePersona(string nombre, string email, string contra) {

            Persona persona = new Persona();
            persona.Password = contra;
            persona.Email = email;
            persona.Nombre = nombre;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseurl + "api/persona/");
            request.Content = JsonContent.Create(persona);

            try
            {
                var response = client.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Usuario creado con exito");
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Error del servidor al crear usuario");
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Error al crear usuario");
            }

        }



    }
}
