using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudentsVisitationsWPF.Entities
{
    [Index(nameof(Name))]
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Student> Students { get; set; }
        public int? StudentCount => Students?.Count;

        public override string ToString()
        {
            return $"Name: {Name}, Students Count: {StudentCount}, Creation Date: {CreationDate}";
        }
    }
}
