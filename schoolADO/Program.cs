using MySqlConnector; // vi vill ha MySqlConnector-biblioteket

class Teacher
{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Email{get;set;}
}

class Program
{
    static void Main()
    {
        // skapa en connection-string
        string connectionString = File.ReadLines("../connectionstring.txt").First(); 

        // skapa en connection och öppna den:
        using MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        // skapa ett MySqlCommand-objekt med en query:
        using MySqlCommand command = new MySqlCommand("SELECT Id, Name, Email FROM Teacher;", connection);
        using MySqlDataReader reader = command.ExecuteReader();

        // loopa igenom reader-objektetes alla element
        List <Teacher> teachers = new List<Teacher>();
        while(reader.Read())
        {
            Teacher temp = new Teacher()
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                Email = reader.GetString("Email")
            };
            teachers.Add(temp); 
        }
        reader.Close();

        foreach(Teacher t in teachers)
        {
            Console.WriteLine(t.Name);
        }
    }
}
