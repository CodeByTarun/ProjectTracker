using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace ProjectTracker.Helpers
{
    public class DragAdorner : Adorner
    {
        public DragAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }
    }
}
