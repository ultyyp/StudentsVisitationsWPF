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
using Microsoft.EntityFrameworkCore.Design;
using StudentsVisitationsWPF.Migrations;
using StudentsVisitationsWPF.Forms;
using StudentsVisitationsWPF.Entities;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
using static System.Net.Mime.MediaTypeNames;

namespace StudentsVisitationsWPF
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Language = XmlLanguage.GetLanguage("en-UK");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var connection = (SqliteConnection) DBMethods.db.Database.GetDbConnection();
            connection.CreateCollation("NOCASE", (x, y) => string.Compare(x, y, ignoreCase: true));

            StudentsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true; //Fix for crashes
            StudentsVisitationsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;
            StudentsGroupsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;
            SubjectsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;
            VisitationsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;
            GroupsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;

            DBMethods.CreateStudentColumns();
            DBMethods.CreateGroupsColumns();
            DBMethods.CreateStudentsGroupsColumns();
            DBMethods.CreateSubjectColumns();
            DBMethods.CreateVisitationColumns();
            DBMethods.CreateStudentsVisitationColumns();

            DBMethods.FillAllGrids();
        }

        private void SearchVisitationButton_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow sw = new SearchWindow();
            sw.ShowDialog();
        }

        private void GenerateStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            AmmountWindow amw = new AmmountWindow("student");
            amw.ShowDialog();
        }

        private void GenerateVisitationsButton_Click(object sender, RoutedEventArgs e)
        {
            AmmountWindow amw = new AmmountWindow("visitation");
            amw.ShowDialog();
        }

        private void GenerateSubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            AmmountWindow amw = new AmmountWindow("subject");
            amw.ShowDialog();
        }

        private void GenerateGroups_Click(object sender, RoutedEventArgs e)
        {
            AmmountWindow amw = new AmmountWindow("group");
            amw.ShowDialog();
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow adw = new AddStudentWindow();
            adw.ShowDialog();
        }

        private void EditStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if(StudentsInfoGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a student to edit!");
                return;
            }

            EditStudentWindow esw = new EditStudentWindow((Student)StudentsInfoGrid.SelectedItem);
            esw.ShowDialog();
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            AddGroupWindow agw = new AddGroupWindow();
            agw.ShowDialog();
        }

        private void AddVisitationButton_Click(object sender, RoutedEventArgs e)
        {
            AddVisitation av = new AddVisitation();
            av.ShowDialog();
        }

        private void AddSubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            AddSubjectWindow asw = new AddSubjectWindow();
            asw.ShowDialog();
        }

        private void FullGroups_Click(object sender, RoutedEventArgs e)
        {
            FullGroupsMethod();   
        }

        private void ClearStudents_Click(object sender, RoutedEventArgs e)
        {
            DBMethods.ClearStudents();
        }

        private void ClearVisitations_Click(object sender, RoutedEventArgs e)
        {
            DBMethods.ClearVisitations();
        }

        private void ClearSubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            DBMethods.ClearSubjects(); 
        }

        private void ClearGroups_Click(object sender, RoutedEventArgs e)
        {
            DBMethods.ClearGroups();
        }

        private void SearchMY_Click(object sender, RoutedEventArgs e)
        {
            MonthYearWindow myw = new MonthYearWindow();
            myw.ShowDialog();
        }

        
        internal async void FullGroupsMethod()
        {
            if (await DBMethods.GetGroupsCount() == 0)
            {
                MessageBox.Show("Groups Table Is Empty!");
                return;
            }
            else if (await DBMethods.GetGroupsCount() >= 1)
            {
                var groups = await DBMethods.GetNonEmptyGroups();
                DBMethods.CreateGroupsColumns();
                GroupsInfoGrid.Items.Clear();

                foreach (var group in groups)
                {
                    GroupsInfoGrid.Items.Add(group);
                }
            }
            else
            {
                MessageBox.Show("Groups Table Doesn't Exist!");
            }
        }

        private void StudentsInfoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StudentsInfoGrid.SelectedItem != null)
            {
                var selectedStudent = (Student)StudentsInfoGrid.SelectedItem;
                DBMethods.ClearItems(StudentsVisitationsInfoGrid);

                if(selectedStudent.Visitations == null) { return; }

                foreach (var visit in selectedStudent.Visitations)
                {
                    StudentsVisitationsInfoGrid.Items.Add(visit);
                }
            }
        }

        private void StudentsGroupsInfoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StudentsGroupsInfoGrid.SelectedItem!= null)
            {
                var selectedGroup = (Group)StudentsGroupsInfoGrid.SelectedItem;
                DBMethods.ClearItems(StudentsInfoGrid);

                if (selectedGroup.Students== null) { return; }

                foreach (var student in selectedGroup.Students)
                {
                    StudentsInfoGrid.Items.Add(student);
                }
            }
        }

        private async void StudentsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DBMethods.ClearItems(StudentsInfoGrid);
            DBMethods.ClearItems(StudentsVisitationsInfoGrid);
            DBMethods.ClearItems(StudentsGroupsInfoGrid);

            if (StudentsTextBox.Text.Trim().Length==0) 
            {
                foreach(var student in DBMethods.db.Students.Include(stu => stu.Visitations))
                {
                    StudentsInfoGrid.Items.Add(student);
                }

                foreach (var group in DBMethods.db.Groups.Include(g => g.Students))
                {
                    StudentsGroupsInfoGrid.Items.Add(group);
                }

                foreach (var visitation in DBMethods.db.Visitations.Include(v => v.Student).Include(v => v.Subject))
                {
                    StudentsVisitationsInfoGrid.Items.Add(visitation);
                }

            }

            var text = StudentsTextBox.Text;
            
            var studentMatches = await DBMethods.db.Students
                    .Include(s=>s.Visitations)
                    .Include(s=>s.Group)
                    .Where(s => s.FIO
                    .Contains(text)).ToListAsync();

            

            if(studentMatches.Count>0)
            {
                foreach (var student in studentMatches)
                {
                    StudentsInfoGrid.Items.Add(student);
                }
            }
            
            var visitationsMatches = await DBMethods.db.Visitations
                .Include(s=>s.Student)
                .Include(s=>s.Subject)
                .Where(s => s.Student.FIO.Contains(text) 
                        || s.Subject.Name.Contains(text)).ToListAsync();

            if(visitationsMatches.Count > 0)
            {
                foreach(var visit in visitationsMatches)
                {
                    StudentsVisitationsInfoGrid.Items.Add(visit);
                }
            }

            var groupsMatches = await DBMethods.db.Groups
                .Include(s => s.Students)
                .Where(s => s.Name.Contains(text)).ToListAsync();

            if (groupsMatches.Count > 0)
            {
                foreach (var group in groupsMatches)
                {
                    StudentsGroupsInfoGrid.Items.Add(group);
                }
            }
        }

        private async void GroupTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DBMethods.ClearItems(GroupsInfoGrid);

            if (GroupTextBox.Text.Trim().Length == 0)
            {
                foreach (var group in DBMethods.db.Groups.Include(g => g.Students))
                {
                    GroupsInfoGrid.Items.Add(group);
                }
            }

            var text = GroupTextBox.Text;

            var groupsMatches = await DBMethods.db.Groups
                .Include(s => s.Students)
                .Where(s => s.Name.Contains(text)).ToListAsync();

            if (groupsMatches.Count > 0)
            {
                foreach (var group in groupsMatches)
                {
                    GroupsInfoGrid.Items.Add(group);
                }
            }


        }

        private async void SubjectsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DBMethods.ClearItems(SubjectsInfoGrid);
            if (SubjectsTextBox.Text.Trim().Length == 0)
            {
                foreach (var subject in DBMethods.db.Subjects)
                {
                    SubjectsInfoGrid.Items.Add(subject);
                }
            }

            var text = SubjectsTextBox.Text;

            var subjectsMatches = await DBMethods.db.Subjects
                .Where(s => s.Name.Contains(text)).ToListAsync();

            if (subjectsMatches.Count > 0)
            {
                foreach (var visit in subjectsMatches)
                {
                    SubjectsInfoGrid.Items.Add(visit);
                }
            }
        }

        private async void VisitationsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DBMethods.ClearItems(VisitationsInfoGrid);
            if (VisitationsTextBox.Text.Trim().Length == 0)
            {
                foreach (var visitation in DBMethods.db.Visitations.Include(v => v.Student).Include(v => v.Subject))
                {
                    VisitationsInfoGrid.Items.Add(visitation);
                }
            }

            var text = VisitationsTextBox.Text;

            var visitationsMatches = await DBMethods.db.Visitations
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .Where(s => s.Student.FIO.Contains(text) 
                    || s.Subject.Name.Contains(text)).ToListAsync();

            if (visitationsMatches.Count > 0)
            {
                foreach (var visit in visitationsMatches)
                {
                    VisitationsInfoGrid.Items.Add(visit);
                }
            }


        }
    }
}


    



