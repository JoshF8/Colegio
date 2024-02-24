using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AlumnoGradoController : Controller {
        //Sera la conexion a la DB
        private Database _context;
        public AlumnoGradoController() {
            _context = new Database();
        }
        //Metodo para obtener todos los AlumnoGrados
        [HttpGet("GetAll", Name = "GetAllAlumnoGrado")]
        public IActionResult GetAll() {
            try {
                List<AlumnoGrado> AlumnoGrados = _context.GetAllAlumnoGrados();
                return new ObjectResult(AlumnoGrados) { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpPut("New", Name = "NewAlumnoGrado")]
        public IActionResult New(AlumnoGrado AlumnoGrado) {
            try {
                if (AlumnoGrado.ID != 0) {
                    return new ObjectResult("Para crear AlumnoGrado el ID debe ser 0") { StatusCode = 400 };
                }
                if (AlumnoGrado.AlumnoID == 0) {
                    return new ObjectResult("Tiene que asignar un alumno para crear el AlumnoGrado") { StatusCode = 400 };
                }
                if (AlumnoGrado.GradoID == 0) {
                    return new ObjectResult("Tiene que asignar un grado para crear el AlumnoGrado") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el AlumnoGrado recibido
                _context.NewAlumnoGrado(AlumnoGrado);
                return new ObjectResult("AlumnoGrado creado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        //Editar AlumnoGrado, siempre y cuando el ID no sea 0
        [HttpPatch("Edit", Name = "EditAlumnoGrado")]
        public IActionResult Edit(AlumnoGrado AlumnoGrado) {
            try {
                if (AlumnoGrado.ID == 0) {
                    return new ObjectResult("No se puede editar AlumnoGrado no existente") { StatusCode = 400 };
                }
                if (AlumnoGrado.AlumnoID == 0) {
                    return new ObjectResult("Tiene que asignar un alumno para editar el AlumnoGrado") { StatusCode = 400 };
                }
                if (AlumnoGrado.GradoID == 0) {
                    return new ObjectResult("Tiene que asignar un grado para editar el AlumnoGrado") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el AlumnoGrado recibido
                _context.EditAlumnoGrado(AlumnoGrado);
                return new ObjectResult("AlumnoGrado editado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpGet("Get/{id}", Name = "GetAlumnoGrado")]
        public IActionResult Get(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede obtener AlumnoGrado no existente") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el AlumnoGrado recibido
                AlumnoGrado AlumnoGrado = _context.GetAlumnoGrado(id);
                return new ObjectResult(AlumnoGrado) { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpDelete("Delete/{id}", Name = "DeleteAlumnoGrado")]
        public IActionResult Delete(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede eliminar AlumnoGrado no existente") { StatusCode = 400 };
                }
                _context.DeleteAlumnoGrado(id);
                return new ObjectResult("AlumnoGrado eliminado exitosamente") { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }
    }
}
