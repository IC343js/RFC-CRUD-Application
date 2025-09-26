using Business;
using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRFC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult IrGenerarRFC()
        {
            return View("GenerarRFC");
        }
        public ActionResult IrBaseDeDatos()
        {
            N_Rfc negocio = new N_Rfc();
            List<Ent_Rfc> rfcs = new List<Ent_Rfc>();
            try
            {
                rfcs = negocio.ObtenerTodos();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            
            return View("BaseDeDatos", rfcs);
        }
        public ActionResult IrEditar(int idRfc)
        {
            D_Rfc dao = new D_Rfc();
            Ent_Rfc rfc = dao.ObtenerRfcPorId(idRfc);
            return View("VistaEditar", rfc);
        }
        
        public ActionResult IrEliminar(int idRfc)
        {
            D_Rfc dao = new D_Rfc();
            Ent_Rfc rfc = dao.ObtenerRfcPorId(idRfc);
            return View("VistaEliminar", rfc);
        }
        public ActionResult EliminarRFC(int idRfc)
        {
            try
            {
                N_Rfc negocio = new N_Rfc();
                negocio.EliminarRFC(idRfc);
                TempData["mensaje"] = $"El RFC se eliminó correctamente.";
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("IrBaseDeDatos");
        }
        public ActionResult RegistrarRFC(Ent_Rfc rfc)
        {
            N_Rfc negocio = new N_Rfc();

            try
            {
                // Basic validations - Note: ApellidoMaterno is now optional
                if (string.IsNullOrWhiteSpace(rfc.Nombre) ||
                    string.IsNullOrWhiteSpace(rfc.ApellidoPaterno) ||
                    rfc.FechaNacimiento == default(DateTime))
                {
                    TempData["error"] = "Los campos Nombre, Apellido Paterno y Fecha de Nacimiento son obligatorios.";
                    return View("GenerarRFC", rfc);
                }

                // Handle empty ApellidoMaterno - set to empty string to avoid null issues
                if (string.IsNullOrWhiteSpace(rfc.ApellidoMaterno))
                {
                    rfc.ApellidoMaterno = "";
                }

                // Generate RFC according to specifications
                rfc.Rfc = GenerarRFC(rfc.Nombre, rfc.ApellidoPaterno, rfc.ApellidoMaterno, rfc.FechaNacimiento);

                // Insert into database
                negocio.AgregarRFC(rfc);

                // Pass the generated RFC to the view
                ViewBag.RfcGenerado = rfc.Rfc;
                TempData["success"] = "RFC generado exitosamente: " + rfc.Rfc;

                return View("MenuRFC", rfc);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al generar RFC: " + ex.Message;
                return View("GenerarRFC", rfc);
            }
        }

        private string GenerarRFC(string nombre, string apellidoPaterno, string apellidoMaterno, DateTime fechaNacimiento)
        {
            // Clean and normalize inputs
            nombre = LimpiarTexto(nombre);
            apellidoPaterno = LimpiarTexto(apellidoPaterno);
            apellidoMaterno = LimpiarTexto(apellidoMaterno); // This will handle null/empty

            // Handle compound names (MARIA, MA, JOSE, J exceptions)
            nombre = ManejarNombreCompuesto(nombre);

            // Handle compound surnames (use only first word)
            apellidoPaterno = ObtenerPrimeraPalabra(apellidoPaterno);
            apellidoMaterno = ObtenerPrimeraPalabra(apellidoMaterno);

            string rfc = "";

            // 1. First letter of first surname - Handle Ñ exception
            if (apellidoPaterno.Length > 0)
            {
                char primeraLetra = apellidoPaterno[0];
                rfc += primeraLetra == 'Ñ' ? "X" : primeraLetra.ToString();
            }
            else
            {
                rfc += "X";
            }

            // 2. First internal vowel of first surname
            rfc += ObtenerPrimeraVocalInterna(apellidoPaterno);

            // 3. First letter of second surname - Handle both Ñ and empty apellido materno exceptions
            if (string.IsNullOrWhiteSpace(apellidoMaterno))
            {
                // Exception: If no second surname, assign X in third position
                rfc += "X";
            }
            else
            {
                char letra = apellidoMaterno[0];
                // Exception: If first letter is Ñ, assign X
                rfc += letra == 'Ñ' ? "X" : letra.ToString();
            }

            // 4. First letter of name - Handle Ñ exception
            if (nombre.Length > 0)
            {
                char letra = nombre[0];
                // Exception: If first letter is Ñ, assign X
                rfc += letra == 'Ñ' ? "X" : letra.ToString();
            }
            else
            {
                rfc += "X";
            }

            // Check for inappropriate words and replace if necessary
            rfc = ValidarPalabraInapropiada(rfc);

            // 5. Add birth date (YYMMDD)
            rfc += fechaNacimiento.ToString("yyMMdd");

            return rfc.ToUpperInvariant();
        }

        private string LimpiarTexto(string texto)
        {
            // Handle null or empty strings
            if (string.IsNullOrWhiteSpace(texto)) return "";

            // Remove accents and normalize
            string[] acentos = { "ÁÉÍÓÚÜ", "AEIOUU" };
            for (int i = 0; i < acentos[0].Length; i++)
            {
                texto = texto.Replace(acentos[0][i], acentos[1][i]);
            }

            return texto.ToUpperInvariant().Trim();
        }

        public ActionResult EditarRFC(Ent_Rfc rfc)
        {
            try
            {
                // Basic validations - Note: ApellidoMaterno is now optional
                if (string.IsNullOrWhiteSpace(rfc.Nombre) ||
                    string.IsNullOrWhiteSpace(rfc.ApellidoPaterno) ||
                    rfc.FechaNacimiento == default(DateTime))
                {
                    TempData["error"] = "Los campos Nombre, Apellido Paterno y Fecha de Nacimiento son obligatorios.";
                    return View("VistaEditar", rfc);
                }

                // Handle empty ApellidoMaterno
                if (string.IsNullOrWhiteSpace(rfc.ApellidoMaterno))
                {
                    rfc.ApellidoMaterno = "";
                }

                // Regenerate RFC with the updated data using the same logic as RegistrarRFC
                rfc.Rfc = GenerarRFC(rfc.Nombre, rfc.ApellidoPaterno, rfc.ApellidoMaterno, rfc.FechaNacimiento);

                // Update the record in database
                D_Rfc dao = new D_Rfc();
                dao.EditarRFC(rfc, rfc.idRfc);

                TempData["mensaje"] = $"El RFC con id: {rfc.idRfc} se editó correctamente. Nuevo RFC: {rfc.Rfc}";

                // Get updated list to show in BaseDeDatos view
                return RedirectToAction("IrBaseDeDatos");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al actualizar RFC: " + ex.Message;
                return View("VistaEditar", rfc);
            }
        }

        private string ManejarNombreCompuesto(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "";

            string[] palabras = nombre.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (palabras.Length > 1)
            {
                // If first word is MARIA, MA, JOSE, or J, use second word
                string primeraPalabra = palabras[0];
                if (primeraPalabra == "MARIA" || primeraPalabra == "MA" ||
                    primeraPalabra == "JOSE" || primeraPalabra == "J")
                {
                    return palabras.Length > 1 ? palabras[1] : primeraPalabra;
                }
            }

            return palabras[0];
        }

        private string ObtenerPrimeraPalabra(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";

            string[] palabras = texto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return palabras.Length > 0 ? palabras[0] : "";
        }

        private string ObtenerPrimeraVocalInterna(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido) || apellido.Length <= 1) return "X";

            string vocales = "AEIOU";

            // Search for internal vowels (skip first letter)
            for (int i = 1; i < apellido.Length; i++)
            {
                if (vocales.Contains(apellido[i]))
                {
                    return apellido[i].ToString();
                }
            }

            return "X"; // No internal vowel found
        }

        private string ValidarPalabraInapropiada(string rfc)
        {
            string[] palabrasInapropiadas = {
        "BUEI", "BUEY", "CACA", "CACO", "CAGA", "CAGO", "CAKA", "CAKO",
        "COGE", "COJA", "COJE", "COJI", "COJO", "CULO", "FETO", "GUEY",
        "JOTO", "KACA", "KACO", "KAGA", "KAGO", "KOGE", "KOJO", "KAKA",
        "KULO", "MAME", "MAMO", "MEAR", "MEAS", "MEON", "MION", "MOCO",
        "MULA", "PEDA", "PEDO", "PENE", "PUTA", "PUTO", "QULO", "RATA"
    };

            // Check if first 4 letters form an inappropriate word
            if (rfc.Length >= 4)
            {
                string primeras4 = rfc.Substring(0, 4);
                if (palabrasInapropiadas.Contains(primeras4))
                {
                    // Replace 4th character with X
                    return rfc.Substring(0, 3) + "X" + (rfc.Length > 4 ? rfc.Substring(4) : "");
                }
            }

            return rfc;
        }
        // Replace your current Buscar method in HomeController with this:
        public ActionResult Buscar(string texto)
        {
            N_Rfc negocio = new N_Rfc();
            List<Ent_Rfc> lista = new List<Ent_Rfc>();

            try
            {
                // Get all RFCs first
                List<Ent_Rfc> todosLosRfcs = negocio.ObtenerTodos();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    // If search is empty, return all records
                    lista = todosLosRfcs;
                }
                else
                {
                    // Search logic: search in names, surnames, dates, and RFC
                    string textoBusqueda = texto.ToUpperInvariant().Trim();

                    lista = todosLosRfcs.Where(rfc =>
                        // Search in full name
                        (rfc.Nombre + " " + rfc.ApellidoPaterno + " " + rfc.ApellidoMaterno).ToUpperInvariant().Contains(textoBusqueda) ||

                        // Search in individual name parts
                        rfc.Nombre.ToUpperInvariant().Contains(textoBusqueda) ||
                        rfc.ApellidoPaterno.ToUpperInvariant().Contains(textoBusqueda) ||
                        (!string.IsNullOrEmpty(rfc.ApellidoMaterno) && rfc.ApellidoMaterno.ToUpperInvariant().Contains(textoBusqueda)) ||

                        // Search in RFC
                        rfc.Rfc.ToUpperInvariant().Contains(textoBusqueda) ||

                        // Search in birth date (multiple formats)
                        rfc.FechaNacimiento.ToString("dd/MM/yyyy").Contains(textoBusqueda) ||
                        rfc.FechaNacimiento.ToString("yyyy-MM-dd").Contains(textoBusqueda) ||
                        rfc.FechaNacimiento.ToString("dd/MMM/yyyy").ToUpperInvariant().Contains(textoBusqueda) ||
                        rfc.FechaNacimiento.Year.ToString().Contains(textoBusqueda) ||
                        rfc.FechaNacimiento.Month.ToString().Contains(textoBusqueda) ||
                        rfc.FechaNacimiento.Day.ToString().Contains(textoBusqueda)

                    ).ToList();

                    // Set search message
                    if (lista.Count == 0)
                    {
                        TempData["mensaje"] = $"No se encontraron resultados para: '{texto}'";
                    }
                    else
                    {
                        TempData["mensaje"] = $"Se encontraron {lista.Count} resultado(s) para: '{texto}'";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al buscar: " + ex.Message;
                lista = new List<Ent_Rfc>();
            }

            return View("BaseDeDatos", lista);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}