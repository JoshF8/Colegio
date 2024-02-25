using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using System.Text;

namespace Frontend.Controllers
{
    public class AlumnosController : Controller
    {
        
        //Se usara un cliente Http para consultar al Back
        private HttpClient apiClient;
        private string apiLink = "";
        //Para obtener datos del appsettings
        private IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        private IConfiguration configuration;
        public AlumnosController(FrontendAlumno context)
        {
            //_context = context;
            configuration = configurationBuilder.Build();
            apiClient = new HttpClient();
            apiLink = configuration.GetValue<string>("VARS:apiBack");
        }

        // GET: Alumnos
        public async Task<IActionResult> Index()
        {
            try {
                List<Alumno> alumnos = await getAllAlumnos(apiClient, apiLink);
                return View(alumnos);
            } catch (Exception) {
                return NotFound();
            }
        }

        // GET: Alumnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null) {
                    return NotFound();
                }
                Alumno alumno = await getAlumno(id);
                return View(alumno);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // GET: Alumnos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alumnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Apellidos,Genero,FechaNacimiento")] Alumno alumno)
        {
            try {
                if (ModelState.IsValid) {
                    //Convertir el alumno en un string, de tipo json, para enviarlo al backend con este formato
                    var alumnoString = JsonConvert.SerializeObject(alumno);
                    StringContent content = new StringContent(alumnoString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PutAsync($"{apiLink}/Alumno/New", content);
                    response.EnsureSuccessStatusCode();
                }
                return RedirectToAction(nameof(Index));
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // GET: Alumnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                Alumno alumno = await getAlumno(id);
                return View(alumno);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Alumnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Apellidos,Genero,FechaNacimiento")] Alumno alumno)
        {
            if (id != alumno.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    //Convertir el alumno en un string, de tipo json, para enviarlo al backend con este formato
                    var alumnoString = JsonConvert.SerializeObject(alumno);
                    StringContent content = new StringContent(alumnoString, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await apiClient.PatchAsync($"{apiLink}/Alumno/Edit", content);
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                   
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        // GET: Alumnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null) {
                    return NotFound();
                }
                Alumno alumno = await getAlumno(id);
                return View(alumno);
            } catch (Exception) {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Alumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (ModelState.IsValid) {
                try {
                    
                    HttpResponseMessage response = await apiClient.DeleteAsync($"{apiLink}/Alumno/Delete/{id}");
                    response.EnsureSuccessStatusCode();
                } catch (DbUpdateConcurrencyException) {
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

      

        //Void para obtener solo un alumno por su ID
        private async Task<Alumno> getAlumno(int? id) {
            try {
                Alumno alumno = new Alumno();
                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/Alumno/Get/{id}");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                //Convertir la respuesta con el modelo
                alumno = JsonConvert.DeserializeObject<Alumno>(response.Content.ReadAsStringAsync().Result);
                return alumno;
            } catch (Exception e) {

                throw e;
            }
        }

        //Metodo que obtiene todos los alumnos, publico por si otros controllers lo necesitan
        public static async Task<List<Alumno>> getAllAlumnos(HttpClient apiClient, string apiLink) {
            try {
                List<Alumno> Alumnos = new List<Alumno> { };

                HttpResponseMessage response = await apiClient.GetAsync($"{apiLink}/Alumno/GetAll");
                //Si el mensaje es un mensaje de error tira error
                response.EnsureSuccessStatusCode();
                Alumnos = JsonConvert.DeserializeObject<List<Alumno>>(response.Content.ReadAsStringAsync().Result);
                return Alumnos;
            } catch (Exception) {

                throw;
            }
        }
    }
}
