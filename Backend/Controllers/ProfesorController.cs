using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ProfesorController : Controller {
        
        //Sera la conexion a la DB
        private Database _context;
        public ProfesorController() {
            _context = new Database();
        }
        //Metodo para obtener todos los Profesors

        //Aunque se podrian crear desde el mismo endpoint, solo poniendole distinto tipo, he preferido que cada controller tuviera solo una responsabilidad
        [HttpGet("GetAll", Name = "GetAllProfesor")]
        public IActionResult GetAll() {
            try {
                List<Profesor> Profesors = _context.GetAllProfesores();
                return new ObjectResult(Profesors) { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpPut("New", Name = "NewProfesor")]
        public IActionResult New(Profesor Profesor) {
            try {
                if (Profesor.ID != 0) {
                    return new ObjectResult("Para crear Profesor el ID debe ser 0") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el Profesor recibido
                _context.NewProfesor(Profesor);
                return new ObjectResult("Profesor creado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        //Editar Profesor, siempre y cuando el ID no sea 0
        [HttpPatch("Edit", Name = "EditProfesor")]
        public IActionResult Edit(Profesor Profesor) {
            try {
                if (Profesor.ID == 0) {
                    return new ObjectResult("No se puede editar Profesor no existente") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el Profesor recibido
                _context.EditProfesor(Profesor);
                return new ObjectResult("Profesor editado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpGet("Get/{id}", Name = "GetProfesor")]
        public IActionResult Get(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede obtener Profesor no existente") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el Profesor recibido
                Profesor Profesor = _context.GetProfesor(id);
                return new ObjectResult(Profesor) { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpDelete("Delete/{id}", Name = "DeleteProfesor")]
        public IActionResult Delete(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede eliminar Profesor no existente") { StatusCode = 400 };
                }
                _context.DeleteProfesor(id);
                return new ObjectResult("Profesor eliminado exitosamente") { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }
    }
}
