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

            var students = await DBMethods.GetStudents();
            foreach (var student in students)
            {
                if (student.FIO.Trim() == st.FIO.Trim())
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


            st.Email = EMAILTextBox.Text.Trim();
            if (st.Email.Length <= 0)
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
            var groups = await DBMethods.GetGroups();
            var selectedGroups = groups.Where(x => x.Name == selectedGroupName).ToList();
            st.Group = selectedGroups[0];


            DBMethods.EditStudent(selectedStudent.Id ,st);
            DBMethods.Refresh("all");
         
            MessageBox.Show("Student Edited!");
            this.Close();
        }

        public async void FillComboBox()
        {
            var groups = await DBMethods.GetGroups();
            foreach (var group in groups)
            {
                GroupComboBox.Items.Add(group.Name);
            }
        }

        
    }
}
