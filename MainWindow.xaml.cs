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
            InfoGrid.BeginningEdit += (s, ss) => ss.Cancel = true; //Fix for crashes
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

        private async void ShowStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            if(await DBMethods.GetStudentsCount() ==0)
            {
                MessageBox.Show("Students Table Is Empty!");
                return;
            }
            else if(await DBMethods.GetStudentsCount() >= 1)
            {
                var students = await DBMethods.GetStudents();
                
                DBMethods.CreateStudentColumns();
                DBMethods.ClearItems();

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
            if(await DBMethods.GetVisitationsCount() == 0)
            {
                MessageBox.Show("Visitations Table Is Empty!");
                return;
            }
            else if (await DBMethods.GetVisitationsCount() >= 1)
            {
                var visitations = await DBMethods.GetVisitations();
                DBMethods.CreateVisitationColumns();
                DBMethods.ClearItems();

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

        private async void ShowSubjectsButton_Click(object sender, RoutedEventArgs e)
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
                DBMethods.ClearItems();

                foreach (var subject in subjects)
                {
                    InfoGrid.Items.Add(subject);
                }
            }
            else
            {
                MessageBox.Show("Subjects Table Doesn't Exist!");
            }
        }

        private async void ShowGroups_Click(object sender, RoutedEventArgs e)
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
                DBMethods.ClearItems();

                foreach (var group in groups)
                {
                    InfoGrid.Items.Add(group);
                }
            }
            else
            {
                MessageBox.Show("Groups Table Doesn't Exist!");
            }
        }

        private async void FullGroups_Click(object sender, RoutedEventArgs e)
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
                DBMethods.ClearItems();

                foreach (var group in groups)
                {
                    InfoGrid.Items.Add(group);
                }
            }
            else
            {
                MessageBox.Show("Groups Table Doesn't Exist!");
            }
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
    }
}


    



