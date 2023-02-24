using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Bogus;
using System.ComponentModel;
using Microsoft.Data.Sqlite;

namespace StudentsVisitationsWPF
{
    public class Visitation
    {
        public int ID { get; set; }
        public long STUDENTID { get; set; }
        public DateOnly DATE { get; set; }

        public override string ToString()
        {
            return $"STUDENT ID: {STUDENTID}, DATE: {DATE}";
        }
    }

    public class Student
    {
        public int ID { get; set; }
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
        private const string ConnectionString = "Data Source=mydatabase.db";

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Visitation> Visitations => Set<Visitation>();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateTablesButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTables();
        }

        private void DropTablesButton_Click(object sender, RoutedEventArgs e)
        {
            DropTables();
            ClearColumns();
            ClearItems();
        }

        private void SearchVisitationButton_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow sw = new SearchWindow();
            sw.ShowDialog();
        }

        private void GenerateStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AmmountWindow amw = new AmmountWindow("student");
                amw.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Students Table Doesn't Exist!");
            }
            
        }

        private void GenerateVisitationsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AmmountWindow amw = new AmmountWindow("visitation");
                amw.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Visitations Table Doesn't Exist!");
            }
            
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {

            AddStudentWindow adw = new AddStudentWindow();
            adw.ShowDialog();
        }

        private void AddVisitationButton_Click(object sender, RoutedEventArgs e)
        {
            AddVisitation av = new AddVisitation();
            av.ShowDialog();
        }

        private async void ShowStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            if(GetStudentsCount()==0)
            {
                MessageBox.Show("Students Table Is Empty!");
                return;
            }
            else if(GetStudentsCount() >= 1)
            {
                var students = await GetStudents();
                CreateStudentColumns();
                ClearItems();

                foreach (var student in students)
                {
                    InfoGrid.Items.Add(student);
                }
            }
            else
            {
                MessageBox.Show("Students Table Doesn't Exist!");
            }
        }

        private async void ShowVisitationsButton_Click(object sender, RoutedEventArgs e)
        {
            if(GetVisitationsCount()==0)
            {
                MessageBox.Show("Visitations Table Is Empty!");
                return;
            }
            else if (GetVisitationsCount() >= 1)
            {
                var visitations = await GetVisitations();
                CreateVisitationColumns();
                ClearItems();

                foreach (var visit in visitations)
                {
                    InfoGrid.Items.Add(visit);
                }
            }
            else
            {
                MessageBox.Show("Visitations Table Doesn't Exist!");
            }
        }

        void ClearColumns()
        {
            for (int i = InfoGrid.Columns.Count - 1; i >= 0; i--)
            {
                InfoGrid.Columns.Remove(InfoGrid.Columns[i]);
            }
        }

        void ClearItems()
        {
            for (int i = InfoGrid.Items.Count - 1; i >= 0; i--)
            {
                InfoGrid.Items.Remove(InfoGrid.Items[i]);
            }
        }

        public Int64 GetVisitationsCount()
        {
            try
            {
                string ConnectionString = "Data Source=mydatabase.db";
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Visitations;";
                Int64 count = (Int64)command.ExecuteScalar();
                return count;
            }
            catch
            {
                return -1;
            }
            
        }
        public Int64 GetStudentsCount()
        {
            try
            {
                string ConnectionString = "Data Source=mydatabase.db";
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Students;";
                Int64 count = (Int64)command.ExecuteScalar();
                return count;
            }
            catch
            {
                return -1;
            }
        }



        void CreateStudentColumns()
        {
            ClearColumns();

            var col1 = new DataGridTextColumn();
            col1.Header = "ID";
            col1.Binding = new Binding("ID");
            InfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "FIO";
            col2.Binding = new Binding("FIO");
            InfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "DOB";
            col3.Binding = new Binding("DOB");
            InfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "EMAIL";
            col4.Binding = new Binding("EMAIL");
            InfoGrid.Columns.Add(col4);

            var col5 = new DataGridCheckBoxColumn();
            col5.Header = "PRESENT";
            col5.IsReadOnly = false;
            InfoGrid.Columns.Add(col5);

        }

        void CreateVisitationColumns()
        {
            ClearColumns();

            var col1 = new DataGridTextColumn();
            col1.Header = "ID";
            col1.Binding = new Binding("ID");
            InfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "STUDENTID";
            col2.Binding = new Binding("STUDENTID");
            InfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "DATE";
            col3.Binding = new Binding("DATE");
            InfoGrid.Columns.Add(col3);
        }






        public async void CreateTables()
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    await db.Database.ExecuteSqlRawAsync(@"CREATE TABLE Students
                        (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FIO      TEXT        NOT NULL ,
                        DOB     DATETIME    NOT NULL,
                        EMAIL    TEXT        NOT NULL
                        );"
                    );


                    await db.Database.ExecuteSqlRawAsync(@"CREATE TABLE Visitations
                        (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        STUDENTID INTEGER    NOT NULL ,
                        DATE     DATETIME    NOT NULL ,
                        FOREIGN KEY(STUDENTID) REFERENCES Students(ID)
                        ); "
                    );

                    MessageBox.Show("Tables Created!");

                }
            }

            catch
            {
                MessageBox.Show("Tables Already Exist!");
            }
        }

        public async void DropTables()
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    await db.Database.ExecuteSqlRawAsync(@"DROP TABLE Visitations");
                    await db.Database.ExecuteSqlRawAsync(@"DROP TABLE Students");
                    MessageBox.Show("Tables Dropped!");

                }
            }
            catch
            {
                MessageBox.Show("Tables Already Dropped!");
            }
        }

        public async Task<Student[]> GetStudents()
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

        public async Task<Visitation[]> GetVisitations()
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

        
    }
}


    



