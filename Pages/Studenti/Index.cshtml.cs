using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

namespace WebApplication1.Pages.Studenti
{

    public class IndexModel : PageModel
    {
        public List<Student> listaStudenti = new List<Student>();
        public List<Nota> listaNote = new List<Nota>();

        public IActionResult OnGet(int? studentId)
        {

            try
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Universitate;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "Select s.Id, g.Denumire, o.Denumire, s.Nume, s.Prenume from Student s, Orase o, Grupa g where s.Oras=o.Id and s.Grupa=g.Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Student student = new Student();
                                student.id = reader.GetInt32(0);
                                student.grupa = reader.GetString(1);
                                student.oras = reader.GetString(2);
                                student.nume = reader.GetString(3);
                                student.prenume = reader.GetString(4);
                                student.note = new List<Nota>();
                                listaStudenti.Add(student);
                            }
                        }
                    }

                    sql = "Select n.Id, n.Student, m.Nume, n.Nota from Note n, Materie m WHERE n.Materie=m.Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Nota nota = new Nota();
                                nota.id = reader.GetInt32(0);
                                nota.student = reader.GetInt32(1);
                                nota.materie = reader.GetString(2);
                                nota.nota = reader.GetInt32(3);
                                Student student = listaStudenti.Find(s => s.id == nota.student);
                                if (student != null)
                                {
                                    student.note.Add(nota);
                                }
                                listaNote.Add(nota);
                            }
                        }
                    }

                    foreach (Student student in listaStudenti)
                    {
                        foreach (var group in student.note.GroupBy(n => n.materie))
                        {
                            var ultimaNota = group.Last();
                            var medieMaterie = ultimaNota.nota;
                            student.medie += medieMaterie;
                        }
                        student.medie /= student.note.Select(n => n.materie).Distinct().Count();
                    }


                    if (studentId.HasValue)
                    {
                        Student student = listaStudenti.FirstOrDefault(s => s.id == studentId.Value);
                        if (student != null)
                        {
                            listaStudenti.Clear();
                            listaStudenti.Add(student);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Nu exista student cu acest ID.";
                            return RedirectToPage("./Index");
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }

        }     

}

    public class Student
    {
        public int id;
        public string nume;
        public string prenume;
        public string grupa;
        public string oras;
        public List<Nota> note;
        public double medie;
    }

    public class Nota
    {
        public int id;
        public int student;
        public string materie;
        public int nota;
    }

}
