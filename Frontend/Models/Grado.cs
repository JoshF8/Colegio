namespace Frontend.Models {
    public class Grado {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int ProfesorID { get; set; }
        //Esta parte del modelo, se utilizara solo para devolverle un resultado mas "visual" al front
        public string? ProfesorNombre { get; set; }
    }
}
