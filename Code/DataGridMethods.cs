using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StudentsVisitationsWPF.Code
{
    public class DataGridMethods
    {
        public static void InitialiseGrids()
        {
            CreateStudentColumns();
            CreateStudentsGroupsColumns();
            CreateStudentsVisitationColumns();
            CreateSubjectColumns();
            CreateVisitationColumns();
            CreateGroupsColumns();

        }

        public static void CreateVisitationColumns()
        {
            var col2 = new DataGridTextColumn();
            col2.Header = "Student";
            col2.Binding = new Binding("Student");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).VisitationsInfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "Subject";
            col3.Binding = new Binding("Subject");
            col3.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).VisitationsInfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "Date";
            col4.Binding = new Binding("Date");
            col4.Binding.StringFormat = "dd/MM/yyyy";
            col4.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).VisitationsInfoGrid.Columns.Add(col4);
        }

        public static void CreateStudentsVisitationColumns()
        {

            var col2 = new DataGridTextColumn();
            col2.Header = "Student";
            col2.Binding = new Binding("Student");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsVisitationsInfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "Subject";
            col3.Binding = new Binding("Subject");
            col3.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsVisitationsInfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "Date";
            col4.Binding = new Binding("Date");
            col4.Binding.StringFormat = "dd/MM/yyyy";
            col4.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsVisitationsInfoGrid.Columns.Add(col4);
        }

        public static void CreateSubjectColumns()
        {
            var col1 = new DataGridTextColumn();
            col1.Header = "Id";
            col1.Binding = new Binding("Id");
            col1.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).SubjectsInfoGrid.Columns.Add(col1);

            var col2 = new DataGridTextColumn();
            col2.Header = "Name";
            col2.Binding = new Binding("Name");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).SubjectsInfoGrid.Columns.Add(col2);
        }

        public static void CreateStudentColumns()
        {

            var col2 = new DataGridTextColumn();
            col2.Header = "FIO";
            col2.Binding = new Binding("FIO");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsInfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "DOB";
            col3.Binding = new Binding("DOB");
            col3.Binding.StringFormat = "dd/MM/yyyy";
            col3.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsInfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "Passport Number";
            col4.Binding = new Binding("PassportNumber");
            col4.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsInfoGrid.Columns.Add(col4);

            var col5 = new DataGridTextColumn();
            col5.Header = "Email";
            col5.Binding = new Binding("Email");
            col5.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsInfoGrid.Columns.Add(col5);

            var col6 = new DataGridTextColumn();
            col6.Header = "Group";
            col6.Binding = new Binding("Group");
            col6.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsInfoGrid.Columns.Add(col6);


        }

        public static void CreateGroupsColumns()
        {
            var col2 = new DataGridTextColumn();
            col2.Header = "Name";
            col2.Binding = new Binding("Name");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).GroupsInfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "Creation Date";
            col3.Binding = new Binding("CreationDate");
            col3.Binding.StringFormat = "dd/MM/yyyy";
            col3.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).GroupsInfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "Students Count";
            col4.Binding = new Binding("StudentCount");
            col4.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).GroupsInfoGrid.Columns.Add(col4);
        }

        public static void CreateStudentsGroupsColumns()
        {
            var col2 = new DataGridTextColumn();
            col2.Header = "Name";
            col2.Binding = new Binding("Name");
            col2.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsGroupsInfoGrid.Columns.Add(col2);

            var col3 = new DataGridTextColumn();
            col3.Header = "Creation Date";
            col3.Binding = new Binding("CreationDate");
            col3.Binding.StringFormat = "dd/MM/yyyy";
            col3.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsGroupsInfoGrid.Columns.Add(col3);

            var col4 = new DataGridTextColumn();
            col4.Header = "Students Count";
            col4.Binding = new Binding("StudentCount");
            col4.IsReadOnly = false;
            ((MainWindow)Application.Current.MainWindow).StudentsGroupsInfoGrid.Columns.Add(col4);

        }
    }
}
