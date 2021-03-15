using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectTracker.Model.Interfaces;

namespace ProjectTracker.Model.Models
{
    public class Group : DomainObject, ILinkedList
    {
        public string Name { get; set; }

        public ObservableCollection<Issue> Issues { get; set; }

        public int BoardID { get; set; }
        public Board Board { get; set; }
        public int NextID { get; set; }
    }
}
