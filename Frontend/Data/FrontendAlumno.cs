using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Frontend.Models;

namespace Frontend.Data
{
    public class FrontendAlumno : DbContext
    {
        public FrontendAlumno (DbContextOptions<FrontendAlumno> options)
            : base(options)
        {
        }

        public DbSet<Frontend.Models.Alumno> Alumno { get; set; } = default!;
        public DbSet<Frontend.Models.Profesor> Profesor { get; set; } = default!;
        public DbSet<Frontend.Models.Grado> Grado { get; set; } = default!;
        public DbSet<Frontend.Models.AlumnoGrado> AlumnoGrado { get; set; } = default!;
    }
}
