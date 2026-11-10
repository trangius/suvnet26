using System.Data;
using MySql.Data.MySqlClient; // dotnet add package MySql.Data
using Dapper; // dotnet add package Dapper

class Teacher
{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Email{get;set;}
}

class Program
{
    static string connectionString = File.ReadLines("../connectionstring.txt").First(); 

    static void Main()
    {
        while(true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Visa alla lärare");
            Console.WriteLine("2. Lägg till lärare");
            Console.WriteLine("0. Avsluta");
            Console.Write("Val: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    IEnumerable<Teacher> teachers = SelectAllTeachers();
                    // Loopa igenom listan (IEnumerablen) och kriv ut lärarnas namn och email.
                    // Notera att allt detta sker bara på din lokala dator:
                    foreach (Teacher t in teachers)
                    {
                        Console.WriteLine(t.Name + " " + t.Email);
                    } 
                    break;
                case "2":
                    // lägg till en ny lärare i Teacher-tabellen:
                    Console.Write("Ange namn på läraren: ");
                    string name = Console.ReadLine();
                    Console.Write("Ange email till läraren: ");
                    string email = Console.ReadLine();
                    AddTeacher(name, email);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Jag förstår inte");
                    break;
            }
        }
    
    }

    static IEnumerable<Teacher> SelectAllTeachers()
    {
        // Skapa en connection:
        using IDbConnection connection = new MySqlConnection(connectionString);
        
        // Gör en SELECT-qurey med hjälp av dappermetoden Query mot servern.
        // Objektet teachers finns sedan lokalt i RAM
        return connection.Query<Teacher>("SELECT Id, Name, Email FROM Teacher;");
    }

    static void AddTeacher(string name, string email)
    {
        // Skapa en connection:
        using IDbConnection connection = new MySqlConnection(connectionString);
        // skapar en string som innehåller queryn:
        string query = $"INSERT INTO Teacher (Name, Email) VALUES (@Name, @Email);";
        Console.WriteLine(query);
        // Använd alltid Execute vid INSERT/UPDATE/DELETE (inte Query).
        // Execute returnerar en in
        connection.Execute(query, new {Name = name, Email = email}); 
    }
}
