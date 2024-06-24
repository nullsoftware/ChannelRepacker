using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace NullSoftware.Controls
{
    /// <summary>
    /// Advanced version of default <see cref="StackPanel"/> 
    /// which allows to use <see cref="Spacing"/> property.
    /// </summary>
    public class UStackPanel : StackPanel
    {
        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                nameof(Spacing),
                typeof(double),
                typeof(UStackPanel),
                new FrameworkPropertyMetadata(0d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets value indicating spacing between child elements.
        /// </summary>
        [Bindable(true)]
        [Category("Layout")]
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        static UStackPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UStackPanel), new FrameworkPropertyMetadata(typeof(UStackPanel)));
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (Orientation == Orientation.Vertical)
            {
                double height = 0d;

                foreach (UIElement child in InternalChildren)
                {
                    child.Arrange(new Rect(new Point(0, height), new Size(arrangeSize.Width, child.DesiredSize.Height)));
                    height += child.DesiredSize.Height + Spacing;
                }
            }
            else
            {
                double width = 0d;

                foreach (UIElement child in InternalChildren)
                {
                    child.Arrange(new Rect(new Point(width, 0), new Size(child.DesiredSize.Width, arrangeSize.Height)));
                    width += child.DesiredSize.Width + Spacing;
                }
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            constraint = base.MeasureOverride(constraint);

            if (Orientation == Orientation.Horizontal)
            {
                constraint.Width += Spacing * Math.Max(0, Children.Count - 1);
            }
            else
            {
                constraint.Height += Spacing * Math.Max(0, Children.Count - 1);
            }

            return constraint;
        }

    }
}
