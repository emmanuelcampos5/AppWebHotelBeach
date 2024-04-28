using ApiHotelBeach.Models;
using AppWebHotelBeach.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppWebHotelBeach.Controllers
{
    public class PaqueteController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private static string direccionAPI = "https://localhost:7125/api/paquetes";

        //Views
        [HttpGet]
        public async Task<IActionResult> Index()
        {           
                var paquetes = await extraerPaquetes();
                return View(paquetes);                       
           
        }

        //-----------------------------------------------------------------

        private async Task<List<Paquete>> extraerPaquetes()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(direccionAPI);

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

        private async Task<Cliente> extraerPaquetes(int id)
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
    }
}
