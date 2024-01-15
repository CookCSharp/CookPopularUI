/*
 *Description: AnimatedHeaderedContentControl
 *Author: Chance.zheng
 *Creat Time: 2024/1/5 17:06:14
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2024 All Rights Reserved.
 */


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CookPopularUI.WPF.Draggable
{
    /// <summary>
    /// Animated headered content control base class.
    /// </summary>
    public class AnimatedHeaderedContentControl : HeaderedContentControl
    {
        #region Private memebers

        /// <summary>
        /// Stores the width key frame.
        /// </summary>
        private SplineDoubleKeyFrame sizeAnimationWidthKeyFrame;

        /// <summary>
        /// Stores the height key frame.
        /// </summary>
        private SplineDoubleKeyFrame sizeAnimationHeightKeyFrame;

        /// <summary>
        /// Stores the posisition X key frame.
        /// </summary>
        private SplineDoubleKeyFrame positionAnimationXKeyFrame;

        /// <summary>
        /// Stores the position Y keyframe.
        /// </summary>
        private SplineDoubleKeyFrame positionAnimationYKeyFrame;

        /// <summary>
        /// Stores the size animation.
        /// </summary>
        private Storyboard sizeAnimation;

        /// <summary>
        /// Stores the position animation.
        /// </summary>
        private Storyboard positionAnimation;

        /// <summary>
        /// Stores a flag indicating if the size is animating.
        /// </summary>
        private bool sizeAnimating;

        /// <summary>
        /// Stores a flag storing if the position is animating.
        /// </summary>
        private bool positionAnimating;

        /// <summary>
        /// Stores the size animation timespan.
        /// </summary>
        private TimeSpan sizeAnimationTimespan = new TimeSpan(0, 0, 0, 0, 500);

        /// <summary>
        /// Stores the position animation time span.
        /// </summary>
        private TimeSpan positionAnimationTimespan = new TimeSpan(0, 0, 0, 0, 500);

        #endregion

        /// <summary>
        /// Gets or sets the size animation duration.
        /// </summary>
        [System.ComponentModel.Category("Animation Properties"), System.ComponentModel.Description("The size animation duration.")]
        public TimeSpan SizeAnimationDuration
        {
            get
            {
                return sizeAnimationTimespan;
            }

            set
            {
                sizeAnimationTimespan = value;
                if (sizeAnimationWidthKeyFrame != null)
                {
                    sizeAnimationWidthKeyFrame.KeyTime = KeyTime.FromTimeSpan(sizeAnimationTimespan);
                }

                if (sizeAnimationHeightKeyFrame != null)
                {
                    sizeAnimationHeightKeyFrame.KeyTime = KeyTime.FromTimeSpan(sizeAnimationTimespan);
                }
            }
        }

        /// <summary>
        /// Gets or sets the position animation duration.
        /// </summary>
        [System.ComponentModel.Category("Animation Properties"), System.ComponentModel.Description("The position animation duration.")]
        public TimeSpan PositionAnimationDuration
        {
            get
            {
                return positionAnimationTimespan;
            }

            set
            {
                positionAnimationTimespan = value;
                if (positionAnimationXKeyFrame != null)
                {
                    positionAnimationXKeyFrame.KeyTime = KeyTime.FromTimeSpan(positionAnimationTimespan);
                }

                if (positionAnimationYKeyFrame != null)
                {
                    positionAnimationYKeyFrame.KeyTime = KeyTime.FromTimeSpan(positionAnimationTimespan);
                }
            }
        }


        public AnimatedHeaderedContentControl()
        {
            sizeAnimation = new Storyboard();
            DoubleAnimationUsingKeyFrames widthAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("(FrameworkElement.Width)"));
            sizeAnimationWidthKeyFrame = new SplineDoubleKeyFrame();
            sizeAnimationWidthKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            sizeAnimationWidthKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            sizeAnimationWidthKeyFrame.Value = 0;
            widthAnimation.KeyFrames.Add(sizeAnimationWidthKeyFrame);

            DoubleAnimationUsingKeyFrames heightAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(heightAnimation, this);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("(FrameworkElement.Height)"));
            sizeAnimationHeightKeyFrame = new SplineDoubleKeyFrame();
            sizeAnimationHeightKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            sizeAnimationHeightKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            sizeAnimationHeightKeyFrame.Value = 0;
            heightAnimation.KeyFrames.Add(sizeAnimationHeightKeyFrame);

            sizeAnimation.Children.Add(widthAnimation);
            sizeAnimation.Children.Add(heightAnimation);
            sizeAnimation.Completed += new EventHandler(SizeAnimation_Completed);

            positionAnimation = new Storyboard();

            DoubleAnimationUsingKeyFrames positionXAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(positionXAnimation, this);
            Storyboard.SetTargetProperty(positionXAnimation, new PropertyPath("(Canvas.Left)"));
            positionAnimationXKeyFrame = new SplineDoubleKeyFrame();
            positionAnimationXKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            positionAnimationXKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            positionAnimationXKeyFrame.Value = 0;
            positionXAnimation.KeyFrames.Add(positionAnimationXKeyFrame);

            DoubleAnimationUsingKeyFrames positionYAnimation = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(positionYAnimation, this);
            Storyboard.SetTargetProperty(positionYAnimation, new PropertyPath("(Canvas.Top)"));
            positionAnimationYKeyFrame = new SplineDoubleKeyFrame();
            positionAnimationYKeyFrame.KeySpline = new KeySpline()
            {
                ControlPoint1 = new Point(0.528, 0),
                ControlPoint2 = new Point(0.142, 0.847)
            };
            positionAnimationYKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));
            positionAnimationYKeyFrame.Value = 0;
            positionYAnimation.KeyFrames.Add(positionAnimationYKeyFrame);

            positionXAnimation.FillBehavior = FillBehavior.Stop;
            positionYAnimation.FillBehavior = FillBehavior.Stop;
            widthAnimation.FillBehavior = FillBehavior.Stop;
            heightAnimation.FillBehavior = FillBehavior.Stop;

            positionAnimation.Children.Add(positionXAnimation);
            positionAnimation.Children.Add(positionYAnimation);

            positionAnimation.Completed += new EventHandler(PositionAnimation_Completed);
        }

        /// <summary>
        /// Animates the size of the control
        /// </summary>
        /// <param name="width">The target width</param>
        /// <param name="height">The target height</param>
        public void AnimateSize(double width, double height)
        {
            if (sizeAnimating)
            {
                sizeAnimation.Pause();
            }

            // Ensure we are in the tree
            if (VisualTreeHelper.GetParent(this) != null)
            {
                Width = ActualWidth;
                Height = ActualHeight;
                sizeAnimating = true;
                sizeAnimationWidthKeyFrame.Value = width;
                sizeAnimationHeightKeyFrame.Value = height;
                sizeAnimation.Begin();
            }
        }

        /// <summary>
        /// Animates the Canvas.Left and Canvas.Top of the control
        /// </summary>
        /// <param name="x">New X position</param>
        /// <param name="y">New Y position</param>
        public void AnimatePosition(double x, double y)
        {
            if (positionAnimating)
            {
                positionAnimation.Pause();
            }

            // Ensure we are in the tree
            if (VisualTreeHelper.GetParent(this) != null)
            {
                positionAnimating = true;
                positionAnimationXKeyFrame.Value = x;
                positionAnimationYKeyFrame.Value = y;
                positionAnimation.Begin();
            }
        }

        /// <summary>
        /// Stores the position
        /// </summary>
        /// <param name="sender">The position animation.</param>
        /// <param name="e">Event args.</param>
        private void PositionAnimation_Completed(object sender, EventArgs e)
        {
            Canvas.SetLeft(this, positionAnimationXKeyFrame.Value);
            Canvas.SetTop(this, positionAnimationYKeyFrame.Value);
        }

        /// <summary>
        /// Stores the values once the animation has completed.
        /// </summary>
        /// <param name="sender">The animated content control.</param>
        /// <param name="e">The event args.</param>
        private void SizeAnimation_Completed(object sender, EventArgs e)
        {
            Width = sizeAnimationWidthKeyFrame.Value;
            Height = sizeAnimationHeightKeyFrame.Value;
        }
    }
}
