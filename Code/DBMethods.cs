﻿using Bogus;
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

namespace StudentsVisitationsWPF
{
    class DBMethods
    {
        internal static string path = ""; //Path to db
        internal static string ConnectionString = "Data Source=" + path;
        internal static AppDbContext db = new AppDbContext();
        internal static SqliteConnection connection = new SqliteConnection(ConnectionString);


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

            public DbSet<Student> Students => Set<Student>();
            public DbSet<Visitation> Visitations => Set<Visitation>();
            public DbSet<Subject> Subjects=> Set<Subject>();
            public DbSet<Group> Groups => Set<Group>();
        }


        public static async Task<Visitation[]> GetVisitations()
        {
            try
            {
                //await using (var db = new AppDbContext())
                return db.Visitations.Include(visit => visit.Student)
                    .Include(visit => visit.Subject).ToArray();
            }
            catch
            {
                MessageBox.Show("Visitations Table Doesn't Exist!");
            }
            return Array.Empty<Visitation>();

        }

        public static async Task<Student[]> GetStudentMonthYear(int month, int year)
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

        public static async Task<Student[]> GetStudents()
        {
            try
            {
                return db.Students.Include(s => s.Group).ToArray();  
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

        public static async Task<Group[]> GetGroups()
        {
            try
            {
                return db.Groups.Include(g => g.Students).ToArray();
            }
            catch
            {
                MessageBox.Show("Groups Table Doesn't Exist!");
            }
            return Array.Empty<Group>();

        }

        public static async Task<Group[]> GetNonEmptyGroups()
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

        public static async Task<int> GetGroupsCount()
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

       
        public static async void EditStudent(Guid guid, Student st)
        {
            var student = db.Students.Include(s => s.Group).Include(s => s.Visitations).Where(s => s.Id == guid).ToArray()[0];
            student.FIO = st.FIO;
            student.DOB = st.DOB;
            student.Email = st.Email;
            student.Group = st.Group;
            await db.SaveChangesAsync();
        }

        public static async void AddStudent(Student student)
        {
            try
            {
                db.Students.Include(s => s.Group).Include(s => s.Visitations);
                await db.Students.AddAsync(student);
                await db.SaveChangesAsync();
                Refresh("all");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        public static async void AddVisit(Visitation visit)
        {
            db.Visitations.Include(v => v.Student);
            db.Visitations.Include(v => v.Subject);
            await db.Visitations.AddAsync(visit);
            await db.SaveChangesAsync();
            Refresh("all");
        }

        public static async void AddSubject(Subject subject)
        {
            await db.Subjects.AddAsync(subject);
            await db.SaveChangesAsync();
            Refresh("all");
        }

        public static async void AddGroup(Group group)
        {
            db.Groups.Include(g => g.Students);
            await db.Groups.AddAsync(group);
            await db.SaveChangesAsync();
            Refresh("all");
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

        public static async void GenerateStudents(int ammount)
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
                .RuleFor(it => it.Email, (f, it) => f.Internet.Email(it.FIO));

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
                    DOB = new DateOnly(randomiser.Int(1970, 2022), randomiser.Int(1, 12), randomiser.Int(1, 29)),
                    Email = randominfo.Email
                };
                student.Group = groups[randomnum];
                Refresh("all");
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
                    stu[randomnum - 1].Visitations.Add(visitation); //Adding the visitation to the student
                }

                Refresh("all");
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
            Refresh("all");
            MessageBox.Show("Subjects Generated!");

        }

        public static void GenerateGroups(int ammount)
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
                Refresh("all");
            }
            MessageBox.Show("Groups Generated!");
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

            try
            {
                foreach (var item in db.Students)
                {
                    db.Students.Remove(item);
                }
                await db.SaveChangesAsync();
                Refresh("all");
                MessageBox.Show("Students Cleared!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please clear visitations first!");
            }
            
        }

        public static async void ClearVisitations()
        {
            try
            {
                foreach (var item in db.Visitations)
                {
                    db.Visitations.Remove(item);
                }
                await db.SaveChangesAsync();
                Refresh("all");
                MessageBox.Show("Visitations Cleared!");
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static async void ClearSubjects()
        {
            try
            {
                foreach (var item in db.Subjects)
                {
                    db.Subjects.Remove(item);
                }
                await db.SaveChangesAsync();
                Refresh("all");
                MessageBox.Show("Subjects Cleared!");
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Please clear visitations first!");
            }
            
        }

        public static async void ClearGroups()
        {
            try
            {
                foreach (var item in db.Groups)
                {
                    db.Groups.Remove(item);
                }
                await db.SaveChangesAsync();
                Refresh("all");
                MessageBox.Show("Groups Cleared!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please clear visitations/students first!");
            }

        }


        public static async void Refresh(string choice)
        {
            var studentgrid = ((MainWindow)Application.Current.MainWindow).StudentsInfoGrid;
            var studentvisitsgrid = ((MainWindow)Application.Current.MainWindow).StudentsVisitationsInfoGrid;
            var studentgroupsgrid = ((MainWindow)Application.Current.MainWindow).StudentsGroupsInfoGrid;
            var subjectgrid = ((MainWindow)Application.Current.MainWindow).SubjectsInfoGrid;
            var groupgrid = ((MainWindow)Application.Current.MainWindow).GroupsInfoGrid;
            var visitationsgrid = ((MainWindow)Application.Current.MainWindow).VisitationsInfoGrid;

            choice = choice.ToLower();
            switch (choice)
            {
                case "allstudents":
                    var st = await db.Students.Include(stu => stu.Visitations).ToListAsync();
                    studentgrid.ItemsSource = st;

                    var gs = await db.Groups.Include(g => g.Students).ToListAsync();
                    studentgroupsgrid.ItemsSource = gs;

                    var vs = await db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();
                    studentvisitsgrid.ItemsSource = vs;
                    break;

                case "groups":
                    var grps = await db.Groups.Include(g => g.Students).ToListAsync();
                    groupgrid.ItemsSource = grps;
                    break;

                case "students":
                    var stus = await db.Students.Include(stu => stu.Visitations).ToListAsync();
                    studentgrid.ItemsSource = stus;
                    break;

                case "visitations":
                    var vsts= await db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();
                    visitationsgrid.ItemsSource = vsts;
                    break;

                case "subjects":
                    var subjs= await db.Subjects.ToListAsync();
                    subjectgrid.ItemsSource = subjs;
                    break;

                case "all":
                default:
                    var students = await db.Students.Include(stu => stu.Visitations).ToListAsync();
                    studentgrid.ItemsSource = students;

                    var subjects = await db.Subjects.ToListAsync();
                    subjectgrid.ItemsSource = subjects;

                    var groups = await db.Groups.Include(g => g.Students).ToListAsync();
                    groupgrid.ItemsSource = groups;
                    studentgroupsgrid.ItemsSource = groups;

                    var visitations = await db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();
                    visitationsgrid.ItemsSource = visitations;
                    studentvisitsgrid.ItemsSource = visitations;

                    break;
            }
        }
    }
}
