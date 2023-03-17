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
using System.IO;
using StudentsVisitationsWPF.ValueObjects;
using static System.Net.Mime.MediaTypeNames;

namespace StudentsVisitationsWPF
{
    public class DBMethods
    {
        public static string path = ""; //Path to db
        public static string ConnectionString = "Data Source=" + path;
        public AppDbContext db = new AppDbContext();
        public SqliteConnection connection = new SqliteConnection(ConnectionString);


        public class AppDbContext : DbContext
        {
            protected override void OnConfiguring(
                DbContextOptionsBuilder optionsBuilder)
            {
                if(path.Trim().Length == 0) 
                { 
                    MessageBox.Show("Please enter a path to the database in DBMethods line 21!");
                    Environment.Exit(0);
                }
                optionsBuilder
                    .UseSqlite(ConnectionString)
                    .LogTo(line =>
                    {
                        File.AppendAllText(
                            "mydatabaselog.txt", line + Environment.NewLine);
                    }); ;
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Student>()
                    .OwnsOne(s => s.Email,
                    builder => builder.Property(it => it.Value)
                    .HasColumnName("Email")
                );

                modelBuilder.Entity<Student>()
                 .OwnsOne(s => s.PassportNumber,
                 builder => builder.Property(it => it.Value)
                 .HasColumnName("PassportNumber")
             );


            }


            public DbSet<Student> Students => Set<Student>();
            public DbSet<Visitation> Visitations => Set<Visitation>();
            public DbSet<Subject> Subjects=> Set<Subject>();
            public DbSet<Group> Groups => Set<Group>();
        }


        public async Task<Visitation[]> GetVisitations()
        {
            try
            {
                //await using (var db = new AppDbContext())
                return await db.Visitations.Include(visit => visit.Student)
                    .Include(visit => visit.Subject).ToArrayAsync();
            }
            catch
            {
                MessageBox.Show("Visitations Table Doesn't Exist!");
            }
            return Array.Empty<Visitation>();

        }

        public async Task<Student[]> GetStudentMonthYear(int month, int year)
        {
            try
            {
                return await db.Students
                          .Include(stu=>stu.Visitations)
                          .Include(stu=>stu.Group)
                          .Where(stu => stu.Visitations.Where(v => v.Date.Month == month && v.Date.Year == year).Any())
                          .ToArrayAsync();
            }
            catch
            {
                MessageBox.Show("Students Table Doesn't Exist!");
            }
            return Array.Empty<Student>();

        }

        public async Task<Student[]> GetStudents()
        {
            try
            {
                return await db.Students.Include(s => s.Group).ToArrayAsync();
            }
            catch
            {
                MessageBox.Show("Students Table Doesn't Exist!");
            }
            return Array.Empty<Student>();

        }

        public async Task<Subject[]> GetSubjects()
        {
            try
            {
                return await db.Subjects.ToArrayAsync();
            }
            catch
            {
                MessageBox.Show("Subject Table Doesn't Exist!");
            }
            return Array.Empty<Subject>();

        }

        public async Task<Group[]> GetGroups()
        {
            try
            {
                return await db.Groups.Include(g => g.Students).ToArrayAsync();
            }
            catch
            {
                MessageBox.Show("Groups Table Doesn't Exist!");
            }
            return Array.Empty<Group>();

        }

        public async Task<Group[]> GetNonEmptyGroups()
        {

            try
            {
                return await db.Groups
                             .Include(g => g.Students)
                             .Where(g => g.Students.Count > 0)
                             .ToArrayAsync();
            }
            catch
            {
                MessageBox.Show("Groups table doesn't exist!");
            }
            return Array.Empty<Group>();
        }

        public async Task<int> GetVisitationsCount()
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


        public async Task<int> GetStudentsCount()
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

        public async Task<int> GetSubjectsCount()
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

        public async Task<int> GetGroupsCount()
        {
            try
            {
                var groupsCount = await db.Groups.CountAsync();
                return groupsCount;
            }
            catch
            {
                return -1;
            }

        }

       
        public async void EditStudent(Guid guid, Student st)
        {
            var student = db.Students.Include(s => s.Group).Include(s => s.Visitations).Where(s => s.Id == guid).ToArray()[0];
            student.FIO = st.FIO;
            student.DOB = st.DOB;
            student.Email = st.Email;
            student.Group = st.Group;
            await db.SaveChangesAsync();
        }

        public async void AddStudent(Student student)
        {
            try
            {
                db.Students.Include(s => s.Group).Include(s => s.Visitations);
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();
                await LoadStudents(false);
                await LoadGroups();
                await LoadStudentGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        public async void AddVisit(Visitation visit)
        {
            db.Visitations.Include(v => v.Student);
            db.Visitations.Include(v => v.Subject);
            await db.Visitations.AddAsync(visit);
            await db.SaveChangesAsync();
            await LoadVisitations();
            await LoadStudentVisitations();
        }

        public async void AddSubject(Subject subject)
        {
            await db.Subjects.AddAsync(subject);
            await db.SaveChangesAsync();
            await LoadSubjects();
        }

        public async void AddGroup(Group group)
        {
            db.Groups.Include(g => g.Students);
            await db.Groups.AddAsync(group);
            await db.SaveChangesAsync();
            await LoadGroups();
            await LoadStudentGroups();
        }

        public bool StudentTableExists()
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

        public bool VisitationTableExists()
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

        public async void GenerateStudents(int ammount)
        {
            if(await GetGroupsCount()==0)
            {
                MessageBox.Show("Groups table is empty!");
                return;
            }
            if (ammount == 0) { return; }

            //Name & email Generator 
            var studentFaker = new Faker<Student>("en")
                .RuleFor(it => it.FIO, f => f.Name.FullName())
                .RuleFor(it => it.Email, (f, it) => new Email(f.Internet.Email(it.FIO)));

            //Randomiser
            var randomiser = new Bogus.Randomizer();
            var groups = await GetGroups();
            

            //Generation
            for (int i = 0; i < ammount; i++)
            {
                var randomnum = randomiser.Number(0, groups.Length - 1);
                var randominfo = studentFaker.Generate();
                Student student = new Student
                {
                    Id = Guid.NewGuid(),
                    FIO = randominfo.FIO,
                    DOB = new DateOnly(randomiser.Int(1975, 2022), randomiser.Int(1, 12), randomiser.Int(1, 29)),
                    PassportNumber = new PassportNumber(randomiser.Int(100000000, 999999999).ToString()),
                    Email = randominfo.Email
                };
                student.Group = groups[randomnum];
                await LoadStudents(false);
                await LoadGroups();
                await LoadStudentGroups();
                AddStudent(student);
                
            }


            MessageBox.Show("Students Generated!");

        }

        public async void GenerateVisitations(int ammount)
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
                    stu[randomnum - 1].Visitations.Add(visitation); //Adding the visitation to the student
                }

                await LoadVisitations();
                await LoadStudentVisitations();

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

        public async void GenerateSubjects(int ammount)
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
            await LoadSubjects();

            MessageBox.Show("Subjects Generated!");

        }

        public async void GenerateGroups(int ammount)
        {
            if (ammount == 0) { return; }

            //Name & email Generator 
            var groupFaker = new Faker<Group>("en")
                .RuleFor(it => it.Name, f => f.Address.State() + f.Address.ZipCode());

            //Generation
            for (int i = 0; i < ammount; i++)
            {
                var randominfo = groupFaker.Generate();
                Group group = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = randominfo.Name,
                    CreationDate = DateTime.Now
                };
                AddGroup(group);
                await LoadGroups();
                await LoadStudentGroups();
            }
            MessageBox.Show("Groups Generated!");
        }

        public bool StudentExists(int id)
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

        public async void ClearStudents()
        {
            try
            {
                await db.Database.ExecuteSqlRawAsync("DELETE FROM Students");
                db.ChangeTracker.Clear();
                await LoadStudents(false);
                await LoadGroups();
                await LoadStudentGroups();
                MessageBox.Show("Cleared!");
            }
            catch
            {
                MessageBox.Show("Clear visitations first!");
            }
            
        }

        public async void ClearVisitations()
        {
            await db.Database.ExecuteSqlRawAsync("DELETE FROM Visitations");
            db.ChangeTracker.Clear();

            await LoadVisitations();
            await LoadStudentVisitations();

            MessageBox.Show("Cleared!");
        }

        public async void ClearSubjects()
        {
            try
            {
                await db.Database.ExecuteSqlRawAsync("DELETE FROM Subjects");
                db.ChangeTracker.Clear();

                await LoadSubjects();
                await LoadVisitations();
                await LoadStudentVisitations();

                MessageBox.Show("Cleared!");
            }
            catch
            {
                MessageBox.Show("Clear visitations first!");
                return;
            }
            
        }

        public async void ClearGroups()
        {
            try
            {
                await db.Database.ExecuteSqlRawAsync("DELETE FROM Groups");
                db.ChangeTracker.Clear();

                await LoadGroups();
                await LoadStudentGroups();
                await LoadVisitations();
                await LoadStudentVisitations();

                MessageBox.Show("Cleared!");
            }
            catch
            {
                MessageBox.Show("Clear students first!");
            }
            
        }

        public async Task LoadAll(bool isSearching)
        {
            await LoadAllStudentTabGrids(isSearching);
            await LoadSubjects();
            await LoadGroups();
            await LoadVisitations();
        }

        public async Task LoadSubjects()
        {
            var subjectgrid = ((MainWindow)System.Windows.Application.Current.MainWindow).SubjectsInfoGrid;

            var subjs = await db.Subjects.ToListAsync();
            subjectgrid.ItemsSource = subjs;
        }

        public async Task LoadVisitations()
        {
            var visitationsgrid = ((MainWindow)System.Windows.Application.Current.MainWindow).VisitationsInfoGrid;

            var vsts = await db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();
            visitationsgrid.ItemsSource = vsts;
        }

        public static int _studentsPageIndex = 0;
        public static int _studentsCount = 0;
        public static int _studentsPerPage = 10;
        public static double _pageCount = 0;

        public async Task LoadStudents(bool isSearching)
        {
            var studentsGrid = ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsInfoGrid;
            var studentsTextBox = ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsTextBox;
            var previousButton = ((MainWindow)System.Windows.Application.Current.MainWindow).PreviousPageButton;
            var nextButton = ((MainWindow)System.Windows.Application.Current.MainWindow).NextPageButton;
            var totalPageLabel = ((MainWindow)System.Windows.Application.Current.MainWindow).TotalPageLabel;
            var pageTextBox = ((MainWindow)System.Windows.Application.Current.MainWindow).PageTextBox;

            if (isSearching == true)
            {
                var _studentMatchesCount = await db.Students
                    .Where(s => EF.Functions.Like(s.FIO, $"%{studentsTextBox.Text}%"))
                    .CountAsync();

                var studentMatches = await db.Students
                    .Include(s => s.Visitations)
                    .Include(s => s.Group)
                    .Where(s => EF.Functions.Like(s.FIO, $"%{studentsTextBox.Text}%"))
                    .Skip(_studentsPageIndex * _studentsPerPage)
                    .Take(_studentsPerPage)
                    .ToListAsync();

                studentsGrid.ItemsSource = studentMatches;
                previousButton.IsEnabled = _studentsPageIndex > 0;

                var pageCount = Math.Ceiling((double)_studentMatchesCount / _studentsPerPage);
                _pageCount = pageCount;
                totalPageLabel.Content = $"/{pageCount}";
                pageTextBox.Text = $"{_studentsPageIndex+1}";
                var lastPageIndex = pageCount - 1;
                nextButton.IsEnabled = _studentsPageIndex < lastPageIndex;

            }
            else if(isSearching == false)
            {
                _studentsCount = await GetStudentsCount();
                List<Student> students = await db.Students
                    .Skip(_studentsPageIndex * _studentsPerPage)
                    .Take(_studentsPerPage)
                    .ToListAsync();

                studentsGrid.ItemsSource = students;
                previousButton.IsEnabled = _studentsPageIndex > 0;

                var pageCount = Math.Ceiling((double) _studentsCount / _studentsPerPage);
                _pageCount = pageCount;
                totalPageLabel.Content = $"/{pageCount}";
                pageTextBox.Text = $"{_studentsPageIndex+1}";
                var lastPageIndex = pageCount- 1;
                nextButton.IsEnabled = _studentsPageIndex < lastPageIndex;
            }
        }

        public void pageIndexNext()
        {
            _studentsPageIndex++;
        }

        public void pageIndexPrevious()
        {
            _studentsPageIndex--;
        }

        public void setPageIndex(int index)
        {
            var pageTextBox = ((MainWindow)System.Windows.Application.Current.MainWindow).PageTextBox;
            if (index > _pageCount)
            {
                _studentsPageIndex = int.Parse( _pageCount.ToString())-1;
                pageTextBox.Text = _pageCount.ToString();
                return;
            }
            else if(index<=0)
            {
                _studentsPageIndex = 0;
                pageTextBox.Text = "1";
                return;
            }
            else
            {
                _studentsPageIndex = index;
            }
        }



        public async Task LoadGroups()
        {
            var groupgrid = ((MainWindow)System.Windows.Application.Current.MainWindow).GroupsInfoGrid;

            var grps = await db.Groups.Include(g => g.Students).ToListAsync();
            groupgrid.ItemsSource = grps;
        }

        public async Task LoadAllStudentTabGrids(bool isSearching)
        {
            var studentvisitsgrid = ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsVisitationsInfoGrid;
            var studentgroupsgrid = ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsGroupsInfoGrid;

            await LoadStudents(isSearching);

            var gs = await db.Groups.Include(g => g.Students).ToListAsync();
            studentgroupsgrid.ItemsSource = gs;

            var vs = await db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();
            studentvisitsgrid.ItemsSource = vs;
        }

        public async Task LoadStudentVisitations()
        {
            var studentvisitsgrid = ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsVisitationsInfoGrid;

            var vsts = await db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();
            studentvisitsgrid.ItemsSource = vsts;
        }

        public async Task LoadStudentGroups()
        {
            var studentgroupsgrid = ((MainWindow)System.Windows.Application.Current.MainWindow).StudentsGroupsInfoGrid;

            var grps = await db.Groups.Include(g => g.Students).ToListAsync();
            studentgroupsgrid.ItemsSource = grps;
        }
    }
}
