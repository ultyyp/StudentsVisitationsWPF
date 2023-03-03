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
using StudentsVisitationsWPF.Forms;
using StudentsVisitationsWPF.Entities;

namespace StudentsVisitationsWPF.Forms
{
    /// <summary>
    /// Interaction logic for AddSubjectWindow.xaml
    /// </summary>
    public partial class AddSubjectWindow : Window
    {
        public AddSubjectWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Subject subject = new Subject
            {
                Id = Guid.NewGuid()
            };
            string name = SubjectNameTextBox.Text.Trim();
            if(name.Length <= 0 ) 
            {
                MessageBox.Show("Please enter a name!");
                return;
            }

            subject.Name= name;
            DBMethods.AddSubject(subject);
            ((MainWindow)Application.Current.MainWindow).SubjectsInfoGrid.Items.Add(subject);
            MessageBox.Show("Subject Added!");
            this.Close();
            
        }
    }
}
