using ApiHotelBeach.Models;
using AppWebHotelBeach.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace AppWebHotelBeach.Controllers
{
    public class ReservaController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private static string direccionAPI = "https://localhost:7125/api/reservas";


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool validacion = await validarRol();
                if (validacion)
                {
                    var reservas = await extraerReservas();
                    return View(reservas);
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

        //-----------------------------------------------------------------

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reserva reserva, string listaPaquete, string ListaMetodoPago, string ListaBanco)
        {
            int validacion = 0;
            
            if (reserva != null)
            {
                List<Reserva> temp = await extraerReservas();
                foreach (var item in temp)
                {
                    if (reserva.NumeroHabitacion == item.NumeroHabitacion)
                    {
                        validacion = -1;
                        TempData["Mensaje"] = "No se logro crear la reserva..";
                    }
                }


                if(validacion == 0)
                {
                    if (ListaBanco != "Seleccione el Banco")
                    {
                        reserva.BancoCheque = ListaBanco;
                    }

                    reserva.MetodoPago = ListaMetodoPago;

                    var ListaPaquetes = await extraerPaquetes();
                    Paquete tempPaquete = ListaPaquetes.FirstOrDefault(p => p.TipoPaquete == listaPaquete);

                    var ListaClientes = await extraerClientes();
                    Cliente tempCliente = ListaClientes.FirstOrDefault(c => c.Email == User.FindFirst(ClaimTypes.Name)?.Value);

                    DatosTipoCambioAPI DatosTipoCambio = await obtenerTipoCambioAPI();
                    reserva.CalcularFactura(DatosTipoCambio.venta, tempPaquete, tempCliente);

                    try
                    {
                        var response = await client.PostAsync(direccionAPI, new StringContent(JsonConvert.SerializeObject(reserva), Encoding.UTF8, "application/json"));

                        this.EnviarEmail(reserva, tempPaquete, tempCliente);
                    }
                    catch (Exception ex)
                    {
                        TempData["Mensaje"] = "No se logro crear la reserva.." + ex.Message;
                    }
                    return RedirectToAction("Index", "Reserva");
                }
                return View();
                
            }
            else
            {
                return View();
            }
        }

        //-----------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Reserva temp = await extraerReservas(id);
            return View(temp);
        }

        [HttpPost]
        public async Task<IActionResult>Delete(int? id)
        {
            await client.DeleteAsync(direccionAPI + "/" + id);
            return RedirectToAction("Index");
        }

        

        //------------------------------------------------------------------
        private async Task<List<Reserva>> extraerReservas()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(direccionAPI);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var reservas = JsonConvert.DeserializeObject<List<Reserva>>(result);
                    return reservas;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<Reserva> extraerReservas(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(direccionAPI + "/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var reserva = JsonConvert.DeserializeObject<Reserva>(result);
                    return reserva;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<List<Paquete>> extraerPaquetes()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7125/api/paquetes");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var paquetes = JsonConvert.DeserializeObject<List<Paquete>>(result);
                    return paquetes;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        private async Task<List<Cliente>> extraerClientes()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7125/api/clientes");

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

        private async Task<DatosTipoCambioAPI> obtenerTipoCambioAPI()
        {          
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://apis.gometa.org/tdc/tdc.json");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var tipoCambio = JsonConvert.DeserializeObject<DatosTipoCambioAPI>(result);
                    return tipoCambio;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;

        }

        private void EnviarEmail(Reserva reserva, Paquete paquete, Cliente cliente)
        {
            try
            {
                EmailReserva email = new EmailReserva();
                email.Enviar(reserva, paquete, cliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            if (rol.NombreRole == "Admin")
            {
                validar = true;
            }
            return validar;
        }








    }
}
