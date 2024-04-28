using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AppWebHotelBeach.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AppWebHotelBeach.Controllers
{
    public class ClienteController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private static string direccionAPI = "https://localhost:7125/api/clientes";

        private static string EmailRestablecer = "";

        //Views
        [HttpGet]
        public async Task<IActionResult> Index()
        {            
            if (User.Identity.IsAuthenticated)
            {
                bool validacion = await validarRol();
                if (validacion)
                {
                    var clientes = await extraerClientes();
                    return View(clientes);
                }
                else
                {
                    return RedirectToAction("VistaError", "Cliente");
                }                
            }
            else
            {
                return RedirectToAction("Login", "Cliente");
            }            
        }

        [HttpGet]
        public IActionResult VistaError()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await extraerClientes(id);

            if (cliente != null)
            {
                return View(cliente);
            }
            return NotFound();
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await extraerClientes(id);

            if (cliente != null)
            {
                return View(cliente);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CrearCuenta()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Restablecer(string? Email)
        {
            var temp = await extraerClientes();
            var cliente = temp.First(cliente => cliente.Email.Equals(Email));

            SeguridadRestablecer restablecer = new SeguridadRestablecer();

            restablecer.Email = cliente.Email;
            restablecer.Password = cliente.Password;

            EmailRestablecer = cliente.Email;

            return View(restablecer);
        }

        //------------------------------------------------------------------------------------
        //Metodos CRUD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            await client.DeleteAsync(direccionAPI + "/" + id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cliente cliente, string listaRoles)
        {
            var temp = await extraerClientes(cliente.ID);
            cliente.Password = temp.Password;
            cliente.FechaRegistro = temp.FechaRegistro;

            if(listaRoles != "Rol")
            {
                cliente.RoleID = int.Parse(listaRoles);
            }
            else
            {
                cliente.RoleID = temp.RoleID;
            }

            await client.PutAsync(direccionAPI, new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json"));
            return RedirectToAction("Index");
        }

        //Devuelve una lista de clientes traida de la API
        private async Task<List<Cliente>> extraerClientes()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(direccionAPI);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var clientes = JsonConvert.DeserializeObject<List<Cliente>>(result);
                    return clientes;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        //devuelve un cliente por medio de un ID
        private async Task<Cliente> extraerClientes(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(direccionAPI + "/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var cliente = JsonConvert.DeserializeObject<Cliente>(result);
                    return cliente;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<DatosClienteAPI> extraerDatosCliente(string cedula)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://apis.gometa.org/cedulas/" + cedula);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    DatosClienteAPI datosClienteAPI = JsonConvert.DeserializeObject<DatosClienteAPI>(result);                  

                    return datosClienteAPI;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<List<Roles>> extraerRoles()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7125/api/roles");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var roles = JsonConvert.DeserializeObject<List<Roles>>(result);
                    return roles;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<bool> validarRol()
        {
            bool validar = false;
            
            var ListaClientes = await extraerClientes();
            Cliente? tempCliente = ListaClientes.FirstOrDefault(c => c.Email == User.FindFirst(ClaimTypes.Name)?.Value);

            var ListaRoles = await extraerRoles();
            Roles rol = ListaRoles.FirstOrDefault(r => r.RoleID == tempCliente.RoleID);

            if(rol.NombreRole == "Admin")
            {
                validar = true;
            }
            return validar;
        } 



        //------------------------------------------------------------------------------------
        //Procesos de autenticacion

        private async Task<Cliente> validarCliente(Cliente temp)
        {
            Cliente autorizado = null;

            var clienteLista = await extraerClientes();

            var user = clienteLista.FirstOrDefault(u => u.Email == temp.Email);

            if (user != null)
            {
                if (user.Password.Equals(temp.Password))
                {
                    autorizado = user;
                }
            }
            return autorizado;
        }

        private async Task<bool> verificarRestablecer(Cliente temp)
        {
            bool verificado = false;

            var clienteLista = await extraerClientes();

            var user = clienteLista.FirstOrDefault(u => u.Email == temp.Email);

            if (user != null)
            {
                if (user.Restablecer == 0)
                {
                    verificado = true;
                }
            }
            return verificado;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Cliente cliente)
        {
            var temp = await this.validarCliente(cliente);

            if (temp != null)
            {
                bool restablecer = false;

                restablecer = await verificarRestablecer(temp);

                if (restablecer)
                {
                    return RedirectToAction("Restablecer", "Cliente", new { Email = temp.Email });
                }
                else
                {
                    var clienteClaims = new List<Claim>() { new Claim(ClaimTypes.Name, temp.Email) };

                    var grandmaIdentity = new ClaimsIdentity(clienteClaims, "User Identity");

                    var clientePrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                    HttpContext.SignInAsync(clientePrincipal);

                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["Mensaje"] = "Email o password incorrecto";
            return View(cliente);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        private string GenerarClave()
        {
            Random random = new Random();
            string clave = string.Empty;
            clave = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(clave, 12).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool EnviarEmail(Cliente temp)
        {
            try
            {
                bool enviado = false;
                Email email = new Email();
                email.Enviar(temp);
                enviado = true;

                return enviado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCuenta(Cliente cliente)
        {
            if (cliente != null)
            {
                var temp = await extraerDatosCliente(cliente.Cedula);

                Result datos = temp.results[0];
                cliente.NombreCliente = datos.Fullname;
                cliente.TipoCedula = datos.Guess_Type;
                cliente.FechaRegistro = DateTime.Now;
                cliente.Password = this.GenerarClave();
                cliente.RoleID = 2;

                try
                {
                    await client.PostAsync(direccionAPI, new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json"));

                    if (this.EnviarEmail(cliente))
                    {
                        TempData["MensajeCreado"] = "Usuario creado correctamente, Su contraseña fue enviada por email";
                    }
                    else
                    {
                        TempData["MensajeCreado"] = "Usuario creado pero no se envio el email, comuniquese con el administrador;";
                    }
                }
                catch (Exception ex)
                {
                    TempData["MensajeError"] = "No se logro crear la cuenta.." + ex.Message;
                }
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restablecer(SeguridadRestablecer pRestablecer)
        {
            if (pRestablecer != null)
            {
                var temp = await extraerClientes();
                var cliente = temp.First(c => c.Email == EmailRestablecer);

                if (cliente.Password.Equals(pRestablecer.Password))
                {
                    if (pRestablecer.NuevoPassword.Equals(pRestablecer.Confirmar))
                    {
                        cliente.Password = pRestablecer.Confirmar;
                        cliente.Restablecer = 1;

                        await client.PutAsync(direccionAPI, new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json"));
                      
                        return RedirectToAction("Login", "Cliente");
                    }
                    else
                    {
                        TempData["MensajeError"] = "Las contraseñas no coinciden";
                        return View(pRestablecer);
                    }
                }
                else
                {
                    TempData["MensajeError"] = "La contraseña es incorrecta";
                    return View(pRestablecer);
                }
            }
            else
            {
                TempData["MensajeError"] = "Datos incorrectos";
                return View(pRestablecer);
            }

        }



    }//fn controller
}//fn namespace
