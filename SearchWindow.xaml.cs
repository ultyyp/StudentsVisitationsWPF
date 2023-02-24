using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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


            //if (GetVisitationsCount() == 0)
            //{
            //    MessageBox.Show("Visitations Table Is Empty!");
            //    return;
            //}
            //else if (GetVisitationsCount() >= 1)
            //{
            //    var visitations = await GetVisitations();
            //    DateTime dt = DATEPICKER.SelectedDate.Value;
            //    DateOnly ddd = new DateOnly(dt.Year, dt.Month, dt.Day);
            //    visitations = visitations.Where(x => x.DATE == ddd);
            //    CreateVisitationColumns();
            //    ClearItems();

            //    foreach (var visit in visitations)
            //    {
            //        InfoGrid.Items.Add(visit);

            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Visitations Table Doesn't Exist!");
            //}
        }

        //public async Task<List<Visitation>> GetVisitations()
        //{
        //    try
        //    {
        //        await using (var db = new AppDbContext())
        //        {
        //            List<Visitation> visitations = await db.Visitations.ToListAsync();
        //            return visitations;
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Visitations Table Doesn't Exist!");
        //    }
        //    return 

        //}

        //public Int64 GetVisitationsCount()
        //{
        //    try
        //    {
        //        string ConnectionString = "Data Source=mydatabase.db";
        //        using var connection = new SqliteConnection(ConnectionString);
        //        connection.Open();
        //        using var command = connection.CreateCommand();
        //        command.CommandText = "SELECT COUNT(*) FROM Visitations;";
        //        Int64 count = (Int64)command.ExecuteScalar();
        //        return count;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }

        //}

        //void ClearColumns()
        //{
        //    for (int i = InfoGrid.Columns.Count - 1; i >= 0; i--)
        //    {
        //        InfoGrid.Columns.Remove(InfoGrid.Columns[i]);
        //    }
        //}

        //void ClearItems()
        //{
        //    for (int i = InfoGrid.Items.Count - 1; i >= 0; i--)
        //    {
        //        InfoGrid.Items.Remove(InfoGrid.Items[i]);
        //    }
        //}
    }
}
