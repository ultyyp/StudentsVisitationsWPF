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
    /// Interaction logic for AddVisitation.xaml
    /// </summary>
    public partial class AddVisitation : Window
    {
        public AddVisitation()
        {
            InitializeComponent();
            var stu = DBMethods.GetStudents().Result;
            foreach (var student in stu)
            {
                ComboBox.Items.Add(student.FIO);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DBMethods.Visitation vs = new DBMethods.Visitation();
            var selectedName = ComboBox.SelectedItem.ToString();
            var students = DBMethods.GetStudents().Result;
            var selectedStudent = students.Where(x => x.FIO == selectedName).ToList();
            vs.STUDENTID = selectedStudent[0].ID;
            DateTime dt = DATEDatePicker.SelectedDate.Value;
            vs.DATE = new DateOnly(dt.Year, dt.Month, dt.Day);
            try
            {
                DBMethods.AddVisit(vs);
                MessageBox.Show("Visit Added!");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Student Doesn't Exist!");
                this.Close();
            }
            
        }

        
    }
}
