using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AlumnoController : Controller {
        //Sera la conexion a la DB
        private Database _context;
        public AlumnoController() {
            _context = new Database();
        }
        //Metodo para obtener todos los alumnos
        [HttpGet("GetAll", Name ="GetAllAlumno")]
        public IActionResult GetAll() {
			try {
                List<Alumno> alumnos =  _context.GetAllAlumnos();
                return new ObjectResult(alumnos) { StatusCode = 200 };
			} catch (Exception) {
                return new ObjectResult("Internal Server Error"){ StatusCode = 500};
			}
        }

        [HttpPut("New", Name = "NewAlumno")]
        public IActionResult New(Alumno alumno) {
            try {
                if(alumno.ID != 0) {
                    return new ObjectResult("Para crear alumno el ID debe ser 0") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el alumno recibido
                _context.NewAlumno(alumno);
                return new ObjectResult("Alumno creado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        //Editar alumno, siempre y cuando el ID no sea 0
        [HttpPatch("Edit", Name = "EditAlumno")]
        public IActionResult Edit(Alumno alumno) {
            try {
                if (alumno.ID == 0) {
                    return new ObjectResult("No se puede editar alumno no existente") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el alumno recibido
                _context.EditAlumno(alumno);
                return new ObjectResult("Alumno editado exitosamente.") { StatusCode = 201 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpGet("Get/{id}", Name = "GetAlumno")]
        public IActionResult Get(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede obtener alumno no existente") { StatusCode = 400 };
                }
                //Se le envia a la base de datos el alumno recibido
                Alumno alumno = _context.GetAlumno(id);
                return new ObjectResult(alumno) { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }

        [HttpDelete("Delete/{id}", Name = "DeleteAlumno")]
        public IActionResult Delete(int id) {
            try {
                if (id == 0) {
                    return new ObjectResult("No se puede eliminar alumno no existente") { StatusCode = 400 };
                }
                _context.DeleteAlumno(id);
                
                return new ObjectResult("Alumno eliminado exitosamente") { StatusCode = 200 };
            } catch (Exception) {
                return new ObjectResult("Internal Server Error") { StatusCode = 500 };
            }
        }
    }
}
