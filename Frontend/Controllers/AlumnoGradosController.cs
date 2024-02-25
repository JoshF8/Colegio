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
    public class AlumnoGradosController : Controller {
        //Se usara un cliente Http para consultar al Back
        private HttpClient apiClient;
        private string apiLink = "";
        //Para obtener datos del appsettings
        private IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        private IConfiguration configuration;
        public AlumnoGradosController(FrontendAlumno context) {
            //_context = context;
            configuration = configurationBuilder.Build();
            apiClient = new HttpClient();
            apiLink = configuration.GetValue<string>("VARS:apiBack");
        }

        // GET: AlumnoGrados
        public async Task<IActionResult> Index() {
            try {
                List<AlumnoGrado> AlumnoGrados = new List<AlumnoGrado> { };
                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/AlumnoGrado/GetAll");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                AlumnoGrados = JsonConvert.DeserializeObject<List<AlumnoGrado>>(response.Content.ReadAsStringAsync().Result);
                return View(AlumnoGrados);
            } catch (Exception) {
                return NotFound();
            }
        }

        // GET: AlumnoGrados/Details/5
        public async Task<IActionResult> Details(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }
                AlumnoGrado AlumnoGrado = await getAlumnoGrado(id);
                return View(AlumnoGrado);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: AlumnoGrados/Create
        public IActionResult Create() {
            TempData["Alumnos"] = getAlumnos().Result;
            TempData["Grados"] = getGrados().Result;
            return View();
        }

        // POST: AlumnoGrados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AlumnoID,GradoID,Seccion,AlumnoNombre,GradoNombre")] AlumnoGrado AlumnoGrado) {
            try {
                if (ModelState.IsValid) {
                    
                    if (AlumnoGrado.AlumnoID == 0) {
                        return RedirectToAction(nameof(Index));
                    }
                    if (AlumnoGrado.GradoID == 0) {
                        return RedirectToAction(nameof(Index));
                    }

                    //Convertir el AlumnoGrado en un string, de tipo json, para enviarlo al backend con este formato
                    var AlumnoGradostring = JsonConvert.SerializeObject(AlumnoGrado);
                    StringContent content = new StringContent(AlumnoGradostring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PutAsync($"{apiLink}/AlumnoGrado/New", content);
                    response.EnsureSuccessStatusCode();
                }
                return RedirectToAction(nameof(Index));
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: AlumnoGrados/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }

                TempData["Alumnos"] = getAlumnos().Result;
                TempData["Grados"] = getGrados().Result;
                AlumnoGrado AlumnoGrado = await getAlumnoGrado(id);
                return View(AlumnoGrado);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AlumnoGrados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AlumnoID,GradoID,Seccion,AlumnoNombre,GradoNombre")] AlumnoGrado AlumnoGrado) {
            if (id != AlumnoGrado.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    if (AlumnoGrado.AlumnoID == 0) {
                        return RedirectToAction(nameof(Index));
                    }
                    if (AlumnoGrado.GradoID == 0) {
                        return RedirectToAction(nameof(Index));
                    }
                    //Convertir el AlumnoGrado en un string, de tipo json, para enviarlo al backend con este formato
                    var AlumnoGradostring = JsonConvert.SerializeObject(AlumnoGrado);
                    StringContent content = new StringContent(AlumnoGradostring, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PatchAsync($"{apiLink}/AlumnoGrado/Edit", content);
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(AlumnoGrado);
        }

        // GET: AlumnoGrados/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            try {
                if (id == null) {
                    return NotFound();
                }
                AlumnoGrado AlumnoGrado = await getAlumnoGrado(id);
                return View(AlumnoGrado);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: AlumnoGrados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            if (ModelState.IsValid) {
                try {

                    HttpResponseMessage response = await apiClient.DeleteAsync($"{apiLink}/AlumnoGrado/Delete/{id}");
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }


        //void para obtener solo un AlumnoGrado por su id
        private async Task<AlumnoGrado> getAlumnoGrado(int? id) {
            try {
                AlumnoGrado AlumnoGrado = new AlumnoGrado();
                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/AlumnoGrado/Get/{id}");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                //Convertir la respuesta con el modelo
                AlumnoGrado = JsonConvert.DeserializeObject<AlumnoGrado>(response.Content.ReadAsStringAsync().Result);
                return AlumnoGrado;
            } catch (Exception e) {

                throw e;
            }
        }

        //void para buscar todos los alumnos, para no ingresar valores de alumnos que no existan
        private async Task<Dictionary<int, string>> getAlumnos() {
            try {
                Dictionary<int, string> alumnos = new Dictionary<int, string>();
                List<Alumno> alumnosList = await AlumnosController.getAllAlumnos(apiClient, apiLink);
                foreach (var item in alumnosList) {
                    alumnos.Add(item.ID, $"{item.Nombre} {item.Apellidos}");
                }
                return alumnos;
            } catch (Exception) {
                throw;
            }
        }

        //void para buscar todos los Grados, para no ingresar valores de Grados que no existan
        private async Task<Dictionary<int, string>> getGrados() {
            try {
                Dictionary<int, string> Grados = new Dictionary<int, string>();
                List<Grado> GradosList = await GradosController.getAllGrados(apiClient, apiLink);
                foreach (var item in GradosList) {
                    Grados.Add(item.ID, $"{item.Nombre}");
                }
                return Grados;
            } catch (Exception) {
                throw;
            }
        }
    }
    
}
