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
            StudentsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true; //Fix for crashes
            SubjectsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;
            VisitationsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;
            GroupsInfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true;
            DBMethods.CreateStudentColumns();
            DBMethods.CreateGroupsColumns();
            DBMethods.CreateSubjectColumns();
            DBMethods.CreateVisitationColumns();
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

        private void ShowStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowStudentsMethod();
        }

        private void ShowVisitationsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowVisitationsMethod();
        }

        private void ShowSubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSubjectsMethod();
        }

        private void ShowGroups_Click(object sender, RoutedEventArgs e)
        {
            ShowGroupsMethod();
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

        internal async void ShowStudentsMethod()
        {
            if (await DBMethods.GetStudentsCount() == 0)
            {
                MessageBox.Show("Students Table Is Empty!");
                return;
            }
            else if (await DBMethods.GetStudentsCount() >= 1)
            {
                var students = await DBMethods.GetStudents();

                DBMethods.ClearItems(StudentsInfoGrid);

                foreach (var student in students)
                {
                    StudentsInfoGrid.Items.Add(student);
                }
            }
            else
            {
                MessageBox.Show("Students Table Doesn't Exist!");
            }
        }

        internal async void ShowVisitationsMethod()
        {
            if (await DBMethods.GetVisitationsCount() == 0)
            {
                MessageBox.Show("Visitations Table Is Empty!");
                return;
            }
            else if (await DBMethods.GetVisitationsCount() >= 1)
            {
                var visitations = await DBMethods.GetVisitations();
                DBMethods.CreateVisitationColumns();
                DBMethods.ClearItems(VisitationsInfoGrid);

                foreach (var visit in visitations)
                {
                    VisitationsInfoGrid.Items.Add(visit);
                }
            }
            else
            {
                MessageBox.Show("Visitations Table Doesn't Exist!");
            }
        }

        internal async void ShowSubjectsMethod()
        {
            if (await DBMethods.GetSubjectsCount() == 0)
            {
                MessageBox.Show("Subjects Table Is Empty!");
                return;
            }
            else if (await DBMethods.GetSubjectsCount() >= 1)
            {
                var subjects = await DBMethods.GetSubjects();
                DBMethods.CreateSubjectColumns();
                DBMethods.ClearItems(SubjectsInfoGrid);

                foreach (var subject in subjects)
                {
                    SubjectsInfoGrid.Items.Add(subject);
                }
            }
            else
            {
                MessageBox.Show("Subjects Table Doesn't Exist!");
            }
        }

        internal async void ShowGroupsMethod()
        {
            if (await DBMethods.GetGroupsCount() == 0)
            {
                MessageBox.Show("Groups Table Is Empty!");
                return;
            }
            else if (await DBMethods.GetGroupsCount() >= 1)
            {
                var groups = await DBMethods.GetGroups();
                DBMethods.CreateGroupsColumns();
                DBMethods.ClearItems(GroupsInfoGrid);

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
                DBMethods.ClearItems(GroupsInfoGrid);

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

        
    }
}


    



