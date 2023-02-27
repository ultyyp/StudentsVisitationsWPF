using Bogus;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentsVisitationsWPF
{
    class DBMethods
    {
        public class Visitation
        {
            public Guid ID { get; set; }
            public Guid STUDENTID { get; set; }
            public DateOnly DATE { get; set; }

            public override string ToString()
            {
                return $"STUDENT ID: {STUDENTID}, DATE: {DATE}";
            }
        }

        public class Student
        {
            public Guid ID { get; set; }
            public string FIO { get; set; }
            public DateOnly DOB { get; set; }

            public string EMAIL { get; set; }

            public override string ToString()
            {
                return $"FIO: {FIO}, DOB: {DOB}, EMAIL: {EMAIL}";
            }
        }

        public class AppDbContext : DbContext
        {
            private const string ConnectionString = "Data Source=G:\\ITSTEP\\SP Projects\\WPF\\StudentsVisitationsWPF\\mydatabase.db";

            protected override void OnConfiguring(
                DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }

            public DbSet<Student> Students => Set<Student>();
            public DbSet<Visitation> Visitations => Set<Visitation>();
        }


        //public static async void CreateTables()
        //{
        //    try
        //    {
        //        await using (var db = new AppDbContext())
        //        {
        //            await db.Database.ExecuteSqlRawAsync(@"CREATE TABLE Students
        //                (
        //                ID TEXT PRIMARY KEY,
        //                FIO      TEXT        NOT NULL ,
        //                DOB     DATETIME    NOT NULL,
        //                EMAIL    TEXT        NOT NULL
        //                );"
        //            );


        //            await db.Database.ExecuteSqlRawAsync(@"CREATE TABLE Visitations
        //                (
        //                ID TEXT PRIMARY,
        //                STUDENTID INTEGER    NOT NULL ,
        //                DATE     DATETIME    NOT NULL ,
        //                FOREIGN KEY(STUDENTID) REFERENCES Students(ID)
        //                ); "
        //            );

        //            MessageBox.Show("Tables Created!");

        //        }
        //    }

        //    catch
        //    {
        //        MessageBox.Show("Tables Already Exist!");
        //    }
        //}

        //public static async void DropTables()
        //{
        //    try
        //    {
        //        await using (var db = new AppDbContext())
        //        {
        //            await db.Database.ExecuteSqlRawAsync(@"DROP TABLE Visitations");
        //            await db.Database.ExecuteSqlRawAsync(@"DROP TABLE Students");
        //            MessageBox.Show("Tables Dropped!");

        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Tables Already Dropped!");
        //    }
        //}

        public static async Task<Visitation[]> GetVisitations()
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    List<Visitation> visitations = await db.Visitations.ToListAsync();
                    return visitations.ToArray();
                }
            }
            catch
            {
                MessageBox.Show("Visitations Table Doesn't Exist!");
            }
            return Array.Empty<Visitation>();

        }

        public static async Task<Student[]> GetStudents()
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    List<Student> students = await db.Students.ToListAsync();
                    return students.ToArray();
                }
            }
            catch
            {
                MessageBox.Show("Students Table Doesn't Exist!");
            }
            return Array.Empty<Student>();

        }

        public static async Task<int> GetVisitationsCount()
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    var visitsCount = await db.Visitations.CountAsync();
                    return visitsCount;
                }
            }
            catch
            {
                return -1;
            }

        }

        public static async Task<int> GetStudentsCount()
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    var studentsCount = await db.Students.CountAsync();
                    return studentsCount;
                }
            }
            catch
            {
                return -1;
            }

        }

        public static async void AddStudent(Student student)
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    await db.Students.AddAsync(student);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        public static async void AddVisit(Visitation visit)
        {
            await using (var db = new AppDbContext())
            {
                await db.Visitations.AddAsync(visit);
                await db.SaveChangesAsync();
            }
        }

        public static bool StudentTableExists()
        {
            string ConnectionString = "Data Source=mydatabase.db";
            using var connection = new SqliteConnection(ConnectionString);
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

        public static bool VisitationTableExists()
        {
            string ConnectionString = "Data Source=mydatabase.db";
            using var connection = new SqliteConnection(ConnectionString);
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

        public static void GenerateStudents(int ammount)
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
                    ID = Guid.NewGuid(),
                    FIO = randominfo.FIO,
                    DOB = new DateOnly(randomiser.Int(1970, 2022), randomiser.Int(1, 12), randomiser.Int(1, 29)),
                    EMAIL = randominfo.EMAIL
                };
                AddStudent(student);
            }

            MessageBox.Show("Students Generated!");

        }

        public static bool StudentExists(int id)
        {
            string ConnectionString = "Data Source=mydatabase.db";
            using var connection = new SqliteConnection(ConnectionString);
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


        public static async void AddVisitation(Visitation visit)
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

        public static async void GenerateVisitations(int ammount)
        {
            if (GetStudentsCount().Result > 0)
            {
                if (ammount == 0) { return; }
                //Randomiser
                var randomiser = new Bogus.Randomizer();
                
                //Students
                var stu = await GetStudents();

                //Generation
                for (int i = 0; i < ammount; i++)
                {
                    var randomnum = randomiser.Int(1, GetStudentsCount().Result);
                    Visitation visitation = new Visitation
                    {
                        STUDENTID = stu[randomnum-1].ID,
                    };
                    DateOnly bd = stu[randomnum-1].DOB;
                    visitation.DATE = new DateOnly(randomiser.Int(bd.Year - 1, 2022), randomiser.Int(bd.Month, 12), randomiser.Int(bd.Day, 29));
                    AddVisitation(visitation);
                }

                MessageBox.Show("Visitations Generated!");

            }
            else
            {
                MessageBox.Show("Students Table Is Empty!");
            }
        }

        public static void ClearColumns()
        {
            for (int i = ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Count - 1; i >= 0; i--)
            {
                ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Remove(((MainWindow)Application.Current.MainWindow).InfoGrid.Columns[i]);
            }
        }

        public static void ClearItems()
        {
            for (int i = ((MainWindow)Application.Current.MainWindow).InfoGrid.Items.Count - 1; i >= 0; i--)
            {
                ((MainWindow)Application.Current.MainWindow).InfoGrid.Items.Remove(((MainWindow)Application.Current.MainWindow).InfoGrid.Items[i]);
            }
        }

        public static void CreateVisitationColumns()
        {
            ClearColumns();

            var col1 = new DataGridTextColumn();
            col1.Header = "ID";
            col1.Binding = new Binding("ID");
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "STUDENTID";
            col2.Binding = new Binding("STUDENTID");
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "DATE";
            col3.Binding = new Binding("DATE");
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col3);
        }

        public static void CreateStudentColumns()
        {
            ClearColumns();

            var col1 = new DataGridTextColumn();
            col1.Header = "ID";
            col1.Binding = new Binding("ID");
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "FIO";
            col2.Binding = new Binding("FIO");
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "DOB";
            col3.Binding = new Binding("DOB");
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "EMAIL";
            col4.Binding = new Binding("EMAIL");
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col4);

            
        }
    }
}
