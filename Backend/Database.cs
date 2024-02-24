using Backend.Models;
using System.Data.SqlClient;

namespace Backend {
    public class Database {
        //Estas 2 variables son para obtener datos del appsettings
        private IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        private IConfiguration _configuration;

        private string sqlConnection = "";



        public Database() {
            _configuration = builder.Build();
            //Inicializar la cadena de conexion desde que se inicializa el objeto DB
            sqlConnection = _configuration.GetValue<string>("VARS:sqlconnection");
        }

        #region alumnos
        //Usar using en las conexiones y readers para hacer dispose en cuanto terminen de usarse
        //Metodo para devolver todos los alumnos de la base de datos
        public List<Alumno> GetAllAlumnos() {
            try {
                List<Alumno> result = new List<Alumno> ();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    //Abrir la conexion
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetAllAlumnos", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    //Crear el reader que servira para leer todos los resultados
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            //Crea un objeto nuevo por cada fila y lo va agregando a la lista final
                            result.Add(new Alumno {
                                ID = int.Parse(reader["ID"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Genero = reader["Genero"].ToString(),
                                FechaNacimiento = DateTime.Parse(reader["FechaNacimiento"].ToString()),
                            });
                        }
                    }
                }
                return result;
            } catch (Exception e) {
                throw e;
            }
        }

        //Metodo para crear alumno
        public void NewAlumno(Alumno alumno) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("InsertAlumno", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el alumno nuevo
                    cmd.Parameters.Add(new SqlParameter("@Nombre", alumno.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Apellidos", alumno.Apellidos));
                    cmd.Parameters.Add(new SqlParameter("@Genero", alumno.Genero));
                    cmd.Parameters.Add(new SqlParameter("@FechaNacimiento", alumno.FechaNacimiento));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }

        //Metodo para obtener un solo alumno
        public Alumno GetAlumno(int id) {
            try {
                Alumno alumno = new Alumno();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetAlumno", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    
                    using (SqlDataReader reader  = cmd.ExecuteReader()) {
                        while(reader.Read()) {
                            alumno = new Alumno {
                                ID = int.Parse(reader["ID"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Genero = reader["Genero"].ToString(),
                                FechaNacimiento = DateTime.Parse(reader["FechaNacimiento"].ToString()),
                            };
                            return alumno;
                        }
                    }
                }
                return alumno;
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para editar alumno
        public void EditAlumno(Alumno alumno) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("EditAlumno", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el alumno a editar, por ello tambien se requiere el ID
                    cmd.Parameters.Add(new SqlParameter("@ID", alumno.ID));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", alumno.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Apellidos", alumno.Apellidos));
                    cmd.Parameters.Add(new SqlParameter("@Genero", alumno.Genero));
                    cmd.Parameters.Add(new SqlParameter("@FechaNacimiento", alumno.FechaNacimiento));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para eliminar el alumno
        public void DeleteAlumno(int id) {
            try {
                Alumno alumno = new Alumno();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DeleteAlumno", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.ExecuteNonQuery();   
                }
            } catch (Exception e) {

                throw e;
            }
        }
        #endregion

        #region profesores
        //Usar using en las conexiones y readers para hacer dispose en cuanto terminen de usarse
        //Metodo para devolver todos los alumnos de la base de datos
        public List<Profesor> GetAllProfesores() {
            try {
                List<Profesor> result = new List<Profesor>();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    //Abrir la conexion
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetAllProfesores", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //Crear el reader que servira para leer todos los resultados
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            //Crea un objeto nuevo por cada fila y lo va agregando a la lista final
                            result.Add(new Profesor {
                                ID = int.Parse(reader["ID"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Genero = reader["Genero"].ToString(),
                            });
                        }
                    }
                }
                return result;
            } catch (Exception e) {
                throw e;
            }
        }

        //Metodo para crear Profesor
        public void NewProfesor(Profesor Profesor) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("InsertProfesor", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el Profesor nuevo
                    cmd.Parameters.Add(new SqlParameter("@Nombre", Profesor.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Apellidos", Profesor.Apellidos));
                    cmd.Parameters.Add(new SqlParameter("@Genero", Profesor.Genero));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }

        //Metodo para obtener un solo Profesor
        public Profesor GetProfesor(int id) {
            try {
                Profesor Profesor = new Profesor();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetProfesor", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Profesor = new Profesor {
                                ID = int.Parse(reader["ID"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Genero = reader["Genero"].ToString(),
                            };
                            return Profesor;
                        }
                    }
                }
                return Profesor;
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para editar Profesor
        public void EditProfesor(Profesor Profesor) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("EditProfesor", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el Profesor a editar, por ello tambien se requiere el ID
                    cmd.Parameters.Add(new SqlParameter("@ID", Profesor.ID));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", Profesor.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Apellidos", Profesor.Apellidos));
                    cmd.Parameters.Add(new SqlParameter("@Genero", Profesor.Genero));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para eliminar el Profesor
        public void DeleteProfesor(int id) {
            try {
                Profesor Profesor = new Profesor();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DeleteProfesor", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }
        #endregion

        #region grados
        public List<Grado> GetAllGrados() {
            try {
                List<Grado> result = new List<Grado>();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    //Abrir la conexion
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetAllGrados", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //Crear el reader que servira para leer todos los resultados
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            //Crea un objeto nuevo por cada fila y lo va agregando a la lista final
                            result.Add(new Grado {
                                ID = int.Parse(reader["ID"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                ProfesorID = int.Parse(reader["ProfesorID"].ToString()),
                                ProfesorNombre = reader["ProfesorNombre"].ToString()
                            });
                        }
                    }
                }
                return result;
            } catch (Exception e) {
                throw e;
            }
        }

        //Metodo para crear Grado
        public void NewGrado(Grado Grado) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("InsertGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el Grado nuevo
                    cmd.Parameters.Add(new SqlParameter("@Nombre", Grado.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@ProfesorID", Grado.ProfesorID));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }

        //Metodo para obtener un solo Grado
        public Grado GetGrado(int id) {
            try {
                Grado Grado = new Grado();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Grado = new Grado {
                                ID = int.Parse(reader["ID"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                ProfesorID = int.Parse(reader["ProfesorID"].ToString()),
                                ProfesorNombre = reader["ProfesorNombre"].ToString()
                            };
                            return Grado;
                        }
                    }
                }
                return Grado;
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para editar Grado
        public void EditGrado(Grado Grado) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("EditGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el Grado a editar, por ello tambien se requiere el ID
                    cmd.Parameters.Add(new SqlParameter("@ID", Grado.ID));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", Grado.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@ProfesorID", Grado.ProfesorID));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para eliminar el Grado
        public void DeleteGrado(int id) {
            try {
                Grado Grado = new Grado();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DeleteGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }

        #endregion

        #region alumnogrados
        public List<AlumnoGrado> GetAllAlumnoGrados() {
            try {
                List<AlumnoGrado> result = new List<AlumnoGrado>();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    //Abrir la conexion
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetAllAlumnoGrados", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //Crear el reader que servira para leer todos los resultados
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            //Crea un objeto nuevo por cada fila y lo va agregando a la lista final
                            result.Add(new AlumnoGrado {
                                ID = int.Parse(reader["ID"].ToString()),
                                AlumnoID = int.Parse(reader["AlumnoID"].ToString()),
                                GradoID = int.Parse(reader["GradoID"].ToString()),
                                Seccion = reader["Seccion"].ToString(),
                                AlumnoNombre = reader["AlumnoNombre"].ToString(),
                                GradoNombre = reader["GradoNombre"].ToString()
                            });
                        }
                    }
                }
                return result;
            } catch (Exception e) {
                throw e;
            }
        }

        //Metodo para crear AlumnoGrado
        public void NewAlumnoGrado(AlumnoGrado AlumnoGrado) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("InsertAlumnoGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el AlumnoGrado nuevo
                    cmd.Parameters.Add(new SqlParameter("@AlumnoID", AlumnoGrado.AlumnoID));
                    cmd.Parameters.Add(new SqlParameter("@GradoID", AlumnoGrado.GradoID));
                    cmd.Parameters.Add(new SqlParameter("@Seccion", AlumnoGrado.Seccion));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }

        //Metodo para obtener un solo AlumnoGrado
        public AlumnoGrado GetAlumnoGrado(int id) {
            try {
                AlumnoGrado AlumnoGrado = new AlumnoGrado();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("GetAlumnoGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            AlumnoGrado = new AlumnoGrado {
                                ID = int.Parse(reader["ID"].ToString()),
                                AlumnoID = int.Parse(reader["AlumnoID"].ToString()),
                                GradoID = int.Parse(reader["GradoID"].ToString()),
                                Seccion = reader["Seccion"].ToString(),
                                AlumnoNombre = reader["AlumnoNombre"].ToString(),
                                GradoNombre = reader["GradoNombre"].ToString()
                            };
                            return AlumnoGrado;
                        }
                    }
                }
                return AlumnoGrado;
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para editar AlumnoGrado
        public void EditAlumnoGrado(AlumnoGrado AlumnoGrado) {
            try {
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("EditAlumnoGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //Agregar los parametros para el AlumnoGrado a editar, por ello tambien se requiere el ID
                    cmd.Parameters.Add(new SqlParameter("@ID", AlumnoGrado.ID));
                    cmd.Parameters.Add(new SqlParameter("@AlumnoID", AlumnoGrado.AlumnoID));
                    cmd.Parameters.Add(new SqlParameter("@GradoID", AlumnoGrado.GradoID));
                    cmd.Parameters.Add(new SqlParameter("@Seccion", AlumnoGrado.Seccion));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }
        //Metodo para eliminar el AlumnoGrado
        public void DeleteAlumnoGrado(int id) {
            try {
                AlumnoGrado AlumnoGrado = new AlumnoGrado();
                using (SqlConnection connection = new SqlConnection(sqlConnection)) {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DeleteAlumnoGrado", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception e) {

                throw e;
            }
        }
        #endregion
    }
}
