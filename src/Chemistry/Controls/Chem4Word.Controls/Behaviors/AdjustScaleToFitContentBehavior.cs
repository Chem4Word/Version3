// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Diagnostics;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Chem4Word.Controls.Behaviors
{
    public class AdjustScaleToFitContentBehavior : Behavior<FrameworkElement>
    {
        #region ContentWidth (DependencyProperty)

        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register(
            "ContentWidth", typeof(double), typeof(AdjustScaleToFitContentBehavior), new PropertyMetadata(default(double), ContentWidthPropertyChangedCallback));

        private static void ContentWidthPropertyChangedCallback(object source, DependencyPropertyChangedEventArgs e)
        {
            (source as AdjustScaleToFitContentBehavior)?.SetScaleRatio();
        }

        #endregion ContentWidth (DependencyProperty)

        #region ContentHeight (DependencyProperty)

        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register(
            "ContentHeight", typeof(double), typeof(AdjustScaleToFitContentBehavior), new PropertyMetadata(default(double), ContentHeightPropertyChangedCallback));

        private static void ContentHeightPropertyChangedCallback(object source, DependencyPropertyChangedEventArgs e)
        {
            (source as AdjustScaleToFitContentBehavior)?.SetScaleRatio();
        }

        #endregion ContentHeight (DependencyProperty)

        #region ScaleTransform (DependencyProperty)

        public ScaleTransform ScaleTransform
        {
            get { return (ScaleTransform)GetValue(ScaleTransformProperty); }
            set { SetValue(ScaleTransformProperty, value); }
        }

        public static readonly DependencyProperty ScaleTransformProperty = DependencyProperty.Register(
            "ScaleTransform", typeof(ScaleTransform), typeof(AdjustScaleToFitContentBehavior), new PropertyMetadata(default(ScaleTransform)));

        #endregion ScaleTransform (DependencyProperty)

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            Debug.Assert(AssociatedObject != null);
            AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
        }

        #endregion Overrides of Behavior

        #region Private Methods

        private void SetScaleRatio()
        {
            if (AssociatedObject?.DataContext == null)
            {
                //ScaleTransform.ScaleX = 1d;
                //ScaleTransform.ScaleY = 1d;
                return;
            }

            double contentAspectRatio = ContentWidth / ContentHeight;
            double controlAspectRatio = AssociatedObject.ActualWidth / AssociatedObject.ActualHeight;

            double ratio;

            if (contentAspectRatio > controlAspectRatio) //picture is flatter than box
            {
                //shrink the x-coordinate until it fits
                ratio = AssociatedObject.ActualWidth / ContentWidth;
            }
            else //picture is taller than box
            {
                ratio = AssociatedObject.ActualHeight / ContentHeight;
            }

            //ratio *= 0.85; //give it an extra bit of breathing space
            ScaleTransform.ScaleX = ratio;
            ScaleTransform.ScaleY = ratio;
        }

        #endregion Private Methods

        #region Private EventHandlers

        private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            SetScaleRatio();
        }

        #endregion Private EventHandlers
    }
}