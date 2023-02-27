﻿using System;
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
using StudentsVisitationsWPF.Entities;

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
            FillComboBoxes();
        }

        public async void FillComboBoxes()
        {
            var stu = await DBMethods.GetStudents();
            foreach (var student in stu)
            {
                ComboBox.Items.Add(student.FIO);
            }

            var sub = await DBMethods.GetSubjects();
            foreach (var subject in sub)
            {
                SubjectComboBox.Items.Add(subject.Name);
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Visitation vs = new Visitation();
            if (ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a student!");
                return;
            }
            if (SubjectComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject!");
                return;
            }
            var selectedStudentName = ComboBox.SelectedItem.ToString();
            var students = await DBMethods.GetStudents();
            var selectedStudent = students.Where(x => x.FIO == selectedStudentName).ToList();
            vs.Student = selectedStudent[0];

            var selectedSubjectName = SubjectComboBox.SelectedItem.ToString();
            var subjects = await DBMethods.GetSubjects();
            var selectedSubject = subjects.Where(x => x.Name == selectedSubjectName).ToList();
            vs.Subject = selectedSubject[0];

            if (DATEDatePicker.SelectedDate.HasValue == false)
            {
                MessageBox.Show("Please select a date!");
                return;
            }
            DateTime dt = DATEDatePicker.SelectedDate.Value;
            vs.Date = new DateOnly(dt.Year, dt.Month, dt.Day);

            DBMethods.AddVisit(vs);
            MessageBox.Show("Visit Added!");
            this.Close();
        }

        
    }
}
