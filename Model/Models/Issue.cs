using ProjectTracker.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace ProjectTracker.Model.Models
{
    public class Issue : DomainObjectWithTag, IItem, ILinkedList
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DeadlineDate { get; set; }

        public int GroupID { get; set; }
        public Group Group { get; set; }
        public int NextID { get; set; }
    }
}
