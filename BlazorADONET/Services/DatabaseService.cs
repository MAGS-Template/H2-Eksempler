using System.Data.SqlClient;
using Domain_Models;

namespace BlazorADONET.Services
{
    public class DatabaseService : IDatabaseService
    {
        string connectionString = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ADONET;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        public List<Class> GetAllClassesWithStudentsAndTeachers()
        {
            var classes = new Dictionary<int, Class>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    // Join Class, StudentClass, and TeacherClass
                    cmd.CommandText = @"
                      SELECT c.*, 
                         (SELECT STRING_AGG(CONCAT(s.FirstName, ' ', s.LastName), ', ') 
                          FROM StudentClass sc 
                          JOIN Student s ON sc.StudentId = s.Id 
                          WHERE sc.ClassId = c.Id) AS StudentNames,
                         (SELECT STRING_AGG(CONCAT(t.FirstName, ' ', t.LastName), ', ') 
                          FROM TeacherClass tc 
                          JOIN Teacher t ON tc.TeacherId = t.Id 
                          WHERE tc.ClassId = c.Id) AS TeacherNames
                      FROM Class c";

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int classId = Convert.ToInt32(reader["Id"]);
                        if (!classes.ContainsKey(classId))
                        {
                            classes[classId] = new Class
                            {
                                Id = classId,
                                ClassName = Convert.ToString(reader["ClassName"]),
                                Students = new List<Student>(),
                                Teachers = new List<Teacher>()
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("StudentNames")))
                        {
                            var studentNames = reader["StudentNames"].ToString().TrimEnd(',').Split(',');
                            foreach (var name in studentNames)
                            {
                                var parts = name.Split(' ');
                                classes[classId].Students.Add(new Student { FirstName = parts[0], LastName = parts[1] });
                            }
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("TeacherNames")))
                        {
                            var teacherNames = reader["TeacherNames"].ToString().TrimEnd(',').Split(',');
                            foreach (var name in teacherNames)
                            {
                                var parts = name.Split(' ');
                                classes[classId].Teachers.Add(new Teacher { FirstName = parts[0], LastName = parts[1] });
                            }
                        }
                    }

                    reader.Close();
                }
            }

            Console.WriteLine($"Retrieved {classes.Count()} classes from the database.");
            return classes.Values.ToList();
        }
        public void CreateClass(Class cls)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Class (ClassName) VALUES (@ClassName)";
                    cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Class GetClassById(int id)
        {
            Class cls = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Class WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cls = new Class
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ClassName = Convert.ToString(reader["ClassName"])
                        };
                    }

                    reader.Close();
                }
            }

            return cls;
        }

        public void UpdateClass(Class cls)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Class SET ClassName = @ClassName WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@ClassName", cls.ClassName);
                    cmd.Parameters.AddWithValue("@Id", cls.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteClass(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM Class WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
