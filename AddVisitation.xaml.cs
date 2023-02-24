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
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DBMethods.Visitation vs = new DBMethods.Visitation();
            vs.STUDENTID = int.Parse(STUDENTIDTextBox.Text);
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
