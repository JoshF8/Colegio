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
    public class ProfesorsController : Controller
    {
        //Se usara un cliente Http para consultar al Back
        private HttpClient apiClient;
        private string apiLink = "";
        //Para obtener datos del appsettings
        private IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        private IConfiguration configuration;
        public ProfesorsController(FrontendAlumno context) {
            //_context = context;
            configuration = configurationBuilder.Build();
            apiClient = new HttpClient();
            apiLink = configuration.GetValue<string>("VARS:apiBack");
        }

        // GET: Profesors
        public async Task<IActionResult> Index() {
            try {
                List<Profesor> Profesors = await getAllProfesors(apiClient, apiLink);
                return View(Profesors);
            } catch (Exception) {
                return NotFound();
            }
        }

        //Metodo que obtiene todos los profesores, publico por si otros controllers lo necesitan
        public static async Task<List<Profesor>> getAllProfesors(HttpClient apiClient, string apiLink) {
            try {
                List<Profesor> Profesors = new List<Profesor> { };
                
                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/Profesor/GetAll");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                Profesors = JsonConvert.DeserializeObject<List<Profesor>>(response.Content.ReadAsStringAsync().Result);
                return Profesors;
            } catch (Exception) {

                throw;
            }
        }

        // GET: Profesors/Details/5
        public async Task<IActionResult> Details(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }
                Profesor Profesor = await getProfesor(id);
                return View(Profesor);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Profesors/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Profesors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Apellidos,Genero")] Profesor Profesor) {
            try {
                if (ModelState.IsValid) {
                    //Convertir el Profesor en un string, de tipo json, para enviarlo al backend con este formato
                    var Profesorstring = JsonConvert.SerializeObject(Profesor);
                    StringContent content = new StringContent(Profesorstring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PutAsync($"{apiLink}/Profesor/New", content);
                    response.EnsureSuccessStatusCode();
                }
                return RedirectToAction(nameof(Index));
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: Profesors/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }

                Profesor Profesor = await getProfesor(id);
                return View(Profesor);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Profesors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Apellidos,Genero")] Profesor Profesor) {
            if (id != Profesor.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    //Convertir el Profesor en un string, de tipo json, para enviarlo al backend con este formato
                    var Profesorstring = JsonConvert.SerializeObject(Profesor);
                    StringContent content = new StringContent(Profesorstring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PatchAsync($"{apiLink}/Profesor/Edit", content);
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                    
                        throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Profesor);
        }

        // GET: Profesors/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }
                Profesor Profesor = await getProfesor(id);
                return View(Profesor);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Profesors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            if (ModelState.IsValid) {
                try {

                    HttpResponseMessage response = await apiClient.DeleteAsync($"{apiLink}/Profesor/Delete/{id}");
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

       
        //Void para obtener un solo profesor por su id
        private async Task<Profesor> getProfesor(int? id) {
            try {
                Profesor Profesor = new Profesor();
                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/Profesor/Get/{id}");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                //Convertir la respuesta con el modelo
                Profesor = JsonConvert.DeserializeObject<Profesor>(response.Content.ReadAsStringAsync().Result);
                return Profesor;
            } catch (Exception e) {

                throw e;
            }
        }
    }
}
