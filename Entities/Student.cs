using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentsVisitationsWPF.ValueObjects;

namespace StudentsVisitationsWPF.Entities
{
    [Index(nameof(FIO))]
    public class Student
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public DateOnly DOB { get; set; }
        public PassportNumber? PassportNumber { get; set; }
        public Email? Email { get; set; }
        public Group? Group { get; set; }
        public List<Visitation?> Visitations { get; set; }

        public override string ToString()
        {
            return $"FIO: {FIO}, DOB: {DOB}, EMAIL: {Email}";
        }
    }
}
