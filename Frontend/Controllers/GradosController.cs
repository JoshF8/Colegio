using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models;
using Newtonsoft.Json;
using System.Text;

namespace Frontend.Controllers
{
    public class GradosController : Controller {
        //Se usara un cliente Http para consultar al Back
        private HttpClient apiClient;
        private string apiLink = "";
        //Para obtener datos del appsettings
        private IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        private IConfiguration configuration;
        public GradosController(FrontendAlumno context) {
            //_context = context;
            configuration = configurationBuilder.Build();
            apiClient = new HttpClient();
            apiLink = configuration.GetValue<string>("VARS:apiBack");
        }

        // GET: Grados
        public async Task<IActionResult> Index() {
            try {
                List<Grado> Grados =  await getAllGrados(apiClient, apiLink);
                return View(Grados);
            } catch (Exception) {
                return NotFound();
            }
        }

        // GET: Grados/Details/5
        public async Task<IActionResult> Details(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }
                Grado Grado = await getGrado(id);
                return View(Grado);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Grados/Create
        public IActionResult Create() {
            TempData["Profesors"] = getProfesors().Result;
            return View();
        }

        // POST: Grados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,ProfesorID,ProfesorNombre")] Grado Grado) {
            try {
                if (ModelState.IsValid) {
                    if (Grado.ProfesorID == 0) {
                        return RedirectToAction(nameof(Index));
                    }

                    //Convertir el Grado en un string, de tipo json, para enviarlo al backend con este formato
                    var Gradostring = JsonConvert.SerializeObject(Grado);
                    StringContent content = new StringContent(Gradostring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PutAsync($"{apiLink}/Grado/New", content);
                    response.EnsureSuccessStatusCode();
                }
                return RedirectToAction(nameof(Index));
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Grados/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }


                TempData["Profesors"] = getProfesors().Result;
                Grado Grado = await getGrado(id);
                return View(Grado);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Grados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,ProfesorID,ProfesorNombre")] Grado Grado) {
            if (id != Grado.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    if (Grado.ProfesorID == 0) {
                        return RedirectToAction(nameof(Index));
                    }
                    //Convertir el Grado en un string, de tipo json, para enviarlo al backend con este formato
                    var Gradostring = JsonConvert.SerializeObject(Grado);
                    StringContent content = new StringContent(Gradostring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PatchAsync($"{apiLink}/Grado/Edit", content);
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                    
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Grado);
        }

        // GET: Grados/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }
                Grado Grado = await getGrado(id);
                return View(Grado);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Grados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            if (ModelState.IsValid) {
                try {

                    HttpResponseMessage response = await apiClient.DeleteAsync($"{apiLink}/Grado/Delete/{id}");
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        

        //void para obtener solo un grado por su id
        private async Task<Grado> getGrado(int? id) {
            try {
                Grado Grado = new Grado();
                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/Grado/Get/{id}");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                //Convertir la respuesta con el modelo
                Grado = JsonConvert.DeserializeObject<Grado>(response.Content.ReadAsStringAsync().Result);
                return Grado;
            } catch (Exception e) {

                throw e;
            }
        }

        //void para buscar todos los profesores, para no ingresar valores de profesores que no existan
        private async Task<Dictionary<int, string>> getProfesors(){
            try {
                Dictionary<int, string> profesores = new Dictionary<int, string>();
                List<Profesor> profesoresList = await ProfesorsController.getAllProfesors(apiClient, apiLink);
                foreach (var item in profesoresList) {
                    profesores.Add(item.ID, $"{item.Nombre} {item.Apellidos}");
                }
                return profesores;
            } catch (Exception) {
                throw;
            }
        }

        //Metodo que obtiene todos los Grados, publico por si otros controllers lo necesitan
        public static async Task<List<Grado>> getAllGrados(HttpClient apiClient, string apiLink) {
            try {
                List<Grado> Grados = new List<Grado> { };

                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/Grado/GetAll");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                Grados = JsonConvert.DeserializeObject<List<Grado>>(response.Content.ReadAsStringAsync().Result);
                return Grados;
            } catch (Exception) {

                throw;
            }
        }
    }
}
