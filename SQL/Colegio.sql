BEGIN TRANSACTION
use Colegio;

CREATE TABLE Alumno(
	ID int identity NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Apellidos VARCHAR(100) NOT NULL,
	Genero VARCHAR(20) NOT NULL,
	FechaNacimiento DateTime NOT NULL
	PRIMARY KEY (ID)
);

CREATE TABLE Profesor(
	ID int identity NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Apellidos VARCHAR(100) NOT NULL,
	Genero VARCHAR(20) NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE Grado(
	ID int identity NOT NULL,
	Nombre VARCHAR(50) NOT NULL,
	ProfesorID int NOT NULL
	PRIMARY KEY(ID),
	FOREIGN KEY(ProfesorID) References Profesor(ID) 
);

CREATE TABLE AlumnoGrado(
	ID int identity NOT NULL,
	AlumnoID int NOT NULL,
	GradoID int NOT NULL,
	Seccion VARCHAR(20),
	PRIMARY KEY(ID),
	FOREIGN KEY (AlumnoID) REFERENCES Alumno(ID),
	FOREIGN KEY (GradoID) REFERENCES Grado(ID)
);

IF @@ERROR > 0 BEGIN
	ROLLBACK;
END;
COMMIT TRANSACTION;

CREATE PROCEDURE GetAllAlumnos
AS
begin
	select * from Alumno;
	end

CREATE PROCEDURE InsertAlumno
@Nombre VARCHAR(100), @Apellidos VARCHAR(100), @Genero VARCHAR(20), @FechaNacimiento DateTime
AS
BEGIN
	Insert into Alumno (Nombre, Apellidos, Genero, FechaNacimiento) VALUES (@Nombre, @Apellidos, @Genero, @FechaNacimiento);
END;

CREATE PROCEDURE GetAlumno
@ID int
AS
BEGIN
	Select * from Alumno Where ID = @ID;
END;

CREATE PROCEDURE DeleteAlumno
@ID int
AS
BEGIN
	DELETE from AlumnoGrado Where AlumnoID = @ID;
	Delete from Alumno Where ID = @ID;
	
END;

CREATE PROCEDURE EditAlumno
@ID int, @Nombre VARCHAR(100), @Apellidos VARCHAR(100), @Genero VARCHAR(20), @FechaNacimiento DateTime
AS
BEGIN
	Update Alumno SET Nombre = @Nombre, Apellidos = @Apellidos, Genero = @Genero, FechaNacimiento = @FechaNacimiento WHERE ID = @ID
END;

--Al menos para alumnos y profesores, los metodos son los mismos, solo cambiando practicamente el nombre de la tabla y el ccampo fecha de nacimiento 
CREATE PROCEDURE GetAllProfesores
AS
begin
	select * from Profesor;
	end

CREATE PROCEDURE InsertProfesor(
@Nombre VARCHAR(100), @Apellidos VARCHAR(100), @Genero VARCHAR(20))
AS
BEGIN
	Insert into Profesor (Nombre, Apellidos, Genero) VALUES (@Nombre, @Apellidos, @Genero);
END;


CREATE PROCEDURE GetProfesor
@ID int
AS
BEGIN
	Select * from Profesor Where ID = @ID;
END;

CREATE PROCEDURE DeleteProfesor
@ID int
AS
BEGIN
	DELETE FROM Grado WHERE ProfesorID = @ID;
	Delete from Profesor Where ID = @ID;
	
END;

CREATE PROCEDURE EditProfesor
@ID int, @Nombre VARCHAR(100), @Apellidos VARCHAR(100), @Genero VARCHAR(20)
AS
BEGIN
	Update Profesor SET Nombre = @Nombre, Apellidos = @Apellidos, Genero = @Genero WHERE ID = @ID
END;

CREATE PROCEDURE GetAllGrados
AS
begin
	Select g.*, CONCAT(p.Nombre, ' ', p.Apellidos) as ProfesorNombre from Grado as g
	inner join Profesor as p on g.ProfesorID = p.ID
	end

CREATE PROCEDURE InsertGrado
@Nombre VARCHAR(100), @ProfesorID int
AS
BEGIN
	Insert into Grado (Nombre, ProfesorID) VALUES (@Nombre, @ProfesorID);
END;

CREATE PROCEDURE GetGrado
@ID int
AS
BEGIN
	Select g.*, CONCAT(p.Nombre, ' ', p.Apellidos) as ProfesorNombre from Grado as g
	inner join Profesor as p on g.ProfesorID = p.ID
	Where g.ID = @ID;
END;

CREATE PROCEDURE DeleteGrado
@ID int
AS
BEGIN
	DELETE FROM AlumnoGrado  WHERE GradoID = @ID;
	Delete from Grado Where ID = @ID;
END;

CREATE PROCEDURE EditGrado
@ID int, @Nombre VARCHAR(100), @ProfesorID int
AS
BEGIN
	Update Grado SET Nombre = @Nombre, ProfesorID = @ProfesorID WHERE ID = @ID
END;

CREATE PROCEDURE GetAllAlumnoGrados
AS
begin
	Select ag.*, CONCAT(a.Nombre, ' ', a.Apellidos) as AlumnoNombre, g.Nombre as GradoNombre from AlumnoGrado as ag
	inner join Alumno as a on ag.AlumnoID = a.ID
	inner join Grado as g on ag.GradoID = g.ID
	end

CREATE PROCEDURE InsertAlumnoGrado
@AlumnoID int, @GradoID int, @Seccion VARCHAR(20)
AS
BEGIN
	Insert into AlumnoGrado (AlumnoID, GradoID, Seccion) VALUES (@AlumnoID, @GradoID, @Seccion);
END;


CREATE PROCEDURE GetAlumnoGrado
@ID int
AS
BEGIN
	Select ag.*, CONCAT(a.Nombre, ' ', a.Apellidos) as AlumnoNombre, g.Nombre as GradoNombre from AlumnoGrado as ag
	inner join Alumno as a on ag.AlumnoID = a.ID
	inner join Grado as g on ag.GradoID = g.ID
	Where ag.ID = @ID;
END;

CREATE PROCEDURE DeleteAlumnoGrado
@ID int
AS
BEGIN
	Delete from AlumnoGrado Where ID = @ID;
END;

CREATE PROCEDURE EditAlumnoGrado
@ID int, @AlumnoID int, @GradoID int, @Seccion VARCHAR(20)
AS
BEGIN
	Update AlumnoGrado SET AlumnoID = @AlumnoID, GradoID = @GradoID, Seccion = @Seccion WHERE ID = @ID
END;