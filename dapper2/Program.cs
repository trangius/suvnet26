using System.Data;
using MySql.Data.MySqlClient; // dotnet add package MySql.Data
using Dapper; // dotnet add package Dapper

class Teacher
{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Email{get;set;}
}

// record:
//   har alltid en default-konstruktor "av sig självt"
//   kan ej ha metoder
//   =-operatorn är överlagrad och ger nya kopior istället för referenser
record StudentAndGrade(string Student, string Course, DateTime EnrollmentDate, int Grade, string Grader);

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
            Console.WriteLine("3. Visa alla registrerade elever och deras betyg");
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
                case "3":
                    IEnumerable<StudentAndGrade> studentAndGrades = SelectStudentsAndGrade();
                    foreach (StudentAndGrade sg in studentAndGrades)
                    {
                        Console.WriteLine($"Student: {sg.Student}, Kurs: {sg.Course}, Betyg: {sg.Grade}, Registerringdatum: {sg.EnrollmentDate}, Lärare: {sg.Grader}");
                    } 
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

    static IEnumerable<StudentAndGrade> SelectStudentsAndGrade()
    {
        // Skapa en connection:
        using IDbConnection connection = new MySqlConnection(connectionString);
        
        // Gör en SELECT-qurey med hjälp av dappermetoden Query mot servern.
        // Objektet teachers finns sedan lokalt i RAM
        return connection.Query<StudentAndGrade>("""
                            select s.Name as Student,
                            c.Name as Course,
                            e.EnrollmentDate,
                            e.Grade,
                            t.Name as Grader
                            from Enrollment e
                            inner join Student s on s.Id = e.StudentId
                            inner join Course c on c.Id = e.CourseId
                            left join Teacher t on t.Id = c.TeacherId
                            order by s.Name, c.Name;
                            """);
    }
    
}
