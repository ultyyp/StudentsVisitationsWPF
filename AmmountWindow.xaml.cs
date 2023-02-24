using Bogus;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StudentsVisitationsWPF
{

    /// <summary>
    /// Interaction logic for AmmountWindow.xaml
    /// </summary>
    public partial class AmmountWindow : Window
    {
        internal string connectionString = "Data Source=mydatabase.db";
        internal string type = string.Empty;

        public AmmountWindow(string type)
        {
            InitializeComponent();
            this.type = type;
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int ammount = int.Parse(AmmountTextBox.Text);
                if (type == "student")
                {
                    if (ammount > 0 && StudentTableExists() == true)
                    {
                        GenerateStudents(ammount);
                    }
                    else
                    {
                        MessageBox.Show("Table Doesn't Exist!");
                    }
                    this.Close();
                }
                else if (type == "visitation")
                {
                    if (ammount > 0 && VisitationTableExists() == true)
                    {
                        GenerateVisitations(ammount);
                    }
                    else
                    {
                        MessageBox.Show("Table Doesn't Exist!");
                    }
                    this.Close();
                }

            }
        }

        public bool StudentTableExists()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Students';";
            Int64 count = (Int64)command.ExecuteScalar();
            if (count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VisitationTableExists()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Visitations';";
            Int64 count = (Int64)command.ExecuteScalar();
            if (count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void AddStudent(Student student)
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    await db.Students.AddAsync(student);
                    await db.SaveChangesAsync();

                    //await db.Database.ExecuteSqlRawAsync($"INSERT INTO Students (FIO, DOB, EMAIL)" +
                    //      $"VALUES ('{student.FIO}', '{student.DOB}', '{student.EMAIL}');");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        void GenerateStudents(int ammount)
        {
            if (ammount == 0) { return; }

            //Name & email Generator 
            var studentFaker = new Faker<Student>("en")
                .RuleFor(it => it.FIO, f => f.Name.FullName())
                .RuleFor(it => it.EMAIL, (f, it) => f.Internet.Email(it.FIO));

            //Randomiser
            var randomiser = new Bogus.Randomizer();

            //Generation
            for (int i = 0; i < ammount; i++)
            {
                var randominfo = studentFaker.Generate();
                Student student = new Student
                {
                    FIO = randominfo.FIO,
                    DOB = new DateOnly(randomiser.Int(1970, 2022), randomiser.Int(1, 12), randomiser.Int(1, 29)),
                    EMAIL = randominfo.EMAIL
                };
                AddStudent(student);
            }

            MessageBox.Show("Students Generated!");

        }

        public bool StudentExists(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM Students WHERE ID = {id};";
            Int64 count = (Int64)command.ExecuteScalar();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async void AddVisitation(Visitation visit)
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    await db.Visitations.AddAsync(visit);
                    await db.SaveChangesAsync();

                    //await db.Database.ExecuteSqlRawAsync($"INSERT INTO Visitations (STUDENTID, DATE)" +
                    //        $"VALUES ({visit.STUDENTID}, '{visit.DATE}');");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        public Int64 StudentsGetCount()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Students;";
            Int64 count = (Int64)command.ExecuteScalar();
            return count;
        }

        public async Task<Student[]> GetStudents()
        {
            await using (var db = new AppDbContext())
            {
                List<Student> students = await db.Students.ToListAsync();
                return students.ToArray();
            }
        }


        async void GenerateVisitations(int ammount)
        {
            if (StudentsGetCount() > 0)
            {
                if (ammount == 0) { return; }
                //Randomiser
                var randomiser = new Bogus.Randomizer();
                //Students
                var stu = await GetStudents();

                //Generation
                for (int i = 0; i < ammount; i++)
                {
                    Visitation visitation = new Visitation
                    {
                        STUDENTID = randomiser.Int(1, (int)StudentsGetCount()),
                    };
                    DateOnly bd = stu[visitation.STUDENTID - 1].DOB;
                    visitation.DATE = new DateOnly(randomiser.Int(bd.Year + 1, 2023), randomiser.Int(bd.Month, 12), randomiser.Int(bd.Day, 29));
                    AddVisitation(visitation);
                }

                MessageBox.Show("Visitations Generated!");

            }
            else
            {
                MessageBox.Show("Students Table Is Empty!");
            }
        }
    }
}
