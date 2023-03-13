using Bogus;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StudentsVisitationsWPF
{

    /// <summary>
    /// Interaction logic for AmmountWindow.xaml
    /// </summary>
    public partial class AmmountWindow : Window
    {
        internal string connectionString = "Data Source=mydatabase.db";
        internal string type = string.Empty;

        public AmmountWindow(string type)
        {
            InitializeComponent();
            this.type = type;
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int ammount = 0;
                try
                {
                     ammount = int.Parse(AmmountTextBox.Text);
                }
                catch 
                {
                    MessageBox.Show("Please enter a valid ammount!");
                    return;
                }

                if(ammount <= 0) 
                {
                    MessageBox.Show("Please enter a valid ammount!");
                    return;
                }

                switch (type)
                {
                    case "student":
                        MainWindow.dbMethods.GenerateStudents(ammount);
                        this.Close();
                        break;
                    case "visitation":
                        MainWindow.dbMethods.GenerateVisitations(ammount);
                        this.Close();
                        break;
                    case "subject":
                        MainWindow.dbMethods.GenerateSubjects(ammount);
                        this.Close();
                        break;
                    case "group":
                        MainWindow.dbMethods.GenerateGroups(ammount);
                        this.Close();
                        break;
                }
            }
        }

        

       

        

        


        
    }
}
