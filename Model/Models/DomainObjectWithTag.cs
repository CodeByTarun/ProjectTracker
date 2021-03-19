using ProjectTracker.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.Model.Models
{
    public class DomainObjectWithTag : DomainObject, ITag
    {
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
