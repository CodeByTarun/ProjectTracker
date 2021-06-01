﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ProjectTracker.Helpers
{
    public class DragAdorner : Adorner
    {
        private Brush vbrush;
        private Point location;
        private Point offset;

        public DragAdorner(UIElement adornedElement, Point offset) : base(adornedElement)
        {
            this.offset = offset;
            this.IsHitTestVisible = false;
            vbrush = new VisualBrush(AdornedElement);
            //vbrush.Opacity = .7;
        }

        public void UpdatePosition(Point location)
        {
            this.location = location;
            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            var p = location;
            p.Offset(-offset.X, -offset.Y);
            dc.DrawRectangle(vbrush, null, new Rect(p, this.RenderSize));
        }

    }
}
