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
using System.Collections.ObjectModel;
using StudentsVisitationsWPF.Entities;
using Bogus.DataSets;

namespace StudentsVisitationsWPF
{
    class DBMethods
    {
        internal static string ConnectionString = "Data Source=G:\\ITSTEP\\SP Projects\\WPF\\StudentsVisitationsWPF\\mydatabase.db";
        internal static AppDbContext db = new AppDbContext();
        internal static SqliteConnection connection = new SqliteConnection(ConnectionString);

        public class AppDbContext : DbContext
        {
           // private const string ConnectionString = "Data Source=G:\\ITSTEP\\SP Projects\\WPF\\StudentsVisitationsWPF\\mydatabase.db";

            protected override void OnConfiguring(
                DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }

            public DbSet<Student> Students => Set<Student>();
            public DbSet<Visitation> Visitations => Set<Visitation>();
            public DbSet<Subject> Subjects=> Set<Subject>();
        }


        public static async Task<Visitation[]> GetVisitations()
        {
            try
            {
                //await using (var db = new AppDbContext())
                List<Visitation> visitations = await db.Visitations.ToListAsync();
                return visitations.ToArray();
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
                List<Student> students = await db.Students.ToListAsync();
                return students.ToArray();
            }
            catch
            {
                MessageBox.Show("Students Table Doesn't Exist!");
            }
            return Array.Empty<Student>();

        }

        public static async Task<Subject[]> GetSubjects()
        {
            try
            {
                List<Subject> subjects = await db.Subjects.ToListAsync();
                return subjects.ToArray();
            }
            catch
            {
                MessageBox.Show("Subject Table Doesn't Exist!");
            }
            return Array.Empty<Subject>();

        }

        public static async Task<int> GetVisitationsCount()
        {
            try
            {
                var visitsCount = await db.Visitations.CountAsync();
                return visitsCount;
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
                var studentsCount = await db.Students.CountAsync();
                return studentsCount;
            }
            catch
            {
                return -1;
            }

        }

        public static async Task<int> GetSubjectsCount()
        {
            try
            {
                var subjectsCount = await db.Subjects.CountAsync();
                return subjectsCount;
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
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        public static async void AddVisit(Visitation visit)
        {
            await db.Visitations.AddAsync(visit);
            await db.SaveChangesAsync();
        }

        public static async void AddSubject(Subject subject)
        {
            await db.Subjects.AddAsync(subject);
            await db.SaveChangesAsync();
        }

        public static bool StudentTableExists()
        {
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
                .RuleFor(it => it.Email, (f, it) => f.Internet.Email(it.FIO));

            //Randomiser
            var randomiser = new Bogus.Randomizer();

            //Generation
            for (int i = 0; i < ammount; i++)
            {
                var randominfo = studentFaker.Generate();
                Student student = new Student
                {
                    Id = Guid.NewGuid(),
                    FIO = randominfo.FIO,
                    DOB = new DateOnly(randomiser.Int(1970, 2022), randomiser.Int(1, 12), randomiser.Int(1, 29)),
                    Email = randominfo.Email
                };
                AddStudent(student);
            }

            MessageBox.Show("Students Generated!");

        }

        public static async void GenerateVisitations(int ammount)
        {
            if (await GetStudentsCount() > 0 && await GetSubjectsCount() > 0)
            {
                if (ammount == 0) { return; }
                //Randomiser
                var randomiser = new Bogus.Randomizer();
                
                //Students
                var stu = await GetStudents();
                var subjects = await GetSubjects();

                //Generation
                for (int i = 0; i < ammount; i++)
                {
                    var randomnum = randomiser.Int(1, await GetStudentsCount());
                    var randomsubjectnum = randomiser.Int(1, await GetSubjectsCount());
                    Visitation visitation = new Visitation
                    {
                        Id = Guid.NewGuid(),
                        Student = stu[randomnum - 1],
                        Subject = subjects[randomsubjectnum - 1]
                    };
                    DateOnly bd = stu[randomnum-1].DOB;
                    visitation.Date = new DateOnly(randomiser.Int(bd.Year - 1, 2022), randomiser.Int(bd.Month, 12), randomiser.Int(bd.Day, 29));
                    AddVisit(visitation);
                }

                MessageBox.Show("Visitations Generated!");

            }
            else if(await GetStudentsCount() == 0)
            {
                MessageBox.Show("Students Table Is Empty!");
            }
            else if (await GetSubjectsCount() == 0)
            {
                MessageBox.Show("Subjects Table Is Empty!");
            }
        }

        public static void GenerateSubjects(int ammount)
        {
            if (ammount == 0) { return; }

            //Name & email Generator 
            var subjectFaker = new Faker<Subject>("en")
                .RuleFor(it => it.Name, f => f.Name.JobTitle());
                

            //Randomiser
            

            //Generation
            for (int i = 0; i < ammount; i++)
            {
                var randominfo = subjectFaker.Generate();
                Subject subject = new Subject
                {
                    Id = Guid.NewGuid(),
                    Name= randominfo.Name
                };
                AddSubject(subject);
            }

            MessageBox.Show("Subjects Generated!");

        }

        public static bool StudentExists(int id)
        {
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

        public static async void ClearStudents()
        {
            foreach (var item in db.Students)
            {
                db.Students.Remove(item);
            }
            db.SaveChanges();
            MessageBox.Show("Students Cleared!");
            ClearItems();
        }

        public static async void ClearVisitations()
        {
            foreach (var item in db.Visitations)
            {
                db.Visitations.Remove(item);
            }
            db.SaveChanges();
            MessageBox.Show("Visitations Cleared!");
            ClearItems();
        }

        public static async void ClearSubjects()
        {
            foreach (var item in db.Subjects)
            {
                db.Subjects.Remove(item);
            }
            db.SaveChanges();
            MessageBox.Show("Subjects Cleared!");
            ClearItems();
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

        public static void AddBindingStudent(ObservableCollection<Student> oc)
        {
            ((MainWindow)Application.Current.MainWindow).InfoGrid.ItemsSource = oc;
        }

        public static void AddBindingVisitations(ObservableCollection<Visitation> oc)
        {
            ((MainWindow)Application.Current.MainWindow).InfoGrid.ItemsSource = oc;
        }

        public static void CreateVisitationColumns()
        {
            ClearColumns();

            var col1 = new DataGridTextColumn();
            col1.Header = "Id";
            col1.Binding = new Binding("Id");
            col1.IsReadOnly = false;
            
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "Student";
            col2.Binding = new Binding("Student");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "Subject";
            col3.Binding = new Binding("Subject");
            col3.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "Date";
            col4.Binding = new Binding("Date");
            col4.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col4);
        }
        public static void CreateSubjectColumns()
        {
            ClearColumns();

            var col1 = new DataGridTextColumn();
            col1.Header = "Id";
            col1.Binding = new Binding("Id");
            col1.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "Name";
            col2.Binding = new Binding("Name");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col2);
        }

        public static void CreateStudentColumns()
        {
            ClearColumns();

            var col1 = new DataGridTextColumn();
            col1.Header = "Id";
            col1.Binding = new Binding("Id");
            col1.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "FIO";
            col2.Binding = new Binding("FIO");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "DOB";
            col3.Binding = new Binding("DOB");
            col3.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "Email";
            col4.Binding = new Binding("Email");
            col4.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).InfoGrid.Columns.Add(col4);

            
        }
    }
}
