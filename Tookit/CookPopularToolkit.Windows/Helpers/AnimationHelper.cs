/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AnimationHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:39:08
 */


using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;

namespace CookPopularToolkit.Windows
{
    public sealed class AnimationHelper
    {
        public static DoubleAnimation CreateDoubleAnimation(double toValue, double milliseconds = 200)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = toValue;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));
            animation.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };

            return animation;
        }

        public static DoubleAnimation CreateDoubleAnimation(double fromValue, double toValue, double seconds = 3)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = fromValue;
            animation.To = toValue;
            animation.Duration = new Duration(TimeSpan.FromSeconds(seconds));
            animation.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };

            return animation;
        }

        public static ObjectAnimationUsingKeyFrames CreateObjectAnimation(object value)
        {
            ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame() { Value = value, KeyTime = KeyTime.FromPercent(0) };
            objectAnimationUsingKeyFrames.KeyFrames.Add(discreteObjectKeyFrame);

            return objectAnimationUsingKeyFrames;
        }
    }
}
