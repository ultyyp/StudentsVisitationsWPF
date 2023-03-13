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
using StudentsVisitationsWPF.Code;

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

            DataGridMethods.InitialiseGrids();
            DBMethods.Refresh("all");
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
                GroupsInfoGrid.ItemsSource = groups;
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
                if (selectedStudent.Visitations == null) { return; }
                StudentsVisitationsInfoGrid.ItemsSource = selectedStudent.Visitations;
            }
        }

        private void StudentsGroupsInfoGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StudentsGroupsInfoGrid.SelectedItem!= null)
            {
                var selectedGroup = (Group)StudentsGroupsInfoGrid.SelectedItem;
                if (selectedGroup.Students == null) { return; }
                StudentsInfoGrid.ItemsSource = selectedGroup.Students;
            }
        }

        private async void StudentsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidText = await DebounceMethods.DebounceTextBox(250, StudentsTextBox, StudentsTextBox.Text);
            if (isValidText ==  false) 
            {
                return;
            }

            SearchStudents();
        }

        private async void GroupTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidText = await DebounceMethods.DebounceTextBox(250, GroupTextBox, GroupTextBox.Text);
            if (isValidText == false)
            {
                return;
            }

            SearchGroups();
        }

        private async void SubjectsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidText = await DebounceMethods.DebounceTextBox(250, SubjectsTextBox, SubjectsTextBox.Text);
            if (isValidText == false)
            {
                return;
            }

            SearchSubjects();
        }

        private async void VisitationsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValidText = await DebounceMethods.DebounceTextBox(250, VisitationsTextBox, VisitationsTextBox.Text);
            if (isValidText == false)
            {
                return;
            }

            SearchVisitations();


        }

        public async void SearchStudents()
        {
            if (StudentsTextBox.Text.Trim().Length == 0)
            {
                StudentsInfoGrid.ItemsSource = await DBMethods.db.Students.Include(stu => stu.Visitations).ToListAsync();

                StudentsGroupsInfoGrid.ItemsSource = await DBMethods.db.Groups.Include(g => g.Students).ToListAsync();

                StudentsVisitationsInfoGrid.ItemsSource = await DBMethods.db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();

                return;
            }

            var text = StudentsTextBox.Text;


            var studentMatches = await DBMethods.db.Students
                    .Include(s => s.Visitations)
                    .Include(s => s.Group)
                    .Where(s => EF.Functions.Like(s.FIO, $"%{text}%"))
                    .ToListAsync();

            StudentsInfoGrid.ItemsSource = studentMatches;

            var visitationsMatches = await DBMethods.db.Visitations
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .Where(s => EF.Functions.Like(s.Student!.FIO, $"%{text}%")
                        || EF.Functions.Like(s.Subject!.Name, $"%{text}%")).ToListAsync();

            StudentsVisitationsInfoGrid.ItemsSource = visitationsMatches;

            var groupsMatches = await DBMethods.db.Groups
                .Include(s => s.Students)
                .Where(s => EF.Functions.Like(s.Name, $"%{text}%")
                || s.Students.Any(s => EF.Functions.Like(s.FIO, $"%{text}%"))
                ).ToListAsync();

            StudentsGroupsInfoGrid.ItemsSource = groupsMatches;
        }

        public async void SearchVisitations()
        {
            if (VisitationsTextBox.Text.Trim().Length == 0)
            {
                VisitationsInfoGrid.ItemsSource = await DBMethods.db.Visitations.Include(v => v.Student).Include(v => v.Subject).ToListAsync();
                return;
            }

            var text = VisitationsTextBox.Text;

            var visitationsMatches = await DBMethods.db.Visitations
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .Where(s => EF.Functions.Like(s.Student.FIO, $"%{text}%")
                    || EF.Functions.Like(s.Subject.Name, $"%{text}%")).ToListAsync();

            if (visitationsMatches.Count > 0)
            {
                VisitationsInfoGrid.ItemsSource = visitationsMatches;
            }
        }

        public async void SearchSubjects()
        {
            if (SubjectsTextBox.Text.Trim().Length == 0)
            {
                SubjectsInfoGrid.ItemsSource = await DBMethods.db.Subjects.ToListAsync();
                return;
            }

            var text = SubjectsTextBox.Text;

            var subjectsMatches = await DBMethods.db.Subjects
                .Where(s => EF.Functions.Like(s.Name, $"%{text}%")).ToListAsync();

            if (subjectsMatches.Count > 0)
            {
                SubjectsInfoGrid.ItemsSource = subjectsMatches;
            }
        }

        public async void SearchGroups()
        {
            if (GroupTextBox.Text.Trim().Length == 0)
            {
                GroupsInfoGrid.ItemsSource = await DBMethods.db.Groups.Include(g => g.Students).ToListAsync();
                return;
            }

            var text = GroupTextBox.Text;

            var groupsMatches = await DBMethods.db.Groups
                .Include(s => s.Students)
                .Where(s => EF.Functions.Like(s.Name, $"%{text}%")
                || s.Students.Any(s => EF.Functions.Like(s.FIO, $"%{text}%")))
                .ToListAsync();

            if (groupsMatches.Count > 0)
            {
                GroupsInfoGrid.ItemsSource = groupsMatches;
            }
        }

    }

   
}


    



