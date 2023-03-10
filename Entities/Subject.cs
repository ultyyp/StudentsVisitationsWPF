using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentsVisitationsWPF.Entities
{
    [Index(nameof(Name))]
    public class Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}
