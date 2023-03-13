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
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Design;
using StudentsVisitationsWPF.Migrations;
using StudentsVisitationsWPF.Forms;
using StudentsVisitationsWPF.Entities;

namespace StudentsVisitationsWPF.Forms
{
    /// <summary>
    /// Interaction logic for AddGroupWindow.xaml
    /// </summary>
    public partial class AddGroupWindow : Window
    {
        public AddGroupWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(NameTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a valid name!");
                return;
            }

            Group group = new Group
            {
                Id = Guid.NewGuid(),
                Name = NameTextBox.Text,
                CreationDate = DateTime.Now
            };
            DBMethods.AddGroup(group);
            DBMethods.Refresh("all");
            MessageBox.Show("Group Added!");
            this.Close();
        }
    }
}
