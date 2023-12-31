using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Domain_Models;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private IConfiguration _configuration;

    public PersonController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string connectionString => _configuration.GetConnectionString("ADONET");

    [HttpPost]
    public void Create(Person person)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO Persons (FirstName, LastName) VALUES (@FirstName, @LastName)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);

                command.ExecuteNonQuery();
            }
        }
    }

    [HttpGet("{id}")]
    public Person Read(int id)
    {
        Person person = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM Persons WHERE PersonID = @PersonID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        person = new Person
                        {
                            PersonID = (int)reader["PersonID"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"]
                        };
                    }
                }
            }
        }

        return person;
    }

    [HttpPut("{id}")]
    public void Update(int id, Person person)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "UPDATE Persons SET FirstName = @FirstName, LastName = @LastName WHERE PersonID = @PersonID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", id);
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);

                command.ExecuteNonQuery();
            }
        }
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "DELETE FROM Persons WHERE PersonID = @PersonID";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", id);

                command.ExecuteNonQuery();
            }
        }
    }
    [HttpGet]
    public List<Person> GetAll()
    {
        List<Person> persons = new List<Person>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM Persons";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Person person = new Person
                        {
                            PersonID = (int)reader["PersonID"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"]
                        };

                        persons.Add(person);
                    }
                }
            }
        }
        return persons;
    }
}
