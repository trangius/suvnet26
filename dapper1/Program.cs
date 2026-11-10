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
    static void Main()
    {
        string connectionString = File.ReadLines("../connectionstring.txt").First(); 
        // Skapa en connection:
        using IDbConnection connection = new MySqlConnection(connectionString);
        
        // Gör en SELECT-qurey med hjälp av dappermetoden Query mot servern.
        // Objektet teachers finns sedan lokalt i RAM
        IEnumerable<Teacher> teachers = connection.Query<Teacher>("SELECT Id, Name, Email FROM Teacher;");

        // Loopa igenom listan (IEnumerablen) och kriv ut lärarnas namn och email.
        // Notera att allt detta sker bara på din lokala dator:
        foreach (Teacher t in teachers)
        {
            Console.WriteLine(t.Name + " " + t.Email);
        }

        // lägg till en ny lärare i Teacher-tabellen:
        string name = "Fia";
        string email = "fia@skola.com";
        string query = $"INSERT INTO Teacher (Name, Email) VALUES ('{name}', '{email}');";
        Console.WriteLine(query);
        // Använd alltid Execute vid INSERT/UPDATE/DELETE (inte Query).
        // Execute returnerar en in
        connection.Execute(query); 
    }
}
