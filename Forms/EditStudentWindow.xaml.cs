using StudentsVisitationsWPF.Entities;
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
using System.Windows.Shapes;
using StudentsVisitationsWPF.ValueObjects;

namespace StudentsVisitationsWPF.Forms
{
    /// <summary>
    /// Interaction logic for EditStudentWindow.xaml
    /// </summary>
    public partial class EditStudentWindow : Window
    {
        internal Student? selectedStudent;

        public EditStudentWindow(Student student)
        {
            InitializeComponent();
            selectedStudent = student;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox();
            FIOTextBox.Text = selectedStudent.FIO.Trim();
            PassportNumbox.Text = selectedStudent.PassportNumber!.ToString().Trim();
            EMAILTextBox.Text = selectedStudent.Email!.ToString().Trim();
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Student st = new Student();
            st.Id = selectedStudent.Id;

            st.FIO = FIOTextBox.Text.Trim();
            if (st.FIO.Length <= 0)
            {
                MessageBox.Show("Please enter a valid name!");
                return;
            }

            var students = await MainWindow.dbMethods.GetStudents();
            foreach (var student in students)
            {
                if (student.FIO.Trim() == st.FIO.Trim() && student.Id != st.Id)
                {
                    MessageBox.Show("Student with that name already exists!");
                    return;
                }
            }

            if (DOBDatePicker.SelectedDate.HasValue == false)
            {
                MessageBox.Show("Please select a date!");
                return;
            }
            DateTime dt = DOBDatePicker.SelectedDate.Value;
            st.DOB = new DateOnly(dt.Year, dt.Month, dt.Day);



            try
            {
                st.PassportNumber = new PassportNumber(PassportNumbox.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Please enter a valid passport number!");
                return;
            }

            try
            {
                st.Email = new Email(EMAILTextBox.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Please enter a valid email!");
                return;
            }

            if (GroupComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a group!");
                return;
            }

            var selectedGroupName = GroupComboBox.SelectedItem.ToString();
            var groups = await MainWindow.dbMethods.GetGroups();
            var selectedGroups = groups.Where(x => x.Name == selectedGroupName).ToList();
            st.Group = selectedGroups[0];


            MainWindow.dbMethods.EditStudent(selectedStudent.Id ,st);
            MainWindow.dbMethods.Refresh("all");
         
            MessageBox.Show("Student Edited!");
            this.Close();
        }

        public async void FillComboBox()
        {
            var groups = await MainWindow.dbMethods.GetGroups();
            foreach (var group in groups)
            {
                GroupComboBox.Items.Add(group.Name);
            }
        }

        
    }
}
