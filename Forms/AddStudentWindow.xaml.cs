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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Student st = new Student();
            st.Id = Guid.NewGuid();
            st.FIO = FIOTextBox.Text.Trim();
            if(st.FIO.Length <= 0 )
            {
                MessageBox.Show("Please enter a valid name!");
                return;
            }

            if(DOBDatePicker.SelectedDate.HasValue==false)
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
            DBMethods.AddStudent(st);
            MessageBox.Show("Student Added!");
            this.Close();
        }

        

    }
}
