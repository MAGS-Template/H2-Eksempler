using System;
using System.Collections.Generic;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        Program program = new Program();
        program.Init();
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ADONET;Integrated Security=True;Connect Timeout=30;Encrypt=False;"; // Replace with your actual connection string
        Console.Write("Enter 'PDB' to populate the database: ");
        string input = Console.ReadLine().ToUpper();

        if (input == "PDB")
        {
            PopulateDatabase(connectionString);
            ReadAndDisplayClasses(connectionString);
        }
    }
    private void Init()
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ADONET;Integrated Security=True;Connect Timeout=30;Encrypt=False;"; // Replace with your actual connection string
        try
        {
            CreateTable(connectionString);
            ReadAndDisplayClasses(connectionString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private void CreateTable(string connectionString)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                // Check if Class table exists
                cmd.CommandText = @"
           IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Class')
           BEGIN
               CREATE TABLE Class
               (
                  Id INT IDENTITY(1,1) PRIMARY KEY,
                  ClassName NVARCHAR(50),
                  CreatedAt DATETIME,
                  UpdatedAt DATETIME
               )
           END";
                cmd.ExecuteNonQuery();

                // Check if Student table exists
                cmd.CommandText = @"
           IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Student')
           BEGIN
               CREATE TABLE Student
               (
                  Id INT IDENTITY(1,1) PRIMARY KEY,
                  CreatedAt DATETIME,
                  UpdatedAt DATETIME,
                  FirstName NVARCHAR(50),
                  LastName NVARCHAR(50)
               )
           END";
                cmd.ExecuteNonQuery();

                // Check if StudentClass table exists
                cmd.CommandText = @"
           IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudentClass')
           BEGIN
               CREATE TABLE StudentClass
               (
                  Id INT IDENTITY(1,1) PRIMARY KEY,
                  StudentId INT FOREIGN KEY REFERENCES Student(Id),
                  ClassId INT FOREIGN KEY REFERENCES Class(Id)
               )
           END";
                cmd.ExecuteNonQuery();

                // Check if Department table exists
                cmd.CommandText = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Department')
            BEGIN
                CREATE TABLE Department
                (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    CreatedAt DATETIME,
                    UpdatedAt DATETIME,
                    DepartmentName NVARCHAR(50)
                )
            END";
                cmd.ExecuteNonQuery();


                // Check if Teacher table exists
                cmd.CommandText = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Teacher')
            BEGIN
                CREATE TABLE Teacher
                (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    CreatedAt DATETIME,
                    UpdatedAt DATETIME,
                    FirstName NVARCHAR(50),
                    LastName NVARCHAR(50),
                    DepartmentId INT FOREIGN KEY REFERENCES Department(Id)
                )
            END";
                cmd.ExecuteNonQuery();

                // Check if TeacherClass table exists
                cmd.CommandText = @"
           IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TeacherClass')
           BEGIN
               CREATE TABLE TeacherClass
               (
                  Id INT IDENTITY(1,1) PRIMARY KEY,
                  TeacherId INT FOREIGN KEY REFERENCES Teacher(Id),
                  ClassId INT FOREIGN KEY REFERENCES Class(Id)
               )
           END";
                cmd.ExecuteNonQuery();
            }
        }


        Console.WriteLine("Tables created successfully!");
    }
    static void PopulateDatabase(string connectionString)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                // Insert some data into the Class table
                cmd.CommandText = "INSERT INTO Class (CreatedAt, UpdatedAt, ClassName) VALUES (@CreatedAt, @UpdatedAt, @ClassName)";
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@ClassName", "Hoved forløb 1");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Class (CreatedAt, UpdatedAt, ClassName) VALUES (@CreatedAt, @UpdatedAt, @ClassName)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@ClassName", "Hoved forløb 2");
                cmd.ExecuteNonQuery();

                // Insert some data into the Department table
                cmd.CommandText = "INSERT INTO Department (CreatedAt, UpdatedAt, DepartmentName) VALUES (@CreatedAt, @UpdatedAt, @DepartmentName)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@DepartmentName", "Computer Science");
                cmd.ExecuteNonQuery();



                // Get the IDs of the inserted classes
                cmd.CommandText = "SELECT Id FROM Class ORDER BY Id DESC";
                SqlDataReader classReader = cmd.ExecuteReader();

                List<int> classIds = new List<int>();
                while (classReader.Read())
                {
                    classIds.Add(Convert.ToInt32(classReader["Id"]));
                }
                classReader.Close();

                // Insert some data into the Student table
                cmd.CommandText = "INSERT INTO Student (CreatedAt, UpdatedAt, FirstName, LastName) VALUES (@CreatedAt, @UpdatedAt, @FirstName, @LastName)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@FirstName", "John");
                cmd.Parameters.AddWithValue("@LastName", "Doe");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Student (CreatedAt, UpdatedAt, FirstName, LastName) VALUES (@CreatedAt, @UpdatedAt, @FirstName, @LastName)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@FirstName", "Jane");
                cmd.Parameters.AddWithValue("@LastName", "Smith");
                cmd.ExecuteNonQuery();

                // Get the IDs of the inserted students
                cmd.CommandText = "SELECT Id FROM Student ORDER BY Id DESC";
                SqlDataReader studentReader = cmd.ExecuteReader();

                List<int> studentIds = new List<int>();
                while (studentReader.Read())
                {
                    studentIds.Add(Convert.ToInt32(studentReader["Id"]));
                }
                studentReader.Close();

                // Insert some data into the StudentClass table
                foreach (int classId in classIds)
                {
                    foreach (int studentId in studentIds)
                    {
                        cmd.CommandText = "INSERT INTO StudentClass (StudentId, ClassId) VALUES (@StudentId, @ClassId)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        cmd.Parameters.AddWithValue("@ClassId", classId);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Insert some data into the Teacher table
                cmd.CommandText = "INSERT INTO Teacher (CreatedAt, UpdatedAt, FirstName, LastName, DepartmentId) VALUES (@CreatedAt, @UpdatedAt, @FirstName, @LastName, @DepartmentId)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@FirstName", "Bob");
                cmd.Parameters.AddWithValue("@LastName", "Johnson");
                cmd.Parameters.AddWithValue("@DepartmentId", 1);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO Teacher (CreatedAt, UpdatedAt, FirstName, LastName) VALUES (@CreatedAt, @UpdatedAt, @FirstName, @LastName)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@FirstName", "Emma");
                cmd.Parameters.AddWithValue("@LastName", "Williams");
                cmd.Parameters.AddWithValue("@DepartmentId", 1);
                cmd.ExecuteNonQuery();

                // Get the IDs of the inserted teachers
                cmd.CommandText = "SELECT Id FROM Teacher ORDER BY Id DESC";
                SqlDataReader teacherReader = cmd.ExecuteReader();

                List<int> teacherIds = new List<int>();
                while (teacherReader.Read())
                {
                    teacherIds.Add(Convert.ToInt32(teacherReader["Id"]));
                }
                teacherReader.Close();

                // Insert some data into the TeacherClass table
                foreach (int classId in classIds)
                {
                    foreach (int teacherId in teacherIds)
                    {
                        cmd.CommandText = "INSERT INTO TeacherClass (TeacherId, ClassId) VALUES (@TeacherId, @ClassId)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        cmd.Parameters.AddWithValue("@ClassId", classId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        Console.WriteLine("Database populated successfully!");
    }
    static void ReadAndDisplayClasses(string connectionString)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                // Get all classes
                cmd.CommandText = "SELECT * FROM Class";
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int classId = Convert.ToInt32(reader["Id"]);
                    string className = Convert.ToString(reader["ClassName"]);
                    Console.WriteLine($"Class: {classId} - {className}");

                    // Close the reader before executing another command
                    reader.Close();

                    // Get all students for this class
                    cmd.CommandText = "SELECT s.* FROM Student s INNER JOIN StudentClass e ON s.Id = e.StudentId WHERE e.ClassId = @ClassId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ClassId", classId);
                    reader = cmd.ExecuteReader();

                    Console.WriteLine("Students:");
                    while (reader.Read())
                    {
                        int studentId = Convert.ToInt32(reader["Id"]);
                        string firstName = Convert.ToString(reader["FirstName"]);
                        string lastName = Convert.ToString(reader["LastName"]);
                        Console.WriteLine($" Student: {studentId}, Name: {firstName} {lastName}");
                    }
                    reader.Close();

                    // Get all teachers for this class
                    cmd.CommandText = "SELECT t.*, d.DepartmentName FROM Teacher t INNER JOIN TeacherClass tc ON t.Id = tc.TeacherId INNER JOIN Department d ON t.DepartmentId = d.Id WHERE tc.ClassId = @ClassId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ClassId", classId);
                    reader = cmd.ExecuteReader();

                    Console.WriteLine("Teachers:");
                    while (reader.Read())
                    {
                        int teacherId = Convert.ToInt32(reader["Id"]);
                        string firstName = Convert.ToString(reader["FirstName"]);
                        string lastName = Convert.ToString(reader["LastName"]);
                        string departmentName = Convert.ToString(reader["DepartmentName"]);
                        Console.WriteLine($" Teacher: {teacherId}, Name: {firstName} {lastName}, Department: {departmentName}");
                    }
                    reader.Close();

                    Console.WriteLine();
                }
                reader.Close();
            }
        }
    }

}