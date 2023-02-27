using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentsVisitationsWPF.DBMethods;

namespace StudentsVisitationsWPF.Entities
{
    public class Visitation
    {
        public Guid Id { get; set; }
        //public Guid StudentID { get; set; }
        public Student? Student { get; set; }
        public Subject? Subject { get; set; }
        public DateOnly Date { get; set; }
    }
}
