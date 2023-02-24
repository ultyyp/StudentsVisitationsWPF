﻿using Microsoft.Data.Sqlite;
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
            if (DBMethods.GetVisitationsCount().Result == 0)
            {
                MessageBox.Show("Visitations Table Is Empty!");
                return;
            }
            else if (DBMethods.GetVisitationsCount().Result >= 1)
            {
                var visitations = await DBMethods.GetVisitations();
                if(DATEPICKER.SelectedDate.HasValue)
                {
                    DateTime dt = DATEPICKER.SelectedDate.Value;
                    DateOnly donly = new DateOnly(dt.Year, dt.Month, dt.Day);

                    DBMethods.CreateVisitationColumns();
                    DBMethods.ClearItems();

                    for (int i = visitations.ToList().Count - 1; i >= 0; i--)
                    {
                        //MessageBox.Show(donly.Year.ToString() + ":" + visitations[i].DATE.Year.ToString() + " " + donly.Month.ToString() + ":" + visitations[i].DATE.Month.ToString() + " " + donly.Day.ToString() + ":" + visitations[i].DATE.Day.ToString());
                        if (visitations[i].DATE.Year == donly.Year && visitations[i].DATE.Month == donly.Month && visitations[i].DATE.Day == donly.Day)
                        {
                            ((MainWindow)Application.Current.MainWindow).InfoGrid.Items.Add(visitations[i]);
                        }
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
