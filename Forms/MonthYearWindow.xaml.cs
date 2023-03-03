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
    /// Interaction logic for MonthYearWindow.xaml
    /// </summary>
    public partial class MonthYearWindow : Window
    {
        public MonthYearWindow()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(MonthTextBox.Text.Trim().Length <=0)
            {
                MessageBox.Show("Please enter a valid month!");
                return;
            }
            if(YearTextBox.Text.Trim().Length <=0)
            {
                MessageBox.Show("Please enter a valid year!");
                return;
            }

            try
            {
                
                DateOnly donly = new DateOnly(int.Parse(YearTextBox.Text),int.Parse(MonthTextBox.Text), 1);
                var students = await DBMethods.GetStudentMonthYear(donly.Month, donly.Year);
                

                if(students.Count() == 0) 
                { 
                    MessageBox.Show("No Students with that date visit found!"); 
                    return; 
                }

                DBMethods.ClearColumns();
                DBMethods.CreateStudentColumns();

                foreach (var stu in students)
                {
                    ((MainWindow)Application.Current.MainWindow).InfoGrid.Items.Add(stu);
                }

                MessageBox.Show("Search Done!");
                this.Close();

                
            }
            catch
            {
                MessageBox.Show("Please enter valid values!");
            }
               
        }
    }
}
