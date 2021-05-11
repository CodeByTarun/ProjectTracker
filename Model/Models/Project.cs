using ProjectTracker.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectTracker.Model.Models
{
    public class Project : DomainObjectWithTag, IItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public string Status { get; set; }
        public ICollection<Board> Boards { get; set; }
    }
}
