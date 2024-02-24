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

GO;

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
	Delete from Profesor Where ID = @ID;
END;

CREATE PROCEDURE EditProfesor
@ID int, @Nombre VARCHAR(100), @Apellidos VARCHAR(100), @Genero VARCHAR(20), @FechaNacimiento DateTime
AS
BEGIN
	Update Profesor SET Nombre = @Nombre, Apellidos = @Apellidos, Genero = @Genero WHERE ID = @ID
END;