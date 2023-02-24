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
            st.FIO = FIOTextBox.Text.Trim();
            DateTime dt = DOBDatePicker.SelectedDate.Value;
            st.DOB = new DateOnly(dt.Year, dt.Month, dt.Day);
            st.EMAIL = EMAILTextBox.Text.Trim();
            AddStudent(st);
            MessageBox.Show("Student Added!");
            this.Close();
        }

        public async void AddStudent(Student student)
        {
            try
            {
                await using (var db = new AppDbContext())
                {
                    await db.Students.AddAsync(student);
                    await db.SaveChangesAsync();

                    //await db.Database.ExecuteSqlRawAsync($"INSERT INTO Students (FIO, DOB, EMAIL)" +
                    //      $"VALUES ('{student.FIO}', '{student.DOB}', '{student.EMAIL}');");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

    }
}
