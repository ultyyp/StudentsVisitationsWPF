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
            if(DBMethods.GetStudentsCount().Result ==0)
            {
                MessageBox.Show("Students Table Is Empty!");
                return;
            }
            else if(DBMethods.GetStudentsCount().Result >= 1)
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
            if(DBMethods.GetVisitationsCount().Result ==0)
            {
                MessageBox.Show("Visitations Table Is Empty!");
                return;
            }
            else if (DBMethods.GetVisitationsCount().Result >= 1)
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

        

        

        
    }
}


    



