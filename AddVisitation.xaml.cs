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
            Visitation vs = new Visitation();
            vs.STUDENTID = int.Parse(STUDENTIDTextBox.Text);
            DateTime dt = DATEDatePicker.SelectedDate.Value;
            vs.DATE = new DateOnly(dt.Year, dt.Month, dt.Day);
            try
            {
                AddVisit(vs);
                MessageBox.Show("Visit Added!");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Student Doesn't Exist!");
                this.Close();
            }
            
        }

        public async void AddVisit(Visitation visit)
        {
            await using (var db = new AppDbContext())
            {
                await db.Visitations.AddAsync(visit);
                await db.SaveChangesAsync();

                //await db.Database.ExecuteSqlRawAsync($"INSERT INTO Visitations (STUDENTID, DATE)" +
                //        $"VALUES ({visit.STUDENTID}, '{visit.DATE}');");
            }
        }
    }
}
