namespace Frontend.Models {
    public class AlumnoGrado {
        public int ID { get; set; }
        public int AlumnoID { get; set; }
        public int GradoID { get; set; }
        public string Seccion { get; set; }
        //Datos nullables
        public string? AlumnoNombre { get; set; }
        public string? GradoNombre { get; set; }
    }
}
