using ProjectTracker.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectTracker.Model.Models
{
    public class Project : DomainObject, IItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public DateTime DateCreated { get; set; }
        public int StatusInt { get; set; }


        public ICollection<Document> Documents { get; set; }
        public ICollection<Board> Boards { get; set; }
    }
}
