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
using StudentsVisitationsWPF.Entities;
using StudentsVisitationsWPF.ValueObjects;

namespace StudentsVisitationsWPF
{
    /// <summary>
    /// Interaction logic for AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        public AddStudentWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {

            Student st = new Student();
            st.Id = Guid.NewGuid();
            st.FIO = FIOTextBox.Text.Trim();
            if(st.FIO.Length <= 0 )
            {
                MessageBox.Show("Please enter a valid name!");
                return;
            }

            var students = await MainWindow.dbMethods.GetStudents();
            foreach( var student in students )
            {
                if(student.FIO.Trim() == st.FIO.Trim() )
                {
                    MessageBox.Show("Student with that name already exists!");
                    return;
                }
            }

            if(DOBDatePicker.SelectedDate.HasValue==false)
            {
                MessageBox.Show("Please select a date!");
                return;
            }
            DateTime dt = DOBDatePicker.SelectedDate.Value;
            st.DOB = new DateOnly(dt.Year, dt.Month, dt.Day);

            try
            {
                st.PassportNumber = new PassportNumber(PassportNumberTextbox.Text.Trim());
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
            

            if(GroupComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a group!");
                return;
            }

            var selectedGroupName = GroupComboBox.SelectedItem.ToString();
            var groups = await MainWindow.dbMethods.GetGroups();
            var selectedGroups = groups.Where(x => x.Name == selectedGroupName).ToList();
            st.Group = selectedGroups[0];


            MainWindow.dbMethods.AddStudent(st);
            MainWindow.dbMethods.Refresh("all");

            MessageBox.Show("Student Added!");
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
