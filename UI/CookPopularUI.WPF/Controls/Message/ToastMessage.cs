/*
 * Description：ToastMessage 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2021-11-25 17:32:33
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2021 All Rights Reserved.
 */


using CookPopularToolkit.Windows;
using CookPopularUI.WPF.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace CookPopularUI.WPF.Controls
{
    /// <summary>
    /// 表示类似Android/IOS的Toast消息框
    /// </summary>
    [TemplatePart(Name = ElementBorder, Type = (typeof(UIElement)))]
    public class ToastMessage : NormalWindow
    {
        /// <summary>
        /// <see cref="ToastMessage"/>的消息图标
        /// </summary>
        public Geometry ToastMessageIcon
        {
            get => (Geometry)GetValue(ToastMessageIconProperty);
            set => SetValue(ToastMessageIconProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ToastMessageIcon"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ToastMessageIconProperty =
            DependencyProperty.Register(nameof(ToastMessageIcon), typeof(Geometry), typeof(ToastMessage), new PropertyMetadata(Geometry.Empty));


        /// <summary>
        /// <see cref="ToastMessage"/>的消息图标画刷
        /// </summary>
        public Brush ToastMessageIconBrush
        {
            get => (Brush)GetValue(ToastMessageIconBrushProperty);
            set => SetValue(ToastMessageIconBrushProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ToastMessageIconBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ToastMessageIconBrushProperty =
            DependencyProperty.Register(nameof(ToastMessageIconBrush), typeof(Brush), typeof(ToastMessage), new PropertyMetadata(default(Brush)));


        private const string ElementBorder = "RootBorder";
        private UIElement _border;

        static ToastMessage()
        {
            StyleProperty.OverrideMetadata(typeof(ToastMessage), new FrameworkPropertyMetadata(default, (s, e) => ResourceHelper.GetResource<Style>("DefaultToastMessageStyle")));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = (UIElement)GetTemplateChild(ElementBorder);
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="content">内容</param>
        public static void ShowInfo(object content)
        {
            var messageInfo = new NotifyMessageInfo()
            {
                Content = content,
                MessageIcon = ResourceHelper.GetResource<Geometry>("InfoGeometry")!,
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogInfoBrush")!,
                PopupAnimation = PopupAnimation.Scroll,
            };
            Show(messageInfo);
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="content">内容</param>
        public static void ShowWarning(object content)
        {
            var messageInfo = new NotifyMessageInfo()
            {
                Content = content,
                MessageIcon = ResourceHelper.GetResource<Geometry>("WarningGeometry")!,
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogWarningBrush")!,
                PopupAnimation = PopupAnimation.Scroll,
            };
            Show(messageInfo);
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="content">内容</param>
        public static void ShowError(object content)
        {
            var messageInfo = new NotifyMessageInfo()
            {
                Content = content,
                MessageIcon = ResourceHelper.GetResource<Geometry>("ErrorGeometry")!,
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogErrorBrush")!,
                PopupAnimation = PopupAnimation.Scroll,
            };
            Show(messageInfo);
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="content">内容</param>
        public static void ShowFatal(object content)
        {
            var messageInfo = new NotifyMessageInfo()
            {
                Content = content,
                MessageIcon = ResourceHelper.GetResource<Geometry>("FatalGeometry")!,
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogFatalBrush")!,
                PopupAnimation = PopupAnimation.Scroll,
            };
            Show(messageInfo);
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="content">内容</param>
        public static void ShowQuestion(object content)
        {
            var messageInfo = new NotifyMessageInfo()
            {
                Content = content,
                MessageIcon = ResourceHelper.GetResource<Geometry>("QuestionGeometry")!,
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogQuestionBrush")!,
                PopupAnimation = PopupAnimation.Scroll,
            };
            Show(messageInfo);
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="content">内容</param>
        public static void ShowSuccess(object content)
        {
            var messageInfo = new NotifyMessageInfo()
            {
                Content = content,
                MessageIcon = ResourceHelper.GetResource<Geometry>("SuccessGeometry")!,
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogSuccessBrush")!,
                PopupAnimation = PopupAnimation.Scroll,
            };
            Show(messageInfo);
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="messageInfo">消息内容</param>
        public static void Show(NotifyMessageInfo messageInfo)
        {
            var toastMessage = new ToastMessage()
            {
                Content = messageInfo.Content,
                Owner = WindowHelper.GetActiveWindow(),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ToastMessageIcon = messageInfo.MessageIcon,
                ToastMessageIconBrush = messageInfo.MessageIconBrush,
            };

            toastMessage.ShowAnimation(toastMessage, messageInfo.PopupAnimation);
            toastMessage.SetTimer(toastMessage, messageInfo.Duration, messageInfo.PopupAnimation);
            toastMessage.Show();
            //Application.Current.Dispatcher.InvokeAsync(() => toastMessage.ShowDialog());
        }

        private void ShowAnimation(ToastMessage toast, PopupAnimation animation)
        {
            switch (animation)
            {
                case PopupAnimation.None:
                    break;
                case PopupAnimation.Fade:
                    toast.Opacity = 0;
                    var fadeAnimation = AnimationHelper.CreateDoubleAnimation(1);
                    toast.BeginAnimation(OpacityProperty, fadeAnimation);
                    break;
                case PopupAnimation.Scroll:
                    toast.Opacity = 0;
                    var scale = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
                    toast.RenderTransform = scale;
                    toast.RenderTransformOrigin = new Point(0.5, 0.5);
                    var animation1 = AnimationHelper.CreateDoubleAnimation(0, 1, 0.1);
                    var animation2 = AnimationHelper.CreateDoubleAnimation(0, 1, 0.1);
                    var Animation3 = AnimationHelper.CreateDoubleAnimation(1, 0);
                    toast.BeginAnimation(OpacityProperty, Animation3);
                    scale.BeginAnimation(ScaleTransform.ScaleXProperty, animation1);
                    scale.BeginAnimation(ScaleTransform.ScaleYProperty, animation2);
                    break;
                case PopupAnimation.Slide:
                    var rotate = new RotateTransform() { Angle = 0 };
                    toast.RenderTransform = rotate;
                    toast.RenderTransformOrigin = new Point(0.5, 0.5);
                    var rotateAnimation = AnimationHelper.CreateDoubleAnimation(360D);
                    rotate.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
                    break;
                default:
                    break;
            }
        }

        private void SetTimer(ToastMessage toast, double duration, PopupAnimation animation)
        {
            var timer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher);
            timer.Interval = TimeSpan.FromSeconds(duration);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                timer = null;

                CloseAnimation(toast, animation);
            };
            timer.Start();
        }

        private void CloseAnimation(ToastMessage toast, PopupAnimation animation)
        {
            switch (animation)
            {
                case PopupAnimation.None:
                    toast.Close();
                    break;
                case PopupAnimation.Fade:
                    var fadeAnimation = AnimationHelper.CreateDoubleAnimation(0);
                    fadeAnimation.Completed += Animation_Completed;
                    toast.BeginAnimation(OpacityProperty, fadeAnimation);
                    break;
                case PopupAnimation.Scroll:
                    var scale = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
                    toast.RenderTransform = scale;
                    toast.RenderTransformOrigin = new Point(0.5, 0.5);
                    var animation1 = AnimationHelper.CreateDoubleAnimation(0);
                    var animation2 = AnimationHelper.CreateDoubleAnimation(0);
                    animation1.Completed += Animation_Completed;
                    animation2.Completed += Animation_Completed;
                    scale.BeginAnimation(ScaleTransform.ScaleXProperty, animation1);
                    scale.BeginAnimation(ScaleTransform.ScaleYProperty, animation2);
                    break;
                case PopupAnimation.Slide: //旋转动画
                    var rotate = new RotateTransform() { Angle = 0 };
                    toast.RenderTransform = rotate;
                    toast.RenderTransformOrigin = new Point(0.5, 0.5);
                    var rotateAnimation = AnimationHelper.CreateDoubleAnimation(-360D);
                    rotateAnimation.Completed += Animation_Completed;
                    rotate.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
                    break;
                default:
                    break;
            }
        }

        private void Animation_Completed(object? sender, EventArgs e) => Close();
    }
}
