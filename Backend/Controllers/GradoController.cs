using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class GradoController : Controller {
        //Sera la conexion a la DB
        private Database _context;
        public GradoController() {
            _context = new Database();
        }
        //Metodo para obtener todos los Grados
        [HttpGet("GetAll", Name = "GetAllGrado")]
        public IActionResult GetAll() {
            try {
                List<Grado> Grados = _context.GetAllGrados();
                return new ObjectResult(Grados) { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpPut("New", Name = "NewGrado")]
        public IActionResult New(Grado Grado) {
            try {
                if (Grado.ID != 0) {
                    return new ObjectResult("Para crear Grado el ID debe ser 0") { StatusCode = 400 };
                }
                if (Grado.ProfesorID == 0) {
                    return new ObjectResult("Tiene que asignar un profesor para crear el grado") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el Grado recibido
                _context.NewGrado(Grado);
                return new ObjectResult("Grado creado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        //Editar Grado, siempre y cuando el ID no sea 0
        [HttpPatch("Edit", Name = "EditGrado")]
        public IActionResult Edit(Grado Grado) {
            try {
                if (Grado.ID == 0) {
                    return new ObjectResult("No se puede editar Grado no existente") { StatusCode = 400 };
                }
                if (Grado.ProfesorID == 0) {
                    return new ObjectResult("Tiene que asignar un profesor para editar el grado") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el Grado recibido
                _context.EditGrado(Grado);
                return new ObjectResult("Grado editado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpGet("Get/{id}", Name = "GetGrado")]
        public IActionResult Get(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede obtener Grado no existente") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el Grado recibido
                Grado Grado = _context.GetGrado(id);
                return new ObjectResult(Grado) { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpDelete("Delete/{id}", Name = "DeleteGrado")]
        public IActionResult Delete(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede eliminar Grado no existente") { StatusCode = 400 };
                }
                _context.DeleteGrado(id);
                return new ObjectResult("Grado eliminado exitosamente") { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }
    }
}
