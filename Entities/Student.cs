using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsVisitationsWPF.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FIO { get; set; }
        public DateOnly DOB { get; set; }
        public string Email { get; set; }
        public Group? Group { get; set; }
        public List<Visitation?> Visitations { get; set; }

        public override string ToString()
        {
            return $"FIO: {FIO}, DOB: {DOB}, EMAIL: {Email}";
        }
    }
}
