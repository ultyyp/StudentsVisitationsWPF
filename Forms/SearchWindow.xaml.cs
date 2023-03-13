using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.dbMethods.GetVisitationsCount().Result == 0)
            {
                MessageBox.Show("Visitations Table Is Empty!");
                return;
            }
            else if (MainWindow.dbMethods.GetVisitationsCount().Result >= 1)
            {
                var visitations = await MainWindow.dbMethods.GetVisitations();
                if(DATEPICKER.SelectedDate.HasValue)
                {
                    DateTime dt = DATEPICKER.SelectedDate.Value;
                    DateOnly donly = new DateOnly(dt.Year, dt.Month, dt.Day);


                    ((MainWindow)Application.Current.MainWindow).VisitationsInfoGrid.ItemsSource =
                        visitations.ToList().Where(v=> v.Date.Year == donly.Year && v.Date.Month == donly.Month && v.Date.Day == donly.Day);

                    if(((MainWindow)Application.Current.MainWindow).VisitationsInfoGrid.Items.Count==0)
                    {
                        MessageBox.Show("No visitations for that day found!");
                    }
                    else
                    {
                        MessageBox.Show("Search Complete!");
                        this.Close();
                    }
                    
                }
                else
                {
                    MessageBox.Show("No Date Entered!");
                }
            } 
        }
    }
}
